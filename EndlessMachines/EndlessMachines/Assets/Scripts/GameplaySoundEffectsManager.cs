using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySoundEffectsManager : MonoBehaviour
{
    static GameplaySoundEffectsManager singletonGameplaySoundEffectsManager;

    public AudioClip puzzlePartPlaced, puzzlePartMisplaced, puzzlePartPickedUp, menuOptionPressed;

    void Awake()
    {
        if (singletonGameplaySoundEffectsManager != null)// && singletonGameplayMusicManager != this) {
        {
            //Debug.Log("hit 1");
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            //Debug.Log("hit 2");
            singletonGameplaySoundEffectsManager = this;
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

    public void PlaySoundEffect(int id)
    {
        //GetComponent<>
        AudioSource source = gameObject.GetComponent<AudioSource>();

        if (id == SoundEffectIDs.puzzlePartPlacced)
            source.clip = puzzlePartPlaced;
        else if (id == SoundEffectIDs.puzzleParpuzzlePartMisplacedtPlacced)
            source.clip = puzzlePartMisplaced;
        else if (id == SoundEffectIDs.puzzlePartPickedUp)
            source.clip = puzzlePartPickedUp;
        else if (id == SoundEffectIDs.menuOptionPressed)
            source.clip = menuOptionPressed;

        source.PlayOneShot(source.clip);

    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(audioClip);
    }

    public void PlaySoundEffect(AudioClip audioClip, float volumeScale)
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(audioClip, volumeScale);
    }

}


static public class SoundEffectIDs
{
    public const int puzzlePartPlacced = 1;
    public const int puzzleParpuzzlePartMisplacedtPlacced = 2;
    public const int puzzlePartPickedUp = 3;
    public const int menuOptionPressed = 4;
}
