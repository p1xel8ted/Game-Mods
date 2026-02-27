// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.IslandConnector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace MMRoomGeneration;

public class IslandConnector : BaseMonoBehaviour
{
  public bool Active;
  public IslandConnector.Direction MyDirection;
  private IslandPiece _ParentIsland;

  public IslandPiece ParentIsland
  {
    get
    {
      if ((Object) this._ParentIsland == (Object) null)
        this._ParentIsland = this.GetComponentInParent<IslandPiece>();
      return this._ParentIsland;
    }
  }

  private void OnEnable() => this.GetComponent<SpriteRenderer>().enabled = false;

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
