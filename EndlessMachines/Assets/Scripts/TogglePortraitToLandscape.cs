using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePortraitAndLandscape : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (Screen.orientation != ScreenOrientation.Portrait)
            Screen.orientation = ScreenOrientation.Portrait;
        else
            Screen.orientation = ScreenOrientation.Landscape;

    }

}
