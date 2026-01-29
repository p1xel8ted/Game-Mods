// Decompiled with JetBrains decompiler
// Type: Achievements_LambTownGhosts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Achievements_LambTownGhosts : MonoBehaviour
{
  public void CheckForLambTownGhostAchievements()
  {
    DataManager.Variables[] variablesArray = new DataManager.Variables[6]
    {
      DataManager.Variables.EncounteredRancher,
      DataManager.Variables.EncounteredBlacksmithShop,
      DataManager.Variables.EncounteredDecoShop,
      DataManager.Variables.EncounteredGraveyardShop,
      DataManager.Variables.EncounteredFlockade,
      DataManager.Variables.EncounteredTarotShop
    };
    bool flag = true;
    foreach (DataManager.Variables variable in variablesArray)
      flag = flag && DataManager.Instance.GetVariable(variable);
    if (!flag)
      return;
    Debug.Log((object) "Achievement: All Woolhaven residents returned.");
  }
}
