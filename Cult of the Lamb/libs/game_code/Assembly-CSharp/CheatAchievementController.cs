// Decompiled with JetBrains decompiler
// Type: CheatAchievementController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
