// Decompiled with JetBrains decompiler
// Type: DLCNodeIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
