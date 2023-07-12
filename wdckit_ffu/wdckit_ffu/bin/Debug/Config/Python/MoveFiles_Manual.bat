@echo off
cd ..\..\Logs\Manual
::get latest html file in current directory
FOR /F "delims=" %%I IN ('DIR *.txt /A:-D /O:D /B') do set "LATEST=%%~fI"
echo %latest% 

echo #########################

set hour=%time:~0,2%
if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
::echo hour=%hour%


::create new folder with date in \\10.0.56.14\Images\Abed\FFU_Logs\Manual
IF exist "%latest%" (mkdir "\\10.0.56.14\Images\Abed\FFU_Logs\Manual\Manual_Test_%date:~7,2%"-"%date:~-10,2%"-"%date:~-4,4%"-"%hour%"-"%time:~3,2%"-"%time:~6,2%" && echo create new folder with date in \\10.0.56.14\Images\Abed\FFU_Logs\Automation ) ELSE ( echo Latest file  not Exist)


::copy files to \\10.0.56.14\Images\Abed\Interoperability_logs\Manual
IF exist "%latest%" (xcopy "%latest%" "\\10.0.56.14\Images\Abed\FFU_Logs\Manual\Manual_Test_%date:~7,2%"-"%date:~-10,2%"-"%date:~-4,4%"-"%hour%"-"%time:~3,2%"-"%time:~6,2%"  && echo files copied to network path ..\FFU_logs\  ) ELSE ( echo Latest file not Exist)

