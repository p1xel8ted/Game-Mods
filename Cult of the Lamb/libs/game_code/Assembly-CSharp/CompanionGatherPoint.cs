// Decompiled with JetBrains decompiler
// Type: CompanionGatherPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CompanionGatherPoint : MonoBehaviour
{
  public static List<CompanionGatherPoint> GatherPoints = new List<CompanionGatherPoint>();
  public float activationRadius = 3f;
  public float delayActivationTime = -1f;
  public float timeAlive;
  public List<GameObject> GatherSlots = new List<GameObject>();
  [Header("Y-Limit Preference")]
  public bool enforceMinY;
  public float minAllowedY = -10f;
  public bool affectYngaGhosts = true;
  public bool affectTwinGhosts;
  public bool spawnGhostsOnEnable;
  public float destroyAfterTime = -1f;
  public bool destroyed;
  public float destroyTime;
  public bool shouldInstantSpawn;

  public void OnEnable()
  {
    this.timeAlive = -Time.time;
    if (!CompanionGatherPoint.GatherPoints.Contains(this))
      CompanionGatherPoint.GatherPoints.Add(this);
    if (this.spawnGhostsOnEnable)
      this.StartCoroutine(this.DelayedGhostSpawn());
    this.destroyed = false;
    this.destroyTime = 0.0f;
  }

  public void Update()
  {
    if (this.destroyed || (double) this.destroyAfterTime == -1.0)
      return;
    this.destroyTime += Time.deltaTime;
    if ((double) this.destroyTime < (double) this.destroyAfterTime)
      return;
    this.destroyed = true;
  }

  public IEnumerator DelayedGhostSpawn()
  {
    yield return (object) new WaitForSeconds(0.15f);
    CompanionBaseArea.SpawnCompanionGhosts();
  }

  public void OnDisable() => CompanionGatherPoint.GatherPoints.Remove(this);

  public void OnDestroy() => CompanionGatherPoint.GatherPoints.Remove(this);

  public Transform GetSlotForCompanion(int index)
  {
    if (this.GatherSlots == null || this.GatherSlots.Count == 0)
      return this.transform;
    int index1 = index % this.GatherSlots.Count;
    return !((Object) this.GatherSlots[index1] != (Object) null) ? this.transform : this.GatherSlots[index1].transform;
  }

  public bool ShouldActivateFor(Vector3 playerPosition)
  {
    return !this.destroyed && (double) this.timeAlive + (double) Time.time > (double) this.delayActivationTime && ((double) Vector3.Distance(playerPosition, this.transform.position) <= (double) this.activationRadius || this.enforceMinY && (double) playerPosition.y < (double) this.minAllowedY);
  }
}
