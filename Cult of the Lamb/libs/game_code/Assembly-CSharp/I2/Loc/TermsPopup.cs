// Decompiled with JetBrains decompiler
// Type: I2.Loc.TermsPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class TermsPopup : PropertyAttribute
{
  [CompilerGenerated]
  public string \u003CFilter\u003Ek__BackingField;

  public TermsPopup(string filter = "") => this.Filter = filter;

  public string Filter
  {
    get => this.\u003CFilter\u003Ek__BackingField;
    set => this.\u003CFilter\u003Ek__BackingField = value;
  }
}
