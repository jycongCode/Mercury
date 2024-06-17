using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;

public struct MercuryMixerJob : IAnimationJob
{
    public NativeArray<TransformStreamHandle> handles;
    public NativeArray<float> weights;
    public int portNum;
    public void ProcessRootMotion(AnimationStream stream)
    {   
        var velocity = Vector3.zero;
        var angularVelocity = Vector3.zero;
        var totalWeight = 0f;
        for(int i = 0;i<portNum;i++)
        {
            if (weights[i]>0f)
            {
                var s = stream.GetInputStream(i);
                velocity = Vector3.Lerp(s.velocity, velocity, totalWeight / (totalWeight + weights[i]));
                angularVelocity = Vector3.Lerp(s.angularVelocity,angularVelocity,totalWeight/(totalWeight + weights[i]));
                totalWeight += weights[i];
            }
        }
        stream.velocity = velocity;
        stream.angularVelocity = angularVelocity;
    }

    public void ProcessAnimation(AnimationStream stream)
    {
        var numHandles = handles.Length;
        for(var i = 0; i < numHandles; i++)
        {
            var handle = handles[i];
            var pos = Vector3.zero;
            var rot = Quaternion.identity;
            var totalWeight = 0f;
            for (int j = 0; j < portNum; j++)
            {
                if (weights[j] > 0f)
                {
                    var s = stream.GetInputStream(j);
                    pos = Vector3.Lerp(handle.GetLocalPosition(s),pos,totalWeight/ (totalWeight + weights[j]));
                    rot = Quaternion.Slerp(handle.GetLocalRotation(s),rot,totalWeight/ (totalWeight+ weights[j]));
                    totalWeight += weights[j];  
                }
            }
            handle.SetLocalPosition(stream, pos);
            handle.SetLocalRotation(stream, rot);
        }
    }
}
