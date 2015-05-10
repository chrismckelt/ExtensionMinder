@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild ExtensionMinder.sln /p:Configuration="Release" /p:OutputPath=build /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

mkdir Build
mkdir Build\lib
%nuget% pack "ExtensionMinder.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version 1.0.2.0 -p Configuration="Release"

pause