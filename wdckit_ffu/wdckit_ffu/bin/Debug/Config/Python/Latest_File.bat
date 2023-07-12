@echo off
cd ..\..\Logs
for /f %%i in ('dir /b/a-d/od/t:c') do set LAST=%%i
echo %LAST%> ..\Config\Python\Latest_File.txt

