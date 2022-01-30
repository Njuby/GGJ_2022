using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.VFX;

public class MutantForm : MonoBehaviour
{
    [SerializeField] private float Interval = 0.5f;
    [SerializeField] private Mutant mutant;
    [SerializeField] private SkinnedMeshRenderer playerRend;
    [SerializeField] private MeshRenderer pistolRend;
    [SerializeField] private PlayerMaterials humanMaterials;
    [SerializeField] private PlayerMaterials mutantMaterials;
    [SerializeField] private VisualEffect gunEffect;
    [SerializeField] private VisualEffect transformEffect;
    [SerializeField] private VisualEffect detransformEffect;
    [SerializeField] private BoolVariable mutantActivated;
    private Sequence detrans;
    private Sequence trans;

    private void Start()
    {
        detrans = DOTween.Sequence();
        trans = DOTween.Sequence();
        mutant.OnMutationModeActivated += Mutant_OnMutationModeActivated;
        mutant.OnMutationModeDectivated += Mutant_OnMutationModeDectivated;
    }

    private void Mutant_OnMutationModeDectivated()
    {
        mutantActivated.Value = false;
        detransformEffect.enabled = true;
        transformEffect.enabled = false;
        detransformEffect.Play();
        if (detrans.IsActive() || detrans.IsPlaying())
        {
            detrans.Kill();
        }
        detrans = DOTween.Sequence();

        detrans.AppendInterval(Interval);
        detrans.AppendCallback(() => humanMaterials.SetMaterials(playerRend, pistolRend));

        gunEffect.enabled = false;
    }

    private void Mutant_OnMutationModeActivated()
    {
        mutantActivated.Value = true;
        transformEffect.enabled = true;
        detransformEffect.enabled = false;
        gunEffect.enabled = true;

        transformEffect.Play();
        if (trans.IsActive() || trans.IsPlaying())
        {
            trans.Kill();
        }
        trans = DOTween.Sequence();
        trans.AppendInterval(Interval);
        trans.AppendCallback(() => mutantMaterials.SetMaterials(playerRend, pistolRend));
    }
}