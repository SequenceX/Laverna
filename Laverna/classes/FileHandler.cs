using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Laverna
{
    class FileHandler
    {
        //Konstruktor
        public FileHandler()
        {


            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        }
        //Getter/Setter
        /* BSP
        private string geneType;
        public string GeneType
        {
            get
            {
                return geneType;
            }
            private set
            {
                geneType = IdentificateName(toGrab);
            }
        }
        */

        //Methoden


        // Auslesen Restriktionsenzym Datenbank
        public static List<string> GetListRestrictionEnzymeDatabase()
        {
            string PathEnzymeDatabase;
            List<string> ListOfRows = new List<string>();


            INIHandler ini = new INIHandler(ProGSYMainFrame.iniPath);
            PathEnzymeDatabase = ini.IniReadValue("Settings", "EnzymeDatabasePath");




            // --- Datei öffnen
            FileStream fs = new FileStream(PathEnzymeDatabase, FileMode.OpenOrCreate, FileAccess.Read);
            // --- Stream öffnen
            StreamReader r = new StreamReader(fs);
            // --- Zeiger auf den Anfang
            r.BaseStream.Seek(0, SeekOrigin.Begin);
            // --- Alle Zeilen lesen und speichern in Liste
            while (r.Peek() > -1)
                ListOfRows.Add(r.ReadLine());
            // --- Reader und Stream schließen
            r.Close();
            fs.Close();

            return ListOfRows;
        }

        public static List<string> GetListRestrictionEnzymeNames(List<string> ListOfRows)
        {
            List<string> ListOfEnzymesNames = new List<string>();
            string EnzymeName;

            foreach (var Row in ListOfRows)
            {
                if (string.Compare(Strings.Mid(Row, 1, 1), "'") == 0)
                {
                }
                else if (string.Compare(Strings.Mid(Row, 1, 1), "#") == 0)
                {
                    // Assymetrische Enzyme müssen gesondert behandelt werden
                    EnzymeName = Strings.Mid(Row, 2, Row.IndexOf(",") - 1);
                    ListOfEnzymesNames.Add(EnzymeName);
                }
                else
                {
                    EnzymeName = Strings.Mid(Row, 1, Row.IndexOf(","));
                    ListOfEnzymesNames.Add(EnzymeName);
                }
            }

            return ListOfEnzymesNames;
        }

        public static List<string> GetListRestrictionEnzymeSites(List<string> ListOfRows)
        {
            List<string> ListOfEnzymesSites = new List<string>();
            string EnzymeSite;

            foreach (var Row in ListOfRows)
            {
                if (string.Compare(Strings.Mid(Row, 1, 1), "'") == 0)
                {
                }
                else if (string.Compare(Strings.Mid(Row, 1, 1), "#") == 0)
                {
                    // Assymetrische Enzyme müssen gesondert behandelt werden
                    EnzymeSite = Strings.Mid(Row, Row.IndexOf(",") + 2, Row.IndexOf(",", Row.IndexOf(",") + 1) - Row.IndexOf(",") - 1);
                    ListOfEnzymesSites.Add(Strings.UCase(EnzymeSite.Replace("/", "")));
                }
                else
                {
                    EnzymeSite = Strings.Mid(Row, Row.IndexOf(",") + 2, Row.LastIndexOf(",") - Row.IndexOf(",") - 1);
                    ListOfEnzymesSites.Add(Strings.UCase(EnzymeSite.Replace("/", "")));
                }
            }

            return ListOfEnzymesSites;
        }

        public static void GetListRestrictionsEnzymeCutPositions(List<string> ListOfRows, ref List<int> ListOfEnzymesCutPositionsFor, ref List<int> ListOfEnzymesCutPositionsRev)
        {
            List<int> ListOfEnzymesCutPositionFor = new List<int>(), ListOfEnzymesCutPositionRev = new List<int>();
            string EnzymeName, EnzymeSite, Positions;
            int CutPositionFor, CutPositionRev ;

            foreach (var Row in ListOfRows)
            {
                if (string.Compare(Strings.Mid(Row, 1, 1), "'") == 0)
                {
                }
                else if (string.Compare(Strings.Mid(Row, 1, 1), "#") == 0)
                {
                    // Assymetrische Enzyme müssen gesondert behandelt werden
                    EnzymeName = Strings.Mid(Row, 2, Row.IndexOf(",") - 1);

                    EnzymeSite = Strings.Mid(Row, Row.IndexOf(",") + 2, Row.IndexOf(",", Row.IndexOf(",") + 1) - Row.IndexOf(",") - 1);

                    Positions = Strings.Mid(Row, Strings.Len(EnzymeName) + Strings.Len(EnzymeSite) + 4, Strings.Len(Row) - (Strings.Len(EnzymeName) + Strings.Len(EnzymeSite) + 4));

                    CutPositionFor = Convert.ToInt32(Strings.Mid(Positions, 1, Positions.IndexOf(",")));
                    CutPositionRev = Convert.ToInt32(Strings.Mid(Positions, Positions.IndexOf(",") + 2, Strings.Len(Positions) - (Positions.IndexOf(",") + 1)));

                    ListOfEnzymesCutPositionFor.Add(CutPositionFor);
                    ListOfEnzymesCutPositionRev.Add(CutPositionRev);
                }
                else
                {
                    EnzymeSite = Strings.Mid(Row, Row.IndexOf(",") + 2, Row.LastIndexOf(",") - Row.IndexOf(",") - 1);

                    CutPositionFor = Strings.Len(EnzymeSite) - EnzymeSite.IndexOf("/") - 1;
                    ListOfEnzymesCutPositionFor.Add(-CutPositionFor);

                    CutPositionRev = EnzymeSite.IndexOf("/");
                    ListOfEnzymesCutPositionRev.Add(-CutPositionRev);
                }
            }

            ListOfEnzymesCutPositionsFor = ListOfEnzymesCutPositionFor;
            ListOfEnzymesCutPositionsRev = ListOfEnzymesCutPositionRev;
        }
    }
}
