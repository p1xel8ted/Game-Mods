// Decompiled with JetBrains decompiler
// Type: I2.Loc.TermsPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class TermsPopup : PropertyAttribute
{
  public TermsPopup(string filter = "") => this.Filter = filter;

  public string Filter { get; private set; }
}
