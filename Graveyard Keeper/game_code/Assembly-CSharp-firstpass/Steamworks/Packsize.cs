// Decompiled with JetBrains decompiler
// Type: Steamworks.Packsize
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public static class Packsize
{
  public const int value = 8;

  public static bool Test()
  {
    int num1 = Marshal.SizeOf(typeof (Packsize.ValvePackingSentinel_t));
    int num2 = Marshal.SizeOf(typeof (RemoteStorageEnumerateUserSubscribedFilesResult_t));
    return num1 == 32 /*0x20*/ && num2 == 616;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct ValvePackingSentinel_t
  {
    public uint m_u32;
    public ulong m_u64;
    public ushort m_u16;
    public double m_d;
  }
}
