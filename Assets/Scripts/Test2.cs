using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
public class Test2 : MonoBehaviour
{
    PlayableGraph graph;
    public AnimationClip clip;
    Animator animator;
    public float Speed1 = 1f;
    public float Speed2 = 1f;
    public float Speed3 = 1f;
    public float MixerSpeed = 1f;
    private AnimationClipPlayable clip1;
    private AnimationClipPlayable clip2;
    private AnimationMixerPlayable mixer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        graph = PlayableGraph.Create();
        var custom = ScriptPlayable<CustomPlayable>.Create(graph,1);
        var output = AnimationPlayableOutput.Create(graph, "Output", animator);
        mixer = AnimationMixerPlayable.Create(graph,2);
        output.SetSourcePlayable(custom);
        graph.Connect(mixer, 0, custom, 0);
        clip1 = AnimationClipPlayable.Create(graph,clip);
        clip2 = AnimationClipPlayable.Create(graph, clip);
        graph.Connect(clip1,0,mixer,0);
        graph.Connect(clip2,0,mixer,1);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 1f);
        graph.Play();   
    }

    private void OnDestroy()
    {
        graph.Destroy();
    }

    public void TestEvent()
    {

    }
}

public class CustomPlayable : PlayableBehaviour
{
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        Debug.Log(info.effectiveParentSpeed);
    }
}
