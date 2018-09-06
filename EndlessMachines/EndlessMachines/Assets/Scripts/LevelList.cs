using System.Collections;
using System.Collections.Generic;
using UnityEngine;



static public class LevelList
{

    static private LinkedList<string> allLevelsByName;

    static public void Init()
    {
        allLevelsByName = new LinkedList<string>();     
    }

    static public void AddLevelNameToList(string levelName)
    {
        if (allLevelsByName == null)
            return;

        allLevelsByName.AddLast(levelName);
    }

    static public string GetNextLevelName(string currentLevelName)
    {
        if (allLevelsByName == null)
            return "";

        bool wasFound = false;

        foreach(string lvl in allLevelsByName)
        {
            if(wasFound)
                return lvl;

            if (lvl == currentLevelName)
                wasFound = true;
        }

        return "";
    }

    static public string GetPreviousLevelName(string currentLevelName)
    {
        if (allLevelsByName == null)
            return "";

        string prev = "";

        foreach (string lvl in allLevelsByName)
        {
            if (lvl == currentLevelName)
                return prev;

            prev = lvl;
        }

        return "";
    }


}
