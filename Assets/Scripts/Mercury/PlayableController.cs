using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
public class PlayableController
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
    public const int MaxMixerInput = 4;
    private const float WEIGHT_THRESHOLD = 0.01f;

    private MercuryDictionary<AnimationState, PlayableInput> playableDictionary;
    private AnimationState[] portHash;
    private PlayableGraph _graph;
    public PlayableGraph Graph { get { return _graph; } }
    private AnimationPlayableOutput _output;
    private AnimationMixerPlayable _mixer;

   
    private int _usedPortNum = 0;


    public static PlayableController Create(Animator animator,string graphName)
    {
        PlayableController controller = new PlayableController();
        controller.InitializeGraph(animator, graphName);
        controller.playableDictionary = new MercuryDictionary<AnimationState, PlayableInput>();
        controller.portHash = new AnimationState[MaxMixerInput];
        return controller;
    }

    private void InitializeGraph(Animator animator,string graphName)
    {
        _graph = PlayableGraph.Create(graphName);
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        _output = AnimationPlayableOutput.Create(_graph,"AnimationOutput",animator);
        _mixer = AnimationMixerPlayable.Create(_graph, MaxMixerInput);
        _output.SetSourcePlayable(_mixer);
    }

    public void LoadState(AnimationState state,float startWeight)
    {
        if (_usedPortNum >= MaxMixerInput) return;
        //Debug.Log("here");
        AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(_graph, state.clip);
        int portIndex = 0;
        while(portIndex<MaxMixerInput)
        {
            if (_mixer.GetInput(portIndex).IsNull()) break;
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
        _graph.Connect(clipPlayable, 0, _mixer, portIndex);
        _mixer.SetInputWeight(portIndex, startWeight);
        UpdateWeight(state, startWeight);
        ++_usedPortNum;
    }

    public double GetPlayableSpeed(AnimationState state)
    {
        if (!playableDictionary.IsRegistered(state)) return 0d;
        Playable source = playableDictionary.GetValue(state).source;
        return PlayableExtensions.GetSpeed(source);
    }

    public void SetPlayableSpeed(AnimationState state,double speed)
    {
        if (!playableDictionary.IsRegistered(state)) return;
        Playable source = playableDictionary.GetValue(state).source;
        source.SetSpeed(speed);
    }

    public double GetPlayableNormalizedTime(AnimationState state)
    {
        if (!playableDictionary.IsRegistered(state)) return 0d;
        Playable source = playableDictionary.GetValue(state).source;
        return source.GetTime() / state.Duration;
    }

    public void SetPlayableNormalizedTime(AnimationState state,double normalizedTime)
    {
        if (!playableDictionary.IsRegistered(state) || normalizedTime < 0) return;
        Playable source = playableDictionary.GetValue(state).source;
        source.SetTime(normalizedTime * state.Duration);
    }

    public void UpdateWeight(AnimationState state,float targetWeight)
    {
        if (!playableDictionary.IsRegistered(state)) return;
        PlayableInput playableInput = playableDictionary.GetValue(state);
        if (_usedPortNum <= 1)
        {
            _mixer.SetInputWeight(playableInput.port, 1f);
        }
        else
        { 
            float restSumWeight = 1f - targetWeight;
            _mixer.SetInputWeight(playableInput.port, targetWeight);
            List<AnimationState> garbageBag = new List<AnimationState>();
            for(int port = 0;port<MaxMixerInput;++port)
            {
                if (port != playableInput.port&&!_mixer.GetInput(port).IsNull())
                {
                    
                    float weight = _mixer.GetInputWeight(port) * restSumWeight;
                    if (Mathf.Abs(weight)<WEIGHT_THRESHOLD)
                    {
                        AnimationState tmp = portHash[port];
                        if(tmp != null)
                        { 
                            _graph.DestroyPlayable(playableDictionary.GetValue(tmp).source);
                            playableDictionary.UnRegister(tmp);
                            portHash[port] = null;
                        }
                        else
                        {
                            _graph.DestroyPlayable(_mixer.GetInput(port));
                        }
                        --_usedPortNum;

                    }
                    else
                    {
                        _mixer.SetInputWeight(port, weight);
                    }
                }
            }
        }
    }
}
