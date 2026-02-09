// Decompiled with JetBrains decompiler
// Type: TrapSmashableBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TrapSmashableBlock : MonoBehaviour
{
  public Health health;

  public void Awake() => this.health = this.GetComponent<Health>();

  public void OnCollisionEnter2D(Collision2D col)
  {
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(col.gameObject);
    if (!col.collider.gameObject.CompareTag("Player") || !TrinketManager.HasTrinket(TarotCards.Card.WalkThroughBlocks, farmingComponent) && (double) farmingComponent.playerRelic.PlayerScaleModifier <= 1.0)
      return;
    this.health.DealDamage(this.health.HP, col.collider.gameObject, this.transform.position);
  }
}
