using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class MercuryComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Animator to controll the gameobject")]
    private Animator _animator;
    private AnimationStateManager _stateManager;
    private bool _activated = false;
    private void Update()
    {
        if(_activated)_stateManager.Update();
    }

    private void OnDisable()
    {
        _activated = false;
        _stateManager.Clear();
    }

    public AnimationState Play(AnimationClip clip,string customName="",EnterType enterType=EnterType.Regular)
    {
        if (!_activated)
        {
            _stateManager = AnimationStateManager.Create(_animator, gameObject.name);
            _activated = true;
        }
        AnimationState state = AnimationState.CreateState(_stateManager, clip, customName,enterType);
        _stateManager.TransitState(state.name);
        return state;
    }
}
