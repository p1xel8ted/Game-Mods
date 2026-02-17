// Decompiled with JetBrains decompiler
// Type: TrapProjectileWall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapProjectileWall : MonoBehaviour, IProjectileTrap
{
  [SerializeField]
  public Health health;
  [SerializeField]
  public GameObject EndPillar;
  [SerializeField]
  public List<GameObject> PillarsOn = new List<GameObject>();
  [SerializeField]
  public List<GameObject> PillarsOff = new List<GameObject>();
  [SerializeField]
  public GameObject Projectile;
  public bool active;
  public bool roomCleared;
  [SerializeField]
  public List<global::Projectile> Projectiles = new List<global::Projectile>();
  public int Count = 5;
  public float Distance = 0.5f;
  public Vector2 Direction = Vector2.one;

  public void Start()
  {
    foreach (GameObject gameObject in this.PillarsOn)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.PillarsOff)
      gameObject.SetActive(true);
  }

  public void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.DeactivateProjectiles);
    this.PlaceEndPillar();
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.DeactivateProjectiles);
    this.active = false;
  }

  public void DeactivateProjectiles()
  {
    this.roomCleared = true;
    this.StopAllCoroutines();
    foreach (GameObject gameObject in this.PillarsOn)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.PillarsOff)
      gameObject.SetActive(true);
    for (int index = this.Projectiles.Count - 1; index >= 0; --index)
    {
      if ((Object) this.Projectiles[index] != (Object) null)
        this.Projectiles[index].DestroyProjectile(true);
    }
    this.Projectiles.Clear();
    this.health.enabled = false;
  }

  public void Update()
  {
    if (!GameManager.RoomActive || this.active || this.roomCleared)
      return;
    this.ActivateProjectiles();
  }

  public void ActivateProjectiles()
  {
    this.health.enabled = true;
    this.active = true;
    this.PlaceEndPillar();
    foreach (GameObject gameObject in this.PillarsOn)
      gameObject.SetActive(true);
    foreach (GameObject gameObject in this.PillarsOff)
      gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.ICreateProjectiles());
  }

  public void PlaceEndPillar()
  {
    this.EndPillar.transform.position = this.transform.position + this.EndPosition;
  }

  public void ResetEndPillar() => this.EndPillar.transform.position = this.transform.position;

  public IEnumerator ICreateProjectiles()
  {
    TrapProjectileWall trapProjectileWall = this;
    yield return (object) new WaitForSeconds(0.5f);
    int i = -1;
    while (++i < trapProjectileWall.Count - 1)
    {
      global::Projectile component = Object.Instantiate<GameObject>(trapProjectileWall.Projectile).GetComponent<global::Projectile>();
      component.transform.position = trapProjectileWall.transform.position + trapProjectileWall.BulletPosition(i);
      component.transform.parent = trapProjectileWall.transform.parent;
      component.transform.localScale = Vector3.zero;
      component.gameObject.SetActive(true);
      component.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      component.health = trapProjectileWall.health;
      component.team = Health.Team.Team2;
      trapProjectileWall.Projectiles.Add(component);
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, 0.5f, Color.red);
    for (int i = 0; i < this.Count; ++i)
      Utils.DrawCircleXY(this.transform.position + this.BulletPosition(i), 0.25f, Color.green);
    Utils.DrawCircleXY(this.transform.position + this.EndPosition, 0.5f, Color.red);
  }

  public Vector3 BulletPosition(int i)
  {
    return (Vector3) this.Direction * (float) (i + 1) * this.Distance;
  }

  public Vector3 EndPosition
  {
    get => (Vector3) this.Direction * ((float) (this.Count + 1) * this.Distance);
  }
}
