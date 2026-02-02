// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTargetDesc`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
