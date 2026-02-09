// Decompiled with JetBrains decompiler
// Type: LootEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class LootEgg : MonoBehaviour
{
  [SerializeField]
  public GameObject followerToSpawn;
  public Health health;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
    Object.Instantiate<GameObject>(this.followerToSpawn, this.transform.position, Quaternion.identity, (Object) GenerateRoom.Instance != (Object) null ? GenerateRoom.Instance.transform : this.transform.parent).GetComponent<Interaction_FollowerSpawn>().Play("");
  }
}
