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
    public AnimationClip WalkClip;
    private MercuryState WalkState;
    [Range(-1,2)]
    public float Speed = 1f;

    public AvatarMask Mask;
    private void Awake()
    {
        mercury = GetComponent<MercuryComponent>();
    }
    void Start()
    {
        WalkState = mercury.Play(IdleClip);
        
    }

    // Update is called once per frame
    void Update()
    {
        mercury.Play(WalkState);
    }
    
}
