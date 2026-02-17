// Decompiled with JetBrains decompiler
// Type: BreakableDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (BoxCollider2D))]
public class BreakableDoor : BaseMonoBehaviour
{
  public BoxCollider2D _collider;
  public float CameraShake = 2f;
  public int maxParticles = 20;
  public float zSpawn = 0.5f;
  public List<GameObject> gameObjectsToDisable;
  public ParticleSystem particles;
  public Vector2 Velocity;
  public Animator doorAnimation;

  public void Start()
  {
    if (!((Object) this._collider == (Object) null))
      return;
    this._collider = this.gameObject.GetComponent<BoxCollider2D>();
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log((object) "Collision");
    if (!collision.gameObject.CompareTag("Player"))
      return;
    this.Velocity = collision.relativeVelocity;
    this._collider.enabled = false;
    this._collider.isTrigger = true;
    this.BreakDoor();
  }

  public void BreakDoor()
  {
    if ((Object) this.doorAnimation != (Object) null)
      this.doorAnimation.SetTrigger("Trigger");
    for (int index = 0; index < this.gameObjectsToDisable.Count; ++index)
      this.gameObjectsToDisable[index].SetActive(false);
    CameraManager.shakeCamera(this.CameraShake, Utils.GetAngle(this.transform.position, this.transform.position));
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * this.zSpawn);
    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
    emitParams.velocity = new Vector3(this.Velocity.x / 4f, this.Velocity.y / 4f, -2f);
    this.particles = Object.Instantiate<ParticleSystem>(this.particles, this.transform.position, Quaternion.identity, this.transform);
    this.particles.transform.parent = this.particles.transform.parent.parent;
    this.particles.Emit(emitParams, this.maxParticles);
    this.StartCoroutine((IEnumerator) this.PauseParticles());
  }

  public IEnumerator PauseParticles()
  {
    yield return (object) new WaitForSeconds(this.particles.main.duration);
    this.particles.Pause();
  }
}
