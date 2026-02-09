// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingRulesResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public class ISteamMatchmakingRulesResponse
{
  public ISteamMatchmakingRulesResponse.VTable m_VTable;
  public IntPtr m_pVTable;
  public GCHandle m_pGCHandle;
  public ISteamMatchmakingRulesResponse.RulesResponded m_RulesResponded;
  public ISteamMatchmakingRulesResponse.RulesFailedToRespond m_RulesFailedToRespond;
  public ISteamMatchmakingRulesResponse.RulesRefreshComplete m_RulesRefreshComplete;

  public ISteamMatchmakingRulesResponse(
    ISteamMatchmakingRulesResponse.RulesResponded onRulesResponded,
    ISteamMatchmakingRulesResponse.RulesFailedToRespond onRulesFailedToRespond,
    ISteamMatchmakingRulesResponse.RulesRefreshComplete onRulesRefreshComplete)
  {
    if (onRulesResponded == null || onRulesFailedToRespond == null || onRulesRefreshComplete == null)
      throw new ArgumentNullException();
    this.m_RulesResponded = onRulesResponded;
    this.m_RulesFailedToRespond = onRulesFailedToRespond;
    this.m_RulesRefreshComplete = onRulesRefreshComplete;
    this.m_VTable = new ISteamMatchmakingRulesResponse.VTable()
    {
      m_VTRulesResponded = new ISteamMatchmakingRulesResponse.InternalRulesResponded(this.InternalOnRulesResponded),
      m_VTRulesFailedToRespond = new ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond(this.InternalOnRulesFailedToRespond),
      m_VTRulesRefreshComplete = new ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete(this.InternalOnRulesRefreshComplete)
    };
    this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingRulesResponse.VTable)));
    Marshal.StructureToPtr<ISteamMatchmakingRulesResponse.VTable>(this.m_VTable, this.m_pVTable, false);
    this.m_pGCHandle = GCHandle.Alloc((object) this.m_pVTable, GCHandleType.Pinned);
  }

  void object.Finalize()
  {
    try
    {
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pGCHandle.IsAllocated)
        return;
      this.m_pGCHandle.Free();
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void InternalOnRulesResponded(IntPtr pchRule, IntPtr pchValue)
  {
    this.m_RulesResponded(InteropHelp.PtrToStringUTF8(pchRule), InteropHelp.PtrToStringUTF8(pchValue));
  }

  public void InternalOnRulesFailedToRespond() => this.m_RulesFailedToRespond();

  public void InternalOnRulesRefreshComplete() => this.m_RulesRefreshComplete();

  public static explicit operator IntPtr(ISteamMatchmakingRulesResponse that)
  {
    return that.m_pGCHandle.AddrOfPinnedObject();
  }

  public delegate void RulesResponded(string pchRule, string pchValue);

  public delegate void RulesFailedToRespond();

  public delegate void RulesRefreshComplete();

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalRulesResponded(IntPtr pchRule, IntPtr pchValue);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalRulesFailedToRespond();

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalRulesRefreshComplete();

  [StructLayout(LayoutKind.Sequential)]
  public class VTable
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
  }
}
