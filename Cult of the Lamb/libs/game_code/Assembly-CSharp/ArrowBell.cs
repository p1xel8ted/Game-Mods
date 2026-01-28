// Decompiled with JetBrains decompiler
// Type: ArrowBell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class ArrowBell : MonoBehaviour
{
  [SerializeField]
  public TrapAvalanche trapAvalanche;
  [SerializeField]
  public GameObject aoeParticles;
  [SerializeField]
  public float radius;
  [SerializeField]
  public Transform arrowImage;
  [SerializeField]
  public float dropDelay;
  [SerializeField]
  public float dropSpeed;
  [SerializeField]
  public float waveFrequency;
  [SerializeField]
  public float waveRadius;
  public Projectile projectile;
  public float freqTimer;

  public void Awake()
  {
    this.projectile = this.GetComponent<Projectile>();
    this.freqTimer = this.waveFrequency;
  }

  public void Update() => this.UpdateFrequencyTimer();

  public void OnEnable()
  {
    this.projectile.health.OnHit += new Health.HitAction(this.OnHit);
    this.projectile.OnDestroyProjectile.AddListener(new UnityAction(this.OnHit));
  }

  public void OnDisable()
  {
    this.projectile.health.OnHit -= new Health.HitAction(this.OnHit);
    this.projectile.OnDestroyProjectile.RemoveListener(new UnityAction(this.OnHit));
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", this.transform.position);
    this.DropAvalanche();
    this.ActivateCloseBells();
  }

  public void OnHit()
  {
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", this.transform.position);
    this.DropAvalanche();
    this.ActivateCloseBells();
  }

  public void DropAvalanche()
  {
    TrapAvalanche component = ObjectPool.Spawn<TrapAvalanche>(this.trapAvalanche, this.transform.position, Quaternion.identity).GetComponent<TrapAvalanche>();
    component.DropDelay = this.dropDelay;
    component.DropSpeed = this.dropSpeed;
    component.Drop();
  }

  public void ActivateCloseBells()
  {
    BellMechanics.ActivateCloseBells(this.aoeParticles, this.transform.position, this.radius);
  }

  public void ActivateCloseBells(float radius, bool visible = true)
  {
    BellMechanics.ActivateCloseBells(this.aoeParticles, this.transform.position, radius, visible: visible);
  }

  public void UpdateFrequencyTimer()
  {
    this.freqTimer += Time.deltaTime;
    if ((double) this.freqTimer < (double) this.waveFrequency)
      return;
    this.ActivateCloseBells(this.waveRadius, false);
    this.freqTimer = 0.0f;
  }
}
