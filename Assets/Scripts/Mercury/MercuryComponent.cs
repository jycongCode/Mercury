using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class MercuryComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Animator to controll the gameobject")]
    private Animator _animator;
    private PlayableController _controller;
    private StateManager _stateManager;

    private void Awake()
    {
        _controller = new PlayableController(this);
        _stateManager = new StateManager(_controller);
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _controller.CreateGraph();
    }

    private void OnDisable()
    {
        _stateManager.Clear();
        _controller.DestroyGraph();
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public AnimationState Play(AnimationClip clip)
    {

        _stateManager.Register(clip);
        AnimationState state = _stateManager.GetState(clip);
        Debug.Log(state == null);
        state.Play();
        return state;
    }
}
