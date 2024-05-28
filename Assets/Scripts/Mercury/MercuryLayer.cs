using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryLayer
{
    private AnimationMixerPlayable _Mixer;
    private MercuryPlayable _Root;
    public MercuryPlayable Root { get => _Root; }
    private int _usedPortNum;
    private const int MAX_MIXER_INPUT = 4;
    private MercuryLayer(MercuryPlayable root)
    {
        _Root = root;
        _usedPortNum = 0;
        _Mixer = AnimationMixerPlayable.Create(_Root.Graph,MAX_MIXER_INPUT);
    }
    public static MercuryLayer Create(MercuryPlayable root)
    {
        return new MercuryLayer(root);
    }

    public void ConnectGraph(int port)
    {
        Root.Graph.Connect(_Mixer,0,Root.Layer,port);
        Root.LayerList[port] = this;
    }
    public void LoadState(MercuryState state, float startWeight)
    {
        if (_usedPortNum >= MAX_MIXER_INPUT) return;
        AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(_Graph, state.clip);
        int portIndex = 0;
        while (portIndex < MAX_MIXER_INPUT)
        {
            if (_Mixer.GetInput(portIndex).IsNull()) break;
            ++portIndex;
        }
        if (playableDictionary.IsRegistered(state))
        {
            int port = playableDictionary.GetValue(state).port;
            portHash[port] = null;
            playableDictionary.UnRegister(state);
        }
        playableDictionary.Register(state, new PlayableInput(clipPlayable, portIndex));
        portHash[portIndex] = state;
        _Graph.Connect(clipPlayable, 0, _Mixer, portIndex);
        _Mixer.SetInputWeight(portIndex, startWeight);
        UpdateWeight(state, startWeight);
        ++_usedPortNum;
    }
}
