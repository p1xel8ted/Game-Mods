// Decompiled with JetBrains decompiler
// Type: Interaction_BlueHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_BlueHeart : Interaction
{
  public int HP = 2;
  private float Delay = 1f;
  public string LabelName = "Blue Heart";
  public static List<Interaction_BlueHeart> Hearts = new List<Interaction_BlueHeart>();

  private void Start() => this.UpdateLocalisation();

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = ".";
    else
      this.Label = "";
  }

  protected override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    Interaction_BlueHeart.Hearts.Add(this);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    Interaction_BlueHeart.Hearts.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    CameraManager.shakeCamera(1f, (float) Random.Range(0, 360));
    HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    if (this.HP == 2)
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_big");
    else
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_small");
    AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", this.gameObject);
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "blue", "BloodImpact_Large_0");
    component.BlueHearts += (float) (this.HP * TrinketManager.GetHealthAmountMultiplier());
    HUD_Manager.Instance.Show(0);
    Interaction_BlueHeart.Hearts.Remove(this);
    Object.Destroy((Object) this.gameObject);
  }
}
