// Decompiled with JetBrains decompiler
// Type: CurrentRelic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CurrentRelic : MonoBehaviour
{
  public RelicType relic;
  [SerializeField]
  public CanvasGroup unchargedRelicCG;
  [SerializeField]
  public Image unchargedIcon;
  [SerializeField]
  public BarController barController;
  [SerializeField]
  public CanvasGroup chargedRelicCG;
  [SerializeField]
  public MMControlPrompt buttonPrompt;
  [SerializeField]
  public Image icon;
  [SerializeField]
  public Image iconOutline;
  [SerializeField]
  public Image fragileIcon;
  [SerializeField]
  public Image Background;
  [SerializeField]
  public CanvasGroup SubIconCanvasGroup;
  [SerializeField]
  public Image SubIcon;
  [SerializeField]
  public Image SubIconOutline;
  [SerializeField]
  public List<RelicCantUse> _cantUseRelics = new List<RelicCantUse>();
  [SerializeField]
  public Image requiresItemIcon;
  public PlayerFarming playerFarming;
  public UI_Transitions transition;
  public Vector3 storedScale;
  public Vector3 requiresItemIconStartPos;
  public Vector3 iconStartingPos;
  public bool playedChargedSFX;
  public RelicType cacheRelic;

  public void Awake() => this.storedScale = this.transform.localScale;

  public void OnEnable()
  {
  }

  public void Init(PlayerFarming playerFarmingVar)
  {
    this.playerFarming = playerFarmingVar;
    if ((Object) this.playerFarming != (Object) null && (this.playerFarming.currentRelicType == RelicType.UseRandomRelic || this.playerFarming.currentRelicType == RelicType.UseRandomRelic_Blessed || this.playerFarming.currentRelicType == RelicType.UseRandomRelic_Dammed))
      this.SubIconCanvasGroup.gameObject.SetActive(true);
    else
      this.SubIconCanvasGroup.gameObject.SetActive(false);
    if ((Object) this.playerFarming == (Object) null || this.playerFarming.currentRelicType == RelicType.None || !GameManager.IsDungeon(PlayerFarming.Location))
      this.transform.localScale = Vector3.zero;
    else
      this.transform.localScale = this.storedScale;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerRelic.OnRelicEquipped -= new PlayerRelic.RelicEvent(this.SetRelic);
      player.playerRelic.OnRelicChargeModified -= new PlayerRelic.RelicEvent(this.OnRelicChargeModified);
      player.playerRelic.OnRelicConsumed -= new PlayerRelic.RelicEvent(this.OnRelicConsumed);
      player.playerRelic.OnRelicCantUse -= new PlayerRelic.RelicEvent(this.OnRelicCantUse);
      player.playerRelic.OnSubRelicChanged -= new PlayerRelic.RelicEvent(this.SetSubIcon);
    }
    this.playerFarming.playerRelic.OnRelicEquipped += new PlayerRelic.RelicEvent(this.SetRelic);
    this.playerFarming.playerRelic.OnRelicChargeModified += new PlayerRelic.RelicEvent(this.OnRelicChargeModified);
    this.playerFarming.playerRelic.OnRelicConsumed += new PlayerRelic.RelicEvent(this.OnRelicConsumed);
    this.playerFarming.playerRelic.OnRelicCantUse += new PlayerRelic.RelicEvent(this.OnRelicCantUse);
    this.playerFarming.playerRelic.OnSubRelicChanged += new PlayerRelic.RelicEvent(this.SetSubIcon);
    if ((Object) this.playerFarming.playerRelic.CurrentRelic != (Object) null)
      this.SetRelic(this.playerFarming.playerRelic.CurrentRelic, this.playerFarming);
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.UpdateFragileIcon);
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.UpdateFragileIcon);
    this.requiresItemIcon.gameObject.SetActive(false);
    this.requiresItemIconStartPos = this.requiresItemIcon.transform.localPosition;
    this.iconStartingPos = this.icon.transform.localPosition;
    this.Background.enabled = false;
  }

  public void OnRelicCantUse(RelicData relic, PlayerFarming target)
  {
    if ((Object) target != (Object) null && (Object) target != (Object) this.playerFarming)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.playerFarming.gameObject);
    this.icon.transform.DOKill();
    this.iconOutline.transform.DOKill();
    this.icon.DOKill();
    this.icon.transform.localPosition = this.iconStartingPos;
    this.iconOutline.transform.localPosition = this.iconStartingPos;
    this.icon.color = Color.red;
    DOTweenModuleUI.DOColor(this.icon, Color.white, 0.5f);
    this.icon.transform.DOShakePosition(1f, new Vector3(10f, 0.0f, 0.0f));
    this.iconOutline.transform.DOShakePosition(1f, new Vector3(10f, 0.0f, 0.0f));
    bool flag = false;
    foreach (RelicCantUse cantUseRelic in this._cantUseRelics)
    {
      if (cantUseRelic.relic == this.relic)
      {
        this.requiresItemIcon.sprite = cantUseRelic.sprite;
        flag = true;
      }
    }
    if (!flag)
      return;
    this.requiresItemIcon.gameObject.SetActive(true);
    this.requiresItemIcon.transform.DOKill();
    this.requiresItemIcon.transform.localPosition = this.requiresItemIconStartPos;
    this.requiresItemIcon.transform.localScale = Vector3.one;
    this.requiresItemIcon.DOKill();
    this.requiresItemIcon.color = Color.white;
    this.requiresItemIcon.transform.localScale = Vector3.one * 2f;
    this.requiresItemIcon.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.requiresItemIcon.transform.DOPunchPosition(new Vector3(10f, 0.0f, 0.0f), 1f);
    this.requiresItemIcon.transform.DOLocalMove(new Vector3(0.0f, 50f, 0.0f), 1.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad);
    DOTweenModuleUI.DOFade(this.requiresItemIcon, 0.0f, 1f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.requiresItemIcon.gameObject.SetActive(false)));
  }

  public void OnDestroy()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerRelic.OnRelicCantUse -= new PlayerRelic.RelicEvent(this.OnRelicCantUse);
      player.playerRelic.OnRelicConsumed -= new PlayerRelic.RelicEvent(this.OnRelicConsumed);
      player.playerRelic.OnSubRelicChanged -= new PlayerRelic.RelicEvent(this.SetSubIcon);
      player.playerRelic.OnRelicEquipped -= new PlayerRelic.RelicEvent(this.SetRelic);
      player.playerRelic.OnRelicChargeModified -= new PlayerRelic.RelicEvent(this.OnRelicChargeModified);
    }
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.UpdateFragileIcon);
  }

  public void OnRelicConsumed(RelicData relic, PlayerFarming target = null)
  {
    if ((Object) target != (Object) null && (Object) target != (Object) this.playerFarming)
      return;
    this.chargedRelicCG.alpha = 1f;
    this.buttonPrompt.gameObject.SetActive(false);
    this.iconOutline.DOKill();
    DOTweenModuleUI.DOFade(this.iconOutline, 0.0f, 0.5f);
    Material target1 = new Material(this.icon.material);
    this.icon.material = target1;
    target1.DOKill();
    target1.DOFloat(1f, "_FillColorLerpFade", 0.2f);
    target1.DOFloat(0.0f, "_FillColorLerpFade", 0.2f).SetDelay<TweenerCore<float, float, FloatOptions>>(1f);
    target1.DOFloat(0.0f, "_DissolveAmount", 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.5f);
    target1.DOFloat(1f, "_DissolveAmount", 0.1f).SetDelay<TweenerCore<float, float, FloatOptions>>(1.5f);
    this.chargedRelicCG.DOKill();
    this.chargedRelicCG.DOFade(0.0f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(1f);
  }

  public void OnRelicChargeModified(RelicData relic, PlayerFarming target = null)
  {
    if ((Object) target != (Object) null && (Object) target != (Object) this.playerFarming)
      return;
    this.barController.gameObject.SetActive(true);
    this.barController.SetBarSize(this.playerFarming.playerRelic.ChargedAmount / this.playerFarming.playerRelic.RequiredChargeAmount, true);
    this.chargedRelicCG.DOKill();
    this.unchargedRelicCG.DOKill();
    if ((double) this.playerFarming.playerRelic.ChargedAmount >= (double) this.playerFarming.playerRelic.RequiredChargeAmount)
    {
      if (!this.playedChargedSFX)
      {
        AudioManager.Instance.PlayOneShot("event:/relics/relic_charged");
        MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
        this.playedChargedSFX = true;
      }
      this.buttonPrompt.gameObject.SetActive(true);
      this.buttonPrompt.playerFarming = this.playerFarming;
      this.buttonPrompt.ForceUpdate();
      this.iconOutline.color = Color.white;
      if ((double) this.chargedRelicCG.alpha != 1.0)
        this.chargedRelicCG.DOFade(1f, 0.5f);
      if ((double) this.unchargedRelicCG.alpha != 0.0)
        this.unchargedRelicCG.DOFade(0.0f, 0.5f);
    }
    else
    {
      this.playedChargedSFX = false;
      if ((double) this.chargedRelicCG.alpha != 0.0)
        this.chargedRelicCG.DOFade(0.0f, 0.5f);
      if ((double) this.unchargedRelicCG.alpha != 1.0)
        this.unchargedRelicCG.DOFade(1f, 0.5f);
    }
    if ((double) this.playerFarming.playerRelic.ChargedAmount > 0.0)
      return;
    this.barController.ShrinkBarToEmpty(0.0f);
  }

  public void SetRelic(RelicData relic, PlayerFarming target = null)
  {
    if ((Object) target != (Object) null && (Object) target != (Object) this.playerFarming)
      return;
    relic = this.playerFarming.playerRelic.CurrentRelic;
    this.SetRelic((Object) relic != (Object) null ? relic.RelicType : RelicType.None, true, this.playerFarming);
  }

  public void SetSubIcon(RelicData relic, PlayerFarming target = null)
  {
    if ((Object) target != (Object) null && (Object) target != (Object) this.playerFarming)
      return;
    relic = this.playerFarming.playerRelic.CurrentRelic;
    this.SetSubIcon((Object) relic != (Object) null ? relic.RelicType : RelicType.None);
  }

  public void SetSubIcon(RelicType relic, PlayerFarming target = null)
  {
    if ((Object) target != (Object) null && (Object) target != (Object) this.playerFarming)
      return;
    Debug.Log((object) $"{nameof (SetSubIcon).Colour(Color.green)}  {relic.ToString()}");
    this.SubIconCanvasGroup.gameObject.SetActive(relic == RelicType.UseRandomRelic || relic == RelicType.UseRandomRelic_Blessed || relic == RelicType.UseRandomRelic_Dammed);
    if (!this.SubIconCanvasGroup.gameObject.activeSelf)
      return;
    this.SubIconCanvasGroup.DOKill();
    this.SubIconCanvasGroup.transform.localScale = Vector3.one * 1.5f;
    this.SubIconCanvasGroup.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.SubIconCanvasGroup.alpha = 0.0f;
    this.SubIconCanvasGroup.DOFade(1f, 0.5f);
    Debug.Log((object) EquipmentManager.NextRandomRelic.ToString().Colour(Color.yellow));
    this.SubIcon.sprite = EquipmentManager.GetRelicIcon(EquipmentManager.NextRandomRelic);
    this.SubIconOutline.sprite = EquipmentManager.GetRelicIconOutline(EquipmentManager.NextRandomRelic);
  }

  public void SetRelic(RelicType relic, bool punch, PlayerFarming playerFarming)
  {
    if ((Object) playerFarming != (Object) null && (Object) playerFarming != (Object) this.playerFarming)
      return;
    Debug.Log((object) nameof (SetRelic).Colour(Color.yellow));
    if (this.cacheRelic == RelicType.None)
    {
      UI_Transitions component = this.GetComponent<UI_Transitions>();
      component.hideBar();
      component.MoveBackInFunction();
    }
    if (relic == RelicType.None)
      return;
    this.relic = relic;
    this.cacheRelic = relic;
    this.fragileIcon.enabled = relic != 0;
    this.iconOutline.enabled = relic != 0;
    this.unchargedIcon.enabled = relic != 0;
    this.icon.enabled = relic != 0;
    this.fragileIcon.enabled = EquipmentManager.GetRelicSingleUse(relic, playerFarming);
    this.chargedRelicCG.alpha = 0.0f;
    this.unchargedRelicCG.alpha = 0.0f;
    if (relic == RelicType.None)
    {
      this.transform.localScale = Vector3.zero;
    }
    else
    {
      this.chargedRelicCG.DOKill();
      if ((double) playerFarming.playerRelic.ChargedAmount >= (double) playerFarming.playerRelic.RequiredChargeAmount)
        this.chargedRelicCG.DOFade(1f, 1f);
      else
        this.unchargedRelicCG.DOFade(1f, 1f);
      this.icon.sprite = EquipmentManager.GetRelicIcon(relic);
      this.iconOutline.sprite = EquipmentManager.GetRelicIconOutline(relic);
      this.unchargedIcon.sprite = this.icon.sprite;
      this.transform.localScale = this.storedScale;
      this.icon.transform.localScale = Vector3.one;
      if (punch)
      {
        this.icon.transform.localScale *= 2f;
        this.icon.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        this.Background.transform.DOKill();
        this.Background.enabled = true;
        this.Background.transform.localScale = Vector3.one * 0.9f;
        this.Background.transform.DOScale(Vector3.one * 2f, 0.5f);
        Color c = this.Background.color;
        DOTweenModuleUI.DOFade(this.Background, 0.0f, 0.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
        {
          this.Background.color = c;
          this.Background.enabled = false;
        }));
      }
      this.OnRelicChargeModified(EquipmentManager.GetRelicData(relic));
    }
  }

  public void UpdateFragileIcon(TarotCards.Card card, PlayerFarming playerFarming)
  {
    if (card != TarotCards.Card.CorruptedBlackHeartForRelic || !((Object) this.fragileIcon != (Object) null))
      return;
    this.fragileIcon.enabled = EquipmentManager.GetRelicSingleUse(this.relic, playerFarming);
  }

  [CompilerGenerated]
  public void \u003COnRelicCantUse\u003Eb__23_0()
  {
    this.requiresItemIcon.gameObject.SetActive(false);
  }
}
