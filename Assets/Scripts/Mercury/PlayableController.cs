using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
public class PlayableController
{
    private PlayableGraph _graph;
    private AnimationPlayableOutput _output;
    private AnimationMixerPlayable _mixer;
    private MercuryComponent _component;
    public const int MaxMixerInput = 4;
    private int _usedPortNum = 0;
    public PlayableController(MercuryComponent component)
    {
        _component = component;
    }
    public void CreateGraph()
    {
        _graph = PlayableGraph.Create("AnimationGraph");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        _output = AnimationPlayableOutput.Create(_graph,"AnimationOutput",_component.GetAnimator());
        _mixer = AnimationMixerPlayable.Create(_graph, MaxMixerInput);
        _output.SetSourcePlayable(_mixer);
    }

    public PlayableGraph GetGraph()
    {
        return _graph;
    }

    public int AddClip(AnimationClipPlayable clipPlayable)
    {
        if (_usedPortNum >= MaxMixerInput) return -1;
        int port;
        for (port = 0; port<MaxMixerInput; ++port)
        {
            if (_mixer.GetInput(port).IsNull()) break;
        }
        _graph.Connect(clipPlayable, 0, _mixer, port);
        _mixer.SetInputWeight(port, 0f);
        ++_usedPortNum;
        //Debug.Log($"Clip{clipPlayable.GetAnimationClip().name},port:{port}");
        return port;
    }

    public void UpdateWeight(int port,float targetWeight)
    {
        if (_usedPortNum <= 1)
        {
            _mixer.SetInputWeight(port, 1f);
        }
        else
        { 
            float restSumWeight = 1f - targetWeight;
            _mixer.SetInputWeight(port, targetWeight);
            for (int portIndex = 0; portIndex < MaxMixerInput; ++portIndex)
            {
                if (portIndex != port && !_mixer.GetInput(portIndex).IsNull())
                {
                    float weight = _mixer.GetInputWeight(portIndex);
                    weight *= restSumWeight;
                    if (weight == 0f)
                    {
                        Debug.Log(portIndex);
                        _graph.DestroyPlayable(_mixer.GetInput(portIndex));
                        --_usedPortNum;
                    }
                    else
                    {
                        _mixer.SetInputWeight(portIndex, weight);
                    }
                }
            }
        }
    }

    public void PlayGraph()
    {
        _graph.Play();
    }

    public void DestroyGraph()
    {
        _graph.Destroy();
    }
}
