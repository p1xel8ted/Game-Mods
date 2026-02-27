// Decompiled with JetBrains decompiler
// Type: CreateChainAndWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreateChainAndWeapon : BaseMonoBehaviour
{
  public bool SpawnChain = true;
  public GameObject ChainPrefab;
  public GameObject WeaponPrefab;
  public GameObject ChainPoint;
  private Chain Chain;
  [HideInInspector]
  public WeaponPet Weapon;

  private void OnEnable()
  {
    this.Weapon = Object.Instantiate<GameObject>(this.WeaponPrefab, this.transform.position, Quaternion.identity).GetComponent<WeaponPet>();
    if (!this.SpawnChain)
      return;
    this.Chain = Object.Instantiate<GameObject>(this.ChainPrefab, this.transform.position, Quaternion.identity).GetComponent<Chain>();
    this.Chain.FixedPoint1 = this.ChainPoint.transform;
    this.Chain.FixedPoint2 = this.Weapon.ChainPoint;
  }
}
