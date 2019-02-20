using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Diagnostics;

namespace Laverna
{
    public partial class LavernaMainFrame : Form
    {
        public LavernaMainFrame()
        {
            InitializeComponent();
            CreateStartINI();
            InstallProGSY();

        }
        //Globale Variablen
        
        string iniPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ProGSY\ProGSY.ini";
        
        private void CreateStartINI()
        {
            string desktopFolderPath;
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Directory.CreateDirectory(documentsFolderPath + @"\ProGSY");
            INIHandler ini = new INIHandler(iniPath);
            if (!File.Exists(iniPath))
            {   // create start INI

                ini.IniWriteValue("Settings", "VersionProGSY", Application.ProductVersion);
                ini.IniWriteValue("Settings", "LaufendeGSY", @"K:\GSM\LaufendeGSYs\");
                ini.IniWriteValue("Settings", "Analysis", @"\\area1.eurofins.local\de\de18dfs01\de18_gs\Analysis\");
                ini.IniWriteValue("Settings", "ToolsPath", documentsFolderPath + @"\ProGSY\Tools");
                ini.IniWriteValue("Settings", "DatabasePath", documentsFolderPath + @"\ProGSY\Database");
            }
            else
            {
                if (ini.IniReadValue("Settings", "VersionProGSY") != Application.ProductVersion)
                {
                    File.Delete(iniPath); // Delete the existing file if exists
                    CreateStartINI();
                }
            }
        }
        private void ResetINIFile()
        {
            string settingINI = @"ProGSY.ini";
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            File.Delete(documentsFolderPath + @"\ProGSY\" + settingINI);
            CreateStartINI();
        }
        private void InstallProGSY()
            //Creating all Folders and Exporting Databasefiles
        {
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            INIHandler ini = new INIHandler(iniPath);
            Directory.CreateDirectory(ini.IniReadValue("Settings", "ToolsPath"));
            Directory.CreateDirectory(ini.IniReadValue("Settings", "DatabasePath"));
            
            string EnzymeDatabasePath = ini.IniReadValue("Settings", "DatabasePath") + @"\EnzymeDatabase.txt";
            if (!File.Exists(EnzymeDatabasePath))
            {
                //File.WriteAllBytes(EnzymeDatabasePath, FileStorage.EnzymeDatabase);
                File.WriteAllText(EnzymeDatabasePath, FileStorage.EnzymeDatabase);
                //File.Copy(FileStorage.EnzymeDatabase, EnzymeDatabasePath);
            }

        }
        







        private void button1_Click(object sender, EventArgs e)
        {
            SettingsForm form2 = new SettingsForm();
            //form2.ShowDialog(); //Modal
            form2.Show();       //not Modal
        }






        //Menu Buttons****************************************************************************************************************************
        //Menu Buttons Functions******************************************************************************************************************







        //Menu Buttons Tools**********************************************************************************************************************
        private void sequenceGrabberToolStripMenuItem_Click(object sender, EventArgs e)
        //Menu Tools>SequenceGrabber
        {
            //Prüfen ob Grabber schon in dem Ordner,wenn nein aus Rescources erstellen
            INIHandler ini = new INIHandler(iniPath);
            string progPath = ini.IniReadValue("Settings", "ToolsPath") + @"\Grabber.exe";
            if (!File.Exists(progPath))
            {
                File.WriteAllBytes(progPath, FileStorage.Sequence_Grabber_V_0_1_0_2);
            }
            Process.Start(progPath);

        }
        private void sESequenceModifierToolStripMenuItem_Click(object sender, EventArgs e)
        //Menu Tools>SESequenceModifier
        {
            //Prüfen ob Grabber schon in dem Ordner,wenn nein aus Rescources erstellen
            INIHandler ini = new INIHandler(iniPath);
            string progPath = ini.IniReadValue("Settings", "ToolsPath") + @"\SE Sequence Modifier.exe";
            if (!File.Exists(progPath))
            {
                File.WriteAllBytes(progPath, FileStorage.SE_Sequence_Modifier__Vers__1_0_1_0_);
            }
            Process.Start(progPath);
        }

        private void strandPrimerDesignerToolStripMenuItem_Click(object sender, EventArgs e)
        //Menu Tools>StrandPrimerDesigner
        {
            //Prüfen ob Grabber schon in dem Ordner,wenn nein aus Rescources erstellen
            INIHandler ini = new INIHandler(iniPath);
            string progPath = ini.IniReadValue("Settings", "ToolsPath") + @"\Strand Primer Designer.exe";
            if (!File.Exists(progPath))
            {
                File.WriteAllBytes(progPath, FileStorage.StrandPrimerDesignerV1_0_0_0);
            }
            Process.Start(progPath);
        }

        

        //Menu Buttons Settings*******************************************************************************************************************
        private void resetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        //Menu Settings>ResetINIFile
        {
            ResetINIFile();
        }





        //Menu Buttons About**********************************************************************************************************************
        private void versionsToolStripMenuItem_Click(object sender, EventArgs e)
        //Menu  Abaout>Versions
        {
            string ProGSY = "Vers. 0.0.0.1";
            string SeSequenceModifier = "Vers. 1.0.1.0";
            string SequenceGrabber = "Vers. 0.1.0.3";
            string StrandPrimerDesigner = "Vers. 1.0.0.0";
            string versionsMessage = String.Format("ProGSY: {0}" + "\n" +
                "SE Sequence Modifier: {1} " + "\n" +
                "Sequence Grabber: {2} " + "\n" +
                "Strand Primer Designer: {3} " + "\n" +
                "", ProGSY, SeSequenceModifier, SequenceGrabber, StrandPrimerDesigner);
            MessageBox.Show(versionsMessage, "Versions", MessageBoxButtons.OK);
        }


    }
}
