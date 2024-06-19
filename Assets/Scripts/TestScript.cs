using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEditor;
using System.Linq;
public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    MercuryComponent mercury;
    [Range(0,1)]
    public float param;
    public AnimationClip[] clips;
    MercuryState state1;
    MercuryState state2;
    private void Awake()
    {
        mercury = GetComponent<MercuryComponent>();
    }
    void Start()
    {
        state1 = mercury.Play(clips[0]);
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state2 = mercury.Play(clips[1],2f);
        }

        if(state2 != null)
        {
            state2.Speed = param;
        }
    }
    
}
