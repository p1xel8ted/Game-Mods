// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarColor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[Serializable]
public class AstarColor
{
  public Color _NodeConnection;
  public Color _UnwalkableNode;
  public Color _BoundsHandles;
  public Color _ConnectionLowLerp;
  public Color _ConnectionHighLerp;
  public Color _MeshEdgeColor;
  public Color _MeshColor;
  public Color[] _AreaColors;
  public static Color NodeConnection = new Color(1f, 1f, 1f, 0.9f);
  public static Color UnwalkableNode = new Color(1f, 0.0f, 0.0f, 0.5f);
  public static Color BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
  public static Color ConnectionLowLerp = new Color(0.0f, 1f, 0.0f, 0.5f);
  public static Color ConnectionHighLerp = new Color(1f, 0.0f, 0.0f, 0.5f);
  public static Color MeshEdgeColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
  public static Color MeshColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
  public static Color[] AreaColors;

  public static Color GetAreaColor(uint area)
  {
    return AstarColor.AreaColors == null || (long) area >= (long) AstarColor.AreaColors.Length ? AstarMath.IntToColor((int) area, 1f) : AstarColor.AreaColors[(int) area];
  }

  public void OnEnable()
  {
    AstarColor.NodeConnection = this._NodeConnection;
    AstarColor.UnwalkableNode = this._UnwalkableNode;
    AstarColor.BoundsHandles = this._BoundsHandles;
    AstarColor.ConnectionLowLerp = this._ConnectionLowLerp;
    AstarColor.ConnectionHighLerp = this._ConnectionHighLerp;
    AstarColor.MeshEdgeColor = this._MeshEdgeColor;
    AstarColor.MeshColor = this._MeshColor;
    AstarColor.AreaColors = this._AreaColors;
  }

  public AstarColor()
  {
    this._NodeConnection = new Color(1f, 1f, 1f, 0.9f);
    this._UnwalkableNode = new Color(1f, 0.0f, 0.0f, 0.5f);
    this._BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
    this._ConnectionLowLerp = new Color(0.0f, 1f, 0.0f, 0.5f);
    this._ConnectionHighLerp = new Color(1f, 0.0f, 0.0f, 0.5f);
    this._MeshEdgeColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    this._MeshColor = new Color(0.125f, 0.686f, 0.0f, 0.19f);
  }
}
