@rem -- Prevent creating "Nfm.sln.cache" file --
@set MSBuildUseNoSolutionCache=1

@c:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe /t:Clean /p:Configuration=Release Nfm.sln
@c:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe /t:ReBuild /p:Configuration=Release Nfm.sln