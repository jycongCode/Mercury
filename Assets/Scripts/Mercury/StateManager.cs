using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
public class AnimationState:IState
{
    private AnimationClip _clip;
    private AnimationClipPlayable _playable;
    private PlayableController _controller;
    private double _fadeInTime = 0.25d;
    private double _fadeoutTime = 1d;
    private float _currentWeight = 0f;
    private float _targetWeight = 1.0f;
    private float _startWeight = 0f;
    private int _port = -1;
    public double Speed {
        get { return (float)PlayableExtensions.GetSpeed(_playable); }
        set => PlayableExtensions.SetSpeed(_playable,value);
    }

    public double NormalizedTime
    {
        get { return PlayableExtensions.GetTime(_playable) / _duration; }
        set => PlayableExtensions.SetTime(_playable, value * _duration);
    }

    private double _duration = 0d;
    public double Duration
    {
        get { return _duration; }
    }

    private bool _isLoop = false;
    public bool IsLoop { get { return _isLoop; } }

    private float _weightThreshold = 0.01f;
    private AnimationState(AnimationClip clip,PlayableController controller)
    {
        _clip = clip;
        _controller = controller;
    }

    public static AnimationState CreateState(AnimationClip clip,PlayableController controller)
    {
        return new AnimationState(clip,controller);
    }

    public void OnEnter()
    {
        _targetWeight = 1.0f;
        _startWeight = 0f;
        _playable = AnimationClipPlayable.Create(_controller.GetGraph(), _clip);
        _port = _controller.AddClip(_playable);
        _duration = _clip.length;
        Debug.Log($"Duration of {_playable.GetAnimationClip().name}:{_duration}");
        _controller.PlayGraph();
    }

    public void OnUpdate()
    {
        if (_currentWeight == _targetWeight) return;
        _currentWeight = _fadeInTime == 0f?1f:Mathf.Lerp(_startWeight, _targetWeight, (float)(NormalizedTime / _fadeInTime));
        //Debug.Log(NormalizedTime);
        if (Mathf.Abs(_currentWeight - _targetWeight) < _weightThreshold) _currentWeight = _targetWeight;
        _controller.UpdateWeight(_port, _currentWeight);
    }

    public void OnExit()
    {
        _currentWeight = 0f;
        _targetWeight = 0f;
    }
}

public class StateManager
{
    private Dictionary<string, AnimationState> _stateDictionary = new Dictionary<string, AnimationState>();
    private string _currentStateName = "";
    private AnimationState _currentState = null;
    private PlayableController _controller;
    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void TransitState(string stateName)
    {
        if(_currentState != null)_currentState.OnExit();
        _currentStateName = stateName;
        _currentState = _stateDictionary[_currentStateName];
        _currentState.OnEnter();
    }

    public StateManager(PlayableController controller)
    {
        _controller = controller;
    }

    public bool IsRegistered(string name)
    {
        return _stateDictionary.ContainsKey(name);
    }

    public AnimationState Register(AnimationClip clip,string customName="")
    {
        string nameToCheck = customName == "" ? clip.name : customName;
        if (IsRegistered(nameToCheck)) return _stateDictionary[nameToCheck];
        AnimationState newState = AnimationState.CreateState(clip, _controller);
        _stateDictionary.Add(nameToCheck, newState);
        return newState;
    }

    public void UnRegister(string name)
    {
        if (!IsRegistered(name)) return;
        _stateDictionary.Remove(name);
    }

    public AnimationState GetState(string name)
    {
        return IsRegistered(name) ? _stateDictionary[name] : null;
    }

    public void Clear() => _stateDictionary.Clear();
}
