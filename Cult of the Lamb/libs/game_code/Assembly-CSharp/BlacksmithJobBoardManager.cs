// Decompiled with JetBrains decompiler
// Type: BlacksmithJobBoardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
public class BlacksmithJobBoardManager : JobBoardManager
{
  public override IEnumerator GiveJobReward(ObjectivesData objective)
  {
    BlacksmithJobBoardManager blacksmithJobBoardManager = this;
    yield return (object) blacksmithJobBoardManager.\u003C\u003En__0(objective);
    Objectives_LegendaryWeaponRun legendaryWeaponRun = objective as Objectives_LegendaryWeaponRun;
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/LegendaryWeaponUpgrade", EquipmentManager.GetWeaponData(legendaryWeaponRun.LegendaryWeapon).GetLocalisedTitle());
    DataManager.Instance.LegendaryWeaponsJobBoardCompleted.Add(legendaryWeaponRun.LegendaryWeapon);
    GameManager.GetInstance().OnConversationEnd();
    if (blacksmithJobBoardManager._jobBoard.BoardCompleted)
      yield return (object) blacksmithJobBoardManager.StartCoroutine((IEnumerator) blacksmithJobBoardManager.HideBoard());
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0(ObjectivesData objective) => base.GiveJobReward(objective);
}
