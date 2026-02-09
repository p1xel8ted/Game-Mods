// Decompiled with JetBrains decompiler
// Type: DeleteIfBossCompleted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DeleteIfBossCompleted : BaseMonoBehaviour
{
  public FollowerLocation Location;
  public GameObject CameraPosition;

  public void Start()
  {
    if (!DataManager.Instance.DoorRoomBossLocksDestroyed.Contains(this.Location))
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
