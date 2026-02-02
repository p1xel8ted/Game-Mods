// Decompiled with JetBrains decompiler
// Type: TrapRockFall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TrapRockFall : BaseMonoBehaviour
{
  [SerializeField]
  public float playerDamage;
  [SerializeField]
  public float enemyDamage;
  [Space]
  [SerializeField]
  public float dropDelay;
  [SerializeField]
  public float dropSpeed;
  [SerializeField]
  public Vector2 rockTorque;
  [Space]
  [SerializeField]
  public GameObject rockContainer;
  [SerializeField]
  public GameObject[] rocks;
  [SerializeField]
  public GameObject pebbleContainer;
  [SerializeField]
  public SpriteRenderer[] pebbles;
  [SerializeField]
  public SpriteRenderer shadow;
  [SerializeField]
  public GameObject shadowToggle;
  [SerializeField]
  public GameObject debris;
  [SerializeField]
  public GameObject target;
  [SerializeField]
  public GameObject marking;
  [Space]
  [SerializeField]
  public CircleCollider2D collider;
  [SerializeField]
  public List<Sprite> particleChunks;
  [SerializeField]
  public float zSpawn;
  [SerializeField]
  public Material particleMaterial;
  [SerializeField]
  public ParticleSystem aoeParticles;
  [Space]
  [SerializeField]
  public UnityEvent onDrop;
  [SerializeField]
  public UnityEvent onLand;
  public bool dropped;
  public bool landed;
  public bool showDebris = true;

  public float DropDelay
  {
    get => this.dropDelay;
    set => this.dropDelay = value;
  }

  public float DropSpeed
  {
    get => this.dropSpeed;
    set => this.dropSpeed = value;
  }

  public void OnDisable()
  {
    if (!this.dropped || this.landed)
      return;
    this.aoeParticles.gameObject.SetActive(false);
    this.shadow.gameObject.SetActive(false);
    this.pebbleContainer.SetActive(false);
    this.rockContainer.SetActive(false);
    this.debris.SetActive(true);
    this.marking.SetActive(false);
  }

  public void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    if (!this.dropped)
      this.dropped = true;
    else if (this.dropped && !this.landed)
    {
      this.StopAllCoroutines();
      this.aoeParticles.gameObject.SetActive(false);
      this.shadow.gameObject.SetActive(false);
      this.pebbleContainer.SetActive(false);
      this.rockContainer.SetActive(false);
      this.debris.SetActive(false);
      this.marking.SetActive(true);
    }
    this.target.transform.DOScale(0.0f, 1f);
  }

  public virtual void Drop(bool showDebris = true)
  {
    this.showDebris = showDebris;
    this.StartCoroutine((IEnumerator) this.DropIE());
  }

  public IEnumerator DropIE()
  {
    TrapRockFall trapRockFall = this;
    trapRockFall.dropped = true;
    float increment = (float) ((double) trapRockFall.dropDelay / (double) trapRockFall.pebbles.Length / 2.0);
    trapRockFall.shadowToggle.SetActive(true);
    trapRockFall.shadow.transform.DOScale(Vector3.one * 3f, 4f);
    AudioManager.Instance.PlayOneShot("event:/material/stone_debris_fall", trapRockFall.transform.position);
    trapRockFall.pebbleContainer.SetActive(true);
    trapRockFall.marking.SetActive(true);
    SpriteRenderer[] spriteRendererArray = trapRockFall.pebbles;
    for (int index = 0; index < spriteRendererArray.Length; ++index)
    {
      SpriteRenderer pebble = spriteRendererArray[index];
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      pebble.DOFade(1f, 0.0f);
      pebble.transform.localPosition = (Vector3) (Random.insideUnitCircle * 1.25f);
      pebble.transform.DOMoveZ(0.0f, Random.Range(trapRockFall.dropDelay / 4f, trapRockFall.dropDelay / 2f)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
      pebble.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 1000f * Random.Range(1f, 3f)), trapRockFall.dropDelay / 2f, RotateMode.LocalAxisAdd).OnComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() => pebble.DOFade(0.0f, Random.Range(0.25f, 0.5f))));
      yield return (object) new WaitForSeconds(increment);
    }
    spriteRendererArray = (SpriteRenderer[]) null;
    while (PlayerRelic.TimeFrozen)
      yield return (object) null;
    trapRockFall.onDrop?.Invoke();
    trapRockFall.rockContainer.SetActive(true);
    trapRockFall.rockContainer.transform.localPosition = new Vector3(0.0f, 0.0f, -10f);
    trapRockFall.rockContainer.transform.DOMoveZ(0.0f, trapRockFall.dropSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(trapRockFall.Landed)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    foreach (GameObject rock in trapRockFall.rocks)
      rock.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 360f) * Random.Range(trapRockFall.rockTorque.x, trapRockFall.rockTorque.y), trapRockFall.dropSpeed, RotateMode.LocalAxisAdd);
    trapRockFall.shadow.DOFade(0.0f, 2.5f);
  }

  public virtual void Landed()
  {
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.collider.radius))
    {
      Health component = collider2D.GetComponent<Health>();
      PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collider2D.gameObject);
      if ((bool) (Object) component && (component.team != Health.Team.PlayerTeam || !TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent)) && !component.ImmuneToTraps)
        component.DealDamage(component.team == Health.Team.PlayerTeam ? this.playerDamage : this.enemyDamage, this.gameObject, this.transform.position);
    }
    AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/land_large", this.transform.position);
    this.rockContainer.SetActive(false);
    this.debris.SetActive(this.showDebris);
    this.marking.SetActive(false);
    this.shadowToggle.SetActive(false);
    int num = -1;
    if (this.particleChunks.Count > 0)
    {
      while (++num < 10)
      {
        if ((Object) this.particleMaterial == (Object) null)
          Particle_Chunk.AddNew(this.transform.position, (float) Random.Range(0, 360), this.particleChunks);
        else
          Particle_Chunk.AddNewMat(this.transform.position, (float) Random.Range(0, 360), this.particleChunks, mat: this.particleMaterial);
      }
    }
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.2f, false);
    this.aoeParticles.Play();
    this.onLand?.Invoke();
    this.onLand.RemoveAllListeners();
    this.landed = true;
  }

  public virtual void OnTriggerEnter2D(Collider2D other)
  {
    if (this.dropped || !other.gameObject.CompareTag("Player") && !((Object) other.GetComponent<FriendlyEnemy>() != (Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.DropIE());
  }
}
