using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationStateManager:StateManagerBase<string,AnimationState>
{
    public PlayableController controller;
    
    public static AnimationStateManager Create(Animator animator,string graphName)
    {
        AnimationStateManager stateManager = new AnimationStateManager();
        stateManager.stateDictionary = new MercuryDictionary<string,AnimationState>();
        stateManager.controller = PlayableController.Create(animator, graphName);
        return stateManager;
    }

    public override void TransitState(string key)
    {
        
        if (stateDictionary.IsRegistered(key))
        {
            if(_currentState == null)
            {
                UpdateCurrentState(key);
                return;
            }
            AnimationState currentState = (AnimationState)_currentState;
            switch (currentState.enterType)
            {
                case EnterType.FromStart:
                {
                    UpdateCurrentState(key);
                    break;
                }
                case EnterType.Regular:
                {
                    if (!ReferenceEquals(currentState, stateDictionary.GetValue(key)))
                        UpdateCurrentState(key);
                    break;
                }
                default:break;
            }
        }
    }

    public override void Clear()
    {
        controller.Graph.Destroy();
        stateDictionary.Clear();
        _currentState = null;
    }

    private void UpdateCurrentState(string key) {
        if(_currentState != null) _currentState.OnExit();
        _currentStateKey = key;
        _currentState = stateDictionary.GetValue(key);
        _currentState.OnEnter();
    }
}
