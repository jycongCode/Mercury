using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Reflection;
class AnimationEvent
{
    public string Name;
    public float NormalizedTime;
    public event Action CallBack;
    public void Invoke()
    {
        CallBack?.Invoke();
    }

    public AnimationEvent(string name,float normalizedTime, Action callback)
    {
        Name = name;
        NormalizedTime = normalizedTime;
        CallBack += callback;
    }
}
public class MercuryEventManager
{
    private float endTime;
    private int playDirection;
    public int PlayDirection
    {
        get { return playDirection; }
        set { 
            playDirection = value;
            if (!isPlayed)
            {
                currendIndex = value < 0 ? this.eventlist.Count - 1 : 0;
            }
        }
    }
    private List<AnimationEvent> eventlist;
    private HashSet<string> nameSet;
    private int currendIndex;
    private float previousTime;
    private bool isPlayed;
    public MercuryEventManager(int playDirection)
    {
        this.isPlayed = false;
        this.playDirection = playDirection;
        this.previousTime = 0f;
        this.eventlist = new List<AnimationEvent>();
        this.nameSet = new HashSet<string>();
        this.currendIndex = -1;
    }

    private float ToLocalTime(float t)
        => t -Mathf.Floor(t);
    public void ReWind(float normalizedTime)
    {
        float localTime = ToLocalTime(normalizedTime);
        int index = -1;
        if(playDirection>0)
        {
            index = Mathf.Max(this.eventlist.FindIndex(value => value.NormalizedTime > localTime), 0);
        }
        else
        {
            index = Mathf.Min(this.eventlist.FindLastIndex(value => value.NormalizedTime < localTime), this.eventlist.Count - 1);
        }
        this.currendIndex = index;
    }

    public void Update(float timePassed)
    {
        float localTime = ToLocalTime((float)timePassed);
        if(Mathf.Floor(previousTime)!=Mathf.Floor(timePassed)) 
        {
            this.currendIndex = (this.currendIndex + this.eventlist.Count) % this.eventlist.Count;
        }

        if (this.currendIndex < this.eventlist.Count && this.currendIndex >= 0)
        {
            if (Mathf.Abs(localTime - this.eventlist[this.currendIndex].NormalizedTime)<0.05f)
            {

                this.eventlist[this.currendIndex].Invoke();
                this.currendIndex += this.playDirection;
            }
        }
        this.previousTime = timePassed;
        this.isPlayed = true;
    }

    public void AddEvent(string name,float timeKey,Action callback)
    {
        if (timeKey > endTime||this.nameSet.Contains(name)) return;
        if (this.eventlist.Count == 0)
        {
            this.currendIndex = 0;
            this.eventlist.Add(new AnimationEvent(name, timeKey, callback));
        }
        else
        {
            int index = this.eventlist.FindLastIndex(value => value.NormalizedTime < timeKey);
            if (index < 0) index = ~index;
            if (index <= this.currendIndex) currendIndex++;
            this.eventlist.Insert(index,new AnimationEvent(name, timeKey, callback));
        }
        nameSet.Add(name);
    }
    
    public void Remove(string name)
    {
        if (!this.nameSet.Contains(name)) return;
        int index = this.eventlist.FindIndex(value => value.Name == name);
        this.eventlist.RemoveAt(index);
        nameSet.Remove(name);
    }
}
