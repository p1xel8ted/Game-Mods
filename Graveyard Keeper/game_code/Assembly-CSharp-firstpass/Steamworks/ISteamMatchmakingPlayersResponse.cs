// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingPlayersResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public class ISteamMatchmakingPlayersResponse
{
  public ISteamMatchmakingPlayersResponse.VTable m_VTable;
  public IntPtr m_pVTable;
  public GCHandle m_pGCHandle;
  public ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;
  public ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;
  public ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

  public ISteamMatchmakingPlayersResponse(
    ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList,
    ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond,
    ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
  {
    if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
      throw new ArgumentNullException();
    this.m_AddPlayerToList = onAddPlayerToList;
    this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
    this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
    this.m_VTable = new ISteamMatchmakingPlayersResponse.VTable()
    {
      m_VTAddPlayerToList = new ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(this.InternalOnAddPlayerToList),
      m_VTPlayersFailedToRespond = new ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
      m_VTPlayersRefreshComplete = new ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
    };
    this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingPlayersResponse.VTable)));
    Marshal.StructureToPtr<ISteamMatchmakingPlayersResponse.VTable>(this.m_VTable, this.m_pVTable, false);
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

  public void InternalOnAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed)
  {
    this.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
  }

  public void InternalOnPlayersFailedToRespond() => this.m_PlayersFailedToRespond();

  public void InternalOnPlayersRefreshComplete() => this.m_PlayersRefreshComplete();

  public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that)
  {
    return that.m_pGCHandle.AddrOfPinnedObject();
  }

  public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

  public delegate void PlayersFailedToRespond();

  public delegate void PlayersRefreshComplete();

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalPlayersFailedToRespond();

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void InternalPlayersRefreshComplete();

  [StructLayout(LayoutKind.Sequential)]
  public class VTable
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
  }
}
