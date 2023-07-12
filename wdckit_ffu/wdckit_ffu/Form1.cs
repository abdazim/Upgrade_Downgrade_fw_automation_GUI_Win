using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;


namespace wdckit_ffu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Ping_file(); //check file Ip_Connect.txt if esist and read ip
            /////////////////////////////////////////////////////////////////////////////////////
            ///combobox      using System.Management;    + add to refenrances from right solution explorer->right click and add (system.managment)
            ///            //{
            //    //Win32_DiskDrive class , https://msdn.microsoft.com/en-us/library/aa394132(v=vs.85).aspx
            ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject moDisk in mosDisks.Get())
            {
                comboBox1.Items.Add(moDisk["Name"].ToString());
                //comboBox1.SelectedIndex = 0;  //nvmekit show disk1(defult selection .. the first opition) 
            }
            ////////////////////////////////////////////////////////////////////////////////////
        }


        //Manual FFU//////////////////////////////////////////////////////
        /// <summary>
        /// Device info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        string gettext = ""; //gettext =comboBox1.SelectedItem (device info button)
        string showinfo;
        string idd;
        private void button1_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
            label2.Refresh();
            Test_LogFile_Manual_FFU("button1_Click-Manual FFU- Device Info start");
            /// combobox(form1()) with if      using System.Management;
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("No Device is Selected");
                return;
            }
            else
            {
                switch (comboBox1.SelectedItem.ToString())
                {
                    case @"\\.\PHYSICALDRIVE0":
                        gettext = "disk0";
                        break;
                    case @"\\.\PHYSICALDRIVE1":
                        gettext = "disk1";
                        break;
                    case @"\\.\PHYSICALDRIVE2":
                        gettext = "disk2";
                        break;
                    case @"\\.\PHYSICALDRIVE3":
                        gettext = "disk3";
                        break;
                    case @"\\.\PHYSICALDRIVE4":
                        gettext = "disk4";
                        break;
                    case @"\\.\PHYSICALDRIVE5":
                        gettext = "disk5";
                        break;
                    case @"\\.\PHYSICALDRIVE6":
                        gettext = "disk6";
                        break;
                    case @"\\.\PHYSICALDRIVE7":
                        gettext = "disk7";
                        break;
                    default:
                        MessageBox.Show("Invalid PHYSICAL DRIVE!");
                        break;
                }
            }

            ///////////////////////////////
            // cmd run commands + get the device name from comobox1 and show the device info
            try
            {
                ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();
                string path = @".\.\wdckit\wdckit.exe";
                process.UseShellExecute = false;
                process.RedirectStandardOutput = true;
                process.RedirectStandardInput = true;
                process.WorkingDirectory = Path.GetDirectoryName(path);
                process.CreateNoWindow = true;
                var proc = Process.Start(process);

                showinfo = ("wdckit show " + gettext);
                proc.StandardInput.WriteLine(showinfo);

                idd = ("wdckit idd " + gettext + " -c "); //info about Drive+FW Number ->> Retrieves Identify Data of controller of all the supported devices.
                proc.StandardInput.WriteLine(idd);

                proc.StandardInput.WriteLine("exit");
                string s = proc.StandardOutput.ReadToEnd(); //show inforamtion in textbox1.text
                //textBox1.Text = s;
                richTextBox1.Text= s;
                Test_LogFile_Manual_FFU("button1_Click-Manual FFU- Device Info : "+ showinfo +" , "+ idd);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Device info: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
            Test_LogFile_Manual_FFU("button1_Click-Manual FFU- Device Info End");
        }



        /// <summary>
        /// manual start ffu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        string gettext1 = ""; //gettext1 =comboBox1.SelectedItem (start button)
        private void button3_Click(object sender, EventArgs e)
        {
            //label3.Text = ""; label3.Refresh();
            label4.Text = ""; label4.Refresh();
            label5.Text = ""; label5.Refresh();
            label6.Text = ""; label6.Refresh();
            richTextBox1.Text = "";

            var dateOne = DateTime.Now;  //date time 
            label2.Visible = false;
            label2.Refresh();
            Application.DoEvents();   //to update the main form every single cycle and show the label
            Test_LogFile_Manual_FFU("button3_Click-Manual FFU start");
            /// combobox(form1()) check if combobx1 !=null      using System.Management;
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(textBox2.Text))  //chosose device and fluf file togother comboBox1.Text+textBox2.Text
            {
                MessageBox.Show("Choose Device / fluf file");
                return;
            }
            else
            {
                switch (comboBox1.SelectedItem.ToString())
                {
                    case @"\\.\PHYSICALDRIVE0":
                        gettext1 = "disk0";
                        break;
                    case @"\\.\PHYSICALDRIVE1":
                        gettext1 = "disk1";
                        break;
                    case @"\\.\PHYSICALDRIVE2":
                        gettext1 = "disk2";
                        break;
                    case @"\\.\PHYSICALDRIVE3":
                        gettext1 = "disk3";
                        break;
                    case @"\\.\PHYSICALDRIVE4":
                        gettext1 = "disk4";
                        break;
                    case @"\\.\PHYSICALDRIVE5":
                        gettext1 = "disk5";
                        break;
                    case @"\\.\PHYSICALDRIVE6":
                        gettext1 = "disk6";
                        break;
                    case @"\\.\PHYSICALDRIVE7":
                        gettext1 = "disk7";
                        break;
                    default:
                        MessageBox.Show("Invalid PHYSICAL DRIVE!");
                        break;
                }
            }
            label2.Visible = false;
            label2.Refresh();

            label2.Visible = true; label2.BackColor = Color.Yellow; label2.Text = "Running";
            label2.Refresh();
            ///////////////////////////////
            // cmd run commands + open file dialog with connect a string str variable 
            try
            {
                //var dateOne1 = DateTime.Now;  //date time 
                //label2.Visible = true; label2.BackColor = Color.Yellow; label2.Text = "Running";
                //label2.Refresh();

                ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";  
                process.UseShellExecute = false;
                process.RedirectStandardOutput = true;
                process.RedirectStandardInput = true;
                process.WorkingDirectory = Path.GetDirectoryName(path);
                process.CreateNoWindow = true;
                var proc = Process.Start(process);
                ////////////
                //string dr1 = ("nvmekit.exe fwdownload " + gettext1.ToString() + " -f " + newpath);   //string str = openfiledialog file name  gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                //proc.StandardInput.WriteLine(dr1);


                //string dr2 = ("nvmekit.exe fwactivate " + gettext1.ToString() + " -s 1"); //gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                //proc.StandardInput.WriteLine(dr2);

                string dr1 = ("wdckit.exe update " + gettext1.ToString() + " -f " + newpath);   //string str = openfiledialog file name  gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                proc.StandardInput.WriteLine(dr1);

                string separation = ("___________________________________________________________________________");
                proc.StandardInput.WriteLine(separation);

                string dr2 = ("wdckit.exe update " + gettext1.ToString() + " -a -s 1"); //gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                proc.StandardInput.WriteLine(dr2);

                proc.StandardInput.WriteLine("exit");
                
                string s = proc.StandardOutput.ReadToEnd();

                output_manual = s;

                //result/////////////////////////////////////////////////////////////////////////////
                //Test Summary/////////////////////////////////////////////////////////////////////////////

                Writelog_manual();
                if (File.Exists(file_manual))
                {
                    //Vars
                    string result1_TestsSummary = "Test Summary:";
                    string result1_Device = "Device:";
                    string result1_FW = "FW:";
                    string result1_Test_Result = "Test Result:";
                    string result1_separate = "####################################################";
                    string Test_Start = "Test Start:";

                    //Read_txt_file(file_manual); run test and show on richtextbox 1 with colors 
                    RichTextBox_result(file_manual); //file_manual log file created after run Writelog_manual();

                    //test summary with colors show on richtextbox 1 append 
                    richTextBox1.AppendText("\n");
                    richTextBox1.AppendText("####################################################" + "\n");
                    richTextBox1.AppendText("Test Summary:" + "\n");
                    richTextBox1.AppendText("Device: " + gettext1 + "\n");
                    richTextBox1.AppendText("FW: " + textBox2.Text+ "\n");

                    int index_1 = 0;
                    //Test_Start = "Test Start:";
                    while (index_1 < richTextBox1.Text.LastIndexOf(Test_Start))
                    {
                        richTextBox1.Find(Test_Start, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.Turquoise;
                        richTextBox1.SelectionFont = new Font("Tahoma", 12, FontStyle.Bold);
                        index_1 = richTextBox1.Text.IndexOf(Test_Start, index_1) + 1;
                    }
                    //result1_TestsSummary = "Tests Summary: ";
                    index_1 = 0;
                    while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsSummary))
                    {
                        richTextBox1.Find(result1_TestsSummary, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.Purple;
                        richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                        index_1 = richTextBox1.Text.IndexOf(result1_TestsSummary, index_1) + 1;
                    }
                    //result1_Device = "Device:";
                    
                    while (index_1 < richTextBox1.Text.LastIndexOf(result1_Device))
                    {
                        richTextBox1.Find(result1_Device, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.Purple;
                        richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                        index_1 = richTextBox1.Text.IndexOf(result1_Device, index_1) + 1;
                    }
                    //result1_FW = "FW:";
                    index_1 = 0;
                    while (index_1 < richTextBox1.Text.LastIndexOf(result1_FW))
                    {
                        richTextBox1.Find(result1_FW, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.Purple;
                        richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                        index_1 = richTextBox1.Text.IndexOf(result1_FW, index_1) + 1;
                    }


                    if (manual_run_count == "Test Pass") // from function RichTextBox_result(file_manual);
                    {
                        richTextBox1.AppendText("Test Result: " + "Test Pass" + "\n");
                        string result1 = "Test Pass";
                        int index = 0;
                        while (index < richTextBox1.Text.LastIndexOf(result1))
                        {
                            richTextBox1.Find(result1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                            richTextBox1.SelectionColor = Color.Green;
                            richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                            index = richTextBox1.Text.IndexOf(result1, index) + 1;
                        }
                        var dateOne_end = DateTime.Now;
                        label2.Visible = true; label2.BackColor = Color.Green; label2.Text = "Test Done" + " , Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss") + " , " + manual_run_count);
                        label2.Refresh();
                    }
                    else if (manual_run_count == "Test Fail") // from function RichTextBox_result(file_manual);
                    {
                        richTextBox1.AppendText("Test Result: " + "Test Fail" + "\n");
                        string result1 = "Test Fail";
                        int index = 0;
                        while (index < richTextBox1.Text.LastIndexOf(result1))
                        {
                            richTextBox1.Find(result1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                            index = richTextBox1.Text.IndexOf(result1, index) + 1;
                        }
                        var dateOne_end2 = DateTime.Now;
                        label2.Visible = true; label2.BackColor = Color.Red; label2.Text = "Test Done" + " , Test Duration: " + ((dateOne_end2 - dateOne).ToString(@"hh\:mm\:ss") + " , " + manual_run_count);
                        label2.Refresh();
                        Test_LogFile_Manual_FFU("Read_txt_file(file_manual) != null " + file_manual);
                    }
                    else if (manual_run_count == "No Device Found")
                    {
                        richTextBox1.AppendText("Test Result: " + "No Device Found" + "\n");
                        string result1 = "No Device Found";
                        int index = 0;
                        while (index < richTextBox1.Text.LastIndexOf(result1))
                        {
                            richTextBox1.Find(result1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                            index = richTextBox1.Text.IndexOf(result1, index) + 1;
                        }
                        var dateOne_end2 = DateTime.Now;
                        label2.Visible = true; label2.BackColor = Color.Red; label2.Text = "Test Done" + " , Test Duration: " + ((dateOne_end2 - dateOne).ToString(@"hh\:mm\:ss") + " , " + manual_run_count);
                        label2.Refresh();              
                        Test_LogFile_Manual_FFU("Read_txt_file(file_manual) != null " + file_manual);

                    }
                    //Command Execution Failed
                    else if (manual_run_count == "Command Execution Failed")
                    {
                        richTextBox1.AppendText("Test Result: " + "Test Fail" + "\n");
                        string result1 = "Test Fail";
                        int index = 0;
                        while (index < richTextBox1.Text.LastIndexOf(result1))
                        {
                            richTextBox1.Find(result1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                            index = richTextBox1.Text.IndexOf(result1, index) + 1;
                        }
                        var dateOne_end2 = DateTime.Now;
                        label2.Visible = true; label2.BackColor = Color.Red; label2.Text = "Test Done" + " , Test Duration: " + ((dateOne_end2 - dateOne).ToString(@"hh\:mm\:ss") + " , " + "Test Fail");
                        label2.Refresh();
                        Test_LogFile_Manual_FFU("Read_txt_file(file_manual) != null " + file_manual);

                    }
                    else
                    {
                        richTextBox1.AppendText("Test Result: " + "Else Failure" + "\n");
                        var dateOne_end2 = DateTime.Now;
                        label2.Visible = true; label2.BackColor = Color.Red; label2.Text = "Test Done" + " , Test Duration: " + ((dateOne_end2 - dateOne).ToString(@"hh\:mm\:ss") + " , " + "Else Failure");
                        label2.Refresh();
                        Test_LogFile_Manual_FFU("Read_txt_file(file_manual) != null " + file_manual);
                    }

                    richTextBox1.AppendText("####################################################");
                    //result1_Test_Result = "Test Result:";
                    index_1 = 0;
                    while (index_1 < richTextBox1.Text.LastIndexOf(result1_Test_Result))
                    {
                        richTextBox1.Find(result1_Test_Result, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.Purple;
                        richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                        index_1 = richTextBox1.Text.IndexOf(result1_Test_Result, index_1) + 1;
                    }

                    //result1_separate = "####################################################";
                    index_1 = 0;
                    while (index_1 < richTextBox1.Text.LastIndexOf(result1_separate))
                    {
                        richTextBox1.Find(result1_separate, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.MediumTurquoise;
                        richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                        index_1 = richTextBox1.Text.IndexOf(result1_separate, index_1) + 1;
                    }
                } //if(file.exist)


                

                //richTextBox1.Text = s;
                //var dateOne_end = DateTime.Now;
                //label2.Visible = true; label2.BackColor = Color.Green; label2.Text = "Test Done" + " , Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss")+ " , "+manual_run_count);
                //label2.Refresh();
                string path1 = Directory.GetCurrentDirectory();
                string directory1 = path1 + @"\Config\Python\";
                string filter2 = @"\Config\Python\MoveFiles_Manual.bat";
                Run_bat_file_new(directory1, filter2);
                Test_LogFile_Manual_FFU("button3_Click-Manual "+ " ,dr1: "+dr1+ " , dr2: "+ dr2);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
            Test_LogFile_Manual_FFU("button3_Click-Manual FFU Done");
        }


        /// <summary>
        /// brouwse fluf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        Stream myStream = null;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        string str;
        string newpath; // new path of openFileDialog1.FileName with " " to avoid space issue  path
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "fluf files (*.fluf)|*.fluf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    str = openFileDialog1.FileName;
                    textBox2.Text = str.ToString();
                    //new path with " " to avoid path issue with spaces 
                    string quote = "\"";
                    newpath = quote + str.ToString() + quote;
                    //MessageBox.Show(str);

                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
                }
            }
        }



        /// <summary>
        /// Log file create function
        /// </summary>
        /// <param name="sEventName"></param>
        /// <param name="sControlName"></param>
        /// <param name="sFormName"></param>
        public void LogFile(string sEventName, string sControlName, string sFormName)
        {
            StreamWriter log;
            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }
            // Write to the file:
            log.WriteLine("===============================================Srart============================================");
            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("--------------");
            //log.WriteLine("Exception Name:" + sExceptionName);
            log.WriteLine("Event Name:" + sEventName);
            log.WriteLine("---------------");
            log.WriteLine("Control Name:" + sControlName);
            log.WriteLine("---------------");
            log.WriteLine("Form Name:" + sFormName);
            log.WriteLine("===============================================End==============================================");
            // Close the stream:
            log.Close();
        }


        //Automation FFU//////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Ini file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "ConfigFile.ini";
            Process.Start(filePath);
        }



        /// <summary>
        /// automation ffu start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //section1
        string s_device; //device number
        string s_Current; //current fw
        string s_Target1; //target1 fw
        string s_Target2; //target2 fw
        //section2
        string f2_Target1;
        string f2_Target2;
        string f2_Target3; //target3 fw
        string f2_Target4; //target4 fw
        string f2_Target5; //target5 fw
        //section3
        string f2_Target1_Ven2;
        string f2_Target2_Ven2;
        string f2_Target3_Ven2; //target3 fw
        string f2_Target4_Ven2; //target4 fw
        string f2_Target5_Ven2; //target5 fw
        private void button8_Click(object sender, EventArgs e)
        {
            Test_LogFile("FFU Automation Test Start  ");
            try
            {
                label2.Text = ""; label2.Refresh();
                label4.Text = ""; label4.Refresh();
                label5.Text = ""; label5.Refresh();
                label6.Text = ""; label6.Refresh();
                richTextBox1.Text = "";
                comboBox1.Text = "";
                
                var dateOne = DateTime.Now;  //date time 
                label5.Visible = true; label5.BackColor = Color.Yellow; label5.Text = "Running";
                label5.Refresh();
                Application.DoEvents();
                ///////////////////////////////////////////////////////////////////////////////////////////               
                Check_ini_file_new(); //check if file exist if not create one  ..\Config\ConfigFile.ini file 
                FFU_Section1_read_new(); //read from ..\Config\ConfigFile.ini file and put into variables  
                File_value(); //put variables into network path-and get fluf file (run all configurations) , need to add to vulcan configurations
                ///////////////////////////////////////////////////////////////////////////////////////////
                //check if have ping to fw versions  ..\Config\network_Ip.txt (10.0.56.14)
                if (Ping_check() == 1)
                {
                    Test_LogFile("Ping_file: " + "Ping_ip= " + Network);
                    Delete_Temp_files();    //delete ..\Config\Python\latestfile.txt before the test start 
                    //###############################################################
                    //##Test Run#####################################################  
                    Automatic_ffu_test_run(); //FFU Run sec 1+2+3 also show colors in richtextbox1 
                    //#######################################################
                    //#######################################################

                }
                else { MessageBox.Show("Network Ip: "+ Network + " - Error"); }

               // Delete_Temp_files2();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// Automatic_ffu_test_run();
        /// </summary>
        public void Automatic_ffu_test_run()
        {
            try
            {
                Test_LogFile("Automatic_ffu_test_run() Function  ");
                string read_1;  //
                string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Latest_File.bat";
                string Latest_File_txt = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Latest_File.txt";
                string Tests_Num = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "Tests_Num.txt";
                string fw_activated_passed = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_passed.txt";
                string fw_activated_failed = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_failed.txt";
                string if_device_found = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "if_device_found.txt";
                string fw_error = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_error.txt";


                var dateOne = DateTime.Now;  //date time 
                label5.Visible = true; label5.BackColor = Color.Yellow; label5.Text = "Running";
                label5.Refresh();

                //Check_ini_file(); // check if ini file exist ,if not create one with default parameters    
                //FFU_Section1_read();
                //FFU_Section2_read();
                //FFU_Section3_read();
                //FFU_Section4_read();
                //Check_ini_file_new();
                //FFU_Section1_read_new();
                //File_value();

                if (Device.ToString() == "") { MessageBox.Show("Enter Device Value(drive1,drive2,..) in [Section1]"); return; }
                else
                {
                    Test_LogFile("Automatic_ffu_test_run()" + " , Disk Number: " + Device.ToString());
                    if (Network.ToString() == "") { MessageBox.Show("Enter Network"); return; } //check target1-must
                    if (Project.ToString() == "") { MessageBox.Show("Enter Project"); return; } // check current-must 
                    if (Key.ToString() == "") { MessageBox.Show("Enter Key"); return; } //check target1-must
                    if (Device.ToString() == "") { MessageBox.Show("Enter drive"); return; } //check target1-must
                    if (Base_FW.ToString() == "") { MessageBox.Show("Enter Base FW"); return; } //check target1-must
                    if (New_FW1.ToString() == "") { MessageBox.Show("Enter Upgrade FW"); return; } //check target1-must
                    if (Project.ToString() == "Vulcan")
                      {
                        if (Vulcan_Version.ToString() == "") { MessageBox.Show("Enter Vulcan_Version "); return; }
                      } 
                    Test_LogFile("Automatic_ffu_test_run()" + "\n" + "Base FW: " + Base_FW + "\n" + "New FW1: " + New_FW1);

                    //FFU_Run(); //run the commands function
                    //Get_fluf(BaseDirectory1);
                    if (Key == "customer")
                    {
                        //Non Sed
                        FFU_Run_new_ckey_1(); // Customer to base 
                        FFU_Run_new_ckey_2(); // customer to fw1
                        //Sed
                        FFU_Run_new_ckey_SED_1(); // SED Customer to base 
                        FFU_Run_new_ckey_SED_2(); // SED Customer to fw1
                    }
                    else if (Key == "EKey")
                    {
                        //FFU_Run_new(); buffer is full if add new test case test will stuck 
                        FFU_Run_new(); //run section 1+2 without MSFT FW1
                        FFU_Run_new_loop1(); //run 4 test cases, repeat test case fw1-base - base-fw1 5 times  
                        FFU_Run_new_2(); //RUN MSFT FW1 + run section 3 Without dell fw2 + msft fw2 
                        FFU_Run_new_3(); //run continue section 3 - >  dell fw 2 + msft fw2 
                    }
                    else if (Key == "customer_Pre_RDT")
                    {
                        //Non Sed
                        FFU_Run_new_ckey_Pre_RDT_3();
                        FFU_Run_new_ckey_Pre_RDT_2();
                        FFU_Run_new_ckey_Pre_RDT_1();

                        //Sed
                        FFU_Run_new_ckey_Pre_RDT_1_sed();
                        FFU_Run_new_ckey_Pre_RDT_2_sed();
                    }

                    else if (Key == "Debug")
                    {
                        //Non Sed
                        if (Vulcan_Version == "Debug_Version")
                        {
                            FFU_Run_new_Debug();
                        }
                        //Sed
                        if (Vulcan_Version == "SED_Debug_Version")
                        {
                            FFU_Run_new_Debug_SED();
                        }



                    }
                    Writelog(); //write log 
                    //result_and_show(); //show summery of test 

                    if (File.Exists(filePath)) //if file not exist   //if (!Directory.Exists(basePath))
                    {
                        Run_bat_file(); // run batch file ..\Config\Python\Latest_File.bat ->create txt file Latest_File.txt
                        Check_Result(); // run python scan file and html create  

                        //take latest file and open RichTextBox func to color the results
                        Read_txt_file(Latest_File_txt); //read file 
                        if (Read_txt_file(Latest_File_txt) != null) //if the contact of the file !=null enter  
                        {
                            read_1 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + Read_txt_file(Latest_File_txt) + ".html";//the new html file
                            RichTextBox_result(read_1); // show file in richbox1 with the colors inside the function
                        }
                    }
                }

 
                //update labels with the result/colors +test summery in the end  of the show Richtextbox /////////////////////////////////////
                //######################Run all batch files in folder output###############################
                Run_bat_file_result(); //check if test pass/fail/no device found/test numers by batch files run
                                       //#############################################################
                //richTextBox1.AppendText("\n");

                //richTextBox1.AppendText("####################################################" + "\n");

                label5.Visible = false;
                Application.DoEvents();

                string result1_TestsSummary = "#Test Summary:";
                string result1_Device = "Device:";
                string result1_Test_Duration = "Test Duration:";
                string result1_Test_cases_num = "Test Cases Number:";

                string result1_TestsPass = "Tests Pass:";
                string result1_TestsFail = "Tests Fail:";
                string result1_separate = "####################################################";
                string result1_test_start = "Test Start:";
                string result1_error_summary = "Tests Error:";
                string result1_warning = "Warning:";
                //string result1_error = "Error";
                //result1_Device = "Device:";


                //Test passed / fail / ERROR 
                #region
                
                //Test Cases Number:
                //######################Test summery + label update###############################
                int index_1 = 0;
                richTextBox1.AppendText("#Test Summary:" + "\n");
                if (File.Exists(if_device_found))
                {
                    Read_txt_file(if_device_found);
                    if (Read_txt_file(if_device_found) != "")
                    {
                        if (Read_txt_file(if_device_found) == "No Device Found")
                        {
                            Read_txt_file(Tests_Num);
                            if (Read_txt_file(Tests_Num) != null)
                            {
                                //label5.Text = "";
                                //label5.Visible = false;
                                //label5.Refresh();
                                //Application.DoEvents();
                                //Test Cases Number:
                                richTextBox1.AppendText("Test Cases Number: " + Read_txt_file(Tests_Num) + "\n");

                                //label4.Visible = true; label4.BackColor = Color.Turquoise; label4.Text = "Test Cases Number:" + Read_txt_file(Tests_Num);
                                //label4.Refresh();
                                //Not Found
                                richTextBox1.AppendText(Device + " : Not Found" + "\n");
                                label5.Visible = true; label5.BackColor = Color.Red; label5.Text = Device + " : Not Found";
                                label5.Refresh();
                                richTextBox1.AppendText("####################################################" + "\n");
                                return; //exit from the program if no device found 
                            }
                        }
                        else if (Read_txt_file(if_device_found) == "Device Found")
                        {
                            richTextBox1.AppendText("Device: " + Device + " Exist" + "\n");
                            richTextBox1.AppendText("Test Cases Number: " + Read_txt_file(Tests_Num) + "\n");
                        }
                    }
                }//if (File.Exists(if_device_found))


                /////////////////////////////////////////////////////////////////
                //1 read bat file to check how much test passed 
                if (File.Exists(Tests_Num))
                {
                    Read_txt_file(Tests_Num);
                    if (Read_txt_file(Tests_Num) != null)
                    {
                        //richTextBox1.AppendText("Test Cases Number: " + Read_txt_file(Tests_Num) + "\n");
                        //label4.Visible = true; label4.BackColor = Color.Turquoise; label4.Text = "Test Cases Number:" + Read_txt_file(Tests_Num);
                        //label4.Refresh();
                    }
                }
                //2 read bat file to check how much test passed 
                if (File.Exists(fw_activated_passed))
                {
                    Read_txt_file(fw_activated_passed);
                    //label5.Text = "";
                    //label5.Visible = false;
                    //label5.Refresh();
                    //Application.DoEvents();
                    if (Read_txt_file(fw_activated_passed) != null)
                    {
                        richTextBox1.AppendText("Tests Pass: " + Read_txt_file(fw_activated_passed) + "\n");
                        if (Read_txt_file(fw_activated_passed) == Read_txt_file(Tests_Num))
                        {
                            label5.Visible = true; label5.BackColor = Color.PaleGreen;
                            label5.Text = "Pass:" + Read_txt_file(fw_activated_passed);
                            label5.Refresh();
                        }
                        else if (Read_txt_file(fw_activated_passed) == "0")
                        {
                            label5.Visible = true; label5.BackColor = Color.PaleGreen;
                            label5.Text = "Pass:" + Read_txt_file(fw_activated_passed);
                            label5.Refresh();
                        }
                        else
                        {
                            label5.Visible = true; label5.BackColor = Color.PaleGreen;
                            label5.Text = "Pass:" + Read_txt_file(fw_activated_passed);
                            label5.Refresh();
                        }
                    }
                }

                //3 read bat file to check how much test failed 
                if (File.Exists(fw_activated_failed))
                {
                    Read_txt_file(fw_activated_failed);
                    if (Read_txt_file(fw_activated_failed) != null)
                    {
                        richTextBox1.AppendText("Tests Fail: " + Read_txt_file(fw_activated_failed) + "\n");
                        if (Read_txt_file(fw_activated_failed) == Read_txt_file(Tests_Num))
                        {
                            label6.Visible = true; label6.BackColor = Color.Red;
                            label6.Text = "Fail:" + Read_txt_file(fw_activated_failed);
                            label6.Refresh();
                        }
                        else if (Read_txt_file(fw_activated_failed) == "0")
                        {
                            label6.Visible = true; label6.BackColor = Color.Red;
                            label6.Text = "Fail:" + Read_txt_file(fw_activated_failed);
                            label6.Refresh();
                        }
                        else
                        {
                            label6.Visible = true; label6.BackColor = Color.Red;
                            label6.Text = "Fail:" + Read_txt_file(fw_activated_failed);
                            label6.Refresh();
                        }
                    }
                }
                //4 read batch file to check if have error (if_error.bat)
                if (File.Exists(fw_error))
                {
                    Read_txt_file(fw_error);
                    if (Read_txt_file(fw_error) != null)
                    {

                        richTextBox1.AppendText("Warning: " + Read_txt_file(fw_error) + "\n");

                        if (Read_txt_file(fw_error) == Read_txt_file(Tests_Num))
                        {
                            label4.Visible = true; label4.BackColor = Color.LightSalmon;
                            label4.Text = "Warning:" + Read_txt_file(fw_error);
                            label4.Refresh();
                        }
                        else if (Read_txt_file(fw_activated_failed) == "0")
                        {
                            label4.Visible = true; label4.BackColor = Color.LightSalmon;
                            label4.Text = "Warning:" + Read_txt_file(fw_error);
                            label4.Refresh();
                        }
                        else
                        {
                            label4.Visible = true;label4.BackColor = Color.LightSalmon; 
                            label4.Text = "Warning:" + Read_txt_file(fw_error);
                            label4.Refresh();
                        }

                    }
                }
                #endregion
                //######################################## richTextBox1 and label colors  #######################################
                //###############################################################################################################
                index_1 = 0;
                //result1_test_start = "Test Start:";
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_test_start))
                {
                    richTextBox1.Find(result1_test_start, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Turquoise;
                    richTextBox1.SelectionFont = new Font("Tahoma", 12, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_test_start, index_1) + 1;
                }

                //Tests Summary: colors richtextbox1
                //result1_TestsSummary = "Tests Summary: ";
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsSummary))
                {
                    richTextBox1.Find(result1_TestsSummary, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Blue;
                    richTextBox1.SelectionFont = new Font("Tahoma", 16, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_TestsSummary, index_1) + 1;
                }
                //result1_Device = "Device:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_Device))
                {
                    richTextBox1.Find(result1_Device, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_Device, index_1) + 1;
                }
                //result1_Test_Duration = "Test Duration:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_Test_Duration))
                {
                    richTextBox1.Find(result1_Test_Duration, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_Test_Duration, index_1) + 1;
                }
                //result1_Test_cases_num = "Test Cases Number:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_Test_cases_num))
                {
                    richTextBox1.Find(result1_Test_cases_num, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_Test_cases_num, index_1) + 1;
                }

                //result1_TestsPass = "Tests Pass:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsPass))
                {
                    richTextBox1.Find(result1_TestsPass, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_TestsPass, index_1) + 1;
                }
                //result1_TestsFail = "Tests Fail:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsFail))
                {
                    richTextBox1.Find(result1_TestsFail, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_TestsFail, index_1) + 1;
                }
                //result1_TestsFail = "Tests error:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_error_summary))
                {
                    richTextBox1.Find(result1_error_summary, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_error_summary, index_1) + 1;
                }
                //result1_warning = "Warning:";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_warning))
                {
                    richTextBox1.Find(result1_warning, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_warning, index_1) + 1;
                }
                //result1_separate = "####################################################";
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_separate))
                {
                    richTextBox1.Find(result1_separate, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.MediumTurquoise;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_separate, index_1) + 1;
                }


                //#################################################################################################################
                //string result1_separate = "####################################################";
                ///string result1_Test_Duration = "Test Duration:";

                var dateOne_end = DateTime.Now;
                richTextBox1.AppendText("Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss")));
                Test_LogFile("Test Done" + " , Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss")));
                richTextBox1.AppendText("\n" + "\n");
                //richTextBox1.AppendText("\n" + "####################################################" + "\n");

                //#####string result1_separate = "####################################################";(color )
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_separate))
                {
                    richTextBox1.Find(result1_separate, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.MediumTurquoise;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_separate, index_1) + 1;
                }

                //result1_Test_Duration = "Test Duration:";  (color )
                index_1 = 0;
                while (index_1 < richTextBox1.Text.LastIndexOf(result1_Test_Duration))
                {
                    richTextBox1.Find(result1_Test_Duration, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index_1 = richTextBox1.Text.IndexOf(result1_Test_Duration, index_1) + 1;
                }
                ////result1_error = "Error";
                //index_1 = 0;
                //while (index_1 < richTextBox1.Text.LastIndexOf(result1_error))
                //{
                //    richTextBox1.Find(result1_error, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                //    richTextBox1.SelectionColor = Color.Red;
                //    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                //    index_1 = richTextBox1.Text.IndexOf(result1_error, index_1) + 1;
                //}

                //run batch file MoveFiles_Automation to copy files to network
                string path = Directory.GetCurrentDirectory();
                string directory = path + @"\Config\Python\";
                string filter1 = @"\Config\Python\MoveFiles_Automation.bat";
                Run_bat_file_new(directory, filter1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Button:" + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void result_and_show()
        {
            Test_LogFile("result_and_show() Function  ");
            //string read_1;  //
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Latest_File.bat";
            string Latest_File_txt = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Latest_File.txt";
            string Tests_Num = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "Tests_Num.txt";
            string fw_activated_passed = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_passed.txt";
            string fw_activated_failed = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_failed.txt";
            string if_device_found = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "if_device_found.txt";
            string fw_error = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_error.txt";


            //RichTextBox control is an advanced text box that provides text editing and advanced formatting features
            //richTextBox1.Text = output1; //output1 varible all the test runs from func FFU_Run_new(); put into richTextBox1.Text
            //richTextBox1.AppendText(output2); //output2 varible all the test runs from func FFU_Run_new(); append into richTextBox1.Text

            //if (File.Exists(filePath)) //if file not exist   //if (!Directory.Exists(basePath))
            //{
            //    Run_bat_file(); // run batch file ..\Config\Python\Latest_File.bat ->create txt file Latest_File.txt
            //    Check_Result(); // run python scan file and html create  

            //    //take latest file and open RichTextBox func to color the results
            //    Read_txt_file(Latest_File_txt); //read file 
            //    if (Read_txt_file(Latest_File_txt) != null) //if the contact of the file !=null enter  
            //    {
            //        read_1 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + Read_txt_file(Latest_File_txt) + ".html";//the new html file
            //        RichTextBox_result(read_1); // show file in richbox1 with the colors inside the function
            //    }
            //}

            //update labels with the result/colors +test summery in the end  of the show Richtextbox /////////////////////////////////////
            //######################Run all batch files in folder output###############################
            Run_bat_file_result(); //check if test pass/fail/no device found/test numers by batch files run
            //#############################################################
            richTextBox1.AppendText("\n");

            //richTextBox1.AppendText("####################################################" + "\n");

            label5.Visible = false;
            Application.DoEvents();

            string result1_TestsSummary = "#Test Summary:";
            string result1_Device = "Device:";
            string result1_Test_Duration = "Test Duration:";
            string result1_Test_cases_num = "Test Cases Number:";

            string result1_TestsPass = "Tests Pass:";
            string result1_TestsFail = "Tests Fail:";
            string result1_separate = "####################################################";
            string result1_test_start = "Test Start:";
            string result1_error_summary = "Tests Error:";
            string result1_warning = "Warning:";
            //string result1_error = "Error";
            //result1_Device = "Device:";

           

            //Test Cases Number:
            //######################Test summery + label update###############################
            int index_1 = 0;
            richTextBox1.AppendText("#Test Summary:" + "\n");
            if (File.Exists(if_device_found))
            {
                Read_txt_file(if_device_found);
                if (Read_txt_file(if_device_found) != "")
                {
                    if (Read_txt_file(if_device_found) == "No Device Found")
                    {
                        Read_txt_file(Tests_Num);
                        if (Read_txt_file(Tests_Num) != null)
                        {
                            //label5.Text = "";
                            //label5.Visible = false;
                            //label5.Refresh();
                            //Application.DoEvents();
                            //Test Cases Number:
                            richTextBox1.AppendText("Test Cases Number: " + Read_txt_file(Tests_Num) + "\n");

                            //label4.Visible = true; label4.BackColor = Color.Turquoise; label4.Text = "Test Cases Number:" + Read_txt_file(Tests_Num);
                            //label4.Refresh();
                            //Not Found
                            richTextBox1.AppendText(Device + " : Not Found" + "\n");
                            label5.Visible = true; label5.BackColor = Color.Red; label5.Text = Device + " : Not Found";
                            label5.Refresh();
                            richTextBox1.AppendText("####################################################" + "\n");
                            return; //exit from the program if no device found 
                        }
                    }
                    else if (Read_txt_file(if_device_found) == "Device Found")
                    {
                        richTextBox1.AppendText("Device: " + Device + " Exist" + "\n");
                        richTextBox1.AppendText("Test Cases Number: " + Read_txt_file(Tests_Num) + "\n");
                    }
                }
            }//if (File.Exists(if_device_found))


            /////////////////////////////////////////////////////////////////
            //1 read bat file to check how much test passed 
            if (File.Exists(Tests_Num))
            {
                Read_txt_file(Tests_Num);
                if (Read_txt_file(Tests_Num) != null)
                {
                    //richTextBox1.AppendText("Test Cases Number: " + Read_txt_file(Tests_Num) + "\n");
                    //label4.Visible = true; label4.BackColor = Color.Turquoise; label4.Text = "Test Cases Number:" + Read_txt_file(Tests_Num);
                    //label4.Refresh();
                }
            }
            //2 read bat file to check how much test passed 
            if (File.Exists(fw_activated_passed))
            {
                Read_txt_file(fw_activated_passed);
                //label5.Text = "";
                //label5.Visible = false;
                //label5.Refresh();
                //Application.DoEvents();
                if (Read_txt_file(fw_activated_passed) != null)
                {
                    richTextBox1.AppendText("Tests Pass: " + Read_txt_file(fw_activated_passed) + "\n");
                    if (Read_txt_file(fw_activated_passed) == Read_txt_file(Tests_Num))
                    {
                        label5.Visible = true; label5.BackColor = Color.PaleGreen;
                        label5.Text = "Pass:" + Read_txt_file(fw_activated_passed);
                        label5.Refresh();
                    }
                    else if (Read_txt_file(fw_activated_passed) == "0")
                    {
                        label5.Visible = true; label5.BackColor = Color.PaleGreen;
                        label5.Text = "Pass:" + Read_txt_file(fw_activated_passed);
                        label5.Refresh();
                    }
                    else
                    {
                        label5.Visible = true; label5.BackColor = Color.PaleGreen;
                        label5.Text = "Pass:" + Read_txt_file(fw_activated_passed);
                        label5.Refresh();
                    }
                }
            }

            //3 read bat file to check how much test failed 
            if (File.Exists(fw_activated_failed))
            {
                Read_txt_file(fw_activated_failed);
                if (Read_txt_file(fw_activated_failed) != null)
                {
                    richTextBox1.AppendText("Tests Fail: " + Read_txt_file(fw_activated_failed) + "\n");
                    if (Read_txt_file(fw_activated_failed) == Read_txt_file(Tests_Num))
                    {
                        label6.Visible = true; label6.BackColor = Color.Red;
                        label6.Text = "Fail:" + Read_txt_file(fw_activated_failed);
                        label6.Refresh();
                    }
                    else if (Read_txt_file(fw_activated_failed) == "0")
                    {
                        label6.Visible = true; label6.BackColor = Color.Red;
                        label6.Text = "Fail:" + Read_txt_file(fw_activated_failed);
                        label6.Refresh();
                    }
                    else
                    {
                        label6.Visible = true; label6.BackColor = Color.Red;
                        label6.Text = "Fail:" + Read_txt_file(fw_activated_failed);
                        label6.Refresh();
                    }
                }
            }
            //4 read batch file to check if have error (if_error.bat)
            if (File.Exists(fw_error))
            {
                Read_txt_file(fw_error);
                if (Read_txt_file(fw_error) != null)
                {

                    richTextBox1.AppendText("Warning: " + Read_txt_file(fw_error) + "\n");

                    if (Read_txt_file(fw_error) == Read_txt_file(Tests_Num))
                    {
                        label4.Visible = true; label4.BackColor = Color.LightSalmon;
                        label4.Text = "Warning:" + Read_txt_file(fw_error);
                        label4.Refresh();
                    }
                    else if (Read_txt_file(fw_activated_failed) == "0")
                    {
                        label4.Visible = true; label4.BackColor = Color.LightSalmon;
                        label4.Text = "Warning:" + Read_txt_file(fw_error);
                        label4.Refresh();
                    }
                    else
                    {
                        label4.Visible = true;
                        //label4.BackColor = Color.Yellow; 
                        label4.Text = "Warning:" + Read_txt_file(fw_error);
                        label4.Refresh();
                    }

                }
            }

            //var dateOne_end = DateTime.Now;
            //label3.Visible = true; label3.BackColor = Color.Green; label3.Text = "Test Done" + " , Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss"));
            //label3.Refresh();
            //richTextBox1.AppendText("Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss")));
            //Test_LogFile("Test Done" + " , Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss")));

            //richTextBox1.AppendText("\n" + "####################################################" + "\n");
            //######################################## End of the sammeray area #############################################
            //###############################################################################################################


            //######################################## richTextBox1 and label colors  #######################################
            //###############################################################################################################
            index_1 = 0;
            //result1_test_start = "Test Start:";
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_test_start))
            {
                richTextBox1.Find(result1_test_start, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Turquoise;
                richTextBox1.SelectionFont = new Font("Tahoma", 12, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_test_start, index_1) + 1;
            }

            //Tests Summary: colors richtextbox1
            //result1_TestsSummary = "Tests Summary: ";
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsSummary))
            {
                richTextBox1.Find(result1_TestsSummary, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.SelectionFont = new Font("Tahoma", 16, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_TestsSummary, index_1) + 1;
            }
            //result1_Device = "Device:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_Device))
            {
                richTextBox1.Find(result1_Device, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_Device, index_1) + 1;
            }
            //result1_Test_Duration = "Test Duration:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_Test_Duration))
            {
                richTextBox1.Find(result1_Test_Duration, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_Test_Duration, index_1) + 1;
            }
            //result1_Test_cases_num = "Test Cases Number:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_Test_cases_num))
            {
                richTextBox1.Find(result1_Test_cases_num, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_Test_cases_num, index_1) + 1;
            }

            //result1_TestsPass = "Tests Pass:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsPass))
            {
                richTextBox1.Find(result1_TestsPass, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_TestsPass, index_1) + 1;
            }
            //result1_TestsFail = "Tests Fail:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_TestsFail))
            {
                richTextBox1.Find(result1_TestsFail, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_TestsFail, index_1) + 1;
            }
            //result1_TestsFail = "Tests error:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_error_summary))
            {
                richTextBox1.Find(result1_error_summary, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_error_summary, index_1) + 1;
            }
            //result1_warning = "Warning:";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_warning))
            {
                richTextBox1.Find(result1_warning, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_warning, index_1) + 1;
            }
            //result1_separate = "####################################################";
            index_1 = 0;
            while (index_1 < richTextBox1.Text.LastIndexOf(result1_separate))
            {
                richTextBox1.Find(result1_separate, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.MediumTurquoise;
                richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                index_1 = richTextBox1.Text.IndexOf(result1_separate, index_1) + 1;
            }

            ////result1_error = "Error";
            //index_1 = 0;
            //while (index_1 < richTextBox1.Text.LastIndexOf(result1_error))
            //{
            //    richTextBox1.Find(result1_error, index_1, richTextBox1.TextLength, RichTextBoxFinds.None);
            //    richTextBox1.SelectionColor = Color.Red;
            //    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
            //    index_1 = richTextBox1.Text.IndexOf(result1_error, index_1) + 1;
            //}

            //run batch file MoveFiles_Automation to copy files to network
            string path = Directory.GetCurrentDirectory();
            string directory = path + @"\Config\Python\";
            string filter1 = @"\Config\Python\MoveFiles_Automation.bat";
            Run_bat_file_new(directory, filter1);


        }






        /// <summary>
        /// Read_txt_file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// 
        public string Read_txt_file(string file)
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\";
            if (Directory.Exists(basePath1))
            {
                if (File.Exists(file))
                {
                    //Pass the file path and file name to the StreamReader 
                    //read from the text file 
                    String line;
                    StreamReader sr = new StreamReader(file);
                    //Read the first line of text
                    line = sr.ReadLine();
                    //read 
                    if (line != null)
                    {
                        //MessageBox.Show(line);
                        return line;
                    }
                    //else { MessageBox.Show("File Empty"); }
                    sr.Close();
                }
                //else { MessageBox.Show("File Not Exist: "+ file); }
            }
            else
            {
               // MessageBox.Show("Directory Not Exist:  " + basePath1);
                Test_LogFile("Read_txt_file(string file)- Directory Not Exist:  " + basePath1);
            }
            Test_LogFile("Read_txt_file(string file):  " + file);
            return null;
        }



        /// <summary>
        /// check \\setting\Configfile.ini if exist if not create one 
        /// </summary>
        public void Check_ini_file()
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config";
            string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "ConfigFile.ini";// open this file

            //IniFile ini = new IniFile(basePath + "\\" + "ConfigFile.ini");
            IniFile ini = new IniFile(Path.Combine(basePath1, "ConfigFile.ini"));
            if (!File.Exists(filePath1)) //if file not exist   //if (!Directory.Exists(basePath))
            {//if file not exist create one with this defult values
                Directory.CreateDirectory(basePath1);
                ini.IniWriteValue("Section1", "Device", "'disk2'");
                ini.IniWriteValue("Section1", "Currnet", "'2'");
                ini.IniWriteValue("Section1", "Vendor1", "'1'");
                ini.IniWriteValue("Section1", "Vendor2", "'1'" + "\n");

                ini.IniWriteValue("Section2", "Vendor1_A", "'GOEM'");
                ini.IniWriteValue("Section2", "Vendor1_B", "'DELL'");
                ini.IniWriteValue("Section2", "Vendor1_C", "'LENOVO'");
                ini.IniWriteValue("Section2", "Vendor1_D", "'HP'");
                ini.IniWriteValue("Section2", "Vendor1_E", "'MSFT'" + "\n");

                ini.IniWriteValue("Section3", "Vendor2_A", "'GOEM'");
                ini.IniWriteValue("Section3", "Vendor2_B", "'DELL'");
                ini.IniWriteValue("Section3", "Vendor2_C", "'LENOVO'");
                ini.IniWriteValue("Section3", "Vendor2_D", "'HP'");
                ini.IniWriteValue("Section3", "Vendor2_E", "'MSFT'" + "\n");

                //ini.IniWriteValue("Section4", "Device", "'disk2'");
                //ini.IniWriteValue("Section4", "Currnet", "'2'"); 
                //ini.IniWriteValue("Section4", "Vendor1", "'1'");
                //ini.IniWriteValue("Section4", "Vendor2", "'1'" + "\n");

                //ini.IniWriteValue("Section5", "Vendor1_A", "'GOEM'");
                //ini.IniWriteValue("Section5", "Vendor1_B", "'DELL'");
                //ini.IniWriteValue("Section5", "Vendor1_C", "'LENOVO'");
                //ini.IniWriteValue("Section5", "Vendor1_D", "'HP'");
                //ini.IniWriteValue("Section5", "Vendor1_E", "'MSFT'" + "\n");

                //ini.IniWriteValue("Section6", "Vendor2_A", "'GOEM'");
                //ini.IniWriteValue("Section6", "Vendor2_B", "'DELL'");
                //ini.IniWriteValue("Section6", "Vendor2_C", "'LENOVO'");
                //ini.IniWriteValue("Section6", "Vendor2_D", "'HP'");
                //ini.IniWriteValue("Section6", "Vendor2_E", "'MSFT'");
                //MessageBox.Show("New INI File Created in ../setting/ConfigFile.ini");
                Test_LogFile("Check_ini_file()-New INI File Created in ../setting/ConfigFile.ini");
            }
            Test_LogFile("Check_ini_file()");
        }



        /// <summary>
        /// Check_ini_file_new()
        /// </summary>
        public void Check_ini_file_new()
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config";
            string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "ConfigFile.ini";// open this file

            //IniFile ini = new IniFile(basePath + "\\" + "ConfigFile.ini");
            IniFile ini = new IniFile(Path.Combine(basePath1, "ConfigFile.ini"));
            if (!File.Exists(filePath1)) //if file not exist   //if (!Directory.Exists(basePath))
            {//if file not exist create one with this defult values
                Directory.CreateDirectory(basePath1);
                ini.IniWriteValue("Section1", "Network", "'10.0.56.14'");
                ini.IniWriteValue("Section1", "Project", "'calypso_perf'");
                ini.IniWriteValue("Section1", "Key", "'customer'");
                ini.IniWriteValue("Section1", "Device", "'disk*'");
                ini.IniWriteValue("Section1", "Base_FW", "'AO0ffC2P'");
                ini.IniWriteValue("Section1", "Upgrade_FW", "'AO0ffC2P'");
                ini.IniWriteValue("Section1", "Downgrade_FW", "'AO0ffC2P'");
                ini.IniWriteValue("Section1", "Vulcan_Version", "'Debug_Version'");
                //ini.IniWriteValue("Section1", "Vulcan_Sed", "'no'");
                //
                //MessageBox.Show("New INI File Created in ../setting/ConfigFile.ini");
                Test_LogFile("Check_ini_file()-New INI File Created in ../Config/ConfigFile.ini");
            }
            Test_LogFile("Check_ini_file()"+" ,File Path: "+ filePath1);
        }



        /// <summary>
        /// initialise FFU func before run the commands 
        /// </summary>
        /// 
        string Network;
        string Project;
        string Key;
        string Device;
        string drive;
        string Base_FW;
        string New_FW1;
        string New_FW2;
        string Vulcan_Version;
        //string Vulcan_sed;
        public void FFU_Section1_read_new()
        {
            try
            {
                Network = ReadConfig_new("Section1", "Network"); // go to function and get from [Section1]-Network value
                Project = ReadConfig_new("Section1", "Project");// go to function and get from [Section1]-Project value
                Key = ReadConfig_new("Section1", "Key");// go to function and get from [Section1]-Key value
                drive = ReadConfig_new("Section1", "Device");// go to function and get from [Section1]-Device value
                Base_FW = ReadConfig_new("Section1", "Base_FW");// go to function and get from [Section1]-Base_FW value
                New_FW1 = ReadConfig_new("Section1", "Upgrade_FW");// go to function and get from [Section1]-Upgrade_FW value
                New_FW2 = ReadConfig_new("Section1", "Downgrade_FW");// go to function and get from [Section1]-Downgrade_FW value 
                Vulcan_Version = ReadConfig_new("Section1", "Vulcan_Version");// go to function and get from [Section1]-Vulcan_Version value 
                //Vulcan_sed = ReadConfig_new("Section1", "Vulcan_Sed");// go to function and get from [Section1]-sed value
                //
                //ini file take value drive0/drive1 and change it to disk0/disk1 to work with wdckit 
                if (drive == "drive0") { Device = "disk0"; }
                else if (drive == "drive1") { Device = "disk1"; }
                else if (drive == "drive2") { Device = "disk2"; }
                else if (drive == "drive3") { Device = "disk3"; }
                else if (drive == "drive4") { Device = "disk4"; }
                else if (drive == "drive5") { Device = "disk5"; }
                else if (drive == "drive6") { Device = "disk6"; }
                else if (drive == "drive7") { Device = "disk7"; }
                else { Device = ""; }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }

            Test_LogFile("###############" + "\n"+"FFU_Section1_read_new()" + "\n"+"Network: " +  Network + "\n" + "Project: " + Project + "\n" + "Key: " + Key + "\n" + "Device: " + Device + "\n" + "Base_FW: " + Base_FW + "\n" + "New_FW1: " + New_FW1 + "\n" + "New_FW2: " + New_FW2 + "\n" + "###############");
      
        }


        /// <summary>
        /// initialise FFU func before run the commands 
        /// </summary>
        /// 
        public void FFU_Section1_read()
        {
            try
            {
                s_device = ReadConfig("Section1", "Device"); // go to function and get from [Section1]-Device value
                s_Current = ReadConfig("Section1", "Current");// go to function and get from [Section1]-Current value
                s_Target1 = ReadConfig("Section1", "Vendor1");// go to function and get from [Section1]-Target1 value
                s_Target2 = ReadConfig("Section1", "Vendor2");// go to function and get from [Section1]-Target2 value              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
            Test_LogFile("FFU_Section1_read()");
        }


        /// <summary>
        /// initialise FFU func before run the commands 
        /// </summary>
        /// 
        public void FFU_Section2_read()
        {
            try
            {
                s_device = ReadConfig("Section1", "Device");
                s_Current = ReadConfig("Section1", "Current");//go to function and get from [Section2]
                f2_Target1 = ReadConfig("Section2", "Vendor1_A");// go to function and get from [Section2]
                f2_Target2 = ReadConfig("Section2", "Vendor1_B");// go to function and get from [Section2]
                f2_Target3 = ReadConfig("Section2", "Vendor1_C");// go to function and get from [Section2]
                f2_Target4 = ReadConfig("Section2", "Vendor1_D");// go to function and get from [Section2]
                f2_Target5 = ReadConfig("Section2", "Vendor1_E");// go to function and get from [Section2]                
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
            Test_LogFile("FFU_Section2_read()");
        }



        /// <summary>
        /// initialise FFU func before run the commands 
        /// </summary>
        /// 
        public void FFU_Section3_read()
        {
            try
            {
                s_device = ReadConfig("Section1", "Device");
                s_Current = ReadConfig("Section1", "Current");//go to function and get from [Section2]
                f2_Target1_Ven2 = ReadConfig("Section3", "Vendor2_A");// go to function and get from [Section2]
                f2_Target2_Ven2 = ReadConfig("Section3", "Vendor2_B");// go to function and get from [Section2]
                f2_Target3_Ven2 = ReadConfig("Section3", "Vendor2_C");// go to function and get from [Section2]
                f2_Target4_Ven2 = ReadConfig("Section3", "Vendor2_D");// go to function and get from [Section2]
                f2_Target5_Ven2 = ReadConfig("Section3", "Vendor2_E");// go to function and get from [Section2]          
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
            Test_LogFile("FFU_Section3_read()");
        }


        /// <summary>
        /// ReadConfig function+ class InFile (read ini files)
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        public string ReadConfig(string section, string key)
        {
            string retVal = string.Empty;
            string bankname = string.Empty;
            string basePath = Environment.CurrentDirectory + "\\" + "Config";
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "ConfigFile.ini";// open this file
            IniFile ini = new IniFile(Path.Combine(basePath, "ConfigFile.ini")); //object from class(combine baspatte to configfile.ini)

            retVal = ini.IniReadValue(section, key); //go to class and read values,put the value in retval
            return retVal;
        }


        /// <summary>
        /// ReadConfig function+ class InFile (read ini files)
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        public string ReadConfig_new(string section, string key)
        {
            string retVal = string.Empty;
            string bankname = string.Empty;
            string basePath = Environment.CurrentDirectory + "\\" + "Config";
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "ConfigFile.ini";// open this file
            IniFile ini = new IniFile(Path.Combine(basePath, "ConfigFile.ini")); //object from class(combine baspatte to configfile.ini)

            retVal = ini.IniReadValue(section, key); //go to class and read values,put the value in retval
            return retVal;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////// AUTOMATION FFU Functions 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// FFU upgrade downgrade commands between FW's - old run with all test cases...  , here in one proccess can't run lot test cases 
        /// </summary>
        /// 
        public void FFU_Run()
        {
            try
            {
                Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process.RedirectStandardOutput = true;
                process.RedirectStandardInput = true;
                process.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process.CreateNoWindow = true;  //start the process in a new window.
                var proc = Process.Start(process);

                string dr1d;
                string dr2d;
                proc.StandardInput.WriteLine("-Current FW: " + s_Current);
                //////////////////////////////////////////////////Current to Current
                proc.StandardInput.WriteLine("***" + "Start Section 1 ");
                proc.StandardInput.WriteLine("***" + "FFU Between Fw's: ");
                proc.StandardInput.WriteLine("------------" + "Current to Current");
                if (File.Exists(s_Current.ToString()))
                { 
                    dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                    proc.StandardInput.WriteLine(dr1d);
                    dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                    proc.StandardInput.WriteLine(dr2d);
                }
                else { MessageBox.Show(s_Current.ToString()+ "error"); }
                ///////////////////////////////////////////////////current to Target 1
                if (File.Exists(s_Target1.ToString()))
                { 
                    proc.StandardInput.WriteLine("------------" + "Current FW to Vendor1 FW: ");
                    dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Target1);   //
                    proc.StandardInput.WriteLine(dr1d);
                    dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                    proc.StandardInput.WriteLine(dr2d);
                }
                /////////////////////////////////////////////////Target 1 to current 
                if (File.Exists(s_Current.ToString()))
                {
                    proc.StandardInput.WriteLine("------------" + "Vendor1 FW to Current FW: ");
                    dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                    proc.StandardInput.WriteLine(dr1d);
                    dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                    proc.StandardInput.WriteLine(dr2d);
                }

                ////////////////////////////////////////////////// 
                if (s_Target2.ToString() != "")
                {
                    if (File.Exists(s_Target2.ToString()))
                    {  
                    ///////////////////////////////////////////////////current to Target 2
                    proc.StandardInput.WriteLine("------------" + "Current FW to Vendor2 FW: ");
                    dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Target2);   //
                    proc.StandardInput.WriteLine(dr1d);
                    dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                    proc.StandardInput.WriteLine(dr2d);

                    /////////////////////////////////////////////////Target 2 to current 
                    proc.StandardInput.WriteLine("------------" + "Vendor2 FW to Current FW: ");
                    dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                    proc.StandardInput.WriteLine(dr1d);
                    dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                    proc.StandardInput.WriteLine(dr2d);

                    proc.StandardInput.WriteLine("***" + "End Section 1 ");
                    }
                    else { MessageBox.Show(s_Target2 + " Error"); }
                }
                //////////////////////////////
                //Section2- External to vendor 1(Same fw) //////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////
                // FFU Between Customers(GOEM,DELL,LENOVO,HP)+MSFT in same FW 
                Thread.Sleep(5000);
                if (s_device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                else
                {
                    //check if must or optional from section 2 ( target1-target5) 
                    //if (s_Current.ToString() == "") { MessageBox.Show("Fill your Current FW"); return; }
                    //if (f2_Target1.ToString() == "") { MessageBox.Show("Fill your Target 1 FW IN [Section2]"); }
                    //if (f2_Target2.ToString() == "") { MessageBox.Show("Fill your Target 2 FW");  }
                    //if (f2_Target3.ToString() == "") { MessageBox.Show("Fill your Target 3 FW");  }
                    //if (f2_Target4.ToString() == "") { MessageBox.Show("Fill your Target 4 FW");  }
                    //if (f2_Target5.ToString() == "") { MessageBox.Show("Fill your Target 4 FW");  }
                    if (File.Exists(f2_Target1) || (File.Exists(f2_Target2)) || (File.Exists(f2_Target3)) || (File.Exists(f2_Target4)) || (File.Exists(f2_Target5)))
                    {
                        proc.StandardInput.WriteLine("***" + "Start Section 2 ");
                        proc.StandardInput.WriteLine("***" + "FFU Between Customers - Ver to customer Vendor 1 ");
                    }
                    ///////////////////////////////////////////////////current Customer to Target 1
                    if (f2_Target1.ToString() != "")
                    {
                        if (File.Exists(f2_Target1.ToString()))
                        { 
                        proc.StandardInput.WriteLine("------------" + "Current Customer to Target1 Customer:  ");
                        dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target1);   //
                        proc.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                        proc.StandardInput.WriteLine(dr2d);

                        /////////////////////////////////////////////////Target 1 to current Customer
                        proc.StandardInput.WriteLine("------------" + "Target1 Customer to Current Customer: ");
                        dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                        proc.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                        proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    ////////////////////////////////////////////////// 
                    if (f2_Target2.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current Customer to Target 2
                        if (File.Exists(f2_Target2.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target2 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target2);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 2 to current Customer 
                            proc.StandardInput.WriteLine("------------" + "Target2 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (f2_Target3.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current Customer to Target 3
                        if (File.Exists(f2_Target3.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target3 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target3);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 3 to current Customer
                            proc.StandardInput.WriteLine("------------" + "Target 3 Customer  to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (f2_Target4.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current to Target 4
                        if (File.Exists(f2_Target4.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target4 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target4);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 4 to current Customer 
                            proc.StandardInput.WriteLine("------------" + "Target4 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (f2_Target5.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current to Target 5 Customer
                        if (File.Exists(f2_Target5.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target5 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target5);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 5 to current Customer
                            proc.StandardInput.WriteLine("------------" + "Target5 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (File.Exists(f2_Target1) || (File.Exists(f2_Target2)) || (File.Exists(f2_Target3)) || (File.Exists(f2_Target4)) || (File.Exists(f2_Target5)))
                    {
                        proc.StandardInput.WriteLine("***" + "End Section 2 ");
                    }
                    Thread.Sleep(5000);

                    //////////////////////////////
                    //Section3- current to External vendor 2 /////////////////////////////////////////////////////////////////////////////////////////////
                    /////////////////////////////
                    if (File.Exists(f2_Target1_Ven2) || (File.Exists(f2_Target2_Ven2)) || (File.Exists(f2_Target3_Ven2)) || (File.Exists(f2_Target4_Ven2)) || (File.Exists(f2_Target5_Ven2)))
                    {
                        proc.StandardInput.WriteLine("***" + "Start Section 3 ");
                        proc.StandardInput.WriteLine("***" + "FFU Between Customers Ver to customer Vendor 2 ");
                    }

                    ///////////////////////////////////////////////////current Customer to Target 1
                    if (f2_Target1_Ven2.ToString() != "")
                    {
                        if (File.Exists(f2_Target1_Ven2.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target1 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target1_Ven2);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 1 to current Customer
                            proc.StandardInput.WriteLine("------------" + "Target1 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    ////////////////////////////////////////////////// 
                    if (f2_Target2_Ven2.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current Customer to Target 2
                        if (File.Exists(f2_Target2_Ven2.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target2 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target2_Ven2);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 2 to current Customer 
                            proc.StandardInput.WriteLine("------------" + "Target2 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (f2_Target3_Ven2.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current Customer to Target 3
                        if (File.Exists(f2_Target3_Ven2.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target3 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target3_Ven2);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 3 to current Customer
                            proc.StandardInput.WriteLine("------------" + "Target 3 Customer  to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (f2_Target4_Ven2.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current to Target 4
                        if (File.Exists(f2_Target4_Ven2.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target4 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target4_Ven2);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 4 to current Customer 
                            proc.StandardInput.WriteLine("------------" + "Target4 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    if (f2_Target5_Ven2.ToString() != "")
                    {
                        ///////////////////////////////////////////////////current to Target 5 Customer
                        if (File.Exists(f2_Target5_Ven2.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Current Customer to Target5 Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + f2_Target5_Ven2);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);

                            /////////////////////////////////////////////////Target 5 to current Customer
                            proc.StandardInput.WriteLine("------------" + "Target5 Customer to Current Customer: ");
                            dr1d = ("wdckit.exe update " + s_device.ToString() + " -f " + s_Current);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + s_device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                        }
                    }
                    ////////////////////////////////////////////////////////////////////////////////////
                    if (File.Exists(f2_Target1_Ven2) || (File.Exists(f2_Target2_Ven2)) || (File.Exists(f2_Target3_Ven2)) || (File.Exists(f2_Target4_Ven2)) || (File.Exists(f2_Target5_Ven2)))
                    {
                        proc.StandardInput.WriteLine("End Section 3");
                    }
                    
                }//else

                //shoud be out the else to print the ffu 1 before the second func
                proc.StandardInput.WriteLine("FFU Done.");
                proc.StandardInput.WriteLine("exit");

                string s = proc.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = s;  //show in textbox1
                richTextBox1.Text = s;
                Test_LogFile("FFU_Run()  ################################ end ################################");
                Writelog(); //func 
                
            }//try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }

        /////////////// EKEY

        /// <summary>
        /// FFU_Run_new_2()
        /// </summary>
        /// 
        string output2;
        public void FFU_Run_new_2()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Section 3 Start ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                //if (Directory.Exists(BaseDirectory1)) { Get_fluf(BaseDirectory1); base_dir = return_fluf_check; } else { if_file_exist = "error"; }
                if (!Directory.Exists(BaseDirectory1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "Section 3 Error , Input Value Wrong "); }
                else
                {

                    Get_fluf(BaseDirectory1);  //send BaseDirectory1 value to get full fluf path func and return value to the var return_fluf_check
                    base_dir = return_fluf_check;  //put return_fluf_check after run the func Get_fluf(BaseDirectory1) to base_dir
                    //Code section 3 ///////////////////////////////////////////////////////////////////////////////////////////////       

                    if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
 
                    //MSFT FW-1 
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(FW1) - MSFT(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1);
                        ///////////////////////////////////////////////////Base to MSFT New_FW1
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to MSFT(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - Base to MSFT(FW1)");
                            /////////////////////////////////////////////////MSFT to Base New_FW1
                            proc1.StandardInput.WriteLine("------------" + "MSFT(FW1) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - MSFT(FW1) to Base");
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(FW1) - MSFT(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion

                    proc1.StandardInput.WriteLine("*** Section 3 Start ***");
                    //GO - FW 2
                    #region

                    proc1.StandardInput.WriteLine("***" + "FFU Between Base to NEW FW 2");
                    Thread.Sleep(5000);
                    if (!Directory.Exists(targetDirectory_GO_2)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_GO_2);

                        //if (targetDirectory_GO_2 != "")
                        //{
                        //    proc1.StandardInput.WriteLine("*** Section 3 Start ***");
                        //    proc1.StandardInput.WriteLine("***" + "FFU Between Base to NEW FW 2");
                        //}
                        //else { proc1.StandardInput.WriteLine(return_fluf_check + "------------" + "New_FW2 Error"); }
                        ///////////////////////////////////////////////////Base to GO New_FW2
                        // if (return_fluf_check.ToString() != "")
                        //{
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to GO(FW2): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                            /////////////////////////////////////////////////New_FW2 to Base
                            proc1.StandardInput.WriteLine("------------" + "GO(FW2) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 - GO(FW2) to Base" + " , Fw Name: " + base_dir);

                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    //}
                    #endregion
                    Thread.Sleep(3000);
                    //HP - FW 2 
                    #region
                    if (!Directory.Exists(targetDirectory_HP_2)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-HP(FW2) - HP(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_HP_2);

                        //if (return_fluf_check.ToString() != "")
                        //{
                        ///////////////////////////////////////////////////Base to HP New_FW2
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to HP(FW2): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 - Base to HP(FW2)" + " , Fw Name: " + return_fluf_check);

                            /////////////////////////////////////////////////HP to Base New_FW2 Error
                            proc1.StandardInput.WriteLine("------------" + " HP(FW2) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -  HP(FW2) to Base" + " , Fw Name: " + base_dir);

                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-HP(FW2) - HP(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //LE - FW 2 
                    #region
                    if (!Directory.Exists(targetDirectory_LE_2)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-LE(FW2) - LE(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_2);

                        //if (return_fluf_check.ToString() != "")
                        //{
                        /////////////////////////////////////////////////Base to LE New_FW2 
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to LE(FW2): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 - Base to LE(FW2) " + " , Fw Name: " + return_fluf_check);

                            ///////////////////////////////////////////////LE to Base New_FW2
                            proc1.StandardInput.WriteLine("------------" + "LE(FW2) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 - LE(FW2) to Base" + " , Fw Name: " + base_dir);

                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-LE(FW2) - LE(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //DELL - FW 2 
                    #region
                    //if (!Directory.Exists(targetDirectory_DE_2)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "Base to DE New_FW2 Error - DE to Base New_FW2 Error"); }
                    //else
                    //{
                    //    Get_fluf(targetDirectory_DE_2);
                    //    /////////////////////////////////////////////////Base to DE New_FW2
                    //    if (File.Exists(return_fluf_check.ToString()))
                    //    {
                    //        proc1.StandardInput.WriteLine("Base to DE New_FW2: ");
                    //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                    //        proc1.StandardInput.WriteLine(dr1d);
                    //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                    //        proc1.StandardInput.WriteLine(dr2d);
                    //        Test_LogFile("Code section 3 - Base to DE New_FW2" + " , Fw Name: " + return_fluf_check);
                    //        ///////////////////////////////////////////////DE to Base New_FW2
                    //        proc1.StandardInput.WriteLine("DE to Base New_FW2: ");
                    //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                    //        proc1.StandardInput.WriteLine(dr1d);
                    //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                    //        proc1.StandardInput.WriteLine(dr2d);
                    //        Test_LogFile("Code section 3 - DE to Base New_FW2" + " , Fw Name: " + base_dir);
                    //    }
                    //    else { proc1.StandardInput.WriteLine("------------" + "Base to DE New_FW2 Error & DE to Base New_FW2 Error"); }
                    //}

                    #endregion
                    Thread.Sleep(3000);
                    //MSFT - FW 2 
                    #region
                    //if (!Directory.Exists(targetDirectory_MSFT_2)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "Base to MSFT New_FW2 Error - MSFT to Base New_FW2 Error "); }
                    //else
                    //{
                    //    Get_fluf(targetDirectory_MSFT_2);
                    //    ///////////////////////////////////////////////////Base to MSFT New_FW2
                    //    if (File.Exists(return_fluf_check.ToString()))
                    //    {
                    //        proc1.StandardInput.WriteLine("------------" + "Base to MSFT New_FW2: ");
                    //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                    //        proc1.StandardInput.WriteLine(dr1d);
                    //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                    //        proc1.StandardInput.WriteLine(dr2d);
                    //        Test_LogFile("Code section 2 - Base to MSFT New_FW2");
                    //        /////////////////////////////////////////////////MSFT to Base New_FW2
                    //        proc1.StandardInput.WriteLine("------------" + "MSFT to Base New_FW2: ");
                    //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                    //        proc1.StandardInput.WriteLine(dr1d);
                    //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                    //        proc1.StandardInput.WriteLine(dr2d);
                    //        Test_LogFile("Code section 2 - MSFT to Base New_FW2");
                    //    }
                    //    else { proc1.StandardInput.WriteLine("------------" + "Base to MSFT New_FW2 Error & MSFT to Base New_FW2 Error "); }
                    //}
                    #endregion
                }
                //proc1.StandardInput.WriteLine("*** Section 3 Done ***");
                // proc1.StandardInput.WriteLine("FFU Completed Successfully.");
                    //} //else
                    //else { proc1.StandardInput.WriteLine("*** Section 3 Error ***"); }
                    proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 

                    string output = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                    //textBox1.Text = output;  //show in textbox1
                    output2 = output;
                    //textBox1.AppendText(output2);

                    proc1.WaitForExit();
                    proc1.Dispose();
                    proc1.Close();
                //Test_LogFile("FFU_Run_new()  ################################  Section 3 End ################################");
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU_Run_new_3()
        /// </summary>
        /// 
        string output3;
        string enter_and_line = "\n" + "------------------------------------------------------------------------------------------------------------------------------------------------------------------"; 
        public void FFU_Run_new_3()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Section 3 Start ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process3 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process3.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process3.RedirectStandardOutput = true;
                process3.RedirectStandardInput = true;
                process3.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process3.CreateNoWindow = true;  //start the process in a new window.

                var proc3 = Process.Start(process3);
                string dr1d;
                string dr2d;
                //proc3.StandardInput.WriteLine("*** Section 3 Start ***");
                //if (Directory.Exists(BaseDirectory1)) { Get_fluf(BaseDirectory1); base_dir = return_fluf_check; } else { if_file_exist = "error"; }
                if (!Directory.Exists(BaseDirectory1)) { if_file_exist = "error"; proc3.StandardInput.WriteLine("------------" + "Section 3 Error , Input Value Wrong "); }
                else
                {

                    Get_fluf(BaseDirectory1);  //send BaseDirectory1 value to get full fluf path func and return value to the var return_fluf_check
                    base_dir = return_fluf_check;  //put return_fluf_check after run the func Get_fluf(BaseDirectory1) to base_dir
                    //Code section 3 ///////////////////////////////////////////////////////////////////////////////////////////////       
                    if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                    //MSFT - FW 2 
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_2)) { if_file_exist = "error"; proc3.StandardInput.WriteLine("------------" + "BASE-MSFT(FW2) - MSFT(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_2);
                        ///////////////////////////////////////////////////Base to MSFT New_FW2
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc3.StandardInput.WriteLine("------------" + "Base to MSFT(FW2): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc3.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc3.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - Base to MSFT(FW2)");
                            /////////////////////////////////////////////////MSFT to Base New_FW2
                            proc3.StandardInput.WriteLine("------------" + "MSFT(FW2) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc3.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc3.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - MSFT(FW2) to Base");
                        }
                        else { proc3.StandardInput.WriteLine("------------" + "BASE-MSFT(FW2) - MSFT(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //DELL - FW 2 
                    #region
                    if (!Directory.Exists(targetDirectory_DE_2)) { if_file_exist = "error"; proc3.StandardInput.WriteLine("------------" + "BASE-DELL(FW2) - DELL(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_2);
                        /////////////////////////////////////////////////Base to DE New_FW2
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc3.StandardInput.WriteLine("------------" + "Base to DELL(FW2): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc3.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc3.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 - Base to DELL(FW2)" + " , Fw Name: " + return_fluf_check);
                            ///////////////////////////////////////////////DE to Base New_FW2
                            proc3.StandardInput.WriteLine("------------" + "DELL(FW2) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc3.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc3.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 - DELL(FW2) to Base" + " , Fw Name: " + base_dir);
                        }
                        else { proc3.StandardInput.WriteLine("------------" + "BASE-DELL(FW2) - DELL(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }

                    #endregion



                    //Sed -> Non sed
                    //SED_Performance_Version / SED_Debug_Version / SED_Performance_Version_Pre_RDT
                    //if (Vulcan_Version == "SED_Performance_Version" || Vulcan_Version == "SED_Debug_Version" || Vulcan_Version == "SED_Performance_Version_Pre_RDT")
                    //{
                    //    if (Vulcan_Version == "SED_Performance_Version")
                    //    {
                    //    }
                    //    if (Vulcan_Version == "SED_Debug_Version") { }
                    //    if (Vulcan_Version == "SED_Performance_Version_Pre_RDT") { }
                    //}

                    //Get_fluf(targetDirectory_DE_1);
                    //string source = @"\\iky-op-fpgcss01.wdc.com\fpgcss_ci\vulcan\Firmware\Releases\Test_Official_Builds\60ba7334\PETE_ZIP_Cust_PRE_RDT\73Z40001\vulcan_perf_pre_rdt\vulcan_perf_pre_rdt_BOT";
                    //string data = getBetween(source, "73Z40", "01");

                    //MessageBox.Show(data);

                }
                proc3.StandardInput.WriteLine("*** Section 3 Done ***");
                proc3.StandardInput.WriteLine("FFU Completed Successfully.");
                //} //else
                //else { proc1.StandardInput.WriteLine("*** Section 3 Error ***"); }
                proc3.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 

                string output_3 = proc3.StandardOutput.ReadToEnd();  //run commands show in textbox1
                                                                   //textBox1.Text = output;  //show in textbox1
                output3 = output_3;
                //textBox1.AppendText(output2);

                proc3.WaitForExit();
                proc3.Dispose();
                proc3.Close();
                Test_LogFile("FFU_Run_new()  ################################  Section 3 End ################################");
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU upgrade downgrade commands between FW's-new - loop test cases , repeat test case base ->fw1-fw1-base 5 times with for loop 
        /// </summary>
        /// 
        string output_loop1;
        public void FFU_Run_new_loop1()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  FFU_Run_new_loop1 ################################");
                ProcessStartInfo process4 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process4.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process4.RedirectStandardOutput = true;
                process4.RedirectStandardInput = true;
                process4.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process4.CreateNoWindow = true;  //start the process in a new window.
                //variables 
                var proc4 = Process.Start(process4);
                string dr1d;
                string dr2d;
                //Code section 1 ///////////////////////////////////////////////////////////////////////////////////////////////
                if (!Directory.Exists(BaseDirectory1)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "Section 1/2 Error , Input Value Wrong "); }
                else
                {

                    Get_fluf(BaseDirectory1);  //send BaseDirectory1 value to get full fluf path func and return value to the var return_fluf_check
                    base_dir = return_fluf_check;  //put return_fluf_check after run the func Get_fluf(BaseDirectory1) to base_dir
                    if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                   //current to Base_FW
                   #region
                        if (File.Exists(base_dir.ToString()))
                    {

                        proc4.StandardInput.WriteLine("------------" + "Current to Base: " + " loop");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                        proc4.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                        proc4.StandardInput.WriteLine(dr2d);
                        Test_LogFile("Code section 1 - Current to Base " + " ,loop"); //ffu from what found in the device to base from config 
                    }
                    else { proc4.StandardInput.WriteLine("------------" + "Current to Base Error"+ " loop" + enter_and_line); }
                    #endregion

                    for (int i = 0; i < 5; i++)
                    {
                        proc4.StandardInput.WriteLine("- loop Number:  " + i);
                        //BASE to New_FW1
                        #region
                        if (!Directory.Exists(targetDirectory1)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "Base-FW1 - FW1-Base " + "," + "The Result: " + "Error " + " ,Loop: " + i + enter_and_line); }
                        else
                        {
                        Get_fluf(targetDirectory1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                                ///////////////////////////////////////////////////Base to New FW 1 Error
                            proc4.StandardInput.WriteLine("------------" + "Base to FW1: " +" Loop: "+ i);
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - Base to New FW1-loop");
                                ///////////////////////////////////////////////////New FW 1 to Base
                            proc4.StandardInput.WriteLine("------------" + "FW 1 to Base: " + " Loop: " + i);
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - FW 1 to Base-loop");
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Base-FW1 - FW1-Base " + "," + "The Result: " + "Error "+ " Loop: " + i + enter_and_line); }
                        }

                        #endregion
                    }//for
                } //else


                proc4.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 

                string s = proc4.StandardOutput.ReadToEnd();  //run commands SAVE TO variable s 
                output_loop1 = s; //save all runs to global varible to use in other places 
                //textBox1.Text = s;  //show in textbox1

                proc4.WaitForExit();
                proc4.Dispose();
                proc4.Close();

                //Writelog(); //write function run after all commands test run in func FFU_Run_new_2()  ,if just this function run sec 1 & 2 shoud run this func here

            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU upgrade downgrade commands between FW's-new
        /// </summary>
        /// 
        string output1;
        string base_dir;
        public void FFU_Run_new()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Section 1 & 2 Start ################################");
                ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process.RedirectStandardOutput = true;
                process.RedirectStandardInput = true;
                process.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process.CreateNoWindow = true;  //start the process in a new window.
                //variables 
                var proc = Process.Start(process);
                string dr1d;
                string dr2d;
                proc.StandardInput.WriteLine("*** Section 1 & 2 Start *** ");
                //Code section 1 ///////////////////////////////////////////////////////////////////////////////////////////////
                if (!Directory.Exists(BaseDirectory1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "Section 1/2 Error , Input Value Wrong "); }
                else
                {

                Get_fluf(BaseDirectory1);  //send BaseDirectory1 value to get full fluf path func and return value to the var return_fluf_check
                base_dir = return_fluf_check;  //put return_fluf_check after run the func Get_fluf(BaseDirectory1) to base_dir

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //else if (return_fluf_check.ToString() != "")
                //else if (if_file_exist.ToString() != "error")
                //{ // close after regions

                    //current to Base_FW+ curent to curent + curent to new fw 1 / curen to fw 2 
                    #region

                    proc.StandardInput.WriteLine("*** Section 1 Start *** ");
                    proc.StandardInput.WriteLine("***" + "FFU Between Base to NEW FW 1 ");
                    //current to Base_FW
                    #region
                    if (File.Exists(base_dir.ToString()))
                    {

                        proc.StandardInput.WriteLine("------------" + "Current to Base: ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                        proc.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                        proc.StandardInput.WriteLine(dr2d);
                        Test_LogFile("Code section 1 - Current to Base "); //ffu from what found in the device to base from config 
                    }
                    else { proc.StandardInput.WriteLine("------------" + "Current-Base " + "," + "The Result: " + "Error " + enter_and_line); }
                    #endregion
                    //Base to Base 
                    #region

                    if (File.Exists(base_dir.ToString()))
                    {

                        proc.StandardInput.WriteLine("------------" + "Base to Base");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);
                        proc.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                        proc.StandardInput.WriteLine(dr2d);
                        Test_LogFile("Code section 1 - Base to Base ");
                    }
                    else { proc.StandardInput.WriteLine("------------" + "Base-Base " + "," + "The Result: " + "Error " + enter_and_line); }
                    #endregion
                    Thread.Sleep(2000);
                    //BASE to New_FW1
                    #region
                    //try for 10 times run 

                    if (!Directory.Exists(targetDirectory1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "Base-FW1 - FW1-Base " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            ///////////////////////////////////////////////////Base to New FW 1 Error
                            proc.StandardInput.WriteLine("------------" + "Base to FW 1: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - Base to New FW 1");
                            ///////////////////////////////////////////////////New FW 1 to Base
                            proc.StandardInput.WriteLine("------------" + "FW 1 to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - New FW 1 to Base");
                        }
                        else { proc.StandardInput.WriteLine("------------" + "Base-FW1 - FW1-Base " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    
                    #endregion
                    Thread.Sleep(2000);
                    //Base to New_FW2
                    #region
                    if (!Directory.Exists(targetDirectory2)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "Base-FW2 - FW2-Base " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory2);
                        //if (return_fluf_check.ToString() != "")
                        //{
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            ///////////////////////////////////////////////////Base to New FW 2 Error
                            proc.StandardInput.WriteLine("------------" + "Base to FW2: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - Base to FW2 ");
                            /////////////////////////////////////////////////FW 2 to Base Error
                            proc.StandardInput.WriteLine("------------" + "FW2 to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - FW 2 to Base");
                        }
                        else { proc.StandardInput.WriteLine("------------" + "Base-FW2 - FW2-Base " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    proc.StandardInput.WriteLine("*** Section 1 Done ***");
                    #endregion
                    Thread.Sleep(2000);
                    //new fw 1 to new fw2 -< to base  
                    #region
                    if (!Directory.Exists(targetDirectory1) || !Directory.Exists(targetDirectory2)) {if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "FW1-FW2 - FW2-FW1 " + "," + "The Result: " + "Error " + enter_and_line); }
                    //else if (!Directory.Exists(targetDirectory2)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "New Fw2 Error "); }
                    else 
                    {
                        Get_fluf(targetDirectory1);
                        //if (return_fluf_check.ToString() != "")
                        //{
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            ///////////////////////////////////////////////////Base to New FW1 
                            proc.StandardInput.WriteLine("------------" + "Base to FW1: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); 
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - Base to New FW1  ");
                            /////////////////////////////////////////////////New FW1 to NEW FW2 
                            Thread.Sleep(2000);
                            Get_fluf(targetDirectory2);
                            proc.StandardInput.WriteLine("------------" + "FW1 to FW2: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - FW1 to FW2");
                            /////////////////////////////////////////////////FW 2 to Base 
                            proc.StandardInput.WriteLine("------------" + "FW 2 to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 1 - FW2 to Base ");
                        }
                        else { proc.StandardInput.WriteLine("------------" + "FW1-FW2 - FW2-FW1 " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion

                    #endregion
                    //}

                //Code section 2 ///////////////////////////////////////////////////////////////////////////////////////////////        
                // (NEW FW 1) FFU Between Customers(GOEM1-base,DELL1-base,LENOVO1-base,HP1-base)+MSFT1-base in same FW    
                //Get_fluf(targetDirectory_GO_1); 
                //Get_fluf(targetDirectory_HP_1); 
                //Get_fluf(targetDirectory_LE_1); 
                //Get_fluf(targetDirectory_DE_1);
                //Get_fluf(targetDirectory_MSFT_1);           
                //Get_fluf(targetDirectory_GO_1);
                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //else if (return_fluf_check.ToString() != "")
                //else if (if_file_exist.ToString() != "error")
                //{ // close after regions
                Thread.Sleep(3000);
                //GO
                #region
            if (!Directory.Exists(targetDirectory_GO_1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "BASE-GO(FW1) - GO(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
        else
        {
            Get_fluf(targetDirectory_GO_1);
            if (File.Exists(return_fluf_check.ToString()))
            {
                proc.StandardInput.WriteLine("*** Section 2 Start ***");
                proc.StandardInput.WriteLine("***" + ">***FFU Between Base to NEW FW 1 ");
                /////////////////////////////////////////////////Base to GO New_FW1
                proc.StandardInput.WriteLine("------------" + "Base to GO(FW1): ");
                dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                proc.StandardInput.WriteLine(dr1d);
                dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                proc.StandardInput.WriteLine(dr2d);
                Test_LogFile("Code section 2 - Base to GO FW1 " + " , Fw Name: " + return_fluf_check);
                /////////////////////////////////////////////////GO to Base New_FW1
                proc.StandardInput.WriteLine("------------" + "GO(FW1) to Base: ");
                dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                proc.StandardInput.WriteLine(dr1d);
                dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                proc.StandardInput.WriteLine(dr2d);
                Test_LogFile("Code section 2 - GO to Base New_FW1" + " , Fw Name: " + base_dir);
            }
            else { proc.StandardInput.WriteLine("------------" + "BASE-GO(FW1) - GO(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
        }
            #endregion
                Thread.Sleep(3000);
                //HP
                #region
                if (!Directory.Exists(targetDirectory_HP_1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "BASE-HP(FW1) - HP(FW1)-BASE  " + "," + "The Result: " + "Error " + enter_and_line); }
                else
                {
                    Get_fluf(targetDirectory_HP_1);

                    //if (return_fluf_check.ToString() != "")
                    //{
                    ///////////////////////////////////////////////////Base to HP New_FW1
                    if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Base to HP(FW1):  ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - Base to HP(FW1)" + " , Fw Name: " + return_fluf_check);
                            /////////////////////////////////////////////////HP to Base New_FW1
                            proc.StandardInput.WriteLine("------------" + "HP(FW1) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - HP(FW1) to Base" + " , Fw Name: " + base_dir);
                        }
                        else { proc.StandardInput.WriteLine("------------" + "BASE-HP(FW1) - HP(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                Thread.Sleep(3000);
                //LE
                #region
                if (!Directory.Exists(targetDirectory_LE_1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "BASE-LE(FW1) - LE(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                else
                {
                    Get_fluf(targetDirectory_LE_1);

                    //if (return_fluf_check.ToString() != "")
                    //{
                    ///////////////////////////////////////////////////Base to LE New_FW1
                    if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Base to LE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); 
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - Base to LE(FW1) " + " , Fw Name: " + return_fluf_check);
                            /////////////////////////////////////////////////LE to Base New_FW1
                            proc.StandardInput.WriteLine("------------" + "LE(FW1) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); 
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - LE(FW1) to Base" + " , Fw Name: " + base_dir);
                        }
                        else { proc.StandardInput.WriteLine("------------" + "BASE-LE(FW1) - LE(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                Thread.Sleep(3000);
                //Dell
                #region
                    if (!Directory.Exists(targetDirectory_DE_1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "BASE-DELL(FW1) - DELL(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_1);
                        //if (return_fluf_check.ToString() != "")
                        //{
                        ///////////////////////////////////////////////////Base to DE New_FW1
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc.StandardInput.WriteLine("------------" + "Base to DELL(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - Base to DELL(FW1)" + " , Fw Name: " + return_fluf_check);
                            /////////////////////////////////////////////////DE to Base New_FW1 
                            proc.StandardInput.WriteLine("------------" + "DELL(FW1) to Base: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                            proc.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 2 - DELL(FW1) to Base" + " , Fw Name: " + base_dir);
                        }
                       else { proc.StandardInput.WriteLine("------------" + "BASE-DELL(FW1) - DELL(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                //Thread.Sleep(3000);
                ////MSFT- run on the another func 
                #region
                //if (!Directory.Exists(targetDirectory_MSFT_1)) { if_file_exist = "error"; proc.StandardInput.WriteLine("------------" + "Base to MSFT New_FW1 Error & MSFT to Base New_FW1 Error "); }
                //else
                //{
                //    Get_fluf(targetDirectory_MSFT_1);
                //    ///////////////////////////////////////////////////Base to MSFT New_FW1
                //    if (File.Exists(return_fluf_check.ToString()))
                //    {
                //        proc.StandardInput.WriteLine("------------" + "Base to MSFT New_FW1: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 2 - Base to MSFT New_FW1");
                //        /////////////////////////////////////////////////MSFT to Base New_FW1
                //        proc.StandardInput.WriteLine("------------" + "MSFT to Base New_FW1: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 2 - MSFT to Base New_FW1");
                //    }
                //    else { proc.StandardInput.WriteLine("------------" + "Base to MSFT New_FW1 Error & MSFT to Base New_FW1 Error "); }
                //}
                #endregion
                proc.StandardInput.WriteLine("*** Section 2 Done ***");
                }
                //else { proc.StandardInput.WriteLine("*** Section 2 Error ***"); }
                ///////////////////////////////////////////
                //SECCTION 3 RUN ON FFU_Run_new_2() ,because if tun all configuratiin in one PROC bugger will full and stuck the tool
                ///////////////////////////////////////////
                //Code section 3 ///////////////////////////////////////////////////////////////////////////////////////////////       
                // (NEW FW 2) FFU Between Customers(GOEM2-base,DELL2-base,LENOVO2-base,HP2-base)+MSFT2-base in same FW    
                //if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //else
                //{
                //------------------------------------------------------------------------
                //sec 3 run on the another func
                //GO
                #region
                //Thread.Sleep(5000);
                //Get_fluf(targetDirectory_GO_2);
                //if (File.Exists(return_fluf_check))
                //{
                //    proc.StandardInput.WriteLine("***" + "Start Section 3 ");
                //    proc.StandardInput.WriteLine("***" + "FFU Between Current FW to NEW FW ");
                //}
                //else { proc.StandardInput.WriteLine(return_fluf_check + "------------" + "Not Exist"); }
                /////////////////////////////////////////////////////current Customer to Target 1
                //if (return_fluf_check.ToString() != "")
                //{
                //    if (File.Exists(return_fluf_check.ToString()))
                //    {
                //        proc.StandardInput.WriteLine("------------" + "Current Customer to GO: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 -Current Customer to GO" + " , Fw Name: " + return_fluf_check);

                //        /////////////////////////////////////////////////Target 1 to current Customer
                //        proc.StandardInput.WriteLine("------------" + "GO to Current Customer: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - GO to Current Customer" + " , Fw Name: " + base_dir);

                //    }
                //    else { proc.StandardInput.WriteLine(return_fluf_check + "------------" + "Not Exist"); }
                //}
                #endregion
                //Thread.Sleep(3000);
                //HP
                #region
                //Get_fluf(targetDirectory_HP_2);
                //if (return_fluf_check.ToString() != "")
                //{
                //    ///////////////////////////////////////////////////current Customer to Target 2
                //    if (File.Exists(return_fluf_check.ToString()))
                //    {
                //        proc.StandardInput.WriteLine("------------" + "Current Customer to HP: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - Current Customer to HP" + " , Fw Name: " + return_fluf_check);

                //        /////////////////////////////////////////////////Target 2 to current Customer 
                //        proc.StandardInput.WriteLine("------------" + "HP to Current Customer: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - HP to Current Customer" + " , Fw Name: " + base_dir);

                //    }
                //    else { proc.StandardInput.WriteLine(return_fluf_check + "------------" + "Not Exist"); }
                //}
                #endregion
                //Thread.Sleep(3000);
                //LE
                #region
                //Get_fluf(targetDirectory_LE_2);
                //if (return_fluf_check.ToString() != "")
                //{
                //    /////////////////////////////////////////////////current Customer to Target 3
                //    if (File.Exists(return_fluf_check.ToString()))
                //    {
                //        proc.StandardInput.WriteLine("------------" + "Current Customer to LE: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - Current Customer to LE" + " , Fw Name: " + return_fluf_check);

                //        ///////////////////////////////////////////////Target 3 to current Customer
                //        proc.StandardInput.WriteLine("------------" + "LE Customer to Current Customer: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - LE Customer to Current Customer" + " , Fw Name: " + base_dir);

                //    }
                //    else { proc.StandardInput.WriteLine(return_fluf_check + "..." + "Not Exist"); }
                //}
                #endregion
                //Thread.Sleep(3000);
                //DELL
                #region
                //Get_fluf(targetDirectory_DE_2);
                //if (return_fluf_check.ToString() != "")
                //{
                //    /////////////////////////////////////////////////current Customer to Target 3
                //    if (File.Exists(return_fluf_check.ToString()))
                //    {
                //        proc.StandardInput.WriteLine("Current Customer to DE: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - Current Customer to DE" + " , Fw Name: " + return_fluf_check);
                //        ///////////////////////////////////////////////Target 3 to current Customer
                //        //proc.StandardInput.WriteLine("------------" + "DE to Current Customer: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - DE to Current Customer" + " , Fw Name: " + base_dir);
                //    }
                //    else { proc.StandardInput.WriteLine(return_fluf_check + "------------" + "Not Exist"); }
                //}

                #endregion
                //Thread.Sleep(3000);
                //MSFT
                #region
                //Get_fluf(targetDirectory_MSFT_2);
                //if (return_fluf_check.ToString() != "")
                //{
                //    ///////////////////////////////////////////////////current to Target 5 Customer
                //    if (File.Exists(return_fluf_check.ToString()))
                //    {
                //        proc.StandardInput.WriteLine("------------" + "Current Customer to MSFT: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - Current Customer to MSFT");
                //        /////////////////////////////////////////////////Target 5 to current Customer
                //        proc.StandardInput.WriteLine("------------" + "MSFT to Current Customer: ");
                //        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                //        proc.StandardInput.WriteLine(dr1d);
                //        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                //        proc.StandardInput.WriteLine(dr2d);
                //        Test_LogFile("Code section 3 - MSFT to Current Customer");

                //        proc.StandardInput.WriteLine("End Section 3");
                //        shoud be out the else to print the ffu 1 before the second func
                //        proc.StandardInput.WriteLine("FFU Completed Successfully.");
                //    }
                //    else { proc.StandardInput.WriteLine(return_fluf_check + "------------" + "Not Exist"); }
                //}
                #endregion
                //} //else

                proc.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 

                string s = proc.StandardOutput.ReadToEnd();  //run commands SAVE TO variable s 
                output1 = s; //save all runs to global varible to use in other places 
                //textBox1.Text = s;  //show in textbox1

                proc.WaitForExit();
                proc.Dispose();
                proc.Close();

                Test_LogFile("FFU_Run_new()  ################################ Section 1 & 2 End ################################");
                //Writelog(); //write function run after all commands test run in func FFU_Run_new_2()  ,if just this function run sec 1 & 2 shoud run this func here

            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////// DEBUG
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// FFU_Run_new_Debug();
        /// </summary>
        /// 
        string output_Debug1;
        public void FFU_Run_new_Debug()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Debug Version UP & Down ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc4 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                proc4.StandardInput.WriteLine("*** Debug Version UP & Down ***");

                if (!Directory.Exists(BaseDirectory1)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "BaseDirectory1 Error , Input Value Wrong "); }
                else
                {
                    //BaseDirectory1
                    #region
                    Get_fluf(BaseDirectory1);  //send BaseDirectory1 value to get full fluf path func and return value to the var return_fluf_check
                    base_dir = return_fluf_check;  //put return_fluf_check after run the func Get_fluf(BaseDirectory1) to base_dir
                    //current to Base_FW
                    #region
                    if (File.Exists(base_dir.ToString()))
                    {

                        proc4.StandardInput.WriteLine("------------" + "DEBUG - Current to Base_FW: ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                        proc4.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                        proc4.StandardInput.WriteLine(dr2d);
                        Test_LogFile("DEBUG - Current to Base_FW"); //ffu from what found in the device to base from config 
                    }
                    else { proc4.StandardInput.WriteLine("------------" + "DEBUG - Current to Base_FW" + enter_and_line); }
                    #endregion

                    //Base_FW to Base_FW
                    #region
                    if (File.Exists(base_dir.ToString()))
                    {
                        proc4.StandardInput.WriteLine("------------" + "DEBUG - Base_FW to Base_FW: ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                        proc4.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                        proc4.StandardInput.WriteLine(dr2d);
                        Test_LogFile("DEBUG - Base_FW to Base_FW"); //ffu from what found in the device to base from config 
                    }
                    else { proc4.StandardInput.WriteLine("------------" + "DEBUG - Base_FW to Base_FW" + enter_and_line); }
                    #endregion
                    #endregion


                    //targetDirectory1
                    #region
                    if (!Directory.Exists(targetDirectory1)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "targetDirectory1 Error , Input Value Wrong "); }
                    {
                        Get_fluf(targetDirectory1);
                        //BASE to fw1
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW1: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- Base_FW to FW1"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW1" + enter_and_line); }
                        #endregion

                        Get_fluf(BaseDirectory1);
                        //fw1 to Base_FW
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- FW1 to Base_FW: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- FW1 to Base_FW"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- FW1 to Base_FW Error" + enter_and_line); }
                        #endregion
                    }
                    #endregion


                    //targetDirectory2
                    #region
                    if (!Directory.Exists(targetDirectory2)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "targetDirectory1 Error , Input Value Wrong "); }
                    {
                        Get_fluf(targetDirectory2);
                        //Base_FW to fw2
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW2: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- Base_FW to FW2"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW2" + enter_and_line); }
                        #endregion

                        Get_fluf(BaseDirectory1);
                        //fw2 to Base_FW
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- FW2 TO Base_FW: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- FW2 TO Base_FW"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- FW2 TO Base_FW" + enter_and_line); }
                        #endregion
                    }
                    #endregion
                }

                proc4.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_1 = proc4.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_Debug1 = ckey_1;
                proc4.WaitForExit();
                proc4.Dispose();
                proc4.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU_Run_new_Debug_SED();
        /// </summary>
        /// 
        string output_Debug2;
        public void FFU_Run_new_Debug_SED()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Debug Version SED UP & Down ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc4 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                proc4.StandardInput.WriteLine("*** Debug Version SED UP & Down ***");

                if (!Directory.Exists(BaseDirectory1)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "BaseDirectory1 Error , Input Value Wrong "); }
                else
                {
                    //BaseDirectory1
                    #region
                    Get_fluf(BaseDirectory1);  //send BaseDirectory1 value to get full fluf path func and return value to the var return_fluf_check
                    base_dir = return_fluf_check;  //put return_fluf_check after run the func Get_fluf(BaseDirectory1) to base_dir
                    //current to Base_FW
                    #region
                    if (File.Exists(base_dir.ToString()))
                    {

                        proc4.StandardInput.WriteLine("------------" + "DEBUG - Current to Base_FW: ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                        proc4.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                        proc4.StandardInput.WriteLine(dr2d);
                        Test_LogFile("DEBUG - Current to Base_FW"); //ffu from what found in the device to base from config 
                    }
                    else { proc4.StandardInput.WriteLine("------------" + "DEBUG - Current to Base_FW" + enter_and_line); }
                    #endregion

                    //Base_FW to Base_FW
                    #region
                    if (File.Exists(base_dir.ToString()))
                    {
                        proc4.StandardInput.WriteLine("------------" + "DEBUG - Base_FW to Base_FW: ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + base_dir);   //
                        proc4.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                        proc4.StandardInput.WriteLine(dr2d);
                        Test_LogFile("DEBUG - Base_FW to Base_FW"); //ffu from what found in the device to base from config 
                    }
                    else { proc4.StandardInput.WriteLine("------------" + "DEBUG - Base_FW to Base_FW" + enter_and_line); }
                    #endregion
                    #endregion


                    //targetDirectory1
                    #region
                    if (!Directory.Exists(targetDirectory1)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "targetDirectory1 Error , Input Value Wrong "); }
                    {
                        Get_fluf(targetDirectory1);
                        //BASE to fw1
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW1: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- Base_FW to FW1"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW1" + enter_and_line); }
                        #endregion

                        Get_fluf(BaseDirectory1);
                        //fw1 to Base_FW
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- FW1 to Base_FW: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- FW1 to Base_FW"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- FW1 to Base_FW Error" + enter_and_line); }
                        #endregion
                    }
                    #endregion


                    //targetDirectory2
                    #region
                    if (!Directory.Exists(targetDirectory2)) { if_file_exist = "error"; proc4.StandardInput.WriteLine("------------" + "targetDirectory1 Error , Input Value Wrong "); }
                    {
                        Get_fluf(targetDirectory2);
                        //Base_FW to fw2
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW2: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- Base_FW to FW2"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- Base_FW to FW2" + enter_and_line); }
                        #endregion

                        Get_fluf(BaseDirectory1);
                        //fw2 to Base_FW
                        #region
                        if (File.Exists(return_fluf_check.ToString()))
                        {

                            proc4.StandardInput.WriteLine("------------" + "Debug- FW2 TO Base_FW: ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);   //
                            proc4.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1"); //
                            proc4.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Debug- FW2 TO Base_FW"); //ffu from what found in the device to base from config 
                        }
                        else { proc4.StandardInput.WriteLine("------------" + "Debug- FW2 TO Base_FW" + enter_and_line); }
                        #endregion
                    }
                    #endregion
                }





                proc4.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_1 = proc4.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_Debug2 = ckey_1;
                proc4.WaitForExit();
                proc4.Dispose();
                proc4.Close();               
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////// CUSTOMER
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// FFU_Run_new_ckey_1()
        /// </summary>
        /// 
        string output_ckey_1;
        public void FFU_Run_new_ckey_1()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Section 3 Start ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //else if (return_fluf_check.ToString() != "")
                //else if (if_file_exist.ToString() != "error")
                //{

                ///////////////to same version( run on vendors)
                proc1.StandardInput.WriteLine("*** Customer Key- Same Version - Between Vendors ***");
                Thread.Sleep(3000);
                //GO - FW base
                #region
                if (!Directory.Exists(targetDirectory_GO_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                else
                {
                    Get_fluf(targetDirectory_GO_1_base);
                    if (File.Exists(return_fluf_check.ToString()))
                    {
                        proc1.StandardInput.WriteLine("------------" + "Base to GO(FW1): ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                        proc1.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                        proc1.StandardInput.WriteLine(dr2d);
                        Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                    }
                    else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                }
                #endregion
                Thread.Sleep(3000);
                //dell- FW base
                #region
                if (!Directory.Exists(targetDirectory_DE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                else
                {
                    Get_fluf(targetDirectory_DE_1_base);
                    if (File.Exists(return_fluf_check.ToString()))
                    {
                        proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                        proc1.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                        proc1.StandardInput.WriteLine(dr2d);
                        Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                    }
                    else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                }
            #endregion
                Thread.Sleep(3000);
                //hp- FW base
                #region
                if (!Directory.Exists(targetDirectory_HP_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                else
                {
                    Get_fluf(targetDirectory_HP_1_base);
                    if (File.Exists(return_fluf_check.ToString()))
                    {
                        proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                        dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                        proc1.StandardInput.WriteLine(dr1d);
                        dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                        proc1.StandardInput.WriteLine(dr2d);
                        Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                    }
                    else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                }
            #endregion
                Thread.Sleep(3000);
                //LE- FW base
                #region
                if (!Directory.Exists(targetDirectory_LE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
            #endregion
                Thread.Sleep(3000);
                //msft- FW base
                #region
                if (!Directory.Exists(targetDirectory_MSFT_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                #endregion

                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_1 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_1 = ckey_1;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU_Run_new_ckey_2()
        /// </summary>
        /// 
        string output_ckey_2;
        public void FFU_Run_new_ckey_2()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Section 3 Start ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //else if (return_fluf_check.ToString() != "")
                //else if (if_file_exist.ToString() != "error")
                //{

                ///////////////to another version(fw1)
                proc1.StandardInput.WriteLine("*** Customer Key- Another Version - Between versions ***");
                if (New_FW1 != "")
                {
                    //GO - FW 1
                    #region
                    Thread.Sleep(5000);
                    if (!Directory.Exists(targetDirectory_GO_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_GO_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to GO(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //dell- FW 1
                    #region
                    Thread.Sleep(5000);
                    if (!Directory.Exists(targetDirectory_DE_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //hp- FW 1
                    #region
                    if (!Directory.Exists(targetDirectory_HP_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_HP_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //LE- FW 1
                    #region
                    if (!Directory.Exists(targetDirectory_LE_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //msft- FW 1
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW2)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW2) - GO(FW2)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }

                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_1 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_2 = ckey_1;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }

        /////////////// CUSTOMER SED - NON SED

        /// <summary>
        /// FFU_Run_new_ckey_SED_1()
        /// </summary>
        /// 
        string output_ckey_sed_1;
        public void FFU_Run_new_ckey_SED_1()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key  ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //if Vulcan_Version == "Performance_Version_Pre_RDT")
                #region
                if (Vulcan_Version == "Performance_Version") //non sed
                {
                    Vulcan_Version = "SED_Performance_Version";
                    //##########################################################
                    //take Base fw split it and add VCO(Sed) instead of VCP(nonsed)
                    string str = Base_FW;
                    str.Substring(0, 5); //AO043
                    Base_FW = str.Substring(0, 5) + "VCO";
                    //##########################################################
                    //go to sed  // vulcan_sed_perf_pre_rdt_BOT
                    targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\GO\";
                    targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\HP\";
                    targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\LE\";
                    targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\DE\";
                    targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\MSFT\";
                    Test_LogFile("File_value()-Customer_Non sed-sed - base" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                }
                #endregion
                //if Vulcan_Version == "SED_Performance_Version_Pre_RDT"
                #region
                else if (Vulcan_Version == "SED_Performance_Version") //sed
                {
                    Vulcan_Version = "Performance_Version";
                    //##########################################################
                    //take Base fw split it and add VCO(Sed) instead of VCP(nonsed)
                    string str = Base_FW;
                    str.Substring(0, 5); //AR043
                    Base_FW = str.Substring(0, 5) + "VCP";
                    targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\GO\";
                    targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\HP\";
                    targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\LE\";
                    targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\DE\";
                    targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\MSFT\";
                    Test_LogFile("File_value()-Customer_sed-non sed-base" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                }
                #endregion


                proc1.StandardInput.WriteLine("*** Customer Key - Non SED -> SED / SED - Non SED***" + Vulcan_Version + "->>" + Vulcan_Version);


                if (Base_FW != "")
                {
                    //dell- FW base
                    Thread.Sleep(3000);
                    #region
                    if (!Directory.Exists(targetDirectory_DE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "SED BASE-DE(Base) - DE(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "SED Base to DE(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("SED Code section 3 -Base to DE(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED BASE-DE(Base) - DE(Base)-BASE " + "," + "The Result : " + "Error " + enter_and_line); }
                    }
                    #endregion
                    //GO - FW base
                    Thread.Sleep(3000);
                    #region

                    if (!Directory.Exists(targetDirectory_GO_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "SED-BASE-GO(Base) - GO(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_GO_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "SED Base to GO(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("SED Code section 3 -Base to GO(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "SED - BASE-GO(Base) - GO(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //hp- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_HP_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "SED BASE-HP(Base) - HP(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_HP_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "SED - Base to HP(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("SED Code section 3 -Base to HP(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "SED BASE-HP(Base) - HP(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }
                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_3 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_sed_1 = ckey_3;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }

        }


        /// <summary>
        /// FFU_Run_new_ckey_SED_2()
        /// </summary>
        /// 
        string output_ckey_sed_2;
        public void FFU_Run_new_ckey_SED_2()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key  ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //if Vulcan_Version == "Performance_Version_Pre_RDT")
                #region
                if (Vulcan_Version == "Performance_Version") //non sed
                {
                    Vulcan_Version = "SED_Performance_Version";
                    //##########################################################
                    //take Base fw split it and add VCO(Sed) instead of VCP(nonsed)
                    string str = Base_FW;
                    str.Substring(0, 5); //AO043
                    Base_FW = str.Substring(0, 5) + "VCO";
                    //##########################################################
                    //go to sed  // vulcan_sed_perf_pre_rdt_BOT
                    targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\GO\";
                    targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\HP\";
                    targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\LE\";
                    targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\DE\";
                    targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\MSFT\";
                    Test_LogFile("File_value()-Customer_Non sed-sed - base" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                }
                #endregion
                //if Vulcan_Version == "SED_Performance_Version_Pre_RDT"
                #region
                else if (Vulcan_Version == "SED_Performance_Version") //sed
                {
                    Vulcan_Version = "Performance_Version";
                    //##########################################################
                    //take Base fw split it and add VCO(Sed) instead of VCP(nonsed)
                    string str = Base_FW;
                    str.Substring(0, 5); //AR043
                    Base_FW = str.Substring(0, 5) + "VCP";
                    targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\GO\";
                    targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\HP\";
                    targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\LE\";
                    targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\DE\";
                    targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\MSFT\";
                    Test_LogFile("File_value()-Customer_sed-non sed-base" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                }
                #endregion


                proc1.StandardInput.WriteLine("*** SED Customer Key - Non SED -> SED / SED - Non SED ***" + Vulcan_Version + "->>" + Vulcan_Version);


                if (Base_FW != "")
                {
                    Thread.Sleep(3000);
                    //msft- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "SED - BASE-MSFT(Base) - MSFT(Base)-BASE Not Exist " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "SED - Base to MSFT(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -SED - Base to MSFT(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "SED - BASE-MSFT(Base) - MSFT(Base)-BASE Not Exist" + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //LE- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_LE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "SED - BASE-LE(Base) - LE(Base)-BASE Not Exist" + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "SED - Base to LE(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -SED - Base to LE(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "SED - BASE-LE(Base) - LE(Base)-BASE Not Exist" + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }
                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_3 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_sed_2 = ckey_3;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////// PRE RDT
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// FFU_Run_new_ckey_Pre_RDT_1()
        /// </summary>
        /// 
        string output_ckey_Pre_RDT_1;
        public void FFU_Run_new_ckey_Pre_RDT_1()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key Pre_RDT ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                ///////////////to same version( run on vendors)
                proc1.StandardInput.WriteLine("*** Customer Key Pre_RDT- Same Version - Between Vendors ***");
                if (Base_FW != "")
                {
                    //GO - FW base
                    Thread.Sleep(3000);
                    #region

                    if (!Directory.Exists(targetDirectory_GO_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(Base) - GO(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_GO_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to GO(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(Base) - GO(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    //dell- FW base
                    Thread.Sleep(3000);
                    #region
                    if (!Directory.Exists(targetDirectory_DE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-DE(Base) - DE(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to DE(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-DE(Base) - DE(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //hp- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_HP_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-HP(Base) - HP(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_HP_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to HP(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to HP(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-HP(Base) - HP(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }

                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_1 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_Pre_RDT_1 = ckey_1;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU_Run_new_ckey_Pre_RDT_2()
        /// </summary>
        /// 
        string output_ckey_Pre_RDT_2;
        public void FFU_Run_new_ckey_Pre_RDT_2()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key Pre_RDT ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                ///////////////to same version( run on vendors)


                ///////////////to another version(fw1)
                proc1.StandardInput.WriteLine("*** Customer Key Pre_RDT- Another Version - Between versions ***");
                if (New_FW1 != "")
                {
                    //GO - FW 1
                    Thread.Sleep(3000);
                    #region
                    if (!Directory.Exists(targetDirectory_GO_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW1) - GO(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_GO_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to GO(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to GO(FW1)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-GO(FW1) - GO(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //dell- FW 1
                    #region
                    Thread.Sleep(5000);
                    if (!Directory.Exists(targetDirectory_DE_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-DE(FW1) - DE(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to DE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to DE(FW1)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-DE(FW1) - DE(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }
                if (Base_FW != "")
                {
                    //LE- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_LE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-LE(Base) - LE(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to LE(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to LE(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-LE(Base) - LE(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //msft- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(Base) - MSFT(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to MSFT(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to MSFT(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(Base) - MSFT(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }
                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_2 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_Pre_RDT_2 = ckey_2;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU_Run_new_ckey_Pre_RDT_3()
        /// </summary>
        /// 
        string output_ckey_Pre_RDT_3;
        public void FFU_Run_new_ckey_Pre_RDT_3()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key Pre_RDT ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                ///////////////to another version(fw1)
                proc1.StandardInput.WriteLine("*** Customer Key Pre_RDT- Another Version - Between versions ***");
                if (New_FW1 != "")
                {
                    Thread.Sleep(3000);
                    //hp- FW 1
                    #region
                    if (!Directory.Exists(targetDirectory_HP_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-HP(FW1) - HP(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_HP_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to HP(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to HP(FW1)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-HP(FW1) - HP(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //msft- FW 1
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(FW1) - MSFT(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to MSFT(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to MSFT(FW1)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(FW1) - MSFT(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //LE- FW 1
                    #region
                    if (!Directory.Exists(targetDirectory_LE_1)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-LE(FW1) - LE(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_1);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to LE(FW1): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to LE(FW1)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-LE(FW1) - LE(FW1)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                }

                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_3 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_Pre_RDT_3 = ckey_3;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }

        /////////////// PRE RDT SED - NON SED

        /// <summary>
        /// FFU_Run_new_ckey_Pre_RDT_1_sed()
        /// </summary>
        /// 
        string output_ckey_Pre_RDT_3_sed;
        public void FFU_Run_new_ckey_Pre_RDT_1_sed()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key Pre_RDT ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty
                //if Vulcan_Version == "Performance_Version_Pre_RDT")
                #region
                if (Vulcan_Version == "Performance_Version_Pre_RDT") //non sed
                {
                    Vulcan_Version = "SED_Performance_Version_Pre_RDT";
                    //##########################################################
                    //take Base fw split it and add VCO(Sed) instead of VCP(nonsed)
                    string str = Base_FW;
                    str.Substring(0, 5); //AR043
                    Base_FW = str.Substring(0, 5) + "VCO";
                    //##########################################################
                    //go to sed  // vulcan_sed_perf_pre_rdt_BOT
                    targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\GO\";
                    targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\HP\";
                    targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\LE\";
                    targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\DE\";
                    targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\MSFT\";
                    Test_LogFile("File_value()-Customer_Non sed-sed - base" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                }
                #endregion
                //if Vulcan_Version == "SED_Performance_Version_Pre_RDT"
                #region
                else if (Vulcan_Version == "SED_Performance_Version_Pre_RDT") //sed
                {
                    Vulcan_Version = "Performance_Version_Pre_RDT";
                    //##########################################################
                    //take Base fw split it and add VCO(Sed) instead of VCP(nonsed)
                    string str = Base_FW;
                    str.Substring(0, 5); //AR043
                    Base_FW = str.Substring(0, 5) + "VCP";
                    targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\GO\";
                    targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\HP\";
                    targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\LE\";
                    targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\DE\";
                    targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\MSFT\";
                    Test_LogFile("File_value()-Customer_sed-non sed-base" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                }
                #endregion


               proc1.StandardInput.WriteLine("*** Customer Key Pre_RDT- Non SED -> SED / SED - Non SED***" + Vulcan_Version + "->>" + Vulcan_Version);


                if (Base_FW != "")
                {
                    //dell- FW base
                    Thread.Sleep(3000);
                    #region
                    if (!Directory.Exists(targetDirectory_DE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED BASE-DE(Base) - DE(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_DE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED Base to DE(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Pre_RDT SED Code section 3 -Base to DE(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED BASE-DE(Base) - DE(Base)-BASE " + "," + "The Result : " + "Error " + enter_and_line); }
                    }
                    #endregion
                    //GO - FW base
                    Thread.Sleep(3000);
                    #region

                    if (!Directory.Exists(targetDirectory_GO_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED-BASE-GO(Base) - GO(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_GO_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED Base to GO(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Pre_RDT SED Code section 3 -Base to GO(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED - BASE-GO(Base) - GO(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //hp- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_HP_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED BASE-HP(Base) - HP(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_HP_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED - Base to HP(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Pre_RDT SED Code section 3 -Base to HP(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "Pre_RDT SED BASE-HP(Base) - HP(Base)-BASE " + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion

                }
                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_3 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_Pre_RDT_3_sed = ckey_3;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// FFU_Run_new_ckey_Pre_RDT_2_sed()
        /// </summary>
        /// 
        string output_ckey_Pre_RDT_3_sed_2;
        public void FFU_Run_new_ckey_Pre_RDT_2_sed()
        {
            try
            {
                Test_LogFile("FFU_Run_new()  ################################  Cusyomer Key Pre_RDT ################################");
                //string base_dir;
                //Test_LogFile("FFU_Run()  ################################ Start ################################");
                ProcessStartInfo process1 = new ProcessStartInfo(@"cmd.exe");
                //string path = Directory.GetCurrentDirectory();   //C:\Users\ff3\Documents\Visual Studio 2015\Projects\nvmekit4\nvmekit4\bin
                string path = @".\.\wdckit\wdckit.exe";
                process1.UseShellExecute = false;  //run all into the tool not outside(cmd)
                process1.RedirectStandardOutput = true;
                process1.RedirectStandardInput = true;
                process1.WorkingDirectory = Path.GetDirectoryName(path); //gets or sets the directory that contains the process to be started.
                process1.CreateNoWindow = true;  //start the process in a new window.

                var proc1 = Process.Start(process1);
                string dr1d;
                string dr2d;

                if (Device.ToString() == "") { MessageBox.Show("Enter Device(disk1,disk2,..))"); } //check if device empty

                if (Base_FW != "")
                {
                    Thread.Sleep(3000);
                    //msft- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_MSFT_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(Base) - MSFT(Base)-BASE Not Exist " + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_MSFT_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to MSFT(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to MSFT(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-MSFT(Base) - MSFT(Base)-BASE Not Exist" + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion
                    Thread.Sleep(3000);
                    //LE- FW base
                    #region
                    if (!Directory.Exists(targetDirectory_LE_1_base)) { if_file_exist = "error"; proc1.StandardInput.WriteLine("------------" + "BASE-LE(Base) - LE(Base)-BASE Not Exist" + "," + "The Result: " + "Error " + enter_and_line); }
                    else
                    {
                        Get_fluf(targetDirectory_LE_1_base);
                        if (File.Exists(return_fluf_check.ToString()))
                        {
                            proc1.StandardInput.WriteLine("------------" + "Base to LE(Base): ");
                            dr1d = ("wdckit.exe update " + Device.ToString() + " -f " + return_fluf_check);
                            proc1.StandardInput.WriteLine(dr1d);
                            dr2d = ("wdckit.exe update " + Device.ToString() + " -a -s 1");
                            proc1.StandardInput.WriteLine(dr2d);
                            Test_LogFile("Code section 3 -Base to LE(Base)" + " , Fw Name: " + return_fluf_check);
                        }
                        else { proc1.StandardInput.WriteLine("------------" + "BASE-LE(Base) - LE(Base)-BASE Not Exist" + "," + "The Result: " + "Error " + enter_and_line); }
                    }
                    #endregion

                }
                proc1.StandardInput.WriteLine("exit"); //without this command the tool enter to Infinite loop(lolaa lo sofet) and stuck 
                string ckey_3 = proc1.StandardOutput.ReadToEnd();  //run commands show in textbox1
                //textBox1.Text = output;  //show in textbox1
                output_ckey_Pre_RDT_3_sed_2 = ckey_3;
                proc1.WaitForExit();
                proc1.Dispose();
                proc1.Close();
                //Writelog(); //func                 
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: " + ex.Message);
                LogFile("FFU_Run_new_2() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Write log func
        /// </summary>
        /// 
        string get_ip;
        string file_path_py;
        string read_txt_file = DateTime.Now.ToString("ddMMyyyy_hhmmss");
        public void Writelog()
        {
            string logs_dir = Environment.CurrentDirectory + "\\" + "Logs";
            if (!Directory.Exists(logs_dir))
            {
                Directory.CreateDirectory(logs_dir);
            }
            get_ip = GetLocalIPAddress();
            //string text_file_same_date;
            string filePath_log = Environment.CurrentDirectory + "\\" + "Logs" + "\\" + "log_" + get_ip + "_" + Device + "_" + read_txt_file + ".txt";
            //File.WriteAllText(filePath_log, textBox1.Text); //Write contents of a textbox to .txt file    <--
            Thread.Sleep(4000);
            // using (StreamWriter sw = new StreamWriter(filePath_log, true))
            //{
                //File.WriteAllText(filePath_log, output1 + output2 + output3); //Write contents of a textbox to *.txt file-output 1+output2 take from ffu rus functions    <--
                 File.WriteAllText(filePath_log, output1 + output2 + output3 + output_loop1 + output_ckey_1+ output_ckey_2+ output_ckey_Pre_RDT_3 + output_ckey_Pre_RDT_2 + output_ckey_Pre_RDT_1 + output_ckey_Pre_RDT_3_sed + output_ckey_Pre_RDT_3_sed_2 + output_ckey_sed_2 + output_ckey_sed_1 + output_Debug1); //Write contents of a textbox to *.txt file-output 1+output2 take from ffu rus functions    <--
            //}

            file_path_py = filePath_log;
            Test_LogFile("Writelog() to path: " + filePath_log);
        }



        /// <summary>
        /// Write log func
        /// </summary>
        /// 
        string output_manual;
        string file_manual;
        public void Writelog_manual()
        {
            string logs_dir = Environment.CurrentDirectory + "\\" + "Logs" + "\\" + "Manual";
            if (!Directory.Exists(logs_dir))
            {
                Directory.CreateDirectory(logs_dir);
            }
            get_ip = GetLocalIPAddress();
            //string text_file_same_date;
            string file_log = Environment.CurrentDirectory + "\\" + "Logs" + "\\" + "Manual"+"\\" + "log_" + get_ip + "_" + "_" + read_txt_file + ".txt";
            //File.WriteAllText(filePath_log, textBox1.Text); //Write contents of a textbox to .txt file    <--
            Thread.Sleep(4000);
            // using (StreamWriter sw = new StreamWriter(filePath_log, true))
            //{
            File.WriteAllText(file_log, output_manual); //Write contents of a textbox to .txt file    <--
            //}

            file_manual = file_log;
            Test_LogFile("Writelog_manual() " + file_log);
        }




        /// <summary>
        /// get ip address
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }



        /// <summary>
        /// Check_Result
        /// </summary>
        ///        
        string Py_log;
        public void Check_Result()
        {
            string py_filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "scan.py";
            string txt_output = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "py_output.txt";// open this file
            string basePath1 = Environment.CurrentDirectory + "\\" + "Logs";// open this file
            string path1 = Directory.GetCurrentDirectory();
            string directory1 = path1 + @"\Config\Python\";
            string filter2 = @"\Config\Python\Run_py.bat";

            if (!File.Exists(py_filePath)) //if file not exist   //if (!Directory.Exists(basePath))
            {
                MessageBox.Show(@"File Not Exist in Path : \Config\Python\");
            }
            else
            {
                //MessageBox.Show("file exist");
                if (Directory.Exists(basePath1))
                {
                    //Python_log(); //Run Python file 
                    //for run python file , create bat file and insert the command (start scan.py) and call a batch file 
                    //Thread.Sleep(3000);
                    Run_bat_file_new(directory1, filter2);
                    Thread.Sleep(5000);
                    if (File.Exists(txt_output))
                    {
                        if (Read_all_txt_file(txt_output) != null)
                        {
                            Py_log = Read_all_txt_file(txt_output);
                        }
                    }
                    else { MessageBox.Show("txt_output not exist"+ "- "+ txt_output); }
                    
                    //#########################################################################################
                    //#########################################################################################
                }

                if (Py_log != "" && Py_log != null)
                {
                    //Txt_to_HTML(textBox1.Text);  
                    Txt_to_HTML(Py_log); ////get result from Py_log to func Txt_to_HTML to converte text to html
                    Html_Create(Py_log);
                }
                else
                { MessageBox.Show("Py_log es empty/null"); }                          
            }//else
            Test_LogFile("Check_Result()" + " , Python file Path: " + py_filePath);
        } //func




        /// <summary>
        /// Run batch file
        /// </summary>
        private void Run_bat_file()
        {
            string path = Directory.GetCurrentDirectory();
            string directory = path + @"\Config\Python\";
            string filter = @"\Config\Python\Latest_File.bat";
            string fullpath = path + filter;

            try
            {
                //int exitCode;
                ProcessStartInfo processInfo2;
                Process process2;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo2 = new ProcessStartInfo(fullpath);
                processInfo2.CreateNoWindow = true;
                processInfo2.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo2.RedirectStandardError = true;
                processInfo2.RedirectStandardOutput = true;
                processInfo2.WorkingDirectory = directory;  // instaead of cd C:\Users\Administrator\Documents ....
                process2 = Process.Start(processInfo2);
                //process.WaitForExit();
                process2.StandardOutput.ReadToEnd();
                //process.StandardError.ReadToEnd();
                //exitCode = process.ExitCode;
                process2.Dispose();
                process2.Close();
                Test_LogFile("Run_bat_file()" + fullpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// Run batch file- check if test pass/fail/no device fount / test number by batch files 
        /// </summary>
        private void Run_bat_file_result()
        {
            Test_LogFile("Run_bat_file_result()");
            string path = Directory.GetCurrentDirectory();

            ////////////////////////////////////////////////////////////
            string directory2 = path + @"\Output\";
            string filter2 = @"\Output\Tests_Num.bat";
            string fullpath2 = path + filter2;
            ////////////////////////////////////////////////////////////
            string filter3 = @"\Output\fw_activated_passed.bat";
            string fullpath3 = path + filter3;
            ///////////////////////////////////////////////////////////
            string filter4 = @"\Output\fw_activated_failed.bat";
            string fullpath4 = path + filter4;
            ////////////////////////////////////////////////////////////
            string filter5 = @"\Output\if_device_found.bat";
            string fullpath5 = path + filter5;
            ////////////////////////////////////////////////////////////
            string filter6 = @"\Output\fw_error.bat";
            string fullpath6 = path + filter6;
            ////////////////////////////////////////////////////////////
            try
            { 
                //2//////////////////////////////////////////////////////
                ProcessStartInfo processInfo3;
                Process process3;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo3 = new ProcessStartInfo(fullpath2);
                processInfo3.CreateNoWindow = true;
                processInfo3.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo3.RedirectStandardError = true;
                processInfo3.RedirectStandardOutput = true;
                processInfo3.WorkingDirectory = directory2;  // instaead of cd C:\Users\Administrator\Documents ....
                process3 = Process.Start(processInfo3);
                //process.WaitForExit();
                process3.StandardOutput.ReadToEnd();
                process3.Close();
                process3.Dispose();
                //process3.Kill();
                Test_LogFile("Run_bat_file_result()" + fullpath2);

                ////////////////////////////////////////////////////////
                ProcessStartInfo processInfo4;
                Process process4;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo4 = new ProcessStartInfo(fullpath3);
                processInfo4.CreateNoWindow = true;
                processInfo4.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo4.RedirectStandardError = true;
                processInfo4.RedirectStandardOutput = true;
                processInfo4.WorkingDirectory = directory2;  // instaead of cd C:\Users\Administrator\Documents ....
                process4 = Process.Start(processInfo4);
                //process.WaitForExit();
                process4.StandardOutput.ReadToEnd();
                process4.Close();
                process4.Dispose();
                //process4.Kill();
                Test_LogFile("Run_bat_file_result()" + fullpath3);


                ////////////////////////////////////////////////////////
                ProcessStartInfo processInfo5;
                Process process5;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo5 = new ProcessStartInfo(fullpath4);
                processInfo5.CreateNoWindow = true;
                processInfo5.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo5.RedirectStandardError = true;
                processInfo5.RedirectStandardOutput = true;
                processInfo5.WorkingDirectory = directory2;  // instaead of cd C:\Users\Administrator\Documents ....
                process5 = Process.Start(processInfo5);
                //process.WaitForExit();
                process5.StandardOutput.ReadToEnd();
                process5.Close();
                process5.Dispose();
                //process5.Kill();
                Test_LogFile("Run_bat_file_result()" + fullpath4);


                ////////////////////////////////////////////////////////
                ProcessStartInfo processInfo6;
                Process process6;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo6 = new ProcessStartInfo(fullpath5);
                processInfo6.CreateNoWindow = true;
                processInfo6.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo6.RedirectStandardError = true;
                processInfo6.RedirectStandardOutput = true;
                processInfo6.WorkingDirectory = directory2;  // instaead of cd C:\Users\Administrator\Documents ....
                process6 = Process.Start(processInfo6);
                //process.WaitForExit();
                process6.StandardOutput.ReadToEnd();
                process6.Close();
                process6.Dispose();
                //process6.Kill();
                Test_LogFile("Run_bat_file_result()" + fullpath5);
                ////////////////////////////////////////////////////////
                ProcessStartInfo processInfo7;
                Process process7;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo7 = new ProcessStartInfo(fullpath6);
                processInfo7.CreateNoWindow = true;
                processInfo7.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo7.RedirectStandardError = true;
                processInfo7.RedirectStandardOutput = true;
                processInfo7.WorkingDirectory = directory2;  // instaead of cd C:\Users\Administrator\Documents ....
                process7 = Process.Start(processInfo7);
                //process.WaitForExit();
                process7.StandardOutput.ReadToEnd();
                process7.Close();
                process7.Dispose();
                //process7.Kill();
                Test_LogFile("Run_bat_file_result()" + fullpath6);
                ////////////////////////////////////////////////////////
                Test_LogFile("Run_bat_file_result() Function End ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("Run_bat_file_result()- " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        /// <summary>
        /// delete temp files by batch file
        /// </summary>
        private void Run_bat_file_del()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                string directory = path + @"\Config\Python\";
                string filter1 = @"\Config\Python\Del_temp_result.bat";
                string fullpath1 = path + filter1;
                ProcessStartInfo processInfo2;
                Process process2;
                processInfo2 = new ProcessStartInfo(fullpath1);
                processInfo2.CreateNoWindow = true;
                processInfo2.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo2.RedirectStandardError = true;
                processInfo2.RedirectStandardOutput = true;
                processInfo2.WorkingDirectory = directory;  // instaead of cd C:\Users\Administrator\Documents ....
                process2 = Process.Start(processInfo2);
                //process.WaitForExit();
                process2.StandardOutput.ReadToEnd();
                process2.Dispose();
                process2.Close();
                Test_LogFile("Run_bat_file_del()" + fullpath1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("Run_bat_file_del() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        /// <summary>
        /// Run_bat_file_new
        /// </summary>
        /// 
        //string path = Directory.GetCurrentDirectory();
        //string directory = path + @"\Config\Python\";
        private void Run_bat_file_new(string directory,string filter1)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                //string directory = path + @"\Config\Python\";
                //string filter1 = @"\Config\Python\Del_temp_result.bat";
                string fullpath1 = path + filter1;
                ProcessStartInfo processInfo2;
                Process process2;
                processInfo2 = new ProcessStartInfo(fullpath1);
                processInfo2.CreateNoWindow = true;
                processInfo2.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo2.RedirectStandardError = true;
                processInfo2.RedirectStandardOutput = true;
                processInfo2.WorkingDirectory = directory;  // instaead of cd C:\Users\Administrator\Documents ....
                process2 = Process.Start(processInfo2);
                //process.WaitForExit();
                process2.StandardOutput.ReadToEnd();
                process2.Dispose();
                process2.Close();
                Test_LogFile("Run_bat_file_new()" + fullpath1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("Run_bat_file_new() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// Python_log
        /// </summary>
        /// 
        public void Python_log()
        {
            //string path = Directory.GetCurrentDirectory();
            //string python =  @"C:\Python27\python.exe";            
            //string myPythonApp = path + @"\Config\Python\scan.py";  // python app to call 
            ////int x = 10; int y = 20;     // dummy parameters to send Python script

            //// Create new process start info 
            //ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            //// make sure we can read the output from stdout 
            //myProcessStartInfo.UseShellExecute = false;
            //myProcessStartInfo.RedirectStandardOutput = true;
            //myProcessStartInfo.CreateNoWindow = false;

            //// start python app with 3 arguments,1st arguments is pointer to itself,
            //// 2nd and 3rd are actual arguments we want to send 
            ////myProcessStartInfo.Arguments = myPythonApp + " " + x + " " + y;
            //myProcessStartInfo.Arguments = myPythonApp;

            //Process myProcess = new Process();
            //myProcess.StartInfo = myProcessStartInfo;// assign start information to the process 

            //myProcess.Start();// start the process

            //// Read the standard output of the app we called.  
            //// in order to avoid deadlock we will read output first 
            //// and then wait for process terminate: 
            ////
            //StreamReader myStreamReader = myProcess.StandardOutput;

            ////string myString = myStreamReader.ReadLine();
            ////if you need to read multiple lines, you might use: 
            ////
            //string myString = myStreamReader.ReadToEnd();

            //// wait exit signal from the app we called and then close it. 
            //myProcess.WaitForExit();
            //myProcess.Close();
            ////MessageBox.Show(myString);
            ////textBox1.Text = myString;  //put the result in textbox1
            //Py_log = myString;

            //////////////////////
            //https://www.youtube.com/watch?v=g1VWGdHRkHs
            //////////////////////
           // var psi = new ProcessStartInfo();
           // psi.FileName = python;

           // var script = myPythonApp;

           // //psi.Arguments= $"\"{script}\"";
           // psi.Arguments = myPythonApp;
           // psi.UseShellExecute = false;
           // psi.RedirectStandardOutput = true;
           // psi.RedirectStandardError = true;
           // psi.CreateNoWindow = true;
           // //var result = "";
           // string result;
           // var proccess12 = Process.Start(psi);
           // //using (var proccess =  Process.Start(psi))
           // //{
           // //errors = proccess.StandardError.ReadToEnd();
           // //result = proccess.StandardOutput.ReadToEnd();
           // StreamReader myStreamReader = proccess12.StandardOutput;
           // result = myStreamReader.ReadToEnd();
           //// }
           // Py_log = result;
           // //////richTextBox1.Text = result;
           // //MessageBox.Show(Py_log);

        }




        /// <summary>
        /// Log file create function
        /// </summary>
        /// <param name="sEventName"></param>
        /// <param name="sControlName"></param>
        /// <param name="sFormName"></param>
        public void Test_LogFile(string Var)
        {
            StreamWriter log;
            if (!File.Exists("Test_LogFile.txt"))
            {
                log = new StreamWriter("Test_LogFile.txt");
            }
            else
            {
                log = File.AppendText("Test_LogFile.txt");
            }
            // Write to the file:

            log.WriteLine("===============================================Srart ============================================");
            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("--------------");
            log.WriteLine(Var);
            //log.WriteLine("Exception Name:" + sExceptionName);
            //log.WriteLine("Event Name:" + sEventName);
            //log.WriteLine("---------------");
            //log.WriteLine("Control Name:" + sControlName);
            //log.WriteLine("---------------");
            //log.WriteLine("Form Name:" + sFormName);
            log.WriteLine("===============================================End ==============================================");
            // Close the stream:
            log.Close();
        }



        /// <summary>
        /// Log file create function
        /// </summary>
        /// <param name="sEventName"></param>
        /// <param name="sControlName"></param>
        /// <param name="sFormName"></param>
        public void Test_LogFile_Manual_FFU(string Var)
        {
            StreamWriter log;
            if (!File.Exists("Test_LogFile_manual_FFU.txt"))
            {
                log = new StreamWriter("Test_LogFile_manual_FFU.txt");
            }
            else
            {
                log = File.AppendText("Test_LogFile_manual_FFU.txt");
            }
            // Write to the file:

            log.WriteLine("===============================================Srart ============================================");
            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("--------------");
            log.WriteLine(Var);
            log.WriteLine("===============================================End ==============================================");
            // Close the stream:
            log.Close();
        }



        /// <summary>
        /// Txt_to_HTML
        /// </summary>
        public string Txt_to_HTML(string text)
        {
            var sb = new StringBuilder();

            var sr = new StringReader(text);
            var str = sr.ReadLine();
            while (str != null)
            {
                str = str.TrimEnd();
                str.Replace("  ", "\n &nbsp;");
                if (str.Length > 80)
                {
                    sb.AppendLine($" {str} ");

                }
                else if (str.Length > 0)
                {
                    sb.AppendLine($"{str} \n");

                }
                str = sr.ReadLine();
            }
            return sb.ToString();
        }



        /// <summary>
        /// delete temporary files that creadted by scripts 
        /// </summary>
        private void Delete_Temp_files()
        {
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python"+ "\\" + "Latest_File.txt";
            string Del_temp_result = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Del_temp_result.bat";
            if (File.Exists(filePath)) {File.Delete(filePath); }
            if (File.Exists(Del_temp_result)){ Run_bat_file_del();}
            //
            string Del_1 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_failed.txt";
            string Del_2 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_passed.txt";
            string Del_3 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_error.txt";
            string Del_4 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "if_device_found.txt";
            string Del_5 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "if_device_found1.txt";
            string Del_6 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "Tests_Num.txt";
            if (File.Exists(Del_1)) { File.Delete(Del_1); }
            if (File.Exists(Del_2)) { File.Delete(Del_2); }
            if (File.Exists(Del_3)) { File.Delete(Del_3); }
            if (File.Exists(Del_4)) { File.Delete(Del_4); }
            if (File.Exists(Del_5)) { File.Delete(Del_5); }
            if (File.Exists(Del_6)) { File.Delete(Del_6); }
        }

        private void Delete_Temp_files2()
        {
            //string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python"+ "\\" + "Latest_File.txt";
            string Del_temp_result = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Del_temp_result.bat";
            //if (File.Exists(filePath)) {File.Delete(filePath); }
            if (File.Exists(Del_temp_result)) { Run_bat_file_del(); }
            //
            string Del_1 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_failed.txt";
            string Del_2 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_activated_passed.txt";
            string Del_3 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "fw_error.txt";
            string Del_4 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "if_device_found.txt";
            string Del_5 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "if_device_found1.txt";
            string Del_6 = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "Tests_Num.txt";
            if (File.Exists(Del_1)) { File.Delete(Del_1); }
            if (File.Exists(Del_2)) { File.Delete(Del_2); }
            if (File.Exists(Del_3)) { File.Delete(Del_3); }
            if (File.Exists(Del_4)) { File.Delete(Del_4); }
            if (File.Exists(Del_5)) { File.Delete(Del_5); }
            if (File.Exists(Del_6)) { File.Delete(Del_6); }
        }


        /// <summary>
        /// Html_Create()
        /// </summary>
        public void Html_Create(string txt_to)
        {
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "Latest_File.txt";
            string read_txt_file;
            string html_result_log_path = file_path_py + ".html";
            //string datenow = DateTime.Now.ToString("ddMMyyyy_hhmmss");
            //string filePath_log = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Result_" + s_device + "_" + datenow + ".html";

            var xDocument = new XDocument(
                        new XDocumentType("html", null, null, null),
                        new XElement("html", new XElement("body", new XElement("pre", txt_to))));

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n"
            };
            //using (var writer = XmlWriter.Create(@"Output\Result.html", settings))
            using (var writer = XmlWriter.Create(html_result_log_path, settings))
            {
                xDocument.WriteTo(writer);
            }

            Read_txt_file(filePath);
            if (Read_txt_file(filePath) != null)
            {
                read_txt_file = Read_txt_file(filePath); 
                //get_ip = GetLocalIPAddress();     
                //string output_folder_html_result = Environment.CurrentDirectory + "\\" + "Output" + "\\" + "log_" + get_ip + "_" + s_device + "_" + this.read_txt_file + ".html";
                string output_folder_html_result = Environment.CurrentDirectory + "\\" + "Output" + "\\" + read_txt_file + ".html";
                File.Move(html_result_log_path, output_folder_html_result); //Move(sourceFilePath, destinationFilePath);
            }
        }




        /// <summary>
        /// about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("All rights reserved to : SanDisk | a Western Digital brand | Abed.azem@wdc.com ");
        }



        /// <summary>
        /// ping check network ip
        /// </summary>
        /// <returns></returns>
        /// 
        //string ping_ip;
        private int Ping_check()
        {
            Ping myPing = new Ping();
            PingReply reply = myPing.Send(Network, 1000);
            //PingReply reply = myPing.Send("10.0.56.14", 1000);
            if (reply != null)
            {
                if (reply.Status.ToString() == "Success")
                {
                    //MessageBox.Show("success");
                    return 1;
                }
                else if (reply.Status.ToString() == "TimedOut")
                {
                    //MessageBox.Show("fail"); 
                    //MessageBox.Show("Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address);
                    return 0;
                }
            }
            return 0;
        }




        /// <summary>
        /// ping file check and read - read network file
        /// </summary>
        /// 
        //string ping_ip;
        public void Ping_file()
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config";
            string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Network_Ip.txt";// open this file

            if (Directory.Exists(basePath1))
            {
                if (File.Exists(filePath1))
                {
                    //Pass the file path and file name to the StreamReader 
                    //read from the text file 
                    String line;
                    StreamReader sr = new StreamReader(filePath1);
                    //Read the first line of text
                    line = sr.ReadLine();
                    //read 
                    if (line != null)
                    {
                        //MessageBox.Show(line);
                        //ping_ip = line;
                    }
                    else { MessageBox.Show(@"Enter the ip please... Config\Ip_Connect.txt "); }
                    sr.Close();
                }
                else { FileStream fs = File.Create(filePath1); }
            }
            else
            {
                MessageBox.Show("not exist-  " + filePath1);
                DirectoryInfo di = Directory.CreateDirectory(basePath1);
                FileStream fs = File.Create(filePath1);
            }
        }


        /// <summary>
        /// Read_txt_file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// 
        string  manual_run_count;
        public void RichTextBox_result(string file)
        {
            Test_LogFile("RichTextBox_result(string file)");
            string read_txt;

            //richTextBox1.AppendText("\n" + "Test Start: " + "\n");
            richTextBox1.AppendText("\n" + "#Test Status: ");
            string[] readText = File.ReadAllLines(file);
            foreach (string s in readText)
            {
                // Printing the string array containing 
                // all lines of the file. 
                //Varaibles
                //up(before the html show)
                string fwdownload_result = "Stored. Activation is still required.";
                string fwactivate_result_passed = "Firmware Activated passed.";
                string fwactivate_result_failed = "Firmware Activated Failed.";
                string fwactivate_result_failed_error_1 = "(Exit Code: -1)";
                string doc = "!DOCTYPE html ";
                string doc2 = "<";
                string doc3 = ">";
                string html = "html";
                string body = "body";
                string pre = "pre";
                string space= "		";
                string error1 = "Error";
                string teststart = "#Test Status:";
                //down(After the html show)
                string pre1 = "/pre";
                string body1 = "/body";
                string html1 = "/html";
                string sle = "/";
                //
                string manual1 = "Activation was successful.";
                string manual2 = "Failure (device reported an error)";
                string manual3 = "No Device Found";
                string command_Exec_Failed = "Command Execution Failed.";
                //


                int index = 0;
                
                richTextBox1.AppendText(s);
                richTextBox1.AppendText("\n"); //line down to text by text - inside loop 
                read_txt = s;

                // ----------------------------------------------------------------------------------
                //richTextBox1 Colors
                // ----------------------------------------------------------------------------------
                //teststart
                while (index < richTextBox1.Text.LastIndexOf(teststart))
                {
                    richTextBox1.Find(teststart, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Blue;
                    richTextBox1.SelectionFont = new Font("Tahoma", 16, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(teststart, index) + 1;
                }
                //fwdonlowad result:
                while (index < richTextBox1.Text.LastIndexOf(fwdownload_result))
                {
                    richTextBox1.Find(fwdownload_result, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(fwdownload_result, index) + 1;
                }

                //fwactivate result:passed
                while (index < richTextBox1.Text.LastIndexOf(fwactivate_result_passed))
                {
                    richTextBox1.Find(fwactivate_result_passed, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(fwactivate_result_passed, index) + 1;
                }

                //fwactivate result:failed
                while (index < richTextBox1.Text.LastIndexOf(fwactivate_result_failed))
                {
                    richTextBox1.Find(fwactivate_result_failed, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(fwactivate_result_failed, index) + 1;
                }

                //fwactivate result:failed -/>  (Exit Code: -1)"
                while (index < richTextBox1.Text.LastIndexOf(fwactivate_result_failed_error_1))
                {
                    richTextBox1.Find(fwactivate_result_failed_error_1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(fwactivate_result_failed_error_1, index) + 1;
                }

                //error1 -/> 
                while (index < richTextBox1.Text.LastIndexOf(error1))
                {
                    richTextBox1.Find(error1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.LightSalmon;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(error1, index) + 1;
                }
                //manual run
                while (index < richTextBox1.Text.LastIndexOf(manual1))
                {
                    richTextBox1.Find(manual1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(manual1, index) + 1;
                    manual_run_count = "Test Pass";
                }

                //manual run
                while (index < richTextBox1.Text.LastIndexOf(manual2))
                {
                    richTextBox1.Find(manual2, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(manual2, index) + 1;
                    manual_run_count = "Test Fail";
                }

                //manual run
                while (index < richTextBox1.Text.LastIndexOf(manual3))
                {
                    richTextBox1.Find(manual3, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(manual3, index) + 1;
                    manual_run_count = "No Device Found";
                }

                //manual run command_Exec_Failed
                while (index < richTextBox1.Text.LastIndexOf(command_Exec_Failed))
                {
                    richTextBox1.Find(command_Exec_Failed, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(command_Exec_Failed, index) + 1;
                    manual_run_count = "Command Execution Failed";
                }
                
                // ----------------------------------------------------------------------------------
                //richTextBox1 Remove string that show by reading HTML file (html><body><pre> "     ")
                // ----------------------------------------------------------------------------------
                //remove string
                while (index < richTextBox1.Text.LastIndexOf(doc))
                {
                    richTextBox1.Find(doc, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectedText = "";
                    index = richTextBox1.Text.IndexOf(doc, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(doc2))
                {
                    richTextBox1.Find(doc2, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectedText = "";
                    index = richTextBox1.Text.IndexOf(doc2, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(doc3))
                {
                    richTextBox1.Find(doc3, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectedText = "";
                    index = richTextBox1.Text.IndexOf(doc3, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(html))
                {
                    richTextBox1.Find(html, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(html, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(body))
                {
                    richTextBox1.Find(body, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(body, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(pre))
                {
                    richTextBox1.Find(pre, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(pre, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(space))
                {
                    richTextBox1.Find(space, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(space, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(pre1))
                {
                    richTextBox1.Find(pre1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(pre1, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(body1))
                {
                    richTextBox1.Find(body1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(body1, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(html1))
                {
                    richTextBox1.Find(html1, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    //richTextBox1.SelectionFont = new Font("Tahoma", 11, FontStyle.Bold);
                    index = richTextBox1.Text.IndexOf(html1, index) + 1;
                }

                //remove string
                while (index < richTextBox1.Text.LastIndexOf(sle))
                {
                    richTextBox1.Find(sle, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    //richTextBox1.SelectionBackColor = Color.Black;
                    richTextBox1.SelectedText = "";
                    index = richTextBox1.Text.IndexOf(sle, index) + 1;
                }
            }
            Test_LogFile("RichTextBox_result:  " + file);
        }



        /// <summary>
        /// File_value()
        /// </summary>
        ///       
        string BaseDirectory1;
        string targetDirectory1;
        string targetDirectory2;
        string targetDirectory_GO_1;
        string targetDirectory_HP_1;
        string targetDirectory_LE_1;
        string targetDirectory_DE_1;
        string targetDirectory_MSFT_1;

        string targetDirectory_GO_1_base;
        string targetDirectory_HP_1_base;
        string targetDirectory_LE_1_base;
        string targetDirectory_DE_1_base;
        string targetDirectory_MSFT_1_base;

        string targetDirectory_GO_2;
        string targetDirectory_HP_2;
        string targetDirectory_LE_2;
        string targetDirectory_DE_2;
        string targetDirectory_MSFT_2;
        string if_file_exist;
        public void File_value()
        {
            try
            {
                //Vulcan + else
                #region

                //vulcan
                #region
                if (Project == "Vulcan")
                {
                    //Vulcan_Version == "Performance_Version"
                    #region
                    if (Vulcan_Version == "Performance_Version")
                    {
                        //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\Performance_Version\AO041VCP\vulcan_perf_BOT
                        //Base_FW
                        BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_perf_BOT";

                        //New_FW1
                        targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_perf_BOT";

                        //New_FW2
                        if (New_FW2 != "")
                        { 
                            targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_BOT";
                        }

                        Test_LogFile("File_value()" + "\n" + "Project: " + Project + "\n" + "Vulcan_Version: " + Vulcan_Version + "\n");
                        if (Key == "customer")
                        {
                            //customer
                            //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\    Firmware\Releases\Official_Builds\Performance_Version\AO041VCP\vulcan_perf_BOT
                            targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\GO\";
                            targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\HP\";
                            targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\LE\";
                            targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\DE\";
                            targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                            targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\GO\";
                            targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\HP\";
                            targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\LE\";
                            targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\DE\";
                            targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                            //customer
                            if (New_FW2 != "")
                            { 
                            targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\GO\";
                            targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\HP\";
                            targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\LE\";
                            targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\DE\";
                            targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW2" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                            }
                        } //if (Key == "customer")
                        else if (Key == "EKey")
                        {

                            //EKey
                            targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_GO\";
                            targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_HP\";
                            targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_LE\";
                            targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_DE\";
                            targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_MSFT\";
                            Test_LogFile("File_value()-EKey-New_FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                            //EKey
                            if (New_FW2 != "")
                            { 
                            targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_GO\";
                            targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_HP\";
                            targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_LE\";
                            targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_DE\";
                            targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_MSFT\";
                            Test_LogFile("File_value()-EKey-New_FW2" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                            }
                        }

                    }
                    #endregion

                    //Vulcan_Version == "Debug_Version"
                    #region
                    if (Vulcan_Version == "Debug_Version")
                    {
                        //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\Debug_Version\AO041VCN\vulcan_BOT
                        //Base_FW
                        BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_BOT";

                        //New_FW1
                        targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_BOT";

                        //New_FW2
                        targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_BOT";

                        Test_LogFile("File_value()" + "\n" + "Project: " + Project + "\n" + "Vulcan_Version: " + Vulcan_Version + "\n");
                    }
                    #endregion

                    //Vulcan_Version == "Performance_Version_Pre_RDT"
                    #region
                    if (Vulcan_Version == "Performance_Version_Pre_RDT")
                    {
                        //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\Performance_Version_Pre_RDT\AR041VCP\vulcan_perf_pre_rdt_BOT
                        //Base_FW
                        //BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_perf_pre_rdt_BOT";

                        //New_FW1
                        //targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_perf_pre_rdt_BOT";

                        //New_FW2
                        //targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT";

                        //Test_LogFile("File_value()" + "\n" + "Project: " + Project + "\n" + "Vulcan_Version: " + Vulcan_Version + "\n");

                        if (Key == "customer_Pre_RDT")
                        {
                            //customer_Pre_RDT-New_FW1
                            // \\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\SED_Performance_Version_Pre_RDT\AR043VCO\vulcan_sed_perf_pre_rdt_BOT
                            targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\GO\";
                            targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\HP\";
                            targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\LE\";
                            targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\DE\";
                            targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_perf_pre_rdt_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);


                            targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_perf_pre_rdt_BOT\GO\";
                            targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_perf_pre_rdt_BOT\HP\";
                            targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_perf_pre_rdt_BOT\LE\";
                            targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_perf_pre_rdt_BOT\DE\";
                            targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_perf_pre_rdt_BOT\MSFT\";
                            Test_LogFile("File_value()-customer_Pre_RDT-New_FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                            //customer_Pre_RDT-New_FW2
                            if (New_FW2 != "")
                            { 
                            targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\GO\";
                            targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\HP\";
                            targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\LE\";
                            targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\DE\";
                            targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\MSFT\";
                            Test_LogFile("File_value()-customer_Pre_RDT-New_FW2" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                            }
                        } //if (Key == "customer_Pre_RDT")
                    }
                    #endregion

                    //Vulcan_Version == "SED_Performance_Version"
                    #region
                    if (Vulcan_Version == "SED_Performance_Version")
                    {
                        //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\SED_Performance_Version\AO041VCO\vulcan_sed_perf_BOT
                        //Base_FW
                        BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_sed_perf_BOT";

                        //New_FW1
                        targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_perf_BOT";

                        //New_FW2
                        if (New_FW2 != "")
                        { 
                         targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_sed_perf_BOT";
                        }
                        Test_LogFile("File_value()" + "\n" + "Project: " + Project + "\n" + "Vulcan_Version: " + Vulcan_Version + "\n");
                        if (Key == "customer")
                        {
                            //customer
                            //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\    Firmware\Releases\Official_Builds\Performance_Version\AO041VCP\vulcan_perf_BOT
                            targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\GO\";
                            targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\HP\";
                            targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\LE\";
                            targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\DE\";
                            targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                            targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_sed_perf_BOT\GO\";
                            targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_sed_perf_BOT\HP\";
                            targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_sed_perf_BOT\LE\";
                            targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_sed_perf_BOT\DE\";
                            targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_sed_perf_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                            //customer
                            if (New_FW2 != "")
                            { 
                            targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_sed_perf_BOT\GO\";
                            targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_sed_perf_BOT\HP\";
                            targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_sed_perf_BOT\LE\";
                            targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_sed_perf_BOT\DE\";
                            targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_sed_perf_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW2" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                            }
                        } //if (Key == "customer")

                        else if (Key == "EKey")
                        {

                            //EKey
                            targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_GO\";
                            targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_HP\";
                            targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_LE\";
                            targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_DE\";
                            targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\" + @"vulcan_perf_BOT\Ekey_MSFT\";
                            Test_LogFile("File_value()-EKey-New_FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                            //EKey
                            if (New_FW2 != "")
                            { 
                            targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_GO\";
                            targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_HP\";
                            targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_LE\";
                            targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_DE\";
                            targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\" + @"vulcan_perf_BOT\Ekey_MSFT\";
                            Test_LogFile("File_value()-EKey-New_FW2" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                            }
                        }
                    }
                    #endregion

                    //Vulcan_Version == "SED_Debug_Version"
                    #region
                    if (Vulcan_Version == "SED_Debug_Version")
                    {
                        //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\SED_Debug_Version\AO041VCS\vulcan_sed_BOT
                        //Base_FW
                        BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_sed_BOT";

                        //New_FW1
                        targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_BOT";

                        //New_FW2
                        targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_sed_BOT";

                        Test_LogFile("File_value()" + "\n" + "Project: " + Project + "\n" + "Vulcan_Version: " + Vulcan_Version + "\n");
                    }
                    #endregion

                    //Vulcan_Version == "SED_Performance_Version_Pre_RDT"
                    #region
                    if (Vulcan_Version == "SED_Performance_Version_Pre_RDT")
                    {
                        //\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\SED_Performance_Version_Pre_RDT\AR041VCO\vulcan_sed_perf_pre_rdt_BOT
                        //Base_FW
                        //BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_sed_perf_pre_rdt_BOT";

                        //New_FW1
                        //targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_perf_pre_rdt_BOT";

                        //New_FW2
                        //targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_sed_perf_pre_rdt_BOT";

                        //Test_LogFile("File_value()" + "\n" + "Project: " + Project + "\n" + "Vulcan_Version: " + Vulcan_Version + "\n");
                        if (Key == "customer_Pre_RDT")
                        {
                            //customer_Pre_RDT-New_FW1
                            // \\\10.0.56.14\Images\PlatformTesting\FW_Versions\Vulcan\Firmware\Releases\Official_Builds\Performance_Version_Pre_RDT\AR041VCP\vulcan_perf_pre_rdt_BOT
                            targetDirectory_GO_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\GO\";
                            targetDirectory_HP_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\HP\";
                            targetDirectory_LE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\LE\";
                            targetDirectory_DE_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\DE\";
                            targetDirectory_MSFT_1_base = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + @"Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\" + @"vulcan_sed_perf_pre_rdt_BOT\MSFT\";
                            Test_LogFile("File_value()-Customer_NEW-FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);


                            targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_perf_pre_rdt_BOT\GO\";
                            targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_perf_pre_rdt_BOT\HP\";
                            targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_perf_pre_rdt_BOT\LE\";
                            targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW1 + @"\vulcan_sed_perf_pre_rdt_BOT\DE\";
                            targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + Base_FW + @"\vulcan_sed_perf_pre_rdt_BOT\MSFT\";
                            Test_LogFile("File_value()-customer_Pre_RDT-New_FW1" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                            //customer_Pre_RDT-New_FW2
                            if (New_FW2 != "")
                            {
                                targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\GO\";
                                targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\HP\";
                                targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\LE\";
                                targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\DE\";
                                targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\Firmware\Releases\Official_Builds\" + Vulcan_Version + @"\" + New_FW2 + @"\vulcan_perf_pre_rdt_BOT\MSFT\";
                                Test_LogFile("File_value()-customer_Pre_RDT-New_FW2" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                            }
                        } //if (Key == "customer_Pre_RDT")

                    }
                    #endregion
                }
                #endregion
                //#########################################################################################################
                //else-vulcan= calypso/athena
                #region
                else   //CALYPSO/ATHENA
                {
                    //\\10.0.56.14\Images\PlatformTesting\FW_Versions\calypso_ix_slc_perf\AO025I2P\_out\calypso_ix_slc_perf\BOT   
                    //Base_FW
                    BaseDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + Base_FW + @"\_out\" + Project + @"\BOT\";

                    //New_FW1
                    targetDirectory1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\";

                    //New_FW2
                    targetDirectory2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\";
                    Test_LogFile("File_value()" + "\n" + "Base: " + BaseDirectory1 + "\n" + "Target 1: " + targetDirectory1 + "\n" + "Target 2: " + targetDirectory2);

                    if (Key == "customer")
                    {
                        //customer
                        targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\GO\";
                        targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\HP\";
                        targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\LE\";
                        targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\DE\";
                        targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\MSFT\";
                        Test_LogFile("File_value()" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);

                        //customer
                        targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\GO\";
                        targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\HP\";
                        targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\LE\";
                        targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\DE\";
                        targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\MSFT\";
                        Test_LogFile("File_value()" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);

                    } //if (Key == "customer")
                    else if (Key == "EKey")
                    {
                        //EKey
                        targetDirectory_GO_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\EKey_GO\";
                        targetDirectory_HP_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\EKey_HP\";
                        targetDirectory_LE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\EKey_LE\";
                        targetDirectory_DE_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\EKey_DE\";
                        targetDirectory_MSFT_1 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW1 + @"\_out\" + Project + @"\BOT\EKey_MSFT\";
                        Test_LogFile("File_value()" + "\n" + "Project: " + Key + "\n" + "Target GO 1: " + targetDirectory_GO_1 + "\n" + "Target HP 1: " + targetDirectory_HP_1 + "\n" + "Target LE 1: " + targetDirectory_LE_1 + "\n" + "Target DE 1: " + targetDirectory_DE_1 + "\n" + "Target MSFT 1: " + targetDirectory_MSFT_1);
                        //EKey
                        targetDirectory_GO_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\EKey_GO\";
                        targetDirectory_HP_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\EKey_HP\";
                        targetDirectory_LE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\EKey_LE\";
                        targetDirectory_DE_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\EKey_DE\";
                        targetDirectory_MSFT_2 = @"\\" + Network + @"\Images\PlatformTesting\FW_Versions\" + Project + @"\" + New_FW2 + @"\_out\" + Project + @"\BOT\EKey_MSFT\";
                        Test_LogFile("File_value()" + "\n" + "Project: " + Key + "\n" + "Target GO 2: " + targetDirectory_GO_2 + "\n" + "Target HP 2: " + targetDirectory_HP_2 + "\n" + "Target LE 2: " + targetDirectory_LE_2 + "\n" + "Target DE 2: " + targetDirectory_DE_2 + "\n" + "Target MSFT 2: " + targetDirectory_MSFT_2);
                    }
                }//if 
                #endregion

                #endregion

            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Values Error" + ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
            Test_LogFile("File_value()" + " , Project: " + Project);
        }




        /// <summary>
        /// looking for fluf file into targetDirectory that send to function, and return full path to return_fluf string
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// 
        string return_fluf;
        string return_fluf_check;
        public void  Get_fluf(string targetDirectory)
        {
            return_fluf = "";
            ////Get LatestFile in a directory
            //string targetDirectory = @"\\10.0.56.14\Images\PlatformTesting\FW_Versions\calypso_perf\AO091C2P\_out\calypso_perf\BOT\GO\";
            //var directory = new DirectoryInfo(targetDirectory); // 
            //var LatestFile = (from f in directory.GetFiles() orderby f.LastWriteTime descending select f).First();
            //string contents = File.ReadAllText(LatestFile.ToString());   //Read contents 
            //MessageBox.Show(LatestFile.ToString());
            try
            {
                //if (File.Exists(targetDirectory))
                //{  
                //if (if_file_exist.ToString() != "error")
               // { 
                string[] fileEntries1 = Directory.GetFiles(targetDirectory, "*CFGenc*.fluf"); //look for file contain Cfgenc and File extension .fluf
                foreach (string fileName in fileEntries1)
                {
                    if (!(fileName == ""))
                    {
                        //MessageBox.Show(fileName.ToString());
                        return_fluf = fileName.ToString();
                        break; // cancle this if want to show all fluf files into directory
                    }
                        else  { return_fluf_check = ""; }
               }

                if (File.Exists(return_fluf))
                {
                    return_fluf_check = return_fluf;
                } else { return_fluf_check = ""; }
                //}
                //}//if (File.Exists(targetDirectory))
                // else { return_fluf_check = "";   }
            } //try
            catch (Exception ex)
            {
                MessageBox.Show("Error Start Button: Could not find Wdckit.exe: " + ex.Message);
                LogFile("Get_fluf(string targetDirectory) - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }

        }


        /// <summary>
        /// read all text file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string Read_all_txt_file(string file)
        {
            string output;
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\";
            if (Directory.Exists(basePath1))
            {
                if (File.Exists(file))
                {
                    var OpenFile = new StreamReader(file);
                    output = OpenFile.ReadToEnd();
                    return output;
                }
            }
            return null;
        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Automation logs open 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void automationLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Environment.CurrentDirectory + "\\" + "Logs"+ "\\" ;
            Process.Start(filePath);
        }


        /// <summary>
        /// Manual logs open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manualLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Environment.CurrentDirectory + "\\" + "Logs" + "\\" + "Manual" + "\\";
            Process.Start(filePath);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //string source = @"\\iky-op-fpgcss01.wdc.com\fpgcss_ci\vulcan\Firmware\Releases\Test_Official_Builds\60ba7334\PETE_ZIP_Cust_PRE_RDT\73Z40001\vulcan_perf_pre_rdt\vulcan_perf_pre_rdt_BOT";
            //string data = getBetween(source, "73Z40", "01");

            //MessageBox.Show(data);
        }


        /// <summary>
        /// search for string inside string (if sed non sed) 
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }



    }
}
