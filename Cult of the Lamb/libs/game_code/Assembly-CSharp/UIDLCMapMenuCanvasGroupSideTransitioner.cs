// Decompiled with JetBrains decompiler
// Type: UIDLCMapMenuCanvasGroupSideTransitioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
public class UIDLCMapMenuCanvasGroupSideTransitioner : UIDLCMapSideTransitioner
{
  [SerializeField]
  public CanvasGroup _insideCanvasGroup;
  [SerializeField]
  public RectTransform _outsideMask;
  [SerializeField]
  public CanvasGroup _outsideCanvasGroup;
  [SerializeField]
  public CanvasGroup _finalCanvasGroup;
  [SerializeField]
  public RectTransform _insideMask;
  [SerializeField]
  public float _fadeDuration = 0.8f;

  public override void Initialise() => this.TransitionToLayer(this.CurrentSide);

  public override async System.Threading.Tasks.Task HideLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side)
  {
    switch (side)
    {
      case UIDLCMapMenuController.DLCDungeonSide.InsideMountain:
        this._insideCanvasGroup.gameObject.SetActive(false);
        break;
      case UIDLCMapMenuController.DLCDungeonSide.OutsideMountain:
        this._outsideCanvasGroup.gameObject.SetActive(false);
        break;
    }
  }

  public override async System.Threading.Tasks.Task ShowLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side)
  {
    switch (side)
    {
      case UIDLCMapMenuController.DLCDungeonSide.InsideMountain:
        this._insideCanvasGroup.gameObject.SetActive(true);
        break;
      case UIDLCMapMenuController.DLCDungeonSide.OutsideMountain:
        this._outsideCanvasGroup.gameObject.SetActive(true);
        break;
    }
  }
}
