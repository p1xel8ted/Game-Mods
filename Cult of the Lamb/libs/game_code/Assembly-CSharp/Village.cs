// Decompiled with JetBrains decompiler
// Type: Village
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Village : BaseMonoBehaviour
{
  public List<GameObject> VillageStructures;
  public GameObject Villager;
  public GameObject King;
  public List<global::Villager> Villagers = new List<global::Villager>();

  public void Start()
  {
    for (int index1 = -1; index1 < 2; ++index1)
    {
      for (int index2 = -1; index2 < 2; ++index2)
      {
        if ((index1 != 0 || index2 != 0) && Random.Range(0, 100) < 75)
          Object.Instantiate<GameObject>(this.VillageStructures[Random.Range(0, this.VillageStructures.Count)], this.transform.position + new Vector3((float) (index1 * 2), (float) (index2 * 2)), Quaternion.identity);
      }
    }
    Vector2 vector2_1 = Random.insideUnitCircle * 3f;
    global::Villager component1 = Object.Instantiate<GameObject>(this.King, this.transform.position + new Vector3(vector2_1.x, vector2_1.y), Quaternion.identity).GetComponent<global::Villager>();
    component1.gameObject.GetComponent<Pet>().Owner = this.gameObject;
    this.Villagers.Add(component1);
    for (int index = 0; index < 3; ++index)
    {
      Vector2 vector2_2 = Random.insideUnitCircle * 3f;
      global::Villager component2 = Object.Instantiate<GameObject>(this.Villager, this.transform.position + new Vector3(vector2_2.x, vector2_2.y), Quaternion.identity).GetComponent<global::Villager>();
      component2.gameObject.GetComponent<Pet>().Owner = this.gameObject;
      this.Villagers.Add(component2);
    }
  }

  public void VillagersAttack(GameObject Attacker)
  {
    foreach (global::Villager villager in this.Villagers)
    {
      villager.formationFighter.enabled = true;
      villager.health.team = Health.Team.Team2;
    }
  }
}
