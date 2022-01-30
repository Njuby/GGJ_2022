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
    [SerializeField] private Color _humanLightColor;
    [SerializeField] private Color _mutantLightColor;
    [Space]
    [SerializeField] private Volume _ppHumanVolume;
    [SerializeField] private Volume _ppMutantVolume;
    [Space]
    [SerializeField] private Color _humanSkyboxColor;
    [SerializeField] private Color _mutantSkyboxColor;
    [SerializeField] private float _humanSkyboxAthmosphericThinkness = 0.85f;
    [SerializeField] private float _mutantSkyboxAthmosphericThinkness = 2.5f; 
    [SerializeField] private float _humanSkyboxExposure = 1.9f;
    [SerializeField] private float _mutantSkyboxExposure = 0.9f;
    [Space]
    [SerializeField] private float _bloomMaxWeight = .5f;


    Color _tempColor;
    float _tempFloat;
    float _tempFloat2;

    void Start()
    {
        _tempColor = _humanSkyboxColor;
        _tempFloat = _humanSkyboxAthmosphericThinkness;
        _tempFloat2 = _humanSkyboxExposure;
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
        _sceneLight.DOColor(_mutantLightColor, duration);

        //Skybox
        DOTween.To(() => _tempColor, x => _tempColor = x, _mutantSkyboxColor, duration).OnUpdate(() => RenderSettings.skybox.SetColor("_SkyTint", _tempColor));
        DOTween.To(() => _tempFloat, x => _tempFloat = x, _mutantSkyboxAthmosphericThinkness, duration / 2).OnUpdate(() => RenderSettings.skybox.SetFloat("_AtmosphereThickness", _tempFloat));
        RenderSettings.skybox.SetFloat("_Exposure", _mutantSkyboxExposure);
        
        //Post Processing
        DOTween.To(() => _ppMutantVolume.weight, x => _ppMutantVolume.weight = x, _bloomMaxWeight, duration);
    }

    public void FadeInHuman(float duration)
    {
        //Sky Light
        _sceneLight.DOColor(_humanLightColor, duration);

        //Skybox
        DOTween.To(() => _tempColor, x => _tempColor = x, _humanSkyboxColor, duration).OnUpdate(() =>RenderSettings.skybox.SetColor("_SkyTint", _tempColor));
        DOTween.To(() => _tempFloat, x => _tempFloat = x, _humanSkyboxAthmosphericThinkness, duration / 2).OnUpdate(() =>RenderSettings.skybox.SetFloat("_AtmosphereThickness", _tempFloat));
        RenderSettings.skybox.SetFloat("_Exposure", _humanSkyboxExposure);
        
        //Post Processing
        //DOTween.To(() => _ppMutantVolume.weight, x => _ppMutantVolume.weight = x, 0f, duration);
        _ppMutantVolume.weight = 0f;
    }

    private void OnApplicationQuit()
    {
        FadeInHuman(_toHumanTransitionTime);
    }
}
