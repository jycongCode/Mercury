using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercuryClipStateParam : IParam
{
    public AnimationClip clip;

    public MercuryClipStateParam(AnimationClip clip)
    {
        this.clip = clip;
    }

    public StateType Type => StateType.ClipState;

    public string Name { get => clip.name; }

    public int PortNum => 4;
}
public class MercuryBlendStateParam : IParam
{
    public AnimationClip[] clips;
    public MercuryBlendStateParam(AnimationClip[] clips)
    {
        this.clips = clips;
    }

    public StateType Type => StateType.BlendState;

    public string Name { get => clips[0].name + "_blend"; }

    public int PortNum => 4;
}
