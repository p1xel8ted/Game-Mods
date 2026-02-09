// Decompiled with JetBrains decompiler
// Type: BuildItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildItemGUI : MonoBehaviour
{
  [HideInInspector]
  public Tooltip tooltip;
  [HideInInspector]
  public UIButton button;
  [HideInInspector]
  public UIWidget widget;
  [HideInInspector]
  public GamepadNavigationItem gamepad_item;
  [HideInInspector]
  public BoxCollider2D box_collider;
  public UI2DSprite icon;
  public UI2DSprite gamepad_selection;
  public UI2DSprite mouse_selection;
  public CraftDefinition _craft_definition;
  public bool _for_gamepad;
  public bool _can_craft;

  public CraftDefinition definition => this._craft_definition;

  public void InitPrefab()
  {
    this.box_collider = this.GetComponentInChildren<BoxCollider2D>();
    this.tooltip = this.GetComponentInChildren<Tooltip>();
    this.button = this.GetComponentInChildren<UIButton>();
    this.widget = this.GetComponentInChildren<UIWidget>();
    this.gamepad_item = this.GetComponentInChildren<GamepadNavigationItem>();
    this.gamepad_selection.Deactivate<UI2DSprite>();
    this.mouse_selection.Deactivate<UI2DSprite>();
    NGUIExtensionMethods.InitEventTriggers((MonoBehaviour) this, new EventDelegate.Callback(this.OnOver), new EventDelegate.Callback(this.OnOut), new EventDelegate.Callback(this.OnItemSelect));
    this.Deactivate<BuildItemGUI>();
  }

  public void Init(ObjectCraftDefinition definition, bool can_craft, bool for_gamepad)
  {
    this._craft_definition = (CraftDefinition) definition;
    this._for_gamepad = for_gamepad;
    this._can_craft = can_craft;
    this.InitIcon();
    this.tooltip.SetCraftDefinition((CraftDefinition) definition);
    this.button.enabled = can_craft;
    this.icon.color = can_craft ? this.button.defaultColor : this.button.disabledColor;
    this.gamepad_selection.Deactivate<UI2DSprite>();
    this.mouse_selection.Deactivate<UI2DSprite>();
    this.gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnItemSelect));
  }

  public void OnOver()
  {
    if (!this._for_gamepad)
    {
      this.mouse_selection.Activate<UI2DSprite>();
    }
    else
    {
      this.gamepad_selection.Activate<UI2DSprite>();
      GUIElements.me.builds.UpdateTip(this._can_craft);
    }
  }

  public void OnOut()
  {
    (this._for_gamepad ? this.gamepad_selection : this.mouse_selection).Deactivate<UI2DSprite>();
  }

  public void OnItemSelect()
  {
    LazyInput.ClearKeyDown(this._for_gamepad ? GameKey.Select : GameKey.LeftClick);
    this.Select();
  }

  public void Select()
  {
    if (!MainGame.me.build_mode_logics.CanBuild(this._craft_definition))
      return;
    MainGame.me.build_mode_logics.CraftBuilding(this._craft_definition);
  }

  public void InitIcon()
  {
    UnityEngine.Sprite sprite = (UnityEngine.Sprite) null;
    if (!string.IsNullOrEmpty(this._craft_definition.icon))
      sprite = EasySpritesCollection.GetSprite(this._craft_definition.icon);
    if ((Object) sprite == (Object) null)
      return;
    this.icon.DrawAndResize(sprite);
    this.gamepad_selection.Update();
    this.box_collider.size = this.icon.localSize;
    this.widget.width = this.gamepad_selection.width;
    this.widget.height = this.gamepad_selection.height;
  }
}
