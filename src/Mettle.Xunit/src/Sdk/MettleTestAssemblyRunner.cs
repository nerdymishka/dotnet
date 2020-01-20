using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mettle.Xunit.Sdk
{
    /// <summary>
    /// The test assembly runner for xUnit.net v2 tests.
    /// </summary>
    public class MettleTestAssemblyRunner : TestAssemblyRunner<IXunitTestCase>
    {
        private IAttributeInfo collectionBehaviorAttribute;
        private bool disableParallelization;
        private bool initialized;
        private int maxParallelThreads;
        private SynchronizationContext originalSyncContext;
        private MaxConcurrencySyncContext syncContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MettleTestAssemblyRunner"/> class.
        /// </summary>
        /// <param name="testAssembly">The assembly that contains the tests to be run.</param>
        /// <param name="testCases">The test cases to be run.</param>
        /// <param name="diagnosticMessageSink">The message sink to report diagnostic messages to.</param>
        /// <param name="executionMessageSink">The message sink to report run status to.</param>
        /// <param name="executionOptions">The user's requested execution options.</param>
        public MettleTestAssemblyRunner(ITestAssembly testAssembly,
                                       IEnumerable<IXunitTestCase> testCases,
                                       IMessageSink diagnosticMessageSink,
                                       IMessageSink executionMessageSink,
                                       ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        {
        }

        public IServiceProvider ServiceProvider { get; set; }

        /// <inheritdoc/>
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public override void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            if (this.syncContext is IDisposable disposable)
                disposable.Dispose();
        }

        /// <inheritdoc/>
        protected override string GetTestFrameworkDisplayName()
            => XunitTestFrameworkDiscoverer.DisplayName;

        /// <inheritdoc/>
        protected override string GetTestFrameworkEnvironment()
        {
            this.Initialize();

            var testCollectionFactory = ExtensibilityPointFactory.GetXunitTestCollectionFactory(DiagnosticMessageSink, collectionBehaviorAttribute, TestAssembly);
            var threadCountText = maxParallelThreads < 0 ? "unlimited" : maxParallelThreads.ToString();

            return $"{base.GetTestFrameworkEnvironment()} [{testCollectionFactory.DisplayName}, {(disableParallelization ? "non-parallel" : $"parallel ({threadCountText} threads)")}]";
        }

        /// <summary>
        /// Gets the synchronization context used when potentially running tests in parallel.
        /// If <paramref name="maxParallelThreads"/> is greater than 0, it creates
        /// and uses an instance of <see cref="Xunit.Sdk.MaxConcurrencySyncContext"/>.
        /// </summary>
        /// <param name="maxParallelThreads">The maximum number of parallel threads.</param>
        protected virtual void SetupSyncContext(int maxParallelThreads)
        {
            if (MaxConcurrencySyncContext.IsSupported && maxParallelThreads > 0)
            {
                syncContext = new MaxConcurrencySyncContext(maxParallelThreads);
                SetSynchronizationContext(syncContext);
            }
        }

        /// <summary>
        /// Ensures the assembly runner is initialized (sets up the collection behavior,
        /// parallelization options, and test orderers from their assembly attributes).
        /// </summary>
        protected void Initialize()
        {
            if (initialized)
                return;

            var serviceProviderFactoryAttribute = this.TestAssembly.Assembly
                .GetCustomAttributes(typeof(ServiceProviderFactoryAttribute))
                .SingleOrDefault();

            if (serviceProviderFactoryAttribute != null)
            {
                var factoryType = (Type)serviceProviderFactoryAttribute.GetConstructorArguments().First();

                // var factoryType = serviceProviderFactoryAttribute.GetNamedArgument<Type>("FactoryType");

                // TODO: consider diagnostic message if the type exists but not the interface.
                if (factoryType != null && factoryType.GetInterface("IServiceProviderFactory") != null)
                {
                    var serviceFactory = (IServiceProviderFactory)Activator.CreateInstance(factoryType);
                    this.ServiceProvider = serviceFactory.CreateProvider();
                }
            }

            this.collectionBehaviorAttribute = this.TestAssembly.Assembly.GetCustomAttributes(typeof(CollectionBehaviorAttribute)).SingleOrDefault();
            if (this.collectionBehaviorAttribute != null)
            {
                this.disableParallelization = this.collectionBehaviorAttribute.GetNamedArgument<bool>("DisableTestParallelization");
                this.maxParallelThreads = this.collectionBehaviorAttribute.GetNamedArgument<int>("MaxParallelThreads");
            }

            this.disableParallelization = this.ExecutionOptions.DisableParallelization() ?? this.disableParallelization;
            this.maxParallelThreads = this.ExecutionOptions.MaxParallelThreads() ?? this.maxParallelThreads;
            if (this.maxParallelThreads == 0)
                this.maxParallelThreads = Environment.ProcessorCount;

            var testCaseOrdererAttribute = this.TestAssembly.Assembly.GetCustomAttributes(typeof(TestCaseOrdererAttribute)).SingleOrDefault();
            if (testCaseOrdererAttribute != null)
            {
                try
                {
                    var testCaseOrderer = ExtensibilityPointFactory.GetTestCaseOrderer(this.DiagnosticMessageSink, testCaseOrdererAttribute);
                    if (testCaseOrderer != null)
                    {
                        this.TestCaseOrderer = testCaseOrderer;
                    }
                    else
                    {
                        var args = testCaseOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                        this.DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Could not find type '{args[0]}' in {args[1]} for assembly-level test case orderer"));
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    var innerEx = ex.Unwrap();
                    var args = testCaseOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    this.DiagnosticMessageSink.OnMessage(
                        new DiagnosticMessage(
                            $"Assembly-level test case orderer '{args[0]}' threw '{innerEx.GetType().FullName}' during construction: {innerEx.Message}{Environment.NewLine}{innerEx.StackTrace}"));
                }
            }

            var testCollectionOrdererAttribute = this.TestAssembly.Assembly.GetCustomAttributes(typeof(TestCollectionOrdererAttribute)).SingleOrDefault();
            if (testCollectionOrdererAttribute != null)
            {
                try
                {
                    var testCollectionOrderer = ExtensibilityPointFactory.GetTestCollectionOrderer(DiagnosticMessageSink, testCollectionOrdererAttribute);
                    if (testCollectionOrderer != null)
                    {
                        this.TestCollectionOrderer = testCollectionOrderer;
                    }
                    else
                    {
                        var args = testCollectionOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                        this.DiagnosticMessageSink.OnMessage(
                            new DiagnosticMessage($"Could not find type '{args[0]}' in {args[1]} for assembly-level test collection orderer"));
                    }
                }
                catch (Exception ex)
                {
                    var innerEx = ex.Unwrap();
                    var args = testCollectionOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    this.DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Assembly-level test collection orderer '{args[0]}' threw '{innerEx.GetType().FullName}' during construction: {innerEx.Message}{Environment.NewLine}{innerEx.StackTrace}"));
                }
            }

            this.initialized = true;
        }

        /// <inheritdoc/>
        protected override Task AfterTestAssemblyStartingAsync()
        {
            this.Initialize();
            return CommonTasks.Completed;
        }

        /// <inheritdoc/>
        protected override Task BeforeTestAssemblyFinishedAsync()
        {
            SetSynchronizationContext(this.originalSyncContext);
            return CommonTasks.Completed;
        }

        /// <inheritdoc/>
        protected override async Task<RunSummary> RunTestCollectionsAsync(IMessageBus messageBus, CancellationTokenSource cancellationTokenSource)
        {
            Check.NotNull(nameof(cancellationTokenSource), cancellationTokenSource);

            this.originalSyncContext = SynchronizationContext.Current;

            if (this.disableParallelization)
            {
                return await base.RunTestCollectionsAsync(messageBus, cancellationTokenSource)
                    .ConfigureAwait(false);
            }

            this.SetupSyncContext(this.maxParallelThreads);

            Func<Func<Task<RunSummary>>, Task<RunSummary>> taskRunner;
            if (SynchronizationContext.Current != null)
            {
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                taskRunner = code => Task.Factory.StartNew(code, cancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler, scheduler).Unwrap();
            }
            else
            {
                taskRunner = code => Task.Run(code, cancellationTokenSource.Token);
            }

            List<Task<RunSummary>> parallel = null;
            List<Func<Task<RunSummary>>> nonParallel = null;
            var summaries = new List<RunSummary>();

            foreach (var collection in this.OrderTestCollections())
            {
                Task<RunSummary> RunTask() => this.RunTestCollectionAsync(messageBus, collection.Item1, collection.Item2, cancellationTokenSource);

                // attr is null here from our new unit test, but I'm not sure if that's expected or there's a cheaper approach here
                // Current approach is trying to avoid any changes to the abstractions at all
                var attr = collection.Item1.CollectionDefinition?.GetCustomAttributes(typeof(CollectionDefinitionAttribute)).SingleOrDefault();
                if (attr?.GetNamedArgument<bool>(nameof(CollectionDefinitionAttribute.DisableParallelization)) == true)
                {
                    (nonParallel ?? (nonParallel = new List<Func<Task<RunSummary>>>())).Add(RunTask);
                }
                else
                {
                    (parallel ?? (parallel = new List<Task<RunSummary>>())).Add(taskRunner(RunTask));
                }
            }

            if (parallel?.Count > 0)
            {
                foreach (var task in parallel)
                {
                    try
                    {
                        summaries.Add(await task.ConfigureAwait(false));
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }

            if (nonParallel?.Count > 0)
            {
                foreach (var task in nonParallel)
                {
                    try
                    {
                        summaries.Add(await taskRunner(task).ConfigureAwait(false));
                        if (cancellationTokenSource.IsCancellationRequested)
                            break;
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }

            return new RunSummary()
            {
                Total = summaries.Sum(s => s.Total),
                Failed = summaries.Sum(s => s.Failed),
                Skipped = summaries.Sum(s => s.Skipped),
            };
        }

        /// <inheritdoc/>
        protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus, ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, CancellationTokenSource cancellationTokenSource)
            => new MettleTestCollectionRunner(
                    testCollection,
                    testCases,
                    this.DiagnosticMessageSink,
                    messageBus,
                    this.TestCaseOrderer,
                    new ExceptionAggregator(this.Aggregator),
                    cancellationTokenSource,
                    this.ServiceProvider)
                .RunAsync();

        [SecuritySafeCritical]
        private static void SetSynchronizationContext(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
        }
    }
}
