// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ThemedElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class ThemedElement : MonoBehaviour
{
  [SerializeField]
  private ThemedElement.ElementInfo[] _elements;

  private void Start() => Rewired.UI.ControlMapper.ControlMapper.ApplyTheme(this._elements);

  [Serializable]
  public class ElementInfo
  {
    [SerializeField]
    private string _themeClass;
    [SerializeField]
    private Component _component;

    public string themeClass => this._themeClass;

    public Component component => this._component;
  }
}
