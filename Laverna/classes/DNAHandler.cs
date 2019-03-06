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
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace Laverna
{
     class DNAHandler
    {
        public static int GetDNALength(string DNASequence)
        {
            return Strings.Len(DNASequence);
        }

        public static string revstr(string c)
        {
            int i2, i;
            string newstr;
            i2 = Strings.Len(c);
            newstr = "";
            // Reverse-complement Funktion
            for (i = i2; i >= 1; i += -1)
            {
                switch (Strings.UCase(Strings.Mid(c, i, 1)))
                {
                    case "A":
                        {
                            newstr = newstr + "T";
                            break;
                        }

                    case "C":
                        {
                            newstr = newstr + "G";
                            break;
                        }

                    case "G":
                        {
                            newstr = newstr + "C";
                            break;
                        }

                    case "T":
                        {
                            newstr = newstr + "A";
                            break;
                        }
                }
            }

            return newstr;
        }

        public static string revstrIUPAC(string c)
        {
            int i2, i;
            string newstr = "";
            i2 = Strings.Len(c);            
            // Reverse-complement Funktion
            for (i = i2; i >= 1; i += -1)
            {
                switch (Strings.UCase(Strings.Mid(c, i, 1)))
                {
                    case "A":
                        {
                            newstr = newstr + "T";
                            break;
                        }

                    case "C":
                        {
                            newstr = newstr + "G";
                            break;
                        }

                    case "G":
                        {
                            newstr = newstr + "C";
                            break;
                        }

                    case "T":
                        {
                            newstr = newstr + "A";
                            break;
                        }

                    case "Y":
                        {
                            newstr = newstr + "R";
                            break;
                        }

                    case "R":
                        {
                            newstr = newstr + "Y";
                            break;
                        }

                    case "S":
                        {
                            newstr = newstr + "S";
                            break;
                        }

                    case "W":
                        {
                            newstr = newstr + "W";
                            break;
                        }

                    case "K":
                        {
                            newstr = newstr + "M";
                            break;
                        }

                    case "M":
                        {
                            newstr = newstr + "K";
                            break;
                        }

                    case "B":
                        {
                            newstr = newstr + "V";
                            break;
                        }

                    case "V":
                        {
                            newstr = newstr + "B";
                            break;
                        }

                    case "D":
                        {
                            newstr = newstr + "H";
                            break;
                        }

                    case "H":
                        {
                            newstr = newstr + "D";
                            break;
                        }

                    case "N":
                        {
                            newstr = newstr + "N";
                            break;
                        }
                }
            }

            return newstr;
        }


        // Validität von DNASequenz und Name
        public static bool IsValidDNASequence(string DNASequence)
        {
            int i;
            List<string> ListOfValidChars = new List<string>();

            ListOfValidChars.Add("A");
            ListOfValidChars.Add("G");
            ListOfValidChars.Add("C");
            ListOfValidChars.Add("T");

            for (i = 0; i <= Strings.Len(DNASequence) - 1; i++)
            {
                if (!ListOfValidChars.Contains(DNASequence.Substring(i, 1)))
                {
                    
                    return false;
                    //return;
                }
            }


            if (string.Compare(DNASequence, "") == 0)
                return false;
            else
                return true;
        }

        public static bool IsValidDNASequenceIUPAC(string DNASequence)
        {
            int i;
            List<string> ListOfValidChars = new List<string>();

            ListOfValidChars.Add("A");
            ListOfValidChars.Add("G");
            ListOfValidChars.Add("C");
            ListOfValidChars.Add("T");
            ListOfValidChars.Add("Y");
            ListOfValidChars.Add("R");
            ListOfValidChars.Add("S");
            ListOfValidChars.Add("W");
            ListOfValidChars.Add("K");
            ListOfValidChars.Add("M");
            ListOfValidChars.Add("V");
            ListOfValidChars.Add("B");
            ListOfValidChars.Add("D");
            ListOfValidChars.Add("H");
            ListOfValidChars.Add("N");

            for (i = 0; i <= Strings.Len(DNASequence) - 1; i++)
            {
                if (!ListOfValidChars.Contains(DNASequence.Substring(i, 1)))
                {
                    return false;
                    //return;
                }
            }


            if (string.Compare(DNASequence, "") == 0)
                return false;
            else
                return true;
        }

        public static bool IsValidGenename(string Genname)
        {
            bool CheckSonderzeichen;

            CheckSonderzeichen = true;
            for (var i = 1; i <= Strings.Len(Genname); i++)
            {
                if (string.Compare(Strings.Mid(Genname, i, 1), @"\", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), "/", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), ":", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), "*", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), "?", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), (Strings.Chr(34)).ToString(), true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), "<", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), ">", true) == 0 | string.Compare(Strings.Mid(Genname, i, 1), "|", true) == 0)
                    CheckSonderzeichen = false;
            }


            if (CheckSonderzeichen == true)
            {
                if (string.Compare(Genname, "", true) != 0)
                    return  true;
                else
                    return false;
            }
            else
                return false;
        }

        public static void ConvertTBToUCase(ref TextBox TB)
        {
            int OldSelectionStart, OldSelectionLength;

            OldSelectionStart = TB.SelectionStart;
            OldSelectionLength = TB.SelectionLength;

            // Entfernen aller Zeilenumbrueche aus dem eingefügten Text
            TB.Text = TB.Text.Replace(Constants.vbCrLf, Constants.vbNullString);
            // Alle Kleinbuchstaben werden in Grosse verwandelt
            TB.Text = TB.Text.ToUpper();

            TB.SelectionStart = OldSelectionStart;
            TB.SelectionLength = OldSelectionLength;
            TB.ScrollToCaret();
        }


        // Schmelztemperatur
        public static double GetNearestNeighbourTemp(string DNASequence)
        {
            int PairCount;
            double Tm, deltaH, deltaS, RlnK;
            string NachbarBasen;

            RlnK = 1.987 * Math.Log(1 / (50 * Math.Pow(10, (-9))));
            RlnK = Math.Round(1000 * RlnK) / 1000;

            PairCount = 0;
            deltaH = 0;
            deltaS = 0;

            // Berechnung von DeltaH und DeltaS für den zu testenden Primer
            for (var i = 1; i <= Strings.Len(DNASequence) - 1; i++)
            {
                NachbarBasen = Strings.Mid(DNASequence, i, 2);
                if (string.Compare(NachbarBasen, "AA", true) == 0 | string.Compare(NachbarBasen, "TT", true) == 0)
                {
                    deltaH = deltaH - 8.0;
                    deltaS = deltaS - 21.9;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "GT", true) == 0 | string.Compare(NachbarBasen, "AC", true) == 0)
                {
                    deltaH = deltaH - 9.4;
                    deltaS = deltaS - 25.5;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "CT", true) == 0 | string.Compare(NachbarBasen, "AG", true) == 0)
                {
                    deltaH = deltaH - 6.6;
                    deltaS = deltaS - 16.4;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "TG", true) == 0 | string.Compare(NachbarBasen, "CA", true) == 0)
                {
                    deltaH = deltaH - 8.2;
                    deltaS = deltaS - 21.0;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "CC", true) == 0 | string.Compare(NachbarBasen, "GG", true) == 0)
                {
                    deltaH = deltaH - 10.9;
                    deltaS = deltaS - 28.4;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "TC", true) == 0 | string.Compare(NachbarBasen, "GA", true) == 0)
                {
                    deltaH = deltaH - 8.8;
                    deltaS = deltaS - 23.5;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "AT", true) == 0)
                {
                    deltaH = deltaH - 5.6;
                    deltaS = deltaS - 15.2;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "CG", true) == 0)
                {
                    deltaH = deltaH - 11.8;
                    deltaS = deltaS - 29.0;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "GC", true) == 0)
                {
                    deltaH = deltaH - 10.5;
                    deltaS = deltaS - 26.4;
                    PairCount = PairCount + 1;
                }
                else if (string.Compare(NachbarBasen, "TA", true) == 0)
                {
                    deltaH = deltaH - 6.6;
                    deltaS = deltaS - 18.4;
                    PairCount = PairCount + 1;
                }
            }

            // Berechnung der Nearest Neighbor Schmelztemperatur

            deltaH = Math.Abs(deltaH);
            deltaS = Math.Abs(deltaS);
            Tm = 1000 * ((deltaH - 3.4) / (deltaS + RlnK));

            // Umrechnung in Grad Celsius
            Tm = Tm - 272.9;

            // Salt Correction
            Tm = Tm + 7.21 * Math.Log(50 / (double)1000);

            return Tm;
        }


        // Repeats
        public static bool IsUnique(string TestDNASequence, string ReferenceDNASequenceFor, string ReferenceDNASequenceRev)
        {
            int i, AnzahlRepeats;
            string DNAfor, DNArev;

            AnzahlRepeats = 0;
            

            DNAfor = ReferenceDNASequenceFor;
            DNArev = ReferenceDNASequenceRev;


            // Check ob sich die Testsequenz noch irgendwo anders in der for oder rev Sequenz befindet
            for (i = 1; i <= (Strings.Len(DNAfor) - Strings.Len(TestDNASequence)) + 1; i++)
            {
                if (string.Compare(Strings.Mid(DNAfor, i, Strings.Len(TestDNASequence)), TestDNASequence, true) == 0)
                    AnzahlRepeats = AnzahlRepeats + 1;
            }

            for (i = 1; i <= (Strings.Len(DNArev) - Strings.Len(TestDNASequence)) + 1; i++)
            {
                if (string.Compare(Strings.Mid(DNArev, i, Strings.Len(TestDNASequence)), TestDNASequence, true) == 0)
                    AnzahlRepeats = AnzahlRepeats + 1;
            }

            if (AnzahlRepeats >= 2)
            { 
                return  false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsUniqueCount(string TestDNASequence, string ReferenceDNASequenceFor, string ReferenceDNASequenceRev, int CountUniqueBases, bool BeginAndEnd)
        {
            bool IsUniqueCountReturn;
            int i, AnzahlRepeats;
            string XBasesVonRechts, XBasesVonLinks, DNAfor, DNArev;

            IsUniqueCountReturn = true;
            //ORIGNAL
            //XBasesVonRechts = Microsoft.VisualBasic.Right(TestDNASequence, CountUniqueBases);
            //XBasesVonLinks = Microsoft.VisualBasic.Left(TestDNASequence, CountUniqueBases);
            XBasesVonRechts = TestDNASequence.Substring(TestDNASequence.Length- CountUniqueBases);
            XBasesVonLinks = TestDNASequence.Substring(0,CountUniqueBases);

            DNAfor = ReferenceDNASequenceFor;
            DNArev = ReferenceDNASequenceRev;

            // BeginAndEnd = False: Es wird nur das 3' Ende der TestDNASequence überprüft
            if (BeginAndEnd == false)
            {

                // Check ob sich die x bp am 3' Ende der TestDNASequenz noch irgendwo anders in der for oder rev RefrenceSequenz befinden
                AnzahlRepeats = 0;

                for (i = 1; i <= (Strings.Len(DNAfor) - CountUniqueBases) + 1; i++)
                {
                    if (string.Compare(Strings.Mid(DNAfor, i, CountUniqueBases), XBasesVonRechts, true) == 0)
                        AnzahlRepeats = AnzahlRepeats + 1;
                }

                for (i = 1; i <= (Strings.Len(DNArev) - CountUniqueBases) + 1; i++)
                {
                    if (string.Compare(Strings.Mid(DNArev, i, CountUniqueBases), XBasesVonRechts, true) == 0)
                        AnzahlRepeats = AnzahlRepeats + 1;
                }

                if (AnzahlRepeats >= 2)
                    IsUniqueCountReturn = false;
            }
            else if (BeginAndEnd == true)
            {

                // Check ob sich die x bp am 5' Ende der TestDNASequenz noch irgendwo anders in der for oder rev RefrenceSequenz befinden
                AnzahlRepeats = 0;

                for (i = 1; i <= (Strings.Len(DNAfor) - CountUniqueBases) + 1; i++)
                {
                    if (string.Compare(Strings.Mid(DNAfor, i, CountUniqueBases), XBasesVonRechts, true) == 0)
                        AnzahlRepeats = AnzahlRepeats + 1;
                }

                for (i = 1; i <= (Strings.Len(DNArev) - CountUniqueBases) + 1; i++)
                {
                    if (string.Compare(Strings.Mid(DNArev, i, CountUniqueBases), XBasesVonRechts, true) == 0)
                        AnzahlRepeats = AnzahlRepeats + 1;
                }

                if (AnzahlRepeats >= 2)
                    IsUniqueCountReturn = false;

                // Check ob sich die x bp am 3' Ende der TestDNASequenz noch irgendwo anders in der for oder rev RefrenceSequenz befinden
                if (IsUniqueCountReturn == true)
                {
                    AnzahlRepeats = 0;

                    for (i = 1; i <= (Strings.Len(DNAfor) - CountUniqueBases) + 1; i++)
                    {
                        if (string.Compare(Strings.Mid(DNAfor, i, CountUniqueBases), XBasesVonLinks, true) == 0)
                            AnzahlRepeats = AnzahlRepeats + 1;
                    }

                    for (i = 1; i <= (Strings.Len(DNArev) - CountUniqueBases) + 1; i++)
                    {
                        if (string.Compare(Strings.Mid(DNArev, i, CountUniqueBases), XBasesVonLinks, true) == 0)
                            AnzahlRepeats = AnzahlRepeats + 1;
                    }

                    if (AnzahlRepeats >= 2)
                        IsUniqueCountReturn = false;
                }
            }
            return IsUniqueCountReturn;
        }

        public static bool IsUniqueCountForOnly(string TestDNASequence, string ReferenceDNASequenceFor, int CountUniqueBases)
        {
            int i, AnzahlRepeats;
            string XBasesVonRechts, DNAfor;
            XBasesVonRechts = TestDNASequence.Substring(TestDNASequence.Length - CountUniqueBases);// ORGINAL XBasesVonRechts = Microsoft.VisualBasic.Right(TestDNASequence, CountUniqueBases);
            DNAfor = ReferenceDNASequenceFor;
            // Check ob sich die x bp am 3' Ende der TestDNASequenz noch irgendwo anders in der for oder rev RefrenceSequenz befinden
            AnzahlRepeats = 0;

            for (i = 1; i <= (Strings.Len(DNAfor) - CountUniqueBases) + 1; i++)
            {
                if (string.Compare(Strings.Mid(DNAfor, i, CountUniqueBases), XBasesVonRechts, true) == 0)
                    AnzahlRepeats = AnzahlRepeats + 1;
            }

            if (AnzahlRepeats >= 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        // Alignment
        public static void AlignTwoSequences(string Seq1, string Seq2, ref string AlignSeq1, ref string AlignSeq2)
        {
            int M, N, i, j, GapPenalty;

            GapPenalty = -1;

            AlignSeq1 = "";
            AlignSeq2 = "";

            M = Strings.Len(Seq1);
            N = Strings.Len(Seq2);

            int[,] CostMatrix = new int[M + 1, N + 1], PointerMatrix = new int[M + 1, N + 1];

            // PointerMatrix: enthält 1 für LeftDirection, 2 für DiagonalDirection und 3 für AboveDirection

            for (i = 0; i <= M; i++)
            {
                CostMatrix[i, 0] = i * GapPenalty;
                PointerMatrix[i, 0] = 1;
            }

            for (i = 0; i <= N; i++)
            {
                CostMatrix[0, i] = i * GapPenalty;
                PointerMatrix[0, i] = 3;
            }

            for (i = 1; i <= M; i++)
            {
                for (j = 1; j <= N; j++)
                    CostMatrix[i, j] = Get_Max(i, j, Seq1, Seq2, CostMatrix, ref PointerMatrix);
            }

            Traceback(PointerMatrix, Seq1, Seq2, ref AlignSeq1, ref AlignSeq2);
        }

        private static int Get_Max(int i, int j, string Seq1, string Seq2, int[,] CostMatrix, ref int[,] PointerMatrix)
        {
            int Similar, NonSimilar, GapPenality, M1, M2, M3, max, Mmax;

            Similar = 1;
            NonSimilar = 0;
            GapPenality = -1;


            int Sim;

            if (string.Compare(Strings.Mid(Seq1, i, 1), Strings.Mid(Seq2, j, 1)) == 0)
                Sim = Similar;
            else
                Sim = NonSimilar;

            M1 = CostMatrix[i - 1, j - 1] + Sim;
            M2 = CostMatrix[i - 1, j] + GapPenality;
            M3 = CostMatrix[i, j - 1] + GapPenality;

            if (M1 >= M2)
                max = M1;
            else
                max = M2;

            if (M3 >= max)
                Mmax = M3;
            else
                Mmax = max;

            if (Mmax == M1)
                PointerMatrix[i, j] = 2;
            else if (Mmax == M2)
                PointerMatrix[i, j] = 1;
            else if (Mmax == M3)
                PointerMatrix[i, j] = 3;

            return Mmax;
        }

        private static void Traceback(int[,] PointerMatrix, string Seq1, string Seq2, ref string AlignSeq1, ref string AlignSeq2)
        {
            int CurrentPositionM, CurrentPositionN;

            CurrentPositionM = Strings.Len(Seq1);
            CurrentPositionN = Strings.Len(Seq2);

            while (CurrentPositionM != 0 & CurrentPositionN == 0)    //ORGINAL while (!CurrentPositionM == 0 & CurrentPositionN == 0)
            {
                if (PointerMatrix[CurrentPositionM, CurrentPositionN] == 2)
                {
                    AlignSeq1 = AlignSeq1 + Strings.Mid(Seq1, CurrentPositionM, 1);
                    AlignSeq2 = AlignSeq2 + Strings.Mid(Seq2, CurrentPositionN, 1);

                    CurrentPositionM = CurrentPositionM - 1;
                    CurrentPositionN = CurrentPositionN - 1;
                }
                else if (PointerMatrix[CurrentPositionM, CurrentPositionN] == 1)
                {
                    AlignSeq1 = AlignSeq1 + Strings.Mid(Seq1, CurrentPositionM, 1);
                    AlignSeq2 = AlignSeq2 + "-";

                    CurrentPositionM = CurrentPositionM - 1;
                }
                else if (PointerMatrix[CurrentPositionM, CurrentPositionN] == 3)
                {
                    AlignSeq1 = AlignSeq1 + "-";
                    AlignSeq2 = AlignSeq2 + Strings.Mid(Seq2, CurrentPositionN, 1);

                    CurrentPositionN = CurrentPositionN - 1;
                }
            }

            AlignSeq1 = Reverse(AlignSeq1);
            AlignSeq2 = Reverse(AlignSeq2);
        }

        private static string Reverse(string Sequence)
        {
           string Reverse1 = "";

            for (var i = Strings.Len(Sequence); i >= 1; i += -1)
                Reverse1 = Reverse1 + Strings.Mid(Sequence, i, 1);
            return Reverse1;
        }
    }
}
