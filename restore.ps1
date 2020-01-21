Param(
    [Switch] $Force
)

if ([Environment]::OSVersion.Platform -ne "Win32NT") {
    # TODO: add a restore.debian.ps1
    throw "restore.ps1 only works on windows"
}

function Update-Environment() {
    Set-Alias "psake" "invoke-psake" -Scope Global 
    Set-Alias "ptest" "invoke-pester" -Scope Global 
}

if ((Test-Path "./.setup") -and !$Force.ToBool()) { 
    Update-Environment
    exit 
}
$mods = Get-Module -ListAvailable

function Install-Mod {
    Param(
        [String] $Name,
        [hashtable] $ArgumentList 
    )

    $args = @{
        "SkipPublisherCheck" = $true 
        "AllowClobber"       = $true 
        "Force"              = $true 
    }
    if ($ArgumentList) {
        foreach ($item in $ArgumentList.Keys) {
            $args[$item] = $ArgumentList[$item]
        }
    }

    if ($mods.Name.Contains($Name)) {
   
        if (!$args["RequiredVersion"]) {
            return;
        }

        $version = New-Object System.Version -ArgumentList $args["RequiredVersion"]
        $mod = $mods | Where-Object { $_.Name -eq $Name } | Select-Object -First 
        
        if ($mod -and $version -lt $mod.Version.ToString()) {
            Update-Module $Name -RequiredVersion $args["RequiredVersion"]
            return 
        }
        else {
            return 
        }
    }

    $scope = "AllUsers" 
    $isAdmin = [bool](([System.Security.Principal.WindowsIdentity]::GetCurrent()).groups -match "S-1-5-32-544")
    if (!$isAdmin) {
        $scope = "CurrentUser"
    }

    Install-Module $Name @args -Scope $scope 
    return 
}

Install-Mod Gz-ChocolateySync 
Install-Mod Psake
Install-Mod Pester 

if ($null -eq (Get-Command choco -EA SilentlyContinue)) {
    if (!$Force.ToBool()) {
        Write-Warning "Chocolatey is required and not installed on path"
        Write-Warning "If you continue, chocolatey will be installed to c:/apps"
        $stop = Read-Host  "install chocolatey now? Press 'y' to continue"
        if ($stop -ne "y") {
            exit 
        }
    }   
}

Sync-Chocolatey -Uri "$PsSCriptRoot/build/chocolatey.json"

if ($null -eq (Get-Command dotnet -EA SilentlyContinue)) {
    $env:Path += ";$Env:ProgramFiles\dotnet"
}

# not available via chocolatey
if ($null -eq (Get-Command nbgv -EA SilentlyContinue)) {
    dotnet tool install -g nbgv 
}

$gitRemotes = git remote 

$remotes = @{
    "gitlab" = "https://gitlab.com/nerdymishka/dotnet"
    "github" = "https://github.com/nerdymishka/dotnet"
    "vsts"   = "https://dev.azure.com/nerdymishka/dotnet/_git/dotnet"
}

foreach ($key in $remotes.Keys) {
    
    if ($gitRemotes.Contains($key)) {
        continue;
    }
    
    if (!$gitRemotes.Contains($key)) {
        git remote add $key "$($remotes[$key])"
    }

    if ($key -eq "origin") {
        continue 
    }

    git remote set-url origin --push --add "$($remotes[$key])"
}

Update-Environment

"" > .setup