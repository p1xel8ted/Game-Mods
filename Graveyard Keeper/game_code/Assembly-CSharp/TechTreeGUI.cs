// Decompiled with JetBrains decompiler
// Type: TechTreeGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using SmartPools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TechTreeGUI : BaseGameGUI
{
  public TechTreeGUIItem _item_prefab;
  public TechBranchGUIItem _branch_item_prefab;
  public TechTreeGUIItem _gamepad_focused_slot;
  public int _cur_branch = 1;
  public List<TechTreeGUIItem> _items = new List<TechTreeGUIItem>();
  public List<TechConnector> _connectors = new List<TechConnector>();
  public List<TechBranchGUIItem> _branches = new List<TechBranchGUIItem>();
  public GameObject content;
  public UITable branches_grid;
  public UILabel txt_tech_points;
  public TechConnector tech_connector_prefab;
  public UIScrollView _scroll_view;
  public const int TECH_SIZE_X = 165;
  public const int TECH_SIZE_Y = 74;
  public int _max_branch_in_balance = -1;
  public Color clr_line_available;
  public Color clr_line_purchased;
  public Color clr_line_not_available;

  public int current_branch => this._cur_branch;

  public override void Init()
  {
    base.Init();
    if (this._max_branch_in_balance == -1)
    {
      foreach (TechDefinition techDefinition in GameBalance.me.techs_data)
      {
        if (techDefinition.branch_type > this._max_branch_in_balance)
          this._max_branch_in_balance = techDefinition.branch_type;
      }
    }
    this._scroll_view = this.GetComponentInChildren<UIScrollView>(true);
    this._item_prefab = this.GetComponentInChildren<TechTreeGUIItem>(true);
    this._item_prefab.InitPrefab();
    this._branch_item_prefab = this.GetComponentInChildren<TechBranchGUIItem>(true);
    this._branch_item_prefab.gameObject.SetActive(false);
    this.tech_connector_prefab = this.GetComponentInChildren<TechConnector>(true);
    SmartPooler.CreatePool<TechConnector>(this.tech_connector_prefab, 100);
    this.tech_connector_prefab.gameObject.SetActive(false);
  }

  public void OpenTech(string tech_id)
  {
    TechDefinition data = GameBalance.me.GetData<TechDefinition>(tech_id);
    if (data != null)
      this._cur_branch = data.branch_type;
    GUIElements.me.game_gui.Open();
    GUIElements.me.game_gui.SelectTab(GameGUI.TabType.Techs);
  }

  public override void Open()
  {
    this._gamepad_focused_slot = (TechTreeGUIItem) null;
    base.Open();
    this.Draw();
    if (BaseGUI.for_gamepad)
    {
      this.gamepad_controller.ReinitItems(false);
      int index = this._cur_branch - 1;
      if (index < 0)
        index = 0;
      else if (index >= this._branches.Count)
        index = this._branches.Count - 1;
      this.gamepad_controller.SetFocusedItem(this._branches[index].GetComponent<GamepadNavigationItem>());
    }
    this.branches_grid.Reposition();
    this.branches_grid.repositionNow = true;
  }

  public override void Update()
  {
    base.Update();
    this._scroll_view.UpdatePosition();
  }

  public new void LateUpdate()
  {
    if (!this._scroll_view.RestrictWithinBounds(false))
      return;
    this._scroll_view.transform.DOKill();
  }

  public void ScrollViewReposition()
  {
    this._scroll_view.RestrictWithinBounds(true);
    this._scroll_view.ResetPosition();
  }

  public void DrawBranch(int tech_branch_id)
  {
    for (int index = 0; index < this._items.Count; ++index)
      NGUITools.Destroy((UnityEngine.Object) this._items[index].gameObject);
    for (int index = 0; index < this._connectors.Count; ++index)
      NGUITools.Destroy((UnityEngine.Object) this._connectors[index].gameObject);
    this._items.Clear();
    this._connectors.Clear();
    this.content.transform.localPosition = Vector3.zero;
    this._scroll_view.ResetPosition();
    float num1 = float.MinValue;
    float num2 = float.MaxValue;
    foreach (TechDefinition tech in GameBalance.me.techs_data)
    {
      if (tech.branch_type == tech_branch_id && tech.GetState() != TechDefinition.TechState.Invisible)
      {
        this._items.Add(this.CreateItem(tech));
        float y = tech.y;
        if ((double) y > (double) num1)
          num1 = y;
        if ((double) y < (double) num2)
          num2 = y;
      }
    }
    foreach (TechTreeGUIItem t1 in this._items)
    {
      TechDefinition.TechState techState = t1.GetTechState();
      foreach (TechDefinition child in t1.tech.children)
      {
        if (child.GetState() != TechDefinition.TechState.Invisible)
        {
          this._connectors.Add(TechConnector.Create(t1, this.GetTechByID(child.id)));
          this._connectors[this._connectors.Count - 1].SetState(techState);
        }
      }
    }
    this.txt_tech_points.text = PlayerComponent.GetTechPointsString("\n");
    this.ScrollViewReposition();
    float num3 = num1 - num2;
    int num4 = Mathf.RoundToInt(37f);
    Debug.Log((object) $"max_y = {num1.ToString()}, min_y = {num2.ToString()}, hgt = {num3.ToString()}");
    Vector3 localPosition = this.content.transform.localPosition;
    if ((double) num3 > 3.5)
    {
      if ((double) num2 > 0.5)
        localPosition.y = (float) (num4 + 5);
    }
    else if ((double) num3 < 4.5)
      localPosition.y = (float) -num4;
    localPosition.y = 2f;
    this.UpdateAllAnchors();
    this.content.transform.localPosition = (Vector3) new Vector2(localPosition.x, localPosition.y);
  }

  public TechTreeGUIItem GetTechByID(string id)
  {
    foreach (TechTreeGUIItem techById in this._items)
    {
      if (techById.tech_id == id)
        return techById;
    }
    Debug.LogError((object) ("Can't find tech with id = " + id));
    return (TechTreeGUIItem) null;
  }

  public TechTreeGUIItem CreateItem(TechDefinition tech)
  {
    TechTreeGUIItem techTreeGuiItem = this._item_prefab.Copy<TechTreeGUIItem>();
    GJL.EnsureChildLabelsHasCorrectFont(techTreeGuiItem.gameObject, false);
    techTreeGuiItem.Draw(tech);
    techTreeGuiItem.transform.localPosition = new Vector3((float) (tech.x * 165), (float) (-(double) tech.y * 74.0));
    return techTreeGuiItem;
  }

  public void OnClickTech(TechDefinition tech)
  {
    Debug.Log((object) ("On Click tech " + tech.id), (UnityEngine.Object) this);
    if (UtilityStuff.ProcessClickTech(tech, (System.Action) (() =>
    {
      this.RedrawTechs();
      this.RestoreGamepadOnTech(tech.id);
    })))
      return;
    switch (tech.GetState())
    {
      case TechDefinition.TechState.Purchased:
        break;
      case TechDefinition.TechState.Hidden:
        Sounds.OnGUIClick();
        if (BaseGUI.for_gamepad)
          this.button_tips.Deactivate<ButtonTipsStr>();
        GUIElements.me.dialog.OpenOK(GJL.L("tech_is_hidden"), (GJCommons.VoidDelegate) (() =>
        {
          if (!BaseGUI.for_gamepad)
            return;
          this.button_tips.Activate<ButtonTipsStr>();
          this.RestoreGamepadOnTech(tech.id);
        }));
        break;
      default:
        Sounds.OnGUIClick();
        if (BaseGUI.for_gamepad)
          this.button_tips.Deactivate<ButtonTipsStr>();
        if (tech.price.IsEmpty())
        {
          GUIElements.me.dialog.OpenOK(GJL.L("tech_cant_be_unlocked"), (GJCommons.VoidDelegate) (() =>
          {
            if (!BaseGUI.for_gamepad)
              return;
            this.button_tips.Activate<ButtonTipsStr>();
            this.RestoreGamepadOnTech(tech.id);
          }));
          break;
        }
        GUIElements.me.tech_dialog.Open(tech, (GJCommons.VoidDelegate) (() =>
        {
          this.RedrawTechs();
          if (!BaseGUI.for_gamepad)
            return;
          this.button_tips.Activate<ButtonTipsStr>();
          this.RestoreGamepadOnTech(tech.id);
        }));
        break;
    }
  }

  public void SelectTechBranch(int branch_id)
  {
    Debug.Log((object) ("Select tech branch = " + branch_id.ToString()));
    this._cur_branch = branch_id;
    foreach (TechBranchGUIItem branch in this._branches)
      branch.Redraw();
    this.DrawBranch(branch_id);
    this.branches_grid.Reposition();
    foreach (UIWidget componentsInChild in this.GetComponentsInChildren<UIWidget>(true))
    {
      if (!((UnityEngine.Object) componentsInChild.gameObject.GetComponentInParent<TechConnector>() != (UnityEngine.Object) null) && !((UnityEngine.Object) componentsInChild.gameObject.GetComponent<UIPanel>() != (UnityEngine.Object) null))
        componentsInChild.UpdateAnchors();
    }
    if (BaseGUI.for_gamepad)
      this.gamepad_controller.ReinitItems(false);
    this.branches_grid.Reposition();
    this.branches_grid.repositionNow = true;
  }

  public void Draw()
  {
    for (int index = 0; index < this._branches.Count; ++index)
    {
      this._branches[index].gameObject.transform.SetParent((Transform) null, false);
      this._branches[index].gameObject.Destroy();
    }
    this._branches.Clear();
    for (int branch_id = 0; branch_id <= this._max_branch_in_balance; ++branch_id)
    {
      if (MainGame.me.save.IsTechBranchVisible(branch_id))
      {
        TechBranchGUIItem techBranchGuiItem = this._branch_item_prefab.Copy<TechBranchGUIItem>();
        techBranchGuiItem.gameObject.SetActive(true);
        techBranchGuiItem.Draw(branch_id);
        this._branches.Add(techBranchGuiItem);
      }
    }
    for (int index = 0; index < this._branches.Count; ++index)
    {
      TechBranchGUIItem branch = this._branches[index];
      if (index != 0)
        this._branches[index - 1].GetComponent<GamepadNavigationItem>();
      if (index != this._branches.Count - 1)
        this._branches[index + 1].GetComponent<GamepadNavigationItem>();
    }
    this.branches_grid.Reposition();
    this.branches_grid.repositionNow = true;
    this.RedrawTechs();
  }

  public void RedrawTechs() => this.DrawBranch(this._cur_branch);

  public void RestoreGamepadOnTech(string tech_id)
  {
    if (!BaseGUI.for_gamepad)
      return;
    TechTreeGUIItem techTreeGuiItem1 = (TechTreeGUIItem) null;
    foreach (TechTreeGUIItem techTreeGuiItem2 in this._items)
    {
      if (techTreeGuiItem2.tech_id == tech_id)
      {
        techTreeGuiItem1 = techTreeGuiItem2;
        break;
      }
    }
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.SetFocusedItem((UnityEngine.Object) techTreeGuiItem1 == (UnityEngine.Object) null ? (GamepadNavigationItem) null : techTreeGuiItem1.GetComponent<GamepadNavigationItem>());
  }

  public override void CloseFromGameGUI()
  {
    base.CloseFromGameGUI();
    TooltipBubbleGUI.ChangeAvaibility(false);
  }

  public override bool OnPressedBack()
  {
    GUIElements.me.game_gui.Hide(true);
    return true;
  }

  public override bool OnPressedNextSubTab()
  {
    ++this._cur_branch;
    if (this._cur_branch > this._max_branch_in_balance)
      this._cur_branch = 0;
    if (!MainGame.me.save.IsTechBranchVisible(this._cur_branch))
      return this.OnPressedNextSubTab();
    this.SelectTechBranch(this._cur_branch);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() => this.gamepad_controller.ReinitItems(true)));
    return true;
  }

  public override bool OnPressedPrevSubTab()
  {
    --this._cur_branch;
    if (this._cur_branch < 0)
      this._cur_branch = this._max_branch_in_balance;
    if (!MainGame.me.save.IsTechBranchVisible(this._cur_branch))
      return this.OnPressedPrevSubTab();
    this.SelectTechBranch(this._cur_branch);
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() => this.gamepad_controller.ReinitItems(true)));
    return true;
  }

  [CompilerGenerated]
  public void \u003COnPressedNextSubTab\u003Eb__36_0() => this.gamepad_controller.ReinitItems(true);

  [CompilerGenerated]
  public void \u003COnPressedPrevSubTab\u003Eb__37_0() => this.gamepad_controller.ReinitItems(true);
}
