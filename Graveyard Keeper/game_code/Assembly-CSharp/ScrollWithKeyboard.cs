// Decompiled with JetBrains decompiler
// Type: ScrollWithKeyboard
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIScrollView))]
public class ScrollWithKeyboard : MonoBehaviour
{
  public const float MOMENTUM_DECAY = 1f;
  public ScrollWithKeyboard.KeyboardScrollType scroll_type;
  public float scroll_sensivity = 0.1f;
  public UIScrollView _scroll_view;

  public UIScrollView scroll_view
  {
    get => this._scroll_view ?? (this._scroll_view = this.GetComponent<UIScrollView>());
  }

  public enum KeyboardScrollType
  {
    Vertical,
    Horizontal,
  }
}
