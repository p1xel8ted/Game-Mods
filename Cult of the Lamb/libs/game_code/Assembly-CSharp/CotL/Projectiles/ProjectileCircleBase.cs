// Decompiled with JetBrains decompiler
// Type: CotL.Projectiles.ProjectileCircleBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CotL.Projectiles;

public abstract class ProjectileCircleBase : BaseMonoBehaviour
{
  public virtual void Init(float radius)
  {
  }

  public virtual void InitDelayed(
    GameObject target,
    float radius,
    float shootDelay,
    System.Action onShoot = null)
  {
  }
}
