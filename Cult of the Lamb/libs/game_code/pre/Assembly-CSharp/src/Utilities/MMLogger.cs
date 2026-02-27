// Decompiled with JetBrains decompiler
// Type: src.Utilities.MMLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
namespace src.Utilities;

public class MMLogger : MonoSingleton<MMLogger>
{
  private const string kFilename = "MMLog.log";
  private bool _enabled = true;
  private StreamWriter _fileStream;
  private List<string> _logQueue = new List<string>();

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
    this._logQueue.Add("Version: " + Application.version);
    string fileDirectory = MMLogger.GetFileDirectory();
    if (File.Exists(fileDirectory))
      File.WriteAllText(fileDirectory, string.Empty);
    this._fileStream = new StreamWriter(fileDirectory, true);
    this.StartCoroutine((IEnumerator) this.DoLogging());
  }

  private void OnDestroy() => this._fileStream.Close();

  private void OnEnable()
  {
    Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
  }

  private void OnDisable()
  {
    Application.logMessageReceived -= new Application.LogCallback(this.HandleLog);
  }

  private void HandleLog(string logString, string stackTrace, LogType type)
  {
    if (type != LogType.Error && type != LogType.Exception)
      return;
    this._logQueue.Add(logString);
    this._logQueue.Add(stackTrace);
  }

  private IEnumerator DoLogging()
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
