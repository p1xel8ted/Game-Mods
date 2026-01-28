// Decompiled with JetBrains decompiler
// Type: I2.Loc.ILocalizeTargetDescriptor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
