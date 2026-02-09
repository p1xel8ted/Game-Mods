// Decompiled with JetBrains decompiler
// Type: Steamworks.MMKVPMarshaller
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public class MMKVPMarshaller
{
  public IntPtr m_pNativeArray;
  public IntPtr m_pArrayEntries;

  public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
  {
    if (filters == null)
      return;
    int num = Marshal.SizeOf(typeof (MatchMakingKeyValuePair_t));
    this.m_pNativeArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (IntPtr)) * filters.Length);
    this.m_pArrayEntries = Marshal.AllocHGlobal(num * filters.Length);
    for (int index = 0; index < filters.Length; ++index)
      Marshal.StructureToPtr<MatchMakingKeyValuePair_t>(filters[index], new IntPtr(this.m_pArrayEntries.ToInt64() + (long) (index * num)), false);
    Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
  }

  void object.Finalize()
  {
    try
    {
      if (this.m_pArrayEntries != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pArrayEntries);
      if (!(this.m_pNativeArray != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this.m_pNativeArray);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public static implicit operator IntPtr(MMKVPMarshaller that) => that.m_pNativeArray;
}
