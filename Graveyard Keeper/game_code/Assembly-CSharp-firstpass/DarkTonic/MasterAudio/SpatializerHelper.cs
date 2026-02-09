// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.SpatializerHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class SpatializerHelper
{
  public static bool SpatializerOptionExists => true;

  public static void TurnOnSpatializerIfEnabled(AudioSource source)
  {
    if (!SpatializerHelper.SpatializerOptionExists || (Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (Object) null || !DarkTonic.MasterAudio.MasterAudio.Instance.useSpatializer)
      return;
    source.spatialize = true;
  }
}
