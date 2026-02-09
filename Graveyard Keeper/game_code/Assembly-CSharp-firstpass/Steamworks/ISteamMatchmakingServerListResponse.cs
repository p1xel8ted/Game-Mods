// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingServerListResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public class ISteamMatchmakingServerListResponse
{
  public ISteamMatchmakingServerListResponse.VTable m_VTable;
  public IntPtr m_pVTable;
  public GCHandle m_pGCHandle;
  public ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;
  public ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;
  public ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

  public ISteamMatchmakingServerListResponse(
    ISteamMatchmakingServerListResponse.ServerResponded onServerResponded,
    ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond,
    ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
  {
    if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
      throw new ArgumentNullException();
    this.m_ServerResponded = onServerResponded;
    this.m_ServerFailedToRespond = onServerFailedToRespond;
    this.m_RefreshComplete = onRefreshComplete;
    this.m_VTable = new ISteamMatchmakingServerListResponse.VTable()
    {
      m_VTServerResponded = new ISteamMatchmakingServerListResponse.InternalServerResponded(this.InternalOnServerResponded),
      m_VTServerFailedToRespond = new ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
      m_VTRefreshComplete = new ISteamMatchmakingServerListResponse.InternalRefreshComplete(this.InternalOnRefreshComplete)
    };
    this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingServerListResponse.VTable)));
    Marshal.StructureToPtr<ISteamMatchmakingServerListResponse.VTable>(this.m_VTable, this.m_pVTable, false);
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

  public void InternalOnServerResponded(HServerListRequest hRequest, int iServer)
  {
    this.m_ServerResponded(hRequest, iServer);
  }

  public void InternalOnServerFailedToRespond(HServerListRequest hRequest, int iServer)
  {
    this.m_ServerFailedToRespond(hRequest, iServer);
  }

  public void InternalOnRefreshComplete(
    HServerListRequest hRequest,
    EMatchMakingServerResponse response)
  {
    this.m_RefreshComplete(hRequest, response);
  }

  public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that)
  {
    return that.m_pGCHandle.AddrOfPinnedObject();
  }

  public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

  public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

  public delegate void RefreshComplete(
    HServerListRequest hRequest,
    EMatchMakingServerResponse response);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalServerResponded(HServerListRequest hRequest, int iServer);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalServerFailedToRespond(HServerListRequest hRequest, int iServer);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalRefreshComplete(
    HServerListRequest hRequest,
    EMatchMakingServerResponse response);

  [StructLayout(LayoutKind.Sequential)]
  public class VTable
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
  }
}
