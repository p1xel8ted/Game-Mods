// Decompiled with JetBrains decompiler
// Type: UIPlayerDamageNoti
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIPlayerDamageNoti : MonoBehaviour
{
  [SerializeField]
  public Image radial;
  [SerializeField]
  public GameObject barContainer;
  [SerializeField]
  public GameObject hungerBar;
  [SerializeField]
  public GameObject sleepBar;
  [SerializeField]
  public TextMeshProUGUI textIcon;
  [SerializeField]
  public Image objectOutline;
  public EventInstance _loopedSound;
  public List<SkeletonGraphic> Tallies = new List<SkeletonGraphic>();
  public bool createdLoop;
  [CompilerGenerated]
  public bool \u003CCachedShowHunger\u003Ek__BackingField;
  public bool Active;
  public bool _stoppedFloat;

  public void OnEnable() => TimeManager.OnSurvivalLossTally += new System.Action(this.OnSurvivalLossTally);

  public void OnDisable()
  {
    TimeManager.OnSurvivalLossTally -= new System.Action(this.OnSurvivalLossTally);
    AudioManager.Instance.StopLoop(this._loopedSound);
    this.createdLoop = false;
  }

  public void OnSurvivalLossTally()
  {
    Debug.Log((object) ("OnSurvivalLossTally  " + TimeManager.SurvivalTallies.ToString()));
    this.Tallies[TimeManager.SurvivalTallies].AnimationState.SetAnimation(0, "fail", false);
    this.Tallies[TimeManager.SurvivalTallies].AnimationState.AddAnimation(0, "failed", true, 0.0f);
    this.transform.DOKill();
    this.transform.DOShakePosition(0.5f, new Vector3(25f, 0.0f));
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this._loopedSound);
    this.createdLoop = false;
  }

  public bool CachedShowHunger
  {
    get => this.\u003CCachedShowHunger\u003Ek__BackingField;
    set => this.\u003CCachedShowHunger\u003Ek__BackingField = value;
  }

  public void Configure(bool showHunger)
  {
    Debug.Log((object) $"{">>>>>>>>>>>>>>>>>>>>>>>>>  Configure!!".Colour(Color.red)}   {showHunger.ToString()}");
    this.CachedShowHunger = showHunger;
    this.hungerBar.SetActive(showHunger);
    this.sleepBar.SetActive(!showHunger);
    this.objectOutline.gameObject.SetActive(false);
    this.transform.DOKill();
    this.transform.localScale = Vector3.one * 3f;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    UIManager.PlayAudio("event:/ui/level_node_beat_level");
    foreach (SkeletonGraphic tally in this.Tallies)
    {
      tally.AnimationState.SetAnimation(0, "appear", false).MixDuration = 0.0f;
      tally.AnimationState.AddAnimation(0, "empty", false, 0.0f);
    }
    this.radial.fillAmount = 0.0f;
    if (!this.createdLoop)
    {
      this._loopedSound = AudioManager.Instance.CreateLoop("event:/ui/radial_heartbeat", true);
      AudioManager.Instance.SetEventInstanceParameter(this._loopedSound, "heart_rate", 0.0f);
      this.createdLoop = true;
    }
    this.Active = true;
  }

  public void Update()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      this.radial.fillAmount = TimeManager.SurvivalDamagedTimer / 20f;
      if (this.createdLoop && (double) Time.timeScale == 0.0)
      {
        AudioManager.Instance.StopLoop(this._loopedSound);
        this._stoppedFloat = true;
      }
      else if ((double) Time.timeScale == 0.0 && this._stoppedFloat)
      {
        AudioManager.Instance.PlayLoop(this._loopedSound);
        this._stoppedFloat = false;
      }
      if ((double) TimeManager.SurvivalDamagedTimer == -1.0)
        return;
      float num = this.radial.fillAmount;
      if ((double) num > 1.0)
        num = 1f;
      if (this.createdLoop)
        AudioManager.Instance.SetEventInstanceParameter(this._loopedSound, "heart_rate", num);
      if (TimeManager.SurvivalTallies <= 0)
      {
        if (!this.objectOutline.gameObject.activeSelf)
        {
          this.objectOutline.gameObject.SetActive(true);
          this.objectOutline.DOKill();
          this.objectOutline.color = new Color(this.objectOutline.color.r, this.objectOutline.color.g, this.objectOutline.color.b, 0.0f);
          DOTweenModuleUI.DOFade(this.objectOutline, 1f, 0.25f);
        }
      }
      else
        this.objectOutline.gameObject.SetActive(false);
    }
    else
      AudioManager.Instance.SetEventInstanceParameter(this._loopedSound, "heart_rate", 0.0f);
    if (this.createdLoop)
    {
      if (HUD_Manager.Instance.Hidden || (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.CustomAnimation || MMConversation.isPlaying || GameManager.InMenu)
      {
        int num1 = (int) this._loopedSound.setVolume(0.0f);
      }
      else
      {
        int num2 = (int) this._loopedSound.setVolume(1f);
      }
    }
    this.hungerBar.transform.localScale = Vector3.one * 1.25f * (1f + Mathf.PingPong(Time.time * 0.5f, 0.15f));
    this.sleepBar.transform.localScale = Vector3.one * 1.25f * (1f + Mathf.PingPong(Time.time * 0.5f, 0.15f));
  }
}
