// Decompiled with JetBrains decompiler
// Type: FlowScriptEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
public class FlowScriptEngine : MonoBehaviour
{
  public static FlowScriptEngine _me;
  public FlowScriptController[] _scripts;

  public void Awake()
  {
    FlowScriptEngine._me = this;
    this._scripts = this.GetComponentsInChildren<FlowScriptController>(true);
  }

  public static bool Assert()
  {
    if (!((Object) FlowScriptEngine._me == (Object) null))
      return false;
    Debug.LogError((object) "FlowScriptEngine has not been initialized");
    return true;
  }

  public static void SendEvent(string event_name)
  {
    if (FlowScriptEngine.Assert())
      return;
    Debug.Log((object) ("FlowScriptEngine.SendEvent: " + event_name));
    foreach (GraphOwner script in FlowScriptEngine._me._scripts)
      script.SendEvent(event_name);
  }

  public static void StopAllBehaviours()
  {
    if (FlowScriptEngine.Assert())
      return;
    foreach (GraphOwner script in FlowScriptEngine._me._scripts)
      script.StopBehaviour();
  }

  public static void StartAllBehaviours()
  {
    if (FlowScriptEngine.Assert())
      return;
    foreach (GraphOwner script in FlowScriptEngine._me._scripts)
      script.StartBehaviour();
  }
}
