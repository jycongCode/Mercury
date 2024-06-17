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
    public AnimationClip boom;
    MercuryState state1;
    MercuryState state2;
    MercuryBlendState blendState;
    MercuryBlendStateParam blendParam;
    private void Awake()
    {
        mercury = GetComponent<MercuryComponent>();
    }
    void Start()
    {
        var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(clips[1]);
        var rootBinding_e = from binding in curveBindings
                          where binding.path == "Root" && binding.propertyName.Contains("m_LocalPosition.z")
                          select binding;
        
        var rootBinding = rootBinding_e.ToArray();
        var curve = AnimationUtility.GetEditorCurve(clips[1], rootBinding[0]);
        
        for(int i = 0; i < 3; ++i)
        {
            var value = curve.Evaluate((i + 1) * 0.03f);
            Debug.Log(value);
        }
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
