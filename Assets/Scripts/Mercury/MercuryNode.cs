using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MercuryNode:IPlayableWrapper
{
    protected float _Weight;
    protected float _TargetWeight;
    protected float _FadeDuration;
    protected float _FadeSpeed;

    public void StartFade(float fadeDuration,float targetWeight)
    {
        _FadeDuration = fadeDuration;
        _TargetWeight = targetWeight;

    }
    public void UpdateWeight()
    {

    }
}
