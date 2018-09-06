using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePart : MonoBehaviour
{

    GameObject parentPuzzleFragment;

    bool hasBeenPlaced;
    Vector3 scatterToPosition;

    Vector3 beingDraggedToPosition;
    float draggedIntoPositionSpeed;
    bool isMovingTowardsDraggedPosition;

    bool hasScatteredToPosition;

    Vector3 accelerometerSpeed;
    float accelerometerMaxSpeed = 0.05f;

    string startingSpriteName;

    float animationSpeed;
    bool stopAnimationOnceComplete;

    public bool disableOnPuzzleComplete;

    Vector3 originalScale;
    bool isPickedUp;
    //float increaseInMaxScalePercent;

    bool isBouncing, isQueuedUpToBounce, isFallingFromBounce;

    GameObject shadow;

    bool stopAnimOnMouseUp;

    Vector3 moveToPosition;
    bool isMovingToMoveToPosition;

    int startingLayer;

    void Start()
    {
        startingSpriteName = GetComponent<SpriteRenderer>().sprite.name;
        //Debug.Log(startingSpriteName);

        originalScale = gameObject.transform.localScale;
        //Debug.Log(originalScale);

        startingLayer = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    }

    void Update()
    {

        if (isMovingTowardsDraggedPosition && !hasBeenPlaced)
        {
            Vector3 distance = beingDraggedToPosition - gameObject.transform.position;

            if (distance.magnitude < draggedIntoPositionSpeed * Time.deltaTime)
            {
                isMovingTowardsDraggedPosition = false;
                draggedIntoPositionSpeed = 0;
                gameObject.transform.position = beingDraggedToPosition;
            }
            else
            {
                draggedIntoPositionSpeed += 2.5f;

                if (draggedIntoPositionSpeed > 100.0f)
                    draggedIntoPositionSpeed = 100.0f;//0.6f;

                gameObject.transform.position += (distance.normalized * draggedIntoPositionSpeed * Time.deltaTime);
            }
        }
        else if(isMovingToMoveToPosition)
        {


            float speed = 30.0f;

            Vector3 distance = moveToPosition - gameObject.transform.position;

            if (distance.magnitude < speed * Time.deltaTime)
            {
                hasScatteredToPosition = true;
                isMovingToMoveToPosition = false;
                speed = 0;
                gameObject.transform.position = moveToPosition;
            }
            else
            {
                //speed += 0.04f;

                //if (speed > 1.2f)
                //    speed = 1.2f;//0.6f;

                gameObject.transform.position += (distance.normalized * speed * Time.deltaTime);
            }

        }

        //else if (hasScatteredToPosition && !hasBeenPlaced)
        //{

            ////Simulate drag
            //accelerometerSpeed.y = accelerometerSpeed.y * 0.5f;
            //accelerometerSpeed.x = accelerometerSpeed.x * 0.5f;

            ////threshold
            //Vector3 newAccel = new Vector3(Input.acceleration.x * 0.03f, Input.acceleration.y * 0.03f, 0);
            //if (Mathf.Abs(newAccel.x) < 0.006f) //0.005f) //0.01f)
            //{
            //    newAccel.x = 0;
            //}
            //if (Mathf.Abs(newAccel.y) < 0.006f)
            //{
            //    newAccel.y = 0;
            //}

            ////speed
            //accelerometerSpeed += newAccel;

            //if (Mathf.Abs(accelerometerSpeed.x) + Mathf.Abs(accelerometerSpeed.y) > accelerometerMaxSpeed)
            //    accelerometerSpeed = accelerometerSpeed.normalized * accelerometerMaxSpeed;

            //transform.Translate(accelerometerSpeed);

            //}

        if (stopAnimationOnceComplete)
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == startingSpriteName)
            {
                gameObject.GetComponent<Animator>().speed = 0;
                stopAnimationOnceComplete = false;
            }
        }

        if (isPickedUp || isBouncing)
        {

            Vector3 maxScale;

            if (isPickedUp)
                maxScale = originalScale * 1.5f;
            else
                maxScale = originalScale * 1.125f;

            Vector3 increment;

            if (isPickedUp)
                increment = originalScale * 2.5f;
            else
                increment = originalScale * 1.25f;
            
            Vector3 curScale = gameObject.transform.localScale + (increment * Time.deltaTime);

            if (curScale.x > maxScale.x)
                curScale.x = maxScale.x;

            if (curScale.y > maxScale.y)
                curScale.y = maxScale.y;

            if (curScale.z > maxScale.z)
                curScale.z = maxScale.z;

            if (curScale == maxScale && isBouncing)
            {
                isBouncing = false;
                isFallingFromBounce = true;
            }

            gameObject.transform.localScale = curScale;
        }
        else if (gameObject.transform.localScale != originalScale)
        {

            Vector3 increment;

            if (isFallingFromBounce)
                increment = originalScale * 1.25f;
            else
                increment = originalScale * 2.5f;

            Vector3 curScale = gameObject.transform.localScale - (increment * Time.deltaTime);

            if (curScale.x < originalScale.x)
                curScale.x = originalScale.x;

            if (curScale.y < originalScale.y)
                curScale.y = originalScale.y;

            if (curScale.z < originalScale.z)
                curScale.z = originalScale.z;

            gameObject.transform.localScale = curScale;

        }
        else if (isQueuedUpToBounce && gameObject.transform.localScale == originalScale)
        {
            isBouncing = true;
            isQueuedUpToBounce = false;
        }
        else if (isFallingFromBounce && gameObject.transform.localScale == originalScale)
        {
            isFallingFromBounce = false;
            
        }

        else if (startingLayer != gameObject.GetComponent<SpriteRenderer>().sortingOrder)
        {
            if(!isMovingToMoveToPosition && gameObject.transform.localScale == originalScale)
                SetOnStartingLayer();
        }
    }

    public void SetParentPuzzleFragment(GameObject ParentPuzzleFragment)
    {
        parentPuzzleFragment = ParentPuzzleFragment;
    }

    public GameObject GetParentPuzzleFragment()
    {
        return parentPuzzleFragment;
    }

    private void OnMouseDown()
    {
        if (!hasBeenPlaced && hasScatteredToPosition)
        {
            parentPuzzleFragment.GetComponent<PuzzleFragment>().GetGameplayObject().GetComponent<Gameplay>().SetPickedUpPuzzlePart(this.gameObject);
            StartAnimator(true);
            isPickedUp = true;
            isQueuedUpToBounce = true;
            isFallingFromBounce = false;
            SetOnTopLayer();
        }
    }

    private void OnMouseUp()
    {
        parentPuzzleFragment.GetComponent<PuzzleFragment>().GetGameplayObject().GetComponent<Gameplay>().ReleasePickedUpPuzzlePart();
        if (stopAnimOnMouseUp)
            StopAnimator();
        isPickedUp = false;
        isFallingFromBounce = false;

        if (!hasBeenPlaced && hasScatteredToPosition)
            SetOnSecondFromTopLayer();
    }

    public bool HasBeenPlaced()
    {
        return hasBeenPlaced;
    }

    public void SetAsPlaced()
    {
        hasBeenPlaced = true;
    }

    public void SetScatterToPosition(Vector3 position)
    {
        scatterToPosition = position;
    }

    public Vector3 GetScatterToPosition()
    {
        return scatterToPosition;
    }

    public void SetBeingDraggedToPosition(Vector3 BeingDraggedToPosition)
    {
        isMovingTowardsDraggedPosition = true;
        beingDraggedToPosition = BeingDraggedToPosition;
    }

    public void SetHasScatteredToPosition()
    {
        hasScatteredToPosition = true;
    }

    public void HasHitLeftSideOfScreen()
    {
        accelerometerSpeed.x = 0;
    }

    public void HasHitRightSideOfScreen()
    {
        accelerometerSpeed.x = 0;
    }

    public void HasHitTopOfScreen()
    {
        accelerometerSpeed.y = 0;
    }

    public void HasHitBottomOfScreen()
    {
        accelerometerSpeed.y = 0;
    }

    public string GetStartingSpriteName()
    {
        return startingSpriteName;
    }

    public void StoreAnimationSpeed()
    {
        animationSpeed = gameObject.GetComponent<Animator>().speed;
    }

    public void StartAnimator(bool StopAnimOnMouseUp)
    {
        if (gameObject.GetComponent<Animator>() != null)
        {
            gameObject.GetComponent<Animator>().speed = animationSpeed;
            stopAnimationOnceComplete = false;
            stopAnimOnMouseUp = StopAnimOnMouseUp;
        }
    }

    public void StopAnimator()
    {
        if (gameObject.GetComponent<Animator>() != null)
            stopAnimationOnceComplete = true;
    }

    public float GetExtraScalePercent()
    {
        float p = (transform.localScale.x / originalScale.x - 1f) / 10f;

        return p;
    }

    public void SetShadow(GameObject Shadow)
    {
        shadow = Shadow;
    }

    public GameObject GetShadow()
    {
        return shadow;
    }

    public void SetMoveToPosition(Vector3 position)
    {
        moveToPosition = position;
        isMovingToMoveToPosition = true;
        isMovingTowardsDraggedPosition = false;
    }

    private void SetOnTopLayer()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 32;
        shadow.GetComponent<SpriteRenderer>().sortingOrder = 31;
    }

    private void SetOnSecondFromTopLayer()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 30;
        shadow.GetComponent<SpriteRenderer>().sortingOrder =29;
    }

    private void SetOnStartingLayer()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = startingLayer;
        shadow.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }


    

}
