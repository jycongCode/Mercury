using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using System.Runtime.CompilerServices;
namespace Mercury_MM{
    public class test : MonoBehaviour
    {
        public AnimationClip clip;
        public string[] bipsName = { "Bip001/Bip001 Pelvis",
                                    "Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf/Bip001 L Foot",
                                    "Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf/Bip001 R Foot"};
        public static string POSITION = "m_LocalPosition";
        public static string ROTATION = "m_LocalRotation";
        private List<Pose> PoseDatabase;
        public void Awake()
        {
            var bindings = UnityEditor.AnimationUtility.GetCurveBindings(clip);
            var selected = from bind in bindings
                           where bipsName.Contains(bind.path)
                           select bind;
            PoseDatabase = new List<Pose>();
            var frameNum = (int)(clip.frameRate * clip.length);
            for(int i = 0;i<frameNum;i++)
            {
                List<Vector3> posePosition = new List<Vector3>();
                List<Quaternion> poseRotation = new List<Quaternion>();
                List<Vector3> pVelocity = new List<Vector3>();
                List<Quaternion> rVelocity = new List<Quaternion>();
                foreach (var bip in bipsName)
                {
                    var bipInfo = from s in selected
                                  where s.path == bip
                                  select s;
                    var position = from info in bipInfo
                                   where info.propertyName.Contains(POSITION)
                                   select info;
                    var rotation = from info in bipInfo
                                   where info.propertyName.Contains(ROTATION)
                                   select info;
                    Vector3 tempP = Vector3.zero;
                    Vector3 tempVP = Vector3.zero;
                    foreach (var p in position)
                    {
                        var curveData = AnimationUtility.GetEditorCurve(clip, p);
                        switch (p.propertyName.Split('.')[1])
                        {
                            case "x":
                                tempP.x = curveData.Evaluate((float)frameNum / clip.frameRate);
                                break;
                            case "y":
                                tempP.y = curveData.Evaluate((float)frameNum / clip.frameRate);
                                break;
                            case "z":
                                tempP.z = curveData.Evaluate((float)frameNum / clip.frameRate);
                                break;
                        }
                    }

                    Vector3 tempR = Vector3.zero;
                    Quaternion tempVR = Quaternion.identity;
                    foreach (var r in rotation)
                    {
                        var curveData = AnimationUtility.GetEditorCurve(clip, r);
                        switch (r.propertyName.Split('.')[1])
                        {
                            case "x":
                                tempR.x = curveData.Evaluate((float)frameNum / clip.frameRate);
                                break;
                            case "y":
                                tempR.y = curveData.Evaluate((float)frameNum / clip.frameRate);
                                break;
                            case "z":
                                tempR.z = curveData.Evaluate((float)frameNum / clip.frameRate);
                                break;
                        }
                    }
                    var tempQ = Quaternion.Euler(tempR);
                    posePosition.Add(tempP);
                    poseRotation.Add(tempQ);
                }
                if(i>0)
                {
                    var lastP = PoseDatabase[i - 1].Positions;
                    for (int j = 0; j < bipsName.Length; j++)
                    {
                        pVelocity.Add((posePosition[j] - lastP[j]) / clip.frameRate);
                    }
                    if (i == 1)
                        PoseDatabase[0].pVelocity = pVelocity.ToArray();
                }
                //PoseDatabase.Add(new Pose(posePosition.ToArray(), poseRotation.ToArray(), pVelocity.ToArray(), 0, frameNum));
            }
            if (clip.isLooping)
            {
                var lastP = PoseDatabase[frameNum-1].Positions;
                List<Vector3> pv = new List<Vector3>();
                List<Quaternion> rv = new List<Quaternion>();
                for (int j = 0; j < bipsName.Length; j++)
                {
                    pv.Add((PoseDatabase[0].Positions[j] - lastP[j]) / clip.frameRate);
                }
                PoseDatabase[0].pVelocity = pv.ToArray();
            }
        }

        private void Start()
        {
            Debug.Log(PoseDatabase.Count);
        }
    }

    public class Pose
    {   
        // pelvis lf rf
        public Vector3[] Positions;
        public Quaternion[] Rotations;
        public Vector3[] pVelocity;

        //trajectory
        public Vector2[] fPosition;
        public Vector2[] fRotation;
        public int ClipIndex;
        public int FrameIndex;
        public Pose(Vector3[] position, Quaternion[] rotation,Vector3[] pv,Vector2[] fp,Vector2[] fr,int clipIndex,int frameIndex)
        {
            Positions = position;
            Rotations = rotation;
            pVelocity = pv;
            fPosition = fp;
            fRotation = fr;
            ClipIndex = clipIndex;
            FrameIndex = frameIndex;
        }
    }

    public struct Joint
    {
        string name;

    }
}
