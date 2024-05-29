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
    public MercuryState(AnimationClip clip,MercuryPlayable root) : base(root)
    {
        _PlayableHandle = AnimationClipPlayable.Create(_Root.Graph, clip);
    }

    
    public void Play()
    {
        _Weight = 1f;
        Parent.PlayableHandle.SetInputWeight(Index, 1f);
    }
}
