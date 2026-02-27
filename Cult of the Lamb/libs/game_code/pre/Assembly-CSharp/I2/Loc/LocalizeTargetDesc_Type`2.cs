// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTargetDesc_Type`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
