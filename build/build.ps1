Set-Location build
GitVersion /output buildserver
$VersionJson = GitVersion | out-string
$Versions = ConvertFrom-Json $VersionJson
Set-Location ..

$Version = $Versions.MajorMinorPatch
$NuGetVersion = $Versions.NuGetVersion

dotnet build -c Release /p:Version=$Version
dotnet test src/LazyStorage.Tests/LazyStorage.Tests.csproj --logger "trx;LogFileName=LazyStorage.Tests.trx"
dotnet pack src\LazyStorage\LazyStorage.csproj -c Release /p:Version=$Version /p:PackageVersion=$NuGetVersion