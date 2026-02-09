// Decompiled with JetBrains decompiler
// Type: ZoneControlItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ZoneControlItem : MonoBehaviour
{
  public GameObject selection;
  public string zone_group;
  public List<string> zones = new List<string>();
  public bool is_selected;
  public static ZoneControlItem current_selected;
  public UI2DSprite icon;

  public void OnEnable() => this.icon.enabled = this.IsEnabled();

  public bool IsEnabled()
  {
    if (!MainGame.me.save.has_global_craft_control)
      return false;
    for (int index = 0; index < this.zones.Count; ++index)
    {
      WorldZone zoneById = WorldZone.GetZoneByID(this.zones[index]);
      if ((Object) zoneById != (Object) null && MainGame.me.save.IsWorldZoneKnown(zoneById.id) && !zoneById.IsDisabled() && zoneById.HasBuilder())
        return true;
    }
    return false;
  }

  public void Awake()
  {
    this.icon = this.GetComponent<UI2DSprite>();
    for (int index = 0; index < GameBalance.me.world_zones_data.Count; ++index)
    {
      if (GameBalance.me.world_zones_data[index].zone_group == this.zone_group)
        this.zones.Add(GameBalance.me.world_zones_data[index].id);
    }
  }

  public void Update()
  {
    if (!LazyInput.gamepad_active || !this.is_selected || !LazyInput.GetKeyDown(GameKey.Select))
      return;
    LazyInput.ClearKeyDown(GameKey.Select);
    this.OnPress();
  }

  public void OnOver()
  {
    if (!this.IsEnabled())
      return;
    if ((Object) ZoneControlItem.current_selected != (Object) null)
      ZoneControlItem.current_selected.OnOut();
    this.selection.gameObject.SetActive(true);
    this.is_selected = true;
    ZoneControlItem.current_selected = this;
  }

  public void OnOut()
  {
    if (!this.IsEnabled())
      return;
    this.selection.gameObject.SetActive(false);
    this.is_selected = false;
  }

  public void OnPress()
  {
    if (!this.IsEnabled())
      return;
    this.selection.gameObject.SetActive(false);
    GUIElements.me.game_gui.Hide(true);
    GUIElements.me.global_craft_control_gui.Open(this.zone_group);
    this.is_selected = false;
    Sounds.OnGUIClick();
  }

  public void OnDisable() => this.OnOut();

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("VirtualCursor"))
      return;
    this.OnOver();
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (!other.CompareTag("VirtualCursor"))
      return;
    this.OnOut();
  }
}
