## WinServiceBaseCore Summary
This is a .Net Core windows service base project that is designed to run multiple processes.  It can be
run from the command line using a flag or as a windows service.  Starts up multiple processes each in it's own
thread.  Processes are expected to have a boolean config flag in the app.config

### Running the application/service
* **When running in Visual Studio Debug the app starts as a console app**
* **To run from a command prompt**
    * pass "--console" to the executable as you would any command line
* **To run as a windows service**
    * Use the "Batch Files" to add the windows service to the local services management console on your local
    or server machine
    * Batch Scripts are written for the path: C:\Services\WinServiceBaseCore\WinServiceBaseCore.exe

### Adding a Process
* Create a new class that derives from ProcessBase
    * **Required**: *StopCode* - string variable used to shutdown the process on command    
    * **Required**: *CanStartProcess* - bool variable used to determine if the process should be started when the application is started
    * **Required**: *Frequency* - Interval (in minutes) that the process is executed
    * **Required**: *DoProcessWork()* - This method is called to start the process in it's own thread.
* Register the new Process in App_Entry/ServiceConfig
    ```C#
    services.AddHostedServices<BasicTimeLogger>();
    ```

* NLog can be used for logging
    * A default logger is instantiated for processes deriving from ProcessBase, use ProcessLogger for creating log messages

### Existing Processes
* Basic Time Logger
    * A simple process that creates a log entry once a minute.
    * Primarily used to test that the service is functional
* Email Test Interval
    * When properly configured this should trigger some startup/shutdown emails as well as a time email once a minute
    * Primarily used to test that the service is functional and that email can be sent
