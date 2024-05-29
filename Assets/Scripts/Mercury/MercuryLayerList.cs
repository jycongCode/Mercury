using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryLayerList : MercuryNode
{
    private const int DEFAULT_LAYER_NUM = 4;
    private MercuryLayer[] _Layers;
    
    public MercuryLayerList(MercuryPlayable root) : base(root)
    {
        _PlayableHandle = AnimationLayerMixerPlayable.Create(_Root.Graph, DEFAULT_LAYER_NUM);
        _Layers = new MercuryLayer[DEFAULT_LAYER_NUM];
        AddLayer(0);

    }

    public void AddLayer(int index)
        => AddLayer(new MercuryLayer(Root), index);
    public void AddLayer(MercuryLayer layer,int index)
    {
        _Layers[index] = layer;
        _PlayableHandle.ConnectInput(index, layer.PlayableHandle, 0);
        _PlayableHandle.SetInputWeight(index, 1f);
    }
    public MercuryLayer GetLayer(MercuryState state) => state.Layer;
    public MercuryLayer GetLayer(int index) => _Layers[index];
}
