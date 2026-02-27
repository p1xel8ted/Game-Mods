// Decompiled with JetBrains decompiler
// Type: HealingBath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HealingBath : BaseMonoBehaviour
{
  private float Timer;
  public ParticleSystem particleSystem;
  private HealthPlayer playerHealth;

  private void Start() => this.particleSystem.Stop();

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player"))
      return;
    this.Timer = 0.0f;
    this.particleSystem.Play();
    this.playerHealth = collision.gameObject.GetComponent<HealthPlayer>();
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player"))
      return;
    if ((double) DataManager.Instance.PLAYER_HEALTH < (double) DataManager.Instance.PLAYER_TOTAL_HEALTH)
    {
      if ((double) (this.Timer += Time.deltaTime) <= 2.0)
        return;
      this.Timer = 0.0f;
      this.playerHealth.Heal(1f);
    }
    else
      this.particleSystem.Stop();
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player"))
      return;
    this.particleSystem.Stop();
  }
}
