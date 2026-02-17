// Decompiled with JetBrains decompiler
// Type: ProjectilePatternBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectilePatternBase : BaseMonoBehaviour
{
  [SerializeField]
  public string key;
  public SkeletonAnimation spine;

  public static event ProjectilePatternBase.ProjectileEvent OnProjectileSpawned;

  public float timeScale => (Object) this.spine != (Object) null ? this.spine.timeScale : 1f;

  public void Awake() => this.spine = this.GetComponentInChildren<SkeletonAnimation>();

  public virtual IEnumerator ShootIE(
    float delay = 0.0f,
    GameObject target = null,
    Transform parent = null,
    bool allowSFXForEachShot = false)
  {
    yield break;
  }

  public void SpawnedProjectile()
  {
    ProjectilePatternBase.ProjectileEvent projectileSpawned = ProjectilePatternBase.OnProjectileSpawned;
    if (projectileSpawned == null)
      return;
    projectileSpawned();
  }

  public delegate void ProjectileEvent();
}
