// Decompiled with JetBrains decompiler
// Type: ControlKeyLineGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ControlKeyLineGUI : MonoBehaviour
{
  public UILabel key_value;
  public UILabel key_description;
  public GameKey key;
  public bool _is_rebinding;
  public bool _blink_vis;
  public float _t;
  [NonSerialized]
  public bool changed;
  public const float BLINK_TIME = 0.05f;

  public void OnEnable() => this._is_rebinding = false;

  public void Redraw()
  {
    this.key_value.text = GameKeyTip.GetIcon(this.key).Replace("[", "").Replace("]", "");
    this.SetKeyAlpha(1f);
    this.RedrawDescription();
  }

  public void RedrawDescription()
  {
    string lng_id;
    switch (this.key)
    {
      case GameKey.Attack:
        lng_id = "control_atk";
        break;
      case GameKey.Dash:
        lng_id = "control_dash";
        break;
      case GameKey.Interaction:
        lng_id = "control_interact";
        break;
      case GameKey.Work:
        lng_id = "control_work";
        break;
      case GameKey.GameGUI:
        lng_id = "control_gmenu";
        break;
      case GameKey.IngameMenu:
        lng_id = "control_pause";
        break;
      case GameKey.Toolbar1:
        lng_id = GJL.L("control_qslot") + " 1";
        break;
      case GameKey.Toolbar2:
        lng_id = GJL.L("control_qslot") + " 2";
        break;
      case GameKey.Toolbar3:
        lng_id = GJL.L("control_qslot") + " 3";
        break;
      case GameKey.Toolbar4:
        lng_id = GJL.L("control_qslot") + " 4";
        break;
      case GameKey.Left:
        lng_id = "key_left";
        break;
      case GameKey.Right:
        lng_id = "key_right";
        break;
      case GameKey.Up:
        lng_id = "key_up";
        break;
      case GameKey.Down:
        lng_id = "key_down";
        break;
      case GameKey.Map:
        lng_id = "map";
        break;
      case GameKey.Inventory:
        lng_id = "tab_Inventory";
        break;
      case GameKey.Techs:
        lng_id = "tab_Techs";
        break;
      case GameKey.KnownNPCs:
        lng_id = "tab_NPCs";
        break;
      default:
        lng_id = "control_" + this.key.ToString().ToLower();
        break;
    }
    this.key_description.text = GJL.L(lng_id).ToLower();
  }

  public void OnRebind()
  {
    this._blink_vis = true;
    this._is_rebinding = true;
    this._t = 0.05f;
    this.key_value.text = "?";
  }

  public void SetKeyAlpha(float a)
  {
    this.key_value.color = this.key_value.color with
    {
      a = a
    };
  }

  public void Update()
  {
    if (!this._is_rebinding)
      return;
    this._t -= Time.deltaTime;
    if ((double) this._t <= 0.0)
    {
      this._t = 0.05f;
      this._blink_vis = !this._blink_vis;
      this.SetKeyAlpha(this._blink_vis ? 1f : 0.0f);
    }
    if (!Input.anyKeyDown)
      return;
    foreach (KeyCode key in Enum.GetValues(typeof (KeyCode)))
    {
      switch (key)
      {
        case KeyCode.Mouse0:
        case KeyCode.Mouse1:
        case KeyCode.Mouse2:
          continue;
        default:
          if (!key.ToString().Contains("Mouse") && Input.GetKeyDown(key))
          {
            this.SetKeyAlpha(1f);
            this._is_rebinding = false;
            if (key == KeyCode.Escape)
              return;
            KeyBindings.RedefineKey(this.key, key);
            this.changed = true;
            this.Redraw();
            return;
          }
          continue;
      }
    }
  }
}
