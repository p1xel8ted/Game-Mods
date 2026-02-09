// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.Room
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[Serializable]
public class Room
{
  public string ID = "";
  public Rect Dimensions;
  [Range(0.0f, 10f)]
  public float TransitionDuration;
  public EaseType TransitionEaseType;
  public bool ScaleCameraToFit;
  public bool Zoom;
  [Range(0.1f, 10f)]
  public float ZoomScale;

  public Room(Room otherRoom)
  {
    this.Dimensions = otherRoom.Dimensions;
    this.TransitionDuration = otherRoom.TransitionDuration;
    this.TransitionEaseType = otherRoom.TransitionEaseType;
    this.ScaleCameraToFit = otherRoom.ScaleCameraToFit;
    this.Zoom = otherRoom.Zoom;
    this.ZoomScale = otherRoom.ZoomScale;
  }

  public Room()
  {
  }
}
