## NerdyMishka.Extensions.Logging.AppInsights.Abstractions

Provides the core abstractions to wire up [IRawTelemetryClient][IRawTelemetryClient]
as a Microsoft.Extensions.Logging.LoggingProvider.  The libray includes logging
builder extensions to register an `IRawTelemetryClient` implementation.

[NerdMishka.Extensions.Logging.AppInsights] takes this abstraction and provides
the implementation of `IRawTelemetryClient` that wraps the TelemetryClient from


```csharp
// lb = Log Builder.
// sp = Service Provider.

services.AddLogging(lb => {
    lb.AddTelemetryClient(
        (sp) => sp.GetRequiredService<IRawTelemetryClient>()
    );
});

services.AddLogging(lb => {
    lb.AddTelemetryClient(
        (sp) => {
            var tc = new TelemetryClient("instrumentKey");
            return new AppInsightsTelemetryClient(tc);
        }
    );
});
```

## License

Source from the [ApplicationInsights-dotnet][ApplicationInsights-dotnet] project
is under the MIT license and source that was added is under the Apache 2.0 license.

------------------

Copyright 2016-2020 Nerdy Mishka, Michael Herndon

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

-----------

Copyright (c) 2015 Microsoft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

[IRawTelemetryClient]: ../../AppInsights.abstractions/src/IRawTelemetryClient.cs
[ApplicationInsights-dotnet]: https://github.com/microsoft/ApplicationInsights-dotnet/tree/develop/LOGGING