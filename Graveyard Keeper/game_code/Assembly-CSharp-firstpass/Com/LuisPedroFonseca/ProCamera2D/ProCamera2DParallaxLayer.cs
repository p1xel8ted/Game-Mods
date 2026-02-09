// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DParallaxLayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[Serializable]
public class ProCamera2DParallaxLayer
{
  public Camera ParallaxCamera;
  [Range(0.0f, 5f)]
  public float Speed = 1f;
  public LayerMask LayerMask;
  [HideInInspector]
  [NonSerialized]
  public Transform CameraTransform;
}
