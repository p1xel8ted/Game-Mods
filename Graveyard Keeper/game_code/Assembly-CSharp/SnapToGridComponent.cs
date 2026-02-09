// Decompiled with JetBrains decompiler
// Type: SnapToGridComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SnapToGridComponent : MonoBehaviour
{
  [Range(1f, 48f)]
  public int grid_divider = 1;
  [Range(-50f, 50f)]
  public int fine_tune_z;
  public static List<int> _grid_mdfrs;

  public virtual void Update()
  {
    if (Application.isPlaying)
      return;
    this.DoRound();
  }

  public void DoRound()
  {
    Transform transform = this.transform;
    float num = this.GetPixelSize() / (float) this.grid_divider;
    Vector3 localPosition = transform.localPosition;
    Vector3 vector3 = new Vector3(Mathf.Round(localPosition.x / num) * num, Mathf.Round(localPosition.y / num) * num, localPosition.z);
    if (Application.isPlaying && (double) (vector3 - transform.localPosition).magnitude < 0.001)
      return;
    transform.localPosition = vector3;
  }

  public virtual float GetPixelSize() => 1f;

  public static List<int> GetGridModifiers()
  {
    if (SnapToGridComponent._grid_mdfrs != null)
      return SnapToGridComponent._grid_mdfrs;
    SnapToGridComponent._grid_mdfrs = new List<int>();
    for (int index = 1; index < 96 /*0x60*/; ++index)
    {
      if (96 /*0x60*/ % index == 0)
        SnapToGridComponent._grid_mdfrs.Add(index);
    }
    return SnapToGridComponent._grid_mdfrs;
  }

  public void OnValidate()
  {
    int index1 = -1;
    int num1 = 99999;
    List<int> gridModifiers = SnapToGridComponent.GetGridModifiers();
    for (int index2 = 0; index2 < gridModifiers.Count; ++index2)
    {
      if (this.grid_divider == gridModifiers[index2])
        return;
      int num2 = Math.Abs(gridModifiers[index2] - this.grid_divider);
      if (num2 < num1)
      {
        num1 = num2;
        index1 = index2;
      }
    }
    this.grid_divider = gridModifiers[index1];
  }
}
