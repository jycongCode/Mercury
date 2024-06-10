using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public abstract class MercuryNode
{
    public string Name;
    public Playable PlayableHandle;
    protected MercuryNode _Parent;
    public MercuryPlayable Root;
    protected List<MercuryNode> _Children;
    protected Dictionary<int,MercuryNode> _Nodes;
    protected readonly int _InputPortNum;
    public int Port = -1;
    public bool IsValid { get => PlayableHandle.IsValid(); }
    public bool IsEmpty { get => _Children.Count == 0; }
    public MercuryNode(MercuryPlayable root,string name,int portNum)
    {
        Root = root;
        Name = name;
        _InputPortNum = portNum;
        _Children = new List<MercuryNode>();
        _Nodes = new Dictionary<int, MercuryNode>();
    }

    public int FindAvailablePort()
    {
        //Debug.Log(_InputPortNum);
        for(int p = 0;p< _InputPortNum; ++p)
        {
            if (PlayableHandle.GetInput(p).IsNull())
            {
                return p;
            }
        }
        return -1;
    }

    public void AddChildren(int port,MercuryNode node)
    {
        if (PlayableHandle.GetInput(port).IsValid())
        {
            Debug.LogWarning($"The port of {Name} is already taken");
            return;
        }
        Root.Graph.Connect(node.PlayableHandle,0,PlayableHandle,port);
        node.Port = port;
        _Children.Add(node);
        _Nodes.Add(port, node);
    }

    public void RemoveChildren(MercuryNode node)
    {
        Root.Graph.Disconnect(PlayableHandle, node.Port);
        _Children.Remove(node);
        _Nodes.Remove(node.Port);
    }

    public void RemoveChildren(int port)
    {
        if (_Nodes.ContainsKey(port))
        {
            RemoveChildren(_Nodes[port]);
        }
    }

    public void SetChildWeight(int port,float weight)
    {
        if (_Nodes.ContainsKey(port))
        {
            PlayableHandle.SetInputWeight(port, weight);
        }
        else
        {
            Debug.LogWarning($"Port not found for MercuryNode {Name}");
        }
    }

    public MercuryNode GetParent() => _Parent;
    public void SetParent(MercuryNode parent)=>_Parent = parent;

}
