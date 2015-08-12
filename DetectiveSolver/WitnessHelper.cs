/*******************************************************************************************************************
 * Author : Venkata Narayana Terli
 * mail id : venkat.terli@gmail.com
 * date : 04-Aug-15
 * Summary : Main purpose for this file is, read the Witness data from JSON file, validate witness remember items  
 *           prepare the witness order and showing result on Console
 * ****************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DetectiveSolver
{
    class WitnessHelper
    {

        protected bool ReadFile(string fileName, ref  List<List<string>> WitnessList,out int totalActions)
        {
            bool bResult = true;
            System.IO.StreamReader file = null;
            totalActions = 0;
            try
            {
                file = new System.IO.StreamReader(fileName);
                int numOfWitness = 0;
                string line;
                List<string> Witness = null;
                List<string> temp = new List<string>();
                string item = string.Empty;
                string readLine = string.Empty;
                while ((readLine = file.ReadLine()) != null)
                {
                    line = readLine.Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line.Trim().Equals("["))
                    {
                        Witness = new List<string>();
                        numOfWitness++;
                    }
                    else if (line.Trim().Equals("]") || line.Trim().Equals("],"))
                    {
                        if (numOfWitness > 1)
                            WitnessList.Add(Witness);

                        numOfWitness--;

                    }
                    else
                    {
                        item = string.Empty;
                        item = line.Trim().Replace("\"", "");
                        if (item.EndsWith(","))
                            item = item.Replace(",", "");

                        if (!temp.Contains(item))
                        {
                            temp.Add(item);
                            totalActions++;
                        }

                        Witness.Add(item);
                        
                    }

                    Console.WriteLine(line);

                }
                if (numOfWitness != 0)
                {
                    bResult = false;
                    Console.WriteLine("Format error");
                }
            }
            catch (IOException ex)
            {
                bResult = false;
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                bResult = false;
                Console.WriteLine("Parsing Error \n" + e.Message);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }

            return bResult;
        }



        //protected void ShowResult()
        //{
             
            //Console.WriteLine("*******output********************\n");
            //int numOfWitness = ListOfWitness.Count;
            //if (numOfWitness == 0) return;

            //Console.WriteLine("\t[");
            //for (int list = 0; list < numOfWitness; list++)
            //{
            //    WitnessRecord witness = ListOfWitness[list];

            //    List<WitnessRecordItem> records = witness.GetWitnessRecordItems();
            //    int numOfRecords = records.Count;
            //    if (numOfRecords > 0)
            //    {
            //        Console.WriteLine("\t [");
            //        for (int recIndex = 0; recIndex < numOfRecords; recIndex++)
            //        {
            //            WitnessRecordItem item = records[recIndex];
            //            if (numOfRecords != (recIndex + 1))
            //                Console.WriteLine("\t  \"" + item.getWitness() + "\",");
            //            else
            //                Console.WriteLine("\t  \"" + item.getWitness() + "\"");
            //        }

            //        if (numOfWitness != (list + 1))
            //            Console.WriteLine("\t ],");
            //        else
            //            Console.WriteLine("\t ]");
            //    }

            //}

            //Console.WriteLine("\t]");

            //Console.WriteLine("\n*******output********************");
       // }



        protected bool CompareTwoWitness(WitnessRecord firstWitness, WitnessRecord nextWitness)
        {
            bool bMatched = false;
            List<WitnessRecordItem> firstRecord = firstWitness.GetWitnessRecordItems();
            List<WitnessRecordItem> nextRecord = nextWitness.GetWitnessRecordItems();

            bMatched = ScanTwoLists(ref firstRecord, ref nextRecord);

            return bMatched;
        }

        public bool ScanTwoLists(ref List<WitnessRecordItem> firstList, ref List<WitnessRecordItem> secondList)
        {
            string str1 = string.Empty;
            string str2 = string.Empty;
            bool bMerge = false;
            int matchCount = 0;
            for (int item = 0; item < firstList.Count; item++)
            {
                str1 = firstList[item].getWitness();

                for (int SLItem = 0; SLItem < secondList.Count; SLItem++)
                {
                    str2 = secondList[SLItem].getWitness();

                    if (str1.Equals(str2))
                    {

                        bMerge = true;
                        if (item == 0 && SLItem == 0)
                            bMerge = false;

                        else if ((item + 1) == firstList.Count && ((SLItem + 1) == secondList.Count))
                        {
                            if (matchCount == 0)
                                bMerge = false;
                        }
                        matchCount++;
                    }
                }
            }

            return bMerge;
        }
 

    }
}