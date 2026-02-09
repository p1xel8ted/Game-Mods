// Decompiled with JetBrains decompiler
// Type: RefinedResourcesRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RefinedResourcesRoomManager : MonoBehaviour
{
  [SerializeField]
  public List<SimpleBark> barks;
  [Space]
  [SerializeField]
  public List<Health> roaches = new List<Health>();

  public void Start()
  {
    foreach (Health roach in this.roaches)
      roach.OnHitEarly += new Health.HitAction(this.OnHitRoach);
  }

  public void OnDestroy()
  {
    foreach (Health roach in this.roaches)
    {
      if ((Object) roach != (Object) null)
        roach.OnHitEarly -= new Health.HitAction(this.OnHitRoach);
    }
  }

  public void OnHitRoach(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    for (int index = 0; index < this.barks.Count; ++index)
      this.barks[index].CloseImmediately();
    this.barks[Random.Range(0, this.barks.Count)].gameObject.SetActive(true);
  }
}
