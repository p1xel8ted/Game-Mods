// Decompiled with JetBrains decompiler
// Type: UIMatingInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIMatingInfoCard : MonoBehaviour
{
  [SerializeField]
  public SkeletonGraphic Follower1;
  [SerializeField]
  public SkeletonGraphic Follower2;
  [SerializeField]
  public TMP_Text textFollower1;
  [SerializeField]
  public TMP_Text textFollower2;
  [SerializeField]
  public Image fillWheel;
  [SerializeField]
  public Image fillWheelBackground;
  [SerializeField]
  public RawImage fillWheelDetails;
  [SerializeField]
  public TMP_Text description;
  [SerializeField]
  public TMP_Text specialEffect;
  [SerializeField]
  public TraitInfoCardController traitInfoCardController;
  [SerializeField]
  public IndoctrinationTraitItem traitItem;
  [SerializeField]
  public List<GameObject> parent1TraitSlots;
  [SerializeField]
  public List<GameObject> parent2TraitSlots;

  public void Configure(
    Interaction_MatingTent matingTent,
    FollowerInfo info1,
    FollowerInfo info2,
    UIMatingMenuController menuController,
    bool fadeFollower1 = false,
    bool fadeFollower2 = false)
  {
    if (info1 != null)
    {
      this.Follower1.ConfigureFollower(info1);
      this.textFollower1.text = info1.Name;
      if (!fadeFollower1)
        this.Follower1.color = Color.white;
      else
        this.Follower1.color = Color.gray;
    }
    else
    {
      this.Follower1.color = Color.black;
      this.textFollower1.text = "";
    }
    if (info2 != null)
    {
      this.Follower2.ConfigureFollower(info2);
      this.textFollower2.text = info2.Name;
      if (!fadeFollower2)
        this.Follower2.color = Color.white;
      else
        this.Follower2.color = Color.gray;
    }
    else
    {
      this.Follower2.color = Color.black;
      this.textFollower2.text = "";
    }
    if (info1 == null || info2 == null)
    {
      this.fillWheel.fillAmount = 0.0f;
      this.fillWheel.CrossFadeAlpha(0.25f, 0.125f, true);
      this.fillWheelBackground.CrossFadeAlpha(0.25f, 0.25f, true);
      this.fillWheelDetails.CrossFadeAlpha(0.25f, 0.5f, true);
    }
    else
    {
      this.fillWheel.CrossFadeAlpha(1f, 0.125f, true);
      this.fillWheelBackground.CrossFadeAlpha(1f, 0.25f, true);
      this.fillWheelDetails.CrossFadeAlpha(1f, 0.5f, true);
    }
    this.description.text = "";
    if (info1 != null || info2 != null)
    {
      if (info1 != null && info2 != null)
      {
        float chanceToMate = Interaction_MatingTent.GetChanceToMate(FollowerBrain.GetOrCreateBrain(info1), FollowerBrain.GetOrCreateBrain(info2));
        this.fillWheel.fillAmount = chanceToMate;
        this.description.text = string.Format(LocalizationManager.GetTranslation("UI/ChanceToBreed"), (object) Mathf.RoundToInt(chanceToMate * 100f));
      }
      this.specialEffect.text = "";
      if (info1 != null && info1.Traits.Contains(FollowerTrait.TraitType.Celibate) || info2 != null && info2.Traits.Contains(FollowerTrait.TraitType.Celibate))
      {
        if (info1 != null && info1.Traits.Contains(FollowerTrait.TraitType.Celibate))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/HasCelibateTrait"), (object) info1.Name)}<br>";
        }
        if (info2 != null && info2.Traits.Contains(FollowerTrait.TraitType.Celibate))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/HasCelibateTrait"), (object) info2.Name)}<br>";
        }
      }
      else
      {
        if (info1 != null && info1.Traits.Contains(FollowerTrait.TraitType.Lustful))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/HasLustfulTrait"), (object) info1.Name)}<br>";
        }
        if (info2 != null && info2.Traits.Contains(FollowerTrait.TraitType.Lustful))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/HasLustfulTrait"), (object) info2.Name)}<br>";
        }
        if (info1 != null && info1.Traits.Contains(FollowerTrait.TraitType.MissionaryExcited))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/HasMissionaryExcitedTrait"), (object) info1.Name)}<br>";
        }
        if (info2 != null && info2.Traits.Contains(FollowerTrait.TraitType.MissionaryExcited))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/HasMissionaryExcitedTrait"), (object) info2.Name)}<br>";
        }
      }
      if (info1 != null && info2 != null)
      {
        if (FollowerManager.IsChildOf(info1.ID, info2.ID))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/IsChild"), (object) info1.Name, (object) info2.Name)}<br>";
        }
        else if (FollowerManager.IsChildOf(info2.ID, info1.ID))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/IsChild"), (object) info2.Name, (object) info1.Name)}<br>";
        }
        if (FollowerManager.AreSiblings(info1.ID, info2.ID))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/IsSibling"), (object) info1.Name, (object) info2.Name)}<br>";
        }
      }
      if (info1 != null && (double) info1.Drunk > 0.0)
      {
        TMP_Text specialEffect = this.specialEffect;
        specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Drunk"), (object) info1.Name)}<br>";
      }
      if (info2 != null && (double) info2.Drunk > 0.0)
      {
        TMP_Text specialEffect = this.specialEffect;
        specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Drunk"), (object) info2.Name)}<br>";
      }
      if (info1 != null && (double) info1.Exhaustion > 0.0)
      {
        TMP_Text specialEffect = this.specialEffect;
        specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Exhausted"), (object) info1.Name)}<br>";
      }
      if (info2 != null && (double) info2.Exhaustion > 0.0)
      {
        TMP_Text specialEffect = this.specialEffect;
        specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Exhausted"), (object) info2.Name)}<br>";
      }
      if (info1 != null && info1.Traits.Contains(FollowerTrait.TraitType.Zombie) && info2 != null && info2.Traits.Contains(FollowerTrait.TraitType.Zombie))
      {
        TMP_Text specialEffect = this.specialEffect;
        specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/BothZombies"), (object) info2.Name)}<br>";
      }
      else
      {
        if (info1 != null && info1.Traits.Contains(FollowerTrait.TraitType.Zombie))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Zombie"), (object) info1.Name)}<br>";
        }
        if (info2 != null && info2.Traits.Contains(FollowerTrait.TraitType.Zombie))
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Zombie"), (object) info2.Name)}<br>";
        }
      }
      if (info1 != null && info2 != null)
      {
        IDAndRelationship relationship = FollowerBrain.GetOrCreateBrain(info1).Info.GetOrCreateRelationship(info2.ID);
        if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
        {
          TMP_Text specialEffect = this.specialEffect;
          specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Friends"), (object) info1.Name, (object) info2.Name)}<br>";
        }
        else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
        {
          if (info1.SpouseFollowerID != -1 && info1.SpouseFollowerID == info2.SpouseFollowerID)
          {
            TMP_Text specialEffect = this.specialEffect;
            specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Married"), (object) info1.Name, (object) info2.Name)}<br>";
          }
          else
          {
            TMP_Text specialEffect = this.specialEffect;
            specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Lovers"), (object) info1.Name, (object) info2.Name)}<br>";
          }
        }
        else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
        {
          if (info1.SpouseFollowerID != -1 && info1.SpouseFollowerID == info2.SpouseFollowerID)
          {
            TMP_Text specialEffect = this.specialEffect;
            specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/UnhappilyMarried"), (object) info1.Name, (object) info2.Name)}<br>";
          }
          else
          {
            TMP_Text specialEffect = this.specialEffect;
            specialEffect.text = $"{specialEffect.text}{string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Enemies"), (object) info1.Name, (object) info2.Name)}<br>";
          }
        }
      }
      if (FollowerBrainStats.IsNudism)
      {
        TMP_Text specialEffect = this.specialEffect;
        specialEffect.text = $"{specialEffect.text}<sprite name=\"icon_GoodTrait\"> {LocalizationManager.GetTranslation("DoctrineUpgradeSystem/Pleasure_Nudist")}<br>";
      }
      this.specialEffect.gameObject.SetActive(!string.IsNullOrEmpty(this.specialEffect.text));
    }
    else
      this.specialEffect.gameObject.SetActive(false);
    for (int index1 = this.parent1TraitSlots.Count - 1; index1 >= 0; --index1)
    {
      if (this.parent1TraitSlots[index1].transform.childCount > 0)
      {
        for (int index2 = this.parent1TraitSlots[index1].transform.childCount - 1; index2 >= 0; --index2)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.parent1TraitSlots[index1].transform.GetChild(index2).gameObject);
      }
    }
    for (int index3 = this.parent2TraitSlots.Count - 1; index3 >= 0; --index3)
    {
      if (this.parent2TraitSlots[index3].transform.childCount > 0)
      {
        for (int index4 = this.parent2TraitSlots[index3].transform.childCount - 1; index4 >= 0; --index4)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.parent2TraitSlots[index3].transform.GetChild(index4).gameObject);
      }
    }
    int index5 = 0;
    if (info1 != null)
    {
      for (int index6 = 0; index6 < info1.Traits.Count; ++index6)
      {
        if (!FollowerTrait.ExcludedFromMating.Contains(info1.Traits[index6]))
        {
          if (index5 >= this.parent1TraitSlots.Count)
          {
            Debug.LogError((object) "Too many traits in comparison to available slots count!");
            break;
          }
          IndoctrinationTraitItem indoctrinationTraitItem = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.parent1TraitSlots[index5].transform);
          ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(30f, 30f);
          indoctrinationTraitItem.transform.localScale = Vector3.one * 0.85f;
          indoctrinationTraitItem.Configure(info1.Traits[index6]);
          indoctrinationTraitItem.Selectable.OnSelected += (System.Action) (() =>
          {
            if (InputManager.General.MouseInputActive)
              return;
            menuController.DisableScrollRect();
          });
          ++index5;
        }
      }
    }
    int index7 = 0;
    if (info2 == null)
      return;
    for (int index8 = 0; index8 < info2.Traits.Count; ++index8)
    {
      if (!FollowerTrait.ExcludedFromMating.Contains(info2.Traits[index8]))
      {
        if (index7 >= this.parent2TraitSlots.Count)
        {
          Debug.LogError((object) "Too many traits in comparison to available slots count!");
          break;
        }
        IndoctrinationTraitItem indoctrinationTraitItem = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.parent2TraitSlots[index7].transform);
        ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(30f, 30f);
        indoctrinationTraitItem.transform.localScale = Vector3.one * 0.85f;
        indoctrinationTraitItem.Configure(info2.Traits[index8]);
        indoctrinationTraitItem.Selectable.OnSelected += (System.Action) (() =>
        {
          if (InputManager.General.MouseInputActive)
            return;
          menuController.DisableScrollRect();
        });
        ++index7;
      }
    }
  }
}
