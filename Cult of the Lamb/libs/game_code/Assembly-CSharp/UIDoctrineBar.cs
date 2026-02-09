// Decompiled with JetBrains decompiler
// Type: UIDoctrineBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDoctrineBar : BaseMonoBehaviour
{
  public SermonCategory CurrentSermonCategory;
  public Image BarInstant;
  public Image BarLerp;
  public Image BarFlash;
  public TextMeshProUGUI SermonXPText;
  public RectTransform Container;
  [Header("Increase Counter")]
  [SerializeField]
  public RectTransform _increaseCounterContainer;
  [SerializeField]
  public TMP_Text _increaseCounter;

  public float DoctrineXPTarget
  {
    get => DoctrineUpgradeSystem.GetXPTargetBySermon(this.CurrentSermonCategory);
  }

  public void Awake() => this._increaseCounterContainer.gameObject.SetActive(false);

  public IEnumerator Show(float xp, SermonCategory CurrentSermonCategory)
  {
    this.CurrentSermonCategory = CurrentSermonCategory;
    float Progress = 0.0f;
    float Duration = 0.5f;
    this.SermonXPText.text = $"{Mathf.RoundToInt(xp * 10f).ToString()}/{Mathf.RoundToInt(this.DoctrineXPTarget * 10f).ToString()}";
    this.BarInstant.transform.localScale = new Vector3(Mathf.Clamp(xp / this.DoctrineXPTarget, 0.0f, 1f), 1f);
    this.BarLerp.transform.localScale = new Vector3(Mathf.Clamp(xp / this.DoctrineXPTarget, 0.0f, 1f), 1f);
    this.BarFlash.enabled = false;
    if ((double) this.Container.anchoredPosition.y >= 150.0)
    {
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        this.Container.anchoredPosition = (Vector2) Vector3.Lerp(new Vector3(0.0f, -150f), new Vector3(0.0f, 150f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
        yield return (object) null;
      }
    }
    this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, 150f);
    yield return (object) new WaitForSeconds(0.5f);
  }

  public IEnumerator Hide()
  {
    if ((double) this.Container.anchoredPosition.y != -150.0)
    {
      float Progress = 0.0f;
      float Duration = 0.5f;
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        this.Container.anchoredPosition = (Vector2) Vector3.Lerp(new Vector3(0.0f, 150f), new Vector3(0.0f, -150f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
        yield return (object) null;
      }
      this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, -150f);
      this.BarInstant.transform.localScale = Vector3.zero;
      this.BarLerp.transform.localScale = Vector3.zero;
    }
  }

  public IEnumerator UpdateFirstBar(float xp, float duration)
  {
    UIDoctrineBar uiDoctrineBar = this;
    float Progress = 0.0f;
    xp = Mathf.Clamp(xp, 0.0f, uiDoctrineBar.DoctrineXPTarget);
    Vector3 Starting = uiDoctrineBar.BarInstant.transform.localScale;
    Vector3 Target = new Vector3(Mathf.Min(1f, xp / uiDoctrineBar.DoctrineXPTarget), 1f);
    while ((double) (Progress += Time.deltaTime) < (double) duration)
    {
      uiDoctrineBar.BarInstant.transform.localScale = Vector3.Lerp(Starting, Target, Progress / duration);
      uiDoctrineBar.SermonXPText.text = $"{Mathf.RoundToInt(xp * 10f).ToString()}/{Mathf.RoundToInt(uiDoctrineBar.DoctrineXPTarget * 10f).ToString()}";
      yield return (object) null;
    }
    uiDoctrineBar.BarInstant.transform.localScale = new Vector3(Mathf.Min(1f, xp / uiDoctrineBar.DoctrineXPTarget), 1f);
    uiDoctrineBar.StartCoroutine((IEnumerator) uiDoctrineBar.ShakeRoutine());
  }

  public IEnumerator UpdateSecondBar(float xp, float duration)
  {
    float progress = 0.0f;
    xp = Mathf.Clamp(xp, 0.0f, this.DoctrineXPTarget);
    float StartingFloat = this.BarLerp.transform.localScale.x;
    while ((double) (progress += Time.deltaTime) < (double) duration)
    {
      this.BarLerp.transform.localScale = new Vector3(Mathf.SmoothStep(StartingFloat, xp / this.DoctrineXPTarget, progress / duration), 1f);
      yield return (object) null;
    }
  }

  public IEnumerator ShakeRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    this.Container.DOKill();
    Vector3 strength = new Vector3(20f, 0.0f, 0.0f);
    this.Container.anchoredPosition = (Vector2) new Vector3(0.0f, 150f);
    this.Container.DOShakePosition(0.25f, strength);
  }

  public IEnumerator FlashBarRoutine(float Delay, float Duration)
  {
    UIDoctrineBar uiDoctrineBar = this;
    uiDoctrineBar.BarFlash.enabled = true;
    uiDoctrineBar.BarFlash.color = Color.white;
    Color FadeColor = new Color(1f, 1f, 1f, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/select_upgrade", uiDoctrineBar.gameObject);
    yield return (object) new WaitForSeconds(Delay);
    CameraManager.shakeCamera(1f);
    RumbleManager.Instance.Rumble();
    uiDoctrineBar.BarFlash.color = Color.white;
    yield return (object) new WaitForSeconds(0.5f);
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      uiDoctrineBar.BarFlash.color = Color.Lerp(Color.white, FadeColor, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    uiDoctrineBar.BarFlash.enabled = false;
  }

  public void SetIncreaseCounter(int count)
  {
    this._increaseCounterContainer.gameObject.SetActive(true);
    this._increaseCounter.text = "+" + (object) count;
    this._increaseCounterContainer.DOKill();
    this._increaseCounterContainer.localScale = Vector3.one;
    if (count <= 0)
      this._increaseCounterContainer.DOScale((Vector3) Vector2.zero, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f);
    else
      this._increaseCounterContainer.DOPunchScale(Vector3.one * 0.15f, 0.2f);
  }
}
