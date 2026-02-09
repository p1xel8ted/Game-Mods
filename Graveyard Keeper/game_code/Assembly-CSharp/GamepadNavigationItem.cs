// Decompiled with JetBrains decompiler
// Type: GamepadNavigationItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (PanelAutoScroll))]
public class GamepadNavigationItem : MonoBehaviour
{
  public int group;
  public int sub_group;
  public GJCommons.VoidDelegate _on_focus;
  public GJCommons.VoidDelegate _on_unfocus;
  public GJCommons.VoidDelegate _on_pressed;
  public GameObject focus_frame;
  public Transform _tf;
  public GamepadNavigationController _controller;
  public int _index;
  public bool _is_focused;
  public Vector2 _half_size;
  public PanelAutoScroll _auto_scroll;
  public bool _active = true;
  [Space(5f)]
  [Header("Custom navigation")]
  public GamepadNavigationItem left_item;
  public GamepadNavigationItem right_item;
  public GamepadNavigationItem up_item;
  public GamepadNavigationItem down_item;

  public bool is_focused => this._is_focused;

  public bool active
  {
    get => this._active;
    set => this._active = value;
  }

  public int index
  {
    get => this._index;
    set => this._index = value;
  }

  public Transform tf => this._tf;

  public Vector2 pos => (Vector2) this.tf.position;

  public GamepadNavigationController controller => this._controller;

  public void Init(int i, GamepadNavigationController controller, float gui_scale)
  {
    this._index = i;
    this._controller = controller;
    this._tf = this.transform;
    this._auto_scroll = this.GetComponent<PanelAutoScroll>();
    if ((Object) this._auto_scroll == (Object) null)
      this._auto_scroll = this.gameObject.AddComponent<PanelAutoScroll>();
    this._auto_scroll.Init();
    UIWidget component = this.GetComponent<UIWidget>();
    this._half_size = (Object) component == (Object) null ? new Vector2(10f, 10f) : new Vector2((float) component.width, (float) component.height);
    this._half_size *= gui_scale / 2f;
  }

  public void SetCallbacks(
    GJCommons.VoidDelegate on_focus,
    GJCommons.VoidDelegate on_unfocus,
    GJCommons.VoidDelegate on_select)
  {
    this._on_focus = on_focus;
    this._on_unfocus = on_unfocus;
    this._on_pressed = on_select;
  }

  public void Focus(bool animate_auto_scroll = true)
  {
    if (this._is_focused)
      return;
    this.SetFocus(true);
    if (this._auto_scroll.avaible)
      this._auto_scroll.Perform(animate_auto_scroll);
    this._on_focus.TryInvoke();
  }

  public void UnFocus()
  {
    if (!this._is_focused)
      return;
    this.SetFocus(false);
    this._on_unfocus.TryInvoke();
  }

  public void SetFocus(bool focused)
  {
    this._is_focused = focused;
    if (!((Object) this.focus_frame != (Object) null))
      return;
    this.focus_frame.SetActive(focused);
  }

  public void Select() => this._on_pressed.TryInvoke();

  public float CalcDistToCurrentPos(Vector2 current_pos, Direction direction)
  {
    Vector2 pos = this.pos;
    switch (direction)
    {
      case Direction.Right:
        pos.x -= this._half_size.x;
        break;
      case Direction.Up:
        pos.y -= this._half_size.y;
        break;
      case Direction.Left:
        pos.x += this._half_size.x;
        break;
      case Direction.Down:
        pos.y += this._half_size.y;
        break;
    }
    return (pos - current_pos).magnitude;
  }

  public bool CorrectDirection(Vector2 other_pos, Direction direction)
  {
    Vector2 vector2 = this.pos - other_pos;
    if (vector2.magnitude.EqualsTo(0.0f))
      return false;
    switch (direction)
    {
      case Direction.Right:
        return (double) vector2.x > (double) this._half_size.x;
      case Direction.Up:
        return (double) vector2.y > (double) this._half_size.y;
      case Direction.Left:
        return (double) vector2.x < -(double) this._half_size.x;
      case Direction.Down:
        return (double) vector2.y < -(double) this._half_size.y;
      default:
        return false;
    }
  }

  public bool CorrectGrid(Vector2 other_pos, Direction direction)
  {
    Vector2 vector2 = this.pos - other_pos;
    switch (direction)
    {
      case Direction.Right:
      case Direction.Left:
        return (double) Mathf.Abs(vector2.x) > (double) Mathf.Abs(vector2.y) && (double) Mathf.Abs(vector2.y) <= (double) this._half_size.y;
      case Direction.Up:
      case Direction.Down:
        return (double) Mathf.Abs(vector2.y) > (double) Mathf.Abs(vector2.x) && (double) Mathf.Abs(vector2.x) <= (double) this._half_size.x;
      default:
        return false;
    }
  }

  public GamepadNavigationItem GetCustomDirectionItem(Direction dir)
  {
    GamepadNavigationItem gamepadNavigationItem = (GamepadNavigationItem) null;
    switch (dir)
    {
      case Direction.Right:
        gamepadNavigationItem = this.right_item;
        break;
      case Direction.Up:
        gamepadNavigationItem = this.up_item;
        break;
      case Direction.Left:
        gamepadNavigationItem = this.left_item;
        break;
      case Direction.Down:
        gamepadNavigationItem = this.down_item;
        break;
    }
    return !((Object) gamepadNavigationItem == (Object) null) && gamepadNavigationItem.isActiveAndEnabled && gamepadNavigationItem.active ? gamepadNavigationItem : (GamepadNavigationItem) null;
  }

  public void SetCustomDirectionItem(GamepadNavigationItem custom_item, Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        this.right_item = custom_item;
        break;
      case Direction.Up:
        this.up_item = custom_item;
        break;
      case Direction.Left:
        this.left_item = custom_item;
        break;
      case Direction.Down:
        this.down_item = custom_item;
        break;
    }
  }
}
