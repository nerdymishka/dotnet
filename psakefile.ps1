
# PS Analyzer doesn't understand nature of Psake's Properties
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
Param()

Properties {

    $msbuild = @{
        configuration = (Get-ConfigProp "NM_BUILD_CONFIG" -Default "Release")
    }
    $ci = @{
        artifactsDir = (Get-ConfigProp "BUILD_ARTIFACTSSTAGINGDIRECTORY" `
            -Default "$PsScriptRoot/artifacts")
    }
}

Task "test:unit" {
    exec { 
        $testDir = "$($ci.artifactsDir)/tests/unit"
        if(!(Test-Path $testDir)) { New-Item $testDir -ItemType Directory }
        dotnet test -c $msbuild.configuration --filter tag=unit -r "$testDir"
    }
}

Task "test:integration" {
    exec { 
        $testDir = "$($ci.artifactsDir)/tests/integration"
        if(!(Test-Path $testDir)) { New-Item $testDir -ItemType Directory }
        dotnet test -c $msbuild.configuration --filter tag=integration
    }
}

Task "restore" {
    exec {
        dotnet restore 
    }
}

Task "clean:artifacts" {
    $items = Get-ChildItem $ci.artifactsDir -EA SilentlyContinue
    
    if($items) {
        $items | Remove-Item -Force -Recurse
    }
}

Task "clean" {
    exec {
        dotnet clean -c $msbuild.configuration
    }
}

Task "build" {
    exec {
        dotnet build --no-restore -c $msbuild.configuration
    }
}

Task "setup" {
    if(!(Test-Path $ci.artifactsDir)) {
        New-Item -ItemType Directory $ci.artifactsDir
    }
}


Task "default" -depends "setup", "clean:artifacts", "restore",`
     "clean", "build", "test:unit" 

function Get-ConfigProp() {
    Param(
        [Parameter(Position = 0)]
        [String[]] $Name,

        [String] $Default 
    )

    foreach($item in $name)
    {
        $value = Get-Item  Env:$Name -EA SilentlyContinue
        if($value) { return $value }
    }
    
    return $Default
}