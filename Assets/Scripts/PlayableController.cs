using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState
{
    AnimationClip clip;
    float weight;
    public AnimationState(AnimationClip clip, float weight)
    {
        this.clip = clip;
        this.weight = weight;
    }
}
public class PlayableController
{
    Dictionary<string, AnimationState> stateDictionary;

    #region State Dictionary Methods
    private void Register(AnimationClip clip)
    {
        if (IsRegistered(clip)) return;
        stateDictionary.Add(clip.name, new AnimationState(clip, 1.0f));
    }

    private void UnRegister(AnimationClip clip)
    {
        if (IsRegistered(clip))
        {
            stateDictionary.Remove(clip.name);
        }
    }
    private bool IsRegistered(AnimationClip clip)
    {
        return IsRegistered(clip.name);
    }
    
    private AnimationState GetAnimationState(AnimationClip clip)
    {
        return IsRegistered(clip.name) ? stateDictionary[clip.name] : null;
    }
    #endregion

    public AnimationState Play(AnimationClip clip) 
    {
        Register(clip);
        
    }
}
