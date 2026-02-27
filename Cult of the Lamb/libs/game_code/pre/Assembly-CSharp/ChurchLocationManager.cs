// Decompiled with JetBrains decompiler
// Type: ChurchLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class ChurchLocationManager : LocationManager
{
  public static ChurchLocationManager Instance;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override Transform UnitLayer => this.transform;

  public override Transform StructureLayer => this.transform;

  protected override void Awake()
  {
    ChurchLocationManager.Instance = this;
    base.Awake();
  }

  protected override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    return ChurchFollowerManager.Instance.DoorPosition.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.0f);
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    return ChurchFollowerManager.Instance.DoorPosition.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.0f);
  }
}
