using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public enum FadeMode
{
    FromStart
}

public enum PlayMessage
{
    Done,
    NotEnoughPort
}
public class MercuryLayer:MercuryNode
{
    public const int DEFAULT_INPUT_NUM = 4;
    private HashSet<MercuryState> _States;
    private MercuryState[] _Ports;
    public MercuryLayer(MercuryPlayable root) : base(root)
    {
        _PlayableHandle = AnimationMixerPlayable.Create(_Root.Graph, DEFAULT_INPUT_NUM);
        _States = new HashSet<MercuryState>();
        _Ports = new MercuryState[DEFAULT_INPUT_NUM];
    }

    public MercuryState Play(AnimationClip clip,float fadeDuration,FadeMode mode)
        =>Play(new MercuryState(clip,Root),fadeDuration, mode);

    public MercuryState Play(MercuryState state,float fadeDuration,FadeMode mode)
    {
        if (!_States.Contains(state))
        {
            AddState(state);
        }
        var port = FindAvailablePort();
        if (port == -1) return null;
        int cnt = 0;
        for(int i = 0;i<DEFAULT_INPUT_NUM;++i)
        {
            if (_Ports[i] is MercuryState pState)
            {
                ++cnt;
                pState.StartFade(fadeDuration, 0f);
            }
        }
        AddToGraph(port, state);
        if (cnt > 0) state.StartFade(fadeDuration, 1f);
        else state.Play();
        return state;
    }


    public override void RemoveFromGraph(int index)
    {
        _PlayableHandle.DisconnectInput(index);
        _Ports[index] = null;
    }
    public override void AddToGraph(int index,MercuryState state)
    {
        state.Index = index;
        _Ports[index] = state;
        _PlayableHandle.ConnectInput(index, state.PlayableHandle, 0);
    }

    public int FindAvailablePort()
    {
        for(int i = 0;i< _Ports.Length; i++)
        {
            if (_Ports[i] == null) return i;
        }
        return -1;
    }

    public void AddState(MercuryState state)
    {
        state.Parent = this;
        state.Layer = this;
        _States.Add(state);
    }
}
