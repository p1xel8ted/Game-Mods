// Decompiled with JetBrains decompiler
// Type: DeleteIfBossCompleted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DeleteIfBossCompleted : BaseMonoBehaviour
{
  public FollowerLocation Location;
  public GameObject CameraPosition;

  private void Start()
  {
    if (!DataManager.Instance.DoorRoomBossLocksDestroyed.Contains(this.Location))
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
