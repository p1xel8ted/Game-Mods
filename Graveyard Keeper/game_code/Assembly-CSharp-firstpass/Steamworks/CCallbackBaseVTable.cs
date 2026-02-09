// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBaseVTable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential)]
public class CCallbackBaseVTable
{
  public const CallingConvention cc = CallingConvention.StdCall;
  [MarshalAs(UnmanagedType.FunctionPtr)]
  [NonSerialized]
  public CCallbackBaseVTable.RunCRDel m_RunCallResult;
  [MarshalAs(UnmanagedType.FunctionPtr)]
  [NonSerialized]
  public CCallbackBaseVTable.RunCBDel m_RunCallback;
  [MarshalAs(UnmanagedType.FunctionPtr)]
  [NonSerialized]
  public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void RunCBDel(IntPtr pvParam);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void RunCRDel(IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);

  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate int GetCallbackSizeBytesDel();
}
