// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorFollowElementSizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace src.UINavigator;

public class UINavigatorFollowElementSizer : MonoBehaviour
{
  [SerializeField]
  public RectTransform _sizeToMe;

  public RectTransform SizeToMe => this._sizeToMe;
}
