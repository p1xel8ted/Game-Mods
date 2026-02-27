// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.UIBuildMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class UIBuildMenuController : UIMenuBase
{
  public Action<StructureBrain.TYPES> OnBuildingChosen;
  [Header("Build Menu")]
  [SerializeField]
  private BuildMenuTabNavigatorBase _tabNavigatorBase;
  [SerializeField]
  private FollowerCategory _followerCategory;
  [SerializeField]
  private FaithCategory _faithCategory;
  [SerializeField]
  private AestheticCategory _aestheticCategory;
  [SerializeField]
  private BuildInfoCardController _infoCardController;
  [Header("Prompts")]
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  private GameObject _editBuildingsText;
  private bool _didCancel;
  private StructureBrain.TYPES _revealingStructure;

  public override void Awake()
  {
    base.Awake();
    DataManager.Instance.Alerts.Structures.CheckStructureUnlocked();
    FollowerCategory followerCategory = this._followerCategory;
    followerCategory.OnBuildingChosen = followerCategory.OnBuildingChosen + new Action<StructureBrain.TYPES>(this.ChosenBuilding);
    FaithCategory faithCategory = this._faithCategory;
    faithCategory.OnBuildingChosen = faithCategory.OnBuildingChosen + new Action<StructureBrain.TYPES>(this.ChosenBuilding);
    AestheticCategory aestheticCategory = this._aestheticCategory;
    aestheticCategory.OnBuildingChosen = aestheticCategory.OnBuildingChosen + new Action<StructureBrain.TYPES>(this.ChosenBuilding);
  }

  public void Show(StructureBrain.TYPES structureToReveal)
  {
    this._revealingStructure = structureToReveal;
    if (FollowerCategory.AllStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 0;
    else if (FaithCategory.AllStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 1;
    else if (AestheticCategory.AllStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 2;
    this.Show();
  }

  protected override IEnumerator DoShowAnimation()
  {
    UIBuildMenuController buildMenuController = this;
    if (buildMenuController._revealingStructure != StructureBrain.TYPES.NONE)
    {
      buildMenuController._controlPrompts.HideAcceptButton();
      buildMenuController._controlPrompts.HideCancelButton();
      buildMenuController._editBuildingsText.SetActive(false);
      buildMenuController._tabNavigatorBase.RemoveAllAlerts();
      buildMenuController._tabNavigatorBase.SetNavigationVisibility(false);
      if (buildMenuController._tabNavigatorBase.CurrentMenu is BuildMenuCategory buildMenuCategory)
      {
        BuildMenuItem target = (BuildMenuItem) null;
        foreach (BuildMenuItem buildItem in buildMenuCategory.BuildItems)
        {
          if (buildItem.Structure == buildMenuController._revealingStructure)
          {
            target = buildItem;
            target.ForceLockedState();
          }
          else if (!buildItem.Locked)
            buildItem.ForceIncognitoState();
        }
        buildMenuCategory.ScrollRect.vertical = false;
        MonoSingleton<UINavigatorNew>.Instance.Clear();
        buildMenuController.SetActiveStateForMenu(false);
        // ISSUE: reference to a compiler-generated method
        yield return (object) buildMenuController.\u003C\u003En__0();
        yield return (object) new WaitForSecondsRealtime(0.1f);
        UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
        yield return (object) buildMenuCategory.ScrollRect.DoScrollTo(target.RectTransform);
        yield return (object) new WaitForSecondsRealtime(0.1f);
        yield return (object) target.DoUnlock();
        yield return (object) new WaitForSecondsRealtime(0.1f);
        buildMenuController._infoCardController.ShowCardWithParam(target.Structure);
        yield return (object) new WaitForSecondsRealtime(0.1f);
        buildMenuController._controlPrompts.ShowAcceptButton();
        while (!InputManager.UI.GetAcceptButtonDown())
          yield return (object) null;
        buildMenuController._controlPrompts.HideAcceptButton();
        buildMenuController.Hide();
        target = (BuildMenuItem) null;
      }
      buildMenuCategory = (BuildMenuCategory) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      yield return (object) buildMenuController.\u003C\u003En__0();
    }
  }

  private void ChosenBuilding(StructureBrain.TYPES structure)
  {
    Action<StructureBrain.TYPES> onBuildingChosen = this.OnBuildingChosen;
    if (onBuildingChosen != null)
      onBuildingChosen(structure);
    this.Hide();
  }

  private void Update()
  {
    if (this._canvasGroup.interactable && DataManager.Instance.HasBuiltShrine1 && DataManager.Instance.HasBuiltTemple1 && InputManager.UI.GetEditBuildingsButtonDown())
    {
      Action<StructureBrain.TYPES> onBuildingChosen = this.OnBuildingChosen;
      if (onBuildingChosen != null)
        onBuildingChosen(StructureBrain.TYPES.EDIT_BUILDINGS);
      this.Hide();
    }
    this._editBuildingsText.SetActive(DataManager.Instance.HasBuiltShrine1 && DataManager.Instance.HasBuiltTemple1 && this._revealingStructure == StructureBrain.TYPES.NONE);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this._didCancel = true;
    this.Hide();
  }

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
    this._followerCategory.CanvasGroup.interactable = false;
    this._faithCategory.CanvasGroup.interactable = false;
    this._aestheticCategory.CanvasGroup.interactable = false;
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    Interactor.CurrentInteraction = (Interaction) null;
    Interactor.PreviousInteraction = (Interaction) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [Serializable]
  public enum Category
  {
    Follower,
    Faith,
    Aesthetic,
  }
}
