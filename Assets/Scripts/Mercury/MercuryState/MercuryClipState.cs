using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MercuryClipState : MercuryState
{
    public MercuryClipState(AnimationClip clip, MercuryPlayable root) : base(clip, root)
    {
        _PlayableHandle = AnimationClipPlayable.Create(_Root.Graph, clip);
    }

}
