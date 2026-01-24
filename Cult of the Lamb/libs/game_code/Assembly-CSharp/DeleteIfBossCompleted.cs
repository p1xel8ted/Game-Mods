// Decompiled with JetBrains decompiler
// Type: DeleteIfBossCompleted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
