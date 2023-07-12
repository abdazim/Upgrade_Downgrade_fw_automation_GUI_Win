@echo off

FOR /F "delims=" %%I IN ('DIR *.html /A:-D /O:D /B') do set "LATEST=%%~fI"
::echo %latest% >Tests_Num1.txt

find "The Result" "%latest%"  >Tests_Num.txt

::total=-2 becuase file TestResult21.txt have first line blank and second line file name 
::this code count the lines that find command write to logfile
cls
setlocal EnableDelayedExpansion
set /a total=-2
(
 for %%f in (Tests_Num.txt) do (
  for /f %%a in ('type "%%f"^|find /C /v  "" ') do set /a total+=%%a
)
set b=2

echo !total!>Tests_Num.txt
)


