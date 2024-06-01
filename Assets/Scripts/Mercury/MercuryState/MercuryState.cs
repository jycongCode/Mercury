using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public enum EnterType
{
    FromStart,
    Regular
}

public class MercuryState : MercuryNode, ICopy
{
    private MercuryLayer _Layer;
    public MercuryLayer Layer
    {
        get => _Layer;
        set => _Layer = value;
    }
    public virtual bool IsLoop{get;set;}
    public Action OnEnd;
    protected MercuryState(MercuryPlayable root) : base(root){}
    public virtual void Play()
    {
        ResetParameter();
        Parent.PlayableHandle.SetInputWeight(Index, 1f);
    }

    public virtual MercuryState CopyFrom()
    {
        throw new System.NotImplementedException();
    }
}
