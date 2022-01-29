using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WorldChanger : MonoBehaviour
{
    [SerializeField] private float _toMutantTransitionTime = 1f;
    [SerializeField] private float _toHumanTransitionTime = 1f;
    [Space]
    [SerializeField] private Light _sceneLight;
    [SerializeField] private Color _humanColor;
    [SerializeField] private Color _mutantColor;
    [Space]
    [SerializeField] private Volume _ppHumanVolume;
    [SerializeField] private Volume _ppMutantVolume;
    [Space]
    [SerializeField] private Material _humanSkybox;
    [SerializeField] private Material _mutantSkybox;

    void Start()
    {
        //Sunscribe to correct Events
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FadeInMutant(_toMutantTransitionTime);
        }  
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            FadeInHuman(_toHumanTransitionTime);
        }


    }

    public void FadeInMutant(float duration)
    {
        //Sky Light
        _sceneLight.DOColor(_mutantColor, duration);

        //Post Processing
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => _ppHumanVolume.weight, x => _ppHumanVolume.weight = x, 0f, duration));
        seq.Join(DOTween.To(() => _ppMutantVolume.weight, x => _ppMutantVolume.weight = x, 1f, duration));

        //Skybox
        RenderSettings.skybox = _mutantSkybox;
    }

    public void FadeInHuman(float duration)
    {
        //Sky Light
        _sceneLight.DOColor(_humanColor, duration);

        //Post Processing
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => _ppHumanVolume.weight, x => _ppHumanVolume.weight = x, 1f, duration));
        seq.Join(DOTween.To(() => _ppMutantVolume.weight, x => _ppMutantVolume.weight = x, 0f, duration));

        //Skybox
        RenderSettings.skybox = _humanSkybox;
    }
}
