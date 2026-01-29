// Decompiled with JetBrains decompiler
// Type: BellMechanics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BellMechanics : BaseMonoBehaviour
{
  public static Dictionary<GameObject, EnemyOoglerBell> OoglerBells = new Dictionary<GameObject, EnemyOoglerBell>();

  public static void AddOoglerBell(GameObject target, EnemyOoglerBell bell)
  {
    BellMechanics.OoglerBells.Add(target, bell);
  }

  public static void RemoveOoglerBellFromTarget(GameObject target)
  {
    BellMechanics.OoglerBells.Remove(target);
  }

  public static bool TargetHasOoglerBell(GameObject target)
  {
    return BellMechanics.OoglerBells.ContainsKey(target);
  }

  public static void BellParticles(GameObject particles, Vector3 position, float radius)
  {
    ParticleSystem component = ObjectPool.Spawn(particles, position).GetComponent<ParticleSystem>();
    if (!(bool) (Object) component)
      return;
    component.transform.localScale = Vector3.one * radius;
    component.Play();
  }

  public static void ActivateCloseBells(
    GameObject particles,
    Vector3 position,
    float radius,
    float delay = 0.33f,
    bool visible = true)
  {
    if (visible)
      BellMechanics.BellParticles(particles, position, radius);
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) position, radius))
    {
      TrapBellAvalanche component2 = component1.GetComponent<TrapBellAvalanche>();
      if ((bool) (Object) component2)
        component2.ActivateTrapDelay(delay);
    }
  }
}
