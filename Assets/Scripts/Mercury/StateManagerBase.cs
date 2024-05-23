using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManagerBase<TKey, TState>
    where TKey:IComparable<TKey>
    where TState:IState
{
    public MercuryDictionary<TKey, TState> stateDictionary;
    protected TKey _currentStateKey;
    protected IState _currentState;

    public virtual void Update() => _currentState.OnUpdate();

    public virtual void TransitState(TKey key)
    {
        if (stateDictionary.IsRegistered(key)&&
            !ReferenceEquals(_currentState,stateDictionary.GetValue(key)))
        {
            if (_currentState != null) _currentState.OnExit();
            _currentStateKey = key;
            _currentState = stateDictionary.GetValue(key);
            _currentState.OnEnter();
        }
    }

    public virtual void Clear() { }
}
