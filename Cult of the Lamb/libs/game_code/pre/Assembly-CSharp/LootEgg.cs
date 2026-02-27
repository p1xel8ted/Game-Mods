// Decompiled with JetBrains decompiler
// Type: LootEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class LootEgg : MonoBehaviour
{
  [SerializeField]
  private GameObject followerToSpawn;
  private Health health;

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
  }

  private void Health_OnDie(
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
