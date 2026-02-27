// Decompiled with JetBrains decompiler
// Type: Interaction_BlackHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_BlackHeart : Interaction
{
  private float Delay = 1f;
  public string LabelName = "Black Heart";

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = ScriptLocalization.Inventory.BLACK_HEART;
  }

  protected override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = ".";
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    CameraManager.shakeCamera(1f, (float) Random.Range(0, 360));
    HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "black", "burst_big");
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "black", "BloodImpact_Large_0");
    component.BlackHearts += (float) (2 * TrinketManager.GetHealthAmountMultiplier());
    HUD_Manager.Instance.Show(0);
    Object.Destroy((Object) this.gameObject);
  }
}
