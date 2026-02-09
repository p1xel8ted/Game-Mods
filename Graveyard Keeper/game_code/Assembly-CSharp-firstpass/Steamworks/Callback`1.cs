// Decompiled with JetBrains decompiler
// Type: Steamworks.Callback`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

public sealed class Callback<T> : IDisposable
{
  public CCallbackBaseVTable m_CallbackBaseVTable;
  public IntPtr m_pVTable = IntPtr.Zero;
  public CCallbackBase m_CCallbackBase;
  public GCHandle m_pCCallbackBase;
  public bool m_bGameServer;
  public int m_size = Marshal.SizeOf(typeof (T));
  public bool m_bDisposed;

  public event Callback<T>.DispatchDelegate m_Func;

  public static Callback<T> Create(Callback<T>.DispatchDelegate func) => new Callback<T>(func);

  public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
  {
    return new Callback<T>(func, true);
  }

  public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
  {
    this.m_bGameServer = bGameServer;
    this.BuildCCallbackBase();
    this.Register(func);
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
    this.Unregister();
    if (this.m_pVTable != IntPtr.Zero)
      Marshal.FreeHGlobal(this.m_pVTable);
    if (this.m_pCCallbackBase.IsAllocated)
      this.m_pCCallbackBase.Free();
    this.m_bDisposed = true;
  }

  public void Register(Callback<T>.DispatchDelegate func)
  {
    if (func == null)
      throw new Exception("Callback function must not be null.");
    if (((int) this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
      this.Unregister();
    if (this.m_bGameServer)
      this.SetGameserverFlag();
    this.m_Func = func;
    NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof (T)));
  }

  public void Unregister()
  {
    NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
  }

  public void SetGameserverFlag() => this.m_CCallbackBase.m_nCallbackFlags |= (byte) 2;

  public void OnRunCallback(IntPtr pvParam)
  {
    try
    {
      this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)));
    }
    catch (Exception ex)
    {
      CallbackDispatcher.ExceptionHandler(ex);
    }
  }

  public void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
  {
    try
    {
      this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)));
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
      m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
      m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
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

  public delegate void DispatchDelegate(T param);
}
