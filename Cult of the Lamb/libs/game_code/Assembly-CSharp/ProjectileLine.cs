// Decompiled with JetBrains decompiler
// Type: ProjectileLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectileLine : BaseMonoBehaviour
{
  [SerializeField]
  public float durationTillMaxLine;
  [SerializeField]
  public float stepSize = 0.5f;
  [SerializeField]
  public Vector2 direction = Vector2.left;
  [SerializeField]
  public int projectilesCount = 37;
  [SerializeField]
  public Projectile projectilePrefab;
  public Projectile[] projectiles;
  public Projectile projectile;

  public void Awake()
  {
    this.projectile = this.GetComponent<Projectile>();
    this.InitializeProjectiles();
  }

  public void InitializeProjectiles()
  {
    if ((Object) this.projectilePrefab == (Object) null)
    {
      Debug.LogError((object) "Missing prefab", (Object) this);
    }
    else
    {
      this.projectiles = new Projectile[this.projectilesCount];
      if (ObjectPool.CountPooled<Projectile>(this.projectilePrefab) != 0)
        return;
      ObjectPool.CreatePool<Projectile>(this.projectilePrefab, this.projectilesCount);
    }
  }

  public void InitDelayed(GameObject target, float shootDelay, float angle)
  {
    float num1 = this.projectiles.Length % 2 == 0 ? this.stepSize / 2f : 0.0f;
    int num2 = this.projectiles.Length % 2;
    for (int index = 0; index < this.projectiles.Length; ++index)
    {
      int num3 = index % 2 > 0 ? -1 : 1;
      Vector2 vector2 = ((float) ((index + num2) / 2) * this.stepSize + num1) * (float) num3 * this.direction;
      Projectile projectile = ObjectPool.Spawn<Projectile>(this.projectilePrefab, this.transform);
      projectile.transform.localPosition = Vector3.zero;
      projectile.transform.localPosition = (Vector3) vector2;
      projectile.health = this.projectile.health;
      projectile.team = Health.Team.Team2;
      projectile.gameObject.SetActive(false);
      this.projectiles[index] = projectile;
    }
    this.StartCoroutine((IEnumerator) this.EnableProjectiles(target, shootDelay, angle));
  }

  public IEnumerator EnableProjectiles(GameObject target, float delay, float angle)
  {
    this.projectile.SpeedMultiplier = 0.0f;
    Projectile[] projectileArray = this.projectiles;
    for (int index = 0; index < projectileArray.Length; ++index)
    {
      projectileArray[index].gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(0.02f);
    }
    projectileArray = (Projectile[]) null;
    yield return (object) new WaitForSeconds(0.25f + delay);
    this.projectile.Angle = angle;
    this.projectile.SpeedMultiplier = 1f;
  }
}
