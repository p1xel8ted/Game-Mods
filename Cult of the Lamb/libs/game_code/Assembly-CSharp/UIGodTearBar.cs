// Decompiled with JetBrains decompiler
// Type: UIGodTearBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIGodTearBar : BaseMonoBehaviour
{
  public Image BarInstant;
  public Image BarLerp;
  public Image BarFlash;
  public TextMeshProUGUI SermonXPText;
  public RectTransform Container;
  public bool Shown;
  public float yPosition = 55f;

  public float XPTarget => (float) DataManager.Instance.CurrentChallengeModeTargetXP;

  public void Start()
  {
    this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, this.yPosition);
  }

  public void Show(float xp)
  {
    if (this.Shown)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ShowRoutine(xp));
  }

  public void Hide()
  {
    if (!this.Shown)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.HideRoutine());
  }

  public IEnumerator ShowRoutine(float xp)
  {
    this.Shown = true;
    float Progress = 0.0f;
    float Duration = 0.5f;
    TextMeshProUGUI sermonXpText = this.SermonXPText;
    int num = Mathf.RoundToInt(xp);
    string str1 = num.ToString();
    string iconByType = FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT);
    num = Mathf.RoundToInt(this.XPTarget);
    string str2 = num.ToString();
    string str3 = $"{str1}{iconByType}/{str2}";
    sermonXpText.text = str3;
    this.BarInstant.transform.localScale = new Vector3(Mathf.Clamp(xp / this.XPTarget, 0.0f, 1f), 1f);
    this.BarLerp.transform.localScale = new Vector3(Mathf.Clamp(xp / this.XPTarget, 0.0f, 1f), 1f);
    this.BarFlash.enabled = false;
    Vector2 Position = this.Container.anchoredPosition;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.anchoredPosition = (Vector2) Vector3.Lerp((Vector3) Position, new Vector3(0.0f, this.yPosition), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, this.yPosition);
  }

  public IEnumerator HideRoutine()
  {
    if ((double) this.Container.anchoredPosition.y != -150.0)
    {
      this.Shown = false;
      float Progress = 0.0f;
      float Duration = 0.5f;
      Vector2 Position = this.Container.anchoredPosition;
      while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
      {
        this.Container.anchoredPosition = (Vector2) Vector3.Lerp((Vector3) Position, new Vector3(0.0f, -150f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
        yield return (object) null;
      }
      this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, -150f);
      this.BarInstant.transform.localScale = Vector3.zero;
      this.BarLerp.transform.localScale = Vector3.zero;
    }
  }

  public IEnumerator UpdateFirstBar(float xp, float duration)
  {
    UIGodTearBar uiGodTearBar = this;
    float Progress = 0.0f;
    xp = Mathf.Clamp(xp, 0.0f, uiGodTearBar.XPTarget);
    Vector3 Starting = uiGodTearBar.BarInstant.transform.localScale;
    Vector3 Target = new Vector3(Mathf.Min(1f, xp / uiGodTearBar.XPTarget), 1f);
    while ((double) (Progress += Time.deltaTime) < (double) duration)
    {
      uiGodTearBar.BarInstant.transform.localScale = Vector3.Lerp(Starting, Target, Progress / duration);
      uiGodTearBar.SermonXPText.text = $"{Mathf.RoundToInt(xp * 10f).ToString()}/{Mathf.RoundToInt(uiGodTearBar.XPTarget * 10f).ToString()}";
      yield return (object) null;
    }
    uiGodTearBar.BarInstant.transform.localScale = new Vector3(Mathf.Min(1f, xp / uiGodTearBar.XPTarget), 1f);
    uiGodTearBar.StartCoroutine((IEnumerator) uiGodTearBar.ShakeRoutine());
  }

  public IEnumerator UpdateSecondBar(float xp, float duration)
  {
    float progress = 0.0f;
    xp = Mathf.Clamp(xp, 0.0f, this.XPTarget);
    float StartingFloat = this.BarLerp.transform.localScale.x;
    while ((double) (progress += Time.deltaTime) < (double) duration)
    {
      this.BarLerp.transform.localScale = new Vector3(Mathf.SmoothStep(StartingFloat, xp / this.XPTarget, progress / duration), 1f);
      yield return (object) null;
    }
  }

  public IEnumerator ShakeRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    this.Container.DOKill();
    Vector3 strength = new Vector3(20f, 0.0f, 0.0f);
    this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, this.yPosition);
    this.Container.DOShakePosition(0.25f, strength);
  }

  public IEnumerator FlashBarRoutine(float Delay, float Duration)
  {
    UIGodTearBar uiGodTearBar = this;
    uiGodTearBar.BarFlash.enabled = true;
    uiGodTearBar.BarFlash.color = Color.white;
    Color FadeColor = new Color(1f, 1f, 1f, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/select_upgrade", uiGodTearBar.gameObject);
    yield return (object) new WaitForSeconds(Delay);
    CameraManager.shakeCamera(1f);
    RumbleManager.Instance.Rumble();
    uiGodTearBar.BarFlash.color = Color.white;
    yield return (object) new WaitForSeconds(0.5f);
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      uiGodTearBar.BarFlash.color = Color.Lerp(Color.white, FadeColor, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    uiGodTearBar.BarFlash.enabled = false;
  }
}
