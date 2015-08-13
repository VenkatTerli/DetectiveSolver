
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
        
        private int TotalActions;

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
                CreateTimeLines();

                ShowResult();
            }
            return;
        }


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

      
       
 
        public void CreateTimeLines()
        {
            bool bSplitNtRqd = false;
            WitnessList revList = null;
            WitnessList tempList = new WitnessList();
            for (int i =0;i<wintessList.GetWitnessList.Count ;i++)
            {
                revList = null;
                for (int j = i + 1; j < wintessList.GetWitnessList.Count; j++)
                {
                    var record1 = wintessList.GetWitnessList[i];
                    var record2 = wintessList.GetWitnessList[j];                    
                    var newWitnessList = ConstraintMatching(record1, record2, bSplitNtRqd);
                     
                    if (revList != null)
                        copyToNewWitnessList(ref newWitnessList,ref revList);

                    revList = CombineMaxTimeLines(newWitnessList);
                                        
                }
 
                if (revList == null) continue;

                copyToNewWitnessList(ref tempList, ref revList);
                tempList = CombineMaxTimeLines(tempList);
  
            }

            copyToMasterlist(tempList);

        }

         
        private WitnessList FindBestMatchInFullList(WitnessRecord W1Record, WitnessRecord W2Record, bool bNoSplit)
        {
            var revWitnesList = ConstraintMatching(W1Record, W2Record, bNoSplit);
             
            var W1RecordLen = W1Record.GetWitnessRecordItems().Count;
            var W2RecordLen = W2Record.GetWitnessRecordItems().Count;
            var revRecordLen = revWitnesList.FullMergeWintess[0].GetWitnessRecordItems().Count;

            if (!((revRecordLen > W1RecordLen) && (revRecordLen > W2RecordLen)))
            {
                
                revWitnesList.addToPartialMerge(W1Record);
                revWitnesList.addToPartialMerge(W2Record);
                
                revWitnesList.FullMergeWintess.Clear();

            }

            return revWitnesList;
        }

        private WitnessList FindBestMatchInPartialList(ref WitnessList newWitnessList)
        {
            var total = newWitnessList.FullMergeWintess.Count;
            var parList = newWitnessList.PartialMergeWitness;
            WitnessList revList = new WitnessList();//ref wintessList);
            if (total == 1)
                newWitnessList.addToPartialMerge(newWitnessList.FullMergeWintess[0]);
            
            
            for (int i = 0; i < parList.Count; i++)
            {
                for (int j = i + 1; j < parList.Count; j++)
                {
                    var revTempWitnesList = ConstraintMatching(parList[i], parList[j], true);

                    if (revTempWitnesList.FullMergeWintess.Count > 0)
                    {
                        var W1RecordLen = parList[i].GetWitnessRecordItems().Count;
                        var W2RecordLen = parList[j].GetWitnessRecordItems().Count;
                        var revRecordLen = revTempWitnesList.FullMergeWintess[0].GetWitnessRecordItems().Count;

                        if (!((revRecordLen > W1RecordLen) && (revRecordLen > W2RecordLen)))
                        {
                            revList.addToPartialMerge(parList[i]);
                            revList.addToPartialMerge(parList[j]);
                        }
                        else
                            revList.addToFullMerge(revTempWitnesList.FullMergeWintess[0]);

                    }
                    else
                    {
                        if (revTempWitnesList.PartialMergeWitness.Count > 0)
                        {
                            foreach (var record in revTempWitnesList.PartialMergeWitness)
                            {
                                revList.addToPartialMerge(record);
                            }
                        }
                    }

                }
            }
            if (revList.FullMergeWintess.Count > 1)
            {
                foreach(var record in revList.FullMergeWintess)
                {
                    revList.addToPartialMerge(record);
                }
                revList.FullMergeWintess.Clear();

            }

            return revList;
        }

        private WitnessList CombineMaxTimeLines(WitnessList newWitnessList)
        {
            var total = newWitnessList.FullMergeWintess.Count;
            WitnessList revList = null;
            if (total == 2)
            {
               revList =  FindBestMatchInFullList(newWitnessList.FullMergeWintess[0], newWitnessList.FullMergeWintess[1], true);
            }
            else
            {
                var PMergeTotal = newWitnessList.PartialMergeWitness.Count;
                if (PMergeTotal == 0 && total == 1)
                {
                    revList = newWitnessList;
                }
                else
                {
                   revList= FindBestMatchInPartialList(ref newWitnessList);
                }
            }
            if (revList == null)
                revList = newWitnessList;
            return revList;
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
        
        
        protected WitnessList ConstraintMatching(WitnessRecord WitnessOne, WitnessRecord WitnessTwo, bool bIgnoreIfSplit)
        {

            WitnessList newWitnessList = new WitnessList(ref wintessList);
                         
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
                                    int stIndex = stPos > 0 ? (stPos - 1) : -1;
                                    if (stIndex >= 0)
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
                                    int stIndex = stPos > 0 ? (stPos - 1) : -1;
                                    if (stIndex >= 0)
                                        CopyTillLastMatch(ref newRecordItems, WitnessTwoItemsList[stIndex].getWitness(), ref subWitnessitems);
                                    int matchedItemPos = stPos;
                                    bool bVisit = false;
                                    for (int j = stPos; j < WitnessTwoItemsList.Count; j++)
                                    {
                                        if (witnessItem_W2.Equals(WitnessTwoItemsList[j].getWitness()))
                                        {
                                            if (!bVisit)
                                            {
                                                matchedItemPos = j;
                                                bVisit = true;
                                            }
                                        }

                                        subWitnessitems.Add(WitnessTwoItemsList[j].getWitness());
                                    }

                                    for (int j = matchedItemPos; j < WitnessTwoItemsList.Count; j++)
                                    {
                                        newRecordItems.Add(WitnessTwoItemsList[j].getWitness());
                                    }
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
                                if ((iLastVistedPos + 1) > WitnessTwoItemsList.Count) // No more elements in W2
                                {
                                    if (subWitnessitems == null)
                                        subWitnessitems = new List<string>();
                                    subWitnessitems.Add(witnessItem_W1);
                                }
                            }
                        }
                        newRecordItems.Add(witnessItem_W1);
                        WitnessOne.SetPrevItemMatched(false, groupName);
                    }

                }


                if (!bSplit)
                {
                    newWitnessList.addToFullMerge(CreateWitnessRecord(newRecordItems));
                }
                else
                {

                    if (!bIgnoreIfSplit)
                    {
                        if (subWitnessitems != null)
                        {
                            newWitnessList.addToPartialMerge(CreateWitnessRecord(subWitnessitems));
                        }

                        newWitnessList.addToPartialMerge(CreateWitnessRecord(newRecordItems));
                    }
                    else
                        newWitnessList.addToPartialMerge(WitnessTwo);
                }
 

            }
            catch (Exception e)
            {
                Console.WriteLine("Contraint matching error" + e.Message);
            }
            finally
            {
                WitnessTwo.LastVistedItem = -1;
            }

            return newWitnessList;
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
                    //Final processing
                    var record1 = wintessList.FullMergeWintess[0];

                    showList.Insert(0, record1);

                    if (wintessList.FullMergeWintess[0].GetWitnessRecordItems().Count == GetAllWitnessActions)
                    {
                        showList = wintessList.FullMergeWintess;
                        bFullListFound = true;
                    }


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
 
    }  
}