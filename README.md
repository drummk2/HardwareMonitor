# HardwareMonitor
A TopShelf windows service/console application that periodically and asynchronously captures and logs a user's:
  * Available RAM.
  * CPU usage.
  * Download Speed.
  * Free space on all "fixed" drives.

Doing so should prove useful when debugging performance issues with one's PC.

## Installing The HardwareMonitor
  1. Clone this repo to a desired location.
  2. Open a CMD window as an administrator and navigate to the top-level folder of the HardwareMonitor.
  3. Run the following command - ```HardwareMonitor.bat install```

## Uninstalling The HardwareMonitor
  1. Open a CMD window as an administrator and navigate to the top-level folder of the HardwareMonitor.
  2. Run the following command - ```HardwareMonitor.bat uninstall```

## Viewing The HardwareMonitor's Results
All log files for the HardwareMonitor will be stored in a folder called "Logs" in the top-level folder of the HardwareMonitor.
