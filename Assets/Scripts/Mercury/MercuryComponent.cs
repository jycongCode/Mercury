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
    private void OnEnable()
    {
        _Animator = GetComponent<Animator>();
        _Playable = MercuryPlayable.Create();
    }

    private void OnDisable()
    {
        _Playable.DestroyGraph();
    }

    public MercuryState Play(AnimationClip clip,string customName="",EnterType enterType=EnterType.Regular)
    {
        if (!_activated)
        {
            _stateManager = AnimationStateManager.Create(_animator, gameObject.name);
            _activated = true;
        }
        MercuryState state = MercuryState.CreateState(_stateManager, clip, customName,enterType);
        _stateManager.TransitState(state.name);
        return state;
    }
}
