// Decompiled with JetBrains decompiler
// Type: src.Utilities.MMLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
namespace src.Utilities;

public class MMLogger : MonoSingleton<MMLogger>
{
  public const string kFilename = "MMLog.log";
  public bool _enabled = true;
  public StreamWriter _fileStream;
  public List<string> _logQueue = new List<string>();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void Initialize()
  {
    GameObject gameObject = new GameObject();
    gameObject.AddComponent<MMLogger>();
    gameObject.name = nameof (MMLogger);
  }

  public bool Enabled
  {
    set
    {
      if (this._enabled == value)
        return;
      this._enabled = value;
      if (!this._enabled)
        this._fileStream.Close();
      else
        this._fileStream = new StreamWriter(MMLogger.GetFileDirectory(), true);
    }
  }

  public override void Start()
  {
    base.Start();
    Debug.Log((object) "Start MMLogger".Colour(Color.cyan));
    this.AddToLog("Version: " + Application.version);
    string fileDirectory = MMLogger.GetFileDirectory();
    if (File.Exists(fileDirectory))
      File.WriteAllText(fileDirectory, string.Empty);
    this._fileStream = new StreamWriter(fileDirectory, true);
    this.StartCoroutine((IEnumerator) this.DoLogging());
  }

  public void OnDestroy() => this._fileStream.Close();

  public void OnEnable()
  {
    Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
  }

  public void OnDisable()
  {
    Application.logMessageReceived -= new Application.LogCallback(this.HandleLog);
  }

  public void HandleLog(string logString, string stackTrace, LogType type)
  {
    if (type != LogType.Error && type != LogType.Exception)
      return;
    this.AddToLog(logString);
    this.AddToLog(stackTrace);
  }

  public void AddToLog(string str)
  {
    this._logQueue.Add($"[{DateTime.Now.ToShortTimeString()}] " + str);
  }

  public IEnumerator DoLogging()
  {
    while (true)
    {
      do
      {
        yield return (object) null;
      }
      while (!this._enabled || this._logQueue.Count <= 0);
      for (int index = 0; index < this._logQueue.Count; ++index)
        this._fileStream.WriteLine(this._logQueue[index]);
      this._logQueue.Clear();
    }
  }

  public static string GetFileDirectory()
  {
    return Path.Combine(Application.persistentDataPath, "MMLog.log");
  }
}
