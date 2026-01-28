// Decompiled with JetBrains decompiler
// Type: CheatAchievementController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CheatAchievementController : MonoBehaviour
{
  public int C_AchievementMax;
  public int C_AchievementCurrent;
  public Achievement C_achievement;
  public Text C_achievementLabelTextBox;
  public Text C_achievementTitleTextBox;
  public Button DefaultButton;
  public ForceSelection[] ForceSelections;
  public GameObject previouslySelected;
}
