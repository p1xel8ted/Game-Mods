// Decompiled with JetBrains decompiler
// Type: HUD_Time
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Time : BaseMonoBehaviour
{
  private Vector3 StartPos;
  private Vector3 MovePos;
  public TextMeshProUGUI DayLabel;
  public Transform Clockhand;
  [Header("Red Overlay")]
  [SerializeField]
  private Image _redOverlay;
  [SerializeField]
  private Color _nightTimeRed;
  [SerializeField]
  private Color _defaultRed;
  [SerializeField]
  private CanvasGroup _speedUpTime;
  private bool timescaleChanged;

  private void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.Init);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.DayLabel.text = $"{TimeManager.CurrentDay}";
    if (SaveAndLoad.Loaded)
      this.Init();
    this._speedUpTime.alpha = 0.0f;
  }

  private void Init() => this.OnNewDay();

  private void Update()
  {
    this.Clockhand.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(0.0f, -360f, TimeManager.CurrentDayProgress));
    if ((double) Time.timeScale > 1.0)
    {
      if (this.timescaleChanged)
        return;
      this._speedUpTime.DOFade(1f, 0.5f);
      this.timescaleChanged = true;
    }
    else
    {
      if (!this.timescaleChanged)
        return;
      this._speedUpTime.DOFade(0.0f, 0.5f);
      this.timescaleChanged = false;
    }
  }

  private void OnNewDay()
  {
    this.DayLabel.text = $"{TimeManager.CurrentDay}";
    this.DayLabel.transform.DOKill();
    this.DayLabel.transform.DOShakeScale(0.75f, 0.5f);
  }

  private void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.Init);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted()
  {
    this._redOverlay.DOKill();
    if (TimeManager.CurrentPhase == DayPhase.Night)
      DOTweenModuleUI.DOColor(this._redOverlay, this._nightTimeRed, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    else
      DOTweenModuleUI.DOColor(this._redOverlay, this._defaultRed, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }
}
