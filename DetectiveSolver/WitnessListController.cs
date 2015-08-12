
/*******************************************************************************************************************
 * Author : Venkata Narayana Terli
 * mail id : venkat.terli@gmail.com
 * date : 04-Aug-15
 * Summary : Main purpose for this file is, take the user input as file name and proces the possible MaxTimelines 
 *            
 * ****************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DetectiveSolver
{
    class WitnessListController : WitnessHelper
    {
        string FileName;
        string WitnessName;
        static int WitnessNumber;
        WitnessList wintessList;
        private int TotalActions;

        public WitnessListController(string FileName)
        {
            this.FileName = FileName;
            WitnessName = string.Empty;
            WitnessNumber = 0;
            wintessList = new WitnessList();
            TotalActions = 0;
        }

        private void ReadInputData()
        {
            List<List<string>> WitnessList = new List<List<string>>();

            bool bResult = ReadFile(FileName, ref WitnessList, out TotalActions);
            if (!bResult) return;

            foreach (var record in WitnessList)
                AddNewWitness(record);
        }

        private void AddNewWitness(string[] items)
        {
            WitnessName = "Witness_" + WitnessNumber;
            WitnessNumber++;
            WitnessRecord witness = new WitnessRecord(WitnessName);
            bool bLastItem = false;
            int listLength = items.Length;
            for (int i = 0; i < listLength; i++)
            {
                if ((i + 1) == listLength)
                    bLastItem = true;

                WitnessRecordItem props = new WitnessRecordItem(items[i], WitnessName, bLastItem);
                witness.addItemToList(props);
            }

            wintessList.addNewWitness(witness);

        }
        private WitnessRecord CreateWitnessRecord(List<string> recorditem)
        {

            WitnessName = "Witness_" + WitnessNumber;
            WitnessNumber++;
            var witness = new WitnessRecord(WitnessName);
            bool bLastItem = false;
            int listLength = recorditem.Count;
            for (int i = 0; i < listLength; i++)
            {
                if ((i + 1) == listLength)
                    bLastItem = true;

                WitnessRecordItem props = new WitnessRecordItem(recorditem[i], WitnessName, bLastItem);
                witness.addItemToList(props);
            }
            return witness;
        }

        private void AddNewWitness(List<string> items)
        {
            WitnessName = "Witness_" + WitnessNumber;
            WitnessNumber++;
            WitnessRecord witness = new WitnessRecord(WitnessName);
            bool bLastItem = false;
            int listLength = items.Count;
            for (int i = 0; i < listLength; i++)
            {
                if ((i + 1) == listLength)
                    bLastItem = true;

                WitnessRecordItem props = new WitnessRecordItem(items[i], WitnessName, bLastItem);
                witness.addItemToList(props);
            }

            wintessList.addNewWitness(witness);

        }
        private int GetAllWitnessActions
        {
            get { return TotalActions; }
        }

        public void StartInvestigation()
        {

            try
            {
                ReadInputData();

                GenerateTimeLines();

            }
            catch (Exception e)
            {
                Console.WriteLine("[Internal Error]" + e.Message);
            }
        }

        private void GenerateTimeLines()
        {
            IdentifyMatchingWitness();

            if (!isWitnessMatched())
                ShowResult();

            else
            {
                ProcesMatchedWitness();
                //temp();
                ShowResult();
            }
            return;
        }

        private void ProcesMatchedWitness()
        {
            var matchedWitnesses = wintessList.GetMatchedList;
            var keys = new List<WitnessRecord>(matchedWitnesses.Keys);

            foreach (var key in keys)
                ScanDictionary(key);
        }


        private void ScanDictionary(WitnessRecord key)
        {
            var dect = wintessList.GetMatchedList;
            if (dect.ContainsKey(key))
            {
                List<WitnessRecord> records = dect[key];
                foreach (var record in records)
                {
                    ScanDictionary(record);
                    dect.Remove(key);
                }
                records.Insert(0, key); //insert the key as front
                ProcessWitnessOrder(records,false);
            }

        }

        //public void ProcessWitnessOrder(List<WitnessRecord> records,bool bIgnoreIfSplit)
        //{

        //    for (int count = 1; count < records.Count; count++)
        //    {
        //        var record1 = records[0];
        //        var record2 = records[count];

        //        ConstraintMatching(record1, record2, bIgnoreIfSplit);
        //        CombineMaxTimeLines();
                
        //    }

        //   CombineMaxPartialTimeLines();
        //}

        public void temp()
        {
            for (int i =0;i<wintessList.GetWitnessList.Count ;i++)
            {
                List<WitnessRecord> tempRecords = new List<WitnessRecord>();
                for (int j = i + 1; j < wintessList.GetWitnessList.Count; j++)
                {
                    tempRecords.Add(wintessList.GetWitnessList[i]);
                    tempRecords.Add(wintessList.GetWitnessList[j]);
                }
                ProcessWitnessOrder(tempRecords, false);
            }
        }
        public void ProcessWitnessOrder(List<WitnessRecord> records, bool bIgnoreIfSplit)
        {

            for (int count = 1; count < records.Count; count++)
            {
                var record1 = records[0];
                var record2 = records[count];

                ConstraintMatching(record1, record2, bIgnoreIfSplit);
                CombineMaxTimeLines();

            }

            CombineMaxPartialTimeLines();
        }

        private void CombineMaxTimeLines()
        {
            var total = wintessList.FullMergeWintess.Count;
            if (total > 1)
            {
                ProcessWitnessOrder(prepareMergeRecrods(), true);
            }
        }

        private List<WitnessRecord> prepareMergeRecrods()
        {
            var records = new List<WitnessRecord>();
            var total = wintessList.FullMergeWintess.Count;
            if (total > 1)
            {
                
                var totalWitness = wintessList.FullMergeWintess;
                for (int count = total - 1; count >= 0; count--)
                    records.Add(totalWitness[count]);

                wintessList.RemoveAllFromFullMergeWintess();
                
            }
            return records;
        }
        private void CombineMaxPartialTimeLines()
        {
            var total = wintessList.PartialMergeWitness.Count;
            if (total > 2)
            {
                var totalWitness = wintessList.PartialMergeWitness;
                 
                var tempRec = new List<WitnessRecord>();
                foreach (var rec in totalWitness)
                {
                    rec.LastVistedItem = -1;
                    tempRec.Add(rec);
                }
                
                
                int remCount = tempRec.Count;

                for (int count = 0; count < remCount; count++)
                {
                    for (int nextW = count + 1; nextW < remCount; nextW++)
                    {
                        ConstraintMatching(tempRec[count], tempRec[nextW], true);
                        var FullMergeTotal = wintessList.FullMergeWintess.Count;
                        if (FullMergeTotal > 1)
                        {
                            var MaxMergedRec = prepareMergeRecrods();
                            if (MaxMergedRec.Count == 2)
                            {
                                ConstraintMatching(MaxMergedRec[0], MaxMergedRec[1], true);
                                //if (wintessList.FullMergeWintess.Count == 1)
                                //{
                                //    if (wintessList.FullMergeWintess[0].GetWitnessRecordItems().Count = GetAllWitnessActions)
                                //    {
                                        
                                //    }
                                //}

                            }
                        }
                    }

                }
            }
        }

        protected void ConstraintMatching(WitnessRecord WitnessOne, WitnessRecord WitnessTwo,bool bIgnoreIfSplit)
        {

            List<string> newRecordItems = new List<string>();
            List<string> subWitnessitems = null;
            bool bSplit = false;
            int iLastVistedPos = 0;
            string lastMatchdString = string.Empty;
            try
            {
                foreach (var WOneRecordItem in WitnessOne.GetWitnessRecordItems())
                {

                    string witnessItem_W1 = WOneRecordItem.getWitness();
                    bool bFirstItem_W1 = WOneRecordItem.IsFirstItem;
                    bool bLastItem_W1 = WOneRecordItem.isLastItem;
                    bool bNextItem_W1 = false;
                    int ItemIndex;

                    if (!bFirstItem_W1 && !bLastItem_W1)
                        bNextItem_W1 = true;


                    bool bItemMatched = isItemMatched(witnessItem_W1, WitnessTwo, out ItemIndex);

                    var WitnessTwoItemsList = WitnessTwo.GetWitnessRecordItems();
                    if (bItemMatched)
                    {
                        int stPos = WitnessTwo.LastVistedItem + 1;

                        var WitnessTwoItems = WitnessTwoItemsList[ItemIndex];
                        string witnessItem_W2 = WitnessTwoItems.getWitness();
                        bool bFirstItem_W2 = WitnessTwoItems.IsFirstItem;
                        bool bLastItem_W2 = WitnessTwoItems.isLastItem;
                        bool bNextItem_W2 = false;
                        string witnessGroup = WitnessTwoItems.WitnessGroup;
                        lastMatchdString = witnessItem_W2;
                        if (!bFirstItem_W2 && !bLastItem_W2)
                        {
                            bNextItem_W2 = true;
                        }

                        if ((bFirstItem_W1 && bFirstItem_W2) ||
                                            (bNextItem_W1 && bFirstItem_W2) ||
                                             (bLastItem_W1 && bFirstItem_W2))
                        {
                            newRecordItems.Add(witnessItem_W2);
                            WitnessTwo.LastVistedItem = ItemIndex;
                        }

                        else if ((bFirstItem_W1 && bNextItem_W2) ||
                                                 (bFirstItem_W1 && bLastItem_W2))
                        {
                            for (int j = stPos; j <= ItemIndex; j++)
                            {
                                newRecordItems.Add(WitnessTwoItemsList[j].getWitness());
                            }
                            WitnessTwo.LastVistedItem = ItemIndex;

                        }


                        else if ((bNextItem_W1 && bNextItem_W2) ||
                                                                  (bNextItem_W1 && bLastItem_W2))
                        {
                            WitnessTwo.LastVistedItem = ItemIndex;
                            if (WitnessOne.GetPrevItemMatched(witnessGroup))
                            {
                                for (int j = stPos; j <= ItemIndex; j++)
                                {
                                    newRecordItems.Add(WitnessTwoItemsList[j].getWitness());
                                }
                            }
                            else
                            {
                                if (!bIgnoreIfSplit)
                                {

                                    //split
                                    bSplit = true;
                                    subWitnessitems = new List<string>();
                                    int stIndex = stPos > 0 ? (stPos-1) : 0;
                                    CopyTillLastMatch(ref newRecordItems, WitnessTwoItemsList[stIndex].getWitness(), ref subWitnessitems);

                                    for (int j = stPos; j <= ItemIndex; j++)
                                    {
                                        subWitnessitems.Add(WitnessTwoItemsList[j].getWitness());
                                    }

                                    newRecordItems.Add(witnessItem_W2);
                                }
                            }
                        }


                        else if ((bLastItem_W1 && bNextItem_W2) ||
                                             (bLastItem_W1 && bLastItem_W2))
                        {
                            WitnessTwo.LastVistedItem = WitnessTwoItemsList.Count;
                            if (WitnessOne.GetPrevItemMatched(witnessGroup))
                            {
                                for (int j = stPos; j < WitnessTwoItemsList.Count; j++)
                                {
                                    newRecordItems.Add(WitnessTwoItemsList[j].getWitness());
                                }
                            }
                            else
                            {
                                if (!bIgnoreIfSplit)
                                {
                                    //split
                                    bSplit = true;
                                    subWitnessitems = new List<string>();
                                    int stIndex = stPos > 0 ? (stPos - 1) : 0;
                                    CopyTillLastMatch(ref newRecordItems, WitnessTwoItemsList[stIndex].getWitness(), ref subWitnessitems);
                                    for (int j = stPos; j < WitnessTwoItemsList.Count; j++)
                                    {
                                        subWitnessitems.Add(WitnessTwoItemsList[j].getWitness());
                                    }

                                    newRecordItems.Add(witnessItem_W2);
                                }
                            }
                        }

                        iLastVistedPos = WitnessTwo.LastVistedItem;
                        WitnessOne.SetPrevItemMatched(true, witnessGroup);
                    }
                    else
                    {
                        iLastVistedPos++;
                        var groupName = WitnessTwoItemsList[0].WitnessGroup;
                        if (!bIgnoreIfSplit)
                        {
                            
                            if (iLastVistedPos + 1 == WitnessTwoItemsList.Count) // last item in second list
                            {
                                bSplit = true;

                                subWitnessitems = new List<string>();
                                CopyTillLastMatch(ref newRecordItems, lastMatchdString, ref subWitnessitems);

                                for (int j = WitnessTwo.LastVistedItem + 1; j < WitnessTwoItemsList.Count; j++)
                                {
                                    subWitnessitems.Add(WitnessTwoItemsList[j].getWitness());
                                }
                            }
                            if (bLastItem_W1 && WitnessOne.GetPrevItemMatched(groupName))
                            {
                                if (subWitnessitems == null)
                                    subWitnessitems = new List<string>();
                                subWitnessitems.Add(witnessItem_W1);
                            }
                        }
                        newRecordItems.Add(witnessItem_W1);
                        WitnessOne.SetPrevItemMatched(false, groupName);
                    }

                }


                if (!bSplit)
                {
                    wintessList.addToFullMerge(CreateWitnessRecord(newRecordItems));                     
                }
                else
                {
                    if (bIgnoreIfSplit)
                    {
                        //wintessList.addToPartialMerge(WitnessOne);
                        //wintessList.addToPartialMerge(WitnessTwo);

                    }
                    else
                    {
                        if (subWitnessitems != null)
                        {
                            wintessList.addToPartialMerge(CreateWitnessRecord(subWitnessitems));
                        }

                        wintessList.addToPartialMerge(CreateWitnessRecord(newRecordItems));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Contraint matching error" + e.Message);
            }

        }
        void CopyTillLastMatch(ref List<string> newRecordItems, string lastMatched, ref List<string> subWitnessitems)
        {
            foreach (var newItem in newRecordItems)
            {
                subWitnessitems.Add(newItem);
                if (newItem.Equals(lastMatched))
                    break;
            }
        }

        bool isItemMatched(string wintessItem, WitnessRecord witnessTwo, out int ItemFoundIndex)
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


        private bool isWitnessMatched()
        {
            return wintessList.MatchListStatus();
        }

        public void IdentifyMatchingWitness()
        {
            var witness = wintessList.GetWitnessList;

            int ListLength = witness.Count;

            bool bMerged = false;

            for (int i = 0; i < ListLength; i++)
            {
                List<WitnessRecord> matchedList = new List<WitnessRecord>();
                for (int j = i + 1; j < ListLength; j++)
                {
                    bMerged = CompareTwoWitness(witness[i], witness[j]);
                    if (bMerged)
                    {
                        matchedList.Add(witness[j]);
                        bMerged = false;
                    }
                }
                if (matchedList.Count > 0)
                {
                    wintessList.addToMatchedList(witness[i], matchedList);
                }
            }


            return;
        }

        protected List<WitnessRecord> generateOutput()
        {
            int ListCount = 0;
            int totalActions = 0;
            ListCount = wintessList.FullMergeWintess.Count;
            List<WitnessRecord> showList;

            if (ListCount >= 1)  //full merge always 1
                totalActions = wintessList.FullMergeWintess[0].GetWitnessRecordItems().Count;

            if ((ListCount == 1) && (GetAllWitnessActions == totalActions))
            {
                showList = wintessList.FullMergeWintess;
            }
            else
            {
                ListCount = wintessList.PartialMergeWitness.Count;
                bool bFullListFound = false;
                if (ListCount > 0)
                {

                    showList = findFullMergeInPartialMerge(out bFullListFound);

                }
                else
                {
                    ListCount = wintessList.GetWitnessList.Count;
                    showList = wintessList.GetWitnessList;
                }

                if (showList.Count > 0 && wintessList.FullMergeWintess.Count > 0)
                {
                    if (!bFullListFound)
                        foreach (var record in wintessList.FullMergeWintess)
                            showList.Insert(0, record);
                }
            }

            return showList;
        }
        protected void ShowResult()
        {
            int ListCount = 0;
            List<WitnessRecord> showList = generateOutput();
            ListCount = showList.Count;

            Console.WriteLine("*******output********************\n");

            if (ListCount == 0)
            {
                Console.WriteLine(" \t\t No Data Found \t\t");
                Console.WriteLine("\n*******output********************");
                return;
            }
            Console.WriteLine("\t[");
            ListCount = showList.Count;
            for (int list = 0; list < ListCount; list++)
            {
                WitnessRecord witness = showList[list];

                List<WitnessRecordItem> records = witness.GetWitnessRecordItems();
                int numOfRecords = records.Count;
                if (numOfRecords > 0)
                {
                    Console.WriteLine("\t [");
                    for (int recIndex = 0; recIndex < numOfRecords; recIndex++)
                    {
                        WitnessRecordItem item = records[recIndex];
                        if (numOfRecords != (recIndex + 1))
                            Console.WriteLine("\t  \"" + item.getWitness() + "\",");
                        else
                            Console.WriteLine("\t  \"" + item.getWitness() + "\"");
                    }

                    if (ListCount != (list + 1))
                        Console.WriteLine("\t ],");
                    else
                        Console.WriteLine("\t ]");
                }

            }
            Console.WriteLine("\t]");


            Console.WriteLine("\n*******output********************");
        }



        protected List<WitnessRecord> findFullMergeInPartialMerge(out bool bFullListFound)
        {
            List<WitnessRecord> showList = null;
            bFullListFound = false;
            int numOfActions = 0;
            bool bFound = false;

            

            foreach (var record in wintessList.PartialMergeWitness)
            {
                numOfActions = record.GetWitnessRecordItems().Count;
                if (GetAllWitnessActions == numOfActions)
                {
                    showList = new List<WitnessRecord>();
                    showList.Add(record);
                    bFound = true;
                    bFullListFound = true;
                    break;
                }

            }
            if (!bFound)
                showList = wintessList.PartialMergeWitness;
            return showList;
        }
    }  
}