using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour {

    GameObject parentPuzzleFragment;

    bool hasBeenFilled;

    public bool disableOnPuzzleComplete = true;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetParentPuzzleFragment(GameObject ParentPuzzleFragment)
    {
        parentPuzzleFragment = ParentPuzzleFragment;
    }

    public GameObject GetParentPuzzleFragment()
    {
        return parentPuzzleFragment;
    }

    public bool HasBeenFilled()
    {
        return hasBeenFilled;
    }

    public void SetAsFilled()
    {
        hasBeenFilled = true;
    }

    //private void OnMouseEnter()
    //{
    //    //parentPuzzleFragment.GetComponent<PuzzleFragment>().GetGameplayObject().GetComponent<Gameplay>().SetFingerOverPuzzleSlot(this.gameObject);
    //}

    //private void OnMouseExit()
    //{
    //    //parentPuzzleFragment.GetComponent<PuzzleFragment>().GetGameplayObject().GetComponent<Gameplay>().ReleaseFingerOverPuzzleSlot(this.gameObject);
    //}


    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.name == "player")
    //    {

    //        // playerCollides with the Enemy

    //    }
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.name == "player")
    //    {

    //        // player is not colliding  with the Enemy  anymore

    //    }
    //}


}
