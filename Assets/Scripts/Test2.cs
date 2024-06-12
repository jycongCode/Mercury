using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
public class Test2 : MonoBehaviour
{

    public AnimationClip idleClip;
    public AnimationClip walkClip;
    [Range(0f, 1f)]
    public float Param = 0.0f;
    // private params
    PlayableGraph graph;
    Animator animator;
    private AnimationClipPlayable clip1;
    private AnimationClipPlayable clip2;
    private AnimationMixerPlayable mixer1;
    private AnimationMixerPlayable mixer2;
    private AnimationPlayableOutput output;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        graph = PlayableGraph.Create();
        clip1 = AnimationClipPlayable.Create(graph, idleClip);
        clip2 = AnimationClipPlayable.Create(graph, walkClip);
        output = AnimationPlayableOutput.Create(graph, "Output", animator);
        mixer1 = AnimationMixerPlayable.Create(graph, 2);
        mixer2 = AnimationMixerPlayable.Create(graph, 2);
        output.SetSourcePlayable(mixer1);
        mixer1.AddInput(mixer2, 0,1.0f);
        graph.Connect(clip1,0,mixer2,0);
        graph.Connect(clip2, 0, mixer2, 1);
        graph.Play();
    }

    private void OnDestroy()
    {
        if(graph.IsValid())
            graph.Destroy();
    }

    private void Update()
    {
        mixer2.SetInputWeight(0, 1f - Param);
        mixer2.SetInputWeight(1, Param);
    }
}

public class CustomPlayable : PlayableBehaviour
{
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        Debug.Log(info.effectiveParentSpeed);
    }
}
