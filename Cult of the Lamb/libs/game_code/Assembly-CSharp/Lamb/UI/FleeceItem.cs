// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FleeceItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class FleeceItem : PlayerMenuItem<int>
{
  [SerializeField]
  public Image _DLCStar;
  [SerializeField]
  public Image _DLCStarTransmog;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public Image _iconOriginal;
  [SerializeField]
  public GameObject _transmogContainer;
  [SerializeField]
  public bool UseTransmogContainer;
  [SerializeField]
  public FleeceIconMapping _fleeceIconMapping;

  public override void Configure(int item)
  {
    this._DLCStar.gameObject.SetActive(false);
    this._DLCStarTransmog.gameObject.SetActive(false);
    this._transmogContainer.gameObject.SetActive(false);
    this._fleeceIconMapping.GetImage(item, this._icon);
    if (item == 1001 || item == 999)
      this._DLCStar.gameObject.SetActive(true);
    if (!this.UseTransmogContainer)
      return;
    foreach (Vector2 customisedFleeceOption in DataManager.Instance.CustomisedFleeceOptions)
    {
      if ((double) customisedFleeceOption.x == (double) item)
      {
        if ((double) customisedFleeceOption.x != (double) customisedFleeceOption.y)
          this._transmogContainer.gameObject.SetActive(true);
        this._fleeceIconMapping.GetImage(item, this._iconOriginal);
        this._fleeceIconMapping.GetImage((int) customisedFleeceOption.y, this._icon);
        if ((int) customisedFleeceOption.y == 1001 || (int) customisedFleeceOption.y == 999)
          this._DLCStar.gameObject.SetActive(true);
        if (item != 1001 && item != 999)
          break;
        this._DLCStarTransmog.gameObject.SetActive(true);
        break;
      }
    }
  }
}
