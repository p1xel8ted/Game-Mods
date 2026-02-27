// Decompiled with JetBrains decompiler
// Type: TrapSpikes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapSpikes : BaseMonoBehaviour
{
  private TrapSpikes.State CurrentState;
  [SerializeField]
  private SpriteRenderer spriteRenderer;
  [SerializeField]
  private Collider2D boxCollider2D;
  private List<Collider2D> colliders;
  private ContactFilter2D contactFilter2D;
  [SerializeField]
  private GameObject SpikesParent;
  [SerializeField]
  private GameObject SpikeObject;
  private BoxCollider2D thisCollider2D;
  [SerializeField]
  private List<TrapSpike> spikes = new List<TrapSpike>();
  private bool deactivated;
  private bool showSpikesTrigger;
  private Health EnemyHealth;
  public GameObject ParentToDestroy;

  private void Start()
  {
    Transform transform = this.transform;
    Vector3 vector3 = transform.position;
    vector3 = new Vector3(vector3.x, vector3.y, -0.015f);
    transform.position = vector3;
    this.contactFilter2D = new ContactFilter2D();
    this.contactFilter2D.NoFilter();
    for (int index = 0; index < this.SpikesParent.transform.childCount; ++index)
      this.spikes.Add(this.SpikesParent.transform.GetChild(index).gameObject.GetComponent<TrapSpike>());
    this.showSpikesDefault();
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void OnEnable()
  {
  }

  private void OnDisable() => this.showSpikesDefault();

  private void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void RoomLockController_OnRoomCleared()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.deactivated = true;
    for (int index = 0; index < this.spikes.Count; ++index)
      this.spikes[index].DisableSpike();
  }

  private void showSpikesDefault()
  {
    this.spriteRenderer.enabled = false;
    for (int index = 0; index < this.spikes.Count; ++index)
      this.spikes[index].AnimateSpike("in-idle", Color.white);
  }

  private void showSpikes()
  {
    this.showSpikesTrigger = true;
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", this.gameObject);
    for (int index = 0; index < this.spikes.Count; ++index)
    {
      this.spikes[index].SetRedSprite();
      this.spikes[index].AnimateSpike("out-idle", Color.red);
    }
  }

  private IEnumerator DoAttack()
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
      trapSpikes.spriteRenderer.color = Color.yellow;
      yield return (object) null;
    }
    trapSpikes.CurrentState = TrapSpikes.State.Attacking;
    if (!trapSpikes.showSpikesTrigger)
      trapSpikes.showSpikes();
    Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      if ((double) Timer < 0.30000001192092896)
      {
        trapSpikes.colliders = new List<Collider2D>();
        trapSpikes.boxCollider2D.OverlapCollider(trapSpikes.contactFilter2D, (List<Collider2D>) trapSpikes.colliders);
        foreach (Collider2D collider in trapSpikes.colliders)
        {
          trapSpikes.EnemyHealth = collider.GetComponent<Health>();
          if ((Object) trapSpikes.EnemyHealth != (Object) null)
            trapSpikes.EnemyHealth.DealDamage(1f, trapSpikes.gameObject, trapSpikes.transform.position);
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
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.deactivated)
      return;
    this.EnemyHealth = collision.gameObject.GetComponent<Health>();
    if (this.CurrentState != TrapSpikes.State.Idle || !((Object) this.EnemyHealth != (Object) null) || this.EnemyHealth.team != Health.Team.PlayerTeam || !((Object) this.EnemyHealth.state != (Object) null) || this.EnemyHealth.state.CURRENT_STATE == StateMachine.State.Dodging)
      return;
    this.CurrentState = TrapSpikes.State.Warning;
    this.StartCoroutine((IEnumerator) this.DoAttack());
  }

  public void DestroySpikes() => Object.Destroy((Object) this.ParentToDestroy);

  private enum State
  {
    Idle,
    Warning,
    Attacking,
  }
}
