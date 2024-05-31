using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MercuryClipState : MercuryState
{
    private AnimationClip _Clip;
    public MercuryClipState(AnimationClip clip, MercuryPlayable root) : base(root)
    {
        _Clip = clip;
        _PlayableHandle = AnimationClipPlayable.Create(_Root.Graph, clip);
    }

    public override MercuryState CopyFrom()
    {
        return new MercuryClipState(_Clip, _Root);
    }

}
