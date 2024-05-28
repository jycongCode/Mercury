using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterType
{
    FromStart,
    Regular
}

public class MercuryState : IState
{
    public AnimationStateManager stateManager;
    public MercuryEventManager Events;
    public readonly string name;
    public readonly AnimationClip clip;
    private double _fadeInTime = 0.5d;
    private double _fadeoutTime = 0.85d;
    public EnterType enterType = EnterType.Regular;
    
    private float _Weight = 0f;
    public float Weight
    {
        get => _Weight;
        set
        {
            _Weight = value;
        }
    }

    private float _TargetWeight = 0f;
    public float TargetWeight
    {
        get=> _TargetWeight;
        set
        {
            _TargetWeight = value;
        }
    }

    private float _FadeSpeed = 0f;
    public float FadeSpeed
    {
        get => _FadeSpeed;
        set
        {
            _FadeSpeed = value;
        }
    }

    public bool isUpdate = false;
    public float FadeTime = 0f;
    public void Update()
    {
        if(isUpdate)
        {
            FadeTime -= MercuryPlayable.DeltaTime;
            if(FadeTime<0f)
            {
                _Weight = _TargetWeight;
            }
            else
            {
                _FadeSpeed = (_TargetWeight - _Weight) / FadeTime;
                _Weight += _FadeSpeed * MercuryPlayable.DeltaTime;
            }
        }
    }
    
    public double Speed
    {
        get { return stateManager.controller.GetPlayableSpeed(this); }
        set { 
            stateManager.controller.SetPlayableSpeed(this, value);
            Events.PlayDirection = value >= 0 ? 1 : -1;
        }
    }

    public double NormalizedTime
    {
        get { return stateManager.controller.GetPlayableNormalizedTime(this); }
        set {
                if (Speed * value >= 0)
                {
                    stateManager.controller.SetPlayableNormalizedTime(this, value);
                    Events.ReWind((float)value);
                }
            }
    }

    public double Duration
    {
        get { return clip.length; }
    }

    private MercuryState(AnimationStateManager stateManager, AnimationClip clip, string name,EnterType enterType)
    {
        this.stateManager = stateManager;
        this.Events = new MercuryEventManager(Speed>0?1:-1);
        this.clip = clip;
        this.name = name;
        this.enterType = enterType;
    }

    public static MercuryState CreateState(AnimationStateManager stateManager, AnimationClip clip, string customName = "",EnterType enterType=EnterType.Regular)
    {
        string name = customName == "" ? clip.name : customName;
        if (stateManager.stateDictionary.IsRegistered(name)) return stateManager.stateDictionary.GetValue(name);
        MercuryState newState = new MercuryState(stateManager, clip, name,enterType);
        stateManager.stateDictionary.Register(newState.name,newState);
        return newState;
    }

    public void OnEnter()
    {
        _currentWeight = 0f;
        NormalizedTime = 0f;
        stateManager.controller.LoadState(this, 0f);
        stateManager.controller.Graph.Play();
        Events.ReWind(0f);
    }

    private float LinearFunction(float start,float end,float x)
    {
        return Mathf.Lerp(start, end, x);
    }

    public void OnUpdate()
    {
        Events.Update((float)(NormalizedTime));
        if (_currentWeight >= 1f) return;
        float weight = _fadeInTime <= WEIGHT_THRESHOLD ? 1f : LinearFunction(0f, 1f, (float)(NormalizedTime / _fadeInTime));
        if (Mathf.Abs(1f-weight) < WEIGHT_THRESHOLD) _currentWeight=weight= 1f;
        stateManager.controller.UpdateWeight(this, weight);
    }

    public void OnExit()
    {

    }
}
