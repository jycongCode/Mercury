using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public abstract class MercuryState : MercuryNode, IUpdate
{
    public MercuryState(MercuryPlayable root,string name,int portNum) : base(root,name, portNum) 
    {
        _Weight = 0f;
        _Speed = 1f;
    }

    protected int _LayerIndex;
    public int LayerIndex
    {
        get => _LayerIndex;
        set
        {
            _LayerIndex = value;

        }
    }
    protected float _FadeDuration;
    protected float _Speed;
    public float Speed
    {
        get => _Speed;
        set
        {
            _Speed = value;
            PlayableExtensions.SetSpeed(PlayableHandle, _Speed);
        }
    }

    public bool IsPlaying { get => _Weight > 0f; }
    public Action OnEnd;
    #region Fade
    // Only Weight and TargetWeight and FadeSpeed matters when updating
    // Fade Duration is used to set them up
    protected float _Weight;
    public float Weight { get => _Weight; }
    protected float _TargetWeight;
    protected float _FadeSpeed;
    
    // Called by MercuryLayer, set the parameters of MercuryState so that we can start update procedure 
    public virtual void StartFade(float fadeDuration, float targetWeight)
    {
        if (fadeDuration <= 0)
        {
            _FadeDuration = 0f;
            _TargetWeight = targetWeight;
            _Weight = targetWeight;
            _FadeSpeed = 0f;
        }
        else
        {
            _FadeDuration = fadeDuration;
            _TargetWeight = targetWeight;
            _FadeSpeed = (_TargetWeight - _Weight) / _FadeDuration;
        }
    }
    #endregion

    #region Update

    // Accumulated parent speed

    public bool Update()
    {
        PreFrameProcess();
        UpdateWeight();
        OnStop();
        return _Weight > 0f;
    }

    public virtual void PreFrameProcess(){}

    public void UpdateWeight()
    {
        if (_FadeSpeed * (_TargetWeight - _Weight) <= 0)
        {
            _Weight = _TargetWeight;
        }
        else
        {
            _Weight += _FadeSpeed * Root.DeltaTime * Speed;
        }
    }
    public virtual void OnStop() { }
    public virtual MercuryState Clone() { return null; }
    #endregion
}
