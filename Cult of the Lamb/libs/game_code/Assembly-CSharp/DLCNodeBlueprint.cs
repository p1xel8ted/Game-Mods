// Decompiled with JetBrains decompiler
// Type: DLCNodeBlueprint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu]
public class DLCNodeBlueprint : ScriptableObject
{
  public Sprite UndiscoveredDungeon5;
  public Sprite UndiscoveredDungeon6;
  public List<DLCNodeIcons> nodes = new List<DLCNodeIcons>();
  [Space]
  public bool Testing;

  public DLCNodeIcons GetIcon(DungeonWorldMapIcon.NodeType dungeonLocation, bool decoration = true)
  {
    foreach (DLCNodeIcons node in this.nodes)
    {
      if (node.NodeType == dungeonLocation)
      {
        Debug.Log((object) $"Found {node.NodeType} in {this.name}");
        return node;
      }
    }
    return new DLCNodeIcons();
  }
}
