// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorFollowElementSizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
