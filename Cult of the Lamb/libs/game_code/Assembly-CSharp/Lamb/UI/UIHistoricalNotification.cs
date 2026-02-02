// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHistoricalNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class UIHistoricalNotification : MonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public MMSelectable _selectable;

  public RectTransform RectTransform => this._rectTransform;

  public MMSelectable Selectable => this._selectable;
}
