// Decompiled with JetBrains decompiler
// Type: Steamworks.CallResult`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public sealed class CallResult<T> : IDisposable
{
  public CCallbackBaseVTable m_CallbackBaseVTable;
  public IntPtr m_pVTable = IntPtr.Zero;
  public CCallbackBase m_CCallbackBase;
  public GCHandle m_pCCallbackBase;
  public SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;
  public int m_size = Marshal.SizeOf(typeof (T));
  public bool m_bDisposed;

  public event CallResult<T>.APIDispatchDelegate m_Func;

  public SteamAPICall_t Handle => this.m_hAPICall;

  public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
  {
    return new CallResult<T>(func);
  }

  public CallResult(CallResult<T>.APIDispatchDelegate func = null)
  {
    this.m_Func = func;
    this.BuildCCallbackBase();
  }

  void object.Finalize()
  {
    try
    {
      this.Dispose();
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void Dispose()
  {
    if (this.m_bDisposed)
      return;
    GC.SuppressFinalize((object) this);
    this.Cancel();
    if (this.m_pVTable != IntPtr.Zero)
      Marshal.FreeHGlobal(this.m_pVTable);
    if (this.m_pCCallbackBase.IsAllocated)
      this.m_pCCallbackBase.Free();
    this.m_bDisposed = true;
  }

  public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
  {
    if (func != null)
      this.m_Func = func;
    if (this.m_Func == null)
      throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or via Set()");
    if (this.m_hAPICall != SteamAPICall_t.Invalid)
      NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) this.m_hAPICall);
    this.m_hAPICall = hAPICall;
    if (!(hAPICall != SteamAPICall_t.Invalid))
      return;
    NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) hAPICall);
  }

  public bool IsActive() => this.m_hAPICall != SteamAPICall_t.Invalid;

  public void Cancel()
  {
    if (!(this.m_hAPICall != SteamAPICall_t.Invalid))
      return;
    NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) this.m_hAPICall);
    this.m_hAPICall = SteamAPICall_t.Invalid;
  }

  public void SetGameserverFlag() => this.m_CCallbackBase.m_nCallbackFlags |= (byte) 2;

  public void OnRunCallback(IntPtr pvParam)
  {
    this.m_hAPICall = SteamAPICall_t.Invalid;
    try
    {
      this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)), false);
    }
    catch (Exception ex)
    {
      CallbackDispatcher.ExceptionHandler(ex);
    }
  }

  public void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall_)
  {
    if (!((SteamAPICall_t) hSteamAPICall_ == this.m_hAPICall))
      return;
    this.m_hAPICall = SteamAPICall_t.Invalid;
    try
    {
      this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)), bFailed);
    }
    catch (Exception ex)
    {
      CallbackDispatcher.ExceptionHandler(ex);
    }
  }

  public int OnGetCallbackSizeBytes() => this.m_size;

  public void BuildCCallbackBase()
  {
    this.m_CallbackBaseVTable = new CCallbackBaseVTable()
    {
      m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
      m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
      m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
    };
    this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (CCallbackBaseVTable)));
    Marshal.StructureToPtr<CCallbackBaseVTable>(this.m_CallbackBaseVTable, this.m_pVTable, false);
    this.m_CCallbackBase = new CCallbackBase()
    {
      m_vfptr = this.m_pVTable,
      m_nCallbackFlags = (byte) 0,
      m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof (T))
    };
    this.m_pCCallbackBase = GCHandle.Alloc((object) this.m_CCallbackBase, GCHandleType.Pinned);
  }

  public delegate void APIDispatchDelegate(T param, bool bIOFailure);
}
