Param(
    [Switch] $Force
)

if([Environment]::OSVersion.Platform -ne "Win32NT") {
    # TODO: add a restore.debian.ps1
    throw "restore.ps1 only works on windows"
}

function Update-Environment() {
    Set-Alias "psake" "invoke-psake" -Scope Global 
    Set-Alias "ptest" "invoke-pester" -Scope Global 
}

if((Test-Path "./.setup") -and !$Force.ToBool()) { 
    Update-Environment
    exit 
}

Install-Module Gz-ChocolateySync -AllowClobber -Force -SkipPublisherCheck
Install-Module Psake -SkipPublisherCheck -AllowClobber -Force
Install-Module Pester -SkipPublisherCheck -Force -AcceptLicense -AllowClobber

if($null -eq (Get-Command choco -EA SilentlyContinue))
{
    if(!$Force.ToBool())
    {
        Write-Warning "Chocolatey is required and not installed on path"
        Write-Warning "If you continue, chocolatey will be installed to c:/apps"
        $stop = Read-Host  "install chocolatey now? Press 'y' to continue"
        if($stop -ne "y")
        {
            exit 
        }
    }   
}

Sync-Chocolatey -Uri "$PsSCriptRoot/build/chocolatey.json"

if($null -eq (Get-Command dotnet -EA SilentlyContinue)) {
    $env:Path += ";$Env:ProgramFiles\dotnet"
}

# not available via chocolatey
if($null -eq (Get-Command nbgv -EA SilentlyContinue)) {
    dotnet tool install -g nbgv 
}

$gitRemotes = git remote 

$remotes = @{
    "gitlab" = "https://gitlab.com/nerdymishka/dotnet"
    "github" = "https://github.com/nerdymishka/dotnet"
    "vsts" = "https://dev.azure.com/nerdymishka/dotnet/_git/dotnet"
}

foreach($key in $remotes.Keys) {
    
    if($gitRemotes.Contains($key)) {
        continue;
    }
    
    if(!$gitRemotes.Contains($key)) {
        git remote add $key "$($remote[$key])"
    }

    if($key -eq "origin" -or $key -eq "gitlab") {
        continue 
    }

    git remote set-url origin --push -add "$($remote[$key])"
}

Update-Environment

"" > .setup