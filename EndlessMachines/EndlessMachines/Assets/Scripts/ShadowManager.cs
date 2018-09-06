using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{

    GameObject puzzlePartParent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = puzzlePartParent.GetComponent<SpriteRenderer>().sprite;

        //Vector3 centerScreen = Vector3.zero;

        Vector3 pos = puzzlePartParent.transform.position;

        pos = pos * puzzlePartParent.GetComponent<PuzzlePart>().GetExtraScalePercent() + pos;//new Vector3(0.1f, 0.1f, 0.1f);

        gameObject.transform.position = pos;


    }

    public void SetPuzzlePartParent(GameObject PuzzlePartParent)
    {
        puzzlePartParent = PuzzlePartParent;
        puzzlePartParent.GetComponent<PuzzlePart>().SetShadow(gameObject);
    }


}
