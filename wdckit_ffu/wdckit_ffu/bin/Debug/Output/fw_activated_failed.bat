@echo off

FOR /F "delims=" %%I IN ('DIR *.html /A:-D /O:D /B') do set "LATEST=%%~fI"
::findstr /I /R /C:"The Result" %latest%>Test_Num.txt
::echo %latest% > aaa.txt

find "Firmware Activated Failed." "%latest%" >fw_activated_failed.txt

::total=-2 becuase file TestResult21.txt have first line blank and second line file name 
::this code count the lines that find command write to logfile
cls
setlocal EnableDelayedExpansion
set /a total=-2
(
 for %%f in (fw_activated_failed.txt) do (
  for /f %%a in ('type "%%f"^|find /C /v  "" ') do set /a total+=%%a
 )
set b=2

echo !total!>fw_activated_failed.txt

)






