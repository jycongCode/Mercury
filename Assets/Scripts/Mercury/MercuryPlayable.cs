using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Unity.VisualScripting;
public class MercuryPlayable:PlayableBehaviour
{
    struct PlayableInput
    {
        public Playable source;
        public int port;
        public PlayableInput(Playable source, int port)
        {
            this.source = source;
            this.port = port;
        }
    }
    public const int MAX_MIXER_INPUT = 4;
    private const float WEIGHT_THRESHOLD = 0.01f;
    private const int DEFAULT_LAYER_NUM = 4;
    private MercuryDictionary<MercuryState, PlayableInput> playableDictionary;
    private MercuryState[] portHash;
    private PlayableGraph _Graph;
    public PlayableGraph Graph { get => _Graph; }
    private AnimationPlayableOutput _Output;
    private AnimationLayerMixerPlayable _Layer;
    public AnimationLayerMixerPlayable Layer { get => _Layer; }
    public MercuryLayer[] LayerList;
    private int _usedPortNum = 0;
    public static float DeltaTime;

    private List<IUpdate> preFrameUpdate;

    private static readonly MercuryPlayable Template = new MercuryPlayable();

    public static MercuryPlayable Create()
    {
        var graph = PlayableGraph.Create();
        return ScriptPlayable<MercuryPlayable>.Create(graph, Template, 2).GetBehaviour();
    }

    #region PlayableBehaviour
    public override void OnPlayableCreate(Playable playable)
    {
        _Graph = playable.GetGraph();
        LayerList = new MercuryLayer[DEFAULT_LAYER_NUM];
        preFrameUpdate = new List<IUpdate>();
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);
    }
    #endregion
    public override void CreateOutput(Animator animator)
    {
        _Output = AnimationPlayableOutput.Create(_Graph,"AnimationOutput",animator);
    }


    public void AddLayer()
    {
        if (_usedPortNum < DEFAULT_LAYER_NUM)
        {
            var port = _usedPortNum;
            _usedPortNum++;
            var layer = MercuryLayer.Create(this);
            layer.ConnectGraph(port);
        }
    }

    
    public double GetPlayableSpeed(MercuryState state)
    {
        if (!playableDictionary.IsRegistered(state)) return 0d;
        Playable source = playableDictionary.GetValue(state).source;
        return PlayableExtensions.GetSpeed(source);
    }

    public void SetPlayableSpeed(MercuryState state,double speed)
    {
        if (!playableDictionary.IsRegistered(state)) return;
        Playable source = playableDictionary.GetValue(state).source;
        source.SetSpeed(speed);
    }

    public double GetPlayableNormalizedTime(MercuryState state)
    {
        if (!playableDictionary.IsRegistered(state)) return 0d;
        Playable source = playableDictionary.GetValue(state).source;
        return source.GetTime() / state.Duration;
    }

    public void SetPlayableNormalizedTime(MercuryState state,double normalizedTime)
    {
        if (!playableDictionary.IsRegistered(state) || normalizedTime < 0) return;
        Playable source = playableDictionary.GetValue(state).source;
        source.SetTime(normalizedTime * state.Duration);
    }

    public void UpdateWeight(MercuryState state,float targetWeight)
    {
        if (!playableDictionary.IsRegistered(state)) return;
        PlayableInput playableInput = playableDictionary.GetValue(state);
        if (_usedPortNum <= 1)
        {
            _Mixer.SetInputWeight(playableInput.port, 1f);
        }
        else
        { 
            float restSumWeight = 1f - targetWeight;
            _Mixer.SetInputWeight(playableInput.port, targetWeight);
            List<MercuryState> garbageBag = new List<MercuryState>();
            for(int port = 0;port<MaxMixerInput;++port)
            {
                if (port != playableInput.port&&!_Mixer.GetInput(port).IsNull())
                {
                    
                    float weight = _Mixer.GetInputWeight(port) * restSumWeight;
                    if (Mathf.Abs(weight)<WEIGHT_THRESHOLD)
                    {
                        MercuryState tmp = portHash[port];
                        if(tmp != null)
                        { 
                            _Graph.DestroyPlayable(playableDictionary.GetValue(tmp).source);
                            playableDictionary.UnRegister(tmp);
                            portHash[port] = null;
                        }
                        else
                        {
                            _Graph.DestroyPlayable(_Mixer.GetInput(port));
                        }
                        --_usedPortNum;

                    }
                    else
                    {
                        _Mixer.SetInputWeight(port, weight);
                    }
                }
            }
        }
    }
}
