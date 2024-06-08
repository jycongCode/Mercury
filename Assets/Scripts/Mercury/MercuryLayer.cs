using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryLayer:MercuryNode,IUpdate
{
    public MercuryLayer(MercuryPlayable root,string name,int portNum) : base(root,name,portNum)
    {
        PlayableHandle = AnimationMixerPlayable.Create(root.Graph,portNum);
    }

    #region State Management
    public MercuryClipState CreateState(AnimationClip clip, string name) 
    {
        var state = new MercuryClipState(clip, name,1,Root);
        AddState(state);
        return state;
    }

    public int AddState(MercuryState state)
    {
        var port = FindAvailablePort();
        if (port != -1) AddChildren(port, state);
        return port;
    }

    public bool ContainsState(MercuryState state)=>_Children.Contains(state);
    #endregion

    #region Play
    public MercuryState Play(AnimationClip clip,float fadeDuration,FadeMode mode)
    {
        var state = CreateState(clip, clip.name);
        Play(state, fadeDuration, mode);
        return state;
    }

    public void Play(MercuryState state,float fadeDuration,FadeMode mode)
    {
        switch (mode)
        {
            case FadeMode.FromStart:
                var newState = state.Clone();
                AddState(newState);
                FadeState(newState,fadeDuration);
                break;
            case FadeMode.Regular:
                if (!ContainsState(state))
                {
                    var p = AddState(state);
                    if(p!=-1)FadeState(state,fadeDuration);
                }
                break;
        }
    }

    public void FadeState(MercuryState state,float fadeDuration)
    {
        foreach(var child in _Children)
        {
            if(child != state)
            {
                (child as MercuryState).StartFade(fadeDuration, 0f);
            }
        }
        if(_Children.Count<=0)
            state.StartFade(fadeDuration, 1f);
        else
            state.StartFade(0f,1f);
    }
    #endregion
    public bool Update()
    {
        int length = _Children.Count;
        for(int i = length - 1; i >= 0; i--)
        {
            var child = _Children[i];
            var isContinue = (child as IUpdate).Update();
            if(!isContinue)_Children.RemoveAt(i);
            else SetChildWeight(child.Port,(child as MercuryState).Weight);
        }
        return true;
    }
    
}
