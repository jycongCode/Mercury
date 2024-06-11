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
    [Range(0,1)]
    public float param;
    public AnimationClip[] clips;
    MercuryBlendState blendState;
    MercuryBlendStateParam blendParam;
    private void Awake()
    {
        mercury = GetComponent<MercuryComponent>();
    }
    void Start()
    {
        blendParam = new MercuryBlendStateParam(clips);
        blendState = mercury.Play(blendParam) as MercuryBlendState;
    }
    
    void Update()
    {
        blendState.Parameter = param;
    }
    
}
