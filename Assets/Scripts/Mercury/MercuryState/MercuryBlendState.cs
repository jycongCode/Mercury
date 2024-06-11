using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryBlendState : MercuryState
{
    AnimationClip[] _Clips;
    AnimationClipPlayable[] _ClipsPlayable;
    float[] _Weights;
    public float Parameter;
    private int _ClipsCount;
    public MercuryBlendState(AnimationClip[] clips,MercuryPlayable root, string name, int portNum) : base(root, name, portNum)
    {
        _Clips = clips;
        _ClipsCount = _Clips.Length;
        PlayableHandle = AnimationMixerPlayable.Create(root.Graph, portNum);
        _ClipsPlayable = new AnimationClipPlayable[_ClipsCount];
        _Weights = new float[_ClipsCount];
        for(int i = 0;i< _ClipsCount; i++)
        {
            _ClipsPlayable[i] = AnimationClipPlayable.Create(root.Graph, _Clips[i]);
            float w = i == 0 ? 1f : 0f;
            Root.Graph.Connect(_ClipsPlayable[i], 0, PlayableHandle, i);
            _Weights[i] = w;
        }
    }

    public MercuryBlendState(IParam parameter,MercuryPlayable root) : base(root, parameter.Name, parameter.PortNum)
    {
        var param = parameter as MercuryBlendStateParam;
        _Clips = param.clips;
        _ClipsCount = _Clips.Length;
        PlayableHandle = AnimationMixerPlayable.Create(root.Graph,param.PortNum);
        _ClipsPlayable = new AnimationClipPlayable[_ClipsCount];
        _Weights = new float[_ClipsCount];
        for (int i = 0; i < _ClipsCount; i++)
        {
            _ClipsPlayable[i] = AnimationClipPlayable.Create(root.Graph, _Clips[i]);
            float w = i == 0 ? 1f : 0f;
            PlayableHandle.AddInput(_ClipsPlayable[i], 0, w);
            _Weights[i] = w;
        }
    }

    public override void PreFrameProcess()
    {
        _Weights[0] = 1.0f - Parameter;
        _Weights[1] = Parameter;
        PlayableHandle.SetInputWeight(0, _Weights[0]);
        PlayableHandle.SetInputWeight(1, _Weights[1]);
    }
}
