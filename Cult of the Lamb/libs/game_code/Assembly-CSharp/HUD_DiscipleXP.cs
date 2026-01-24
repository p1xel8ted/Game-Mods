// Decompiled with JetBrains decompiler
// Type: HUD_DiscipleXP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_DiscipleXP : MonoBehaviour
{
  public Image InstantBar;
  public Image LerpBar;
  public Image FlashBar;
  public Coroutine cLerpBarRoutine;
  public Coroutine cFlashBarRoutine;

  public float XP => DataManager.Instance.DiscipleXP;

  public float TargetXP
  {
    get
    {
      return DataManager.TargetDiscipleXP[Mathf.Min(DataManager.Instance.DiscipleLevel, DataManager.TargetDiscipleXP.Count - 1)];
    }
  }

  public void OnEnable()
  {
    PlayerFarming.OnGetDiscipleXP += new System.Action(this.OnGetXP);
    RectTransform rectTransform1 = this.InstantBar.rectTransform;
    RectTransform rectTransform2 = this.LerpBar.rectTransform;
    Vector3 vector3_1 = new Vector3(this.XP / this.TargetXP, 1f);
    Vector3 vector3_2 = vector3_1;
    rectTransform2.localScale = vector3_2;
    Vector3 vector3_3 = vector3_1;
    rectTransform1.localScale = vector3_3;
    this.FlashBar.enabled = false;
  }

  public void OnDisable() => PlayerFarming.OnGetDiscipleXP -= new System.Action(this.OnGetXP);

  public void OnGetXP()
  {
    if ((double) this.XP >= (double) this.TargetXP)
    {
      this.LerpBar.rectTransform.localScale = this.InstantBar.rectTransform.localScale = Vector3.zero;
      if (this.cFlashBarRoutine != null)
        this.StopCoroutine(this.cFlashBarRoutine);
      this.StartCoroutine((IEnumerator) this.FlashBarRoutine());
    }
    else
    {
      this.InstantBar.rectTransform.localScale = new Vector3(this.XP / this.TargetXP, 1f);
      if (this.cLerpBarRoutine != null)
        this.StopCoroutine(this.cLerpBarRoutine);
      if ((double) this.InstantBar.rectTransform.localScale.x > (double) this.LerpBar.rectTransform.localScale.x)
        this.cLerpBarRoutine = this.StartCoroutine((IEnumerator) this.LerpBarRoutine());
      else
        this.LerpBar.rectTransform.localScale = this.InstantBar.rectTransform.localScale;
    }
  }

  public IEnumerator LerpBarRoutine()
  {
    yield return (object) new WaitForSecondsRealtime(0.2f);
    Vector3 StartPosition = this.LerpBar.rectTransform.localScale;
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.LerpBar.rectTransform.localScale = Vector3.Lerp(StartPosition, this.InstantBar.rectTransform.localScale, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.LerpBar.rectTransform.localScale = this.InstantBar.rectTransform.localScale;
  }

  public IEnumerator FlashBarRoutine()
  {
    this.FlashBar.enabled = true;
    this.FlashBar.color = Color.white;
    Color FadeColor = new Color(1f, 1f, 1f, 0.0f);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    float Progress = 0.0f;
    float Duration = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.FlashBar.color = Color.Lerp(Color.white, FadeColor, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.FlashBar.enabled = false;
  }
}
