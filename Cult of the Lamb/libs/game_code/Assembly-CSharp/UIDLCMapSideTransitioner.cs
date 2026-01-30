// Decompiled with JetBrains decompiler
// Type: UIDLCMapSideTransitioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class UIDLCMapSideTransitioner : MonoBehaviour
{
  [SerializeField]
  public RectTransform _outsideContainer;
  [SerializeField]
  public RectTransform _insideContainer;
  [CompilerGenerated]
  public UIDLCMapMenuController.DLCDungeonSide \u003CCurrentSide\u003Ek__BackingField = UIDLCMapMenuController.DLCDungeonSide.OutsideMountain;

  public RectTransform OutsideContainer => this._outsideContainer;

  public RectTransform InsideContainer => this._insideContainer;

  public UIDLCMapMenuController.DLCDungeonSide CurrentSide
  {
    get => this.\u003CCurrentSide\u003Ek__BackingField;
    set => this.\u003CCurrentSide\u003Ek__BackingField = value;
  }

  public RectTransform CurrentContainer => this.GetContainerForSide(this.CurrentSide);

  public RectTransform GetContainerForSide(UIDLCMapMenuController.DLCDungeonSide side)
  {
    return side == UIDLCMapMenuController.DLCDungeonSide.InsideMountain || side != UIDLCMapMenuController.DLCDungeonSide.OutsideMountain ? this.InsideContainer : this.OutsideContainer;
  }

  public void SetCurrentLayer(UIDLCMapMenuController.DLCDungeonSide side)
  {
    this.CurrentSide = side;
  }

  public abstract void Initialise();

  public abstract System.Threading.Tasks.Task HideLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side);

  public abstract System.Threading.Tasks.Task ShowLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side);

  public void TransitionToLayer(UIDLCMapMenuController.DLCDungeonSide side)
  {
    this.HideLayerAsync(UIDLCMapMenuController.DLCDungeonSide.InsideMountain);
    this.HideLayerAsync(UIDLCMapMenuController.DLCDungeonSide.OutsideMountain);
    this.ShowLayerAsync(side);
  }
}
