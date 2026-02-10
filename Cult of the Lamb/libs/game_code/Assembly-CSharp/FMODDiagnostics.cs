// Decompiled with JetBrains decompiler
// Type: FMODDiagnostics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#nullable disable
public class FMODDiagnostics : MonoBehaviour
{
  [Header("Display Settings")]
  public bool ShowCPU = true;
  public bool ShowChannels = true;
  public bool ShowBanks = true;
  public bool ShowActiveEvents;
  [Header("Overlay Settings")]
  public Vector2 position = new Vector2(10f, 10f);
  public float lineHeight = 18f;
  public Color textColor = Color.green;
  public GUIStyle _style;
  public List<string> _lines = new List<string>();

  public void OnGUI()
  {
    if (this._style == null)
      this._style = new GUIStyle(GUI.skin.label)
      {
        fontSize = 12,
        normal = {
          textColor = this.textColor
        }
      };
    this._lines.Clear();
    if (this.ShowCPU)
      this.LogCPUUsage();
    if (this.ShowChannels)
      this.LogChannels();
    if (this.ShowBanks)
      this.LogBanks();
    if (this.ShowActiveEvents)
      this.LogActiveEvents();
    float y = this.position.y;
    foreach (string line in this._lines)
    {
      GUI.Label(new Rect(this.position.x, y, 600f, this.lineHeight), line, this._style);
      y += this.lineHeight;
    }
  }

  public void LogCPUUsage()
  {
    MethodInfo method = typeof (FMOD.System).GetMethod("getCPUUsage", new System.Type[5]
    {
      typeof (float).MakeByRefType(),
      typeof (float).MakeByRefType(),
      typeof (float).MakeByRefType(),
      typeof (float).MakeByRefType(),
      typeof (float).MakeByRefType()
    });
    if (method != (MethodInfo) null)
    {
      object[] parameters = new object[5];
      if ((RESULT) method.Invoke((object) RuntimeManager.CoreSystem, parameters) != RESULT.OK)
        return;
      this._lines.Add($"[FMOD CPU] DSP: {(float) parameters[0]:F2}% | Stream: {(float) parameters[1]:F2}% | Update: {(float) parameters[3]:F2}%");
    }
    else
    {
      FMOD.CPU_USAGE usage;
      if (RuntimeManager.CoreSystem.getCPUUsage(out usage) != RESULT.OK)
        return;
      this._lines.Add($"[FMOD CPU] DSP: {usage.dsp:F2}% | Stream: {usage.stream:F2}% | Update: {usage.update:F2}%");
    }
  }

  public void LogChannels()
  {
    int channels;
    int realchannels;
    if (RuntimeManager.CoreSystem.getChannelsPlaying(out channels, out realchannels) != RESULT.OK)
      return;
    this._lines.Add($"[FMOD Channels] Playing: {channels}/{realchannels}");
  }

  public void LogBanks()
  {
    Bank[] array;
    if (RuntimeManager.StudioSystem.getBankList(out array) != RESULT.OK)
      return;
    this._lines.Add($"[FMOD Banks] Loaded: {array.Length}");
    foreach (Bank bank in array)
    {
      string path;
      if (bank.isValid() && bank.getPath(out path) == RESULT.OK)
        this._lines.Add("   - " + Path.GetFileName(path));
    }
  }

  public void LogActiveEvents()
  {
    Bank[] array1;
    if (RuntimeManager.StudioSystem.getBankList(out array1) != RESULT.OK)
      return;
    foreach (Bank bank in array1)
    {
      EventDescription[] array2;
      if (bank.isValid() && bank.getEventList(out array2) == RESULT.OK)
      {
        UnityEngine.Debug.Log((object) $"Bank: {bank.getPath(out string _)}");
        foreach (EventDescription eventDescription in array2)
        {
          string path1;
          int path2 = (int) eventDescription.getPath(out path1);
          UnityEngine.Debug.Log((object) ("  Event: " + path1));
        }
      }
    }
  }
}
