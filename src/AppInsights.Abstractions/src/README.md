## NerdyMishka.Extensions.AppInsights.Abstractions

Interfaces and data contracts for abstracting the TelemetryClient found in
the dotnet implementation of Application Insights.

The abstraction enables Inversion of Control and the ability to replace the
client with another implementation should Azure App Insights be discontinued,
for testing purposes and creating logging providers.

It important to use [IRawTelemetryClient](IRawTelemetryClient.cs) class to create
logging providers as it provides TrackTrace and TrackEvent methods that pass in
a data contract for those methods which can be used to format messages.

## License

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
