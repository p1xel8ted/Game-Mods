// Decompiled with JetBrains decompiler
// Type: Interaction_WoolShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_WoolShrine : Interaction
{
  [SerializeField]
  public GameObject[] unlocks;
  public const int INCREMENT_COST = 10;
  public const int MIN_COST = 10;
  public const int MAX_COST = 100;
  public const int MAX_LEVEL = 20;

  public int Level
  {
    get => DataManager.Instance.LambTownLevel;
    set => DataManager.Instance.LambTownLevel = value;
  }

  public int WoolGiven
  {
    get => DataManager.Instance.LambTownWoolGiven;
    set => DataManager.Instance.LambTownWoolGiven = value;
  }

  public void Awake()
  {
    for (int index = 0; index < this.unlocks.Length; ++index)
      this.unlocks[index].gameObject.SetActive(index < this.Level);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = LocalizationManager.GetTranslation("Interactions/DepositWool");
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) > 0)
    {
      this.playerFarming.indicator.ShowTopInfo($"{this.WoolGiven.ToString()}/{Mathf.Clamp((this.Level + 1) * 10, 10, 100).ToString()}");
      UIManager instance = MonoSingleton<UIManager>.Instance;
      PlayerFarming playerFarming = this.playerFarming;
      List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
      items.Add(InventoryItem.ITEM_TYPE.WOOL);
      ItemSelector.Params parameters = new ItemSelector.Params()
      {
        Key = "wool_shrine",
        Context = ItemSelector.Context.Add,
        Offset = new Vector2(0.0f, 150f),
        ShowEmpty = true,
        DontCache = true
      };
      UIItemSelectorOverlayController itemSelector = instance.ShowItemSelector(playerFarming, items, parameters);
      itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
      {
        Indicator indicator = this.playerFarming.indicator;
        int num = this.WoolGiven;
        string str1 = num.ToString();
        num = Mathf.Clamp((this.Level + 1) * 10, 10, 100);
        string str2 = num.ToString();
        string text = $"{str1}/{str2}";
        indicator.ShowTopInfo(text);
        Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.WOOL, -1);
        ResourceCustomTarget.Create(this.gameObject, this._playerFarming.transform.position, InventoryItem.ITEM_TYPE.WOOL, (System.Action) null);
        ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.WOOL);
        ++DataManager.Instance.LandResourcesGiven;
        if (DataManager.Instance.LandResourcesGiven < Mathf.Clamp((this.Level + 1) * 10, 10, 100))
          return;
        this.Interactable = false;
        this.HasChanged = true;
        itemSelector.Hide(true);
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.UpgradeIE());
      });
      UIItemSelectorOverlayController overlayController = itemSelector;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => { });
    }
    else
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    Indicator indicator = playerFarming.indicator;
    int num = this.WoolGiven;
    string str1 = num.ToString();
    num = Mathf.Clamp((this.Level + 1) * 10, 10, 100);
    string str2 = num.ToString();
    string text = $"{str1}/{str2}";
    indicator.ShowTopInfo(text);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public IEnumerator UpgradeIE()
  {
    if (DataManager.Instance.LambTownLevel + 1 < 20)
    {
      GameObject target = this.unlocks[DataManager.Instance.LambTownLevel];
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(target, 6f);
      yield return (object) new WaitForSeconds(2f);
      target.gameObject.SetActive(true);
      target.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
      ++DataManager.Instance.LambTownLevel;
      yield return (object) new WaitForSeconds(2f);
      GameManager.GetInstance().OnConversationEnd();
    }
  }
}
