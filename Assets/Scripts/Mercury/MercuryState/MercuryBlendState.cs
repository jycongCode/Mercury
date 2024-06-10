using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MercuryBlendState : MercuryState
{
    AnimationClip[] _Clips;
    public float Paramter;
    public MercuryBlendState(AnimationClip[] clips,MercuryPlayable root, string name, int portNum) : base(root, name, portNum)
    {
        PlayableHandle = AnimationMixerPlayable.Create(root.Graph, portNum);
        _Clips = clips;
    }


}
