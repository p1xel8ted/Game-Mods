// Decompiled with JetBrains decompiler
// Type: DeleteIfBossCompleted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
