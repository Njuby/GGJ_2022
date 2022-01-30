using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] private BoolVariable onMutantChanged;
    [SerializeField] private AudioCue mutantMusic;
    [SerializeField] private AudioCue normalMusic;

    private void Start()
    {
        onMutantChanged.Changed.Register(ChangeMusic);
        ChangeMusic(onMutantChanged.Value);
    }

    public void ChangeMusic(bool mutant)
    {
        if (mutant)
        {
            mutantMusic.PlayAudioCue(transform);
        }
        else
        {
            normalMusic.PlayAudioCue(transform);
        }
    }

    private void OnDestroy()
    {
        onMutantChanged.Changed.Unregister(ChangeMusic);
    }
}