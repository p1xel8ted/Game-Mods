// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.CinematicTarget
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[Serializable]
public class CinematicTarget
{
  public Transform TargetTransform;
  public float EaseInDuration = 1f;
  public float HoldDuration = 1f;
  public float Zoom = 1f;
  public EaseType EaseType = EaseType.EaseOut;
  public string SendMessageName;
  public string SendMessageParam;
}
