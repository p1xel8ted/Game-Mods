// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ThemedElement
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class ThemedElement : MonoBehaviour
{
  [SerializeField]
  public ThemedElement.ElementInfo[] _elements;

  public void Start() => Rewired.UI.ControlMapper.ControlMapper.ApplyTheme(this._elements);

  [Serializable]
  public class ElementInfo
  {
    [SerializeField]
    public string _themeClass;
    [SerializeField]
    public Component _component;

    public string themeClass => this._themeClass;

    public Component component => this._component;
  }
}
