// Decompiled with JetBrains decompiler
// Type: Interaction_RedHeart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_RedHeart : Interaction_HeartPickupBase
{
  public int HP = 2;
  public float Delay = 1f;
  public string LabelName = "Heart";
  public PickUp p;
  public static List<Interaction_RedHeart> Hearts = new List<Interaction_RedHeart>();

  public override bool CanMultiplePlayersInteract
  {
    get
    {
      List<PlayerFarming> players = PlayerFarming.players;
      int num = 0;
      foreach (Component component1 in players)
      {
        HealthPlayer component2 = component1.GetComponent<HealthPlayer>();
        if ((Object) component2 != (Object) null && this.IsPlayerFullHealth(component2))
          ++num;
      }
      return num != players.Count;
    }
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.p = this.GetComponent<PickUp>();
    this.Label = ".";
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    if (this.HP == 2)
      this.LabelName = ScriptLocalization.Inventory.RED_HEART;
    else
      this.LabelName = ScriptLocalization.Inventory.HALF_HEART;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_RedHeart.Hearts.Remove(this);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_RedHeart.Hearts.Add(this);
    this.AutomaticallyInteract = true;
    this.Interactable = true;
  }

  public override void GetLabel()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((Object) player.interactor.TempInteraction != (Object) this))
      {
        HealthPlayer component = player.GetComponent<HealthPlayer>();
        if ((Object) component != (Object) null && !this.IsPlayerFullHealth(component))
        {
          this.AutomaticallyInteract = true;
          this.Interactable = true;
          this.Label = ".";
        }
        else
        {
          this.AutomaticallyInteract = false;
          this.Interactable = false;
          this.Label = ScriptLocalization.Interactions.Fullhealth;
        }
      }
    }
  }

  public override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    HealthPlayer component = state.GetComponent<HealthPlayer>();
    if (!((Object) component != (Object) null) || this.IsPlayerFullHealth(component))
      return;
    base.OnInteract(state);
    CameraManager.shakeCamera(1f, (float) Random.Range(0, 360));
    if (this.HP == 2)
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
    else
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_small");
    BiomeConstants.Instance.EmitBloodImpact(this.transform.position, 0.0f, "red", "BloodImpact_Large_0");
    AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.gameObject);
    if (this.playerFarming.IsKnockedOut)
      this.DoRevive(this.HP * TrinketManager.GetHealthAmountMultiplier(this.playerFarming), Interaction_HeartPickupBase.HeartPickupType.Red);
    else
      component.Heal((float) (this.HP * TrinketManager.GetHealthAmountMultiplier(this.playerFarming)));
    Object.Destroy((Object) this.gameObject);
  }

  public bool IsPlayerFullHealth(HealthPlayer playerHealth)
  {
    return (double) playerHealth.HP + (double) playerHealth.SpiritHearts >= (double) playerHealth.totalHP + (double) playerHealth.TotalSpiritHearts;
  }
}
