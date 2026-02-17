// Decompiled with JetBrains decompiler
// Type: UIFollowerXPIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFollowerXPIcon : BaseMonoBehaviour
{
  public float DisplayXP;
  public TextMeshProUGUI Name;
  public TextMeshProUGUI Level;
  public TextMeshProUGUI ReadyForPromotion;
  public Image XPProgressBar;
  public SkeletonGraphic Spine;
  [HideInInspector]
  public FollowerBrain Brain;
  public CanvasGroup canvasGroup;
  public Coroutine cUpdateXPRoutine;

  public void OnLevelUp()
  {
    if (this.cUpdateXPRoutine != null)
      this.StopCoroutine(this.cUpdateXPRoutine);
    this.StartCoroutine((IEnumerator) this.FlashColour());
    this.canvasGroup.alpha = 1f;
    this.UpdateBar();
  }

  public void OnDestroy()
  {
    if (this.Brain == null || this.Brain.Info == null)
      return;
    this.Brain.Info.OnReadyToPromote -= new System.Action(this.OnLevelUp);
  }

  public void Play(FollowerBrain Brain)
  {
    this.Brain = Brain;
    this.Brain.Info.OnReadyToPromote += new System.Action(this.OnLevelUp);
    this.Name.text = Brain.Info.Name;
    this.DisplayXP = Brain.Info.CacheXP;
    Debug.Log((object) ("DisplayXP " + this.DisplayXP.ToString()));
    this.UpdateBar();
    this.Spine.Skeleton.SetSkin(Brain.Info.SkinName);
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(Brain.Info.SkinName);
    if (colourData != null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(Brain.Info.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = this.Spine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    this.canvasGroup.alpha = 0.0f;
  }

  public void UpdateXP(float Delay)
  {
    if (this.cUpdateXPRoutine != null)
      this.StopCoroutine(this.cUpdateXPRoutine);
    this.cUpdateXPRoutine = this.StartCoroutine((IEnumerator) this.UpdateXPRoutine(Delay));
  }

  public IEnumerator UpdateXPRoutine(float Delay)
  {
    UIFollowerXPIcon uiFollowerXpIcon = this;
    yield return (object) new WaitForSecondsRealtime(Delay);
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      uiFollowerXpIcon.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    uiFollowerXpIcon.UpdateBar();
    yield return (object) new WaitForSecondsRealtime(0.2f);
    uiFollowerXpIcon.StartCoroutine((IEnumerator) uiFollowerXpIcon.FlashColour());
  }

  public IEnumerator FlashColour()
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    Color ProgressBarColor = this.XPProgressBar.color;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.XPProgressBar.color = Color.Lerp(Color.white, ProgressBarColor, Mathf.SmoothStep(0.5f, 1f, Progress / Duration));
      yield return (object) null;
    }
  }

  public void Skip()
  {
    if (this.cUpdateXPRoutine != null)
      this.StopCoroutine(this.cUpdateXPRoutine);
    this.UpdateBar();
  }

  public void UpdateBar()
  {
    float x = 1f;
    this.XPProgressBar.transform.localScale = new Vector3(x, this.XPProgressBar.transform.localScale.y);
    this.Level.text = "Level " + this.Brain.Info.XPLevel.ToNumeral();
    this.ReadyForPromotion.enabled = (double) x >= 1.0;
  }
}
