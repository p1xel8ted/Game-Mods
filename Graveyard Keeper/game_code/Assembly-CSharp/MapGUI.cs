// Decompiled with JetBrains decompiler
// Type: MapGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapGUI : BaseGameGUI
{
  public UIScrollView _scroll;
  public float gamepad_scroll_sensitivity = 0.1f;
  public float momentum_decay = 0.1f;
  public Vector2 _momentum = Vector2.zero;
  public Transform left;
  public Transform right;
  public Transform up;
  public Transform down;
  public VirtualCursor virtual_cursor;
  public Dictionary<string, MapZoneGUI> _zones = new Dictionary<string, MapZoneGUI>();

  public override void Init()
  {
    this._scroll = this.GetComponentInChildren<UIScrollView>(true);
    base.Init();
    this._zones.Clear();
    foreach (MapZoneGUI componentsInChild in this.GetComponentsInChildren<MapZoneGUI>(true))
      this._zones.Add(componentsInChild.name, componentsInChild);
  }

  public override void Open()
  {
    base.Open();
    WorldZone myWorldZone = MainGame.me.player.GetMyWorldZone();
    foreach (KeyValuePair<string, MapZoneGUI> zone in this._zones)
    {
      zone.Value.label.text = GJL.L(string.IsNullOrEmpty(zone.Value.override_name) ? "zone_" + zone.Key : zone.Value.override_name);
      bool flag = MainGame.me.save.known_world_zones.Contains(zone.Key);
      zone.Value.gameObject.SetActive(flag);
      if ((Object) zone.Value.hidden != (Object) null)
        zone.Value.hidden.SetActive(!flag);
      if ((Object) myWorldZone != (Object) null)
      {
        int num = zone.Key == myWorldZone.id ? 1 : 0;
      }
    }
    this.button_tips.gameObject.SetActive(MainGame.me.save.has_global_craft_control);
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.LeftStick(), GameKeyTip.RightStick());
    this.virtual_cursor.gameObject.SetActive(BaseGUI.for_gamepad && MainGame.me.save.has_global_craft_control);
    if (!BaseGUI.for_gamepad || !MainGame.me.save.has_global_craft_control)
      return;
    this.virtual_cursor.EnableAsMapCursor(this.left, this.right, this.up, this.down);
  }

  public void OnDisable()
  {
    this.virtual_cursor.gameObject.SetActive(false);
    ZoneControlItem.current_selected = (ZoneControlItem) null;
    this.button_tips.Clear();
  }

  public override bool OnPressedBack()
  {
    GUIElements.me.game_gui.Hide(true);
    return true;
  }

  public override void Update()
  {
    Vector2 direction = LazyInput.GetDirection();
    if (!direction.magnitude.EqualsTo(0.0f))
      this._momentum = -direction;
    else
      this._momentum *= Mathf.Pow(this.momentum_decay, Time.deltaTime);
    if ((double) this._momentum.magnitude < 0.001)
    {
      this._momentum = Vector2.zero;
    }
    else
    {
      Vector2 relative = this._momentum * Time.deltaTime * this.gamepad_scroll_sensitivity;
      relative = new Vector2(Mathf.Round(relative.x), Mathf.Round(relative.y));
      this._scroll.MoveRelative((Vector3) relative);
      this._scroll.RestrictWithinBounds(true);
    }
    base.Update();
  }
}
