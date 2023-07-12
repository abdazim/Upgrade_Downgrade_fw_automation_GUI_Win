@echo off
::get latest html file in current directory
FOR /F "delims=" %%I IN ('DIR *.html /A:-D /O:D /B') do set "LATEST=%%~fI"
::echo %latest% > aaa.txt
::find "No device found" "%latest%" >if_device_found.txt

::check if No device found exist in latest file  
(FindStr /IC:"No device found" "%latest%" >Nul && (Echo No Device Found) || Echo Device Found)>if_device_found1.txt
(FindStr /IC:"Error: Update failed " "%latest%" >Nul && (Echo No Device Found) || Echo Device Found)>if_device_found1.txt
(FindStr /IC:"Error: No Device Found " "if_device_found1.txt" >Nul && (Echo No Device Found) || Echo Device Found)>if_device_found.txt


