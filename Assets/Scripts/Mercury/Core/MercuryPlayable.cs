using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.Reflection;
using Unity.VisualScripting;

public class MercuryPlayable:PlayableBehaviour
{
    private PlayableGraph _Graph;
    public PlayableGraph Graph { get => _Graph; }
    private AnimationPlayableOutput _Output;
    private Playable RootPlayable;

    private MercuryLayer _BaseLayer;
    public MercuryLayer BaseLayer { get => _BaseLayer; }

    private float _DeltaTime = 0f;
    public float DeltaTime { get => _DeltaTime; }
    private MercuryLayerList _LayerList;
    private static readonly MercuryPlayable Template = new MercuryPlayable();
    public static MercuryPlayable Create()
    {
        var graph = PlayableGraph.Create();
        return ScriptPlayable<MercuryPlayable>.Create(graph, Template, 2).GetBehaviour();
    }

    #region PlayableBehaviour
    public override void OnPlayableCreate(Playable playable)
    {
        RootPlayable = playable;
        _Graph = playable.GetGraph();
        _LayerList = new MercuryLayerList(this,"LayerList",4);
        Graph.Connect(_LayerList.PlayableHandle, 0, playable, 0);
        _BaseLayer = _LayerList.CreateLayer("BaseLayer");
        _LayerList.AddLayer(BaseLayer);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        _DeltaTime = info.deltaTime;
        _LayerList.Update();
    }

    #endregion
    public void CreateOutput(Animator animator)
    {
        _Output = AnimationPlayableOutput.Create(_Graph, "AnimationOutput", animator);
        _Output.SetSourcePlayable(RootPlayable);
    }

    #region Play
    public MercuryState Play(AnimationClip clip,float fadeDuration,FadeMode mode)
        => BaseLayer.Play(clip, fadeDuration, mode);
    
    public void Play(MercuryState state,float fadeDuration, FadeMode mode)
        =>_LayerList.GetLayer(state).Play(state, fadeDuration, mode);

    public MercuryLayer CreateLayer(string name, AvatarMask mask, bool isAdditive) => _LayerList.CreateLayer(name, mask, isAdditive);
    #endregion
    public void DestroyGraph()
    {
        _Graph.Destroy();
    }
}
