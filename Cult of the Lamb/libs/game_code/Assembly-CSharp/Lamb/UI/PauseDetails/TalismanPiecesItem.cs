// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.TalismanPiecesItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class TalismanPiecesItem : PlayerMenuItem<int>
{
  [SerializeField]
  public GameObject _lockedContainer;
  [SerializeField]
  public GameObject _top;
  [SerializeField]
  public GameObject _right;
  [SerializeField]
  public GameObject _bottom;
  [SerializeField]
  public GameObject _left;
  [SerializeField]
  public MMSelectable _selectable;

  public MMSelectable Selectable => this._selectable;

  public override void Configure(int pieces)
  {
    this._lockedContainer.SetActive(Inventory.KeyPieces + Inventory.TempleKeys == 0 && !DataManager.Instance.HadFirstTempleKey);
    this._top.SetActive(pieces >= 1);
    this._right.SetActive(pieces >= 2);
    this._bottom.SetActive(pieces >= 3);
    this._left.SetActive(pieces >= 4);
  }
}
