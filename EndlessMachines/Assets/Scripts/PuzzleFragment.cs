using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFragment : MonoBehaviour {

    GameObject gameplayObject;
    GameObject puzzlePartObject;
    GameObject puzzleSlotObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetGameplayObject(GameObject GameplayObject)
    {
        gameplayObject = GameplayObject;
    }

    public void SetPuzzlePartObject(GameObject PuzzlePartObject)
    {
        puzzlePartObject = PuzzlePartObject;
    }

    public void SetPuzzleSlotObject(GameObject PuzzleSlotObject)
    {
        puzzleSlotObject = PuzzleSlotObject;
    }

    public GameObject GetGameplayObject()
    {
        return gameplayObject;
    }

    public GameObject GetPuzzlePartObject()
    {
        return puzzlePartObject;
    }

    public GameObject GetPuzzleSlotObject()
    {
        return puzzleSlotObject;
    }



}
