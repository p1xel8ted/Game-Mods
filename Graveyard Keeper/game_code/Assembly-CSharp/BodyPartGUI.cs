// Decompiled with JetBrains decompiler
// Type: BodyPartGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BodyPartGUI : MonoBehaviour
{
  public Color mouse_frame_color = Color.magenta;
  public Color gamepad_frame_color = Color.magenta;
  public ItemDefinition.ItemType type;
  [SerializeField]
  public UI2DSprite _sprite_back;
  [SerializeField]
  public UI2DSprite _sprite_selected;
  [SerializeField]
  public UI2DSprite _sprite_frame;
  public AutopsyGUI _autopsy_gui;
  public GamepadNavigationItem _gamepad_item;
  public bool _for_gamepad;
  public BodyPartGUI.State _state;

  public bool exists => this._state == BodyPartGUI.State.Exists;

  public GamepadNavigationItem gamepad_item
  {
    get
    {
      if ((Object) this._gamepad_item == (Object) null)
        this._gamepad_item = this.GetComponent<GamepadNavigationItem>();
      return this._gamepad_item;
    }
  }

  public void Init()
  {
    this.gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnPartSelect));
    NGUIExtensionMethods.InitEventTriggers((MonoBehaviour) this, new EventDelegate.Callback(this.OnOver), new EventDelegate.Callback(this.OnOut), new EventDelegate.Callback(this.OnPartSelect));
  }

  public void Open(AutopsyGUI autopsy_gui, bool for_gamepad, bool exists)
  {
    this.Activate<BodyPartGUI>();
    this._autopsy_gui = autopsy_gui;
    this._for_gamepad = for_gamepad;
    this._sprite_frame.color = for_gamepad ? this.gamepad_frame_color : this.mouse_frame_color;
    this._sprite_frame.Deactivate<UI2DSprite>();
    this._sprite_selected.Deactivate<UI2DSprite>();
    this._state = exists ? BodyPartGUI.State.Exists : BodyPartGUI.State.Empty;
    this.gamepad_item.active = exists;
    this.gameObject.SetActive(exists);
    this._sprite_back.SetActive(exists);
    this._sprite_selected.SetActive(exists);
  }

  public void OnOver()
  {
    if (!this.exists)
      return;
    this._sprite_frame.SetActive(true);
  }

  public void OnOut() => this._sprite_frame.SetActive(false);

  public void OnPartSelect()
  {
    int num = this.exists ? 1 : 0;
  }

  public void ChangeSelectionState(bool active)
  {
    if (!this.exists)
      return;
    this._sprite_selected.SetActive(active);
  }

  public void Reinit()
  {
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      UI2DSprite component = this.transform.GetChild(index).GetComponent<UI2DSprite>();
      string name = component.name;
      if (name.Contains("back"))
        this._sprite_back = component;
      if (name.Contains("selected"))
        this._sprite_selected = component;
      if (name.Contains("frame"))
        this._sprite_frame = component;
    }
    this._sprite_back.Activate<UI2DSprite>();
    this._sprite_selected.Activate<UI2DSprite>();
    this._sprite_frame.Activate<UI2DSprite>();
  }

  public enum State
  {
    Empty,
    Exists,
  }
}
