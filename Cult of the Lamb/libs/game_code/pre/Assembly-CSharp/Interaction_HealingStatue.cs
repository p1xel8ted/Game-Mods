// Decompiled with JetBrains decompiler
// Type: Interaction_HealingStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_HealingStatue : Interaction
{
  private string sLabel;
  private bool Activated;
  public HealingWaterFade HealingWaterFade;
  private HealthPlayer _healthPlayer;

  private HealthPlayer healthPlayer
  {
    get
    {
      if ((Object) this._healthPlayer == (Object) null)
        this.healthPlayer = PlayerFarming.Instance.GetComponent<HealthPlayer>();
      return this._healthPlayer;
    }
    set => this._healthPlayer = value;
  }

  private void Start()
  {
    this.UpdateLocalisation();
    this.Activated = false;
  }

  public override void GetLabel()
  {
    if ((double) this.healthPlayer.HP >= (double) this.healthPlayer.totalHP && (double) this.healthPlayer.SpiritHearts >= (double) this.healthPlayer.TotalSpiritHearts)
      this.Interactable = false;
    else
      this.Interactable = true;
    this.sLabel = this.Interactable ? ScriptLocalization.Interactions.HealingStatue : ScriptLocalization.Interactions.Fullhealth;
    this.Label = this.sLabel;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = this.Interactable ? ScriptLocalization.Interactions.HealingStatue : ScriptLocalization.Interactions.Fullhealth;
  }

  public override void OnInteract(StateMachine state)
  {
    if ((double) this.healthPlayer.HP >= (double) this.healthPlayer.totalHP && (double) this.healthPlayer.SpiritHearts >= (double) this.healthPlayer.TotalSpiritHearts)
      return;
    base.OnInteract(state);
    this.healthPlayer.HP = this.healthPlayer.totalHP;
    this.healthPlayer.SpiritHearts = this.healthPlayer.TotalSpiritHearts;
    this.HealingWaterFade?.CrossFade();
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.healthPlayer.transform.position, 0.0f, "red", "burst_big");
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
    this.Activated = true;
    this.Interactable = false;
    this.HasChanged = true;
  }
}
