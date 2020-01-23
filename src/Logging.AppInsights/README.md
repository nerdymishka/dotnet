## NerdyMishka.Extensions.Logging.AppInsights

Provides a logger builder for adding the [IRawTelemetryClient][IRawTelemetryClient]
that is implemented from TelemetryClient and enables app insights as a logger.

```csharp
// lb = Log Builder
servers.AddLogging((lb) => {
    lb.AddAppInsights("{instrumentKey}")
});

servers.AddLogging((lb) => {
    lb.AddAppInsights(() =>
        // customize initialization
        new TelemetryClient("key"),

        // customize options
        (options) => {
            options.UseCustomEvents = true
            // extra meta data for enriching the logging for each request.
            options.IncludeScopes = new Dictionary<string, string>() {

            };
        });
});
```

## License

Copyright 2020 Nerdy Mishka, Michael Herndon

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.


[IRawTelemetryClient]: ../../AppInsights.abstractions/src/IRawTelemetryClient.cs
[ApplicationInsights-dotnet]: https://github.com/microsoft/ApplicationInsights-dotnet/tree/develop/LOGGING