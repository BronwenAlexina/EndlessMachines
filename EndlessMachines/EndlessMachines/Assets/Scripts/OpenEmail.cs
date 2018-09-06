using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEmail : MonoBehaviour {

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

        string email = "";
        string subject = "Endless Machines";
        string body = "I am the law!";
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public void SetSoundEffectsManager(GameObject SoundEffectsManager)
    {
        soundEffectsManager = SoundEffectsManager;
    }

}
