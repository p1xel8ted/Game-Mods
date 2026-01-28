// Decompiled with JetBrains decompiler
// Type: ChurchLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class ChurchLocationManager : LocationManager
{
  public static ChurchLocationManager Instance;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override Transform UnitLayer => this.transform;

  public override Transform StructureLayer => this.transform;

  public override void Awake()
  {
    ChurchLocationManager.Instance = this;
    UIItemSelectorOverlayController.CloseAllOverLays();
    base.Awake();
  }

  public override void OnEnable()
  {
    UIItemSelectorOverlayController.CloseAllOverLays();
    base.OnEnable();
  }

  public override void OnDisable()
  {
    UIItemSelectorOverlayController.CloseAllOverLays();
    base.OnDisable();
  }

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    return ChurchFollowerManager.Instance.DoorPosition.position;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    return ChurchFollowerManager.Instance.DoorPosition.position;
  }
}
