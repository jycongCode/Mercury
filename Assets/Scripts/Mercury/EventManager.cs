using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventManager
{
    struct AnimationEvent
    {
        public double normalizedTime;
        public Action callback;
        public AnimationEvent(double normalizedTime,Action callback)
        {
            this.normalizedTime = normalizedTime;
            this.callback = callback;
        }
    }

    private LinkedList<AnimationEvent> _eventList;
    private LinkedListNode<AnimationEvent> _currentPtr;

    EventManager()
    {
        _eventList = new LinkedList<AnimationEvent>();
    }

    public static EventManager Create()
    {
        return new EventManager();
    }

    //public LinkedListNode<AnimationEvent> Locate(double currentTime)
    //{
    //    LinkedListNode<AnimationEvent> node = _eventList.First;
    //    while (node!=null&&node.Value.normalizedTime < currentTime)
    //    {
    //        node = _currentPtr.Next;
    //    }
    //    return node;
    //}

    public void UpdateInvoke(double currentTime)
    {
        if (_currentPtr != null && _currentPtr.Value.normalizedTime > currentTime)
        {
            _currentPtr.Value.callback();
            _currentPtr = _currentPtr.Next;
        }
    }

    public void Register(double normalizedTime,Action callback)
    {
        AnimationEvent newEvent = new AnimationEvent(normalizedTime, callback);
        //LinkedListNode<AnimationEvent> node = Relocate(normalizedTime);

    }

}
