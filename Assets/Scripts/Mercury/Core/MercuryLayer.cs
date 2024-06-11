using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryLayer:MercuryNode,IUpdate
{
    public AvatarMask Mask;
    public bool IsAdditive;
    private float _Weight;
    public float Weight { get => _Weight; }
    public MercuryLayer(MercuryPlayable root,string name,int portNum,AvatarMask mask=null,bool isAdditive=false) : base(root,name,portNum)
    {
        PlayableHandle = AnimationMixerPlayable.Create(root.Graph,portNum);
        Mask = mask;
        IsAdditive = isAdditive;
        _Weight = 1f;
    }

    #region State Management
    public MercuryState CreateState(IParam parameter, string name) 
    {
        switch(parameter.Type)
        {
            case StateType.ClipState:
                return new MercuryClipState(parameter, Root);
            case StateType.BlendState:
                return new MercuryBlendState(parameter, Root);
            default:
                return null;
        }
    }



    public int AddState(MercuryState state)
    {
        var port = FindAvailablePort();
        if (port != -1) AddChildren(port, state);
        return port;
    }

    public bool ContainsState(MercuryState state)=>_Children.Contains(state);
    public MercuryState GetState(int port)
    {
        if (_Nodes.ContainsKey(port))
        {
            return _Nodes[port] as MercuryState;
        }
        else return null;
    }
    #endregion

    #region Play
    public MercuryState Play(IParam parameter,float fadeDuration,FadeMode mode)
    {
        var state = CreateState(parameter,parameter.Name);
        Play(state, fadeDuration, mode);
        return state;
    }

    public void Play(MercuryState state,float fadeDuration,FadeMode mode)
    {
        switch (mode)
        {
            case FadeMode.FromStart:
                if (ContainsState(state))
                {
                    var newState = state.Clone();
                    if(AddState(newState)==-1)break;
                    FadeState(newState, fadeDuration);
                }else
                {
                    if (AddState(state) == -1) break;
                    FadeState(state, fadeDuration);
                }
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
        if(_Children.Count>1)
            state.StartFade(fadeDuration, 1f);
        else
        {
            state.StartFade(0f, 1f);
        }
    }
    #endregion
    public bool Update()
    {
        int length = _Children.Count;
        for(int i = length - 1; i >= 0; i--)
        {
            var child = _Children[i];
            
            var isContinue = (child as IUpdate).Update();
            //Debug.Log((child as MercuryState).Weight);
            if (!isContinue)
            {
                RemoveChildren(child);
            }
            else SetChildWeight(child.Port,(child as MercuryState).Weight);
        }
        return true;
    }
    
}
