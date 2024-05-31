using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.Reflection;

public class MercuryPlayable:PlayableBehaviour
{
   
    private PlayableGraph _Graph;
    public PlayableGraph Graph { get => _Graph; }
    private AnimationPlayableOutput _Output;
    public AnimationPlayableOutput Output { get => _Output; }
    private Playable _Playable;
    public static float DeltaTime { get; set; }
    private MercuryLayerList _LayerList;
    private HashSet<IUpdate> _PreFrameUpdate;
    private HashSet<IUpdate> _PreUpdateToDelete;
    private static readonly MercuryPlayable Template = new MercuryPlayable();
    public static MercuryPlayable Create()
    {
        var graph = PlayableGraph.Create();
        return ScriptPlayable<MercuryPlayable>.Create(graph, Template, 2).GetBehaviour();
    }

    #region Update
    public void RequirePreUpdate(IUpdate update)
    {
        _PreFrameUpdate.Add(update); 
    }

    public void CancelPreUpdate(IUpdate update)
    {
        _PreUpdateToDelete.Add(update);
    }

    public void ClearPreUpdate()
    {
        foreach(var update in _PreUpdateToDelete)
        {
            _PreFrameUpdate.Remove(update);
        }
        _PreUpdateToDelete.Clear();
    }
    #endregion

    #region PlayableBehaviour
    public override void OnPlayableCreate(Playable playable)
    {
        _Playable = playable;
        _Graph = playable.GetGraph();
        _LayerList = new MercuryLayerList(this);
        _PreFrameUpdate = new HashSet<IUpdate>();
        _PreUpdateToDelete = new HashSet<IUpdate>();
        _Graph.Connect(_LayerList.PlayableHandle, 0, _Playable, 0);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        foreach(var node in _PreFrameUpdate)
        {
            node.Update();
        }
        DeltaTime = info.deltaTime * info.effectiveParentSpeed;
        ClearPreUpdate();
    }
    #endregion
    public void CreateOutput(Animator animator)
    {
        _Output = AnimationPlayableOutput.Create(_Graph, "AnimationOutput", animator);
        _Output.SetSourcePlayable(_Playable);
    }

    #region Play
    public MercuryState Play(AnimationClip clip,uint layerIndex, float fadeDuration,FadeMode mode)
        =>_LayerList.GetLayer(layerIndex).Play(clip, fadeDuration, mode);
    
    public MercuryState Play(MercuryState state, uint layerIndex, float fadeDuration, FadeMode mode)
        =>_LayerList.GetLayer(layerIndex).Play(state, fadeDuration, mode);
    #endregion
    public void DestroyGraph()
    {
        _Graph.Destroy();
    }

    public uint AddLayer(float weight,bool isAdditive,AvatarMask mask)
        => _LayerList.AddLayer(weight,isAdditive,mask);
}
