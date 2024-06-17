using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MercuryMixerPlayable
{
    public static Playable Create(PlayableGraph graph,Animator animator,int portNum)
    {
        var transforms = animator.GetComponentsInChildren<Transform>();
        var numTransforms = transforms.Length - 1;

        var m_Handles = new NativeArray<TransformStreamHandle>(numTransforms, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        var m_Weights = new NativeArray<float>(portNum, Allocator.Persistent, NativeArrayOptions.ClearMemory); 
        for (var i = 0; i < numTransforms; ++i)
        {
            m_Handles[i] = animator.BindStreamTransform(transforms[i + 1]);
        }

        for (var i = 0;  i< portNum; i++)
        {
            m_Weights[i] = 0f;
        }

        var job = new MercuryMixerJob()
        {
            handles = m_Handles,
            weights = m_Weights,
            portNum = portNum
        };
        return AnimationScriptPlayable.Create(graph, job, portNum);
    }
    public static void SetInputWeight(AnimationScriptPlayable playable,int port,float weight)
    {
        var job = playable.GetJobData<MercuryMixerJob>();
        job.weights[port] = weight;
        playable.SetJobData(job);
    }
    public static void ClearInput(AnimationScriptPlayable playable,int port)=>SetInputWeight(playable,port,0f);
}