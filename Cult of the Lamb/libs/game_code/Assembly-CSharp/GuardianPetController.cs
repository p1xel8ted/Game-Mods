// Decompiled with JetBrains decompiler
// Type: GuardianPetController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GuardianPetController : MonoBehaviour
{
  [CompilerGenerated]
  public GuardianPet \u003CInactivePet\u003Ek__BackingField;
  public GuardianPet[] PetPrefabs;
  [CompilerGenerated]
  public Health \u003CHostHealth\u003Ek__BackingField;
  [CompilerGenerated]
  public SkeletonAnimation \u003CHostSpine\u003Ek__BackingField;
  [SerializeField]
  public int petIndex = -1;
  public List<GuardianPet> activePets = new List<GuardianPet>();
  public bool HalfHealth;
  public bool ThirdHealth;

  public GuardianPet InactivePet
  {
    get => this.\u003CInactivePet\u003Ek__BackingField;
    set => this.\u003CInactivePet\u003Ek__BackingField = value;
  }

  public Health HostHealth
  {
    get => this.\u003CHostHealth\u003Ek__BackingField;
    set => this.\u003CHostHealth\u003Ek__BackingField = value;
  }

  public SkeletonAnimation HostSpine
  {
    get => this.\u003CHostSpine\u003Ek__BackingField;
    set => this.\u003CHostSpine\u003Ek__BackingField = value;
  }

  public int PetIndex
  {
    get => this.petIndex;
    set => this.petIndex = value;
  }

  public int Init(Health host, SkeletonAnimation hostSpine)
  {
    this.HostHealth = host;
    this.HostSpine = hostSpine;
    if (this.petIndex == -1)
      this.petIndex = Random.Range(0, this.PetPrefabs.Length);
    return this.petIndex;
  }

  public GuardianPet InstantiatePet()
  {
    return Object.Instantiate<GuardianPet>(this.PetPrefabs[this.petIndex], this.transform);
  }

  public void OnHit(
    GameObject attacker,
    Vector3 attacklocation,
    Health.AttackTypes attacktype,
    bool frombehind)
  {
  }

  public void OnDisable()
  {
    if (!((Object) this.HostHealth != (Object) null))
      return;
    this.HostHealth.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void OnEnable()
  {
    if ((Object) this.HostHealth != (Object) null)
    {
      this.HostHealth.OnHit -= new Health.HitAction(this.OnHit);
      this.HostHealth.OnHit += new Health.HitAction(this.OnHit);
    }
    foreach (GuardianPet activePet in this.activePets)
      activePet.Play();
  }

  public void OnHostDie()
  {
    if ((Object) this.InactivePet != (Object) null)
    {
      Object.Destroy((Object) this.InactivePet.gameObject);
      this.InactivePet = (GuardianPet) null;
    }
    for (int index = this.activePets.Count - 1; index >= 0; --index)
    {
      if ((Object) this.activePets[index] != (Object) null)
      {
        this.activePets[index].health.invincible = false;
        this.activePets[index].health.DealDamage(9999f, this.HostHealth.gameObject, this.activePets[index].transform.position);
      }
    }
  }

  public void RemoveActivePet(GuardianPet petToRemove) => this.activePets.Remove(petToRemove);

  public void Launch()
  {
    if ((Object) this.InactivePet == (Object) null)
      this.InactivePet = this.InstantiatePet();
    if ((Object) this.InactivePet != (Object) null)
      this.InactivePet.LaunchPet(this);
    this.activePets.Add(this.InactivePet);
    this.InactivePet = (GuardianPet) null;
    this.GetComponentInParent<EnemySimpleGuardian>().RemovePetSkin(this.NameFromIndex(this.petIndex));
  }

  public string NameFromIndex(int index)
  {
    string str = "Beam";
    switch (index)
    {
      case 1:
        str = "Melee";
        break;
      case 2:
        str = "Projectiles";
        break;
    }
    return str;
  }
}
