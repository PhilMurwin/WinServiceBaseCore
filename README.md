## WinServiceBase Summary
This is a windows service base project that is designed to run multiple processes semi-dynamically.  It can be
run from the command line using a flag or as a windows service.  Starts up multiple processes each in it's own
thread.  Processes are expected to have a boolean config flag in the app.config

### Running the application/service
* When in Visual Studio Debug mode the application starts in console mode
* **To run from a command prompt**
    * pass "--console" to the executable as you would any command line
* **To run as a windows service**
    * Use the "Batch Files" to add the windows service to the local services management console on your local
    or server machine
    * Batch Scripts are written for the path: C:\Services\WinServiceBaseCore\WinServiceBaseCore.exe

### Adding a Process
* Create a new class that derives from ProcessBase
    * **Required**: StopCode - string variable used to shutdown the process on command
    * **Required**: *CanStartProcess* - bool variable used to determine if the process should be started when the application is started
    * **Required**: *DoProcessWork()* - This method is called to start the process in it's own thread.

* NLog can be used for logging
    * A default logger is instantiated for processes deriving from ProcessBase, use ProcessLogger for creating log messages

### Existing Processes
* Basic Time Logger
    * A simple process that creates a log entry once a minute.
    * Primarily used to test that the service is functional
