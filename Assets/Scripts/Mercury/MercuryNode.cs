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
    protected int _Index;
    protected Playable _PlayableHandle;
    protected IPlayableWrapper _Parent;
    protected MercuryPlayable _Root;
    protected bool _IsPlaying;
    public bool IsPlaying
    {
        get => _IsPlaying;
        set
        {
            _IsPlaying = value;
            if (!_IsPlaying) Stop();
        }
    }
    public MercuryNode(MercuryPlayable root)
    {
        _Root = root;
        _Weight = 0f;
        _TargetWeight = 1f;
        _FadeDuration = 0f;
        _Index = -1;
        _Speed = 1f;
        _FadeSpeed = float.PositiveInfinity;
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
           _Speed = Mathf.Clamp(value, 0f, 1f);
            PlayableExtensions.SetSpeed(_PlayableHandle, (double)_Speed);
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

    public void StartFade(float fadeDuration,float targetWeight)
    {
        _FadeDuration = fadeDuration;
        _TargetWeight = targetWeight;
        _FadeSpeed = (_TargetWeight - _Weight) / _FadeDuration;
        IsPlaying = true;
        Root?.RequirePreUpdate(this);
        //Debug.Log(Root == null);
    }

    public void Update()
    {
        //Debug.Log(
        //    $"Weight:{_Weight},TargetWeight{_TargetWeight}");

          UpdateWeight();
          SetGraphWeight();
    }

    public void UpdateWeight()
    {
        Debug.Log($"{Index},weight:{_Weight}");
        if (_FadeSpeed * (_TargetWeight - _Weight) < 0)
        {
            _Weight = _TargetWeight;
            _FadeSpeed = 0f;
            _FadeDuration = float.PositiveInfinity;
            Root?.CancelPreUpdate(this);
        }
        else
        {
            _Weight += _FadeSpeed * MercuryPlayable.DeltaTime;
        }

        if (_Weight <= 0f)
        {
            _Parent.RemoveChild(_Index);
            IsPlaying = false;
        }
    }

    public void SetGraphWeight()
    {
        SetInputWeight(_Index, _Weight);
    }

    public virtual void Stop()
    {

    }

    public virtual void RemoveChild(int index)
    {
    }
}
