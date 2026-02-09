// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.DebugLogText
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Display a UI label on the agent's position if seconds to run is not 0 and also logs the message")]
[Category("✫ Utility")]
public class DebugLogText : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<string> log = (BBParameter<string>) "Hello World";
  public float labelYOffset;
  public float secondsToRun = 1f;
  public DebugLogText.LogMode logMode;
  public CompactStatus finishStatus = CompactStatus.Success;
  public Texture2D _tex;

  public override string info
  {
    get
    {
      return $"Log {this.log.ToString()}{((double) this.secondsToRun > 0.0 ? $" for {this.secondsToRun.ToString()} sec." : "")}";
    }
  }

  public override void OnExecute()
  {
    string message = $"(<b>{this.agent.gameObject.name}</b>) {this.log.value}";
    if (this.logMode == DebugLogText.LogMode.Log)
      Debug.Log((object) message, (UnityEngine.Object) this.agent.gameObject);
    if (this.logMode == DebugLogText.LogMode.Warning)
      Debug.LogWarning((object) message, (UnityEngine.Object) this.agent.gameObject);
    if (this.logMode == DebugLogText.LogMode.Error)
      Debug.LogError((object) message, (UnityEngine.Object) this.agent.gameObject);
    if ((double) this.secondsToRun <= 0.0)
      return;
    MonoManager.current.onGUI += new Action(this.OnGUI);
  }

  public override void OnStop()
  {
    if ((double) this.secondsToRun <= 0.0)
      return;
    MonoManager.current.onGUI -= new Action(this.OnGUI);
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.secondsToRun)
      return;
    this.EndAction(this.finishStatus == CompactStatus.Success);
  }

  public Texture2D tex
  {
    get
    {
      if ((UnityEngine.Object) this._tex == (UnityEngine.Object) null)
      {
        this._tex = new Texture2D(1, 1);
        this._tex.SetPixel(0, 0, Color.white);
        this._tex.Apply();
      }
      return this._tex;
    }
  }

  public void OnGUI()
  {
    if ((UnityEngine.Object) Camera.main == (UnityEngine.Object) null)
      return;
    Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.agent.position + new Vector3(0.0f, this.labelYOffset, 0.0f));
    Vector2 vector2 = new GUIStyle((GUIStyle) "label").CalcSize(new GUIContent(this.log.value));
    Rect position = new Rect(screenPoint.x - vector2.x / 2f, (float) Screen.height - screenPoint.y, vector2.x + 10f, vector2.y);
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.DrawTexture(position, (Texture) this.tex);
    GUI.color = new Color(0.2f, 0.2f, 0.2f, 1f);
    position.x += 4f;
    GUI.Label(position, this.log.value);
    GUI.color = Color.white;
  }

  public enum LogMode
  {
    Log,
    Warning,
    Error,
  }
}
