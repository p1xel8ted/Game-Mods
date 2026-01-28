// Decompiled with JetBrains decompiler
// Type: UIAbilityUnlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIAbilityUnlock : BaseMonoBehaviour
{
  public static UIAbilityUnlock Instance;
  public CanvasGroup canvasGroup;
  public List<Image> MainImage;
  public TextMeshProUGUI Title;
  public TextMeshProUGUI Description;
  public TextMeshProUGUI Explanation;
  public TextMeshProUGUI CrownAbilityUnlocked;
  public MMControlPrompt ControlPrompt;
  public List<UIAbilityUnlock.AbilityInfo> Abilities = new List<UIAbilityUnlock.AbilityInfo>();

  public static void Play(UIAbilityUnlock.Ability Ability)
  {
    UIAbilityUnlock.Instance = (UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/UI/UI Ability Unlock"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIAbilityUnlock>();
    UIAbilityUnlock.Instance.Show(Ability);
  }

  public void Show(UIAbilityUnlock.Ability Ability)
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.Hide(false, 0);
    UIAbilityUnlock.AbilityInfo abilityInfo = this.GetAbilityInfo(Ability);
    foreach (Image image in this.MainImage)
    {
      if ((UnityEngine.Object) abilityInfo.Sprite == (UnityEngine.Object) null)
      {
        image.enabled = false;
      }
      else
      {
        image.enabled = true;
        image.sprite = abilityInfo.Sprite;
        image.rectTransform.eulerAngles = abilityInfo.ImageRotation;
      }
    }
    this.Title.text = LocalizationManager.Sources[0].GetTranslation(abilityInfo.Title);
    this.Description.text = LocalizationManager.Sources[0].GetTranslation(abilityInfo.Description);
    this.Explanation.text = LocalizationManager.Sources[0].GetTranslation(abilityInfo.Explanation);
    this.CrownAbilityUnlocked.enabled = abilityInfo.ShowCrownAbilityUnlockedText;
    if (abilityInfo.ShowControlPrompt)
    {
      this.ControlPrompt.gameObject.SetActive(true);
      this.ControlPrompt.Action = abilityInfo.Action;
      this.ControlPrompt.ForceUpdate();
    }
    else
      this.ControlPrompt.gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.ScreenRoutine());
  }

  public IEnumerator ScreenRoutine()
  {
    UIAbilityUnlock uiAbilityUnlock = this;
    Time.timeScale = 0.0f;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      uiAbilityUnlock.canvasGroup.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    while (!InputManager.UI.GetAcceptButtonUp() && !InputManager.UI.GetCancelButtonUp())
      yield return (object) null;
    Time.timeScale = 1f;
    Progress = 0.0f;
    Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      uiAbilityUnlock.canvasGroup.alpha = Mathf.Lerp(1f, 0.0f, Progress / Duration);
      yield return (object) null;
    }
    PlayerFarming.SetStateForAllPlayers();
    HUD_Manager.Instance.Show();
    UnityEngine.Object.Destroy((UnityEngine.Object) uiAbilityUnlock.gameObject);
  }

  public UIAbilityUnlock.AbilityInfo GetAbilityInfo(UIAbilityUnlock.Ability Ability)
  {
    foreach (UIAbilityUnlock.AbilityInfo ability in this.Abilities)
    {
      if (ability.AbilityType == Ability)
        return ability;
    }
    return (UIAbilityUnlock.AbilityInfo) null;
  }

  public enum Ability
  {
    GrappleHook,
    CultLevel1,
    CultLevel2,
    MenticideMushroom,
    FishingRod,
    Arrows,
    LevelUpFollower,
    HeavyAttack,
    PyreResurrect,
    Abilities_Heart_I,
    Heal,
    UpgradeHeal,
    Resurrection,
    Eat,
    TeleportHome,
  }

  [Serializable]
  public class AbilityInfo
  {
    public UIAbilityUnlock.Ability AbilityType;
    [TermsPopup("")]
    public string Title;
    [TermsPopup("")]
    public string Description;
    [TermsPopup("")]
    public string Explanation;
    public Sprite Sprite;
    public bool ShowControlPrompt;
    [ActionIdProperty(typeof (RewiredConsts.Action))]
    public int Action = 9;
    public Vector3 ImageRotation = new Vector3(0.0f, 0.0f, -35.26f);
    public bool ShowCrownAbilityUnlockedText = true;

    public void AutoSet()
    {
      this.Description = this.Title.Replace("Title", "Description");
      this.Explanation = this.Title.Replace("Title", "Explanation");
    }
  }
}
