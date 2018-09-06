using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicTrackManager : MonoBehaviour
{

    static GameplayMusicTrackManager singletonGameplayMusicManager;

    public AudioClip logoJingle, mainMenuTheme, puzzleSolvingTheme, victoryFanfare;

    void Awake()
    {
        if (singletonGameplayMusicManager != null)// && singletonGameplayMusicManager != this) {
        {
            //Debug.Log("hit 1");
            DestroyImmediate(this.gameObject);
            return;
        } 
        else
        {
            //Debug.Log("hit 2");
            singletonGameplayMusicManager = this;
            DontDestroyOnLoad(this.gameObject);
        }

        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }    

    public void PlayMusicTrack(int id)
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();

        if (id == MusicTrackIDs.PuzzleSolvingTheme)
            source.clip = puzzleSolvingTheme;
        else if (id == MusicTrackIDs.VictoryFanfare)
            source.clip = victoryFanfare;
        else if (id == MusicTrackIDs.MainMenuTheme)
            source.clip = mainMenuTheme;
        else if (id == MusicTrackIDs.LogoJingle)
            source.clip = logoJingle;

        source.Play();
    }

}

static public class MusicTrackIDs
{
    public const int PuzzleSolvingTheme = 1;
    public const int VictoryFanfare = 2;
    public const int MainMenuTheme = 3;
    public const int LogoJingle = 4;
}
