using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevelButton : MonoBehaviour {

    string nameOfLevelToLoad;
    GameObject soundEffectsManager;

	// Use this for initialization
	void Start () {        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        nameOfLevelToLoad = LevelList.GetNextLevelName(SceneManager.GetActiveScene().name);
    }

    void OnMouseDown()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameOfLevelToLoad);
        soundEffectsManager.GetComponent<GameplaySoundEffectsManager>().PlaySoundEffect(SoundEffectIDs.menuOptionPressed);
    }

    public string GetNameOfLevelToLoad()
    {
        return nameOfLevelToLoad;
    }

    public void SetSoundEffectsManager(GameObject SoundEffectsManager)
    {
        soundEffectsManager = SoundEffectsManager;
    }

}
