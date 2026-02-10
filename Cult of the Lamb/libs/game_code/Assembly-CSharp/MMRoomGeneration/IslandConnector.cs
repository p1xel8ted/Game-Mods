// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.IslandConnector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace MMRoomGeneration;

public class IslandConnector : BaseMonoBehaviour
{
  public bool Active;
  public IslandConnector.Direction MyDirection;
  public IslandPiece _ParentIsland;

  public IslandPiece ParentIsland
  {
    get
    {
      if ((Object) this._ParentIsland == (Object) null)
        this._ParentIsland = this.GetComponentInParent<IslandPiece>();
      return this._ParentIsland;
    }
  }

  public void OnEnable() => this.GetComponent<SpriteRenderer>().enabled = false;

  public void SetActive()
  {
    this.Active = true;
    this.ParentIsland.NorthConnectors.Remove(this);
    this.ParentIsland.EastConnectors.Remove(this);
    this.ParentIsland.SouthConnectors.Remove(this);
    this.ParentIsland.WestConnectors.Remove(this);
  }

  public enum Direction
  {
    North,
    East,
    South,
    West,
  }
}
