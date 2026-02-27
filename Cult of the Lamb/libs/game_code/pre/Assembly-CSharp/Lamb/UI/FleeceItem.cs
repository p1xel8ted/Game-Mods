// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FleeceItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class FleeceItem : PlayerMenuItem<int>
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private FleeceIconMapping _fleeceIconMapping;

  public override void Configure(int item) => this._fleeceIconMapping.GetImage(item, this._icon);
}
