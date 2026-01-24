// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorFollowElementSizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
