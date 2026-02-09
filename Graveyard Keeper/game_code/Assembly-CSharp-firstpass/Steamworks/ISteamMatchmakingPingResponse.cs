// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingPingResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public class ISteamMatchmakingPingResponse
{
  public ISteamMatchmakingPingResponse.VTable m_VTable;
  public IntPtr m_pVTable;
  public GCHandle m_pGCHandle;
  public ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;
  public ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

  public ISteamMatchmakingPingResponse(
    ISteamMatchmakingPingResponse.ServerResponded onServerResponded,
    ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
  {
    this.m_ServerResponded = onServerResponded != null && onServerFailedToRespond != null ? onServerResponded : throw new ArgumentNullException();
    this.m_ServerFailedToRespond = onServerFailedToRespond;
    this.m_VTable = new ISteamMatchmakingPingResponse.VTable()
    {
      m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
      m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
    };
    this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingPingResponse.VTable)));
    Marshal.StructureToPtr<ISteamMatchmakingPingResponse.VTable>(this.m_VTable, this.m_pVTable, false);
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

  public void InternalOnServerResponded(gameserveritem_t server) => this.m_ServerResponded(server);

  public void InternalOnServerFailedToRespond() => this.m_ServerFailedToRespond();

  public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
  {
    return that.m_pGCHandle.AddrOfPinnedObject();
  }

  public delegate void ServerResponded(gameserveritem_t server);

  public delegate void ServerFailedToRespond();

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalServerResponded(gameserveritem_t server);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalServerFailedToRespond();

  [StructLayout(LayoutKind.Sequential)]
  public class VTable
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
  }
}
