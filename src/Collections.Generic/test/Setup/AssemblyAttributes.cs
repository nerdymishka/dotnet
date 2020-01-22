[assembly: Mettle.MettleXunitFramework]
[assembly: Mettle.ServiceProviderFactory(typeof(Tests.ServiceProviderFactory))]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1062:Validate arguments of public methods",
    Justification = "ByDesign")]