using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
public class PlayableController
{
    private PlayableGraph _graph;
    private AnimationPlayableOutput _output;
    private MercuryComponent _component;

    public PlayableController(MercuryComponent component)
    {
        _component = component;
    }
    public void CreateGraph()
    {
        _graph = PlayableGraph.Create("AnimationGraph");
        _output = AnimationPlayableOutput.Create(_graph,"AnimationOutput",_component.GetAnimator());
    }

    public PlayableGraph GetGraph()
    {
        return _graph;
    }

    public void AddClip(AnimationClipPlayable clipPlayable)
    {
        _output.SetSourcePlayable(clipPlayable);
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
