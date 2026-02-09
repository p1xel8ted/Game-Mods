// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.CameraTarget
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[Serializable]
public class CameraTarget
{
  public Transform TargetTransform;
  [Range(0.0f, 1f)]
  public float TargetInfluenceH = 1f;
  [Range(0.0f, 1f)]
  public float TargetInfluenceV = 1f;
  public Vector2 TargetOffset;
  public Vector3 _targetPosition;

  public float TargetInfluence
  {
    set
    {
      this.TargetInfluenceH = value;
      this.TargetInfluenceV = value;
    }
  }

  public Vector3 TargetPosition
  {
    get
    {
      return (UnityEngine.Object) this.TargetTransform != (UnityEngine.Object) null ? (this._targetPosition = this.TargetTransform.position) : this._targetPosition;
    }
  }
}
