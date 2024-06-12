using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercuryStateFactory
{
    private MercuryPlayable _Root;
    public MercuryStateFactory(MercuryPlayable root)
    {
        _Root = root;    
    }
    public MercuryState CreateState(IParam parameter,string name)
    {
        switch (parameter.Type)
        {
            case StateType.ClipState:
                return new MercuryClipState(parameter, _Root);
            case StateType.BlendState:
                return new MercuryBlendState(parameter, _Root);
            default:
                return null;
        }
    }
}
