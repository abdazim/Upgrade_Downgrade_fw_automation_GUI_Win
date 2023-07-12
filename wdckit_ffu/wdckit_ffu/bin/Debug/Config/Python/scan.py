from collections import OrderedDict
from collections import namedtuple
from Parm import *

# The function collects the information about the process and put it in the dict.
def createResultsDict():
    dictOfVersions = OrderedDict()
    result = namedtuple('result', ['fwDownload', 'fwActivate'])
    pathFinalAdress,fwDownloadResult,fwDownloadResult,fwDownloadResult,fwActivateResult,pathName = EMPTY_STRING , '','','','','' # resets the variables.
    
    
    for line in open(LOG_PATH, "r").readlines():


        if "No devices found" in line:
            fwDownloadResult ="No device found" 
            fwActivateResult = "No device found"
            print fwDownloadResult
            break

        if FW_CURRENT_INDETIFIER in line:
            print (CURRENT_FW_PATH + getFwPath(line) + NEW_LINE + SEPARATION)
            
        if FW_SWITCH_IDENTIFIER in line:
            switchFWsStatus = getSwitchFwStatus(line)
            pathFinalAdress += switchFWsStatus

        if PATH_IDENTIFIER in line:
            fwName = getFwName(line)
            pathFinalAdress += fwName
            pathName = pathFinalAdress

        if FW_DOWNLOAD_IDENTIFIER in line:
            fwDownloadResult = getFwDownloadResult(line)

            if UNABLE_TO_OPEN_FIRMWARE_IMAGE in fwDownloadResult:
                fwDownloadResult = UNABLE_TO_OPEN_FIRMWARE_IMAGE_OUT_PUT

            if FAILED_DRIVE in fwDownloadResult:
                fwDownloadResult = FAILED_DRIVE_OUT_PUT

        # if FAIL_KEY_WORD = "FAILED" in log file show :Activated Failed"
        if FW_ACTIVATED_FAIL_KEY_WORD in line:
            fwActivateResult = ACTIVATED_FAILURE_EXPRESSION
            pathFinalAdress = EMPTY_STRING

        # if PASS_KEY_WORD = "successfully" show ":Activated passed"
        if FW_ACTIVATED_PASS_KEY_WORD in line:
            fwActivateResult = ACTIVATED_PASS_EXPRESSION \
                if fwDownloadResult != UNABLE_TO_OPEN_FIRMWARE_IMAGE_OUT_PUT \
                else EMPTY_STRING
            pathFinalAdress = EMPTY_STRING
                  
        if FAIL_CODE in line:
          #  print ( getActive(line))
            fwActivateResult += line
            New_Fail_Code = FAIL_CODE
            
            
        if pathName != '':
            finalResultTuple = result(fwDownloadResult.strip(), fwActivateResult.strip())
            dictOfVersions[pathName] = DOWNLOAD_PASSED_ACTIVATE_FAILED_RESULT_IS_FAILED_AS_EXPECTED_EXPRESSION if DOWNLOAD_RESULT_IS_PASS_INDECATOR in fwDownloadResult \
                                                                                                                  and ACTIVATE_RESULT_IS_FAIL_INDECATOR in fwActivateResult \
                                                                                                                    else finalResultTuple # The value for each key will be a string or a tuple which represents the result.
       
    return dictOfVersions


# The function prints the dict to the console.
def printToConsule(dictOfVersions):
    for version, result in dictOfVersions.items():
        output = (version + RESULT_TITLE + result + NEW_LINE + NEW_LINE + SEPARATION ) if isinstance(result,str) \
                                                                                        else (version + RESULT_TITLE + NEW_LINE + result.fwDownload + NEW_LINE +  result.fwActivate + NEW_LINE +  SEPARATION)        
        print(output)
            #write output to file
    for version, result in dictOfVersions.items():
        output = (version + RESULT_TITLE + result + NEW_LINE + NEW_LINE + SEPARATION) if isinstance(result, str) \
                                                                                        else (version + RESULT_TITLE + NEW_LINE + result.fwDownload + NEW_LINE + result.fwActivate + NEW_LINE + SEPARATION)
        write_File(output)
        
#if file exist remove and create again to start write from begaining
def check_file():
    if os.path.exists("py_output.txt"):
        print("The file exist")
        os.remove("py_output.txt")
        textfile = open('py_output.txt', 'w')
        textfile.write("\n")

#write file to txt file , call from printToConsule(dictOfVersions): function
def write_File(output):
    textfile = open('py_output.txt', 'a')
    textfile.write("\n")
    textfile.write(output)
    textfile.close()        


def main():
    check_file()
    printToConsule(createResultsDict())


main()
