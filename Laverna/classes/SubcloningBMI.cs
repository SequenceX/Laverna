using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using Laverna.Forms;
using Laverna;


namespace Laverna
    {

     class Subcloning
    {
        public static void CreateInSilicoSequenceWithSameSitesForVectorAndInsert(string Vectorname, string VectorSequence, string Genename, string InsertSequence, int Index5RestrictionEnzyme, int Index3RestrictionEnzyme)
        {
            List<string> ListOfRowsFromEnzymeDatabase = new List<string>(), ListOfEnzymeSites = new List<string>(), ListOfEnzymeNames = new List<string>();
            List<int> StartPositionsEnzyme5SiteVector = new List<int>(), StartPositionsEnzyme3SiteVector = new List<int>(), StartPositionsEnzyme5SiteInsert = new List<int>(), StartPositionsEnzyme3SiteInsert = new List<int>(), OrientationEnzyme5SiteVector = new List<int>(), OrientationEnzyme3SiteVector = new List<int>(), OrientationEnzyme5SiteInsert = new List<int>(), OrientationEnzyme3SiteInsert = new List<int>(), ListOfEnzymeCutPositionsFor = new List<int>(), ListOfEnzymeCutPositionsrev = new List<int>();
            string RestrictionEnzyme5Site, RestrictionEnzyme3Site, RestrictionEnzyme5Name, RestrictionEnzyme3Name, InSilicoSequence;
            bool FiveSitesCompatible, ThreeSitesCompatible, FiveVectorThreeInsertComaptible, ThreeVectorFiveInsertCompatible, AllSitesFound;
            var MessageAntwort = DialogResult.None;
            
            ListOfRowsFromEnzymeDatabase = FileHandler.GetListRestrictionEnzymeDatabase();
            ListOfEnzymeSites = FileHandler.GetListRestrictionEnzymeSites(ListOfRowsFromEnzymeDatabase);
            ListOfEnzymeNames = FileHandler.GetListRestrictionEnzymeNames(ListOfRowsFromEnzymeDatabase);

            RestrictionEnzyme5Site = ListOfEnzymeSites[Index5RestrictionEnzyme];
            RestrictionEnzyme3Site = ListOfEnzymeSites[Index3RestrictionEnzyme];

            RestrictionEnzyme5Name = ListOfEnzymeNames[Index5RestrictionEnzyme];
            RestrictionEnzyme3Name = ListOfEnzymeNames[Index3RestrictionEnzyme];

            // Auffinden der 5' und 3' sites in Vektor und Insert: Speichern von Startpositionen und Orientierungen
            // Problem: sites mit uneindeutigen Basen
            GetListStartPositionsAndOrientationRestrictionSiteInTarget(VectorSequence, RestrictionEnzyme5Site, ref StartPositionsEnzyme5SiteVector, ref OrientationEnzyme5SiteVector);
            GetListStartPositionsAndOrientationRestrictionSiteInTarget(VectorSequence, RestrictionEnzyme3Site, ref StartPositionsEnzyme3SiteVector, ref OrientationEnzyme3SiteVector);

            GetListStartPositionsAndOrientationRestrictionSiteInTarget(InsertSequence, RestrictionEnzyme5Site, ref StartPositionsEnzyme5SiteInsert, ref OrientationEnzyme5SiteInsert);
            GetListStartPositionsAndOrientationRestrictionSiteInTarget(InsertSequence, RestrictionEnzyme3Site, ref StartPositionsEnzyme3SiteInsert, ref OrientationEnzyme3SiteInsert);

            // Die zu benutzenden Startpositionen und Orientierungen der sites werden in den Listen unter Index 0 gespeichert.
            // Sind mehrere sites vorhanden muss der User die korrekte auswählen, die dann unter Index 0 gespeichert wird, ist eine site nicht vorhanden muss der User seine Eingabe korrigieren
            AllSitesFound = true;

            if (StartPositionsEnzyme5SiteVector.Count > 1)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme5SiteVector, ref OrientationEnzyme5SiteVector, RestrictionEnzyme5Name, "Vector sequence", "5'");
            else if (StartPositionsEnzyme5SiteVector.Count == 0)
            {
               
                MessageBox.Show("Das von Ihnen ausgewählte 5' Restriktionsenzym wurde nicht gefunden in der Vektorsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.","Error", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (StartPositionsEnzyme3SiteVector.Count > 1 & AllSitesFound == true)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme3SiteVector, ref OrientationEnzyme3SiteVector, RestrictionEnzyme3Name, "Vector sequence", "3'");
            else if (StartPositionsEnzyme3SiteVector.Count == 0 & AllSitesFound == true)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 3' Restriktionsenzym wurde nicht gefunden in der Vektorsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "Error", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (StartPositionsEnzyme5SiteInsert.Count > 1 & AllSitesFound == true)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme5SiteInsert, ref OrientationEnzyme5SiteInsert, RestrictionEnzyme5Name, "Insert sequence", "5'");
            else if (StartPositionsEnzyme5SiteInsert.Count == 0 & AllSitesFound == true)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 5' Restriktionsenzym wurde nicht gefunden in der Insertsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "Error", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (StartPositionsEnzyme3SiteInsert.Count > 1 & AllSitesFound == true)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme3SiteInsert, ref OrientationEnzyme3SiteInsert, RestrictionEnzyme3Name, "Insert sequence", "3'");
            else if (StartPositionsEnzyme3SiteInsert.Count == 0 & AllSitesFound == true)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 3' Restriktionsenzym wurde nicht gefunden in der Insertsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "Error", MessageBoxButtons.OK);
                AllSitesFound = false;
                AllSitesFound = false;
            }

            if (AllSitesFound == true)
            {

                // Sind alle sites in der richtigen Reihenfolge auf der gleichen Sequenz?
                if (StartPositionsEnzyme5SiteVector[0] <= StartPositionsEnzyme3SiteVector[0] & StartPositionsEnzyme5SiteInsert[0] < StartPositionsEnzyme3SiteInsert[0])
                {
                    FileHandler.GetListRestrictionsEnzymeCutPositions(ListOfRowsFromEnzymeDatabase, ref ListOfEnzymeCutPositionsFor,ref ListOfEnzymeCutPositionsrev);

                    // Schneiden die Enzyme nicht aus der Sequenz hinaus?
                    if (AreRestrictionSitesTooCloseToTheEnd(VectorSequence, StartPositionsEnzyme5SiteVector, StartPositionsEnzyme3SiteVector, Index5RestrictionEnzyme, Index3RestrictionEnzyme, OrientationEnzyme5SiteVector, OrientationEnzyme3SiteVector, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false & AreRestrictionSitesTooCloseToTheEnd(InsertSequence, StartPositionsEnzyme5SiteInsert, StartPositionsEnzyme3SiteInsert, Index5RestrictionEnzyme, Index3RestrictionEnzyme, OrientationEnzyme5SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false)
                    {

                        // Haben die sites auf der gleichen Sequenz genügend Abstand voneinander (Bsp. downstream cutter am 5'Ende schneidet über die 3' site hinweg)? 
                        if (AreRestrictionSitesOverlapping(VectorSequence, StartPositionsEnzyme5SiteVector, StartPositionsEnzyme3SiteVector, Index5RestrictionEnzyme, Index3RestrictionEnzyme, OrientationEnzyme5SiteVector, OrientationEnzyme3SiteVector, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false & AreRestrictionSitesOverlapping(InsertSequence, StartPositionsEnzyme5SiteInsert, StartPositionsEnzyme3SiteInsert, Index5RestrictionEnzyme, Index3RestrictionEnzyme, OrientationEnzyme5SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false)
                        {

                            // Sind die Überhänge kompatibel zueinander
                            FiveSitesCompatible = AreRestrictionSitesCompatible(VectorSequence, InsertSequence, Index5RestrictionEnzyme, Index5RestrictionEnzyme, StartPositionsEnzyme5SiteVector, OrientationEnzyme5SiteVector, StartPositionsEnzyme5SiteInsert, OrientationEnzyme5SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);
                            ThreeSitesCompatible = AreRestrictionSitesCompatible(VectorSequence, InsertSequence, Index3RestrictionEnzyme, Index3RestrictionEnzyme, StartPositionsEnzyme3SiteVector, OrientationEnzyme3SiteVector, StartPositionsEnzyme3SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);

                            if (FiveSitesCompatible == true & ThreeSitesCompatible == true)
                            {

                                // Verbinden von Vektor und Insert
                                InSilicoSequence = GetInSilicoSequence(VectorSequence, InsertSequence, StartPositionsEnzyme5SiteVector, StartPositionsEnzyme5SiteInsert, StartPositionsEnzyme3SiteVector, StartPositionsEnzyme3SiteInsert, Index5RestrictionEnzyme, Index5RestrictionEnzyme, Index3RestrictionEnzyme, Index3RestrictionEnzyme, OrientationEnzyme5SiteVector, OrientationEnzyme5SiteInsert, OrientationEnzyme3SiteVector, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);

                                // Schreiben der Output Datei
                                WriteInSilicoSequence(Vectorname, Genename, InSilicoSequence);
                            }
                            else
                            {
                                FiveVectorThreeInsertComaptible = AreRestrictionSitesCompatibleAfterAutoFlip(VectorSequence, InsertSequence, Index5RestrictionEnzyme, Index3RestrictionEnzyme, StartPositionsEnzyme5SiteVector, OrientationEnzyme5SiteVector, StartPositionsEnzyme3SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);
                                ThreeVectorFiveInsertCompatible = AreRestrictionSitesCompatibleAfterAutoFlip(VectorSequence, InsertSequence, Index3RestrictionEnzyme, Index5RestrictionEnzyme, StartPositionsEnzyme3SiteVector, OrientationEnzyme3SiteVector, StartPositionsEnzyme5SiteInsert, OrientationEnzyme5SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);

                                if (FiveVectorThreeInsertComaptible == true & ThreeVectorFiveInsertCompatible == true)
                                {
                                    MessageAntwort = MessageBox.Show("Die von Ihnen ausgewählten Restriktionsenzyme erzeugen kompatible Überhänge. Für die Erstellung einer in silico Sequenz ist jedoch ein reverse complement der Insert Sequenz und einige Einstellungen notwendig. Soll dies automatisch vorgenommen werden?", "", MessageBoxButtons.YesNo);
                                    if (MessageAntwort == DialogResult.Yes)
                                    {
                                        CreateInSilicoSequenceWithDifferentSitesForVectorAndInsert( Vectorname,  VectorSequence,  Genename,  DNAHandler.revstr(InsertSequence),  Index5RestrictionEnzyme,  Index3RestrictionEnzyme, Index3RestrictionEnzyme, Index5RestrictionEnzyme);
                                        //ProGSYMainFrame FRM1 = new ProGSYMainFrame();
                                        //FRM1.SubkloErrorKorr1();
                                        //FRM1.rtbSuInsertInput.Text = DNAHandler.revstr(FRM1.rtbSuInsertInput.Text);
                                        //FRM1.cbSuSameSites.CheckState = CheckState.Unchecked;
                                        //FRM1.lbSU5InsertEnzym.SelectedIndex = FRM1.lbSU3VectorEnzym.SelectedIndex;
                                        //FRM1.lbSU3InsertEnzym.SelectedIndex = FRM1.lbSU5VectorEnzym.SelectedIndex;
                                        //FRM1.StartSubcloning();
                                        //FRM1 = null;
                                    }
                                }
                                else
                                    MessageBox.Show("Die von Ihnen ausgewählten Restriktionsenzyme erzeugen keine kompatiblen Überhänge. Bitte überprüfen sie Ihre Eingabe und starten sie den Vorgang neu.", "", MessageBoxButtons.OK);
                            }
                        }
                        else
                            MessageBox.Show("Die von Ihnen ausgewählten Restriktionsenzyme in Vektor oder Insert erzeugen überlappende Schnitte. Bitte überprüfen sie Ihre Eingabe und starten sie den Vorgang neu.", "", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("Die von Ihnen ausgewählten Restriktionsenzyme in Vektor oder Insert schneiden über die Sequenzenden hinweg. Bitte überprüfen sie Ihre Eingabe und starten sie den Vorgang neu.", "", MessageBoxButtons.OK);
                }
                else if (StartPositionsEnzyme5SiteInsert[0] == StartPositionsEnzyme3SiteInsert[0])
                    MessageBox.Show("Durch die von Ihnen ausgewählten Restriktionsenzyme würde ein Insert mit einer Länge von 0 bp entstehen. Bitte korrigieren sie Ihre Eingabe und starten sie den Vorgang neu.", "", MessageBoxButtons.OK);
                else if (StartPositionsEnzyme5SiteVector[0] > StartPositionsEnzyme3SiteVector[0])
                {
                    MessageAntwort = MessageBox.Show("Die Reihenfolge (5' - 3') der von Ihnen ausgewählten Restriktionsenzyme stimmt nicht mit der Reihenfolge auf der Vektor Sequenz überein, sie ist genau umgekehrt. Soll automatisch ein reverse complement der Vectorsequenz durchgeführt werden?", "", MessageBoxButtons.YesNo);
                    if (MessageAntwort == DialogResult.Yes)
                    {
                        CreateInSilicoSequenceWithSameSitesForVectorAndInsert(Vectorname, DNAHandler.revstr(VectorSequence), Genename, InsertSequence, Index5RestrictionEnzyme, Index3RestrictionEnzyme);
                        //ProGSYMainFrame FRM1 = new ProGSYMainFrame();
                        //FRM1.SubkloErrorKorr2();
                        //FRM1.rtbSuVectorInput.Text = DNAHandler.revstr(FRM1.rtbSuVectorInput.Text);
                        //FRM1.StartSubcloning();
                        //FRM1 = null;
                        //FrmGeneSynthesisPro.TBVectorSubcloning.Text = DNAHandler.revstr(FrmGeneSynthesisPro.TBVectorSubcloning.Text);
                        //FrmGeneSynthesisPro.StartSubcloning();
                    }
                }
                else if (StartPositionsEnzyme5SiteInsert[0] > StartPositionsEnzyme3SiteInsert[0])
                {
                    MessageAntwort = MessageBox.Show("Die Reihenfolge (5' - 3') der von Ihnen ausgewählten Restriktionsenzyme stimmt nicht mit der Reihenfolge auf der Insert Sequenz überein, sie ist genau umgekehrt. Soll automatisch ein reverse complement der Vectorsequenz durchgeführt werden?", "", MessageBoxButtons.YesNo);
                    if (MessageAntwort == DialogResult.Yes)
                    {
                        CreateInSilicoSequenceWithSameSitesForVectorAndInsert(Vectorname, VectorSequence, Genename, DNAHandler.revstr(InsertSequence), Index5RestrictionEnzyme, Index3RestrictionEnzyme);
                        //ProGSYMainFrame FRM1 = new ProGSYMainFrame();
                        //FRM1.SubkloErrorKorr3();
                        //FRM1.rtbSuInsertInput.Text = DNAHandler.revstr(FRM1.rtbSuInsertInput.Text);
                        //FRM1.StartSubcloning();
                        //FRM1 = null;
                        //FrmGeneSynthesisPro.TBInsertSubcloning.Text = DNAHandler.revstr(FrmGeneSynthesisPro.TBInsertSubcloning.Text);
                        //FrmGeneSynthesisPro.StartSubcloning();
                    }
                }
            }
        }

        public static void CreateInSilicoSequenceWithDifferentSitesForVectorAndInsert(string Vectorname, string VectorSequence, string Genename, string InsertSequence, int Index5RestrictionEnzymeVector, int Index3RestrictionEnzymeVector, int Index5RestrictionEnzymeInsert, int Index3RestrictionEnzymeInsert)
        {
            List<string> ListOfRowsFromEnzymeDatabase = new List<string>(), ListOfEnzymeSites = new List<string>(), ListOfEnzymeNames = new List<string>();
            List<int> StartPositionsEnzyme5SiteVector = new List<int>(), StartPositionsEnzyme3SiteVector = new List<int>(), StartPositionsEnzyme5SiteInsert = new List<int>(), StartPositionsEnzyme3SiteInsert = new List<int>(), OrientationEnzyme5SiteVector = new List<int>(), OrientationEnzyme3SiteVector = new List<int>(), OrientationEnzyme5SiteInsert = new List<int>(), OrientationEnzyme3SiteInsert = new List<int>(), ListOfEnzymeCutPositionsFor = new List<int>(), ListOfEnzymeCutPositionsrev = new List<int>();
            string RestrictionEnzyme5SiteVector, RestrictionEnzyme3SiteVector, RestrictionEnzyme5SiteInsert, RestrictionEnzyme3SiteInsert, RestrictionEnzyme5VectorName, RestrictionEnzyme3VectorName, RestrictionEnzyme5InsertName, RestrictionEnzyme3InsertName, InSilicoSequence;
            bool FiveSitesCompatible, ThreeSitesCompatible, FiveVectorThreeInsertComaptible, ThreeVectorFiveInsertCompatible, AllSitesFound;
            var MessageAntwort=DialogResult.None;

            ListOfRowsFromEnzymeDatabase = FileHandler.GetListRestrictionEnzymeDatabase();

            ListOfEnzymeSites = FileHandler.GetListRestrictionEnzymeSites(ListOfRowsFromEnzymeDatabase);

            ListOfEnzymeNames = FileHandler.GetListRestrictionEnzymeNames(ListOfRowsFromEnzymeDatabase);

            RestrictionEnzyme5SiteVector = ListOfEnzymeSites[Index5RestrictionEnzymeVector];
            RestrictionEnzyme3SiteVector = ListOfEnzymeSites[Index3RestrictionEnzymeVector];
            RestrictionEnzyme5SiteInsert = ListOfEnzymeSites[Index5RestrictionEnzymeInsert];
            RestrictionEnzyme3SiteInsert = ListOfEnzymeSites[Index3RestrictionEnzymeInsert];

            RestrictionEnzyme5VectorName = ListOfEnzymeNames[Index5RestrictionEnzymeVector];
            RestrictionEnzyme3VectorName = ListOfEnzymeNames[Index3RestrictionEnzymeVector];
            RestrictionEnzyme5InsertName = ListOfEnzymeNames[Index5RestrictionEnzymeInsert];
            RestrictionEnzyme3InsertName = ListOfEnzymeNames[Index3RestrictionEnzymeInsert];

            // Auffinden der 5' und 3' sites in Vektor und Insert: Speichern von Startpositionen und Orientierungen
            // Problem: sites mit uneindeutigen Basen
            GetListStartPositionsAndOrientationRestrictionSiteInTarget(VectorSequence, RestrictionEnzyme5SiteVector, ref StartPositionsEnzyme5SiteVector, ref OrientationEnzyme5SiteVector);
            GetListStartPositionsAndOrientationRestrictionSiteInTarget(VectorSequence, RestrictionEnzyme3SiteVector, ref StartPositionsEnzyme3SiteVector, ref OrientationEnzyme3SiteVector);

            GetListStartPositionsAndOrientationRestrictionSiteInTarget(InsertSequence, RestrictionEnzyme5SiteInsert, ref StartPositionsEnzyme5SiteInsert, ref OrientationEnzyme5SiteInsert);
            GetListStartPositionsAndOrientationRestrictionSiteInTarget(InsertSequence, RestrictionEnzyme3SiteInsert, ref StartPositionsEnzyme3SiteInsert, ref OrientationEnzyme3SiteInsert);

            // Die zu benutzenden Startpositionen und Orientierungen der sites werden in den Listen unter Index 0 gespeichert.
            // Sind mehrere sites vorhanden muss der User die korrekte auswählen, die dann unter Index 0 gespeichert wird, ist eine site nicht vorhanden muss der User seine Eingabe korrigieren
            AllSitesFound = true;

            if (StartPositionsEnzyme5SiteVector.Count > 1)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme5SiteVector, ref OrientationEnzyme5SiteVector, RestrictionEnzyme5VectorName, "Vector sequence", "5'");
            else if (StartPositionsEnzyme5SiteVector.Count == 0)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 5' Restriktionsenzym wurde nicht gefunden in der Vektorsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (StartPositionsEnzyme3SiteVector.Count > 1 & AllSitesFound == true)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme3SiteVector, ref OrientationEnzyme3SiteVector, RestrictionEnzyme3VectorName, "Vector sequence", "3'");
            else if (StartPositionsEnzyme3SiteVector.Count == 0 & AllSitesFound == true)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 3' Restriktionsenzym wurde nicht gefunden in der Vektorsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (StartPositionsEnzyme5SiteInsert.Count > 1 & AllSitesFound == true)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme5SiteInsert, ref OrientationEnzyme5SiteInsert, RestrictionEnzyme5InsertName, "Insert sequence", "5'");
            else if (StartPositionsEnzyme5SiteInsert.Count == 0 & AllSitesFound == true)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 5' Restriktionsenzym wurde nicht gefunden in der Insertsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (StartPositionsEnzyme3SiteInsert.Count > 1 & AllSitesFound == true)
                GetNewListOfStartPositionsAndOrientations(ref StartPositionsEnzyme3SiteInsert, ref OrientationEnzyme3SiteInsert, RestrictionEnzyme3InsertName, "Insert sequence", "3'");
            else if (StartPositionsEnzyme3SiteInsert.Count == 0 & AllSitesFound == true)
            {
                MessageBox.Show("Das von Ihnen ausgewählte 3' Restriktionsenzym wurde nicht gefunden in der Insertsequenz. Bitte korrigieren sie Eingabe und starten sie den Vorgang von Neuem.", "", MessageBoxButtons.OK);
                AllSitesFound = false;
            }

            if (AllSitesFound == true)
            {

                // Sind alle sites in der richtigen Reihenfolge auf der gleichen Sequenz?
                if (StartPositionsEnzyme5SiteVector[0] <= StartPositionsEnzyme3SiteVector[0] & StartPositionsEnzyme5SiteInsert[0] < StartPositionsEnzyme3SiteInsert[0])
                {
                    FileHandler.GetListRestrictionsEnzymeCutPositions(ListOfRowsFromEnzymeDatabase,ref ListOfEnzymeCutPositionsFor,ref ListOfEnzymeCutPositionsrev);

                    // Schneiden die Enzyme nicht aus der Sequenz hinaus?
                    if (AreRestrictionSitesTooCloseToTheEnd(VectorSequence, StartPositionsEnzyme5SiteVector, StartPositionsEnzyme3SiteVector, Index5RestrictionEnzymeVector, Index3RestrictionEnzymeVector, OrientationEnzyme5SiteVector, OrientationEnzyme3SiteVector, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false & AreRestrictionSitesTooCloseToTheEnd(InsertSequence, StartPositionsEnzyme5SiteInsert, StartPositionsEnzyme3SiteInsert, Index5RestrictionEnzymeInsert, Index3RestrictionEnzymeInsert, OrientationEnzyme5SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false)
                    {

                        // Haben die sites auf der gleichen Sequenz genügend Abstand voneinander (Bsp. downstream cutter am 5'Ende schneidet über die 3' site hinweg)? 
                        if (AreRestrictionSitesOverlapping(VectorSequence, StartPositionsEnzyme5SiteVector, StartPositionsEnzyme3SiteVector, Index5RestrictionEnzymeVector, Index3RestrictionEnzymeVector, OrientationEnzyme5SiteVector, OrientationEnzyme3SiteVector, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false & AreRestrictionSitesOverlapping(InsertSequence, StartPositionsEnzyme5SiteInsert, StartPositionsEnzyme3SiteInsert, Index5RestrictionEnzymeInsert, Index3RestrictionEnzymeInsert, OrientationEnzyme5SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites) == false)
                        {

                            // Sind die Überhänge kompatibel zueinander
                            FiveSitesCompatible = AreRestrictionSitesCompatible(VectorSequence, InsertSequence, Index5RestrictionEnzymeVector, Index5RestrictionEnzymeInsert, StartPositionsEnzyme5SiteVector, OrientationEnzyme5SiteVector, StartPositionsEnzyme5SiteInsert, OrientationEnzyme5SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);
                            ThreeSitesCompatible = AreRestrictionSitesCompatible(VectorSequence, InsertSequence, Index3RestrictionEnzymeVector, Index3RestrictionEnzymeInsert, StartPositionsEnzyme3SiteVector, OrientationEnzyme3SiteVector, StartPositionsEnzyme3SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);

                            if (FiveSitesCompatible == true & ThreeSitesCompatible == true)
                            {

                                // Verbinden von Vektor und Insert
                                InSilicoSequence = GetInSilicoSequence(VectorSequence, InsertSequence, StartPositionsEnzyme5SiteVector, StartPositionsEnzyme5SiteInsert, StartPositionsEnzyme3SiteVector, StartPositionsEnzyme3SiteInsert, Index5RestrictionEnzymeVector, Index5RestrictionEnzymeInsert, Index3RestrictionEnzymeVector, Index3RestrictionEnzymeInsert, OrientationEnzyme5SiteVector, OrientationEnzyme5SiteInsert, OrientationEnzyme3SiteVector, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);

                                // Schreiben der Output Datei
                                WriteInSilicoSequence(Vectorname, Genename, InSilicoSequence);
                            }
                            else
                            {
                                FiveVectorThreeInsertComaptible = AreRestrictionSitesCompatibleAfterAutoFlip(VectorSequence, InsertSequence, Index5RestrictionEnzymeVector, Index3RestrictionEnzymeInsert, StartPositionsEnzyme5SiteVector, OrientationEnzyme5SiteVector, StartPositionsEnzyme3SiteInsert, OrientationEnzyme3SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);
                                ThreeVectorFiveInsertCompatible = AreRestrictionSitesCompatibleAfterAutoFlip(VectorSequence, InsertSequence, Index3RestrictionEnzymeVector, Index5RestrictionEnzymeInsert, StartPositionsEnzyme3SiteVector, OrientationEnzyme3SiteVector, StartPositionsEnzyme5SiteInsert, OrientationEnzyme5SiteInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev, ListOfEnzymeSites);

                                if (FiveVectorThreeInsertComaptible == true & ThreeVectorFiveInsertCompatible == true)
                                {
                                    MessageAntwort = MessageBox.Show("Die von Ihnen ausgewählten Restriktionsenzyme erzeugen kompatible Überhänge. Für die Erstellung einer in silico Sequenz sind jedoch ein reverse complement der Insert Sequenz und einige Einstellungen notwendig. Sollen diese Veränderungen automatisch vorgenommen werden?", "", MessageBoxButtons.YesNo);
                                    if (MessageAntwort == DialogResult.Yes)
                                    {

                                        CreateInSilicoSequenceWithDifferentSitesForVectorAndInsert(Vectorname, VectorSequence, Genename, DNAHandler.revstr(InsertSequence),  Index5RestrictionEnzymeVector,  Index3RestrictionEnzymeVector,  Index5RestrictionEnzymeInsert,  Index3RestrictionEnzymeInsert);
                                        //ProGSYMainFrame FRM1 = new ProGSYMainFrame();
                                        //FRM1.SubkloErrorKorr1();
                                        //FRM1.rtbSuInsertInput.Text = DNAHandler.revstr(FRM1.rtbSuInsertInput.Text);
                                        //FRM1.cbSuSameSites.CheckState = CheckState.Unchecked;
                                        //FRM1.lbSU5InsertEnzym.SelectedIndex = FRM1.lbSU3VectorEnzym.SelectedIndex;
                                        //FRM1.lbSU3InsertEnzym.SelectedIndex = FRM1.lbSU5VectorEnzym.SelectedIndex;
                                        //FRM1.StartSubcloning();
                                        //FRM1 = null;
                                        //FrmGeneSynthesisPro.TBInsertSubcloning.Text = DNAHandler.revstr(FrmGeneSynthesisPro.TBInsertSubcloning.Text);
                                        //FrmGeneSynthesisPro.CBUseSameSitesSubcloning.CheckState = CheckState.Unchecked;
                                        //FrmGeneSynthesisPro.LB5InsertEnzymesSubcloning.SelectedIndex = FrmGeneSynthesisPro.LB3VectorEnzymesSubcloning.SelectedIndex;
                                        //FrmGeneSynthesisPro.LB3InsertEnzmyesSubcloning.SelectedIndex = FrmGeneSynthesisPro.LB5VectorEnzymesSubcloning.SelectedIndex;
                                        //FrmGeneSynthesisPro.StartSubcloning();
                                    }
                                }
                                else
                                    Interaction.MsgBox("Die von Ihnen ausgewählten Restriktionsenzyme erzeugen keine kompatiblen Überhänge. Bitte überprüfen sie Ihre Eingabe und starten sie den Vorgang neu.", Constants.vbCritical , Constants.vbOKOnly);
                            }
                        }
                        else
                            Interaction.MsgBox("Die von Ihnen ausgewählten Restriktionsenzyme in Vektor oder Insert erzeugen überlappende Schnitte. Bitte überprüfen sie Ihre Eingabe und starten sie den Vorgang neu.", Constants.vbCritical , Constants.vbOKOnly);
                    }
                    else
                        Interaction.MsgBox("Die von Ihnen ausgewählten Restriktionsenzyme in Vektor oder Insert schneiden über die Sequenzenden hinweg. Bitte überprüfen sie Ihre Eingabe und starten sie den Vorgang neu.", Constants.vbCritical , Constants.vbOKOnly);
                }
                else if (StartPositionsEnzyme5SiteInsert[0] == StartPositionsEnzyme3SiteInsert[0])
                    Interaction.MsgBox("Durch die von Ihnen ausgewählten Restriktionsenzyme würde ein Insert mit einer Länge von 0 bp entstehen. Bitte korrigieren sie Ihre Eingabe und starten sie den Vorgang neu.", Constants.vbOKOnly , Constants.vbCritical);
                else if (StartPositionsEnzyme5SiteVector[0] > StartPositionsEnzyme3SiteVector[0])
                {
                    MessageAntwort = MessageBox.Show("Die Reihenfolge (5' - 3') der von Ihnen ausgewählten Restriktionsenzyme stimmt nicht mit der Reihenfolge auf der Vektor Sequenz überein, sie ist genau umgekehrt. Soll automatisch ein reverse complement der Vectorsequenz durchgeführt werden?", "", MessageBoxButtons.YesNo);
                    if (MessageAntwort == DialogResult.Yes)
                    {

                        CreateInSilicoSequenceWithDifferentSitesForVectorAndInsert(Vectorname, DNAHandler.revstr(VectorSequence), Genename, InsertSequence, Index5RestrictionEnzymeVector, Index3RestrictionEnzymeVector, Index5RestrictionEnzymeInsert, Index3RestrictionEnzymeInsert);
                        //ProGSYMainFrame FRM1 = new ProGSYMainFrame();
                        //FRM1.SubkloErrorKorr2();
                        //FRM1.rtbSuVectorInput.Text = DNAHandler.revstr(FRM1.rtbSuVectorInput.Text);
                        //FRM1.StartSubcloning();
                        //FRM1 = null;
                        //FrmGeneSynthesisPro.TBVectorSubcloning.Text = DNAHandler.revstr(FrmGeneSynthesisPro.TBVectorSubcloning.Text);
                        //FrmGeneSynthesisPro.StartSubcloning();
                    }
                }
                else if (StartPositionsEnzyme5SiteInsert[0] > StartPositionsEnzyme3SiteInsert[0])
                {
                    MessageAntwort = MessageBox.Show("Die Reihenfolge (5' - 3') der von Ihnen ausgewählten Restriktionsenzyme stimmt nicht mit der Reihenfolge auf der Insert Sequenz überein, sie ist genau umgekehrt. Soll automatisch ein reverse complement der Vectorsequenz durchgeführt werden?", "", MessageBoxButtons.YesNo);
                    if (MessageAntwort == DialogResult.Yes)
                    {
                        CreateInSilicoSequenceWithDifferentSitesForVectorAndInsert(Vectorname, VectorSequence, Genename, DNAHandler.revstr(InsertSequence), Index5RestrictionEnzymeVector, Index3RestrictionEnzymeVector, Index5RestrictionEnzymeInsert, Index3RestrictionEnzymeInsert);
                        //ProGSYMainFrame FRM1 = new ProGSYMainFrame();
                        //FRM1.SubkloErrorKorr3();
                        //FRM1.rtbSuInsertInput.Text = DNAHandler.revstr(FRM1.rtbSuInsertInput.Text);
                        //FRM1.StartSubcloning();
                        //FRM1 = null;
                        //FrmGeneSynthesisPro.TBInsertSubcloning.Text = DNAHandler.revstr(FrmGeneSynthesisPro.TBInsertSubcloning.Text);
                        //FrmGeneSynthesisPro.StartSubcloning();
                    }
                }
            }
        }

        private static void GetListStartPositionsAndOrientationRestrictionSiteInTarget(string DNASequence, string RestrictionSite, ref List<int> ListStartPositionsRestrictionSite, ref List<int> ListOrientationRestrictionSite)
        {
            List<int> StartPositionsRestrictionSite = new List<int>(), OrientationRestrictionSite = new List<int>();
            int i, d, LengthRestrictionSite;
            string TestRestrictionSite, TestCharacter, ReferenceCharacter, RevRestrictionSite;
            bool SiteMatchs;

            LengthRestrictionSite = Strings.Len(RestrictionSite);
            TestCharacter = "";
            RevRestrictionSite = DNAHandler.revstrIUPAC(RestrictionSite);

            // Durchlaufen der DNA Sequenz und Suche nach der forward Restriction site, Startpositionen werden in Liste gespeichert
            for (i = 1; i <= (Strings.Len(DNASequence) + 1) - LengthRestrictionSite; i++)
            {
                TestRestrictionSite = Strings.Mid(DNASequence, i, Strings.Len(RestrictionSite));

                SiteMatchs = true;

                for (d = 1; d <= LengthRestrictionSite; d++)
                {
                    TestCharacter = Strings.Mid(TestRestrictionSite, d, 1);
                    ReferenceCharacter = Strings.Mid(RestrictionSite, d, 1);

                    switch (ReferenceCharacter)
                    {
                        case "A":
                            {
                                if (string.Compare(TestCharacter, "A", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "G":
                            {
                                if (string.Compare(TestCharacter, "G", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "C":
                            {
                                if (string.Compare(TestCharacter, "C", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "T":
                            {
                                if (string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "R":
                            {
                                if (string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "A", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "Y":
                            {
                                if (string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "S":
                            {
                                if (string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "C", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "W":
                            {
                                if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "K":
                            {
                                if (string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "M":
                            {
                                if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "B":
                            {
                                if (string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "D":
                            {
                                if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "H":
                            {
                                if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "V":
                            {
                                if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "G", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }

                        case "N":
                            {
                                // Trifft immer zu, daher bleibt site matches immer true
                                if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                    SiteMatchs = false;
                                break;
                            }
                    }

                    if (SiteMatchs == false)
                        break;
                }

                if (SiteMatchs == true)
                {
                    StartPositionsRestrictionSite.Add(i);
                    OrientationRestrictionSite.Add(1);
                }

                // Durchlaufen der DNA Sequenz und Suche nach der reverse Restriction site, Startpositionen werden in Liste gespeichert
                if (SiteMatchs == false)
                {
                    SiteMatchs = true;

                    for (d = 1; d <= LengthRestrictionSite; d++)
                    {
                        TestCharacter = Strings.Mid(TestRestrictionSite, d, 1);
                        ReferenceCharacter = Strings.Mid(RevRestrictionSite, d, 1);

                        switch (ReferenceCharacter)
                        {
                            case "A":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "G":
                                {
                                    if (string.Compare(TestCharacter, "G", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "C":
                                {
                                    if (string.Compare(TestCharacter, "C", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "T":
                                {
                                    if (string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "R":
                                {
                                    if (string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "A", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "Y":
                                {
                                    if (string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "S":
                                {
                                    if (string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "C", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "W":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "K":
                                {
                                    if (string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "M":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "B":
                                {
                                    if (string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "D":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "H":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "V":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "G", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }

                            case "N":
                                {
                                    if (string.Compare(TestCharacter, "A", false) != 0 & string.Compare(TestCharacter, "C", false) != 0 & string.Compare(TestCharacter, "G", false) != 0 & string.Compare(TestCharacter, "T", false) != 0)
                                        SiteMatchs = false;
                                    break;
                                }
                        }

                        if (SiteMatchs == false)
                            break;
                    }

                    if (SiteMatchs == true)
                    {
                        StartPositionsRestrictionSite.Add(i);
                        OrientationRestrictionSite.Add(-1);
                    }
                }
            }

            ListStartPositionsRestrictionSite = StartPositionsRestrictionSite;
            ListOrientationRestrictionSite = OrientationRestrictionSite;
        }

        private static void GetNewListOfStartPositionsAndOrientations(ref List<int> ListOfStartPositions, ref List<int> ListOfOrientation, string EnzymeName, string VectorOrInsert, string FiveOrThree)
        {
            int i, NewIndexNull;

            FrmSuChooseSite FRM1 = new FrmSuChooseSite();

            //FrmSubcloningChooseSite.LblChooseSiteFiveOrThree.Text = FiveOrThree;
            FRM1.label1.Text = String.Format("The following {0}' Site was found multiple times:  ", FiveOrThree);
            //FrmSubcloningChooseSite.LblChooseSiteMultipleSite.Text = EnzymeName;
            FRM1.label2.Text = EnzymeName;
            //FrmSubcloningChooseSite.LblChooseSiteSequence.Text = VectorOrInsert;
            FRM1.label4.Text = String.Format("{0} Sequence", VectorOrInsert);
            //FrmSubcloningChooseSite.CBChooseSiteStartPosition.Items.Clear();
            FRM1.comboBox1.Items.Clear();

            
            

            for (i = 0; i <= ListOfStartPositions.Count - 1; i++)
                FRM1.comboBox1.Items.Add(ListOfStartPositions[i]);

            FRM1.comboBox1.SelectedIndex = 0;
            
            //FrmSubcloningChooseSite.ShowDialog();
            FRM1.ShowDialog();
            NewIndexNull = FRM1.comboBox1.SelectedIndex;

            ListOfStartPositions[0] = ListOfStartPositions[NewIndexNull];
            ListOfOrientation[0] = ListOfOrientation[NewIndexNull];


            FRM1 = null;

        }

        private static bool AreRestrictionSitesTooCloseToTheEnd(string DNASequence, List<int> StartPositionEnzyme5SiteDNASequence, List<int> StartPositionEnzyme3SiteDNASequence, int IndexRestrictionEnzyme5DNASequence, int IndexRestrictionEnzyme3DNASequence, List<int> Orientation5SiteDNASequence, List<int> Orientation3SiteDNASequence, List<int> ListofEnzymeCutPositionsFor, List<int> ListOfEnzymeCutPositionsRev, List<string> ListOfEnzymeSites)
        {
            int TypeOfOverhangDNASequence5, TypeOfOverhangDNASequence3, BeginOverhangDNASequence5=0, BeginOverhangDNASequence3=0, CountOverhangBases3Corrected=0;

            bool AreRestrictionSitesTooCloseToTheEndReturn = true;

            TypeOfOverhangDNASequence5 = DetermineTypeOfOverhang(IndexRestrictionEnzyme5DNASequence, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);
            TypeOfOverhangDNASequence3 = DetermineTypeOfOverhang(IndexRestrictionEnzyme3DNASequence, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);

            // BeginOverhangVector ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation5SiteDNASequence[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangDNASequence5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5DNASequence]);
                else if (TypeOfOverhangDNASequence5 == 1)
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5DNASequence]) - 1;
                else if (TypeOfOverhangDNASequence5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5DNASequence]);
            }
            else if (Orientation5SiteDNASequence[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangDNASequence5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5DNASequence]);
                else if (TypeOfOverhangDNASequence5 == 1)
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5DNASequence]);
                else if (TypeOfOverhangDNASequence5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5DNASequence]);
            }

            // BeginOverhangVector ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation3SiteDNASequence[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangDNASequence3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence]);
                else if (TypeOfOverhangDNASequence3 == 1)
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence]) - 1;
                else if (TypeOfOverhangDNASequence3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3DNASequence]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);
            }
            else if (Orientation3SiteDNASequence[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangDNASequence3 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);
                else if (TypeOfOverhangDNASequence3 == 1)
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);
                else if (TypeOfOverhangDNASequence3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence]);
            }

            if (TypeOfOverhangDNASequence3 == 0 | TypeOfOverhangDNASequence3 == 2)
                CountOverhangBases3Corrected = Math.Abs(ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence] - ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]) - 1;
            else if (TypeOfOverhangDNASequence3 == 1)
                CountOverhangBases3Corrected = Math.Abs(ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence] - ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);

            if (BeginOverhangDNASequence5 < 0 | BeginOverhangDNASequence3 + CountOverhangBases3Corrected > Strings.Len(DNASequence))
                AreRestrictionSitesTooCloseToTheEndReturn = true;
            else
                AreRestrictionSitesTooCloseToTheEndReturn = false;

            return AreRestrictionSitesTooCloseToTheEndReturn;
        }

        private static bool AreRestrictionSitesOverlapping(string DNASequence, List<int> StartPositionEnzyme5SiteDNASequence, List<int> StartPositionEnzyme3SiteDNASequence, int IndexRestrictionEnzyme5DNASequence, int IndexRestrictionEnzyme3DNASequence, List<int> Orientation5SiteDNASequence, List<int> Orientation3SiteDNASequence, List<int> ListofEnzymeCutPositionsFor, List<int> ListOfEnzymeCutPositionsRev, List<string> ListOfEnzymeSites)
        {
            int TypeOfOverhangDNASequence5, TypeOfOverhangDNASequence3, BeginOverhangDNASequence5=0, BeginOverhangDNASequence3=0;

            bool AreRestrictionSitesOverlappingReturn = true;

            TypeOfOverhangDNASequence5 = DetermineTypeOfOverhang(IndexRestrictionEnzyme5DNASequence, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);
            TypeOfOverhangDNASequence3 = DetermineTypeOfOverhang(IndexRestrictionEnzyme3DNASequence, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);

            // BeginOverhangVector ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation5SiteDNASequence[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangDNASequence5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5DNASequence]);
                else if (TypeOfOverhangDNASequence5 == 1)
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5DNASequence]) - 1;
                else if (TypeOfOverhangDNASequence5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5DNASequence]);
            }
            else if (Orientation5SiteDNASequence[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangDNASequence5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5DNASequence]);
                else if (TypeOfOverhangDNASequence5 == 1)
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5DNASequence]);
                else if (TypeOfOverhangDNASequence5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence5 = StartPositionEnzyme5SiteDNASequence[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5DNASequence]);
            }

            // BeginOverhangVector ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation3SiteDNASequence[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangDNASequence3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence]);
                else if (TypeOfOverhangDNASequence3 == 1)
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3DNASequence]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence]) - 1;
                else if (TypeOfOverhangDNASequence3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3DNASequence]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);
            }
            else if (Orientation3SiteDNASequence[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangDNASequence3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);
                else if (TypeOfOverhangDNASequence3 == 1)
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3DNASequence]);
                else if (TypeOfOverhangDNASequence3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangDNASequence3 = StartPositionEnzyme3SiteDNASequence[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3DNASequence]);
            }

            if (StartPositionEnzyme5SiteDNASequence[0] == StartPositionEnzyme3SiteDNASequence[0])
                AreRestrictionSitesOverlappingReturn = false;
            else if ((BeginOverhangDNASequence5 + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5DNASequence]) - 1) < BeginOverhangDNASequence3)
                AreRestrictionSitesOverlappingReturn = false;
            return AreRestrictionSitesOverlappingReturn;
        }

        private static bool AreRestrictionSitesCompatible(string DNASequenceVector, string DNASequenceInsert, int IndexRestrictionEnzymeVector, int IndexRestrictionEnzymeInsert, List<int> StartPositionsEnzymeSiteVector, List<int> OrientationEnzymeSiteVector, List<int> StartPositionsEnzymeSiteInsert, List<int> OrientationEnzymeSiteInsert, List<int> ListOfEnzymeCutPositionsFor, List<int> ListOfEnzymeCutPositionsrev, List<string> ListOfEnzymeSites)
        {
            int BeginOverhangVector=0, BeginOverhangInsert=0, TypeOfOverhangVector, TypeOfOverhangInsert, CountOverhangBasesVector, CountOverhangBasesInsert;
            string OverhangBasesVector, OverhangBasesInsert;

            bool AreRestrictionSitesCompatibleReturn = true;

            // Bestimmung, ob es sich um einen 5' Überhang (=0), blunt overhang (=1) oder 3' Überhang (=2) handelt
            TypeOfOverhangVector = DetermineTypeOfOverhang(IndexRestrictionEnzymeVector, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev);
            TypeOfOverhangInsert = DetermineTypeOfOverhang(IndexRestrictionEnzymeInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev);


            // Sind die sites kompatibel durch die Art des Überhangs? Berücksichtigung des Falls blunt ends
            if (TypeOfOverhangVector == 1 & TypeOfOverhangInsert == 1)
                AreRestrictionSitesCompatibleReturn = true;
            else if (TypeOfOverhangVector == 1 & TypeOfOverhangInsert != 1)
                AreRestrictionSitesCompatibleReturn = false;
            else if (TypeOfOverhangInsert == 1 & TypeOfOverhangVector != 1)
                AreRestrictionSitesCompatibleReturn = false;
            else if (TypeOfOverhangInsert == TypeOfOverhangVector)
            {
                CountOverhangBasesVector = Math.Abs(ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeVector] - ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeVector]);
                CountOverhangBasesInsert = Math.Abs(ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeInsert] - ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeInsert]);

                if (CountOverhangBasesVector == CountOverhangBasesInsert)
                {
                    if (OrientationEnzymeSiteVector[0] == 1)
                    {
                        // Gilt für For sites
                        if (TypeOfOverhangVector == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeVector]) + (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeVector]);
                        else if (TypeOfOverhangVector == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeVector]) + (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeVector]);
                    }
                    else if (OrientationEnzymeSiteVector[0] == -1)
                    {
                        // Gilt für Rev sites
                        if (TypeOfOverhangVector == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] - (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeVector]);
                        else if (TypeOfOverhangVector == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] - (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeVector]);
                    }


                    if (OrientationEnzymeSiteInsert[0] == 1)
                    {
                        // Gilt für For sites
                        if (TypeOfOverhangInsert == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeInsert]) + (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeInsert]);
                        else if (TypeOfOverhangInsert == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeInsert]) + (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeInsert]);
                    }
                    else if (OrientationEnzymeSiteInsert[0] == -1)
                    {
                        // Gilt für Rev sites
                        if (TypeOfOverhangInsert == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] - (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeInsert]);
                        else if (TypeOfOverhangInsert == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] - (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeInsert]);
                    }


                    if (BeginOverhangInsert > 0 & BeginOverhangVector > 0)
                    {
                        OverhangBasesVector = Strings.Mid(DNASequenceVector, BeginOverhangVector, CountOverhangBasesVector);
                        OverhangBasesInsert = Strings.Mid(DNASequenceInsert, BeginOverhangInsert, CountOverhangBasesInsert);

                        if (string.Compare(OverhangBasesVector, OverhangBasesInsert, false) == 0)
                            AreRestrictionSitesCompatibleReturn = true;
                        else
                            AreRestrictionSitesCompatibleReturn = false;
                    }
                    else
                        AreRestrictionSitesCompatibleReturn = false;
                }
                else
                    AreRestrictionSitesCompatibleReturn = false;
            }
            else
                AreRestrictionSitesCompatibleReturn = false;
            return AreRestrictionSitesCompatibleReturn;
        }

        private static bool AreRestrictionSitesCompatibleAfterAutoFlip(string DNASequenceVector, string DNASequenceInsert, int IndexRestrictionEnzymeVector, int IndexRestrictionEnzymeInsert, List<int> StartPositionsEnzymeSiteVector, List<int> OrientationEnzymeSiteVector, List<int> StartPositionsEnzymeSiteInsert, List<int> OrientationEnzymeSiteInsert, List<int> ListOfEnzymeCutPositionsFor, List<int> ListOfEnzymeCutPositionsrev, List<string> ListOfEnzymeSites)
        {
            int BeginOverhangVector=0, BeginOverhangInsert=0, TypeOfOverhangVector, TypeOfOverhangInsert, CountOverhangBasesVector, CountOverhangBasesInsert;
            string OverhangBasesVector, OverhangBasesInsert;

            bool AreRestrictionSitesCompatibleAfterAutoFlipReturn = true;

            // Bestimmung, ob es sich um einen 5' Überhang (=0), blunt overhang (=1) oder 3' Überhang (=2) handelt
            TypeOfOverhangVector = DetermineTypeOfOverhang(IndexRestrictionEnzymeVector, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev);
            TypeOfOverhangInsert = DetermineTypeOfOverhang(IndexRestrictionEnzymeInsert, ListOfEnzymeCutPositionsFor, ListOfEnzymeCutPositionsrev);


            // Sind die sites kompatibel durch die Art des Überhangs? Berücksichtigung des Falls blunt ends
            if (TypeOfOverhangVector == 1 & TypeOfOverhangInsert == 1)
                AreRestrictionSitesCompatibleAfterAutoFlipReturn = true;
            else if (TypeOfOverhangVector == 1 & TypeOfOverhangInsert != 1)
                AreRestrictionSitesCompatibleAfterAutoFlipReturn = false;
            else if (TypeOfOverhangInsert == 1 & TypeOfOverhangVector != 1)
                AreRestrictionSitesCompatibleAfterAutoFlipReturn = false;
            else if (TypeOfOverhangInsert == TypeOfOverhangVector)
            {
                CountOverhangBasesVector = Math.Abs(ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeVector] - ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeVector]);
                CountOverhangBasesInsert = Math.Abs(ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeInsert] - ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeInsert]);

                if (CountOverhangBasesVector == CountOverhangBasesInsert)
                {
                    if (OrientationEnzymeSiteVector[0] == 1)
                    {
                        // Gilt für For sites
                        if (TypeOfOverhangVector == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeVector]) + (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeVector]);
                        else if (TypeOfOverhangVector == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeVector]) + (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeVector]);
                    }
                    else if (OrientationEnzymeSiteVector[0] == -1)
                    {
                        // Gilt für Rev sites
                        if (TypeOfOverhangVector == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] - (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeVector]);
                        else if (TypeOfOverhangVector == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangVector = StartPositionsEnzymeSiteVector[0] - (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeVector]);
                    }


                    if (OrientationEnzymeSiteInsert[0] == 1)
                    {
                        // Gilt für For sites
                        if (TypeOfOverhangInsert == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeInsert]) + (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeInsert]);
                        else if (TypeOfOverhangInsert == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzymeInsert]) + (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeInsert]);
                    }
                    else if (OrientationEnzymeSiteInsert[0] == -1)
                    {
                        // Gilt für Rev sites
                        if (TypeOfOverhangInsert == 0)
                            // Betrachtung von 5' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] - (ListOfEnzymeCutPositionsrev[IndexRestrictionEnzymeInsert]);
                        else if (TypeOfOverhangInsert == 2)
                            // Betrachtung von 3' Überhängen
                            BeginOverhangInsert = StartPositionsEnzymeSiteInsert[0] - (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzymeInsert]);
                    }


                    if (BeginOverhangInsert > 0 & BeginOverhangVector > 0)
                    {
                        OverhangBasesVector = Strings.Mid(DNASequenceVector, BeginOverhangVector, CountOverhangBasesVector);
                        OverhangBasesInsert = Strings.Mid(DNASequenceInsert, BeginOverhangInsert, CountOverhangBasesInsert);

                        if (string.Compare(OverhangBasesVector, DNAHandler.revstr(OverhangBasesInsert), false) == 0)
                            AreRestrictionSitesCompatibleAfterAutoFlipReturn = true;
                        else
                            AreRestrictionSitesCompatibleAfterAutoFlipReturn = false;
                    }
                    else
                        AreRestrictionSitesCompatibleAfterAutoFlipReturn = false;
                }
                else
                    AreRestrictionSitesCompatibleAfterAutoFlipReturn = false;
            }
            else
                AreRestrictionSitesCompatibleAfterAutoFlipReturn = false;
            return AreRestrictionSitesCompatibleAfterAutoFlipReturn;
        }

        private static int DetermineTypeOfOverhang(int IndexRestrictionEnzyme, List<int> ListOfEnzymeCutPositionsFor, List<int> ListOfEnzymeCutPositionsrev)
        {
            int TypeOfOverhang=0;

            // Bestimmung, ob es sich um einen 5' Überhang (=0), blunt overhang (=1) oder 3' Überhang (=2) handelt
            if (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzyme] < ListOfEnzymeCutPositionsrev[IndexRestrictionEnzyme])
                TypeOfOverhang = 0;
            else if (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzyme] == ListOfEnzymeCutPositionsrev[IndexRestrictionEnzyme])
                TypeOfOverhang = 1;
            else if (ListOfEnzymeCutPositionsFor[IndexRestrictionEnzyme] > ListOfEnzymeCutPositionsrev[IndexRestrictionEnzyme])
                TypeOfOverhang = 2;

            return TypeOfOverhang;
        }

        private static string GetInSilicoSequence(string DNASequenceVector, string DNASequenceInsert, List<int> StartPositionEnzyme5SiteVector, List<int> StartPositionEnzyme5SiteInsert, List<int> StartPositionEnzyme3SiteVector, List<int> StartPositionEnzyme3SiteInsert, int IndexRestrictionEnzyme5Vector, int IndexRestrictionEnzyme5Insert, int IndexRestrictionEnzyme3Vector, int IndexRestrictionEnzyme3Insert, List<int> Orientation5SiteVector, List<int> Orientation5SiteInsert, List<int> Orientation3SiteVector, List<int> Orientation3SiteInsert, List<int> ListofEnzymeCutPositionsFor, List<int> ListOfEnzymeCutPositionsRev, List<string> ListOfEnzymeSites)
        {
            int BeginOverhangVector5=0, BeginOverhangInsert5=0, TypeOfOverhangVector5, TypeOfOverhangInsert5, BeginOverhangVector3=0, BeginOverhangInsert3=0, TypeOfOverhangVector3, TypeOfOverhangInsert3;
            string InSilicoSequence;
            int CountOverhangBasesVector5, CountOverhangBasesInsert5, CountOverhangBasesVector3;

            InSilicoSequence = "";

            TypeOfOverhangVector5 = DetermineTypeOfOverhang(IndexRestrictionEnzyme5Vector, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);
            TypeOfOverhangInsert5 = DetermineTypeOfOverhang(IndexRestrictionEnzyme5Insert, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);
            TypeOfOverhangVector3 = DetermineTypeOfOverhang(IndexRestrictionEnzyme3Vector, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);
            TypeOfOverhangInsert3 = DetermineTypeOfOverhang(IndexRestrictionEnzyme3Insert, ListofEnzymeCutPositionsFor, ListOfEnzymeCutPositionsRev);

            // BeginOverhangVector ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation5SiteVector[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangVector5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangVector5 = StartPositionEnzyme5SiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5Vector]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Vector]);
                else if (TypeOfOverhangVector5 == 1)
                    BeginOverhangVector5 = StartPositionEnzyme5SiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5Vector]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Vector]) - 1;
                else if (TypeOfOverhangVector5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangVector5 = StartPositionEnzyme5SiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5Vector]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Vector]);
            }
            else if (Orientation5SiteVector[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangVector5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangVector5 = StartPositionEnzyme5SiteVector[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Vector]);
                else if (TypeOfOverhangVector5 == 1)
                    BeginOverhangVector5 = StartPositionEnzyme5SiteVector[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Vector]) - 1;
                else if (TypeOfOverhangVector5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangVector5 = StartPositionEnzyme5SiteVector[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Vector]);
            }


            CountOverhangBasesVector5 = Math.Abs(ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Vector] - ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Vector]);

            if (TypeOfOverhangVector5 == 0 | TypeOfOverhangVector5 == 2)
                InSilicoSequence = Strings.Mid(DNASequenceVector, 1, BeginOverhangVector5 + CountOverhangBasesVector5 - 1);
            else if (TypeOfOverhangVector5 == 1)
                InSilicoSequence = Strings.Mid(DNASequenceVector, 1, BeginOverhangVector5);


            // BeginOverhangInsert ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation5SiteInsert[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangInsert5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangInsert5 = StartPositionEnzyme5SiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5Insert]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Insert]);
                else if (TypeOfOverhangInsert5 == 1)
                    BeginOverhangInsert5 = StartPositionEnzyme5SiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5Insert]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Insert]) - 1;
                else if (TypeOfOverhangInsert5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangInsert5 = StartPositionEnzyme5SiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme5Insert]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Insert]);
            }
            else if (Orientation5SiteInsert[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangInsert5 == 0)
                    // Betrachtung von 5' Überhängen
                    BeginOverhangInsert5 = StartPositionEnzyme5SiteInsert[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Insert]);
                else if (TypeOfOverhangInsert5 == 1)
                    BeginOverhangInsert5 = StartPositionEnzyme5SiteInsert[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Insert]) - 1;
                else if (TypeOfOverhangInsert5 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangInsert5 = StartPositionEnzyme5SiteInsert[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Insert]);
            }

            // BeginOverhangInsert ist bei einem Blunt cuttern die letzteBase vor dem Schnitt
            if (Orientation3SiteInsert[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangInsert3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangInsert3 = StartPositionEnzyme3SiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3Insert]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Insert]);
                else if (TypeOfOverhangInsert3 == 1)
                    BeginOverhangInsert3 = StartPositionEnzyme3SiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3Insert]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Insert]) - 1;
                else if (TypeOfOverhangInsert3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangInsert3 = StartPositionEnzyme3SiteInsert[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3Insert]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Insert]);
            }
            else if (Orientation3SiteInsert[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangInsert3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangInsert3 = StartPositionEnzyme3SiteInsert[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Insert]);
                else if (TypeOfOverhangInsert3 == 1)
                    BeginOverhangInsert3 = StartPositionEnzyme3SiteInsert[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Insert]) - 1;
                else if (TypeOfOverhangInsert3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangInsert3 = StartPositionEnzyme3SiteInsert[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Insert]);
            }

            CountOverhangBasesInsert5 = Math.Abs(ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme5Insert] - ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme5Insert]);

            if ((TypeOfOverhangInsert5 == 0 | TypeOfOverhangInsert5 == 2) & (TypeOfOverhangInsert3 == 0 | TypeOfOverhangInsert3 == 2))
                InSilicoSequence = InSilicoSequence + Strings.Mid(DNASequenceInsert, BeginOverhangInsert5 + CountOverhangBasesInsert5, BeginOverhangInsert3 - (BeginOverhangInsert5 + CountOverhangBasesInsert5));
            else if (TypeOfOverhangInsert5 == 1 & (TypeOfOverhangInsert3 == 0 | TypeOfOverhangInsert3 == 2))
                InSilicoSequence = InSilicoSequence + Strings.Mid(DNASequenceInsert, BeginOverhangInsert5 + 1, BeginOverhangInsert3 - (BeginOverhangInsert5 + 1));
            else if ((TypeOfOverhangInsert5 == 0 | TypeOfOverhangInsert5 == 2) & TypeOfOverhangInsert3 == 1)
                InSilicoSequence = InSilicoSequence + Strings.Mid(DNASequenceInsert, BeginOverhangInsert5 + CountOverhangBasesInsert5, BeginOverhangInsert3 - (BeginOverhangInsert5 + CountOverhangBasesInsert5) + 1);
            else if (TypeOfOverhangInsert5 == 1 & TypeOfOverhangInsert3 == 1)
                InSilicoSequence = InSilicoSequence + Strings.Mid(DNASequenceInsert, BeginOverhangInsert5 + 1, BeginOverhangInsert3 + 1 - (BeginOverhangInsert5 + 1));

            if (Orientation3SiteVector[0] == 1)
            {
                // Gilt für For sites
                if (TypeOfOverhangVector3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangVector3 = StartPositionEnzyme3SiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3Vector]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Vector]);
                else if (TypeOfOverhangVector3 == 1)
                    BeginOverhangVector3 = StartPositionEnzyme3SiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3Vector]) + (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Vector]) - 1;
                else if (TypeOfOverhangVector3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangVector3 = StartPositionEnzyme3SiteVector[0] + Strings.Len(ListOfEnzymeSites[IndexRestrictionEnzyme3Vector]) + (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Vector]);
            }
            else if (Orientation3SiteVector[0] == -1)
            {
                // Gilt für Rev sites
                if (TypeOfOverhangVector3 == 0)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangVector3 = StartPositionEnzyme3SiteVector[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Vector]);
                else if (TypeOfOverhangVector3 == 1)
                    BeginOverhangVector3 = StartPositionEnzyme3SiteVector[0] - (ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Vector]) - 1;
                else if (TypeOfOverhangVector3 == 2)
                    // Betrachtung von 3' Überhängen
                    BeginOverhangVector3 = StartPositionEnzyme3SiteVector[0] - (ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Vector]);
            }

            CountOverhangBasesVector3 = Math.Abs(ListofEnzymeCutPositionsFor[IndexRestrictionEnzyme3Vector] - ListOfEnzymeCutPositionsRev[IndexRestrictionEnzyme3Vector]);

            if (TypeOfOverhangVector3 == 0 | TypeOfOverhangVector3 == 2)
                InSilicoSequence = InSilicoSequence + Strings.Mid(DNASequenceVector, BeginOverhangVector3, Strings.Len(DNASequenceVector) - BeginOverhangVector3 + 1);
            else if (TypeOfOverhangVector3 == 1)
                InSilicoSequence = InSilicoSequence + Strings.Mid(DNASequenceVector, BeginOverhangVector3 + 1, Strings.Len(DNASequenceVector) - (BeginOverhangVector3 + 1) + 1);

            return InSilicoSequence;
        }

        private static void WriteInSilicoSequence(string Vectorname, string Genename, string InSilicoSequence)
        {
            var MessageAntwort= DialogResult.None;

            // Verzeichnis für Zielverzeichnis
            //string sPath = INIHandler.GetOutPutDirectoryFromIni(); Orginal
            string sPath = "Desktop";
            if (string.Compare(sPath, "Desktop") == 0)
                sPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            // Check, ob Zielverzeichnis exsistiert ... wenn ja, fragen, obDateien aus Altem gelöscht und durch neue ersetzt werden können...wenn nein, erzeugen ohne Nachfrage
            if (System.IO.Directory.Exists(sPath + @"\" + Genename + @"-Subcloning\") == true)
            {
                MessageAntwort = MessageBox.Show("Das Verzeichnis für die Datei-Ausgabe exsistiert bereits. Sind sie sicher, dass sie das alte Verzeichnis löschen und durch das neu erzeugte ersetzen wollen?", "", MessageBoxButtons.YesNo);
                if (MessageAntwort == DialogResult.Yes)
                {
                    try
                    {
                        Directory.Delete(sPath + @"\" + Genename + @"-Subcloning\");
                       //Orinal
                       // foreach (string foundFile in My.Computer.FileSystem.GetFiles(sPath + @"\" + Genename + @" -Subcloning\", Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.*"))
                       //     My.Computer.FileSystem.DeleteFile(foundFile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox("Der Zugriff auf die zu löschenden Dateien wurde verweigert. Fehlermeldung:" + Constants.vbCrLf + Constants.vbCrLf + ex.Message, Constants.vbCritical);
                        goto Ausgang;
                    }
                }
                else
                    goto Ausgang;
            }

            //FileHandler.MakeDir(sPath + @"\" + Genename + @"-Subcloning\");
            Directory.CreateDirectory(sPath + @"\" + Genename + @"-Subcloning\");
            // Speichern als Seqman
            File.WriteAllText(sPath + @"\" + Genename + @"-Subcloning\" + Genename + " in " + Vectorname + " in silico.seq", InSilicoSequence, System.Text.Encoding.ASCII);
            //My.Computer.FileSystem.WriteAllText(sPath + @"\" + Genename + @"-Subcloning\" + Genename + " in " + Vectorname + " in silico.seq", InSilicoSequence, false, System.Text.Encoding.ASCII);
            
       Ausgang:
            ;
        }
    }



}

