using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{


    LinkedList<GameObject> puzzleFragments;

    GameObject pickedUpPuzzlePart;

    LinkedList<GameObject> lastHeldPuzzleParts;

    bool hasScattered;
    float timer = 0.0f;

    int gameplayState;

    GameObject uiCamera;
    GameObject nextLevelButton, previousLevelButton, mainMenuButton;

    GameObject soundEffectsManager, musicTrackManager;

    GameObject descriptionButton;

    public GameObject[] gameObjectsToEnableOnPuzzleCompletion;
    public GameObject[] gameObjectsToDisableOnPuzzleCompletion;

    GameObject shadowForPuzzleParts;

    private void PuzzleHasBeenCompletedEvent()
    {

        musicTrackManager.GetComponent<GameplayMusicTrackManager>().PlayMusicTrack(MusicTrackIDs.VictoryFanfare);

        foreach (GameObject go in gameObjectsToEnableOnPuzzleCompletion)
            go.SetActive(true);

        foreach (GameObject go in gameObjectsToDisableOnPuzzleCompletion)
            go.SetActive(false);

        foreach (GameObject go in puzzleFragments)
        {
            GameObject puzzlePart = go.GetComponent<PuzzleFragment>().GetPuzzlePartObject();
            puzzlePart.SetActive(!puzzlePart.GetComponent<PuzzlePart>().disableOnPuzzleComplete);

            GameObject puzzleSlot = go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject();
            puzzleSlot.SetActive(!puzzleSlot.GetComponent<PuzzleSlot>().disableOnPuzzleComplete);
        }


        string lvlName = SceneManager.GetActiveScene().name;

        if (lvlName == "Level1")
        {
            Debug.Log("Level1 has been completed");
        }
        else if (lvlName == "Level2")
        {
            Debug.Log("Level2 has been completed");
        }
        else if (lvlName == "Level3")
        {

        }
        else if (lvlName == "PrefabBuilderForGameplay")
        {
            foreach (GameObject go in puzzleFragments)
            {
                GameObject puzzlePart = go.GetComponent<PuzzleFragment>().GetPuzzlePartObject();

                if (puzzlePart.activeInHierarchy)
                    puzzlePart.GetComponent<PuzzlePart>().StartAnimator(false);
            }
        }


        ////Bronwen, if you prefer using switch/case/break, feel free to switch out the solution for spefic level selection.  I've always thought if/else to be more clear to look at.
        //switch (lvlName)
        //{
        //    case "Level1":
        //        Debug.Log("Level1 has been completed");
        //        break;

        //    case "Level2":
        //        Debug.Log("Level2 has been completed");
        //        break;

        //    case "Level3":

        //        break;

        //    case "PrefabBuilderForGameplay":
        //        foreach (GameObject go in puzzleFragments)
        //        {
        //            GameObject puzzlePart = go.GetComponent<PuzzleFragment>().GetPuzzlePartObject();

        //            if (puzzlePart.activeInHierarchy)
        //                puzzlePart.GetComponent<PuzzlePart>().StartAnimator();
        //        }
        //        break;

        //    default:
        //        break;
        //}



    }

    void Start()
    {

        puzzleFragments = new LinkedList<GameObject>();
        lastHeldPuzzleParts = new LinkedList<GameObject>();

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        //Debug.Log("there are " + allObjects.Length);

        foreach (GameObject go in allObjects)
        {
            PuzzleFragment pf = go.GetComponent<PuzzleFragment>();

            if (pf != null)
            {
                puzzleFragments.AddLast(pf.gameObject);
                pf.SetGameplayObject(this.gameObject);

                foreach (PuzzlePart pp in pf.gameObject.GetComponentsInChildren<PuzzlePart>())
                {
                    pf.SetPuzzlePartObject(pp.gameObject);
                    pp.SetParentPuzzleFragment(pf.gameObject);
                }

                foreach (PuzzleSlot ps in pf.gameObject.GetComponentsInChildren<PuzzleSlot>())
                {
                    pf.SetPuzzleSlotObject(ps.gameObject);
                    ps.SetParentPuzzleFragment(pf.gameObject);
                }

            }

            //CameraDepthManager cdm = go.GetComponent<CameraDepthManager>();
            //if (cdm != null)
            //mainCamera = Camera.main.gameObject;//go;

            LoadNextLevelButton lnlb = go.GetComponent<LoadNextLevelButton>();
            if (lnlb != null)
            {
                nextLevelButton = go;
                go.SetActive(false);
            }

            LoadPreviousLevelButton lplb = go.GetComponent<LoadPreviousLevelButton>();
            if (lplb != null)
            {
                previousLevelButton = go;
                go.SetActive(false);
            }

            LoadSceneButton lsb = go.GetComponent<LoadSceneButton>();
            if (lsb != null)
                mainMenuButton = go;

            Camera cam = go.GetComponent<Camera>();
            if (cam != null)
            {
                if (cam.gameObject != Camera.main.gameObject)
                    uiCamera = cam.gameObject;
            }

            GameplaySoundEffectsManager gsem = go.GetComponent<GameplaySoundEffectsManager>();
            if (gsem != null)
                soundEffectsManager = go;

            GameplayMusicTrackManager gmtm = go.GetComponent<GameplayMusicTrackManager>();
            if (gmtm != null)
                musicTrackManager = go;

            DescriptionButton db = go.GetComponent<DescriptionButton>();
            if (db != null)
                descriptionButton = go;

            ShadowManager sm = go.GetComponent<ShadowManager>();
            if (sm != null)
                shadowForPuzzleParts = go;

        }

        foreach (GameObject go in puzzleFragments)
        {
            GameObject puzzlePart = go.GetComponent<PuzzleFragment>().GetPuzzlePartObject();

            puzzlePart.GetComponent<PuzzlePart>().SetScatterToPosition(puzzlePart.transform.position);
            puzzlePart.transform.position = go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().transform.position;

            if (puzzlePart.GetComponent<Animator>() != null)
            {
                puzzlePart.GetComponent<PuzzlePart>().StoreAnimationSpeed();
                puzzlePart.GetComponent<Animator>().speed = 0;
            }

            GameObject newShadow = Instantiate(shadowForPuzzleParts, puzzlePart.transform);
            newShadow.GetComponent<ShadowManager>().SetPuzzlePartParent(puzzlePart);
        }

        shadowForPuzzleParts.SetActive(false);

        Vector3 temp = uiCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(((float)Screen.width), ((float)Screen.height), 0));
        float screenTop = temp.y;
        float screenRightSide = temp.x;

        temp = uiCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, 0, 0));
        float screenBottom = temp.y;
        float screenLeftSide = temp.x;

        Bounds b = nextLevelButton.GetComponent<SpriteRenderer>().bounds;
        nextLevelButton.transform.position = new Vector3(screenRightSide - b.extents.x - 0.2f, uiCamera.transform.position.y, 0);

        b = previousLevelButton.GetComponent<SpriteRenderer>().bounds;
        previousLevelButton.transform.position = new Vector3(screenLeftSide + b.extents.x + 0.2f, uiCamera.transform.position.y, 0);

        b = mainMenuButton.GetComponent<SpriteRenderer>().bounds;
        mainMenuButton.transform.position = new Vector3(screenLeftSide + b.extents.x + 0.3f, screenTop - b.extents.y - 0.2f, 0);

        b = mainMenuButton.GetComponent<SpriteRenderer>().bounds;
        descriptionButton.transform.position = new Vector3(screenRightSide - b.extents.x - 0.3f, screenBottom + b.extents.y + 0.2f, 0); 

        nextLevelButton.GetComponent<LoadNextLevelButton>().SetSoundEffectsManager(soundEffectsManager);
        previousLevelButton.GetComponent<LoadPreviousLevelButton>().SetSoundEffectsManager(soundEffectsManager);
        mainMenuButton.GetComponent<LoadSceneButton>().SetSoundEffectsManager(soundEffectsManager);

        gameplayState = GamePlayStates.Animating;

        musicTrackManager.GetComponent<GameplayMusicTrackManager>().PlayMusicTrack(MusicTrackIDs.PuzzleSolvingTheme);

    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    ListRaycasts();
        //}

        //for (int i = 0; i < Input.touchCount; ++i)
        //{
        //    if (Input.GetTouch(i).phase == TouchPhase.Began)
        //        Debug.Log(Input.GetTouch(i).position);
        //}

        if (pickedUpPuzzlePart != null)
        {
            //Camera cam = mainCamera.GetComponent<Camera>();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -1 * Camera.main.GetComponent<Transform>().position.z;

            Vector3 pz = Camera.main.ScreenToWorldPoint(mousePos);//Input.mousePosition);

            //Vector3 pz = GetWorldPositionOnPlane(Input.mousePosition, -10);

            pz.z = 0;

            pickedUpPuzzlePart.GetComponent<PuzzlePart>().SetBeingDraggedToPosition(pz);
            //pickedUpPuzzlePart.transform.position = pz;
        }

        if (!hasScattered)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                hasScattered = true;
                gameplayState = GamePlayStates.Playing;

                foreach (GameObject go in puzzleFragments)
                {
                    GameObject puzzlePart = go.GetComponent<PuzzleFragment>().GetPuzzlePartObject();
                    //puzzlePart.transform.position = puzzlePart.GetComponent<PuzzlePart>().GetScatterToPosition();
                    puzzlePart.GetComponent<PuzzlePart>().SetMoveToPosition(puzzlePart.GetComponent<PuzzlePart>().GetScatterToPosition());
                    //puzzlePart.GetComponent<PuzzlePart>().SetHasScatteredToPosition();
                }

            }
        }

        //if (Microphone.devices.Length > 0)
        //    Debug.Log(Microphone.devices[0]);

        //Debug.Log(Input.acceleration);

        //Screen.orientation = ScreenOrientation.Portrait;

        //Debug.Log(Input.gyro.userAcceleration);

        //RefreshLayersForPuzzleFragments();

    }

    private void LateUpdate()
    {
        EnsurePuzzlePartsAreInScreenSpace();
    }

    public void SetPickedUpPuzzlePart(GameObject PickedUpPuzzlePart)
    {
        if (gameplayState == GamePlayStates.Playing)
        {
            pickedUpPuzzlePart = PickedUpPuzzlePart;
            pickedUpPuzzlePart.GetComponent<SpriteRenderer>().sortingOrder = 31;
            pickedUpPuzzlePart.GetComponent<PuzzlePart>().GetShadow().GetComponent<SpriteRenderer>().sortingOrder = 30;


            soundEffectsManager.GetComponent<GameplaySoundEffectsManager>().PlaySoundEffect(SoundEffectIDs.puzzlePartPickedUp);
            NetworkClient.SendToServer("Part Picked Up " + pickedUpPuzzlePart.GetComponent<PuzzlePart>().GetStartingSpriteName());
        }
    }

    public void ReleasePickedUpPuzzlePart()
    {

        if (pickedUpPuzzlePart == null)
            return;

        Collider2D collider, collider2;

        collider = pickedUpPuzzlePart.GetComponent<Collider2D>();

        bool sndFxWasPlayed = false;

        foreach (GameObject go in puzzleFragments)
        {
            if (go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().GetComponent<PuzzleSlot>().HasBeenFilled())
                continue;

            bool canBePlacedIntoSlot = false;
            GameObject parentPuzzleFragmentOfPickedUpPuzzlePart = pickedUpPuzzlePart.GetComponent<PuzzlePart>().GetParentPuzzleFragment();
            SpriteRenderer puzzlePartFromParentFragment = parentPuzzleFragmentOfPickedUpPuzzlePart.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<SpriteRenderer>();
            //string nameOfStartingSpriteFor = 

            if (go == parentPuzzleFragmentOfPickedUpPuzzlePart)
                canBePlacedIntoSlot = true;

            //Debug.Log(go.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<SpriteRenderer>().sprite.name == puzzlePartFromParentFragment.sprite.name);

            //if ((go.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<SpriteRenderer>().sprite.name == puzzlePartFromParentFragment.sprite.name) && (go.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<SpriteRenderer>().color == puzzlePartFromParentFragment.color))
            if ((go.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<PuzzlePart>().GetStartingSpriteName() == parentPuzzleFragmentOfPickedUpPuzzlePart.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<PuzzlePart>().GetStartingSpriteName()) && (go.GetComponent<PuzzleFragment>().GetPuzzlePartObject().GetComponent<SpriteRenderer>().color == puzzlePartFromParentFragment.color))
                canBePlacedIntoSlot = true;

            if (canBePlacedIntoSlot)
            {
                collider2 = go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().GetComponent<Collider2D>();

                if (collider.bounds.Intersects(collider2.bounds))
                {
                    pickedUpPuzzlePart.GetComponent<PuzzlePart>().SetAsPlaced();
                    go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().GetComponent<PuzzleSlot>().SetAsFilled();

                    pickedUpPuzzlePart.GetComponent<PuzzlePart>().SetMoveToPosition(go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().transform.position);
                    //pickedUpPuzzlePart.transform.position = go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().transform.position;

                    sndFxWasPlayed = true;
                    soundEffectsManager.GetComponent<GameplaySoundEffectsManager>().PlaySoundEffect(SoundEffectIDs.puzzlePartPlacced);
                    break;
                }
            }

        }

        if (!sndFxWasPlayed)
        {
            soundEffectsManager.GetComponent<GameplaySoundEffectsManager>().PlaySoundEffect(SoundEffectIDs.puzzleParpuzzlePartMisplacedtPlacced);
            pickedUpPuzzlePart.GetComponent<PuzzlePart>().SetMoveToPosition(pickedUpPuzzlePart.GetComponent<PuzzlePart>().GetScatterToPosition());
        }
            

        if (lastHeldPuzzleParts.Contains(pickedUpPuzzlePart))
            lastHeldPuzzleParts.Remove(pickedUpPuzzlePart);

        lastHeldPuzzleParts.AddLast(pickedUpPuzzlePart);

        //RefreshLayersForPuzzleFragments();

        pickedUpPuzzlePart = null;

#if UNITY_IOS
            Handheld.Vibrate();
#endif

#if UNITY_ANDROID
            Handheld.Vibrate();
#endif


        CheckIfPuzzleIsComplete();

    }

    private void CheckIfPuzzleIsComplete()
    {
        bool isComplete = true;

        foreach (GameObject go in puzzleFragments)
        {
            if (!go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject().GetComponent<PuzzleSlot>().HasBeenFilled())
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            //Debug.Log("Is Complete");
            LevelDataManager.AddLevelToCompletedList(SceneManager.GetActiveScene().name);
            LevelDataManager.Save();

            if (nextLevelButton != null)
            {
                nextLevelButton.GetComponent<LoadNextLevelButton>().Init();
                //Debug.Log(nextLevelButton.GetComponent<LoadNextLevelButton>().GetNameOfLevelToLoad());
                if (nextLevelButton.GetComponent<LoadNextLevelButton>().GetNameOfLevelToLoad() != "")
                    nextLevelButton.SetActive(true);
            }

            if (previousLevelButton != null)
            {
                previousLevelButton.GetComponent<LoadPreviousLevelButton>().Init();

                if (previousLevelButton.GetComponent<LoadPreviousLevelButton>().GetNameOfLevelToLoad() != "")
                    previousLevelButton.SetActive(true);
            }

            PuzzleHasBeenCompletedEvent();
        }



    }

    private void RefreshLayersForPuzzleFragments()
    {


//        1 to 3 - background layers
//4 - filled silhouette layer
//5 - placed part layer
//6 - unfilled silhouette layer
//7 - unplacedd part layer
//8 - moving to position part layer
//9 - last held part's shadow layer
//10 - last held part layer
//11 - picked up part's shadow layer
//12 - picked up part layer
//13 + -Foreground layers


        foreach (GameObject go in puzzleFragments)
        {
            GameObject pp = go.GetComponent<PuzzleFragment>().GetPuzzlePartObject();
            GameObject ps = go.GetComponent<PuzzleFragment>().GetPuzzleSlotObject();

            pp.GetComponent<PuzzlePart>().GetShadow().GetComponent<SpriteRenderer>().sortingOrder = 0;

            if (pp.GetComponent<PuzzlePart>().HasBeenPlaced())
                pp.GetComponent<SpriteRenderer>().sortingOrder = 5;
            else
                pp.GetComponent<SpriteRenderer>().sortingOrder = 7;

            if (ps.GetComponent<PuzzleSlot>().HasBeenFilled())
                ps.GetComponent<SpriteRenderer>().sortingOrder = 4;
            else
                ps.GetComponent<SpriteRenderer>().sortingOrder = 6;


            //Bounds b = pp.GetComponent<SpriteRenderer>().bounds;           
            //float bottom = (b.center - b.extents).y;            

            //pp.transform.position = new Vector3(pp.transform.position.x, pp.transform.position.y, bottom);

        }


        //if (lastHeldPuzzleParts.Count > 0)
        //{
        //    lastHeldPuzzleParts.Last.Value.GetComponent<SpriteRenderer>().sortingOrder = 10;
        //    lastHeldPuzzleParts.Last.Value.GetComponent<PuzzlePart>().GetShadow().GetComponent<SpriteRenderer>().sortingOrder = 9;
        //}

        if (pickedUpPuzzlePart != null)
        {
            pickedUpPuzzlePart.GetComponent<SpriteRenderer>().sortingOrder = 12;
            pickedUpPuzzlePart.GetComponent<PuzzlePart>().GetShadow().GetComponent<SpriteRenderer>().sortingOrder = 11;
        }



        //int count = 1;

        //foreach (GameObject go in lastHeldPuzzleParts)
        //{
        //    go.GetComponent<SpriteRenderer>().sortingOrder = 4 + count;
        //    count++;
        //}        

    }

    private void EnsurePuzzlePartsAreInScreenSpace()
    {

        Vector3 temp = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(((float)Screen.width), ((float)Screen.height), 0));
        float screenTop = temp.y;
        float screenRightSide = temp.x;

        temp = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, 0, 0));
        float screenBottom = temp.y;
        float screenLeftSide = temp.x;

        foreach (GameObject pf in puzzleFragments)
        {
            GameObject puzzlePart = pf.GetComponent<PuzzleFragment>().GetPuzzlePartObject();
            Bounds b = puzzlePart.GetComponent<SpriteRenderer>().bounds;

            temp = (b.center + b.extents);
            float spriteTop = temp.y;
            float spriteRightSide = temp.x;

            temp = (b.center - b.extents);
            float spriteBottom = temp.y;
            float spriteLeftSide = temp.x;

            //Debug.Log("Top " + spriteTop);
            //Debug.Log("Bottom " + spriteBottom);
            //Debug.Log("left " + spriteLeftSide);
            //Debug.Log("right " + spriteRightSide);

            Vector3 reposition = Vector3.zero;

            if (screenTop < spriteTop)
            {
                reposition.y = screenTop - spriteTop;
                puzzlePart.GetComponent<PuzzlePart>().HasHitTopOfScreen();
            }

            if (screenRightSide < spriteRightSide)
            {
                reposition.x = screenRightSide - spriteRightSide;
                puzzlePart.GetComponent<PuzzlePart>().HasHitRightSideOfScreen();
            }

            if (screenBottom > spriteBottom)
            {
                reposition.y = screenBottom - spriteBottom;
                puzzlePart.GetComponent<PuzzlePart>().HasHitBottomOfScreen();
            }

            if (screenLeftSide > spriteLeftSide)
            {
                reposition.x = screenLeftSide - spriteLeftSide;
                puzzlePart.GetComponent<PuzzlePart>().HasHitLeftSideOfScreen();
            }

            puzzlePart.transform.position += reposition;
        }

    }


    //private void ListRaycasts()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = -1 * mainCamera.GetComponent<Transform>().position.z;

    //    Ray ray = Camera.main.ScreenPointToRay(mousePos);
    //    RaycastHit[] hits;
    //    hits = Physics.RaycastAll(ray);
    //    int i = 0;
    //    while (i < hits.Length)
    //    {
    //        RaycastHit hit = hits[i];
    //        Debug.Log(hit.collider.gameObject.name);
    //        hit.collider.gameObject.SendMessage("OnMouseDown");
    //        i++;
    //    }
    //}

    //puzzleFragments


    ///// if (pickedUpPuzzlePart != null && fingerOverPuzzleSlot != null)
    // {

    //     bool accept = false;

    //     //if (fingerOverPuzzleSlot == pickedUpPuzzlePart.GetComponent<PuzzlePart>().GetParentPuzzleFragment().GetComponent<PuzzleFragment>().GetPuzzleSlotObject())
    //         accept = true;

    //     //create better checking

    //     if(accept)
    //     {
    //         //pickedUpPuzzlePart.transform.position = fingerOverPuzzleSlot.transform.position;

    //     }
    // }
    //public void SetFingerOverPuzzleSlot(GameObject slot)
    //{
    //    fingerOverPuzzleSlot = slot;
    //}

    //public void ReleaseFingerOverPuzzleSlot(GameObject slot)
    //{
    //    if (fingerOverPuzzleSlot == slot)
    //        fingerOverPuzzleSlot = null;
    //}

    //public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
    //    Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
    //    float distance;
    //    xy.Raycast(ray, out distance);
    //    return ray.GetPoint(distance);
    //}

}

static public class GamePlayStates
{
    public const int Playing = 1;
    public const int Animating = 2;


}

//static public class GamePlayLevelScenes
//{
//    static public LinkedList<string> levels;

//    static public void Init()
//    {
//        levels = new LinkedList<string>();

//        //levels.AddLast()

//    }

//}
