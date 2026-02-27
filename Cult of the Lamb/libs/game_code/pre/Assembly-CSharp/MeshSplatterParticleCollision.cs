// Decompiled with JetBrains decompiler
// Type: MeshSplatterParticleCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MeshSplatterParticleCollision : BaseMonoBehaviour
{
  private ParticleSystem particles;
  private GameObject Player;
  private PlayerController playerController;
  private int numEnter;
  private List<ParticleSystem.Particle> enter;

  private void Start()
  {
    this.particles = this.GetComponent<ParticleSystem>();
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  private IEnumerator WaitForPlayer()
  {
    while ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      yield return (object) null;
    this.playerController = this.Player.GetComponent<PlayerController>();
    this.particles.trigger.SetCollider(0, (Component) this.Player.GetComponent<CircleCollider2D>());
  }

  private void OnParticleTrigger()
  {
    if ((Object) this.playerController == (Object) null || !this.enabled)
      return;
    this.enter = new List<ParticleSystem.Particle>();
    this.numEnter = this.particles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, (List<ParticleSystem.Particle>) this.enter);
    if (this.numEnter <= 0)
      return;
    this.playerController.SetFootSteps((Color) this.enter[0].GetCurrentColor(this.particles));
  }
}
