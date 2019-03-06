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
     public partial class ProGSYMainFrame : Form
    {
        public ProGSYMainFrame()
        {
            InitializeComponent();
            InstallProGSY();
            LoadSubclonig();
            // Methode für Check Ob Revison fällig, bzw Counter für letzten 30 Tage anzeigen
            
        }







        //Globale Variablen


        public static string desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string ProGSYInstallPath = documentsFolderPath + @"\ProGSY";
        public static string iniPath = ProGSYInstallPath + @"\ProGSY.ini";
        public static DateTime nextRevisionDate = DateTime.Now;
        

        private void CreateStartINI()
        {
            INIHandler ini = new INIHandler(iniPath);
            if (!File.Exists(iniPath))
            {   // create start INI

                ini.IniWriteValue("Settings", "VersionProGSY", Application.ProductVersion);
                ini.IniWriteValue("Settings", "LaufendeGSY", @"K:\GSM\LaufendeGSYs\");
                ini.IniWriteValue("Settings", "Analysis", @"\\area1.eurofins.local\de\de18dfs01\de18_gs\Analysis\");
                ini.IniWriteValue("Settings", "ToolsPath", documentsFolderPath + @"\ProGSY\Tools");
                ini.IniWriteValue("Settings", "DatabasePath", documentsFolderPath + @"\ProGSY\Database");
                ini.IniWriteValue("Settings", "EnzymeDatabasePath", ini.IniReadValue("Settings", "DatabasePath") + @"\EnzymeDatabase.txt");
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
            File.Delete(documentsFolderPath + @"\ProGSY\" + settingINI);
            CreateStartINI();
        }

        private void InstallProGSY()
            //Creating all Folders and Exporting Databasefiles
        {
            
            INIHandler ini = new INIHandler(iniPath);
            Directory.CreateDirectory(ProGSYInstallPath);

            CreateStartINI();

            Directory.CreateDirectory(ini.IniReadValue("Settings", "ToolsPath"));
            Directory.CreateDirectory(ini.IniReadValue("Settings", "DatabasePath"));
            
            string EnzymeDatabasePath = ini.IniReadValue("Settings", "EnzymeDatabasePath");
            if (!File.Exists(EnzymeDatabasePath))
            {
                File.WriteAllText(EnzymeDatabasePath, FileStorage.EnzymeDatabase);
            }


        }
        

        private void LoadSubclonig()
        {
            {
                List<string> ListEnzymeDatabase = new List<string>(), ListEnzymeNames = new List<string>();
                ListEnzymeDatabase = FileHandler.GetListRestrictionEnzymeDatabase();

                ListEnzymeNames = FileHandler.GetListRestrictionEnzymeNames(ListEnzymeDatabase);

                for (var i = 0; i <= ListEnzymeNames.Count - 1; i++)
                {
                    ;
                    
                    lbSU5VectorEnzym.Items.Add(ListEnzymeNames[i]);
                    lbSU3VectorEnzym.Items.Add(ListEnzymeNames[i]);
                    lbSU5InsertEnzym.Items.Add(ListEnzymeNames[i]);
                    lbSU3InsertEnzym.Items.Add(ListEnzymeNames[i]);

                    //LB5VectorEnzymeLIC.Items.Add(ListEnzymeNames[i]); //GIBSON
                    //LB3VectorEnzymeLIC.Items.Add(ListEnzymeNames[i]);//GIBSON
                    //LB5InsertEnzymeLIC.Items.Add(ListEnzymeNames[i]);//GIBSON
                    //LB3InsertEnzymeLIC.Items.Add(ListEnzymeNames[i]);//GIBSON
                }

                lbSU5VectorEnzym.SelectedIndex = 0;
                lbSU3VectorEnzym.SelectedIndex = 0;
                lbSU5InsertEnzym.SelectedIndex = 0;
                lbSU3InsertEnzym.SelectedIndex = 0;

                //LB5VectorEnzymeLIC.SelectedIndex = 0;//GIBSON
                //LB3VectorEnzymeLIC.SelectedIndex = 0;//GIBSON
                //LB5InsertEnzymeLIC.SelectedIndex = 0;//GIBSON
                //LB3InsertEnzymeLIC.SelectedIndex = 0;//GIBSON

                //CBUseSameSitesLIC.CheckState = CheckState.Checked;//GIBSON

                cbSuSameSites.CheckState = CheckState.Checked;
            }


        }





        private void button1_Click(object sender, EventArgs e)
            // TEST AUF BUTTON 1
        {
            SettingsForm form2 = new SettingsForm();
            //form2.ShowDialog(); //Modal
            //form2.Show();       //not Modal
            
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
                "Next revision: {4} " + "\n" +
                "", ProGSY, SeSequenceModifier, SequenceGrabber, StrandPrimerDesigner, nextRevisionDate.ToLongDateString());
            MessageBox.Show(versionsMessage, "Versions", MessageBoxButtons.OK);
        }







        private void bSuCreate_Click(object sender, EventArgs e)
        //  BUTTON STart Subcloníng
        {
            StartSubcloning();
        }











        //BMI 

     


        public void StartSubcloning()
        {
            if (DNAHandler.IsValidDNASequence(rtbSuVectorInput.Text) == false)
                MessageBox.Show("Bitte geben sie eine gültige DNA Sequenz für den Vektor ein.","", MessageBoxButtons.OK);
            else if (DNAHandler.IsValidDNASequence(rtbSuInsertInput.Text) == false)
                MessageBox.Show("Bitte geben sie eine gültige DNA Sequenz für das Insert ein.", "", MessageBoxButtons.OK);
            else if (DNAHandler.IsValidGenename(rtbSuGeneName.Text) == false)
                MessageBox.Show("Bitte geben sie einen gültigen Gennamen ein.", "", MessageBoxButtons.OK);
            else if (DNAHandler.IsValidGenename(rtbSuVectorName.Text) == false)
                MessageBox.Show("Bitte geben sie einen gültigen Vektornamen ein.", "", MessageBoxButtons.OK);
            else if (cbSuSameSites.CheckState == CheckState.Checked)
                Subcloning.CreateInSilicoSequenceWithSameSitesForVectorAndInsert(rtbSuVectorName.Text, rtbSuVectorInput.Text, rtbSuGeneName.Text, rtbSuInsertInput.Text, lbSU5VectorEnzym.SelectedIndex, lbSU3VectorEnzym.SelectedIndex);
            else
                Subcloning.CreateInSilicoSequenceWithDifferentSitesForVectorAndInsert(rtbSuVectorName.Text, rtbSuVectorInput.Text, rtbSuGeneName.Text, rtbSuInsertInput.Text, lbSU5VectorEnzym.SelectedIndex, lbSU3VectorEnzym.SelectedIndex, lbSU5InsertEnzym.SelectedIndex, lbSU3InsertEnzym.SelectedIndex);
        }


        public void SubkloErrorKorr1()
        {

            //"Die von Ihnen ausgewählten Restriktionsenzyme erzeugen kompatible Überhänge. Für die Erstellung einer in silico Sequenz ist jedoch ein reverse complement der Insert Sequenz und einige Einstellungen notwendig. Soll dies automatisch vorgenommen werden?", "", MessageBoxButtons.YesNo);
            rtbSuInsertInput.Text = DNAHandler.revstr(rtbSuInsertInput.Text);
            cbSuSameSites.CheckState = CheckState.Unchecked;
            lbSU5InsertEnzym.SelectedIndex = lbSU3VectorEnzym.SelectedIndex;
            lbSU3InsertEnzym.SelectedIndex = lbSU5VectorEnzym.SelectedIndex;            
        }
        public void SubkloErrorKorr2()
        {
            rtbSuVectorInput.Text = DNAHandler.revstr(rtbSuVectorInput.Text);
        }

        public void SubkloErrorKorr3()
        {
            rtbSuInsertInput.Text = DNAHandler.revstr(rtbSuInsertInput.Text);
        }

        private void cbSuSameSites_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSuSameSites.CheckState==CheckState.Checked)
            {
                lbSU5InsertEnzym.Enabled = false;
                lbSU3InsertEnzym.Enabled = false;
                label6.Enabled = false;
                label7.Enabled = false;
                label10.Enabled = false;

            }
            else
            {
                lbSU5InsertEnzym.Enabled = true;
                lbSU3InsertEnzym.Enabled = true;
                label6.Enabled = true;
                label7.Enabled = true;
                label10.Enabled = true;
            }
        }

        private void bSuClear_Click(object sender, EventArgs e)
        {
            cbSuSameSites.CheckState = CheckState.Checked;
            rtbSuVectorInput.Text = "";
            rtbSuInsertInput.Text = "";
            rtbSuVectorName.Text = "";
            rtbSuGeneName.Text = "";
            lbSU3InsertEnzym.SelectedIndex = 0;
            lbSU3VectorEnzym.SelectedIndex = 0;
            lbSU5InsertEnzym.SelectedIndex = 0;
            lbSU5VectorEnzym.SelectedIndex = 0;
        }
    }
}
