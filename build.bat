@echo off
cls
"Tools\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "Tools" "-ExcludeVersion"
"Tools\FAKE\tools\Fake.exe" build.fsx
