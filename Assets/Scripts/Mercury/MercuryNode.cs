using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public abstract class MercuryNode : IPlayableWrapper, IUpdate
{
    protected float _Weight;
    protected float _TargetWeight;
    protected float _FadeDuration;
    protected float _FadeSpeed;
    protected float _Speed = 1f;
    protected float _NormalizedTime = 0f;
    protected int _Index;
    protected Playable _PlayableHandle;
    protected IPlayableWrapper _Parent;
    protected MercuryPlayable _Root;
    protected bool _IsPlaying;
    protected ulong _frameID;
    public MercuryNode(MercuryPlayable root)
    {
        _Root = root;
        _Weight = 0f;
        _TargetWeight = 1f;
        _FadeDuration = 0f;
        _Index = -1;
        _Speed = 1f;
    }
    protected float effectiveParentSpeed
    {
        get
        {
            var playable = _Parent;
            float speed = 1f;
            while(playable != null&&playable is IPlayableWrapper) 
            {
                speed *= playable.Speed;
                playable = playable.Parent;
            }
            return speed;
        }
    }
    public bool IsPlaying
    {
        get => _IsPlaying;
        set
        {
            _IsPlaying = value;
            if (!_IsPlaying) Stop();
        }
    }
    
    protected float effectiveSpeed
    {
        get
        {
            var speed = Speed;
            var parentPlayable = Parent;
            while(parentPlayable is MercuryNode)
            {
                speed *= parentPlayable.Speed;
            }
            return speed;
        }
    }

    public float Speed
    {
        get
        {
            return _Speed;
        }
        set
        {
           _Speed = value;
           PlayableExtensions.SetSpeed(_PlayableHandle,_Speed);
        }
    }

    public virtual float NormalizedTime
    {
        get
        {
            return _NormalizedTime;
        }
        set
        {
            _NormalizedTime = value;
        }
    }

    public Playable PlayableHandle { 
        get => _PlayableHandle; 
        set => _PlayableHandle = value;
    }

    public IPlayableWrapper Parent { 
        get => _Parent; 
        set => _Parent = value; 
    }

    public MercuryPlayable Root { 
        get => _Root; 
    }

    public virtual void SetInputWeight(int index,float weight)
    {
        Parent.PlayableHandle.SetInputWeight(index,weight);
    }
    public int Index { 
        get => _Index;
        set => _Index = value;
    }
    
    public void ResetParameter()
    {
        _Weight = 1f;
        _TargetWeight = 1f;
        _FadeDuration = 0f;
        _FadeSpeed = float.PositiveInfinity;
    }

    public virtual void StartFade(float fadeDuration,float targetWeight)
    {
        _frameID = MercuryPlayable.FrameID;
        if(fadeDuration <= 0)
        {
            Play(targetWeight);
        }
        else
        {
            _FadeDuration = fadeDuration;
            _TargetWeight = targetWeight;
            _FadeSpeed = (_TargetWeight - _Weight) / _FadeDuration;
        }
        IsPlaying = true;
        Root?.RequirePreUpdate(this);
    }

    public virtual void Play(float targetWeight)
    {
        _TargetWeight = targetWeight;
        _FadeDuration = 0f;
        _Weight = targetWeight;
        IsPlaying = true;
        _Root?.RequirePreUpdate(this);
    }
    void IUpdate.Update()
    {
        UpdateWeight();
        SetGraphWeight(); 
        OnStop();
    }

    public virtual void OnStop(){}
    public void UpdateWeight()
    {
        if (_FadeSpeed * (_TargetWeight - _Weight) <= 0)
        {
            _Weight = _TargetWeight;
            _FadeSpeed = 0f;
            _FadeDuration = float.PositiveInfinity;
        }
        else
        {
            //Debug.Log(effectiveParentSpeed);
            _Weight += _FadeSpeed * MercuryPlayable.DeltaTime * effectiveParentSpeed *Speed;
        }
    }

    public void SetGraphWeight()
    {
        if(MercuryPlayable.DeltaTime > 0 && _Weight <= 0)
        {
            Root?.CancelPreUpdate(this);
            _Parent.RemoveFromGraph(_Index);
            IsPlaying = false;
        }
        else
        {
            SetInputWeight(_Index, _Weight);
        }
    }

    public virtual void Stop()
    {
        Debug.Log("Stop");
    }

    public virtual void RemoveFromGraph(int index)
    {
        throw new System.NotImplementedException();
    }

    public virtual void AddToGraph(int index, MercuryState state) 
    {
        throw new System.NotImplementedException();
    }

    public void SetPlayable()
    {
        throw new System.NotImplementedException();
    }
}
