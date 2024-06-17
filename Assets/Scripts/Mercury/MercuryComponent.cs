using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MercuryComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Animator to controll the gameobject")]
    private Animator _Animator;
    private MercuryPlayable _Playable;
    private void OnEnable()
    {
        _Playable = MercuryPlayable.Create();
        _Playable.CreateOutput(_Animator);
        _Playable.Graph.Play();
    }

    private void OnDisable()
    {
        _Playable.DestroyGraph();
    }

    #region Play
    public MercuryState Play(AnimationClip clip, float fadeDuration=0.25f,FadeMode mode=FadeMode.FromStart)
        => _Playable.Play(new MercuryClipStateParam(clip),fadeDuration, mode);
    public MercuryState Play(IParam parameter,float fadeDuration=0.25f,FadeMode mode = FadeMode.FromStart)
        => _Playable.Play(parameter,fadeDuration, mode);
    public void Play(MercuryState state)
        => _Playable.Play(state,0.25f,FadeMode.FromStart);
    public MercuryLayer CreateLayer(string name, AvatarMask mask, bool isAdditive) => _Playable.CreateLayer(_Animator,name, mask, isAdditive);
    #endregion
}
