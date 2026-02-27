// Decompiled with JetBrains decompiler
// Type: Interaction_RedHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_RedHeart : Interaction
{
  public int HP = 2;
  private float Delay = 1f;
  public string LabelName = "Heart";
  private PickUp p;
  public static List<Interaction_RedHeart> Hearts = new List<Interaction_RedHeart>();

  private void Start()
  {
    this.UpdateLocalisation();
    this.p = this.GetComponent<PickUp>();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    if (this.HP == 2)
      this.LabelName = ScriptLocalization.Inventory.RED_HEART;
    else
      this.LabelName = ScriptLocalization.Inventory.HALF_HEART;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    Interaction_RedHeart.Hearts.Remove(this);
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    Interaction_RedHeart.Hearts.Add(this);
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    if ((double) PlayerFarming.Instance.health.HP + (double) PlayerFarming.Instance.health.SpiritHearts < (double) PlayerFarming.Instance.health.totalHP + (double) PlayerFarming.Instance.health.TotalSpiritHearts)
    {
      this.AutomaticallyInteract = true;
      this.Interactable = true;
    }
    else
    {
      if ((Object) this.p != (Object) null)
        this.p.MagnetToPlayer = false;
      this.Interactable = false;
      this.AutomaticallyInteract = false;
      this.Label = ScriptLocalization.Interactions.Fullhealth;
    }
  }

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
    {
      if ((double) PlayerFarming.Instance.health.HP + (double) PlayerFarming.Instance.health.SpiritHearts < (double) PlayerFarming.Instance.health.totalHP + (double) PlayerFarming.Instance.health.TotalSpiritHearts)
      {
        this.AutomaticallyInteract = true;
        this.Interactable = true;
        this.Label = ".";
      }
      else
      {
        this.p.MagnetToPlayer = false;
        this.Interactable = false;
        this.AutomaticallyInteract = false;
        this.Label = ScriptLocalization.Interactions.Fullhealth;
      }
    }
    else
      this.Label = "";
  }

  protected override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void OnInteract(StateMachine state)
  {
    HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    base.OnInteract(state);
    CameraManager.shakeCamera(1f, (float) Random.Range(0, 360));
    if (this.HP == 2)
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
    else
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_small");
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "red", "BloodImpact_Large_0");
    AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.gameObject);
    double healing = (double) (this.HP * TrinketManager.GetHealthAmountMultiplier());
    component.Heal((float) healing);
    Object.Destroy((Object) this.gameObject);
  }
}
