param([string]$PackageDirectory = 'artifacts')

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

# These assemblies embed Castle.Core; publishing it as a dependency breaks
# consumers using a different Castle version (refs HERMA-15243).
foreach ($packageId in @('HermaFx.SimpleConfig', 'HermaFx.SettingsAdapter')) {
    $packages = @(Get-ChildItem $PackageDirectory -Filter "$packageId.*.nupkg")
    if ($packages.Count -ne 1) {
        throw "Expected exactly one package for $packageId, found $($packages.Count)."
    }

    $archive = [System.IO.Compression.ZipFile]::OpenRead($packages[0].FullName)
    try {
        $entry = $archive.GetEntry("$packageId.nuspec")
        $reader = [System.IO.StreamReader]::new($entry.Open())
        try {
            [xml]$manifest = $reader.ReadToEnd()
        }
        finally {
            $reader.Dispose()
        }

        $castleDependency = $manifest.SelectSingleNode("//*[local-name()='dependency' and @id='Castle.Core']")
        if ($null -ne $castleDependency) {
            throw "$packageId must not declare Castle.Core as a dependency: it is embedded in the assembly."
        }
        Write-Output "$packageId embeds Castle.Core without an external dependency: OK"
    }
    finally {
        $archive.Dispose()
    }
}
