// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTargetDesc_Child
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace I2.Loc;

public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
{
  public override bool CanLocalize(Localize cmp) => cmp.transform.childCount > 1;
}
