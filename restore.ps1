




function Update-Environment() {
    Set-Alias "psake" "invoke-psake" -Scope Global 
    Set-Alias "ptest" "invoke-pester" -Scope Global 
}

if(Test-Path "./.setup") { 
    Update-Environment
    exit 
}

Install-Module Psake -SkipPublisherCheck -AllowClobber -Force
Install-Module Pester -SkipPublisherCheck -Force -AcceptLicense -AllowClobber
Update-Environment

"" > .setup