// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Services.MonoManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Services;

public class MonoManager : MonoBehaviour
{
  public static bool isQuiting;
  public static MonoManager _current;

  public static MonoManager.UpdateMode updateMode
  {
    get
    {
      return !MonoManager.current.enabled ? MonoManager.UpdateMode.Manual : MonoManager.UpdateMode.Auto;
    }
    set => MonoManager.current.enabled = value == MonoManager.UpdateMode.Auto;
  }

  public event Action onUpdate;

  public event Action onLateUpdate;

  public event Action onFixedUpdate;

  public event Action onGUI;

  public event Action onApplicationQuit;

  public event Action<bool> onApplicationPause;

  public static MonoManager current
  {
    get
    {
      if ((UnityEngine.Object) MonoManager._current == (UnityEngine.Object) null && !MonoManager.isQuiting)
      {
        MonoManager._current = UnityEngine.Object.FindObjectOfType<MonoManager>();
        if ((UnityEngine.Object) MonoManager._current == (UnityEngine.Object) null)
          MonoManager._current = new GameObject("_MonoManager").AddComponent<MonoManager>();
      }
      return MonoManager._current;
    }
  }

  public static void Create() => MonoManager._current = MonoManager.current;

  public void OnApplicationQuit()
  {
    MonoManager.isQuiting = true;
    if (this.onApplicationQuit == null)
      return;
    this.onApplicationQuit();
  }

  public void OnApplicationPause(bool isPause)
  {
    if (this.onApplicationPause == null)
      return;
    this.onApplicationPause(isPause);
  }

  public void Awake()
  {
    if ((UnityEngine.Object) MonoManager._current != (UnityEngine.Object) null && (UnityEngine.Object) MonoManager._current != (UnityEngine.Object) this)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.gameObject);
    }
    else
    {
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      MonoManager._current = this;
    }
  }

  public void Update()
  {
    if (this.onUpdate == null)
      return;
    this.onUpdate();
  }

  public void LateUpdate()
  {
    if (this.onLateUpdate == null)
      return;
    this.onLateUpdate();
  }

  public void FixedUpdate()
  {
    if (this.onFixedUpdate == null)
      return;
    this.onFixedUpdate();
  }

  public void OnGUI()
  {
    if (this.onGUI == null)
      return;
    this.onGUI();
  }

  public enum UpdateMode
  {
    Auto,
    Manual,
  }
}
