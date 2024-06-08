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
    private MercuryState IdleState;
    [Range(-1,2)]
    public float Speed = 1f;

    public AvatarMask Mask;
    private void Awake()
    {
        mercury = GetComponent<MercuryComponent>();
    }
    void Start()
    {
        WalkState = mercury.Play(WalkClip,0,0.25f,FadeMode.Regular);
        WalkState.OnEnd += PlayIdle;
    }

    void PlayIdle()
    {
        Debug.Log("here");
        IdleState = mercury.Play(IdleClip);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            mercury.Play(IdleClip, 0, 0.25f, FadeMode.FromStart);
        }
    }
    
}
