using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScreen : MonoBehaviour {

    float timer = 0.0f;

    // Use this for initialization
    void Start () {

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        //Debug.Log("there are " + allObjects.Length);

        foreach (GameObject go in allObjects)
        {
            if(go.GetComponent<GameplayMusicTrackManager>() != null)
                go.GetComponent<GameplayMusicTrackManager>().PlayMusicTrack(MusicTrackIDs.LogoJingle);
        }
            
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > 4)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
