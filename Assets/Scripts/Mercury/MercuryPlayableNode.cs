using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MercuryPlayableNode:IEnumerable
{
    public const int MAX_CHILD_COUNT = 4;
    public MercuryPlayableNode Root;
    public MercuryPlayableNode Parent;
    public MercuryPlayableNode[] Children;
    protected int _childrenNum;
    public int ChildrenNum { get { return _childrenNum; } }
    protected MercuryPlayableNode(MercuryPlayableNode root)
    {
        Children = new MercuryPlayableNode[MAX_CHILD_COUNT];
        Root = root;
        _childrenNum = 0;
    }
    public virtual void SetChild(int index,MercuryPlayableNode child)
    {
        if (_childrenNum < MAX_CHILD_COUNT && Children[index] == null)
        {
            Children[index] = child;
            child.SetParent(this);
            _childrenNum++;
        }
    }

    public virtual void SetParent(MercuryPlayableNode parent)
    {
        Parent = parent;
    }

    public virtual void RemoveChild(int index) {
        if (_childrenNum == 0) return;
        if(Children[index] != null )
        {
            Children[index].Destroy();
        }
    }

    public virtual void Destroy() { }

    public IEnumerator GetEnumerator()
    {
        return new MercuryPlayableNodeEnumerator(Children);
    }
}

public class MercuryPlayableNodeEnumerator : IEnumerator
{
    private MercuryPlayableNode[] _children;
    private int _current = -1;
    public MercuryPlayableNodeEnumerator(MercuryPlayableNode[] children)
    {
        this._children = children;
    }
    public object Current
    {
        get
        {
            if (_current < 0 || _current > MercuryPlayableNode.MAX_CHILD_COUNT)
            {
                return null;
            }
            else
            {
                return _children[_current];
            }
        }
    }

    public bool MoveNext()
    {
        _current++;
        while (_current < MercuryPlayableNode.MAX_CHILD_COUNT && _children[_current] == null) _current++;
        if (_current < MercuryPlayableNode.MAX_CHILD_COUNT) return true;
        return false;
    }

    public void Reset()
    {
        _current = 0;
    }
}
