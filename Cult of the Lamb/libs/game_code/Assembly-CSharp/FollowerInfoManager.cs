// Decompiled with JetBrains decompiler
// Type: FollowerInfoManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class FollowerInfoManager : BaseMonoBehaviour
{
  public static System.Action OnWorshipperDied;
  public FollowerInfo v_i;
  public FollowerHatType Hat;
  public FollowerOutfitType outfit;
  public FollowerClothingType Clothing;
  public FollowerCustomisationType Customisation;
  public FollowerSpecialType Special;
  public TextMeshPro Name;
  public static List<FollowerInfoManager> followerInfoManagers = new List<FollowerInfoManager>();
  public Health health;
  public bool RandomOnStart;
  public bool InitOnStart = true;
  public SkeletonAnimation Spine;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkinOverride;
  [SpineSkin("", "", true, false, false)]
  public string ForceOutfitSkinOverride;
  [SpineSkin("", "", true, false, false)]
  public string ForceExtraSkinOverride;
  public bool ForceSkin;
  public bool ForceOutfitSkin;
  public bool ForceExtraSkin;
  public bool IsHooded;
  public StateMachine state;

  public void OnEnable()
  {
    if (this.v_i == null && this.InitOnStart)
      this.NewV_I();
    FollowerInfoManager.followerInfoManagers.Add(this);
    this.health = this.GetComponent<Health>();
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnDie += new Health.DieAction(this.OnDie);
      this.health.OnHit += new Health.HitAction(this.OnHit);
    }
    if (!this.RandomOnStart)
      return;
    this.NewV_I();
  }

  public void OnDisable()
  {
    FollowerInfoManager.followerInfoManagers.Remove(this);
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void SetV_I(FollowerInfo v_i)
  {
    this.v_i = v_i;
    this.Initialise();
  }

  public void NewV_I()
  {
    this.v_i = this.ForceSkin ? FollowerInfo.NewCharacter(PlayerFarming.Location, this.ForceSkinOverride) : FollowerInfo.NewCharacter(PlayerFarming.Location);
    this.v_i.Outfit = this.outfit;
    this.Initialise();
    this.SetOutfit();
  }

  public void Initialise()
  {
    this.SetColor();
    this.AddName();
    this.SetHealth();
  }

  public IDAndRelationship GetRelationship(int ID)
  {
    foreach (IDAndRelationship relationship in this.v_i.Relationships)
    {
      if (relationship.ID == ID)
        return relationship;
    }
    IDAndRelationship relationship1 = new IDAndRelationship();
    relationship1.ID = ID;
    relationship1.Relationship = 0;
    this.v_i.Relationships.Add(relationship1);
    return relationship1;
  }

  public void SetOutfit()
  {
    if ((UnityEngine.Object) this.Spine == (UnityEngine.Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    this.SetOutfit(this.v_i.Outfit);
  }

  public void SetOutfit(FollowerOutfitType outfit)
  {
    FollowerBrain.SetFollowerCostume(this.Spine.skeleton, this.v_i, forceUpdate: true);
  }

  public void SetColor()
  {
    if ((UnityEngine.Object) this.Spine == (UnityEngine.Object) null)
      this.Spine = this.GetComponentInChildren<SkeletonAnimation>();
    this.state = this.GetComponent<StateMachine>();
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.SetOutfit();
  }

  public void SetSlotColour(string SlotName, Color color)
  {
    Slot slot = this.Spine.skeleton.FindSlot(SlotName);
    if (slot == null)
      return;
    slot.SetColor(color);
  }

  public void AddName()
  {
    if (!((UnityEngine.Object) this.Name != (UnityEngine.Object) null))
      return;
    this.Name.text = this.v_i.Name;
  }

  public void SetHealth()
  {
    Health component = this.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.HP = this.v_i.HP;
    component.totalHP = this.v_i.MaxHP;
  }

  public static FollowerInfoManager GetWorshipperInfoManagerByDwellingID(int ID)
  {
    foreach (FollowerInfoManager followerInfoManager in FollowerInfoManager.followerInfoManagers)
    {
      if (followerInfoManager.v_i != null && followerInfoManager.v_i.DwellingID == ID)
        return followerInfoManager;
    }
    return (FollowerInfoManager) null;
  }

  public virtual void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.OnDie();
  }

  public void OnDie()
  {
    System.Action onWorshipperDied = FollowerInfoManager.OnWorshipperDied;
    if (onWorshipperDied == null)
      return;
    onWorshipperDied();
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.v_i.HP = this.health.HP;
  }

  public enum Outfit
  {
    Rags,
    Sherpa,
    Warrior,
    Follower,
  }
}
