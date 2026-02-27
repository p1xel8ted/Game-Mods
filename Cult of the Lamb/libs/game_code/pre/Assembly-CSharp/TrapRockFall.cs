// Decompiled with JetBrains decompiler
// Type: TrapRockFall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private float playerDamage;
  [SerializeField]
  private float enemyDamage;
  [Space]
  [SerializeField]
  private float dropDelay;
  [SerializeField]
  private float dropSpeed;
  [SerializeField]
  private Vector2 rockTorque;
  [Space]
  [SerializeField]
  private GameObject rockContainer;
  [SerializeField]
  private GameObject[] rocks;
  [SerializeField]
  private GameObject pebbleContainer;
  [SerializeField]
  private SpriteRenderer[] pebbles;
  [SerializeField]
  private SpriteRenderer shadow;
  [SerializeField]
  private GameObject shadowToggle;
  [SerializeField]
  private GameObject debris;
  [SerializeField]
  private GameObject target;
  [SerializeField]
  private GameObject marking;
  [Space]
  [SerializeField]
  private CircleCollider2D collider;
  [SerializeField]
  private List<Sprite> particleChunks;
  [SerializeField]
  public float zSpawn;
  [SerializeField]
  public Material particleMaterial;
  [SerializeField]
  private ParticleSystem aoeParticles;
  [Space]
  [SerializeField]
  private UnityEvent onDrop;
  [SerializeField]
  private UnityEvent onLand;
  private bool dropped;
  private bool landed;
  private bool showDebris = true;

  private void OnDisable()
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

  private void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void RoomLockController_OnRoomCleared()
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

  public void Drop(bool showDebris = true)
  {
    this.showDebris = showDebris;
    this.StartCoroutine((IEnumerator) this.DropIE());
  }

  private IEnumerator DropIE()
  {
    TrapRockFall trapRockFall = this;
    trapRockFall.dropped = true;
    float increment = (float) ((double) trapRockFall.dropDelay / (double) trapRockFall.pebbles.Length / 2.0);
    trapRockFall.shadowToggle.SetActive(true);
    trapRockFall.shadow.transform.DOScale(Vector3.one * 3f, 4f);
    AudioManager.Instance.PlayOneShot("event:/material/stone_debris_fall", trapRockFall.transform.position);
    trapRockFall.pebbleContainer.SetActive(true);
    SpriteRenderer[] spriteRendererArray = trapRockFall.pebbles;
    for (int index = 0; index < spriteRendererArray.Length; ++index)
    {
      SpriteRenderer pebble = spriteRendererArray[index];
      pebble.transform.localPosition = (Vector3) (Random.insideUnitCircle * 1.25f);
      pebble.transform.DOMoveZ(0.0f, Random.Range(trapRockFall.dropDelay / 4f, trapRockFall.dropDelay / 2f)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
      pebble.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 1000f * Random.Range(1f, 3f)), trapRockFall.dropDelay / 2f, RotateMode.LocalAxisAdd).OnComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() => pebble.DOFade(0.0f, Random.Range(0.25f, 0.5f))));
      yield return (object) new WaitForSeconds(increment);
    }
    spriteRendererArray = (SpriteRenderer[]) null;
    trapRockFall.onDrop?.Invoke();
    trapRockFall.rockContainer.transform.DOMoveZ(0.0f, trapRockFall.dropSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(trapRockFall.Landed)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    foreach (GameObject rock in trapRockFall.rocks)
      rock.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 360f) * Random.Range(trapRockFall.rockTorque.x, trapRockFall.rockTorque.y), trapRockFall.dropSpeed, RotateMode.LocalAxisAdd);
    trapRockFall.shadow.DOFade(0.0f, 2.5f);
  }

  private void Landed()
  {
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.collider.radius))
    {
      Health component2 = component1.GetComponent<Health>();
      if ((bool) (Object) component2)
        component2.DealDamage(component2.team == Health.Team.PlayerTeam ? this.playerDamage : this.enemyDamage, this.gameObject, this.transform.position);
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
    this.landed = true;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (this.dropped || !(other.gameObject.tag == "Player"))
      return;
    this.StartCoroutine((IEnumerator) this.DropIE());
  }
}
