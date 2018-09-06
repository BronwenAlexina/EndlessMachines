using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{

    public string sceneNameToLoad;
    GameObject soundEffectsManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        NetworkClient.SendToServer("Scene Loaded " + sceneNameToLoad);
        soundEffectsManager.GetComponent<GameplaySoundEffectsManager>().PlaySoundEffect(SoundEffectIDs.menuOptionPressed);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNameToLoad);

    }

    public string GetSceneNameToLoad()
    {
        return sceneNameToLoad;
    }

    public void SetSoundEffectsManager(GameObject SoundEffectsManager)
    {
        soundEffectsManager = SoundEffectsManager;
    }

}
