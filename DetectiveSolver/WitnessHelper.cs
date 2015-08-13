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
        protected WitnessList wintessList;
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

        protected void copyToNewWitnessList(ref WitnessList newWitnessList, ref WitnessList revList)
        {
            foreach (var fullList in revList.FullMergeWintess)
                newWitnessList.addToFullMerge(fullList);

            foreach (var fullList in revList.PartialMergeWitness)
                newWitnessList.addToPartialMerge(fullList);
        }

        protected void copyToMasterlist(WitnessList revList)
        {
            foreach (var fullList in revList.FullMergeWintess)
                wintessList.addToFullMerge(fullList);

            foreach (var fullList in revList.PartialMergeWitness)
                wintessList.addToPartialMerge(fullList);
        }

        protected void CopyTillLastMatch(ref List<string> newRecordItems, string lastMatched, ref List<string> subWitnessitems)
        {
            if (string.IsNullOrEmpty(lastMatched)) return;

            foreach (var newItem in newRecordItems)
            {
                subWitnessitems.Add(newItem);
                if (newItem.Equals(lastMatched))
                    break;
            }
        }

        protected bool isItemMatched(string wintessItem, WitnessRecord witnessTwo, out int ItemFoundIndex)
        {
            bool bItemFound = false;
            ItemFoundIndex = -1;
            var witnessItemsList = witnessTwo.GetWitnessRecordItems();
            int stIndex = witnessTwo.LastVistedItem + 1;

            for (int item = stIndex; item < witnessItemsList.Count; item++)
            {
                if (wintessItem.Equals(witnessItemsList[item].getWitness()))
                {
                    ItemFoundIndex = witnessItemsList[item].WitnessPos;
                    bItemFound = true;
                    break;
                }
            }

            return bItemFound;
        }


        protected bool isWitnessMatched()
        {
            return wintessList.MatchListStatus();
        }

        

        
    }
}