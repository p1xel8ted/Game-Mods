// Decompiled with JetBrains decompiler
// Type: CreateChainAndWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreateChainAndWeapon : BaseMonoBehaviour
{
  public bool SpawnChain = true;
  public GameObject ChainPrefab;
  public GameObject WeaponPrefab;
  public GameObject ChainPoint;
  public Chain Chain;
  [HideInInspector]
  public WeaponPet Weapon;

  public void OnEnable()
  {
    this.Weapon = Object.Instantiate<GameObject>(this.WeaponPrefab, this.transform.position, Quaternion.identity).GetComponent<WeaponPet>();
    if (!this.SpawnChain)
      return;
    this.Chain = Object.Instantiate<GameObject>(this.ChainPrefab, this.transform.position, Quaternion.identity).GetComponent<Chain>();
    this.Chain.FixedPoint1 = this.ChainPoint.transform;
    this.Chain.FixedPoint2 = this.Weapon.ChainPoint;
  }
}
