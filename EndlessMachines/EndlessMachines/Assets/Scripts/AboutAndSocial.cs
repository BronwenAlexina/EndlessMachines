using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutAndSocial : MonoBehaviour {


    GameObject soundEffectsManager, musicTrackManager;

    public GameObject parentsOnlyGate, parentsOnlyAccept;

    public GameObject emailButton, fbButton, twitterButton;

    public GameObject mainMenuButton;

    // Use this for initialization
    void Start () {

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

        parentsOnlyAccept.GetComponent<ParentsOnly>().SetSoundEffectsManager(soundEffectsManager);
        emailButton.GetComponent<OpenEmail>().SetSoundEffectsManager(soundEffectsManager);
        fbButton.GetComponent<OpenURL>().SetSoundEffectsManager(soundEffectsManager);
        twitterButton.GetComponent<OpenURL>().SetSoundEffectsManager(soundEffectsManager);
        mainMenuButton.GetComponent<LoadSceneButton>().SetSoundEffectsManager(soundEffectsManager);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveGrownUpsOnlyGate()
    {
        parentsOnlyGate.SetActive(false);
        parentsOnlyAccept.SetActive(false);

        emailButton.SetActive(true);
        fbButton.SetActive(true);
        twitterButton.SetActive(true);
    }
}
