// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHistoricalNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
