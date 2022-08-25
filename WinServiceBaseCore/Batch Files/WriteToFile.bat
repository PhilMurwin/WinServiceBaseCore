@echo off

:WriteToFileShare
echo Calling WinServiceBaseCore to Write a file to a file share
"C:\Source\GitHub\WinServiceBaseCore\WinServiceBaseCore\bin\Debug\net6.0\WinServiceBaseCore.exe" --WriteFile

:exit
set /p leave=Press any key to quit
exit