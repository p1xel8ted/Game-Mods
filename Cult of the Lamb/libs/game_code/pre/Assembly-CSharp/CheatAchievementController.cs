// Decompiled with JetBrains decompiler
// Type: CheatAchievementController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Unify;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class CheatAchievementController : MonoBehaviour
{
  private int C_AchievementMax;
  private int C_AchievementCurrent;
  private Achievement C_achievement;
  public Text C_achievementLabelTextBox;
  public Text C_achievementTitleTextBox;
  public Button DefaultButton;
  private ForceSelection[] ForceSelections;
  private GameObject previouslySelected;

  private void Start()
  {
    this.C_AchievementMax = Achievements.Instance.GetCount();
    this.C_AchievementCurrent = 0;
    this.C_achievement = Achievements.Instance.Get(this.C_AchievementCurrent);
    this.UpdateTexts();
    if ((Object) EventSystem.current.currentSelectedGameObject != (Object) null)
    {
      Debug.Log((object) ("previouslySelected = " + EventSystem.current.currentSelectedGameObject.name));
      this.previouslySelected = EventSystem.current.currentSelectedGameObject;
    }
    if ((Object) this.previouslySelected != (Object) null)
      this.previouslySelected.transform.parent.gameObject.SetActive(false);
    this.DefaultButton.Select();
  }

  public void NextAchievement()
  {
    ++this.C_AchievementCurrent;
    if (this.C_AchievementCurrent >= this.C_AchievementMax)
      this.C_AchievementCurrent = 0;
    this.UpdateTexts();
  }

  public void PreviousAchievement()
  {
    --this.C_AchievementCurrent;
    if (this.C_AchievementCurrent < 0)
      this.C_AchievementCurrent = this.C_AchievementMax - 1;
    this.UpdateTexts();
  }

  public void UnlockAchievement() => AchievementsWrapper.UnlockAchievement(this.C_achievement);

  public void ResetAchievement() => SteamUserStats.ResetAllStats(true);

  public void Close()
  {
    if ((Object) this.previouslySelected != (Object) null)
    {
      this.previouslySelected.transform.parent.gameObject.SetActive(true);
      this.previouslySelected.GetComponent<Button>().Select();
    }
    Object.Destroy((Object) this.gameObject);
  }

  private void UpdateTexts()
  {
    this.C_achievement = Achievements.Instance.Get(this.C_AchievementCurrent);
    this.C_achievementLabelTextBox.text = $"{(object) (this.C_AchievementCurrent + 1)} / {(object) this.C_AchievementMax}";
    this.C_achievementTitleTextBox.text = this.C_achievement.label;
  }
}
