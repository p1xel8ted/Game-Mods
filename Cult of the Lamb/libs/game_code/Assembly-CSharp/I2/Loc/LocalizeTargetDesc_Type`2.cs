// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTargetDesc_Type`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTargetDesc_Type<T, G> : LocalizeTargetDesc<G>
  where T : Object
  where G : LocalizeTarget<T>
{
  public override bool CanLocalize(Localize cmp) => (Object) cmp.GetComponent<T>() != (Object) null;

  public override ILocalizeTarget CreateTarget(Localize cmp)
  {
    T component = cmp.GetComponent<T>();
    if ((Object) component == (Object) null)
      return (ILocalizeTarget) null;
    G instance = ScriptableObject.CreateInstance<G>();
    instance.mTarget = component;
    return (ILocalizeTarget) instance;
  }
}
