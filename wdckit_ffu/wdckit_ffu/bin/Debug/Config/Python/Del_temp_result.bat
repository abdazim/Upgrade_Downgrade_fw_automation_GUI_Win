@echo off

::__________________________________________________________________________

::check if C:\Users\qa\Desktop\vdbench50407\Logs\TestRun001 exist and delete
IF exist "Latest_File" (del "Latest_File" && echo Latest_File Deleted) ELSE ( echo TestRun001 Not exists)
IF exist "..\..\Output\Tests_Num.txt" (del /s /q "..\..\Output\Tests_Num.txt" && echo ..\..\Output\Tests_Num.txt Deleted) ELSE ( echo ..\..\Output\Tests_Num.txt Not exists)
IF exist "..\..\Output\fw_activated_passed.txt" (del /s /q "..\..\Output\fw_activated_passed.txt" && echo ..\..\Output\fw_activated_passed.txt Deleted) ELSE ( echo ..\..\Output\fw_activated_passed.txt Not exists)
IF exist "..\..\Output\fw_activated_failed.txt" (del /s /q "..\..\Output\fw_activated_failed.txt" && echo ..\..\Output\fw_activated_failed.txt Deleted) ELSE ( echo ..\..\Output\fw_activated_failed.txt Not exists)
IF exist "..\..\Output\if_device_found.txt" (del /s /q "..\..\Output\if_device_found.txt" && echo ..\..\Output\if_device_found.txt Deleted) ELSE ( echo ..\..\Output\if_device_found.txt Not exists)
IF exist "..\..\Output\if_device_found1.txt" (del /s /q "..\..\Output\if_device_found1.txt" && echo ..\..\Output\if_device_found1.txt Deleted) ELSE ( echo ..\..\Output\if_device_found1.txt Not exists)


::__________________________________________________________________________
::Removes (deletes) a directory.
::RMDIR [/S] [/Q] [drive:]path RD [/S] [/Q] [drive:]path
::/S      Removes all directories and files in the specified directory in addition to the directory itself.  Used to remove a directory tree.
::/Q      Quiet mode, do not ask if ok to remove a directory tree with /S
::__________________________________________________________________________
::rd /s /q "C:\My Folder\"
::/s: Deletes all files and folder from selected path.
::/q: Suppress any message.




