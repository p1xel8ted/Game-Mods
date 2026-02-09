// Decompiled with JetBrains decompiler
// Type: WolfBossPreRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WolfBossPreRoomManager : MonoBehaviour
{
  public int originalFleece;
  public int originalVisualFleece;

  public void Awake()
  {
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5)
      return;
    this.originalFleece = DataManager.Instance.PlayerFleece;
    this.originalVisualFleece = DataManager.Instance.PlayerVisualFleece;
    DataManager.Instance.PlayerFleece = 0;
    DataManager.Instance.PlayerVisualFleece = 0;
  }

  public void OnDestroy()
  {
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5)
      return;
    DataManager.Instance.PlayerFleece = this.originalFleece;
    DataManager.Instance.PlayerVisualFleece = this.originalVisualFleece;
  }
}
