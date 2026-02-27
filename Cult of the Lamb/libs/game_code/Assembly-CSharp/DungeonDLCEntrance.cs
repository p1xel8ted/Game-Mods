// Decompiled with JetBrains decompiler
// Type: DungeonDLCEntrance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class DungeonDLCEntrance : MonoBehaviour
{
  public IEnumerator Start()
  {
    while ((Object) PlayerFarming.Instance == (Object) null || PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    if (DataManager.Instance.BeatenWolf && !DataManager.Instance.Dungeon5Harder && PlayerFarming.Location == FollowerLocation.Dungeon1_5)
    {
      NotificationCentreScreen.Play(string.Format(LocalizationManager.GetTranslation("Notifications/EnemiesStronger"), (object) $"<color=#FFD201>{ScriptLocalization.NAMES_Places.Dungeon1_5}</color>"));
      DataManager.Instance.Dungeon5Harder = true;
    }
    else if (DataManager.Instance.BeatenYngya && !DataManager.Instance.Dungeon6Harder && PlayerFarming.Location == FollowerLocation.Dungeon1_6)
    {
      NotificationCentreScreen.Play(string.Format(LocalizationManager.GetTranslation("Notifications/EnemiesStronger"), (object) $"<color=#FFD201>{ScriptLocalization.NAMES_Places.Dungeon1_6}</color>"));
      DataManager.Instance.Dungeon6Harder = true;
    }
  }
}
