// Decompiled with JetBrains decompiler
// Type: EasyCurvedLine.LineSmoother_
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace EasyCurvedLine;

public static class LineSmoother_
{
  public static Vector3[] SmoothLine(Vector3[] inputPoints, float segmentSize)
  {
    AnimationCurve animationCurve1 = new AnimationCurve();
    AnimationCurve animationCurve2 = new AnimationCurve();
    AnimationCurve animationCurve3 = new AnimationCurve();
    Keyframe[] keyframeArray1 = new Keyframe[inputPoints.Length];
    Keyframe[] keyframeArray2 = new Keyframe[inputPoints.Length];
    Keyframe[] keyframeArray3 = new Keyframe[inputPoints.Length];
    for (int time = 0; time < inputPoints.Length; ++time)
    {
      keyframeArray1[time] = new Keyframe((float) time, inputPoints[time].x);
      keyframeArray2[time] = new Keyframe((float) time, inputPoints[time].y);
      keyframeArray3[time] = new Keyframe((float) time, inputPoints[time].z);
    }
    animationCurve1.keys = keyframeArray1;
    animationCurve2.keys = keyframeArray2;
    animationCurve3.keys = keyframeArray3;
    for (int index = 0; index < inputPoints.Length; ++index)
    {
      animationCurve1.SmoothTangents(index, 0.0f);
      animationCurve2.SmoothTangents(index, 0.0f);
      animationCurve3.SmoothTangents(index, 0.0f);
    }
    List<Vector3> vector3List = new List<Vector3>();
    for (int index1 = 0; index1 < inputPoints.Length; ++index1)
    {
      vector3List.Add(inputPoints[index1]);
      if (index1 + 1 < inputPoints.Length)
      {
        float num1 = Vector3.Distance(inputPoints[index1], inputPoints[index1 + 1]);
        if ((double) segmentSize > 1.0)
        {
          int num2 = (int) ((double) num1 / (double) segmentSize);
          for (int index2 = 1; index2 < num2; ++index2)
          {
            float time = (float) index2 / (float) num2 + (float) index1;
            Vector3 vector3 = new Vector3(animationCurve1.Evaluate(time), animationCurve2.Evaluate(time), animationCurve3.Evaluate(time));
            vector3List.Add(vector3);
          }
        }
      }
    }
    return vector3List.ToArray();
  }
}
