using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MercuryBlend1D : MercuryNode
{
    public const int DEFAULT_INPUT_NUM = 3;
    public float parameter;
    public MercuryBlend1D(MercuryPlayable root) : base(root)
    {
        _PlayableHandle = AnimationMixerPlayable.Create(_Root.Graph,DEFAULT_INPUT_NUM);

    }

}
