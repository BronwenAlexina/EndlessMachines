using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

static public class LevelDataManager
{

    static private string filePath = Application.persistentDataPath + "/levelData.txt";

    static private LinkedList<string> completedLevels;

    static public void Load()
    {
        completedLevels = new LinkedList<string>();

        //File.Delete(filePath);

        if (File.Exists(filePath))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = null;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] procStr = line.Split(',');

                    if (procStr[0] == "0")
                        completedLevels.AddLast(procStr[1]);

                    //Debug.Log(line);

                }
            }
        }

    }

    static public void Save()
    {
        if (completedLevels == null)
            return;

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            foreach(string lvl in completedLevels)
                sw.WriteLine("0," + lvl);
        }
    }

    static public void AddLevelToCompletedList(string levelName)
    {
        if (completedLevels == null)
            return;

        if (!completedLevels.Contains(levelName))
            completedLevels.AddLast(levelName);
    }

    static public bool HasLevelBeenCompleted(string levelName)
    {
        if (completedLevels == null)
            return false;

        return completedLevels.Contains(levelName);
    }

}

