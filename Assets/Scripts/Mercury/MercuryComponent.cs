using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
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
    public MercuryState Play(AnimationClip clip,uint layerIndex = 0,float fadeDuration=0.25f,FadeMode mode = FadeMode.FromStart)
        => _Playable.Play(clip ,layerIndex, fadeDuration, mode);

    public MercuryState Play(MercuryState state)
        => _Playable.Play(state, state.Layer,state.FadeDuration,state.Mode);
    #endregion

    public uint AddLayer(float weight, bool isAdditive=false, AvatarMask mask=null)
        =>_Playable.AddLayer(weight, isAdditive, mask);
}
