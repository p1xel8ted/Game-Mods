// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ThemedElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class ThemedElement : MonoBehaviour
{
  [SerializeField]
  public ThemedElement.ElementInfo[] _elements;

  public void Start() => this.ApplyTheme();

  public void OnEnable() => Rewired.UI.ControlMapper.ControlMapper.Register(this);

  public void OnDisable() => Rewired.UI.ControlMapper.ControlMapper.Unregister(this);

  public void ApplyTheme() => Rewired.UI.ControlMapper.ControlMapper.ApplyTheme(this._elements);

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
