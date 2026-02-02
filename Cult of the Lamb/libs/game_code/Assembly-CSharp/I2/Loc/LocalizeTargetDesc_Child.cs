// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTargetDesc_Child
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace I2.Loc;

public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
{
  public override bool CanLocalize(Localize cmp) => cmp.transform.childCount > 1;
}
