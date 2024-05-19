using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
public class AnimationState
{
    private AnimationClip _clip;
    private AnimationClipPlayable _playable;
    private PlayableController _controller;
    private float _weight;
    private float _normalizedTime;
    public float Speed {
        get => (float)PlayableExtensions.GetSpeed(_playable);
        set => PlayableExtensions.SetSpeed(_playable,value);
    }
    public float NormalizedTime { get { return _normalizedTime; } }

    private AnimationState(AnimationClip clip,PlayableController controller)
    {
        _clip = clip;
        _controller = controller;
        _weight = 1.0f;
        _normalizedTime = 0f;
    }

    public static AnimationState CreateState(AnimationClip clip,PlayableController controller)
    {
        return new AnimationState(clip,controller);
    }

    public void Play()
    {
        _playable = AnimationClipPlayable.Create(_controller.GetGraph(), _clip);
        _controller.AddClip(_playable);
        _controller.PlayGraph();
    }
}

public class StateManager
{
    Dictionary<string, AnimationState> stateDictionary = new Dictionary<string, AnimationState>();
    PlayableController _controller;
    public StateManager(PlayableController controller)
    {
        _controller = controller;
    }
    public bool IsRegistered(AnimationClip clip)
    {
        return stateDictionary.ContainsKey(clip.name);
    }

    public void Register(AnimationClip clip)
    {
        if (IsRegistered(clip)) return;
        stateDictionary.Add(clip.name, AnimationState.CreateState(clip,_controller));
    }

    public void UnRegister(AnimationClip clip)
    {
        if (!IsRegistered(clip)) return;
        stateDictionary.Remove(clip.name);
    }

    public AnimationState GetState(AnimationClip clip)
    {
        return IsRegistered(clip) ? stateDictionary[clip.name] : null;
    }

    public void Clear() => stateDictionary.Clear();
}
