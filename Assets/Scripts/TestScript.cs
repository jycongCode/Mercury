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
    void Start()
    {
        mercury = GetComponent<MercuryComponent>();
        mercury.Play(IdleClip, "Idle");
        LinkedList<int> t = new LinkedList<int>();
        Debug.Log(t.First == null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mercury.Play(walkClip, "Walk",EnterType.Regular);
        }
    }
    
}
