// Decompiled with JetBrains decompiler
// Type: MouseCursorAutoHide
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MouseCursorAutoHide : MonoBehaviour
{
  public const float MOUSE_AUTOHIDE_TIME = 4f;
  public Vector2 _last_mouse_pos = Vector2.zero;
  public float _last_move_time;
  public float _dtime;
  public bool _mouse_shown = true;

  public void Awake() => this._last_move_time = Time.time;

  public void Update()
  {
    Vector2 mousePosition = (Vector2) Input.mousePosition;
    double magnitude = (double) (this._last_mouse_pos - mousePosition).magnitude;
    this._last_mouse_pos = mousePosition;
    bool flag = true;
    if (magnitude > 0.10000000149011612)
    {
      this._last_move_time = Time.time;
      this._dtime = 0.0f;
    }
    else
    {
      this._dtime = Time.time - this._last_move_time;
      if ((double) this._dtime > 4.0)
        flag = false;
    }
    if (!MainGame.game_started || !BaseGUI.all_guis_closed && !BaseGUI.for_gamepad)
    {
      flag = true;
      this._last_move_time = Time.time;
    }
    if (this._mouse_shown == flag)
      return;
    Cursor.visible = flag;
    this._mouse_shown = flag;
  }
}
