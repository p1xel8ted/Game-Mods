// Decompiled with JetBrains decompiler
// Type: Microsoft.Websocket
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

#nullable disable
namespace Microsoft;

public class Websocket : MonoBehaviour, IDisposable
{
  public bool disposedValue;
  public Uri uri;
  public IntPtr websocketHandle = IntPtr.Zero;
  public Thread websocketThread;
  public object socketLock = new object();
  public static Dictionary<IntPtr, Websocket> socketsByHandle = new Dictionary<IntPtr, Websocket>();
  public Queue<object> websocketEvents = new Queue<object>();
  public Queue<object> processingEvents = new Queue<object>();
  public bool open;

  public event EventHandler OnOpen;

  public event EventHandler<MessageEventArgs> OnMessage;

  public event EventHandler<ErrorEventArgs> OnError;

  public event EventHandler<CloseEventArgs> OnClose;

  public void Update()
  {
    if (this.websocketEvents.Count <= 0)
      return;
    lock (this.websocketEvents)
    {
      this.processingEvents = this.websocketEvents;
      this.websocketEvents = new Queue<object>();
    }
    while (this.processingEvents.Count > 0)
    {
      object obj = this.processingEvents.Dequeue();
      if (obj is string)
      {
        if (!this.open)
        {
          this.open = true;
          this.RaiseConnect(obj as string);
        }
        else
          this.RaiseMessage(obj as string);
      }
      else if (obj is Reason)
      {
        Reason reason = obj as Reason;
        if (reason.isClose)
          this.RaiseClose(reason.code, reason.reason);
        else
          this.RaiseError(reason.code, reason.reason);
      }
    }
  }

  public void OnDestroy() => this.Close();

  public void Open(Uri uri) => this.Open(uri, (Dictionary<string, string>) null);

  public void Open(Uri uri, Dictionary<string, string> headers)
  {
    this.uri = uri;
    int ret = Websocket.create_websocket(ref this.websocketHandle);
    if (ret != 0)
      throw new WebsocketException("Failed to create websocket.");
    Websocket.socketsByHandle.Add(this.websocketHandle, this);
    if (headers != null)
    {
      foreach (KeyValuePair<string, string> header in headers)
      {
        ret = Websocket.add_header(this.websocketHandle, header.Key, header.Value);
        if (ret != 0)
          throw new WebsocketException($"Failed to add header [{header.Key}]: {header.Value}");
      }
    }
    this.websocketThread = new Thread((ThreadStart) (() =>
    {
      try
      {
        ret = Websocket.open_websocket(this.websocketHandle, this.uri.AbsoluteUri, new Websocket.OnConnectDelegate(Websocket.OnConnectHandler), new Websocket.OnMessageDelegate(Websocket.OnMessageHandler), new Websocket.OnErrorDelegate(Websocket.OnErrorHandler), new Websocket.OnCloseDelegate(Websocket.OnCloseHandler));
        if (ret != 0)
          throw new WebsocketException("Failed to open websocket.");
      }
      catch (Exception ex)
      {
        if (ex is WebsocketException)
          throw;
        Debug.LogException(ex);
      }
    }));
    this.websocketThread.Start();
  }

  public void Send(string message)
  {
    lock (this.socketLock)
    {
      int num = !(this.websocketHandle == IntPtr.Zero) ? Websocket.write_websocket(this.websocketHandle, message) : throw new WebsocketException("Socket closed.");
      if (num != 0)
        throw new WebsocketException("Send failed: " + num.ToString());
    }
  }

  public void Close() => this.Dispose();

  public virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (this.websocketHandle != IntPtr.Zero)
    {
      if (Websocket.close_websocket(this.websocketHandle) != 0)
        this.websocketThread.Abort();
      if (disposing)
        Websocket.socketsByHandle.Remove(this.websocketHandle);
      this.websocketHandle = IntPtr.Zero;
      this.websocketThread.Join();
    }
    this.disposedValue = true;
  }

  void object.Finalize()
  {
    try
    {
      this.Dispose(false);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  [MonoPInvokeCallback(typeof (Websocket.OnConnectDelegate))]
  public static void OnConnectHandler(
    IntPtr websocketHandle,
    [MarshalAs(UnmanagedType.LPStr)] string connectMessage,
    uint connectMessageSize)
  {
    Websocket websocket = (Websocket) null;
    Websocket.socketsByHandle.TryGetValue(websocketHandle, out websocket);
    if ((UnityEngine.Object) null == (UnityEngine.Object) websocket)
    {
      Debug.LogError((object) "Failed to find Websocket instance for this callback.");
    }
    else
    {
      lock (websocket.websocketEvents)
        websocket.websocketEvents.Enqueue((object) connectMessage);
    }
  }

  [MonoPInvokeCallback(typeof (Websocket.OnMessageDelegate))]
  public static void OnMessageHandler(IntPtr websocketHandle, [MarshalAs(UnmanagedType.LPStr)] string message, uint messageSize)
  {
    Websocket websocket = (Websocket) null;
    Websocket.socketsByHandle.TryGetValue(websocketHandle, out websocket);
    if ((UnityEngine.Object) null == (UnityEngine.Object) websocket)
    {
      Debug.LogError((object) "Failed to find Websocket instance for this callback.");
    }
    else
    {
      lock (websocket.websocketEvents)
        websocket.websocketEvents.Enqueue((object) message);
    }
  }

  [MonoPInvokeCallback(typeof (Websocket.OnErrorDelegate))]
  public static void OnErrorHandler(
    IntPtr websocketHandle,
    ushort code,
    [MarshalAs(UnmanagedType.LPStr)] string message,
    uint messageSize)
  {
    Websocket websocket = (Websocket) null;
    Websocket.socketsByHandle.TryGetValue(websocketHandle, out websocket);
    if ((UnityEngine.Object) null == (UnityEngine.Object) websocket)
    {
      Debug.LogError((object) "Failed to find Websocket instance for this callback.");
    }
    else
    {
      lock (websocket.websocketEvents)
        websocket.websocketEvents.Enqueue((object) new Reason(code, message, false));
    }
  }

  [MonoPInvokeCallback(typeof (Websocket.OnCloseDelegate))]
  public static void OnCloseHandler(
    IntPtr websocketHandle,
    ushort code,
    [MarshalAs(UnmanagedType.LPStr)] string reason,
    uint reasonSize)
  {
    Websocket websocket = (Websocket) null;
    Websocket.socketsByHandle.TryGetValue(websocketHandle, out websocket);
    if ((UnityEngine.Object) null == (UnityEngine.Object) websocket)
    {
      Debug.LogError((object) "Failed to find Websocket instance for this callback.");
    }
    else
    {
      lock (websocket.websocketEvents)
        websocket.websocketEvents.Enqueue((object) new Reason(code, reason, false));
    }
  }

  public void RaiseConnect([MarshalAs(UnmanagedType.LPStr)] string connectMessage)
  {
    if (this.disposedValue)
      return;
    EventHandler onOpen = this.OnOpen;
    if (onOpen == null)
      return;
    onOpen((object) this, (EventArgs) null);
  }

  public void RaiseMessage([MarshalAs(UnmanagedType.LPStr)] string message)
  {
    if (this.disposedValue)
      return;
    EventHandler<MessageEventArgs> onMessage = this.OnMessage;
    if (onMessage == null)
      return;
    onMessage((object) this, new MessageEventArgs(message));
  }

  public void RaiseError(ushort errorCode, string errorMessage)
  {
    if (this.disposedValue)
      return;
    EventHandler<ErrorEventArgs> onError = this.OnError;
    if (onError == null)
      return;
    onError((object) this, new ErrorEventArgs((int) errorCode, errorMessage));
  }

  public void RaiseClose(ushort code, [MarshalAs(UnmanagedType.LPStr)] string reason)
  {
    if (this.disposedValue)
      return;
    EventHandler<CloseEventArgs> onClose = this.OnClose;
    if (onClose == null)
      return;
    onClose((object) this, new CloseEventArgs((int) code, reason));
  }

  [DllImport("simplewebsocket")]
  public static extern int create_websocket(ref IntPtr websocketHandlePtr);

  [DllImport("simplewebsocket")]
  public static extern int add_header(IntPtr websocketHandle, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

  [DllImport("simplewebsocket")]
  public static extern int open_websocket(
    IntPtr websocketHandle,
    [MarshalAs(UnmanagedType.LPStr)] string uri,
    Websocket.OnConnectDelegate onConnect,
    Websocket.OnMessageDelegate onMessage,
    Websocket.OnErrorDelegate onError,
    Websocket.OnCloseDelegate onClose);

  [DllImport("simplewebsocket")]
  public static extern int write_websocket(IntPtr websocketHandle, [MarshalAs(UnmanagedType.LPStr)] string message);

  [DllImport("simplewebsocket")]
  public static extern int read_websocket(
    IntPtr websocketHandle,
    Websocket.OnMessageDelegate onMessage);

  [DllImport("simplewebsocket")]
  public static extern int close_websocket(IntPtr websocketHandle);

  public delegate void OnConnectDelegate(
    IntPtr websocketHandle,
    [MarshalAs(UnmanagedType.LPStr)] string connectMessage,
    uint connectMessageSize);

  public delegate void OnMessageDelegate(IntPtr websocketHandle, [MarshalAs(UnmanagedType.LPStr)] string message, uint messageSize);

  public delegate void OnErrorDelegate(
    IntPtr websocketHandle,
    ushort errorCode,
    [MarshalAs(UnmanagedType.LPStr)] string errorMessage,
    uint errorMessageSize);

  public delegate void OnCloseDelegate(
    IntPtr websocketHandle,
    ushort code,
    [MarshalAs(UnmanagedType.LPStr)] string reason,
    uint reasonSize);
}
