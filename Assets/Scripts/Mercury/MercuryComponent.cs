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
    public Animator GetAnimator()=>_Animator;
    private bool _IsPlayed = false;
    public bool IsPlayed
    {
        get => _IsPlayed;
        set
        {
            if(_IsPlayed == false&&value)
            {
                _Playable.CreateOutput(_Animator);
                _Playable.Graph.Play();
            }
            _IsPlayed = value;
        }
    }
    private void OnEnable()
    {
        _Animator = GetComponent<Animator>();
        _Playable = MercuryPlayable.Create();
    }

    private void OnDisable()
    {
        _Playable.DestroyGraph();
    }

    public MercuryState Play(AnimationClip clip,float fadeDuration=0.25f,FadeMode mode = FadeMode.FromStart)
    {
        IsPlayed = true;
        return _Playable.Play(clip, fadeDuration, mode);
    }
}
