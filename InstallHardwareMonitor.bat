@echo off

net session >nul 2>&1
if %errorLevel% == 0 (
    goto init
) else (
    echo ERROR: Script must be run in an elevated command window!
    goto end
)

:init
if "%1"=="" (
    goto help
)

if exist "HardwareMonitor/HardwareMonitor/bin/Release" (
    set location="HardwareMonitor/HardwareMonitor/bin/Release"
) else if exist "HardwareMonitor/HardwareMonitor/bin/Debug" (
    set location="HardwareMonitor/HardwareMonitor/bin/Debug"
) else (
    echo ERROR: Source code must be compiled before the service can install/uninstall!
    goto end
)

set action=%~1

if %action%==install (
    cd %location%
    HardwareMonitor.exe install
    cd ..\..\..\..\
    goto end
) else if %action%==uninstall (
    cd %location%
    HardwareMonitor.exe uninstall
    cd ..\..\..\..\
    goto end
)

:help
echo "Please supply one of the following commands - install/uninstall"

:end