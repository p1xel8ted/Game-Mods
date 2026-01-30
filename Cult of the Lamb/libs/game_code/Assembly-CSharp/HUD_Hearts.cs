// Decompiled with JetBrains decompiler
// Type: HUD_Hearts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUD_Hearts : BaseMonoBehaviour
{
  public List<HUD_Heart> HeartIcons = new List<HUD_Heart>();
  public HUD_ActiveTrinkets activeTrinkets;
  public HUD_ActiveTrinketsCoOp activeTrinketsCoOp;
  public FaithAmmo faithAmmo;
  public GameObject faithAmmoContainer;
  public CurrentWeapon currentWeapon;
  public CurrentCurse currentCurse;
  public CurrentRelic currentRelic;
  public CoopIndicatorIcon coopIndicatorIcon;
  [SerializeField]
  public GameObject heartsSpacer;
  [SerializeField]
  public GameObject swordCurseContainer;
  public int _health;
  public PlayerFarming playerFarming;
  public bool isLamb;
  public EventInstance loopedSound;
  public bool createdloop;
  public int count;

  public void CheckCoOp()
  {
    if (PlayerFarming.players.Count != 1)
      return;
    this.coopIndicatorIcon.gameObject.SetActive(false);
  }

  public void InitDungeon(PlayerFarming playerFarmingVar)
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) playerFarmingVar)
        this.ClearHealthEvents();
      if ((UnityEngine.Object) this.playerFarming.hudHearts != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming.hudHearts != (UnityEngine.Object) this)
        this.playerFarming.hudHearts.ClearHealthEvents();
    }
    this.playerFarming = playerFarmingVar;
    this.isLamb = this.playerFarming.isLamb;
    this.playerFarming.hudHearts = this;
    if (!(bool) (UnityEngine.Object) this.playerFarming)
      return;
    this.currentRelic.gameObject.SetActive(true);
    this.faithAmmo.gameObject.SetActive(true);
    this.currentWeapon.gameObject.SetActive(true);
    if (PlayerFarming.players.Count > 1)
    {
      if ((UnityEngine.Object) this.activeTrinketsCoOp != (UnityEngine.Object) null)
        this.activeTrinketsCoOp.gameObject.SetActive(true);
    }
    else
      this.activeTrinkets.gameObject.SetActive(true);
    this.CheckCoOp();
    HealthPlayer health = this.playerFarming.health;
    if ((bool) (UnityEngine.Object) health)
    {
      this.ClearHealthEvents();
      health.OnTotalHPUpdated += new HealthPlayer.HPUpdated(this.OnTotalHPUpdated);
      health.OnHPUpdated += new HealthPlayer.HPUpdated(this.OnHPUpdated);
      if (PlayerFarming.players.Count > 1)
        this.activeTrinketsCoOp?.Init(this.playerFarming);
      else
        this.activeTrinkets.Init(this.playerFarming);
      this.faithAmmo.Init(this.playerFarming);
      this.OnHPUpdated(health);
    }
    this.currentCurse.Init(this.playerFarming);
    this.currentWeapon.Init(this.playerFarming);
    this.currentRelic.Init(this.playerFarming);
    if ((UnityEngine.Object) this.swordCurseContainer != (UnityEngine.Object) null)
    {
      if (this.playerFarming.currentCurse == EquipmentType.None && this.playerFarming.currentWeapon == EquipmentType.None)
        this.swordCurseContainer.SetActive(false);
      PlayerSpells.OnCurseChanged += new PlayerSpells.CurseEvent(this.SetCurse);
      PlayerWeapon.OnWeaponChanged += new PlayerWeapon.WeaponEvent(this.SetWeapon);
    }
    if (this.playerFarming.isLamb)
      this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
    else
      this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Goat);
  }

  public void SetWeapon(EquipmentType weapon, int level, PlayerFarming playerfarming)
  {
    if ((UnityEngine.Object) playerfarming != (UnityEngine.Object) this.playerFarming)
      return;
    this.swordCurseContainer.SetActive(true);
    PlayerWeapon.OnWeaponChanged -= new PlayerWeapon.WeaponEvent(this.SetWeapon);
  }

  public void SetCurse(EquipmentType curse, int level, PlayerFarming playerfarming)
  {
    if ((UnityEngine.Object) playerfarming != (UnityEngine.Object) this.playerFarming)
      return;
    this.swordCurseContainer.SetActive(true);
    PlayerSpells.OnCurseChanged -= new PlayerSpells.CurseEvent(this.SetCurse);
  }

  public void InitBase(PlayerFarming playerFarmingVar)
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) playerFarmingVar)
        this.ClearHealthEvents();
      if ((UnityEngine.Object) this.playerFarming.hudHearts != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming.hudHearts != (UnityEngine.Object) this)
        this.playerFarming.hudHearts.ClearHealthEvents();
    }
    this.playerFarming = playerFarmingVar;
    this.playerFarming.hudHearts = this;
    this.isLamb = this.playerFarming.isLamb;
    if ((bool) (UnityEngine.Object) this.playerFarming)
    {
      this.CheckCoOp();
      HealthPlayer health = this.playerFarming.health;
      if ((bool) (UnityEngine.Object) health)
      {
        this.ClearHealthEvents();
        health.OnTotalHPUpdated += new HealthPlayer.HPUpdated(this.OnTotalHPUpdated);
        health.OnHPUpdated += new HealthPlayer.HPUpdated(this.OnHPUpdated);
        this.playerFarming.playerRelic.OnRelicEquipped -= new PlayerRelic.RelicEvent(this.PlayerRelicOnOnRelicEquipped);
        this.playerFarming.playerRelic.OnRelicConsumed -= new PlayerRelic.RelicEvent(this.PlayerRelicOnOnRelicConsumed);
        this.playerFarming.playerRelic.OnRelicEquipped += new PlayerRelic.RelicEvent(this.PlayerRelicOnOnRelicEquipped);
        this.playerFarming.playerRelic.OnRelicConsumed += new PlayerRelic.RelicEvent(this.PlayerRelicOnOnRelicConsumed);
        this.OnHPUpdated(health);
      }
      if ((UnityEngine.Object) this.heartsSpacer != (UnityEngine.Object) null)
        this.heartsSpacer.gameObject.SetActive(false);
      this.currentRelic.gameObject.SetActive(false);
      this.faithAmmo.gameObject.SetActive(false);
      this.currentWeapon.gameObject.SetActive(false);
      this.activeTrinkets.gameObject.SetActive(false);
    }
    if (!((UnityEngine.Object) this.coopIndicatorIcon != (UnityEngine.Object) null))
      return;
    if (this.playerFarming.isLamb)
      this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
    else
      this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Goat);
    this.coopIndicatorIcon.SetUsername(this.playerFarming.isLamb ? 0 : 1);
  }

  public void PlayerRelicOnOnRelicConsumed(RelicData relic, PlayerFarming target)
  {
    if (!((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) target))
      return;
    this.currentRelic.transition.hideBar();
  }

  public void PlayerRelicOnOnRelicEquipped(RelicData relic, PlayerFarming target)
  {
    if (!((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) target))
      return;
    this.currentRelic.transition.MoveBackInFunction();
    this.currentRelic.Init(target);
  }

  public void Awake()
  {
    if ((UnityEngine.Object) this.currentRelic != (UnityEngine.Object) null)
      this.currentRelic.gameObject.SetActive(false);
    this.faithAmmo.gameObject.SetActive(false);
    this.currentWeapon.gameObject.SetActive(false);
    this.activeTrinkets.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    if (!(bool) (UnityEngine.Object) this.playerFarming)
      return;
    this.ClearHealthEvents();
    this.playerFarming.playerRelic.OnRelicEquipped -= new PlayerRelic.RelicEvent(this.PlayerRelicOnOnRelicEquipped);
    this.playerFarming.playerRelic.OnRelicConsumed -= new PlayerRelic.RelicEvent(this.PlayerRelicOnOnRelicConsumed);
  }

  public void OnEnable()
  {
    if (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null))
      return;
    HealthPlayer health = this.playerFarming.health;
    if (!(bool) (UnityEngine.Object) health)
      return;
    this.ClearHealthEvents();
    health.OnTotalHPUpdated += new HealthPlayer.HPUpdated(this.OnTotalHPUpdated);
    health.OnHPUpdated += new HealthPlayer.HPUpdated(this.OnHPUpdated);
  }

  public void OnDisable()
  {
    this.ClearHealthEvents();
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public void OnHPUpdated(HealthPlayer Target)
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming.gameObject == (UnityEngine.Object) null || (UnityEngine.Object) Target != (UnityEngine.Object) this.playerFarming.health)
      return;
    this.UpdateHearts(this.playerFarming.health, true);
    this.lowHealthCheck();
  }

  public void lowHealthCheck()
  {
    int num1 = (int) this.playerFarming.health.PLAYER_HEALTH + (int) this.playerFarming.health.PLAYER_SPIRIT_HEARTS + (int) this.playerFarming.health.PLAYER_BLUE_HEARTS + (int) this.playerFarming.health.PLAYER_BLACK_HEARTS + (int) this.playerFarming.health.PLAYER_FIRE_HEARTS + (int) this.playerFarming.health.PLAYER_ICE_HEARTS;
    this._health = num1;
    bool flag = DataManager.Instance.PlayerFleece == 7;
    if (num1 == 2 && (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH != 2.0 && !flag && !HUD_Manager.Instance._playerDamageNoti.Active)
    {
      this.StopAllCoroutines();
      if (!this.createdloop)
      {
        this.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/misc/whispers", PlayerFarming.Instance.gameObject, true);
        this.createdloop = true;
      }
      int num2 = (int) this.loopedSound.setVolume(0.5f);
      this.StartCoroutine((IEnumerator) this.lowHealthEffect(2f, this.HeartIcons[0]));
    }
    else if (num1 == 1 && !flag && !HUD_Manager.Instance._playerDamageNoti.Active)
    {
      this.StopAllCoroutines();
      if (!this.createdloop)
      {
        if ((double) Time.timeScale != 0.0)
          AudioManager.Instance.SetMusicFilter("filter", 0.666f);
        this.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/misc/whispers", PlayerFarming.Instance.gameObject, true);
        this.createdloop = true;
      }
      int num3 = (int) this.loopedSound.setVolume(1f);
      this.StartCoroutine((IEnumerator) this.lowHealthEffect(1f, this.HeartIcons[0]));
    }
    else
    {
      if (this.createdloop && (double) Time.timeScale != 0.0)
        AudioManager.Instance.SetMusicFilter("filter", 0.0f);
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.createdloop = false;
      this.StopAllCoroutines();
    }
  }

  public IEnumerator lowHealthEffect(float waitTime, HUD_Heart Heart)
  {
    this.count = 4;
    while ((int) this.playerFarming.health.PLAYER_HEALTH + (int) this.playerFarming.health.PLAYER_SPIRIT_HEARTS + (int) this.playerFarming.health.PLAYER_BLUE_HEARTS + (int) this.playerFarming.health.PLAYER_BLACK_HEARTS + (int) this.playerFarming.health.PLAYER_FIRE_HEARTS + (int) this.playerFarming.health.PLAYER_ICE_HEARTS <= 2)
    {
      yield return (object) new WaitForSeconds(waitTime);
      --this.count;
      if (this.count >= 0)
        UIManager.PlayAudio("event:/ui/heartbeat");
      Heart.transform.DOKill();
      Heart.transform.localScale = Vector3.one;
      Heart.transform.DOPunchScale(new Vector3(0.2f, -0.5f), 0.5f);
    }
  }

  public void UpdateHearts(HealthPlayer health, bool DoEffects)
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming.gameObject == (UnityEngine.Object) null)
      return;
    int index = -1;
    int hp = (int) health.HP;
    int playerTotalHealth = (int) this.playerFarming.health.PLAYER_TOTAL_HEALTH;
    int spiritHearts = (int) health.SpiritHearts;
    int totalSpiritHearts = (int) health.TotalSpiritHearts;
    int blueHearts = (int) health.BlueHearts;
    int blackHearts = (int) health.BlackHearts;
    int fireHearts = (int) health.FireHearts;
    int iceHearts = (int) health.IceHearts;
    while (++index < this.HeartIcons.Count)
    {
      HUD_Heart heartIcon = this.HeartIcons[index];
      if (!((UnityEngine.Object) heartIcon.gameObject == (UnityEngine.Object) null))
      {
        if ((double) Mathf.Ceil(this.playerFarming.health.PLAYER_TOTAL_HEALTH / 2f) + (double) Mathf.Ceil(health.TotalSpiritHearts / 2f) + (double) Mathf.Ceil(this.playerFarming.health.PLAYER_BLUE_HEARTS / 2f) + (double) Mathf.Ceil(this.playerFarming.health.PLAYER_BLACK_HEARTS / 2f) + (double) Mathf.Ceil(this.playerFarming.health.PLAYER_FIRE_HEARTS / 2f) + (double) Mathf.Ceil(this.playerFarming.health.PLAYER_ICE_HEARTS / 2f) <= (double) index)
        {
          if (heartIcon.MyState == HUD_Heart.HeartState.HeartHalf && heartIcon.MyHeartType == HUD_Heart.HeartType.Blue)
            heartIcon.Activate(false, true);
          else
            heartIcon.Activate(false, false);
        }
        else
        {
          heartIcon.Activate(true, false);
          if (playerTotalHealth >= 1)
          {
            if (hp >= 2)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects);
              hp -= 2;
            }
            else if (hp == 1)
            {
              if (playerTotalHealth >= 2)
                heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalfFull, DoEffects);
              else
                heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects);
              --hp;
            }
            else if (playerTotalHealth == 1)
              heartIcon.SetSprite(HUD_Heart.HeartState.HalfHeartContainer, DoEffects);
            else
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartContainer, DoEffects);
            playerTotalHealth -= 2;
          }
          else if (totalSpiritHearts >= 1)
          {
            if (spiritHearts >= 2)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Spirit);
              spiritHearts -= 2;
            }
            else if (spiritHearts == 1)
            {
              if (totalSpiritHearts == 1)
                heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Spirit);
              else
                heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalfFull, DoEffects, HUD_Heart.HeartType.Spirit);
              --spiritHearts;
            }
            else if (totalSpiritHearts == 1)
              heartIcon.SetSprite(HUD_Heart.HeartState.HalfHeartContainer, DoEffects, HUD_Heart.HeartType.Spirit);
            else
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartContainer, DoEffects, HUD_Heart.HeartType.Spirit);
            totalSpiritHearts -= 2;
          }
          else if ((double) index >= (double) Mathf.Ceil(this.playerFarming.health.PLAYER_TOTAL_HEALTH / 2f) + (double) Mathf.Ceil(health.TotalSpiritHearts / 2f))
          {
            if (blueHearts >= 2)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Blue);
              blueHearts -= 2;
            }
            else if (blueHearts == 1)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Blue);
              --blueHearts;
            }
            else if (blackHearts >= 2)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Black);
              blackHearts -= 2;
            }
            else if (blackHearts == 1)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Black);
              --blackHearts;
            }
            else if (fireHearts >= 2)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Fire);
              fireHearts -= 2;
            }
            else if (fireHearts == 1)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Fire);
              --fireHearts;
            }
            else if (iceHearts >= 2)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Ice);
              iceHearts -= 2;
            }
            else if (iceHearts == 1)
            {
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Ice);
              --iceHearts;
            }
          }
        }
      }
    }
  }

  public void OnTotalHPUpdated(HealthPlayer Target)
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming.gameObject == (UnityEngine.Object) null || (UnityEngine.Object) Target != (UnityEngine.Object) this.playerFarming.health)
      return;
    double hp = (double) Target.HP;
    float blueHearts = Target.BlueHearts;
    float spiritHearts = Target.SpiritHearts;
    double num = (double) blueHearts;
    float a = (float) (hp + num) + spiritHearts;
    float Delay = 0.0f;
    for (int index = 0; index < this.HeartIcons.Count; ++index)
    {
      if ((double) index < Math.Ceiling((double) a))
      {
        HUD_Heart heartIcon = this.HeartIcons[index];
        if (!heartIcon.gameObject.activeSelf)
        {
          heartIcon.ActivateAndScale(Delay);
          Delay += 0.5f;
          heartIcon.SetSprite(HUD_Heart.HeartState.HeartContainer);
        }
      }
    }
    this.UpdateHearts(this.playerFarming.health, false);
  }

  public Vector3 GetNextPosition()
  {
    int index = -1;
    while (++index < this.HeartIcons.Count)
    {
      if (!this.HeartIcons[index].gameObject.activeSelf)
        return this.HeartIcons[index - 1].rectTransform.position + Vector3.right * (this.HeartIcons[index - 1].rectTransform.position.x / (float) index);
    }
    return Vector3.zero;
  }

  public void StopLoopedSound() => AudioManager.Instance.StopLoop(this.loopedSound);

  public void LowHealthCheckUpdate() => this.lowHealthCheck();

  public void ClearHealthEvents()
  {
    if (!(bool) (UnityEngine.Object) this.playerFarming)
      return;
    this.playerFarming.health.OnTotalHPUpdated -= new HealthPlayer.HPUpdated(this.OnTotalHPUpdated);
    this.playerFarming.health.OnHPUpdated -= new HealthPlayer.HPUpdated(this.OnHPUpdated);
  }
}
