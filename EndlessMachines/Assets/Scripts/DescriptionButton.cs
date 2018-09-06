using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionButton : MonoBehaviour {

    public Sprite clipIsPlayingSprite;

    Sprite clipIsNotPlayingSprite;
	// Use this for initialization
	void Start () {
        clipIsNotPlayingSprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {

        if (!GetComponent<AudioSource>().isPlaying && GetComponent<SpriteRenderer>().sprite == clipIsPlayingSprite)
            GetComponent<SpriteRenderer>().sprite = clipIsNotPlayingSprite;
    }

    void OnMouseDown()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<SpriteRenderer>().sprite = clipIsPlayingSprite;
        }
        else
            StopPlaying();
    }

    private void StopPlaying()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<SpriteRenderer>().sprite = clipIsNotPlayingSprite;
    }


}
