// Decompiled with JetBrains decompiler
// Type: Interaction_CoopRevive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_CoopRevive : Interaction
{
  public string labelString = "Interactions/CoopRevive";
  public int reviveCost = 2;
  public int currentReviveCost;

  public override void GetLabel()
  {
    this.label = LocalizationManager.GetTranslation(this.labelString);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.UpdateInteraction(playerFarming);
  }

  public void UpdateInteraction(PlayerFarming playerFarming)
  {
    if ((double) playerFarming.health.CurrentHP > (double) this.reviveCost)
    {
      this.labelString = "Interactions/CoopRevive";
      this.Interactable = true;
    }
    else
    {
      this.labelString = "Interactions/CoopReviveCannotAfford";
      this.Interactable = false;
    }
    this.GetLabel();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.currentReviveCost = this.reviveCost;
    float currentHp1 = this.playerFarming.health.CurrentHP;
    PlayerFarming component = this.gameObject.GetComponent<PlayerFarming>();
    float currentHp2 = component.health.CurrentHP;
    if ((component.IsKnockedOut || (double) currentHp2 <= 0.0) && (double) currentHp1 > (double) this.reviveCost)
    {
      for (; (double) this.playerFarming.health.SpiritHearts > 0.0 && this.currentReviveCost > 0; --this.currentReviveCost)
        --this.playerFarming.health.SpiritHearts;
      for (; (double) this.playerFarming.health.BlackHearts > 0.0 && this.currentReviveCost > 0; --this.currentReviveCost)
        --this.playerFarming.health.BlackHearts;
      for (; (double) this.playerFarming.health.FireHearts > 0.0 && this.currentReviveCost > 0; --this.currentReviveCost)
        --this.playerFarming.health.FireHearts;
      for (; (double) this.playerFarming.health.IceHearts > 0.0 && this.currentReviveCost > 0; --this.currentReviveCost)
        --this.playerFarming.health.IceHearts;
      for (; (double) this.playerFarming.health.BlueHearts > 0.0 && this.currentReviveCost > 0; --this.currentReviveCost)
        --this.playerFarming.health.BlueHearts;
      for (; (double) this.playerFarming.health.HP > 0.0 && this.currentReviveCost > 0; --this.currentReviveCost)
        --this.playerFarming.health.HP;
      CoopManager.WakeKnockedOutPlayer(component, (float) this.reviveCost, this.playerFarming, pauseTime: true);
      this.enabled = false;
      this.currentReviveCost = this.reviveCost;
    }
    else
    {
      Debug.Log((object) "Something has gone wrong with this revive, not enough health?");
      CameraManager.instance.ShakeCameraForDuration(1f, 3f, 1f);
      this.playerFarming.indicator.PlayShake();
    }
  }
}
