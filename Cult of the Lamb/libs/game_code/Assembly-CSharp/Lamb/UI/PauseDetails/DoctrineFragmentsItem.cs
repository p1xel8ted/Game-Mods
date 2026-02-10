// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.DoctrineFragmentsItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
