// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTargetDesc`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
{
  public override ILocalizeTarget CreateTarget(Localize cmp)
  {
    return (ILocalizeTarget) ScriptableObject.CreateInstance<T>();
  }

  public override System.Type GetTargetType() => typeof (T);
}
