// Decompiled with JetBrains decompiler
// Type: Interaction_IceHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_IceHeart : Interaction_HeartPickupBase
{
  public float Delay = 1f;
  public string LabelName = "Ice Heart";

  public void Awake()
  {
    if (!PlayerFleeceManager.FleecePreventsHealthPickups())
      return;
    this.gameObject.SetActive(false);
  }

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = ScriptLocalization.Inventory.ICE_HEART;
  }

  public override void Update()
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
    HealthPlayer component = this.playerFarming.GetComponent<HealthPlayer>();
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "black", "burst_big");
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "black", "BloodImpact_Large_0");
    component.IceHearts += (float) (2 * TrinketManager.GetHealthAmountMultiplier(this.playerFarming));
    HUD_Manager.Instance.Show(0);
    Object.Destroy((Object) this.gameObject);
  }
}
