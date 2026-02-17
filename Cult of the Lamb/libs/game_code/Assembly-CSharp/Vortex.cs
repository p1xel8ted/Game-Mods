// Decompiled with JetBrains decompiler
// Type: Vortex
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Vortex : BaseMonoBehaviour
{
  [SerializeField]
  public float force = 0.2f;
  [SerializeField]
  public CircleCollider2D collider;
  [SerializeField]
  public GameObject distortionObject;
  public List<UnitObject> enteredEnemies = new List<UnitObject>();
  public float lifeTime = 5f;
  public float timer;
  [CompilerGenerated]
  public float \u003CLifeTimeMultiplier\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CForceMultiplier\u003Ek__BackingField = 1f;
  public EventInstance LoopInstance;
  public bool createdLoop;

  public float LifeTimeMultiplier
  {
    get => this.\u003CLifeTimeMultiplier\u003Ek__BackingField;
    set => this.\u003CLifeTimeMultiplier\u003Ek__BackingField = value;
  }

  public float ForceMultiplier
  {
    get => this.\u003CForceMultiplier\u003Ek__BackingField;
    set => this.\u003CForceMultiplier\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    AudioManager.Instance.PlayOneShot(" event:/player/Curses/vortex_start", this.gameObject);
    if (!this.createdLoop)
    {
      this.LoopInstance = AudioManager.Instance.CreateLoop("event:/player/Curses/vortex_loop", this.gameObject, true);
      this.createdLoop = true;
    }
    this.lifeTime *= this.LifeTimeMultiplier;
    ParticleSystem[] componentsInChildren = this.GetComponentsInChildren<ParticleSystem>();
    foreach (ParticleSystem particleSystem in componentsInChildren)
    {
      particleSystem.Stop();
      ParticleSystem.MainModule main = particleSystem.main;
      main.duration *= this.LifeTimeMultiplier;
      ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
      startLifetime.constant *= this.LifeTimeMultiplier;
      main.startLifetime = startLifetime;
    }
    foreach (ParticleSystem particleSystem in componentsInChildren)
      particleSystem.Play();
  }

  public void OnDestroy() => AudioManager.Instance.StopLoop(this.LoopInstance);

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.LoopInstance);
    Object.Destroy((Object) this.gameObject);
  }

  public void Update()
  {
    foreach (UnitObject enteredEnemy in this.enteredEnemies)
    {
      if (!((Object) enteredEnemy == (Object) null))
      {
        float num = Vector3.Distance(this.transform.position, enteredEnemy.transform.position) / this.collider.radius;
        Vector3 normalized = (this.transform.position - enteredEnemy.transform.position).normalized;
        enteredEnemy.DisableForces = true;
        enteredEnemy.rb.velocity = (Vector2) (normalized * this.force * num * this.ForceMultiplier);
      }
    }
    if ((double) (this.timer += Time.deltaTime) <= (double) this.lifeTime)
      return;
    AudioManager.Instance.StopLoop(this.LoopInstance);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/vortex_end", this.gameObject);
    foreach (UnitObject enteredEnemy in this.enteredEnemies)
      enteredEnemy.DisableForces = false;
    Object.Destroy((Object) this.gameObject);
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    UnitObject component = collision.GetComponent<UnitObject>();
    if (!(bool) (Object) component || component.health.team != Health.Team.Team2 || this.enteredEnemies.Contains(component))
      return;
    this.enteredEnemies.Add(component);
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    UnitObject component = collision.GetComponent<UnitObject>();
    if (!(bool) (Object) component || component.health.team != Health.Team.Team2 || !this.enteredEnemies.Contains(component))
      return;
    component.DisableForces = false;
    this.enteredEnemies.Remove(component);
  }
}
