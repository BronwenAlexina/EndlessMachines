using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDepthManager : MonoBehaviour
{

    public double projectionSizeAt4x3Ratio;
    public double projectionSizeAt16x9Ratio;

    // Use this for initialization
    void Start()
    {
        SetScreenProjectionSize();
    }

    // Update is called once per frame
    void Update()
    {

        //uncomment while debugging screen size changes, comment out before publishing
        SetScreenProjectionSize();
    }

    private void SetScreenProjectionSize()
    {
        if (projectionSizeAt16x9Ratio <= 0 || projectionSizeAt4x3Ratio <= 0)
            return;

        if (projectionSizeAt16x9Ratio == projectionSizeAt4x3Ratio)
        {
            gameObject.GetComponent<Camera>().orthographicSize = (float)projectionSizeAt16x9Ratio;
            return;
        }


        // y = camera projection size, x = screen ratio
        // y = mx + b

        double screenRatio = (double)Screen.width / (double)Screen.height;

        if (Screen.height > Screen.width)
            screenRatio = (double)Screen.height / (double)Screen.width;

        //calculate slope
        // m = (y2 - y1) / (x2 - x1)

        double x1 = 4f / 3f;
        double x2 = 16f / 9f;

        double m = (projectionSizeAt16x9Ratio - projectionSizeAt4x3Ratio) / (x2 - x1);

        //Debug.Log("slope = " + m);

        //b = y - mx
        double b = projectionSizeAt16x9Ratio - (m * x2);

        //Debug.Log("b = " + b);

        // y = camera projection size, x = current ratio
        // y = mx + b

        double currentScreenDepth = (m * screenRatio) + b;

        //Debug.Log("screen depth = " + currentScreenDepth);

        gameObject.GetComponent<Camera>().orthographicSize = (float)currentScreenDepth;

    }


}
