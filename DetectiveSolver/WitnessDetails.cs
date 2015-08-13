
/*******************************************************************************************************************
 * Author : Venkata Narayana Terli
 * mail id : venkat.terli@gmail.com
 * date : 04-Aug-15
 * Summary : Main purpose for this file, create Witness Record with help of Witness remeber items
 *           properties and group name will be helpful for witness items state
 * ****************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DetectiveSolver
{

    class WitnessList
    {
        List<WitnessRecord> ActualRecords;
        List<WitnessRecord> FullMerge;
        List<WitnessRecord> PartialMerge;
        List<WitnessRecord> NoMerge;
        Dictionary<WitnessRecord, List<WitnessRecord>> MatchedList;
        public WitnessList()
        {
            ActualRecords = new List<WitnessRecord>();
            FullMerge = new List<WitnessRecord>();
            PartialMerge = new List<WitnessRecord>();
            NoMerge = new List<WitnessRecord>();
            MatchedList = new Dictionary<WitnessRecord, List<WitnessRecord>>();
        }

        public WitnessList(ref WitnessList cloneObj)
        {
            ActualRecords = new List<WitnessRecord>();
            FullMerge = new List<WitnessRecord>();
            PartialMerge = new List<WitnessRecord>();
            NoMerge = new List<WitnessRecord>();
            MatchedList = new Dictionary<WitnessRecord, List<WitnessRecord>>();

            foreach (var ARec in cloneObj.ActualRecords)
                ActualRecords.Add(ARec);


            foreach (var ARec in cloneObj.FullMergeWintess)
                FullMerge.Add(ARec);


            foreach (var ARec in cloneObj.PartialMergeWitness)
                PartialMerge.Add(ARec);


            foreach (var ARec in cloneObj.NoMergeWitness)
                NoMerge.Add(ARec);

        }

        public void addNewWitness(WitnessRecord record)
        {
            ActualRecords.Add(record);
        }

        public void addToFullMerge(WitnessRecord record)
        {
            if(!FullMerge.Contains(record))
                FullMerge.Add(record);
        }

        public void addToPartialMerge(WitnessRecord record)
        {
            //PartialMerge.Add(record);
            if(!PartialMerge.Contains(record))
                PartialMerge.Insert(0, record);
             
        }
        
        public void addNoMerge(WitnessRecord record)
        {
            NoMerge.Add(record);
        }

        public void addToMatchedList(WitnessRecord key,List<WitnessRecord> value)
        {
            if (!MatchedList.ContainsKey(key))
            {                 
                MatchedList.Add(key, value);
            }             
            
        }
        public bool MatchListStatus()
        {
            bool bResult = false;

            if (MatchedList.Count != 0)
                bResult = true;
            return bResult;
        }
        public Dictionary<WitnessRecord, List<WitnessRecord>> GetMatchedList
        {
            get { return MatchedList; }
        }
        public List<WitnessRecord> GetWitnessList
        {
            get { return ActualRecords; }
        }
        public List<WitnessRecord> AddWitness
        {            
            set { ActualRecords = value; }
        }

        public List<WitnessRecord> FullMergeWintess
        {
            get { return FullMerge; }

            set { FullMerge = value; }
        }

        public List<WitnessRecord> PartialMergeWitness
        {
            get { return PartialMerge; }

            set { PartialMerge = value; }
        }

        public List<WitnessRecord> NoMergeWitness
        {
            get { return NoMerge; }

            set { NoMerge = value; }
        }


        public void RemoveAllFromFullMergeWintess()
        {
            FullMerge.Clear();             
        }

        

    }


    class WitnessRecord
    {
        string witness;
        List<WitnessRecordItem> items;
        int ItemPos;
        int vistedItemIndex;
        bool partialVisit;
        List<string> prevItemWitnessList;
        public WitnessRecord(string witnessName)
        {
            items = new List<WitnessRecordItem>();
            this.witness = witnessName;
            ItemPos = 0;
            vistedItemIndex = -1;
            partialVisit = false;
            prevItemWitnessList = new List<string>();
        }
        public void SetPrevItemMatched(bool bResult, string witnessName)
        {
            if (bResult)
            {
                if (!prevItemWitnessList.Contains(witnessName))
                    prevItemWitnessList.Add(witnessName);
            }
            else
                prevItemWitnessList.Remove(witnessName);
        }

        public bool GetPrevItemMatched(string witnessName)
        {
            if (prevItemWitnessList.Count == 0) return false;
            return prevItemWitnessList.Contains(witnessName);
        }

        public string getWitnessName
        {
            get { return witness; }
        }
        public void addItemToList(WitnessRecordItem props)
        {
            if (ItemPos == 0)
                props.IsFirstItem = true;

            props.WitnessPos = ItemPos;
            items.Add(props);
            ItemPos++;
        }
        public List<WitnessRecordItem> GetWitnessRecordItems()
        {
            return items;
        }


        public bool PartialList
        {
            set { partialVisit = value; }
            get { return partialVisit; }
        }
        public int LastVistedItem
        {
            set { vistedItemIndex = value; }
            get { return vistedItemIndex; }
        }

    }

    class WitnessRecordItem
    {
        string WitnessData;
        int WIndex;
        bool FirstItem;
        bool LastItem;
        bool Visited;
        string WGroup;
        bool prevItem;
        public WitnessRecordItem(string data, string Group, bool lastItem)
        {
            this.WitnessData = data;
            this.WGroup = Group;
            this.FirstItem = false;
            this.LastItem = lastItem;
            this.Visited = false;
            this.prevItem = false;

        }

        public string WitnessGroup
        {
            get { return WGroup; }
        }

        public bool isPrevItemMatched
        {
            get { return prevItem; }
            set { prevItem = value; }
        }

        public string getWitness()
        {
            return WitnessData;
        }

        public string getWitness(int index)
        {
            if (index == WIndex)
                return WitnessData;
            else
                return string.Empty;
        }
        public int WitnessPos
        {
            get { return WIndex; }
            set { WIndex = value; }
        }

        public bool IsFirstItem
        {
            get { return FirstItem; }
            set { FirstItem = value; }
        }
        public bool isLastItem
        {
            get { return LastItem; }
            set { LastItem = value; }
        }

        public bool VistedItem
        {
            get { return Visited; }
            set { Visited = value; }
        }
    }

    
}
