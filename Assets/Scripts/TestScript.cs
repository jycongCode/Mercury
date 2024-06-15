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
    public AnimationClip boom;
    MercuryBlendState blendState;
    MercuryBlendStateParam blendParam;
    private void Awake()
    {
        mercury = GetComponent<MercuryComponent>();
    }
    void Start()
    {
        var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(clips[0]);
        foreach(var binding in curveBindings)
        {
            Debug.Log($"{binding.path}--{binding.propertyName}");
        }
        blendParam = new MercuryBlendStateParam(clips);
        blendState = mercury.Play(blendParam) as MercuryBlendState;
    }
    
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            mercury.Play(boom);
        }
        else
        {
            blendState.Parameter = param;
        }
    }
    
}
