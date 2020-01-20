
# NerdyMiska Project Structure

## Root Folders

- **docs** - contains conceptual documentation for the NerdyMishka .NET project.
- **src**  - contains source code including tests.
- **.vscode** - contains Visual Studio Code's workspace ("project") specific
  configuration files.
- **artifacts** - build artifacts for local builds. This directory is ignored
  and must not be added to the code repository.

## Artifacts Folder

- **tests** - contains test result artifacts.
- **packages** - contains nuget packages to publish.

## Files

- **[.gitignore](../.gitignore)** is the configuration file that
  instructs git on the files to hide from staging and commiting history.

- **[.markdownlint.json](../.markdownlint.json)** is the configuration for
  the markdown linter, which has a [vscode plugin][markdown_lint].

- **[nerdymishka.ruleset](../nerdymishka.ruleset)** is the default
  [rules][ruleset] for this project's .NET based [Rosalyn analyzers][analyzers].

- **[Directory.Build.props](../Directory.Build.props)** is the root [msbuild
  properties][msbuild_ext] values that apply to all .net project files within
  this project.

- **[Directory.Build.targets](../Directory.Build.targets)** is the root
  build targets that apply to all .net project files within this project.

- **[omnipshaprp.json](../omnisharp.json)** is project specific [configuration
  file][omnisharp_configuration] for omnisharp.

- **[.vscode/settings.json](../.vscode/settings.json)** is the preferences file
  for VS Code that applies to this workspace/project.



[analyzers]: https://docs.microsoft.com/en-us/dotnet/standard/analyzers/
[ruleset]: https://docs.microsoft.com/en-us/visualstudio/code-quality/using-rule-sets-to-group-code-analysis-rules?view=vs-2019
[msbuild_ext]: https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2019
[markdown_lint]: https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint
[omnisharp_configuration]: https://github.com/OmniSharp/omnisharp-roslyn/wiki/Configuration-Options
[azdo_variables]: https://docs.microsoft.com/en-us/azure/devops/pipelines/build/variables?view=azure-devops&tabs=yaml
