// Decompiled with JetBrains decompiler
// Type: TrapBurningWoodStack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TrapBurningWoodStack : BaseMonoBehaviour
{
  [SerializeField]
  public Health healthInactive;
  [SerializeField]
  public Collider2D colldierInactive;
  [SerializeField]
  public ColliderEvents colldierActive;

  public void Awake()
  {
    this.healthInactive.OnDie += new Health.DieAction(this.OnInactiveDie);
    this.colldierActive.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageCollisionTrigger);
  }

  public void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnInactiveDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.colldierInactive.gameObject.SetActive(false);
    this.colldierActive.gameObject.SetActive(true);
  }

  public void OnDamageCollisionTrigger(Collider2D collider)
  {
    Health component = collider.gameObject.GetComponent<Health>();
    if (!((Object) component != (Object) null))
      return;
    component.AddBurn(this.gameObject);
  }

  public void RoomLockController_OnRoomCleared()
  {
    this.colldierActive.GetComponent<CircleCollider2D>().enabled = false;
    foreach (ParticleSystem componentsInChild in this.colldierActive.GetComponentsInChildren<ParticleSystem>())
      this.StopParticle(componentsInChild);
    foreach (ParticleSystem componentsInChild in this.colldierInactive.GetComponentsInChildren<ParticleSystem>())
      this.StopParticle(componentsInChild);
  }

  public void StopParticle(ParticleSystem particleSystem)
  {
    particleSystem.Stop();
    particleSystem.main.playOnAwake = false;
  }
}
