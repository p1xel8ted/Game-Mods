// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.DoctrineFragmentsItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class DoctrineFragmentsItem : PlayerMenuItem<int>
{
  [SerializeField]
  private GameObject _lockedContainer;
  [SerializeField]
  private GameObject _top;
  [SerializeField]
  private GameObject _right;
  [SerializeField]
  private GameObject _left;

  public override void Configure(int pieces)
  {
    this._lockedContainer.SetActive(DataManager.Instance.CompletedDoctrineStones == 0 && !DataManager.Instance.FirstDoctrineStone);
    this._left.SetActive(pieces >= 1);
    this._right.SetActive(pieces >= 2);
    this._top.SetActive(pieces >= 3);
  }
}
