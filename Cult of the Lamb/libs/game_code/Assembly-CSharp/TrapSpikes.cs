// Decompiled with JetBrains decompiler
// Type: TrapSpikes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapSpikes : BaseMonoBehaviour
{
  public TrapSpikes.State CurrentState;
  [SerializeField]
  public SpriteRenderer spriteRenderer;
  [SerializeField]
  public Collider2D boxCollider2D;
  public List<Collider2D> colliders;
  public ContactFilter2D contactFilter2D;
  [SerializeField]
  public GameObject SpikesParent;
  [SerializeField]
  public GameObject SpikeObject;
  [SerializeField]
  public bool startUp;
  public BoxCollider2D thisCollider2D;
  [SerializeField]
  public List<TrapSpike> spikes = new List<TrapSpike>();
  public bool deactivated;
  public bool showSpikesTrigger;
  public static Collider2D[] _hitBuffer = new Collider2D[16 /*0x10*/];
  public Health EnemyHealth;
  public GameObject ParentToDestroy;

  public void Start()
  {
    Transform transform = this.transform;
    Vector3 vector3 = transform.position;
    vector3 = new Vector3(vector3.x, vector3.y, -0.015f);
    transform.position = vector3;
    this.contactFilter2D = new ContactFilter2D();
    this.contactFilter2D.NoFilter();
    for (int index = 0; index < this.SpikesParent.transform.childCount; ++index)
      this.spikes.Add(this.SpikesParent.transform.GetChild(index).gameObject.GetComponent<TrapSpike>());
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
    if (this.startUp)
      this.SpikesUp();
    else
      this.showSpikesDefault();
  }

  public void OnEnable()
  {
  }

  public void OnDisable()
  {
    if (this.startUp)
      this.SpikesUp(false);
    else
      this.showSpikesDefault();
  }

  public void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.deactivated = true;
    this.DisableAllSpikes();
  }

  public void DisableAllSpikes()
  {
    for (int index = 0; index < this.spikes.Count; ++index)
      this.spikes[index].DisableSpike();
  }

  public void showSpikesDefault()
  {
    this.spriteRenderer.enabled = false;
    for (int index = 0; index < this.spikes.Count; ++index)
      this.spikes[index].AnimateSpike("in-idle", Color.white, true);
  }

  public void showSpikes(bool playSFX = true)
  {
    this.showSpikesTrigger = true;
    if (playSFX)
      AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", this.gameObject);
    for (int index = 0; index < this.spikes.Count; ++index)
    {
      this.spikes[index].SetRedSprite();
      this.spikes[index].AnimateSpike("out-idle", Color.red, true);
    }
  }

  public IEnumerator DoAttack(PlayerFarming playerFarming)
  {
    TrapSpikes trapSpikes = this;
    AudioManager.Instance.PlayOneShot("event:/material/footstep_hard", trapSpikes.transform.position);
    float Timer = 0.0f;
    foreach (TrapSpike spike in trapSpikes.spikes)
    {
      spike.SetWarningSprite();
      spike.transform.DOShakePosition(0.5f - Timer, new Vector3(0.1f, 0.0f, 0.0f));
    }
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      trapSpikes.spriteRenderer.color = Color.yellow;
      yield return (object) null;
    }
    trapSpikes.CurrentState = TrapSpikes.State.Attacking;
    if (!trapSpikes.showSpikesTrigger)
      trapSpikes.showSpikes();
    Timer = 0.0f;
    while ((double) (Timer += (double) Time.deltaTime == 0.0 ? 0.6f : Time.deltaTime) < 0.5)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      if ((double) Timer < 0.30000001192092896)
      {
        Transform transform = trapSpikes.boxCollider2D.transform;
        Bounds bounds = trapSpikes.boxCollider2D.bounds;
        Vector2 center = (Vector2) bounds.center;
        Vector2 size1 = (Vector2) bounds.size;
        float z = transform.eulerAngles.z;
        Vector2 size2 = size1;
        double angle = (double) z;
        Collider2D[] hitBuffer = TrapSpikes._hitBuffer;
        int num = Physics2D.OverlapBoxNonAlloc(center, size2, (float) angle, hitBuffer);
        for (int index = 0; index < num; ++index)
        {
          Collider2D collider2D = TrapSpikes._hitBuffer[index];
          Health component;
          if ((bool) (Object) collider2D && collider2D.TryGetComponent<Health>(out component))
          {
            if ((Object) playerFarming == (Object) null)
              playerFarming = PlayerFarming.Instance;
            if ((component.team != Health.Team.PlayerTeam || !TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, playerFarming)) && !component.ImmuneToTraps)
              component.DealDamage(1f, trapSpikes.gameObject, trapSpikes.transform.position);
          }
        }
      }
      trapSpikes.spriteRenderer.color = Color.red;
      yield return (object) null;
    }
    for (int index = 0; index < trapSpikes.spikes.Count; ++index)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_retract", trapSpikes.gameObject);
      trapSpikes.spikes[index].AnimateSpike("in", Color.white);
    }
    trapSpikes.showSpikesTrigger = false;
    trapSpikes.CurrentState = TrapSpikes.State.Idle;
    trapSpikes.spriteRenderer.color = Color.white;
    if (trapSpikes.deactivated)
      trapSpikes.DisableAllSpikes();
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.deactivated)
      return;
    this.EnemyHealth = collision.gameObject.GetComponent<Health>();
    if (this.CurrentState != TrapSpikes.State.Idle || !((Object) this.EnemyHealth != (Object) null) || this.EnemyHealth.team != Health.Team.PlayerTeam || !((Object) this.EnemyHealth.state != (Object) null) || this.EnemyHealth.state.CURRENT_STATE == StateMachine.State.Dodging)
      return;
    this.CurrentState = TrapSpikes.State.Warning;
    this.StartCoroutine((IEnumerator) this.DoAttack(this.EnemyHealth.GetComponent<PlayerFarming>()));
  }

  public void SpikesUp() => this.showSpikes();

  public void SpikesUp(bool playSFX) => this.showSpikes(playSFX);

  public void SpikesDown()
  {
    for (int index = 0; index < this.spikes.Count; ++index)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_retract", this.gameObject);
      this.spikes[index].AnimateSpike("in", Color.white);
    }
  }

  public void DestroySpikes() => Object.Destroy((Object) this.ParentToDestroy);

  public enum State
  {
    Idle,
    Warning,
    Attacking,
  }
}
