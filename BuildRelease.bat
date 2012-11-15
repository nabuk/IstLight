@echo off
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild /nologo /verbosity:minimal /m:8 BuildRelease.msbuild
pause