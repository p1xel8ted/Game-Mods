// Decompiled with JetBrains decompiler
// Type: AstarDebugger
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Text;
using UnityEngine;

#nullable disable
[HelpURL("http://arongranberg.com/astar/docs/class_astar_debugger.php")]
[ExecuteInEditMode]
[AddComponentMenu("Pathfinding/Debugger")]
public class AstarDebugger : MonoBehaviour
{
  public int yOffset = 5;
  public bool show = true;
  public bool showInEditor;
  public bool showFPS;
  public bool showPathProfile;
  public bool showMemProfile;
  public bool showGraph;
  public int graphBufferSize = 200;
  public UnityEngine.Font font;
  public int fontSize = 12;
  public StringBuilder text = new StringBuilder();
  public string cachedText;
  public float lastUpdate = -999f;
  public AstarDebugger.GraphPoint[] graph;
  public float delayedDeltaTime = 1f;
  public float lastCollect;
  public float lastCollectNum;
  public float delta;
  public float lastDeltaTime;
  public int allocRate;
  public int lastAllocMemory;
  public float lastAllocSet = -9999f;
  public int allocMem;
  public int collectAlloc;
  public int peakAlloc;
  public int fpsDropCounterSize = 200;
  public float[] fpsDrops;
  public Rect boxRect;
  public GUIStyle style;
  public Camera cam;
  public float graphWidth = 100f;
  public float graphHeight = 100f;
  public float graphOffset = 50f;
  public int maxVecPool;
  public int maxNodePool;
  public AstarDebugger.PathTypeDebug[] debugTypes = new AstarDebugger.PathTypeDebug[1]
  {
    new AstarDebugger.PathTypeDebug("ABPath", (Func<int>) (() => PathPool.GetSize(typeof (ABPath))), (Func<int>) (() => PathPool.GetTotalCreated(typeof (ABPath))))
  };

  public void Start()
  {
    this.useGUILayout = false;
    this.fpsDrops = new float[this.fpsDropCounterSize];
    this.cam = this.GetComponent<Camera>();
    if ((UnityEngine.Object) this.cam == (UnityEngine.Object) null)
      this.cam = Camera.main;
    this.graph = new AstarDebugger.GraphPoint[this.graphBufferSize];
    for (int index = 0; index < this.fpsDrops.Length; ++index)
      this.fpsDrops[index] = 1f / Time.deltaTime;
  }

  public void Update()
  {
    if (!this.show || !Application.isPlaying && !this.showInEditor)
      return;
    int num1 = GC.CollectionCount(0);
    if ((double) this.lastCollectNum != (double) num1)
    {
      this.lastCollectNum = (float) num1;
      this.delta = Time.realtimeSinceStartup - this.lastCollect;
      this.lastCollect = Time.realtimeSinceStartup;
      this.lastDeltaTime = Time.deltaTime;
      this.collectAlloc = this.allocMem;
    }
    this.allocMem = (int) GC.GetTotalMemory(false);
    bool flag = this.allocMem < this.peakAlloc;
    this.peakAlloc = !flag ? this.allocMem : this.peakAlloc;
    if ((double) Time.realtimeSinceStartup - (double) this.lastAllocSet > 0.30000001192092896 || !Application.isPlaying)
    {
      int num2 = this.allocMem - this.lastAllocMemory;
      this.lastAllocMemory = this.allocMem;
      this.lastAllocSet = Time.realtimeSinceStartup;
      this.delayedDeltaTime = Time.deltaTime;
      if (num2 >= 0)
        this.allocRate = num2;
    }
    if (Application.isPlaying)
    {
      this.fpsDrops[Time.frameCount % this.fpsDrops.Length] = (double) Time.deltaTime != 0.0 ? 1f / Time.deltaTime : float.PositiveInfinity;
      int index = Time.frameCount % this.graph.Length;
      this.graph[index].fps = (double) Time.deltaTime < (double) Mathf.Epsilon ? 0.0f : 1f / Time.deltaTime;
      this.graph[index].collectEvent = flag;
      this.graph[index].memory = (float) this.allocMem;
    }
    if (!Application.isPlaying || !((UnityEngine.Object) this.cam != (UnityEngine.Object) null) || !this.showGraph)
      return;
    this.graphWidth = (float) this.cam.pixelWidth * 0.8f;
    float num3 = float.PositiveInfinity;
    float num4 = 0.0f;
    float num5 = float.PositiveInfinity;
    float num6 = 0.0f;
    for (int index = 0; index < this.graph.Length; ++index)
    {
      num3 = Mathf.Min(this.graph[index].memory, num3);
      num4 = Mathf.Max(this.graph[index].memory, num4);
      num5 = Mathf.Min(this.graph[index].fps, num5);
      num6 = Mathf.Max(this.graph[index].fps, num6);
    }
    int num7 = Time.frameCount % this.graph.Length;
    Matrix4x4 m = Matrix4x4.TRS(new Vector3((float) (((double) this.cam.pixelWidth - (double) this.graphWidth) / 2.0), this.graphOffset, 1f), Quaternion.identity, new Vector3(this.graphWidth, this.graphHeight, 1f));
    for (int index = 0; index < this.graph.Length - 1; ++index)
    {
      if (index != num7)
      {
        this.DrawGraphLine(index, m, (float) index / (float) this.graph.Length, (float) (index + 1) / (float) this.graph.Length, AstarMath.MapTo(num3, num4, this.graph[index].memory), AstarMath.MapTo(num3, num4, this.graph[index + 1].memory), Color.blue);
        this.DrawGraphLine(index, m, (float) index / (float) this.graph.Length, (float) (index + 1) / (float) this.graph.Length, AstarMath.MapTo(num5, num6, this.graph[index].fps), AstarMath.MapTo(num5, num6, this.graph[index + 1].fps), Color.green);
      }
    }
  }

  public void DrawGraphLine(
    int index,
    Matrix4x4 m,
    float x1,
    float x2,
    float y1,
    float y2,
    Color col)
  {
    Debug.DrawLine(this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x1, y1))), this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x2, y2))), col);
  }

  public void Cross(Vector3 p)
  {
    p = this.cam.cameraToWorldMatrix.MultiplyPoint(p);
    Debug.DrawLine(p - Vector3.up * 0.2f, p + Vector3.up * 0.2f, Color.red);
    Debug.DrawLine(p - Vector3.right * 0.2f, p + Vector3.right * 0.2f, Color.red);
  }

  public void OnGUI()
  {
    if (!this.show || !Application.isPlaying && !this.showInEditor)
      return;
    if (this.style == null)
    {
      this.style = new GUIStyle();
      this.style.normal.textColor = Color.white;
      this.style.padding = new RectOffset(5, 5, 5, 5);
    }
    float num1;
    if ((double) Time.realtimeSinceStartup - (double) this.lastUpdate > 0.5 || this.cachedText == null || !Application.isPlaying)
    {
      this.lastUpdate = Time.realtimeSinceStartup;
      this.boxRect = new Rect(5f, (float) this.yOffset, 310f, 40f);
      this.text.Length = 0;
      this.text.AppendLine("A* Pathfinding Project Debugger");
      this.text.Append("A* Version: ").Append(AstarPath.Version.ToString());
      if (this.showMemProfile)
      {
        this.boxRect.height += 200f;
        this.text.AppendLine();
        this.text.AppendLine();
        this.text.Append("Currently allocated".PadRight(25));
        this.text.Append(((float) this.allocMem / 1000000f).ToString("0.0 MB"));
        this.text.AppendLine();
        this.text.Append("Peak allocated".PadRight(25));
        StringBuilder text1 = this.text;
        num1 = (float) this.peakAlloc / 1000000f;
        string str1 = num1.ToString("0.0 MB");
        text1.Append(str1).AppendLine();
        this.text.Append("Last collect peak".PadRight(25));
        StringBuilder text2 = this.text;
        num1 = (float) this.collectAlloc / 1000000f;
        string str2 = num1.ToString("0.0 MB");
        text2.Append(str2).AppendLine();
        this.text.Append("Allocation rate".PadRight(25));
        StringBuilder text3 = this.text;
        num1 = (float) this.allocRate / 1000000f;
        string str3 = num1.ToString("0.0 MB");
        text3.Append(str3).AppendLine();
        this.text.Append("Collection frequency".PadRight(25));
        this.text.Append(this.delta.ToString("0.00"));
        this.text.Append("s\n");
        this.text.Append("Last collect fps".PadRight(25));
        StringBuilder text4 = this.text;
        num1 = 1f / this.lastDeltaTime;
        string str4 = num1.ToString("0.0 fps");
        text4.Append(str4);
        this.text.Append(" (");
        this.text.Append(this.lastDeltaTime.ToString("0.000 s"));
        this.text.Append(")");
      }
      if (this.showFPS)
      {
        this.text.AppendLine();
        this.text.AppendLine();
        StringBuilder stringBuilder = this.text.Append("FPS".PadRight(25));
        num1 = 1f / this.delayedDeltaTime;
        string str = num1.ToString("0.0 fps");
        stringBuilder.Append(str);
        float num2 = float.PositiveInfinity;
        for (int index = 0; index < this.fpsDrops.Length; ++index)
        {
          if ((double) this.fpsDrops[index] < (double) num2)
            num2 = this.fpsDrops[index];
        }
        this.text.AppendLine();
        this.text.Append($"Lowest fps (last {this.fpsDrops.Length.ToString()})".PadRight(25)).Append(num2.ToString("0.0"));
      }
      if (this.showPathProfile)
      {
        AstarPath active = AstarPath.active;
        this.text.AppendLine();
        if ((UnityEngine.Object) active == (UnityEngine.Object) null)
        {
          this.text.Append("\nNo AstarPath Object In The Scene");
        }
        else
        {
          if (ListPool<Vector3>.GetSize() > this.maxVecPool)
            this.maxVecPool = ListPool<Vector3>.GetSize();
          if (ListPool<GraphNode>.GetSize() > this.maxNodePool)
            this.maxNodePool = ListPool<GraphNode>.GetSize();
          this.text.Append("\nPool Sizes (size/total created)");
          for (int index = 0; index < this.debugTypes.Length; ++index)
            this.debugTypes[index].Print(this.text);
        }
      }
      this.cachedText = this.text.ToString();
    }
    if ((UnityEngine.Object) this.font != (UnityEngine.Object) null)
    {
      this.style.font = this.font;
      this.style.fontSize = this.fontSize;
    }
    this.boxRect.height = this.style.CalcHeight(new GUIContent(this.cachedText), this.boxRect.width);
    GUI.Box(this.boxRect, "");
    GUI.Label(this.boxRect, this.cachedText, this.style);
    if (!this.showGraph)
      return;
    float num3 = float.PositiveInfinity;
    float num4 = 0.0f;
    float num5 = float.PositiveInfinity;
    float num6 = 0.0f;
    for (int index = 0; index < this.graph.Length; ++index)
    {
      num3 = Mathf.Min(this.graph[index].memory, num3);
      num4 = Mathf.Max(this.graph[index].memory, num4);
      num5 = Mathf.Min(this.graph[index].fps, num5);
      num6 = Mathf.Max(this.graph[index].fps, num6);
    }
    GUI.color = Color.blue;
    float num7 = (float) Mathf.RoundToInt(num4 / 100000f);
    Rect position1 = new Rect(5f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num3, num4, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, (float) ((double) num7 * 1000.0 * 100.0)) - 10.0), 100f, 20f);
    num1 = num7 / 10f;
    string text5 = num1.ToString("0.0 MB");
    GUI.Label(position1, text5);
    float num8 = Mathf.Round(num3 / 100000f);
    Rect position2 = new Rect(5f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num3, num4, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, (float) ((double) num8 * 1000.0 * 100.0)) - 10.0), 100f, 20f);
    num1 = num8 / 10f;
    string text6 = num1.ToString("0.0 MB");
    GUI.Label(position2, text6);
    GUI.color = Color.green;
    float num9 = Mathf.Round(num6);
    GUI.Label(new Rect(55f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num5, num6, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, num9) - 10.0), 100f, 20f), num9.ToString("0 FPS"));
    float num10 = Mathf.Round(num5);
    GUI.Label(new Rect(55f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num5, num6, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, num10) - 10.0), 100f, 20f), num10.ToString("0 FPS"));
  }

  public struct GraphPoint
  {
    public float fps;
    public float memory;
    public bool collectEvent;
  }

  public struct PathTypeDebug(string name, Func<int> getSize, Func<int> getTotalCreated)
  {
    public string name = name;
    public Func<int> getSize = getSize;
    public Func<int> getTotalCreated = getTotalCreated;

    public void Print(StringBuilder text)
    {
      int num = this.getTotalCreated();
      if (num <= 0)
        return;
      text.Append("\n").Append(("  " + this.name).PadRight(25)).Append(this.getSize()).Append("/").Append(num);
    }
  }
}
