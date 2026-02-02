// Decompiled with JetBrains decompiler
// Type: Interaction_BlueHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_BlueHeart : Interaction_HeartPickupBase
{
  public int HP = 2;
  public float Delay = 1f;
  public string LabelName = "Blue Heart";
  public static List<Interaction_BlueHeart> Hearts = new List<Interaction_BlueHeart>();

  public void Start() => this.UpdateLocalisation();

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = ".";
    else
      this.Label = "";
  }

  public override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_BlueHeart.Hearts.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_BlueHeart.Hearts.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    CameraManager.shakeCamera(1f, (float) Random.Range(0, 360));
    HealthPlayer component = this.playerFarming.GetComponent<HealthPlayer>();
    if (this.HP == 2)
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_big");
    else
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_small");
    if (this.playerFarming.IsKnockedOut)
      this.DoRevive(this.HP * TrinketManager.GetHealthAmountMultiplier(this.playerFarming), Interaction_HeartPickupBase.HeartPickupType.Blue);
    else
      component.BlueHearts += (float) (this.HP * TrinketManager.GetHealthAmountMultiplier(this.playerFarming));
    AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", this.gameObject);
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "blue", "BloodImpact_Large_0");
    HUD_Manager.Instance.Show(0);
    Interaction_BlueHeart.Hearts.Remove(this);
    Object.Destroy((Object) this.gameObject);
  }
}
