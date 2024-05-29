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
    public MercuryState Play(AnimationClip clip, float fadeDuration,FadeMode mode)
    {
        return _LayerList.GetLayer(0).Play(clip, fadeDuration, mode);
    }
    #endregion
    public void DestroyGraph()
    {
        _Graph.Destroy();
    }


    //public void AddLayer()
    //{
    //    if (_usedPortNum < DEFAULT_LAYER_NUM)
    //    {
    //        var port = _usedPortNum;
    //        _usedPortNum++;
    //        var layer = MercuryLayer.Create(this);
    //        layer.ConnectGraph(port);
    //    }
    //}


    //public double GetPlayableSpeed(MercuryState state)
    //{
    //    if (!playableDictionary.IsRegistered(state)) return 0d;
    //    Playable source = playableDictionary.GetValue(state).source;
    //    return PlayableExtensions.GetSpeed(source);
    //}

    //public void SetPlayableSpeed(MercuryState state,double speed)
    //{
    //    if (!playableDictionary.IsRegistered(state)) return;
    //    Playable source = playableDictionary.GetValue(state).source;
    //    source.SetSpeed(speed);
    //}

    //public double GetPlayableNormalizedTime(MercuryState state)
    //{
    //    if (!playableDictionary.IsRegistered(state)) return 0d;
    //    Playable source = playableDictionary.GetValue(state).source;
    //    return source.GetTime() / state.Duration;
    //}

    //public void SetPlayableNormalizedTime(MercuryState state,double normalizedTime)
    //{
    //    if (!playableDictionary.IsRegistered(state) || normalizedTime < 0) return;
    //    Playable source = playableDictionary.GetValue(state).source;
    //    source.SetTime(normalizedTime * state.Duration);
    //}

    //public void UpdateWeight(MercuryState state,float targetWeight)
    //{
    //    if (!playableDictionary.IsRegistered(state)) return;
    //    PlayableInput playableInput = playableDictionary.GetValue(state);
    //    if (_usedPortNum <= 1)
    //    {
    //        _Mixer.SetInputWeight(playableInput.port, 1f);
    //    }
    //    else
    //    { 
    //        float restSumWeight = 1f - targetWeight;
    //        _Mixer.SetInputWeight(playableInput.port, targetWeight);
    //        List<MercuryState> garbageBag = new List<MercuryState>();
    //        for(int port = 0;port<MaxMixerInput;++port)
    //        {
    //            if (port != playableInput.port&&!_Mixer.GetInput(port).IsNull())
    //            {

    //                float weight = _Mixer.GetInputWeight(port) * restSumWeight;
    //                if (Mathf.Abs(weight)<WEIGHT_THRESHOLD)
    //                {
    //                    MercuryState tmp = portHash[port];
    //                    if(tmp != null)
    //                    { 
    //                        _Graph.DestroyPlayable(playableDictionary.GetValue(tmp).source);
    //                        playableDictionary.UnRegister(tmp);
    //                        portHash[port] = null;
    //                    }
    //                    else
    //                    {
    //                        _Graph.DestroyPlayable(_Mixer.GetInput(port));
    //                    }
    //                    --_usedPortNum;

    //                }
    //                else
    //                {
    //                    _Mixer.SetInputWeight(port, weight);
    //                }
    //            }
    //        }
    //    }
    //}
}
