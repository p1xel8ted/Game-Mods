// Decompiled with JetBrains decompiler
// Type: Interaction_HealingStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_HealingStatue : Interaction
{
  public string sLabel;
  public bool Activated;
  public HealingWaterFade HealingWaterFade;
  [SerializeField]
  public bool healingFire;
  [SerializeField]
  public bool healingIce;
  public HealthPlayer _healthPlayer;

  public override bool InactiveAfterStopMoving => false;

  public HealthPlayer healthPlayer
  {
    get
    {
      if ((Object) this._healthPlayer == (Object) null)
        this.healthPlayer = this.playerFarming.GetComponent<HealthPlayer>();
      return this._healthPlayer;
    }
    set => this._healthPlayer = value;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.Activated = false;
  }

  public override void GetLabel()
  {
    List<PlayerFarming> players = PlayerFarming.players;
    if (this.Activated)
    {
      this.Interactable = false;
    }
    else
    {
      foreach (PlayerFarming playerFarming in players)
      {
        if (!((Object) playerFarming.interactor.TempInteraction != (Object) this))
        {
          HealthPlayer component = playerFarming.GetComponent<HealthPlayer>();
          if (this.healingFire || this.healingIce)
            this.Interactable = true;
          else if ((Object) this.healthPlayer != (Object) null && ((double) component.HP < (double) component.totalHP || (double) component.SpiritHearts < (double) component.TotalSpiritHearts))
            this.Interactable = true;
          else
            this.Interactable = false;
        }
      }
    }
    this.sLabel = this.Interactable ? ScriptLocalization.Interactions.HealingStatue : ScriptLocalization.Interactions.Recharging;
    this.Label = this.sLabel;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = this.Interactable ? ScriptLocalization.Interactions.HealingStatue : ScriptLocalization.Interactions.Fullhealth;
  }

  public override void OnInteract(StateMachine state)
  {
    HealthPlayer component = state.GetComponent<HealthPlayer>();
    if ((Object) component != (Object) null)
      this.healthPlayer = component;
    if ((double) this.healthPlayer.HP >= (double) this.healthPlayer.totalHP && (double) this.healthPlayer.SpiritHearts >= (double) this.healthPlayer.TotalSpiritHearts && !this.healingIce && !this.healingFire)
      return;
    base.OnInteract(state);
    if (this.healingIce)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/player/collect_ice_heart");
      this.healthPlayer.IceHearts += 2f;
      if (DataManager.Instance.BroughtFishingRod)
        this.healthPlayer.IceHearts += 2f;
    }
    else if (this.healingFire)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/player/collect_fire_heart");
      this.healthPlayer.FireHearts += 2f;
      if (DataManager.Instance.BroughtFishingRod)
        this.healthPlayer.FireHearts += 2f;
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
      this.healthPlayer.HP = this.healthPlayer.totalHP;
      this.healthPlayer.SpiritHearts = this.healthPlayer.TotalSpiritHearts;
    }
    this.HealingWaterFade?.CrossFade();
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.healthPlayer.transform.position, 0.0f, "red", "burst_big");
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success, this.playerFarming);
    this.Activated = true;
    this.Interactable = false;
    this.HasChanged = true;
  }
}
