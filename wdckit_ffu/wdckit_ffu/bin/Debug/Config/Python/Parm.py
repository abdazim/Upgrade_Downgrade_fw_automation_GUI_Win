from Parm import *
import glob
import os

#_________________________________________________________________________________________________________________________________
# User params:
list_of_files = glob.glob('..\..\Logs\*.txt') # * means all if need specific format then *.txt
latest_file = max(list_of_files, key=os.path.getctime)
print ("Latest Log File:  ") +  str(list_of_files) + latest_file
LOG_PATH =  latest_file
#LOG_PATH = r"C:\Users\33330\Desktop\Python\d.txt"
#LOG_PATH = r"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\wdckit_ffu\wdckit_ffu\bin\Debug\Logs\log_10.0.57.37_disk2_01032021_045904.txt"
#_________________________________________________________________________________________________________________________________
#Activation Status Error
FAIL_CODE = 'Exit Code'
Start_Status = 0 
End_Status = 20
#Update disk2: Firmware downloaded successfully , srart from 50
FW_DOWNLOAD_RESULT_START = 127
#durent to curent start from 112 
SWITCH_FWS_STATUS_START = 112
SWITCH_FWS_STATUS_END = 600
#fw after curent to current path - 127-500  -> \\10.0.56.14\Images\PlatformTesting\FW_Versions\calypso_perf ....
FW_NAME_START = 127
FW_NAME_END = 500
# Current FW: START FROM 105 TO 300 TO SHOW THE PATH (C:\Users\Qa\Desktop\FFU_Automation_tool_1.0)
CURRENT_FW_START = 113
CURRENT_FW_END = 300
#_________________________________________________________________________________________________________________________________
NEW_LINE = '\n'
SEPARATION = '-' * 150
PATH_IDENTIFIER = "-f"
Current_FW= "-Current FW:"
FW_ACTIVATED_PASS_KEY_WORD = "Activation was successful"
FW_ACTIVATED_FAIL_KEY_WORD = "Failure (device reported an error)"
EMPTY_STRING = ""
SPACE = " "
FW_SWITCH_IDENTIFIER = "------------"
FW_DOWNLOAD_IDENTIFIER = "Stored"
FW_CURRENT_INDETIFIER = '-Current FW:'
RESULT_TITLE = '\nThe Result: '
CURRENT_FW_PATH = "Current FW Path:  "
UNABLE_TO_OPEN_FIRMWARE_IMAGE = 'No devices found'
FAILED_DRIVE = 'FAILED DRIVE'
FAILED_DRIVE_OUT_PUT = 'driver ERROR '
FAILURE = 'Fail'
ACTIVATED_FAILURE_EXPRESSION = 'Firmware Activated Failed.'
ACTIVATED_PASS_EXPRESSION = 'Firmware Activated passed.'
DOWNLOAD_PASSED_ACTIVATE_FAILED_RESULT_IS_FAILED_AS_EXPECTED_EXPRESSION = 'Failed as expected'
DOWNLOAD_RESULT_IS_PASS_INDECATOR = 'successfully'
ACTIVATE_RESULT_IS_FAIL_INDECATOR = 'Fail'
UNABLE_TO_OPEN_FIRMWARE_IMAGE_OUT_PUT = "no"
#_________________________________________________________________________________________________________________________________
# Get functions - do not change.
def getFwPath(line):
    return line[CURRENT_FW_START:CURRENT_FW_END]

def getSwitchFwStatus(line):
    return line[SWITCH_FWS_STATUS_START:SWITCH_FWS_STATUS_END]

def getFwName(line):
    return line[FW_NAME_START:FW_NAME_END].strip()

def getFwDownloadResult(line):
    return line[FW_DOWNLOAD_RESULT_START::]
    
def getActive(line):
    return line[Start_Status:End_Status]