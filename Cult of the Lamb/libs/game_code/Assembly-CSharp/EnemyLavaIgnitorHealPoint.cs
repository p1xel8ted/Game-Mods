// Decompiled with JetBrains decompiler
// Type: EnemyLavaIgnitorHealPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class EnemyLavaIgnitorHealPoint : MonoBehaviour
{
  public static List<EnemyLavaIgnitorHealPoint> healPoints = new List<EnemyLavaIgnitorHealPoint>();
  public static List<EnemyLavaIgnitorHealPoint> usedHealPoints = new List<EnemyLavaIgnitorHealPoint>();
  public GameObject mustBeActiveToHealGameObject;
  public float healTimeNeeded;
  public bool requiresJumpToHeal;
  public bool requiresJumpBackAfterHeal;
  public bool isAttachedToFireTrap;
  public float jumpArcHeight = 1.5f;
  public float jumpArcDuration = 0.5f;
  public GameObject jumpTargetObject;
  public float healTriggerDistance = 0.25f;

  public void OnEnable()
  {
    if (!EnemyLavaIgnitorHealPoint.healPoints.Contains(this))
    {
      EnemyLavaIgnitorHealPoint.healPoints.Add(this);
      EnemyLavaIgnitorHealPoint.healPoints = EnemyLavaIgnitorHealPoint.healPoints.OrderBy<EnemyLavaIgnitorHealPoint, float>((Func<EnemyLavaIgnitorHealPoint, float>) (_ => UnityEngine.Random.value)).ToList<EnemyLavaIgnitorHealPoint>();
    }
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<MeshRenderer>())
      componentsInChild.enabled = false;
  }

  public void OnDisable()
  {
    EnemyLavaIgnitorHealPoint.healPoints.Remove(this);
    EnemyLavaIgnitorHealPoint.usedHealPoints.Remove(this);
  }

  public void OnDestroy()
  {
    EnemyLavaIgnitorHealPoint.healPoints.Remove(this);
    EnemyLavaIgnitorHealPoint.usedHealPoints.Remove(this);
  }
}
