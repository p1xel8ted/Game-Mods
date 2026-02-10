// Decompiled with JetBrains decompiler
// Type: FollowerBrainInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerBrainInfo
{
  public FollowerInfo _info;
  public FollowerBrain _brain;
  public System.Action OnPromotion;
  public float CacheXP;
  public static FollowerBrainStats.StatChangedEvent OnReadyToLevelUp;
  public System.Action OnReadyToPromote;
  public const float IllnessRadius = 3f;
  public const int MAX_LEVEL = 10;

  public FollowerBrainInfo(FollowerInfo info, FollowerBrain brain)
  {
    this._info = info;
    this._brain = brain;
  }

  public int ID => this._info.ID;

  public string Name
  {
    get => this._info.Name;
    set => this._info.Name = value;
  }

  public int Age
  {
    get => this._info.Age;
    set => this._info.Age = value;
  }

  public int LifeExpectancy
  {
    get
    {
      return this._info.LifeExpectancy * (this._info.Necklace == InventoryItem.ITEM_TYPE.Necklace_3 ? 2 : 1);
    }
    set => this._info.LifeExpectancy = value;
  }

  public bool OldAge
  {
    set => this._info.OldAge = value;
    get => this._info.OldAge;
  }

  public bool MarriedToLeader
  {
    set => this._info.MarriedToLeader = value;
    get => this._info.MarriedToLeader;
  }

  public int Pleasure
  {
    get => this._info.Pleasure;
    set => this._info.Pleasure = value;
  }

  public int TotalPleasure
  {
    get => this._info.TotalPleasure;
    set => this._info.TotalPleasure = value;
  }

  public float PrayProgress
  {
    get => this._info.PrayProgress;
    set => this._info.PrayProgress = value;
  }

  public bool FirstTimeSpeakingToPlayer
  {
    get => this._info.FirstTimeSpeakingToPlayer;
    set => this._info.FirstTimeSpeakingToPlayer = value;
  }

  public bool ComplainingAboutNoHouse
  {
    get => this._info.ComplainingAboutNoHouse;
    set => this._info.ComplainingAboutNoHouse = value;
  }

  public bool ComplainingNeedBetterHouse
  {
    get => this._info.ComplainingNeedBetterHouse;
    set => this._info.ComplainingNeedBetterHouse = value;
  }

  public bool IsDisciple
  {
    get => this._info.IsDisciple;
    set => this._info.IsDisciple = value;
  }

  public bool IsSnowman
  {
    get => this._info.IsSnowman;
    set => this._info.IsSnowman = value;
  }

  public bool IsGoodSnowman => this._info.IsSnowman && this._info.SkinName.Contains("Good");

  public bool IsDrunk => this._info.IsDrunk;

  public Thought CursedState
  {
    get => this._info.CursedState;
    set => this._info.CursedState = value;
  }

  public FollowerRole FollowerRole
  {
    get => this._info.FollowerRole;
    set => this._info.FollowerRole = value;
  }

  public WorkerPriority WorkerPriority
  {
    get => this._info.WorkerPriority;
    set => this._info.WorkerPriority = value;
  }

  public FollowerOutfitType Outfit
  {
    get => this._info.Outfit;
    set => this._info.Outfit = value;
  }

  public FollowerHatType Hat
  {
    get => this._info.Hat;
    set => this._info.Hat = value;
  }

  public FollowerClothingType Clothing
  {
    get => this._info.Clothing;
    set => this._info.Clothing = value;
  }

  public string ClothingVariant
  {
    get => this._info.ClothingVariant;
    set => this._info.ClothingVariant = value;
  }

  public string ClothingPreviousVariant
  {
    get => this._info.ClothingPreviousVariant;
    set => this._info.ClothingPreviousVariant = value;
  }

  public FollowerCustomisationType Customisation
  {
    get => this._info.Customisation;
    set => this._info.Customisation = value;
  }

  public FollowerSpecialType Special
  {
    get => this._info.Special;
    set => this._info.Special = value;
  }

  public FollowerProtectionType Protection
  {
    get
    {
      return this.Clothing != FollowerClothingType.None ? TailorManager.GetClothingData(this.Clothing).ProtectionType : FollowerProtectionType.None;
    }
  }

  public InventoryItem.ITEM_TYPE Necklace
  {
    get => this._info.Necklace;
    set => this._info.Necklace = value;
  }

  public string SkinName
  {
    get => this._info.SkinName;
    set => this._info.SkinName = value;
  }

  public int SkinColour
  {
    get => this._info.SkinColour;
    set => this._info.SkinColour = value;
  }

  public int SkinCharacter
  {
    get => this._info.SkinCharacter;
    set => this._info.SkinCharacter = value;
  }

  public int SkinVariation
  {
    get => this._info.SkinVariation;
    set => this._info.SkinVariation = value;
  }

  public int SacrificialValue => this._info.SacrificialValue;

  public InventoryItem.ITEM_TYPE SacrificialType
  {
    get
    {
      return !GameManager.HasUnlockAvailable() && !DataManager.Instance.DeathCatBeaten ? InventoryItem.ITEM_TYPE.BLACK_GOLD : this._info.SacrificialType;
    }
    set => this._info.SacrificialType = value;
  }

  public bool ShowingNecklace
  {
    get => this._info.ShowingNecklace;
    set => this._info.ShowingNecklace = value;
  }

  public bool NudistWinner
  {
    get => this._info.NudistWinner;
    set => this._info.NudistWinner = value;
  }

  public bool TaxEnforcer
  {
    get => this._info.TaxEnforcer;
    set => this._info.TaxEnforcer = value;
  }

  public bool FaithEnforcer
  {
    get => this._info.FaithEnforcer;
    set => this._info.FaithEnforcer = value;
  }

  public string ViewerID
  {
    get => this._info.ViewerID;
    set => this._info.ViewerID = value;
  }

  public static int DAYS_TILL_ROT_DEATH
  {
    get => !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.EmbraceRot) ? 10 : 15;
  }

  public float ProductivityMultiplier
  {
    get
    {
      float num = (float) (1.0 + (this._brain.ThoughtExists(Thought.Intimidated) ? 1.0 : 0.0) + (this._brain.HasTrait(FollowerTrait.TraitType.Industrious) ? 0.15000000596046448 : 0.0) - (this._brain.HasTrait(FollowerTrait.TraitType.Lazy) ? 0.10000000149011612 : 0.0) + (this._brain.HasTrait(FollowerTrait.TraitType.MushroomBanned) ? 0.05000000074505806 : 0.0) + (!this._brain.HasTrait(FollowerTrait.TraitType.Libertarian) || DataManager.Instance.Followers_Imprisoned_IDs.Count > 0 ? 0.0 : 0.05000000074505806) + (this._brain.ThoughtExists(Thought.PropogandaSpeakers) ? 0.20000000298023224 : 0.0) - (this._brain.HasTrait(FollowerTrait.TraitType.Hedonism) ? 0.10000000149011612 : 0.0) + (this._brain.HasTrait(FollowerTrait.TraitType.Asceticism) ? 0.15000000596046448 : 0.0) + (this._brain.HasTrait(FollowerTrait.TraitType.CriminalReformed) ? 0.20000000298023224 : 0.0) + (this._brain.HasTrait(FollowerTrait.TraitType.MarriedHappily) ? 0.20000000298023224 : 0.0) - (this._brain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily) ? 0.20000000298023224 : 0.0) + (this._brain.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_Winter || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter ? 0.0 : 0.5) + (!this._brain.HasTrait(FollowerTrait.TraitType.WinterBody) || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter ? -0.20000000298023224 : 0.30000001192092896) + (this._brain.HasTrait(FollowerTrait.TraitType.MasterfulSnowman) ? 1.0 : 0.0) - (this._brain.HasTrait(FollowerTrait.TraitType.ShoddySnowman) ? 0.25 : 0.0));
      return (float) (1.0 + ((double) Mathf.Clamp((float) this.XPLevel, 0.8f, 10f) - 1.0) / 5.0) * num;
    }
  }

  public int XPLevel
  {
    get => this._info.XPLevel;
    set => this._info.XPLevel = value;
  }

  public IEnumerable<IDAndRelationship> Relationships
  {
    get
    {
      foreach (IDAndRelationship relationship in this._info.Relationships)
        yield return relationship;
    }
  }

  public List<FollowerTrait.TraitType> Traits => this._info.Traits;

  public bool HasTrait(FollowerTrait.TraitType type)
  {
    bool flag = false;
    foreach (FollowerTrait.TraitType trait in this.Traits)
    {
      if (trait == type)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public IDAndRelationship GetOrCreateRelationship(int ID)
  {
    foreach (IDAndRelationship relationship1 in this.Relationships)
    {
      if (relationship1.ID == ID)
      {
        IDAndRelationship relationship2 = relationship1;
        if (ID < this._info.ID)
        {
          FollowerInfo infoById = FollowerInfo.GetInfoByID(ID);
          if (infoById != null)
          {
            FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
            if (brain != null)
              relationship2 = brain.Info.GetOrCreateRelationship(this._info.ID);
          }
        }
        return relationship2;
      }
    }
    IDAndRelationship relationship = new IDAndRelationship();
    relationship.ID = ID;
    relationship.Relationship = 0;
    this._info.Relationships.Add(relationship);
    return relationship;
  }

  public void NextSkin()
  {
    List<WorshipperData.CharacterSkin> skin = WorshipperData.Instance.GetCharacters(this.SkinName).Skin;
    ++this._info.SkinVariation;
    if (this._info.SkinVariation > skin.Count - 1)
      this._info.SkinVariation = 0;
    this._info.SkinName = skin[this._info.SkinVariation].Skin;
  }

  public void PrevSkin()
  {
    List<WorshipperData.CharacterSkin> skin = WorshipperData.Instance.GetCharacters(this.SkinName).Skin;
    --this._info.SkinVariation;
    if (this._info.SkinVariation < 0)
      this._info.SkinVariation = skin.Count - 1;
    this._info.SkinName = skin[this._info.SkinVariation].Skin;
  }

  public void NextSkinColor()
  {
    ++this._info.SkinColour;
    if (this._info.SkinColour <= WorshipperData.Instance.GetColourData(this.SkinName).SlotAndColours.Count - 1)
      return;
    this._info.SkinColour = 0;
  }

  public void PrevSkinColor()
  {
    --this._info.SkinColour;
    if (this._info.SkinColour >= 0)
      return;
    this._info.SkinColour = WorshipperData.Instance.GetColourData(this.SkinName).SlotAndColours.Count - 1;
  }
}
