// Decompiled with JetBrains decompiler
// Type: DormantEnemyChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DormantEnemyChecker : MonoBehaviour
{
  public static DormantEnemyChecker Instance;
  [HideInInspector]
  public float nextCheck;

  public void OnEnable()
  {
    if ((Object) DormantEnemyChecker.Instance == (Object) null)
      DormantEnemyChecker.Instance = this;
    this.nextCheck = Time.time + 1f;
  }

  public void OnDisable()
  {
    if (!((Object) DormantEnemyChecker.Instance == (Object) this))
      return;
    DormantEnemyChecker.Instance = (DormantEnemyChecker) null;
  }

  public void LateUpdate()
  {
    if (!((Object) DormantEnemyChecker.Instance == (Object) this) || RoomLockController.DoorsOpen || (double) Time.time <= (double) this.nextCheck)
      return;
    this.nextCheck = Time.time + 0.5f;
    bool flag1 = true;
    bool flag2 = false;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.Team2)
      {
        flag2 = true;
        if (!allUnit.Dormant)
          flag1 = false;
      }
    }
    if (!(flag1 & flag2))
      return;
    RoomLockController.OpenAll();
    if (!((Object) HUD_Manager.Instance != (Object) null))
      return;
    HUD_Manager.Instance.ShowTopRight();
  }
}
