// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.DoctrineFragmentsItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class DoctrineFragmentsItem : PlayerMenuItem<int>
{
  [SerializeField]
  public GameObject _lockedContainer;
  [SerializeField]
  public GameObject _top;
  [SerializeField]
  public GameObject _right;
  [SerializeField]
  public GameObject _left;
  [SerializeField]
  public MMSelectable _selectable;

  public MMSelectable Selectable => this._selectable;

  public override void Configure(int pieces)
  {
    this._lockedContainer.SetActive(DataManager.Instance.CompletedDoctrineStones == 0 && !DataManager.Instance.FirstDoctrineStone);
    if (DoctrineUpgradeSystem.TrySermonsStillAvailable())
    {
      this._left.SetActive(pieces >= 1);
      this._right.SetActive(pieces >= 2);
      this._top.SetActive(pieces >= 3);
    }
    else
    {
      this._left.SetActive(true);
      this._right.SetActive(true);
      this._top.SetActive(true);
    }
  }
}
