// Decompiled with JetBrains decompiler
// Type: SaveSlotGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SaveSlotGUI : MonoBehaviour
{
  public UILabel slot_name;
  public UILabel txt_realtime;
  public UILabel txt_descr;
  public UILabel txt_stats;
  public GameObject delete_button;
  public UI2DSprite back;
  public Color mouse_over_color;
  public UIWidget gamepad_frame;
  [SerializeField]
  [HideInInspector]
  public SaveSlotsMenuGUI _menu;
  [HideInInspector]
  [SerializeField]
  public GamepadNavigationItem _gamepad_item;
  public SaveSlotData _data;

  public void InitPrefab(SaveSlotsMenuGUI menu)
  {
    this.Deactivate<SaveSlotGUI>();
    this._menu = menu;
    this._gamepad_item = this.GetComponent<GamepadNavigationItem>();
  }

  public void Show(SaveSlotData save)
  {
    this._data = save;
    if (this._data == null)
    {
      this.slot_name.text = GJL.L("new game");
      this.txt_realtime.text = this.txt_stats.text = this.txt_descr.text = "";
    }
    else
    {
      this.slot_name.text = "";
      this.txt_realtime.text = save.real_time;
      this.txt_stats.text = save.stats;
      this.txt_descr.text = "[c][F0A33E]" + GJL.L("save_slot_descr", $"{Mathf.Max(0.0f, save.game_time - 1.5f):0.0}").Replace(":", ":[-][/c]");
    }
    this.delete_button.SetActive(!BaseGUI.for_gamepad && this._data != null);
    if (BaseGUI.for_gamepad)
      this._gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(this.OnOut), new GJCommons.VoidDelegate(this.OnSlotSelect));
    this.gamepad_frame.Deactivate<UIWidget>();
  }

  public void OnMouseOvered()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.OnOver();
  }

  public void OnMouseOut()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.OnOut();
  }

  public void OnOver()
  {
    this._menu.OnSlotGamepadOvered(this._data, this);
    this.gamepad_frame.Activate<UIWidget>();
    Sounds.OnGUIHover();
  }

  public void OnOut() => this.gamepad_frame.Deactivate<UIWidget>();

  public void OnSlotSelect()
  {
    Debug.Log((object) ("On select " + this.slot_name.text));
    this._menu.OnSelectSlotPressed(this._data);
  }

  public void OnDeletePressed()
  {
    this.OnOut();
    this._menu.OnDeleteSlotPressed(this._data);
  }
}
