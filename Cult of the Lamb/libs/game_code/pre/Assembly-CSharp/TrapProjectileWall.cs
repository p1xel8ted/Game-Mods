// Decompiled with JetBrains decompiler
// Type: TrapProjectileWall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapProjectileWall : MonoBehaviour
{
  [SerializeField]
  private Health health;
  [SerializeField]
  private GameObject EndPillar;
  [SerializeField]
  private List<GameObject> PillarsOn = new List<GameObject>();
  [SerializeField]
  private List<GameObject> PillarsOff = new List<GameObject>();
  [SerializeField]
  private GameObject Projectile;
  private bool active;
  private bool roomCleared;
  [SerializeField]
  private List<global::Projectile> Projectiles = new List<global::Projectile>();
  public int Count = 5;
  public float Distance = 0.5f;
  public Vector2 Direction = Vector2.one;

  private void Start()
  {
    foreach (GameObject gameObject in this.PillarsOn)
      gameObject.SetActive(false);
    foreach (GameObject gameObject in this.PillarsOff)
      gameObject.SetActive(true);
  }

  private void OnEnable()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.DeactivateProjectiles);
    this.PlaceEndPillar();
  }

  private void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.DeactivateProjectiles);
    this.active = false;
  }

  private void DeactivateProjectiles()
  {
    this.roomCleared = true;
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
  }

  private void Update()
  {
    if (!GameManager.RoomActive || this.active || this.roomCleared)
      return;
    this.ActivateProjectiles();
  }

  private void ActivateProjectiles()
  {
    this.active = true;
    this.PlaceEndPillar();
    foreach (GameObject gameObject in this.PillarsOn)
      gameObject.SetActive(true);
    foreach (GameObject gameObject in this.PillarsOff)
      gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.ICreateProjectiles());
  }

  private void PlaceEndPillar()
  {
    this.EndPillar.transform.position = this.transform.position + this.EndPosition;
  }

  private void ResetEndPillar() => this.EndPillar.transform.position = this.transform.position;

  private IEnumerator ICreateProjectiles()
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

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, 0.5f, Color.red);
    for (int i = 0; i < this.Count; ++i)
      Utils.DrawCircleXY(this.transform.position + this.BulletPosition(i), 0.25f, Color.green);
    Utils.DrawCircleXY(this.transform.position + this.EndPosition, 0.5f, Color.red);
  }

  private Vector3 BulletPosition(int i)
  {
    return (Vector3) this.Direction * (float) (i + 1) * this.Distance;
  }

  private Vector3 EndPosition
  {
    get => (Vector3) this.Direction * ((float) (this.Count + 1) * this.Distance);
  }
}
