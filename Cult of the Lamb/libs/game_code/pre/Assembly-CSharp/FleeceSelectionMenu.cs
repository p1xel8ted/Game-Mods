// Decompiled with JetBrains decompiler
// Type: FleeceSelectionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class FleeceSelectionMenu : UISubmenuBase
{
  [Header("Buttons")]
  [SerializeField]
  private List<Button> _fleeceButtons;
  [Header("Text")]
  [SerializeField]
  private TextMeshProUGUI _titleText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private TextMeshProUGUI _keyText;
  [SerializeField]
  private TextMeshProUGUI _equippedText;
  private bool PlayGrowAndFade;
  private bool _hasInitialised;

  protected override void OnShowStarted()
  {
    if (this._hasInitialised)
      return;
    int index = -1;
    while (++index < this._fleeceButtons.Count)
    {
      Button b = this._fleeceButtons[index];
      FleeceMenuIcon f = b.GetComponent<FleeceMenuIcon>();
      f.FleeceNumber = index;
      f.Init();
      b.onClick.AddListener((UnityAction) (() => this.SetFleece((Selectable) b, f)));
      if (index == DataManager.Instance.PlayerFleece)
        this.OverrideDefault((Selectable) b);
    }
    this._keyText.text = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN).ToString() + "x <sprite name=\"icon_key\">";
    this._hasInitialised = true;
  }

  public static string LocalisedName(string type)
  {
    return LocalizationManager.GetTranslation($"TarotCards/Fleece{type}/Name");
  }

  public static string LocalisedDescription(string type)
  {
    return LocalizationManager.GetTranslation($"TarotCards/Fleece{type}/Description");
  }

  private void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  private void OnDisable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  private void OnSelection(Selectable obj)
  {
    FleeceMenuIcon component = obj.GetComponent<FleeceMenuIcon>();
    this._titleText.text = FleeceSelectionMenu.LocalisedName(component.FleeceNumber.ToString());
    this._descriptionText.text = FleeceSelectionMenu.LocalisedDescription(component.FleeceNumber.ToString());
    switch (component.State)
    {
      case FleeceMenuIcon.States.Locked:
        this._equippedText.text = ScriptLocalization.Interactions.Requires + " <sprite name=\"icon_key\"> x1";
        break;
      case FleeceMenuIcon.States.Unlocked:
        this._equippedText.text = ScriptLocalization.Interactions.Equip;
        break;
      case FleeceMenuIcon.States.Available:
        this._equippedText.text = ScriptLocalization.UpgradeSystem.Unlock + " <sprite name=\"icon_key\"> x1";
        break;
      case FleeceMenuIcon.States.Equipped:
        this._equippedText.text = ScriptLocalization.Interactions.Equipped;
        break;
    }
  }

  private void OnSelectionChanged(Selectable arg1, Selectable arg2) => this.OnSelection(arg1);

  private void SetFleece(Selectable obj, FleeceMenuIcon f)
  {
    switch (f.State)
    {
      case FleeceMenuIcon.States.Locked:
        f.Container.DOShakePosition(0.5f, new Vector3(10f, 0.0f), randomness: 0.0f).SetUpdate<Tweener>(true);
        break;
      case FleeceMenuIcon.States.Unlocked:
        this.Equip(f.FleeceNumber);
        break;
      case FleeceMenuIcon.States.Available:
        Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN, -1);
        this._keyText.text = Inventory.TempleKeys.ToString() + "x <sprite name=\"icon_key\">";
        if (!DataManager.Instance.UnlockedFleeces.Contains(f.FleeceNumber))
          DataManager.Instance.UnlockedFleeces.Add(f.FleeceNumber);
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("UNLOCK_TUNIC"));
        if (DataManager.Instance.UnlockedFleeces.Count >= 6)
          AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("UNLOCK_ALL_TUNICS"));
        this.Equip(f.FleeceNumber);
        break;
    }
    foreach (Component fleeceButton in this._fleeceButtons)
      fleeceButton.GetComponent<FleeceMenuIcon>().Init();
    this.OnSelection(obj);
    if (f.State != FleeceMenuIcon.States.Equipped)
      return;
    f.transform.localScale = Vector3.one * 1.5f;
    f.transform.DOKill();
    f.transform.DOScale(Vector3.one * 1.1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  private void Equip(int FleeceNumber)
  {
    this.PlayGrowAndFade = true;
    DataManager.Instance.PlayerFleece = FleeceNumber;
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerFleece.ToString());
  }

  protected override void OnHideCompleted()
  {
    base.OnHideCompleted();
    if (!this.PlayGrowAndFade)
      return;
    PlayerFarming.Instance.growAndFade.Play();
  }
}
