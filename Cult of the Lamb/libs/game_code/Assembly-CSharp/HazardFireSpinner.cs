// Decompiled with JetBrains decompiler
// Type: HazardFireSpinner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class HazardFireSpinner : MonoBehaviour
{
  public Vector3 spinSpeed = new Vector3(0.0f, 0.0f, 60f);
  public Vector3 initialRotation = new Vector3(0.0f, 0.0f, 0.0f);
  public Transform flameRotatorTransform;
  public Transform[] flames;
  public float playerDamage = 1f;
  public float enemyDamage = 1f;
  public GameObject TrapOn;
  public GameObject TrapOff;
  public bool destroyed;

  public void Start()
  {
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.ChestAppeared);
    this.flameRotatorTransform.Rotate(this.initialRotation);
    for (int index = 0; index < this.flames.Length; ++index)
      this.flames[index].transform.Rotate(-this.initialRotation);
  }

  public void ChestAppeared()
  {
    this.destroyed = true;
    for (int index = 0; index < this.flames.Length; ++index)
    {
      Transform flame = this.flames[index];
      flame.transform.DOScale(0.0f, 1f);
      ParticleSystem componentInChildren = flame.GetComponentInChildren<ParticleSystem>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.Stop();
      Object.Destroy((Object) flame.gameObject, 1f);
    }
    this.TrapOn.SetActive(false);
    this.TrapOff.SetActive(true);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.ChestAppeared);
  }

  public void OnDestroy()
  {
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.ChestAppeared);
  }

  public void FixedUpdate()
  {
    if (this.destroyed || PlayerRelic.TimeFrozen)
      return;
    this.flameRotatorTransform.Rotate(this.spinSpeed * Time.deltaTime);
    for (int index = 0; index < this.flames.Length; ++index)
      this.flames[index].transform.Rotate(-this.spinSpeed * Time.deltaTime);
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    UnitObject component = collision.GetComponent<UnitObject>();
    if (!((Object) component != (Object) null) || !((Object) component.health != (Object) null))
      return;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collision.gameObject);
    if (component.health.team == Health.Team.PlayerTeam && !TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent))
    {
      component.health.DealDamage(this.playerDamage, this.gameObject, this.transform.position);
    }
    else
    {
      if (component.isFlyingEnemy)
        return;
      component.health.DealDamage(this.playerDamage, this.gameObject, this.transform.position);
    }
  }
}
