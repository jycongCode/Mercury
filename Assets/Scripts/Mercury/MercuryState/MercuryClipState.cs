using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryClipState : MercuryState
{
    private AnimationClip _Clip;
    public float Length { get => _Clip.length; }
    public bool IsLoop { get => _Clip.isLooping;}

    public MercuryClipState(AnimationClip clip,string name,int portNum,MercuryPlayable root) : base(root, name, portNum)
    {
        _Clip = clip;
        PlayableHandle = AnimationClipPlayable.Create(Root.Graph, _Clip);
    }
    public MercuryClipState(IParam parameter,MercuryPlayable root) : base(root,parameter.Name,parameter.PortNum)
    {
        var param = parameter as MercuryClipStateParam;
        _Clip = param.clip;
        PlayableHandle = AnimationClipPlayable.Create(Root.Graph, _Clip);
    }
    private bool isStop = false;
    public override void OnStop()
    {
        if (!IsLoop&&!isStop)
        {
            float time = (float)PlayableExtensions.GetTime(PlayableHandle);
            if(time>=Length)
            {
                isStop = true;
                OnEnd?.Invoke();
            }
        }
    }
    public override MercuryState Clone()
    {
        return new MercuryClipState(_Clip, Name, _InputPortNum, Root);
    }

}
