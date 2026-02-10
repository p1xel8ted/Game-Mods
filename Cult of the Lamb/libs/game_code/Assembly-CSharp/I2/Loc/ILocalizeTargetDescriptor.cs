// Decompiled with JetBrains decompiler
// Type: I2.Loc.ILocalizeTargetDescriptor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace I2.Loc;

public abstract class ILocalizeTargetDescriptor
{
  public string Name;
  public int Priority;

  public abstract bool CanLocalize(Localize cmp);

  public abstract ILocalizeTarget CreateTarget(Localize cmp);

  public abstract Type GetTargetType();
}
