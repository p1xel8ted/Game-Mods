// Decompiled with JetBrains decompiler
// Type: PlayerSleepBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerSleepBar : MonoBehaviour
{
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public Image whitePulse;
  [SerializeField]
  public BarController barController;
  public float previousValue;
  public float pulseTimestamp;
  public DG.Tweening.Sequence sequence;
  public DG.Tweening.Sequence PulseSequence;

  public void Start()
  {
    this.previousValue = DataManager.Instance.SurvivalMode_Sleep;
    this.barController.SetBarSize(DataManager.Instance.SurvivalMode_Sleep / 100f, true);
    this.container.gameObject.SetActive(DataManager.Instance.SurvivalSleepOnboarded);
  }

  public void Update()
  {
    if ((double) Mathf.Abs(this.previousValue - DataManager.Instance.SurvivalMode_Sleep) > 1.0)
    {
      this.previousValue = DataManager.Instance.SurvivalMode_Sleep;
      this.barController.SetBarSize(DataManager.Instance.SurvivalMode_Sleep / 100f, true);
    }
    if ((double) Time.time > (double) this.pulseTimestamp && (double) DataManager.Instance.SurvivalMode_Sleep < 25.0)
    {
      this.pulseTimestamp = Time.time + 5f;
      if (this.sequence != null)
        return;
      this.sequence = DOTween.Sequence();
      this.barController.transform.localScale = Vector3.one * 1.25f;
      this.sequence.Append((Tween) this.barController.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f));
      this.sequence.AppendInterval(2f);
      this.sequence.SetLoops<DG.Tweening.Sequence>(-1);
      this.sequence.Play<DG.Tweening.Sequence>();
      this.PulseSequence = DOTween.Sequence();
      this.PulseSequence.AppendCallback((TweenCallback) (() =>
      {
        this.whitePulse.transform.localScale = Vector3.one;
        this.whitePulse.transform.DOScale(Vector3.one * 1.25f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
        this.whitePulse.color = new Color(1f, 1f, 1f, 0.8f);
        DOTweenModuleUI.DOFade(this.whitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
      }));
      this.PulseSequence.AppendInterval(2.5f);
      this.PulseSequence.SetLoops<DG.Tweening.Sequence>(-1);
      this.PulseSequence.Play<DG.Tweening.Sequence>();
    }
    else
    {
      if (this.sequence == null)
        return;
      this.sequence.Kill();
      this.sequence = (DG.Tweening.Sequence) null;
      this.PulseSequence.Kill();
      this.PulseSequence = (DG.Tweening.Sequence) null;
    }
  }

  public IEnumerator RevealRoutine()
  {
    PlayerSleepBar playerSleepBar = this;
    while (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden)
      yield return (object) null;
    while ((double) Time.timeScale < 1.0)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
    Vector3 LocalPosition = playerSleepBar.container.transform.localPosition;
    playerSleepBar.container.transform.parent = HUD_Manager.Instance.transform;
    playerSleepBar.container.transform.localPosition = Vector3.zero;
    playerSleepBar.container.transform.parent = playerSleepBar.transform;
    DataManager.Instance.ShowCultFaith = true;
    playerSleepBar.canvasGroup.alpha = 0.0f;
    playerSleepBar.canvasGroup.DOFade(1f, 1f);
    playerSleepBar.container.SetActive(true);
    playerSleepBar.container.transform.localScale = Vector3.one * 3f;
    playerSleepBar.container.transform.DOScale(Vector3.one * 1.25f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.4f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.6f);
    playerSleepBar.container.transform.DOLocalMove(LocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.8f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    playerSleepBar.container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    DataManager.Instance.SurvivalSleepOnboarded = true;
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__9_0()
  {
    this.whitePulse.transform.localScale = Vector3.one;
    this.whitePulse.transform.DOScale(Vector3.one * 1.25f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.whitePulse.color = new Color(1f, 1f, 1f, 0.8f);
    DOTweenModuleUI.DOFade(this.whitePulse, 0.0f, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine);
  }
}
