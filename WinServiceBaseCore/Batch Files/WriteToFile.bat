@echo off

:WriteToFileShare
echo Calling WinServiceBaseCore to Write a file to a file share
"D:\Source\GitHub\WinServiceBaseCore\WinServiceBaseCore\bin\Debug\net8.0\WinServiceBaseCore.exe" --WriteFile

:exit
pause
exit