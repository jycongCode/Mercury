using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryLayerList : MercuryNode
{
    private const int DEFAULT_LAYER_NUM = 4;
    private List<MercuryLayer> _Layers;
    public const int BASE_LAYER_INDEX = 0;
    public MercuryLayerList(MercuryPlayable root) : base(root)
    {
        _PlayableHandle = AnimationLayerMixerPlayable.Create(_Root.Graph, DEFAULT_LAYER_NUM);
        _Layers = new List<MercuryLayer>();
        AddLayer(1f,false,null);

    }
    public uint AddLayer(float weight,bool isAdditive, AvatarMask mask)
        => AddLayer(new MercuryLayer(Root),weight,isAdditive,mask);
    public uint AddLayer(MercuryLayer layer,float weight,bool isAdditive, AvatarMask mask)
    {
        var index = _Layers.Count;
        var layerMixer = (AnimationLayerMixerPlayable)_PlayableHandle;
        _Layers.Add(layer);
        _PlayableHandle.ConnectInput(index, layer.PlayableHandle, 0);
        _PlayableHandle.SetInputWeight(index, weight);
        layerMixer.SetLayerAdditive((uint)index,isAdditive);
        if (mask) layerMixer.SetLayerMaskFromAvatarMask((uint)index,mask);
        return (uint)index;
    }
    public MercuryLayer GetLayer(uint index) => _Layers[(int)index];
    public MercuryLayer GetLayer(MercuryState state) => state.Layer;
}
