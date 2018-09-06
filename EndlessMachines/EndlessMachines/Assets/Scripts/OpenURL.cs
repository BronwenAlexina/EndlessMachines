using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour {

    public string url;

    GameObject soundEffectsManager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        soundEffectsManager.GetComponent<GameplaySoundEffectsManager>().PlaySoundEffect(SoundEffectIDs.menuOptionPressed);

        Application.OpenURL(url);
    }

    public void SetSoundEffectsManager(GameObject SoundEffectsManager)
    {
        soundEffectsManager = SoundEffectsManager;
    }
}
