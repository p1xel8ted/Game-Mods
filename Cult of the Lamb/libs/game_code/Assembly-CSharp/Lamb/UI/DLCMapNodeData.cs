// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DLCMapNodeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public struct DLCMapNodeData
{
  public Vector3 Position;
  public float RadiusScale;
  public float Influence;

  public static int Size() => 20;
}
