using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    MercuryComponent mercury;
    public AnimationClip clip;
    public float Speed = 1.0f;
    AnimationState state;
    void Start()
    {
        mercury = GetComponent<MercuryComponent>();
        //Debug.Log(mercury == null);
        state = mercury.Play(clip);

    }

    // Update is called once per frame
    void Update()
    {
        state.Speed = this.Speed;
    }
}
