// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.UIBuildMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class UIBuildMenuController : UIMenuBase
{
  public Action<StructureBrain.TYPES> OnBuildingChosen;
  [Header("Build Menu")]
  [SerializeField]
  public BuildMenuTabNavigatorBase _tabNavigatorBase;
  [SerializeField]
  public FollowerCategory _followerCategory;
  [SerializeField]
  public FaithCategory _faithCategory;
  [SerializeField]
  public AestheticCategory _aestheticCategory;
  [SerializeField]
  public MajorDLCCategory _majorDLCCategory;
  [SerializeField]
  public BuildInfoCardController _infoCardController;
  [SerializeField]
  public GameObject _dlcButton;
  [SerializeField]
  public GameObject _dlcAlert;
  [Header("Prompts")]
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public GameObject _editBuildingsText;
  public bool _didCancel;
  public StructureBrain.TYPES _revealingStructure;
  public List<StructureBrain.TYPES> _revealingStructures = new List<StructureBrain.TYPES>();

  public bool DidCancel => this._didCancel;

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
    MajorDLCCategory majorDlcCategory = this._majorDLCCategory;
    majorDlcCategory.OnBuildingChosen = majorDlcCategory.OnBuildingChosen + new Action<StructureBrain.TYPES>(this.ChosenBuilding);
    this._dlcButton.gameObject.SetActive(DataManager.Instance.OnboardedDLCBuildMenu);
  }

  public void Show(StructureBrain.TYPES structureToReveal)
  {
    this._revealingStructure = structureToReveal;
    if (FollowerCategory.AllStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 0;
    else if (FaithCategory.AllStructures().Contains(structureToReveal) || FaithCategory.SinStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 1;
    else if (AestheticCategory.AllStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 2;
    else if (MajorDLCCategory.AllStructures().Contains(structureToReveal))
      this._tabNavigatorBase.DefaultTabIndex = 3;
    this._tabNavigatorBase.ClearPersistentTab();
    this.Show();
  }

  public void Show(List<StructureBrain.TYPES> structuresToReveal)
  {
    this._revealingStructure = structuresToReveal[0];
    this._revealingStructures = structuresToReveal;
    if (FollowerCategory.AllStructures().Contains(structuresToReveal[0]))
      this._tabNavigatorBase.DefaultTabIndex = 0;
    else if (FaithCategory.AllStructures().Contains(structuresToReveal[0]) || FaithCategory.SinStructures().Contains(structuresToReveal[0]))
      this._tabNavigatorBase.DefaultTabIndex = 1;
    else if (AestheticCategory.AllStructures().Contains(structuresToReveal[0]))
      this._tabNavigatorBase.DefaultTabIndex = 2;
    else if (MajorDLCCategory.AllStructures().Contains(structuresToReveal[0]))
      this._tabNavigatorBase.DefaultTabIndex = 3;
    this._tabNavigatorBase.ClearPersistentTab();
    this.Show();
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    AudioManager.Instance.SetBuildSnapshotEnabled(true);
    if (DataManager.Instance.OnboardedDLCBuildMenu || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Furnace))
      return;
    this.StartCoroutine((IEnumerator) this.MajorDLCSequence());
  }

  public override IEnumerator DoShowAnimation()
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
        if (buildMenuController._revealingStructures.Count == 0)
          buildMenuController._revealingStructures.Add(buildMenuController._revealingStructure);
        if (buildMenuCategory is AestheticCategory aestheticCategory1 && DataManager.DecorationsForType(DataManager.DecorationType.Major_DLC).Contains(buildMenuController._revealingStructures[0]))
          aestheticCategory1.ShowMajorDLCCategory();
        if (buildMenuCategory is AestheticCategory aestheticCategory2 && DataManager.DecorationsForType(DataManager.DecorationType.Woolhaven).Contains(buildMenuController._revealingStructures[0]))
          aestheticCategory2.ShowMajorDLCWoolhavenCategory();
        if (buildMenuCategory is AestheticCategory aestheticCategory3 && DataManager.DecorationsForType(DataManager.DecorationType.Wolf).Contains(buildMenuController._revealingStructures[0]))
          aestheticCategory3.ShowMajorDLCEwefallCategory();
        if (buildMenuCategory is AestheticCategory aestheticCategory4 && DataManager.DecorationsForType(DataManager.DecorationType.Rot).Contains(buildMenuController._revealingStructures[0]))
          aestheticCategory4.ShowMajorDLCRotCategory();
        buildMenuCategory.ScrollRect.vertical = false;
        MonoSingleton<UINavigatorNew>.Instance.Clear();
        buildMenuController.SetActiveStateForMenu(false);
        yield return (object) buildMenuController.\u003C\u003En__0();
        yield return (object) new WaitForSecondsRealtime(0.1f);
        for (int i = 0; i < buildMenuController._revealingStructures.Count; ++i)
        {
          foreach (BuildMenuItem buildItem in buildMenuCategory.BuildItems)
          {
            if (buildItem.Structure == buildMenuController._revealingStructures[i])
              target = buildItem;
          }
          UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
          yield return (object) buildMenuCategory.ScrollRect.DoScrollTo(target.RectTransform);
          yield return (object) new WaitForSecondsRealtime(0.1f);
          yield return (object) target.DoUnlock();
        }
        yield return (object) new WaitForSecondsRealtime(0.1f);
        buildMenuController._infoCardController.ShowCardWithParam(target.Structure);
        yield return (object) new WaitForSecondsRealtime(0.1f);
        buildMenuController._controlPrompts.ShowAcceptButton();
        buildMenuController._controlPrompts.ShowCancelButton();
        while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          yield return (object) null;
        buildMenuController._controlPrompts.HideAcceptButton();
        buildMenuController._controlPrompts.HideCancelButton();
        buildMenuController.Hide();
        target = (BuildMenuItem) null;
      }
      buildMenuCategory = (BuildMenuCategory) null;
    }
    else
      yield return (object) buildMenuController.\u003C\u003En__0();
  }

  public void ChosenBuilding(StructureBrain.TYPES structure)
  {
    Action<StructureBrain.TYPES> onBuildingChosen = this.OnBuildingChosen;
    if (onBuildingChosen != null)
      onBuildingChosen(structure);
    this.Hide();
  }

  public void Update()
  {
    if (this._canvasGroup.interactable && DataManager.Instance.HasBuiltShrine1 && DataManager.Instance.HasBuiltTemple1 && InputManager.UI.GetEditBuildingsButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
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

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
    this._followerCategory.CanvasGroup.interactable = false;
    this._faithCategory.CanvasGroup.interactable = false;
    this._aestheticCategory.CanvasGroup.interactable = false;
    this._majorDLCCategory.CanvasGroup.interactable = false;
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    PlayerFarming.Instance.interactor.CurrentInteraction = (Interaction) null;
    PlayerFarming.Instance.interactor.PreviousInteraction = (Interaction) null;
    AudioManager.Instance.SetBuildSnapshotEnabled(false);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator MajorDLCSequence()
  {
    UIBuildMenuController buildMenuController = this;
    buildMenuController.SetActiveStateForMenu(false);
    yield return (object) new WaitForSecondsRealtime(1f);
    buildMenuController._dlcButton.gameObject.SetActive(true);
    buildMenuController._dlcButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f).SetUpdate<Tweener>(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/ui/buildmenu/woolhaven_tab_unlock");
    yield return (object) new WaitForSecondsRealtime(1f);
    buildMenuController._dlcAlert.gameObject.SetActive(true);
    buildMenuController._dlcAlert.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f).SetUpdate<Tweener>(true);
    DataManager.Instance.OnboardedDLCBuildMenu = true;
    yield return (object) new WaitForSecondsRealtime(1f);
    buildMenuController.SetActiveStateForMenu(true);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();

  [Serializable]
  public enum Category
  {
    Follower,
    Faith,
    Aesthetic,
    MajorDLC,
  }
}
