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

public class MercuryState : MercuryNode
{
    private MercuryLayer _Layer;
    public MercuryLayer Layer
    {
        get => _Layer;
        set => _Layer = value;
    }
    protected MercuryState(MercuryPlayable root) : base(root){}
    public virtual void Play()
    {
        ResetParameter();
        Parent.PlayableHandle.SetInputWeight(Index, 1f);
    }
}
