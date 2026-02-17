// Decompiled with JetBrains decompiler
// Type: WorshipperInfoManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class WorshipperInfoManager : BaseMonoBehaviour
{
  public static System.Action OnWorshipperDied;
  public Villager_Info v_i;
  public TextMeshPro Name;
  public static List<WorshipperInfoManager> worshipperInfoManagers = new List<WorshipperInfoManager>();
  public Health health;
  public bool RandomOnStart;
  public SkeletonAnimation Spine;
  public bool IsHooded;
  public StateMachine state;

  public void OnEnable()
  {
    WorshipperInfoManager.worshipperInfoManagers.Add(this);
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
    WorshipperInfoManager.worshipperInfoManagers.Remove(this);
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void SetV_I(Villager_Info v_i)
  {
    this.v_i = v_i;
    this.Initialise();
  }

  public void NewV_I()
  {
    this.v_i = Villager_Info.NewCharacter();
    this.Initialise();
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

  public void SetOutfit(WorshipperInfoManager.Outfit outfit, bool Hooded)
  {
    Skin newSkin = new Skin("New Skin");
    newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(this.v_i.SkinName));
    switch (outfit)
    {
      case WorshipperInfoManager.Outfit.Rags:
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Clothes/Rags"));
        break;
      case WorshipperInfoManager.Outfit.Sherpa:
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Clothes/Sherpa"));
        break;
      case WorshipperInfoManager.Outfit.Warrior:
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Clothes/Warrior"));
        break;
      case WorshipperInfoManager.Outfit.Follower:
        if (this.v_i.DwellingClaimed)
        {
          newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Clothes/Robes_Lvl" + this.v_i.Level.ToString()));
          break;
        }
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Clothes/NoHouse_Lvl" + this.v_i.Level.ToString()));
        break;
    }
    if (Hooded)
    {
      newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Clothes/Hooded_Lvl" + this.v_i.Level.ToString()));
      this.IsHooded = true;
    }
    else
      this.IsHooded = false;
    this.Spine.Skeleton.SetSkin(newSkin);
    this.Spine.skeleton.SetSlotsToSetupPose();
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(this.v_i.SkinName);
    if (colourData == null)
      return;
    foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(this.v_i.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
      this.SetSlotColour(slotAndColour.Slot, slotAndColour.color);
  }

  public void SetColor()
  {
    if ((UnityEngine.Object) this.Spine == (UnityEngine.Object) null)
      this.Spine = this.GetComponentInChildren<SkeletonAnimation>();
    this.state = this.GetComponent<StateMachine>();
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.SetOutfit(this.v_i.Outfit, false);
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
    component.totalHP = this.v_i.TotalHP;
  }

  public static WorshipperInfoManager GetWorshipperInfoManagerByJobID(string ID)
  {
    foreach (WorshipperInfoManager worshipperInfoManager in WorshipperInfoManager.worshipperInfoManagers)
    {
      if (worshipperInfoManager.v_i != null && worshipperInfoManager.v_i.WorkPlace == ID)
        return worshipperInfoManager;
    }
    return (WorshipperInfoManager) null;
  }

  public static WorshipperInfoManager GetWorshipperInfoManagerByDwellingID(string ID)
  {
    foreach (WorshipperInfoManager worshipperInfoManager in WorshipperInfoManager.worshipperInfoManagers)
    {
      if (worshipperInfoManager.v_i != null && worshipperInfoManager.v_i.Dwelling == ID)
        return worshipperInfoManager;
    }
    return (WorshipperInfoManager) null;
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
    System.Action onWorshipperDied = WorshipperInfoManager.OnWorshipperDied;
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
