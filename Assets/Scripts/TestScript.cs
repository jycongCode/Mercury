using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEditor;
public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    MercuryComponent mercury;
    public AnimationClip IdleClip;
    public AnimationClip walkClip;
    [Range(0f, 1f)]
    public double NormalizedTime;
    MercuryState state;
    void Start()
    {
        mercury = GetComponent<MercuryComponent>();
        state = mercury.Play(walkClip);
        state.Events.AddEvent("Half",0.5f,OnHalfRaised);
        state.Speed = 1;
    }

    public void OnHalfRaised()
    {
        Debug.Log($"OnHalfRaised : time{state.NormalizedTime}");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            state.NormalizedTime = Random.Range(0f, 1f);
            Debug.Log(state.NormalizedTime);
        }
    }
    
}
