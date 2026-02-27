// Decompiled with JetBrains decompiler
// Type: ProjectilePatternBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectilePatternBase : BaseMonoBehaviour
{
  [SerializeField]
  private string key;

  public static event ProjectilePatternBase.ProjectileEvent OnProjectileSpawned;

  public virtual IEnumerator ShootIE(float delay = 0.0f, GameObject target = null, Transform parent = null)
  {
    yield break;
  }

  protected void SpawnedProjectile()
  {
    ProjectilePatternBase.ProjectileEvent projectileSpawned = ProjectilePatternBase.OnProjectileSpawned;
    if (projectileSpawned == null)
      return;
    projectileSpawned();
  }

  public delegate void ProjectileEvent();
}
