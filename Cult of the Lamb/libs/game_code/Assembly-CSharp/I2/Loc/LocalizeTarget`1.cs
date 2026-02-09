// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public abstract class LocalizeTarget<T> : ILocalizeTarget where T : Object
{
  public T mTarget;

  public override bool IsValid(Localize cmp)
  {
    if ((Object) this.mTarget != (Object) null)
    {
      Component mTarget = (object) this.mTarget as Component;
      if ((Object) mTarget != (Object) null && (Object) mTarget.gameObject != (Object) cmp.gameObject)
        this.mTarget = default (T);
    }
    if ((Object) this.mTarget == (Object) null)
      this.mTarget = cmp.GetComponent<T>();
    return (Object) this.mTarget != (Object) null;
  }
}
