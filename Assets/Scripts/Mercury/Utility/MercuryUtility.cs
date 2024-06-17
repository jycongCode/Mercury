using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utility
{
   public static AnimationClip LoadClipFromFBX(string fbxName,string clipName)
   {
        var clips = Resources.LoadAll<AnimationClip>(fbxName);
        return clips.FirstOrDefault(clip=>clip.name == clipName);
   }
}
