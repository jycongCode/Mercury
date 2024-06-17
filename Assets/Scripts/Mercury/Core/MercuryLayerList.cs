using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryLayerList : MercuryNode,IUpdate
{
    public MercuryLayerList(MercuryPlayable root,string name,int portNum) : base(root,name,portNum)
    {
        PlayableHandle = AnimationLayerMixerPlayable.Create(Root.Graph, portNum);
    }

    public MercuryLayer CreateLayer(Animator animator,string name,AvatarMask mask=null,bool isAdditive=false)
    {
        var port = FindAvailablePort();
        if (port == -1)
        {
            Debug.LogWarning($"Layer {name} creation failed");
            return null;
        }
        var layer = new MercuryLayer(animator,Root, name, 4,mask,isAdditive);
        layer.Port = port;
        return layer;
    }

    public void AddLayer(MercuryLayer layer)
    {
        AddChildren(layer.Port, layer);
        if (layer.Mask != null)
        {
            var layerMixer = (AnimationLayerMixerPlayable)PlayableHandle;
            layerMixer.SetLayerMaskFromAvatarMask((uint)layer.Port,layer.Mask);
            layerMixer.SetLayerAdditive((uint)layer.Port, layer.IsAdditive);
        }
    }

    public void RemoveLayer(MercuryLayer layer)=>RemoveChildren(layer);

    public void RemoveLayer(int layerIndex)=>RemoveChildren(layerIndex);

    public MercuryLayer GetLayer(int index)
    {
        if(_Nodes.ContainsKey(index))return _Nodes[index] as MercuryLayer;
        return null;
    }
    public MercuryLayer GetLayer(MercuryState state)
    {
        if (_Nodes.ContainsKey(state.LayerIndex))
        {
            return _Nodes[state.LayerIndex] as MercuryLayer;
        }
        else return null;
    }

    public bool Update()
    {
        int length = _Children.Count;
        for (int i = length - 1; i >= 0; i--)
        {
            var child = _Children[i];
            var isContinue = (child as IUpdate).Update();
            if (!isContinue) _Children.RemoveAt(i);
            else SetChildWeight(child.Port, (child as MercuryLayer).Weight);
        }
        return true;
    }
}
