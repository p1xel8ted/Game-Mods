// Decompiled with JetBrains decompiler
// Type: HealingBath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HealingBath : BaseMonoBehaviour
{
  public float Timer;
  public ParticleSystem particleSystem;
  public HealthPlayer playerHealth;

  public void Start() => this.particleSystem.Stop();

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    this.Timer = 0.0f;
    this.particleSystem.Play();
    this.playerHealth = collision.gameObject.GetComponent<HealthPlayer>();
  }

  public void OnTriggerStay2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    if ((double) this.playerHealth.PLAYER_HEALTH < (double) this.playerHealth.PLAYER_TOTAL_HEALTH)
    {
      if ((double) (this.Timer += Time.deltaTime) <= 2.0)
        return;
      this.Timer = 0.0f;
      this.playerHealth.Heal(1f);
    }
    else
      this.particleSystem.Stop();
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    this.particleSystem.Stop();
  }
}
