// Decompiled with JetBrains decompiler
// Type: BuildToolbar
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

#nullable disable
public class BuildToolbar : MonoBehaviour
{
  public const int MAX_BUILD_MODE = 2;
  public const int NORMAL_Y_POS = 20;
  public const int HIDDEN_Y_POS = -30;
  public BuildToolbar.BuildMode _build_mode;
  public Sequence _seq;

  public void AnimateAppear()
  {
    this.gameObject.SetActive(true);
    this.Init();
  }

  public void AnimateDisppear()
  {
  }

  public void AnimateToolbarsSwap(UIRect.AnchorPoint show, UIRect.AnchorPoint hide)
  {
    show.absolute = -30;
    hide.absolute = 20;
    if (this._seq != null)
      this._seq.Kill();
    this._seq = DOTween.Sequence();
    this._seq.Append((Tween) DOTween.To((DOGetter<int>) (() => hide.absolute), (DOSetter<int>) (x => hide.absolute = x), -30, 0.2f));
    this._seq.Append((Tween) DOTween.To((DOGetter<int>) (() => show.absolute), (DOSetter<int>) (x => show.absolute = x), 20, 0.2f));
  }

  public void Init() => this.Redraw();

  public void Redraw()
  {
  }

  public void SwitchTool(int dir)
  {
    int num = (int) (this._build_mode + dir);
    if (num < 0)
      num = 2;
    if (num > 2)
      num = 0;
    this._build_mode = (BuildToolbar.BuildMode) num;
    this.Redraw();
  }

  public BuildToolbar.BuildMode GetCurrentBuildMode() => this._build_mode;

  public enum BuildMode
  {
    Build,
    PickUp,
    Remove,
  }
}
