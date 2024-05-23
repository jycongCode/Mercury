using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterType
{
    FromStart,
    Regular
}
public class AnimationState : IState
{
    
    public AnimationStateManager stateManager;
    public readonly string name;
    public readonly AnimationClip clip;
    private double _fadeInTime = 0.25d;
    private double _fadeoutTime = 1d;
    public EnterType enterType = EnterType.Regular;
    private const float WEIGHT_THRESHOLD = 0.01f;


    private float _currentWeight = 0f;
    public float CurrentWeight { get { return _currentWeight; } }

    public double Speed
    {
        get { return stateManager.controller.GetPlayableSpeed(this); }
        set { stateManager.controller.SetPlayableSpeed(this, value); }
    }

    public double NormalizedTime
    {
        get { return stateManager.controller.GetPlayableNormalizedTime(this); }
        set { stateManager.controller.SetPlayableNormalizedTime(this, value); }
    }

    public double Duration
    {
        get { return clip.length; }
    }

    private AnimationState(AnimationStateManager stateManager, AnimationClip clip, string name,EnterType enterType)
    {
        this.stateManager = stateManager;
        this.clip = clip;
        this.name = name;
        this.enterType = enterType;
    }

    public static AnimationState CreateState(AnimationStateManager stateManager, AnimationClip clip, string customName = "",EnterType enterType=EnterType.Regular)
    {
        string name = customName == "" ? clip.name : customName;
        if (stateManager.stateDictionary.IsRegistered(name)) return stateManager.stateDictionary.GetValue(name);
        AnimationState newState = new AnimationState(stateManager, clip, name,enterType);
        stateManager.stateDictionary.Register(newState.name,newState);
        return newState;
    }

    public void OnEnter()
    {
        _currentWeight = 0f;
        stateManager.controller.LoadState(this, 0f);
        stateManager.controller.Graph.Play();
    }

    private float LinearFunction(float start,float end,float x)
    {
        return Mathf.Lerp(start, end, x);
    }

    public void OnUpdate()
    {
        if (_currentWeight >= 1f) return;
        float weight = _fadeInTime <= WEIGHT_THRESHOLD ? 1f : LinearFunction(0f, 1f, (float)(NormalizedTime / _fadeInTime));
        if (Mathf.Abs(1f-weight) < WEIGHT_THRESHOLD) _currentWeight=weight= 1f;
        stateManager.controller.UpdateWeight(this, weight);
    }

    public void OnExit()
    {
        
    }
}
