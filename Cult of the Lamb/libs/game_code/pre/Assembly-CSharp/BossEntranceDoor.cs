// Decompiled with JetBrains decompiler
// Type: BossEntranceDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BossEntranceDoor : BaseMonoBehaviour
{
  public Bounds LimitCameraBounds;

  private void OnEnable()
  {
    GameManager.GetInstance().CamFollowTarget.SetCameraLimits(this.LimitCameraBounds);
  }

  private void OnDisable() => GameManager.GetInstance().CamFollowTarget.DisableCameraLimits();
}
