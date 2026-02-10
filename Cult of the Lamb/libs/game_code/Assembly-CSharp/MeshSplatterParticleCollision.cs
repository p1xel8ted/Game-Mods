// Decompiled with JetBrains decompiler
// Type: MeshSplatterParticleCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MeshSplatterParticleCollision : BaseMonoBehaviour
{
  public ParticleSystem particles;
  public GameObject Player;
  public PlayerController playerController;
  public int numEnter;
  public List<ParticleSystem.Particle> enter;

  public void Start()
  {
    this.particles = this.GetComponent<ParticleSystem>();
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  public IEnumerator WaitForPlayer()
  {
    while ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      yield return (object) null;
    this.playerController = this.Player.GetComponent<PlayerController>();
    this.particles.trigger.SetCollider(0, (Component) this.Player.GetComponent<CircleCollider2D>());
  }

  public void OnParticleTrigger()
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
