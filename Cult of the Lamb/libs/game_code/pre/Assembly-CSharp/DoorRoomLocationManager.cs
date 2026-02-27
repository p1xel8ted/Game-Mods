// Decompiled with JetBrains decompiler
// Type: DoorRoomLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class DoorRoomLocationManager : LocationManager
{
  public static DoorRoomLocationManager Instance;
  public Transform DoorPosition;
  public Animator SkyAnimator;

  public override FollowerLocation Location => FollowerLocation.DoorRoom;

  public override Transform UnitLayer => this.transform;

  public override Transform StructureLayer => this.transform;

  protected override void Awake()
  {
    DoorRoomLocationManager.Instance = this;
    base.Awake();
  }

  protected override void OnEnable() => base.OnEnable();

  protected override void Update()
  {
    base.Update();
    this.SkyAnimator.SetBool("BloodMoon", FollowerBrainStats.IsBloodMoon);
  }

  protected override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    return DoorRoomLocationManager.Instance.DoorPosition.position;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    return DoorRoomLocationManager.Instance.DoorPosition.position;
  }
}
