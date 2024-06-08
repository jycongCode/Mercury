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

    protected readonly int _InputPortNum;
    public int Port = -1;
    public bool IsValid { get => PlayableHandle.IsValid(); }
    public MercuryNode(MercuryPlayable root,string name,int portNum)
    {
        Root = root;
        Name = name;
        _InputPortNum = portNum;
        _Children = new List<MercuryNode>();
    }

    public int FindAvailablePort()
    {
        int port = -1;
        int lenth = _Children.Count;
        for(int i = 0;i<lenth; i++)
        {
            if (!_Children[i].IsValid)
            {
                port = i;
                break;
            }
        }
        Debug.LogWarning($"No available port in {Name}");
        return port;
    }

    public void AddChildren(int port,MercuryNode node)
    {
        Root.Graph.Connect(node.PlayableHandle,0,PlayableHandle,port);
        node.Port = port;
        _Children.Add(node);
    }

    public void RemoveChildren(MercuryNode node)
    {
        Root.Graph.Disconnect(PlayableHandle, node.Port);
        node.Port = -1;
        _Children.Remove(node);
    }

    public void SetChildWeight(int port,float weight)
    {
        if (_Children[port].IsValid)
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
