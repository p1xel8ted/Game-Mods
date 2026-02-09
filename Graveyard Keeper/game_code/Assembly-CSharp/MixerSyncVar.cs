// Decompiled with JetBrains decompiler
// Type: MixerSyncVar
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[AttributeUsage(AttributeTargets.Field)]
public class MixerSyncVar : Attribute
{
  public float updateInterval;

  public MixerSyncVar(double newUpdateInterval = 1.0)
  {
    this.updateInterval = (float) newUpdateInterval;
  }
}
