// Decompiled with JetBrains decompiler
// Type: EasyCurvedLine.LineSmoother_
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace EasyCurvedLine;

public class LineSmoother_
{
  public AnimationCurve curveX = new AnimationCurve();
  public AnimationCurve curveY = new AnimationCurve();
  public AnimationCurve curveZ = new AnimationCurve();
  public Keyframe[] keysX = new Keyframe[0];
  public Keyframe[] keysY = new Keyframe[0];
  public Keyframe[] keysZ = new Keyframe[0];
  public List<Vector3> lineSegments = new List<Vector3>();

  public Vector3[] SmoothLine(Vector3[] inputPoints, float segmentSize)
  {
    if (inputPoints.Length != this.keysX.Length)
      this.UpdateKeyframesLengths(inputPoints.Length);
    for (int index = 0; index < inputPoints.Length; ++index)
    {
      this.keysX[index].value = inputPoints[index].x;
      this.keysY[index].value = inputPoints[index].y;
      this.keysZ[index].value = inputPoints[index].z;
    }
    this.curveX.keys = this.keysX;
    this.curveY.keys = this.keysY;
    this.curveZ.keys = this.keysZ;
    for (int index = 0; index < inputPoints.Length; ++index)
    {
      this.curveX.SmoothTangents(index, 0.0f);
      this.curveY.SmoothTangents(index, 0.0f);
      this.curveZ.SmoothTangents(index, 0.0f);
    }
    this.lineSegments.Clear();
    for (int index1 = 0; index1 < inputPoints.Length; ++index1)
    {
      this.lineSegments.Add(inputPoints[index1]);
      if (index1 + 1 < inputPoints.Length)
      {
        float num1 = Vector3.Distance(inputPoints[index1], inputPoints[index1 + 1]);
        if ((double) segmentSize > 1.0)
        {
          int num2 = (int) ((double) num1 / (double) segmentSize);
          for (int index2 = 1; index2 < num2; ++index2)
          {
            float time = (float) index2 / (float) num2 + (float) index1;
            this.lineSegments.Add(new Vector3(this.curveX.Evaluate(time), this.curveY.Evaluate(time), this.curveZ.Evaluate(time)));
          }
        }
      }
    }
    return this.lineSegments.ToArray();
  }

  public void UpdateKeyframesLengths(int length)
  {
    if (length != this.keysX.Length)
    {
      this.keysX = new Keyframe[length];
      this.keysY = new Keyframe[length];
      this.keysZ = new Keyframe[length];
    }
    for (int time = 0; time < length; ++time)
    {
      this.keysX[time] = new Keyframe((float) time, 0.0f);
      this.keysY[time] = new Keyframe((float) time, 0.0f);
      this.keysZ[time] = new Keyframe((float) time, 0.0f);
    }
  }
}
