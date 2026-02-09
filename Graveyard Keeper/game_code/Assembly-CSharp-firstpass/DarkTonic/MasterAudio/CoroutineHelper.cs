// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.CoroutineHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class CoroutineHelper
{
  public static IEnumerator WaitForActualSeconds(float time)
  {
    float start = AudioUtil.Time;
    while ((double) AudioUtil.Time < (double) start + (double) time)
      yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
  }
}
