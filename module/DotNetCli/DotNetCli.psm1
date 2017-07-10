$script:IsUnix = $PSVersionTable.PSEdition -and $PSVersionTable.PSEdition -eq "Core" -and !$IsWindows

function Get-DotNetSdk {
    [Cmdletbinding()]
    param (
        [string]$SdkVersion,
        [string]$InstallPath = "$pwd/.dotnet"
    )

    # TODO: Check for dotnet.exe first

    Get-ChildItem "$InstallPath/sdk" `
    | Where-Object { (-not $SdkVersion) -or $_.Name -eq $SdkVersion } `
    | ForEach-Object { $_.FullName }
}

function Install-DotNetSdk {
    [CmdletBinding()]
    param (
        [string]$SdkVersion,

        [string]$InstallPath = "$pwd/.dotnet"
    )

    $dotnetExePath = if ($script:IsUnix) { "$dotnetPath/dotnet" } else { "$dotnetPath/dotnet.exe" }
    $originalDotNetExePath = $dotnetExePath

    if (!(Test-Path $dotnetExePath)) {
        $installedDotnet = Get-Command dotnet -ErrorAction Ignore
        if ($installedDotnet) {
            $dotnetExePath = $installedDotnet.Source
        }
        else {
            $dotnetExePath = $null
        }
    }

    # Make sure the dotnet we found is the right version
    if ($dotnetExePath -and (& $dotnetExePath --version) -eq $requiredSdkVersion) {
        $script:dotnetExe = $dotnetExePath
    }
    else {
        # Clear the path so that we invoke installation
        $script:dotnetExe = $null
    }

    Write-Host "`n### Installing .NET CLI $requiredSdkVersion...`n" -ForegroundColor Green

    # The install script is platform-specific
    $installScriptExt = if ($script:IsUnix) { "sh" } else { "ps1" }
    $installScriptPath = "$PSScriptRoot/scripts/dotnet-install.$installScriptExt"

    if (!$script:IsUnix) {
        & $installScriptPath -Version $requiredSdkVersion -InstallDir "$env:DOTNET_INSTALL_DIR"
    }
    else {
        & /bin/bash $installScriptPath -Version $requiredSdkVersion -InstallDir "$env:DOTNET_INSTALL_DIR"
        $env:PATH = $dotnetExeDir + [System.IO.Path]::PathSeparator + $env:PATH
    }

    Write-Host "`n### Installation complete." -ForegroundColor Green
    $script:dotnetExe = $originalDotnetExePath

    # This variable is used internally by 'dotnet' to know where it's installed
    $script:dotnetExe = Resolve-Path $script:dotnetExe
    if (!$env:DOTNET_INSTALL_DIR)
    {
        $dotnetExeDir = [System.IO.Path]::GetDirectoryName($script:dotnetExe)
        $env:PATH = $dotnetExeDir + [System.IO.Path]::PathSeparator + $env:PATH
        $env:DOTNET_INSTALL_DIR = $dotnetExeDir
    }

    Write-Host "`n### Using dotnet v$requiredSDKVersion at path $script:dotnetExe`n" -ForegroundColor Green
}

function Use-DotNetSdk {

}

function Invoke-DotNetCli {
    [CmdletBinding()]
    param (
        [Parameter(Position=1)]
        [string]$Command,

        [Parameter(Position=2)]
        [string[]]$Arguments
    )

    & $script:dotnetExe $Command ,$Arguments
}