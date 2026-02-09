// Decompiled with JetBrains decompiler
// Type: UITraitSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UITraitSelector : UIMenuBase
{
  [SerializeField]
  public IndoctrinationTraitItem traitItem;
  [SerializeField]
  public TMP_Text selectedText;
  [SerializeField]
  public TMP_Text selectedGoldenEggText;
  [SerializeField]
  public MMButton button;
  [SerializeField]
  public TraitInfoCardController infoCardController;
  [SerializeField]
  public GameObject goldenTraitsContainer;
  [SerializeField]
  public GameObject goldenContainer;
  [SerializeField]
  public TMP_Text promptText;
  [SerializeField]
  public TMP_Text parent1Header;
  [SerializeField]
  public TMP_Text parent2Header;
  [SerializeField]
  public SkeletonGraphic parent1Spine;
  [SerializeField]
  public SkeletonGraphic parent2Spine;
  [SerializeField]
  public List<GameObject> parent1TraitSlots;
  [SerializeField]
  public List<GameObject> parent2TraitSlots;
  [SerializeField]
  public List<GameObject> randomTaitSlots;
  [SerializeField]
  public List<GameObject> selectedTaitSlots;
  public List<FollowerTrait.TraitType> randomTraits = new List<FollowerTrait.TraitType>();
  public List<FollowerTrait.TraitType> selectedFollowerTraits = new List<FollowerTrait.TraitType>();
  public List<IndoctrinationTraitItem> parentTraitItems = new List<IndoctrinationTraitItem>();
  public List<IndoctrinationTraitItem> randomTraitsItems = new List<IndoctrinationTraitItem>();
  public List<IndoctrinationTraitItem> selectedTraitsItems = new List<IndoctrinationTraitItem>();
  public Action<List<FollowerTrait.TraitType>> TraitsChosen;
  public const int DEFAULT_AMOUNT = 2;
  public const int GOLDEN_AMOUNT = 2;
  public bool isGoldenEgg;
  public List<FollowerTrait.TraitType> parent1Traits;
  public List<FollowerTrait.TraitType> parent2Traits;
  public string parent1Skin;
  public string parent2Skin;

  public void Configure(
    FollowerInfo parent1,
    FollowerInfo parent2,
    List<FollowerTrait.TraitType> p1traits,
    List<FollowerTrait.TraitType> p2traits,
    List<FollowerTrait.TraitType> randomTraits,
    StructuresData.EggData eggData)
  {
    this.randomTraits = randomTraits;
    this.isGoldenEgg = eggData.Golden;
    this.parent1Traits = p1traits;
    this.parent2Traits = p2traits;
    this.parent1Skin = parent1.SkinName;
    this.parent2Skin = parent2.SkinName;
    this.selectedText.text = "0/" + 2.ToString();
    this.selectedGoldenEggText.text = "0/" + 2.ToString();
    this.parent1Spine.ConfigureFollower(parent1);
    this.parent2Spine.ConfigureFollower(parent2);
    this.parent1Header.text = string.Format(LocalizationManager.GetTranslation("UI/ParentsTrait"), (object) parent1.Name);
    this.parent2Header.text = string.Format(LocalizationManager.GetTranslation("UI/ParentsTrait"), (object) parent2.Name);
    this.ConfigurePureBloodTraits();
    this.ConfigureYngyaChildrenTraits(parent1.ID, parent2.ID);
    this.ConfigureSnowmanChildrenTraits(parent1, parent2);
    this.ConfigureRottedChildrenTraits(parent1, parent2, eggData);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-1.5f, 1f));
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    for (int index = 0; index < Mathf.Clamp(this.parent1Traits.Count, 0, this.parent1TraitSlots.Count); ++index)
    {
      IndoctrinationTraitItem item = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.parent1TraitSlots[index].transform);
      ((RectTransform) item.transform).anchoredPosition = new Vector2(40f, 40f);
      item.Configure(this.parent1Traits[index]);
      this.parentTraitItems.Add(item);
      MMButton component = item.GetComponent<MMButton>();
      component.onClick.AddListener((UnityAction) (() =>
      {
        if (this.selectedFollowerTraits.Contains(item.TraitType))
          return;
        this.OnTraitSelected(item);
      }));
      component.OnSelected += (System.Action) (() =>
      {
        this.infoCardController.ShowCardWithParam(item.TraitType);
        this.OnTraitHighlighted(item);
      });
    }
    for (int index = 0; index < Mathf.Clamp(this.parent2Traits.Count, 0, this.parent2TraitSlots.Count); ++index)
    {
      IndoctrinationTraitItem item = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.parent2TraitSlots[index].transform);
      ((RectTransform) item.transform).anchoredPosition = new Vector2(40f, 40f);
      item.Configure(this.parent2Traits[index]);
      this.parentTraitItems.Add(item);
      MMButton component = item.GetComponent<MMButton>();
      component.onClick.AddListener((UnityAction) (() =>
      {
        if (this.selectedFollowerTraits.Contains(item.TraitType))
          return;
        this.OnTraitSelected(item);
      }));
      component.OnSelected += (System.Action) (() =>
      {
        this.infoCardController.ShowCardWithParam(item.TraitType);
        this.OnTraitHighlighted(item);
      });
    }
    for (int index = 0; index < randomTraits.Count; ++index)
    {
      IndoctrinationTraitItem item = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.randomTaitSlots[index].transform);
      ((RectTransform) item.transform).anchoredPosition = new Vector2(40f, 40f);
      this.randomTraitsItems.Add(item);
      item.Configure(randomTraits[index]);
      MMButton component = item.GetComponent<MMButton>();
      component.onClick.AddListener((UnityAction) (() =>
      {
        if (this.selectedFollowerTraits.Contains(item.TraitType))
          return;
        this.OnTraitSelected(item);
      }));
      component.OnSelected += (System.Action) (() =>
      {
        this.infoCardController.ShowCardWithParam(item.TraitType);
        this.OnTraitHighlighted(item);
      });
    }
    List<IndoctrinationTraitItem> indoctrinationTraitItemList = new List<IndoctrinationTraitItem>((IEnumerable<IndoctrinationTraitItem>) this.parentTraitItems);
    indoctrinationTraitItemList.AddRange((IEnumerable<IndoctrinationTraitItem>) this.randomTraitsItems);
    if (indoctrinationTraitItemList.Count == 0)
    {
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.button);
    }
    else
    {
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) indoctrinationTraitItemList[0].Button);
      this.infoCardController.ShowCardWithParam(indoctrinationTraitItemList[0].TraitType);
    }
    for (int index = 0; index < this.selectedTaitSlots.Count; ++index)
      this.selectedTaitSlots[index].gameObject.SetActive(index < 2 || this.isGoldenEgg);
    this.goldenTraitsContainer.gameObject.SetActive(this.isGoldenEgg);
    this.goldenContainer.gameObject.SetActive(this.isGoldenEgg);
    if (parent1.Traits.Contains(FollowerTrait.TraitType.Zombie) || parent2.Traits.Contains(FollowerTrait.TraitType.Zombie))
      this.CreateNewTraitItem(FollowerTrait.TraitType.Zombie);
    if (DataManager.Instance.PalworldSkins.Contains<string>(this.parent1Skin) && DataManager.Instance.PalworldSkins.Contains<string>(this.parent2Skin) && !DataManager.Instance.FollowerSkinsUnlocked.Contains("PalworldTwo") && this.parent1Skin != this.parent2Skin)
      this.CreateNewTraitItem(FollowerTrait.TraitType.Insomniac);
    this.button.onClick.AddListener((UnityAction) (() =>
    {
      AudioManager.Instance.PlayOneShot("event:/building/mating_tent/birds_and_bees");
      this.Hide();
    }));
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    this.button.Interactable = this.selectedFollowerTraits.Count >= 2;
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnTraitHighlighted(IndoctrinationTraitItem item)
  {
    if (this.selectedTraitsItems.Contains(item))
      this.promptText.text = ScriptLocalization.UI_PhotoMode.Remove;
    else
      this.promptText.text = ScriptLocalization.UI_Generic.Accept;
  }

  public void OnTraitSelected(IndoctrinationTraitItem item)
  {
    if (this.selectedFollowerTraits.Count >= 2 && !this.isGoldenEgg || this.selectedFollowerTraits.Count >= 4 && this.isGoldenEgg)
    {
      if (this.isGoldenEgg)
      {
        this.selectedGoldenEggText.transform.DOKill();
        this.selectedGoldenEggText.transform.localPosition = new Vector3(0.0f, this.selectedGoldenEggText.transform.localPosition.y, this.selectedGoldenEggText.transform.localPosition.z);
        this.selectedGoldenEggText.transform.DOShakePosition(1f, new Vector3(10f, 0.0f));
      }
      else
      {
        this.selectedText.transform.DOKill();
        this.selectedText.transform.localPosition = new Vector3(0.0f, this.selectedText.transform.localPosition.y, this.selectedText.transform.localPosition.z);
        this.selectedText.transform.DOShakePosition(1f, new Vector3(10f, 0.0f));
      }
    }
    else
    {
      item.Button.Confirmable = false;
      this.CreateNewTraitItem(item.TraitType);
    }
  }

  public void CreateNewTraitItem(FollowerTrait.TraitType traitType)
  {
    IndoctrinationTraitItem newItem = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.selectedTaitSlots[this.selectedFollowerTraits.Count].transform);
    ((RectTransform) newItem.transform).anchoredPosition = new Vector2(40f, 40f);
    newItem.Configure(traitType);
    MMButton component = newItem.GetComponent<MMButton>();
    component.onClick.AddListener((UnityAction) (() => this.OnTraitDeselected(newItem)));
    component.OnSelected += (System.Action) (() =>
    {
      this.infoCardController.ShowCardWithParam(newItem.TraitType);
      this.OnTraitHighlighted(newItem);
    });
    this.selectedFollowerTraits.Add(traitType);
    this.selectedTraitsItems.Add(newItem);
    TMP_Text selectedText = this.selectedText;
    int num1 = Mathf.Clamp(this.selectedFollowerTraits.Count, 0, 2);
    string str1 = num1.ToString();
    num1 = 2;
    string str2 = num1.ToString();
    string str3 = $"{str1}/{str2}";
    selectedText.text = str3;
    TMP_Text selectedGoldenEggText = this.selectedGoldenEggText;
    int num2 = Mathf.Clamp(this.selectedFollowerTraits.Count - 2, 0, 4);
    string str4 = num2.ToString();
    num2 = 2;
    string str5 = num2.ToString();
    string str6 = $"{str4}/{str5}";
    selectedGoldenEggText.text = str6;
    this.button.Interactable = this.button.Confirmable = this.selectedFollowerTraits.Count >= 2;
    if (FollowerTrait.PureBloodTraits.Contains(traitType) || traitType == FollowerTrait.TraitType.Zombie || traitType == FollowerTrait.TraitType.Mutated || traitType == FollowerTrait.TraitType.FreezeImmune || traitType == FollowerTrait.TraitType.BornToTheRot || traitType == FollowerTrait.TraitType.MasterfulSnowman || traitType == FollowerTrait.TraitType.ShoddySnowman || traitType == FollowerTrait.TraitType.InfusibleSnowman)
      newItem.Button.Confirmable = false;
    if (traitType == FollowerTrait.TraitType.Insomniac && DataManager.Instance.PalworldSkins.Contains<string>(this.parent1Skin) && DataManager.Instance.PalworldSkins.Contains<string>(this.parent2Skin) && !DataManager.Instance.FollowerSkinsUnlocked.Contains("PalworldTwo") && this.parent1Skin != this.parent2Skin)
      newItem.Button.Confirmable = false;
    this.UpdateOtherTraits();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    Action<List<FollowerTrait.TraitType>> traitsChosen = this.TraitsChosen;
    if (traitsChosen != null)
      traitsChosen(this.selectedFollowerTraits);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
  }

  public void OnTraitDeselected(IndoctrinationTraitItem item)
  {
    foreach (IndoctrinationTraitItem parentTraitItem in this.parentTraitItems)
    {
      if (parentTraitItem.TraitType == item.TraitType)
      {
        parentTraitItem.Button.Confirmable = true;
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) parentTraitItem.Button);
      }
    }
    foreach (IndoctrinationTraitItem randomTraitsItem in this.randomTraitsItems)
    {
      if (randomTraitsItem.TraitType == item.TraitType)
      {
        randomTraitsItem.Button.Confirmable = true;
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) randomTraitsItem.Button);
      }
    }
    this.selectedFollowerTraits.Remove(item.TraitType);
    TMP_Text selectedText = this.selectedText;
    string str1 = Mathf.Clamp(this.selectedFollowerTraits.Count, 0, 2).ToString();
    int num = 2;
    string str2 = num.ToString();
    string str3 = $"{str1}/{str2}";
    selectedText.text = str3;
    TMP_Text selectedGoldenEggText = this.selectedGoldenEggText;
    num = Mathf.Clamp(this.selectedFollowerTraits.Count - 2, 0, 4);
    string str4 = num.ToString();
    num = 2;
    string str5 = num.ToString();
    string str6 = $"{str4}/{str5}";
    selectedGoldenEggText.text = str6;
    this.button.Interactable = this.button.Confirmable = this.selectedFollowerTraits.Count >= 2;
    this.UpdateOtherTraits();
    this.selectedTraitsItems.Remove(item);
    item.Button.onClick.RemoveAllListeners();
    UnityEngine.Object.Destroy((UnityEngine.Object) item.gameObject);
    for (int index = 0; index < this.selectedTraitsItems.Count; ++index)
    {
      this.selectedTraitsItems[index].transform.parent = this.selectedTaitSlots[index].transform;
      ((RectTransform) this.selectedTraitsItems[index].transform).anchoredPosition = new Vector2(40f, 40f);
    }
  }

  public void UpdateOtherTraits()
  {
    foreach (IndoctrinationTraitItem parentTraitItem in this.parentTraitItems)
    {
      parentTraitItem.Button.Confirmable = !this.selectedFollowerTraits.Contains(parentTraitItem.TraitType);
      for (int index = 0; index < FollowerTrait.ExclusiveTraits.Count; ++index)
      {
        KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> keyValuePair = FollowerTrait.ExclusiveTraits.ElementAt<KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType>>(index);
        if (parentTraitItem.TraitType == keyValuePair.Key || parentTraitItem.TraitType == keyValuePair.Value)
        {
          bool flag = !this.selectedFollowerTraits.Contains(parentTraitItem.TraitType);
          if (keyValuePair.Key == parentTraitItem.TraitType && this.selectedFollowerTraits.Contains(keyValuePair.Value) || keyValuePair.Value == parentTraitItem.TraitType && this.selectedFollowerTraits.Contains(keyValuePair.Key))
            flag = false;
          parentTraitItem.Button.Confirmable = flag;
          break;
        }
      }
    }
    foreach (IndoctrinationTraitItem randomTraitsItem in this.randomTraitsItems)
    {
      randomTraitsItem.Button.Confirmable = !this.selectedFollowerTraits.Contains(randomTraitsItem.TraitType);
      for (int index = 0; index < FollowerTrait.ExclusiveTraits.Count; ++index)
      {
        KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> keyValuePair = FollowerTrait.ExclusiveTraits.ElementAt<KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType>>(index);
        if (randomTraitsItem.TraitType == keyValuePair.Key || randomTraitsItem.TraitType == keyValuePair.Value)
        {
          bool flag = !this.selectedFollowerTraits.Contains(randomTraitsItem.TraitType);
          if (keyValuePair.Key == randomTraitsItem.TraitType && this.selectedFollowerTraits.Contains(keyValuePair.Value) || keyValuePair.Value == randomTraitsItem.TraitType && this.selectedFollowerTraits.Contains(keyValuePair.Key))
            flag = false;
          randomTraitsItem.Button.Confirmable = flag;
          break;
        }
      }
    }
  }

  public void ConfigurePureBloodTraits()
  {
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) this.parent1Traits);
    traitTypeList.AddRange((IEnumerable<FollowerTrait.TraitType>) this.parent2Traits);
    FollowerTrait.TraitType traitType = FollowerTrait.TraitType.None;
    for (int index = traitTypeList.Count - 1; index >= 0; --index)
    {
      if (traitTypeList[index] == FollowerTrait.TraitType.PureBlood_1)
        traitType = FollowerTrait.TraitType.PureBlood_2;
      else if (traitTypeList[index] == FollowerTrait.TraitType.PureBlood_2)
        traitType = FollowerTrait.TraitType.PureBlood_3;
      else if (traitTypeList[index] == FollowerTrait.TraitType.PureBlood_3)
        traitType = FollowerTrait.TraitType.ChosenOne;
      if (FollowerTrait.PureBloodTraits.Contains(traitTypeList[index]))
      {
        this.parent1Traits.Remove(traitTypeList[index]);
        this.parent2Traits.Remove(traitTypeList[index]);
        this.randomTraits.Remove(traitTypeList[index]);
      }
    }
    if (traitType == FollowerTrait.TraitType.ChosenOne)
    {
      if (DataManager.Instance.HasProducedChosenOne)
      {
        traitType = FollowerTrait.TraitType.None;
      }
      else
      {
        List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers);
        followerInfoList.AddRange((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
        foreach (FollowerTrait.TraitType pureBloodTrait in FollowerTrait.PureBloodTraits)
        {
          foreach (FollowerInfo followerInfo in followerInfoList)
          {
            if (followerInfo.Traits.Contains(pureBloodTrait))
            {
              followerInfo.Traits.Remove(pureBloodTrait);
              followerInfo.Traits.Add(FollowerTrait.TraitType.PureBlood);
            }
          }
        }
        DataManager.Instance.HasProducedChosenOne = true;
      }
    }
    if (traitType == FollowerTrait.TraitType.None)
      return;
    this.CreateNewTraitItem(traitType);
  }

  public void ConfigureYngyaChildrenTraits(int parent1ID, int parent2ID)
  {
    if (parent1ID != 100007 && parent2ID != 100007 || FollowerManager.UniqueFollowerIDs.Contains(parent1ID) && FollowerManager.UniqueFollowerIDs.Contains(parent2ID))
      return;
    this.CreateNewTraitItem(FollowerTrait.TraitType.FreezeImmune);
  }

  public void ConfigureSnowmanChildrenTraits(FollowerInfo parent1, FollowerInfo parent2)
  {
    if (!parent1.IsSnowman || !parent2.IsSnowman)
      return;
    this.CreateNewTraitItem(FollowerTrait.TraitType.InfusibleSnowman);
    this.CreateNewTraitItem(FollowerTrait.TraitType.MasterfulSnowman);
  }

  public void ConfigureRottedChildrenTraits(
    FollowerInfo parent1,
    FollowerInfo parent2,
    StructuresData.EggData eggData)
  {
    if (parent1.Traits.Contains(FollowerTrait.TraitType.Mutated) || parent2.Traits.Contains(FollowerTrait.TraitType.Mutated))
      this.CreateNewTraitItem(FollowerTrait.TraitType.Mutated);
    if (!eggData.RottingUnique)
      return;
    this.CreateNewTraitItem(FollowerTrait.TraitType.BornToTheRot);
  }

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__29_0()
  {
    AudioManager.Instance.PlayOneShot("event:/building/mating_tent/birds_and_bees");
    this.Hide();
  }
}
