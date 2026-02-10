// Decompiled with JetBrains decompiler
// Type: FleeceSelectionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public List<Button> _fleeceButtons;
  [Header("Text")]
  [SerializeField]
  public TextMeshProUGUI _titleText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public TextMeshProUGUI _keyText;
  [SerializeField]
  public TextMeshProUGUI _equippedText;
  public bool PlayGrowAndFade;
  public bool _hasInitialised;

  public override void OnShowStarted()
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

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  public new void OnDisable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  public void OnSelection(Selectable obj)
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

  public void OnSelectionChanged(Selectable arg1, Selectable arg2) => this.OnSelection(arg1);

  public void SetFleece(Selectable obj, FleeceMenuIcon f)
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
        int count = DataManager.Instance.UnlockedFleeces.Count;
        foreach (int unlockedFleece in DataManager.Instance.UnlockedFleeces)
        {
          if (PlayerFleeceManager.NOT_INCLUDED_IN_ACHIEVEMENT.Contains((PlayerFleeceManager.FleeceType) unlockedFleece))
            --count;
        }
        if (count >= 12)
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

  public void Equip(int FleeceNumber)
  {
    this.PlayGrowAndFade = true;
    DataManager.Instance.PlayerFleece = FleeceNumber;
    foreach (Vector2 customisedFleeceOption in DataManager.Instance.CustomisedFleeceOptions)
    {
      if ((double) customisedFleeceOption.x == (double) FleeceNumber)
        DataManager.Instance.PlayerVisualFleece = (int) customisedFleeceOption.y;
    }
    ObjectiveManager.CompleteShowFleeceObjective();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.isLamb)
      {
        if (DataManager.Instance.PlayerVisualFleece == 1003)
          player.SetSkin();
        else
          player.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerVisualFleece.ToString());
      }
    }
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    if (!this.PlayGrowAndFade)
      return;
    PlayerFarming.Instance.growAndFade.Play();
  }
}
