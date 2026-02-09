// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.DuckGroupInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class DuckGroupInfo
{
  public string soundType = "[None]";
  public float riseVolStart = 0.5f;
  public float unduckTime = 1f;
  public float duckedVolumeCut = -6f;
  public bool isTemporary;
}
