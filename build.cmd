if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\" goto msbuildvs15
if exist "%ProgramFiles(x86)%\MSBuild\14.0\bin" goto msbuildx86
if exist "%ProgramFiles%\MSBuild\14.0\bin" goto msbuild
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\" goto msbuildvs19preview

echo "Unable to detect suitable environment. Check if msbuild is installed."
exit 1

:msbuildvs15
set msbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\
goto build

:msbuildx86
set msbuildpath=%ProgramFiles(x86)%\MSBuild\14.0\bin\
goto build

:msbuild
set msbuildpath=%ProgramFiles%\MSBuild\14.0\bin\
goto build

:msbuildvs19preview
set msbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\
goto build

:build
set targets=Build
if not "%~1" == "" set targets=%~1
"%msbuildpath%msbuild.exe" /t:%targets% build\build.proj /fl /flp:logfile=build.log