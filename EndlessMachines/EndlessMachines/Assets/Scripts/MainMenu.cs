using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    static bool hasLoadedOnce = false;

    public GameObject levelButtonContainer;

    GameObject soundEffectsManager, musicTrackManager;

    public GameObject aboutAndSocialButton;

    void Start()
    {

        if (!MainMenu.hasLoadedOnce)
        {
            LevelDataManager.Load();

            MainMenu.hasLoadedOnce = true;

            LevelList.Init();

            foreach (LoadSceneButton lsb in levelButtonContainer.GetComponentsInChildren<LoadSceneButton>())
            {
                //if (lsb.GetSceneNameToLoad() == "AboutAndSocial")
                //    continue;

                LevelList.AddLevelNameToList(lsb.GetSceneNameToLoad());
            }


            NetworkClient.Connect();

            GameObject NetworkUpdater = new GameObject();
            //Component c = new NetworkUpdater();
            NetworkUpdater.AddComponent<NetworkUpdater>();


            GameObject NetworkUpdater2 = Instantiate(NetworkUpdater, transform.position, new Quaternion()) as GameObject;
            NetworkUpdater2.name = "Network Updater";
            //nameOfGameObject.position = newPosition;

            //            NetworkUpdater.transform = Camera.main.transform;
            DontDestroyOnLoad(NetworkUpdater2);

        }

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            GameplaySoundEffectsManager gsem = go.GetComponent<GameplaySoundEffectsManager>();
            if (gsem != null)
                soundEffectsManager = go;

            GameplayMusicTrackManager gmtm = go.GetComponent<GameplayMusicTrackManager>();
            if (gmtm != null)
                musicTrackManager = go;
        }

        foreach (LoadSceneButton lsb in levelButtonContainer.GetComponentsInChildren<LoadSceneButton>())
        {
            foreach (Transform t in lsb.gameObject.GetComponentsInChildren<Transform>())
            {
                if (t.gameObject.tag == "LevelCompletionGraphic")
                {
                    if (LevelDataManager.HasLevelBeenCompleted(lsb.GetSceneNameToLoad()))
                        t.gameObject.SetActive(true);
                    else
                        t.gameObject.SetActive(false);
                }
            }

            lsb.SetSoundEffectsManager(soundEffectsManager);

        }

        musicTrackManager.GetComponent<GameplayMusicTrackManager>().PlayMusicTrack(MusicTrackIDs.MainMenuTheme);

        aboutAndSocialButton.GetComponent<LoadSceneButton>().SetSoundEffectsManager(soundEffectsManager);



#if UNITY_IOS
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

#if UNITY_ANDROID
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif




    }

    void Update()
    {

        //Debug.Log(Time.realtimeSinceStartup);



    }
}
