// Decompiled with JetBrains decompiler
// Type: SwitchPerformanceLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using System;
using System.Text;
using Unify;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class SwitchPerformanceLog : MonoBehaviour
{
  public const int FrameInterval = 30;
  public const string MSFormat = "0.000";
  public const string FloatFormat = "0.00";
  public const string _mountName = "test";
  public const int _baseBiome = 13;
  public bool _recording;
  public bool _isLoading;
  public bool _discardFrame;
  public string _nickname;
  public string _fileName;
  public string _filePath;
  public string _startTime;
  public int _dungeonNumber;
  public int _lastValidDungeonNumber;
  public int _layerNumber;
  public int _floorNumber;
  public int _performanceModeOn;
  public int _currentRoomSeed;
  public int _roomType;
  public int _enemyCount;
  public float3 _playerPositionXYZ;
  public float _fpsCurrent;
  public float _fpsAverage;
  public float _fpsCurrentMin;
  public float _fpsMin;
  public float _cpuAvgMs;
  public float _gpuAvgMs;
  public float _cumulativeCpuTime;
  public float _cumulativeGpuTime;
  public string _spineWaitTimeMS;
  public int _currentDecalCount;
  public int _achivementCalls;
  public double _simAvgMs;
  public StringBuilder _CSVData;
  public FrameTiming[] currentFrame = new FrameTiming[1];
  public const float _interval = 0.5f;
  public float _elapsedTime;
  public float _accumulator;
  public int _frame;

  public string GetFormatedTime() => DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

  public bool FrameBefore() => (Time.frameCount + 1) % 30 == 0;

  public bool WriteThisFrame() => Time.frameCount % 30 == 0;

  public bool IsBossRoom() => BiomeGenerator.Instance.CurrentRoom.IsBoss;

  public void OnEnable()
  {
    SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnSceneChange);
    MMTransition.OnBeginTransition += new System.Action(this.OnLoadStart);
    MMTransition.OnTransitionCompelte += new System.Action(this.OnLoadComplete);
  }

  public void OnDisable()
  {
    SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(this.OnSceneChange);
    MMTransition.OnBeginTransition -= new System.Action(this.OnLoadStart);
    MMTransition.OnTransitionCompelte -= new System.Action(this.OnLoadComplete);
  }

  public void OnLoadStart() => this._isLoading = true;

  public void OnLoadComplete()
  {
    this._isLoading = false;
    this._discardFrame = true;
  }

  public void OnSceneChange(Scene last, Scene active)
  {
    if (this.IsDungeon(active))
    {
      this.StartRecordingFile();
      Debug.LogError((object) "<color=orange>BEGINNING DATA CAPTURE</color>");
    }
    else
    {
      if (!this._recording)
        return;
      this.StopRecordingFile();
      Debug.LogError((object) "<color=orange>DATA CAPTURE ENDED</color>");
    }
  }

  public void StartRecordingFile()
  {
    this._nickname = UserHelper.GetPlayer(0).nickName;
    this._CSVData = new StringBuilder();
    this.WriteCSVKeys();
    this._recording = true;
    this._startTime = this.GetFormatedTime();
  }

  public void StopRecordingFile()
  {
    this._recording = false;
    this.Save();
  }

  public void Update()
  {
    this.TrackFPS();
    if (!this._recording || this._isLoading)
      return;
    this.RecordCpuGpuTimes();
    if (this.FrameBefore())
    {
      this.GatherData();
    }
    else
    {
      if (!this.WriteThisFrame())
        return;
      this.WriteFrame();
    }
  }

  public void RecordCpuGpuTimes()
  {
    FrameTimingManager.CaptureFrameTimings();
    int latestTimings = (int) FrameTimingManager.GetLatestTimings(1U, this.currentFrame);
    this._cumulativeCpuTime += (float) this.currentFrame[0].cpuFrameTime;
    this._cumulativeGpuTime += (float) this.currentFrame[0].gpuFrameTime;
  }

  public void TrackFPS()
  {
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    ++this._frame;
    if ((double) unscaledDeltaTime == 0.0)
    {
      this._fpsMin = 0.0f;
    }
    else
    {
      this._fpsCurrent = 1f / unscaledDeltaTime;
      this._fpsCurrent = (double) this._fpsCurrent > 60.0 ? 60f : this._fpsCurrent;
      if ((double) this._fpsCurrent < (double) this._fpsMin)
        this._fpsMin = this._fpsCurrent;
      this._accumulator += this._fpsCurrent;
      this._elapsedTime += unscaledDeltaTime;
      if ((double) this._elapsedTime <= 0.5)
        return;
      this._fpsAverage = this._accumulator / (float) this._frame;
      this._fpsCurrentMin = this._fpsMin;
      this._elapsedTime = 0.0f;
      this._accumulator = 0.0f;
      this._fpsMin = 60f;
      this._frame = 0;
    }
  }

  public void GatherData()
  {
    if (this._isLoading || this._discardFrame)
    {
      this._cumulativeCpuTime = 0.0f;
      this._cumulativeGpuTime = 0.0f;
      AchievementsWrapper.UnlockAchivementCount = 0;
      this._discardFrame = false;
    }
    else
    {
      this._lastValidDungeonNumber = this._dungeonNumber;
      if (this._dungeonNumber == 13)
      {
        this._currentRoomSeed = 0;
        this._roomType = 0;
      }
      else
      {
        this._currentRoomSeed = BiomeGenerator.Instance.CurrentRoom.Seed;
        this._roomType = this.IsBossRoom() ? 1 : 0;
      }
      this._layerNumber = GameManager.CurrentDungeonLayer;
      this._floorNumber = GameManager.CurrentDungeonFloor;
      this._performanceModeOn = PerformanceModeManager.IsPerformanceMode() ? 1 : 0;
      this._enemyCount = Health.team2.Count;
      this._playerPositionXYZ = (float3) (Health.playerTeam.Count > 0 ? Health.playerTeam[0].transform.position : Vector3.zero);
      this._cpuAvgMs = this._cumulativeCpuTime / 30f;
      this._gpuAvgMs = this._cumulativeGpuTime / 30f;
      this._cumulativeCpuTime = 0.0f;
      this._cumulativeGpuTime = 0.0f;
      this._achivementCalls = AchievementsWrapper.UnlockAchivementCount;
      AchievementsWrapper.UnlockAchivementCount = 0;
    }
  }

  public void WriteCSVKeys()
  {
    this._CSVData.Append("dungeon #,");
    this._CSVData.Append("layer #,");
    this._CSVData.Append("floor #,");
    this._CSVData.Append("p mode on,");
    this._CSVData.Append("room seed,");
    this._CSVData.Append("room type,");
    this._CSVData.Append("enemy count,");
    this._CSVData.Append("lamb pos,");
    this._CSVData.Append("fps average,");
    this._CSVData.Append("fps min,");
    this._CSVData.Append("avg cpu frq,");
    this._CSVData.Append("avg gpu frq,");
    this._CSVData.Append("spine wait ms,");
    this._CSVData.Append("decal count,");
    this._CSVData.Append("achivement calls,");
    this._CSVData.Append("sim avg ms,");
    this._CSVData.Append("\n");
  }

  public void WriteFrame()
  {
    if (this._isLoading || this._discardFrame)
      return;
    this._CSVData.Append($"{this._dungeonNumber},");
    this._CSVData.Append($"{this._layerNumber},");
    this._CSVData.Append($"{this._floorNumber},");
    this._CSVData.Append($"{this._performanceModeOn},");
    this._CSVData.Append($"{this._currentRoomSeed},");
    this._CSVData.Append($"{this._roomType},");
    this._CSVData.Append($"{this._enemyCount},");
    this._CSVData.Append($"({this._playerPositionXYZ.x.ToString("0.00")} {this._playerPositionXYZ.y.ToString("0.00")} {this._playerPositionXYZ.z.ToString("0.00")}),");
    this._CSVData.Append(this._fpsAverage.ToString("0.000") + ",");
    this._CSVData.Append(this._fpsCurrentMin.ToString("0.000") + ",");
    this._CSVData.Append(this._cpuAvgMs.ToString("0.00") + ",");
    this._CSVData.Append(this._gpuAvgMs.ToString("0.00") + ",");
    this._CSVData.Append(this._spineWaitTimeMS + ",");
    this._CSVData.Append($"{this._currentDecalCount},");
    this._CSVData.Append($"{this._achivementCalls},");
    this._CSVData.Append(this._simAvgMs.ToString("0.000") + ",");
    this._CSVData.Append("\n");
  }

  public bool IsDungeon(Scene active)
  {
    int dungeonNumber = this._dungeonNumber;
    switch (active.name)
    {
      case "Base Biome 1":
        this._dungeonNumber = 13;
        break;
      case "Dungeon Final":
        this._dungeonNumber = 6;
        break;
      case "Dungeon Sandbox":
        this._dungeonNumber = 5;
        break;
      case "Dungeon1":
        this._dungeonNumber = 1;
        break;
      case "Dungeon2":
        this._dungeonNumber = 2;
        break;
      case "Dungeon3":
        this._dungeonNumber = 3;
        break;
      case "Dungeon4":
        this._dungeonNumber = 4;
        break;
      case "Game Biome Intro":
        this._dungeonNumber = 0;
        break;
      default:
        this._dungeonNumber = -1;
        break;
    }
    return this._dungeonNumber > -1;
  }

  public void Save()
  {
  }
}
