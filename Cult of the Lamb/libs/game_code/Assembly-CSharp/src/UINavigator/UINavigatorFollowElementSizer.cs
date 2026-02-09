// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorFollowElementSizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
