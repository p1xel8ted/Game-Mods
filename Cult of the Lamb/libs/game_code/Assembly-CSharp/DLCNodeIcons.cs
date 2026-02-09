// Decompiled with JetBrains decompiler
// Type: DLCNodeIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct DLCNodeIcons
{
  public DungeonWorldMapIcon.NodeType NodeType;
  public DLCNodeIcons.DungeonType dungeonType;
  public Material outlineKeyMaterial;
  public Sprite outlineKeySprite;
  public Material outlineLockMaterial;
  public Sprite outlineLockSprite;
  public Sprite NodeIcon;

  public enum DungeonType
  {
    Dungeon5,
    Dungeon6,
    Key,
    Lock,
    Door,
    None,
  }
}
