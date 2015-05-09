@echo Off
set nuget=
if "%nuget%" == "" (
    set nuget=nuget
)

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild ExtensionMinder.sln /p:Configuration="Release" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /p:OutputPath=\\build\\lib\\net40\\

%nuget% pack extensionminder.nuspec"  -NoPackageAnalysis -OutputDirectory $buildArtifactsDirectory -verbosity detailed Configuration="Release"

