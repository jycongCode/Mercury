using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryClipState : MercuryState
{
    private AnimationClip _Clip;
    public float Length { get => _Clip.length; }
    public override bool IsLoop { get => _Clip.isLooping;}
    public MercuryClipState(AnimationClip clip, MercuryPlayable root) : base(root)
    {
        _Clip = clip;
        _PlayableHandle = AnimationClipPlayable.Create(_Root.Graph, clip);
    }
    public override void OnStop()
    {
        if (!IsLoop)
        {
            float time = (float)PlayableExtensions.GetTime(_PlayableHandle);
            if(time>=Length)
            {
                Root?.CancelPreUpdate(this);
                OnEnd?.Invoke();
            }
        }
    }
    public override MercuryState CopyFrom()
    {
        return new MercuryClipState(_Clip, _Root);
    }

}
