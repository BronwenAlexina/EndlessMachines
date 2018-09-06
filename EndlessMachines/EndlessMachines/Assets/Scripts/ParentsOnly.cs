using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentsOnly : MonoBehaviour {

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
        Camera.main.GetComponent<AboutAndSocial>().RemoveGrownUpsOnlyGate();   
    }

    public void SetSoundEffectsManager(GameObject SoundEffectsManager)
    {
        soundEffectsManager = SoundEffectsManager;
    }

}
