using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.VFX;

public class DashEffect : MonoBehaviour
{
    [SerializeField] private VoidEvent OnDash;
    [SerializeField] private VisualEffect dashVFX;
    private Sequence sequence;

    private void Start()
    {
        OnDash.Register(ApplyDashEffect);
        sequence = DOTween.Sequence();
    }

    private void ApplyDashEffect(UnityAtoms.Void vd)
    {
        if (sequence.active || sequence.IsPlaying())
        {
            sequence.Kill();
            dashVFX.enabled = false;
        }
        dashVFX.enabled = true;

        sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => dashVFX.enabled = false);
    }

    private void OnDestroy()
    {
        OnDash.Unregister(ApplyDashEffect);
    }
}