// Decompiled with JetBrains decompiler
// Type: I2.Loc.ILocalizeTargetDescriptor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
