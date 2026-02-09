// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.AFPSCounter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using CodeStage.AdvancedFPSCounter.CountersData;
using CodeStage.AdvancedFPSCounter.Labels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
namespace CodeStage.AdvancedFPSCounter;

[DisallowMultipleComponent]
[AddComponentMenu("Code Stage/Advanced FPS Counter")]
public class AFPSCounter : MonoBehaviour
{
  public const string MENU_PATH = "Code Stage/Advanced FPS Counter";
  public const string COMPONENT_NAME = "Advanced FPS Counter";
  public const string LOG_PREFIX = "<b>[AFPSCounter]:</b> ";
  public const char NEW_LINE = '\n';
  public const char SPACE = ' ';
  public FPSCounterData fpsCounter = new FPSCounterData();
  public MemoryCounterData memoryCounter = new MemoryCounterData();
  public DeviceInfoCounterData deviceInfoCounter = new DeviceInfoCounterData();
  [Tooltip("Used to enable / disable plugin at runtime.\nSet to None to disable.")]
  public KeyCode hotKey = KeyCode.BackQuote;
  [Tooltip("Used to enable / disable plugin at runtime.\nMake two circle gestures with your finger \\ mouse to switch plugin on and off.")]
  public bool circleGesture;
  [Tooltip("Hot key modifier: any Control on Windows or any Command on Mac.")]
  public bool hotKeyCtrl;
  [Tooltip("Hot key modifier: any Shift.")]
  public bool hotKeyShift;
  [Tooltip("Hot key modifier: any Alt.")]
  public bool hotKeyAlt;
  [SerializeField]
  [Tooltip("Prevents current or other topmost Game Object from destroying on level (scene) load.\nApplied once, on Start phase.")]
  public bool keepAlive = true;
  public Canvas canvas;
  public CanvasScaler canvasScaler;
  public bool externalCanvas;
  public DrawableLabel[] labels;
  public int anchorsCount;
  public int cachedVSync = -1;
  public int cachedFrameRate = -1;
  public bool inited;
  public List<Vector2> gesturePoints = new List<Vector2>();
  public int gestureCount;
  [Tooltip("Disabled: removes labels and stops all internal processes except Hot Key listener.\n\nBackground: removes labels keeping counters alive; use for hidden performance monitoring.\n\nNormal: shows labels and runs all internal processes as usual.")]
  [SerializeField]
  public OperationMode operationMode = OperationMode.Normal;
  [Tooltip("Allows to see how your game performs on specified frame rate.\nDoes not guarantee selected frame rate. Set -1 to render as fast as possible in current conditions.\nIMPORTANT: this option disables VSync while enabled!")]
  [SerializeField]
  public bool forceFrameRate;
  [SerializeField]
  [Range(-1f, 200f)]
  public int forcedFrameRate = -1;
  [SerializeField]
  [Tooltip("Background for all texts. Cheapest effect. Overhead: 1 Draw Call.")]
  public bool background = true;
  [Tooltip("Color of the background.")]
  [SerializeField]
  public Color backgroundColor = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 155);
  [Tooltip("Padding of the background.")]
  [Range(0.0f, 30f)]
  [SerializeField]
  public int backgroundPadding = 5;
  [Tooltip("Shadow effect for all texts. This effect uses extra resources. Overhead: medium CPU and light GPU usage.")]
  [SerializeField]
  public bool shadow;
  [Tooltip("Color of the shadow effect.")]
  [SerializeField]
  public Color shadowColor = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 128 /*0x80*/);
  [SerializeField]
  [Tooltip("Distance of the shadow effect.")]
  public Vector2 shadowDistance = new Vector2(1f, -1f);
  [Tooltip("Outline effect for all texts. Resource-heaviest effect. Overhead: huge CPU and medium GPU usage. Not recommended for use unless really necessary.")]
  [SerializeField]
  public bool outline;
  [Tooltip("Color of the outline effect.")]
  [SerializeField]
  public Color outlineColor = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 128 /*0x80*/);
  [SerializeField]
  [Tooltip("Distance of the outline effect.")]
  public Vector2 outlineDistance = new Vector2(1f, -1f);
  [SerializeField]
  [Tooltip("Controls own canvas scaler scale mode. Chec to use ScaleWithScreenSize. Otherwise ConstantPixelSize will be used.")]
  public bool autoScale;
  [Range(0.0f, 30f)]
  [Tooltip("Controls global scale of all texts.")]
  [SerializeField]
  public float scaleFactor = 1f;
  [SerializeField]
  [Tooltip("Leave blank to use default font.")]
  public Font labelsFont;
  [Tooltip("Set to 0 to use font size specified in the font importer.")]
  [SerializeField]
  [Range(0.0f, 100f)]
  public int fontSize = 14;
  [Tooltip("Space between lines in labels.")]
  [Range(0.0f, 10f)]
  [SerializeField]
  public float lineSpacing = 1f;
  [Range(0.0f, 10f)]
  [SerializeField]
  [Tooltip("Lines count between different counters in a single label.")]
  public int countersSpacing;
  [SerializeField]
  [Tooltip("Pixel offset for anchored labels. Automatically applied to all labels.")]
  public Vector2 paddingOffset = new Vector2(5f, 5f);
  [Tooltip("Controls own canvas Pixel Perfect property.")]
  [SerializeField]
  public bool pixelPerfect = true;
  [SerializeField]
  [Tooltip("Sorting order to use for the canvas.\nSet higher value to get closer to the user.")]
  public int sortingOrder = 10000;
  [CompilerGenerated]
  public static AFPSCounter \u003CInstance\u003Ek__BackingField;

  public bool KeepAlive => this.keepAlive;

  public OperationMode OperationMode
  {
    get => this.operationMode;
    set
    {
      if (this.operationMode == value || !Application.isPlaying)
        return;
      this.operationMode = value;
      if (this.operationMode != OperationMode.Disabled)
      {
        if (this.operationMode == OperationMode.Background)
        {
          for (int index = 0; index < this.anchorsCount; ++index)
            this.labels[index].Clear();
        }
        this.OnEnable();
        this.fpsCounter.UpdateValue();
        this.memoryCounter.UpdateValue();
        this.deviceInfoCounter.UpdateValue();
        this.UpdateTexts();
      }
      else
        this.OnDisable();
    }
  }

  public bool ForceFrameRate
  {
    get => this.forceFrameRate;
    set
    {
      if (this.forceFrameRate == value || !Application.isPlaying)
        return;
      this.forceFrameRate = value;
      if (this.operationMode == OperationMode.Disabled)
        return;
      this.RefreshForcedFrameRate();
    }
  }

  public int ForcedFrameRate
  {
    get => this.forcedFrameRate;
    set
    {
      if (this.forcedFrameRate == value || !Application.isPlaying)
        return;
      this.forcedFrameRate = value;
      if (this.operationMode == OperationMode.Disabled)
        return;
      this.RefreshForcedFrameRate();
    }
  }

  public bool Background
  {
    get => this.background;
    set
    {
      if (this.background == value || !Application.isPlaying)
        return;
      this.background = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeBackground(this.background);
    }
  }

  public Color BackgroundColor
  {
    get => this.backgroundColor;
    set
    {
      if (this.backgroundColor == value || !Application.isPlaying)
        return;
      this.backgroundColor = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeBackgroundColor(this.backgroundColor);
    }
  }

  public int BackgroundPadding
  {
    get => this.backgroundPadding;
    set
    {
      if (this.backgroundPadding == value || !Application.isPlaying)
        return;
      this.backgroundPadding = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeBackgroundPadding(this.backgroundPadding);
    }
  }

  public bool Shadow
  {
    get => this.shadow;
    set
    {
      if (this.shadow == value || !Application.isPlaying)
        return;
      this.shadow = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeShadow(this.shadow);
    }
  }

  public Color ShadowColor
  {
    get => this.shadowColor;
    set
    {
      if (this.shadowColor == value || !Application.isPlaying)
        return;
      this.shadowColor = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeShadowColor(this.shadowColor);
    }
  }

  public Vector2 ShadowDistance
  {
    get => this.shadowDistance;
    set
    {
      if (this.shadowDistance == value || !Application.isPlaying)
        return;
      this.shadowDistance = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeShadowDistance(this.shadowDistance);
    }
  }

  public bool Outline
  {
    get => this.outline;
    set
    {
      if (this.outline == value || !Application.isPlaying)
        return;
      this.outline = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeOutline(this.outline);
    }
  }

  public Color OutlineColor
  {
    get => this.outlineColor;
    set
    {
      if (this.outlineColor == value || !Application.isPlaying)
        return;
      this.outlineColor = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeOutlineColor(this.outlineColor);
    }
  }

  public Vector2 OutlineDistance
  {
    get => this.outlineDistance;
    set
    {
      if (this.outlineDistance == value || !Application.isPlaying)
        return;
      this.outlineDistance = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeOutlineDistance(this.outlineDistance);
    }
  }

  public bool AutoScale
  {
    get => this.autoScale;
    set
    {
      if (this.autoScale == value || !Application.isPlaying)
        return;
      this.autoScale = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null || (UnityEngine.Object) this.canvasScaler == (UnityEngine.Object) null)
        return;
      this.canvasScaler.uiScaleMode = this.autoScale ? CanvasScaler.ScaleMode.ScaleWithScreenSize : CanvasScaler.ScaleMode.ConstantPixelSize;
    }
  }

  public float ScaleFactor
  {
    get => this.scaleFactor;
    set
    {
      if ((double) Math.Abs(this.scaleFactor - value) < 1.0 / 1000.0 || !Application.isPlaying)
        return;
      this.scaleFactor = value;
      if (this.operationMode == OperationMode.Disabled || (UnityEngine.Object) this.canvasScaler == (UnityEngine.Object) null || (UnityEngine.Object) this.canvasScaler == (UnityEngine.Object) null)
        return;
      this.canvasScaler.scaleFactor = this.scaleFactor;
    }
  }

  public Font LabelsFont
  {
    get => this.labelsFont;
    set
    {
      if ((UnityEngine.Object) this.labelsFont == (UnityEngine.Object) value || !Application.isPlaying)
        return;
      this.labelsFont = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeFont(this.labelsFont);
    }
  }

  public int FontSize
  {
    get => this.fontSize;
    set
    {
      if (this.fontSize == value || !Application.isPlaying)
        return;
      this.fontSize = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeFontSize(this.fontSize);
    }
  }

  public float LineSpacing
  {
    get => this.lineSpacing;
    set
    {
      if ((double) Math.Abs(this.lineSpacing - value) < 1.0 / 1000.0 || !Application.isPlaying)
        return;
      this.lineSpacing = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeLineSpacing(this.lineSpacing);
    }
  }

  public int CountersSpacing
  {
    get => this.countersSpacing;
    set
    {
      if (this.countersSpacing == value || !Application.isPlaying)
        return;
      this.countersSpacing = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      this.UpdateTexts();
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].dirty = true;
    }
  }

  public Vector2 PaddingOffset
  {
    get => this.paddingOffset;
    set
    {
      if (this.paddingOffset == value || !Application.isPlaying)
        return;
      this.paddingOffset = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].ChangeOffset(this.paddingOffset);
    }
  }

  public bool PixelPerfect
  {
    get => this.pixelPerfect;
    set
    {
      if (this.pixelPerfect == value || !Application.isPlaying)
        return;
      this.pixelPerfect = value;
      if (this.operationMode == OperationMode.Disabled || this.labels == null)
        return;
      this.canvas.pixelPerfect = this.pixelPerfect;
    }
  }

  public int SortingOrder
  {
    get => this.sortingOrder;
    set
    {
      if (this.sortingOrder == value || !Application.isPlaying)
        return;
      this.sortingOrder = value;
      if (this.operationMode == OperationMode.Disabled || (UnityEngine.Object) this.canvas == (UnityEngine.Object) null)
        return;
      this.canvas.sortingOrder = this.sortingOrder;
    }
  }

  public static AFPSCounter Instance
  {
    get => AFPSCounter.\u003CInstance\u003Ek__BackingField;
    set => AFPSCounter.\u003CInstance\u003Ek__BackingField = value;
  }

  public static AFPSCounter GetOrCreateInstance(bool keepAlive)
  {
    if ((UnityEngine.Object) AFPSCounter.Instance != (UnityEngine.Object) null)
      return AFPSCounter.Instance;
    AFPSCounter objectOfType = UnityEngine.Object.FindObjectOfType<AFPSCounter>();
    if ((UnityEngine.Object) objectOfType != (UnityEngine.Object) null)
      AFPSCounter.Instance = objectOfType;
    else
      AFPSCounter.CreateInScene(false).keepAlive = keepAlive;
    return AFPSCounter.Instance;
  }

  public static AFPSCounter AddToScene() => AFPSCounter.AddToScene(true);

  public static AFPSCounter AddToScene(bool keepAlive)
  {
    return AFPSCounter.GetOrCreateInstance(keepAlive);
  }

  public static void Dispose()
  {
    if (!((UnityEngine.Object) AFPSCounter.Instance != (UnityEngine.Object) null))
      return;
    AFPSCounter.Instance.DisposeInternal();
  }

  public static string Color32ToHex(Color32 color)
  {
    return color.r.ToString("x2") + color.g.ToString("x2") + color.b.ToString("x2") + color.a.ToString("x2");
  }

  public static AFPSCounter CreateInScene(bool lookForExistingContainer = true)
  {
    GameObject gameObject = lookForExistingContainer ? GameObject.Find("Advanced FPS Counter") : (GameObject) null;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      gameObject = new GameObject("Advanced FPS Counter")
      {
        layer = LayerMask.NameToLayer("UI")
      };
    return gameObject.AddComponent<AFPSCounter>();
  }

  public void Awake()
  {
    if ((UnityEngine.Object) AFPSCounter.Instance != (UnityEngine.Object) null && AFPSCounter.Instance.keepAlive)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
    else
    {
      AFPSCounter.Instance = this;
      this.fpsCounter.Init(this);
      this.memoryCounter.Init(this);
      this.deviceInfoCounter.Init(this);
      this.ConfigureCanvas();
      this.ConfigureLabels();
      this.inited = true;
    }
  }

  public void Start()
  {
    if (!this.keepAlive)
      return;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.transform.root.gameObject);
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnLevelWasLoadedNew);
  }

  public void Update()
  {
    if (!this.inited)
      return;
    this.ProcessHotKey();
    if (!this.circleGesture || !this.CircleGestureMade())
      return;
    this.SwitchCounter();
  }

  public void OnLevelWasLoadedNew(Scene scene, LoadSceneMode mode) => this.OnLevelLoadedCallback();

  public void OnLevelLoadedCallback()
  {
    if (!this.inited || !this.fpsCounter.Enabled)
      return;
    this.fpsCounter.OnLevelLoadedCallback();
  }

  public void OnEnable()
  {
    if (!this.inited || this.operationMode == OperationMode.Disabled)
      return;
    this.ActivateCounters();
    this.Invoke("RefreshForcedFrameRate", 0.5f);
  }

  public void OnDisable()
  {
    if (!this.inited)
      return;
    this.DeactivateCounters();
    if (this.IsInvoking("RefreshForcedFrameRate"))
      this.CancelInvoke("RefreshForcedFrameRate");
    this.RefreshForcedFrameRate(true);
    for (int index = 0; index < this.anchorsCount; ++index)
    {
      if (this.labels[index] != null)
        this.labels[index].Clear();
    }
  }

  public void OnDestroy()
  {
    if (this.inited)
    {
      this.fpsCounter.Dispose();
      this.memoryCounter.Dispose();
      this.deviceInfoCounter.Dispose();
      if (this.labels != null)
      {
        for (int index = 0; index < this.anchorsCount; ++index)
          this.labels[index].Dispose();
        Array.Clear((Array) this.labels, 0, this.anchorsCount);
        this.labels = (DrawableLabel[]) null;
      }
      this.inited = false;
    }
    if ((UnityEngine.Object) this.canvas != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.canvas.gameObject);
    if (this.transform.childCount <= 1)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if (!((UnityEngine.Object) AFPSCounter.Instance == (UnityEngine.Object) this))
      return;
    AFPSCounter.Instance = (AFPSCounter) null;
  }

  public void MakeDrawableLabelDirty(LabelAnchor anchor)
  {
    if (this.operationMode != OperationMode.Normal)
      return;
    this.labels[(int) anchor].dirty = true;
  }

  public void UpdateTexts()
  {
    if (this.operationMode != OperationMode.Normal)
      return;
    bool flag = false;
    if (this.fpsCounter.Enabled)
    {
      DrawableLabel label = this.labels[(int) this.fpsCounter.Anchor];
      if (label.newText.Length > 0)
        label.newText.Append(new string('\n', this.countersSpacing + 1));
      label.newText.Append((object) this.fpsCounter.text);
      label.dirty |= this.fpsCounter.dirty;
      this.fpsCounter.dirty = false;
      flag = true;
    }
    if (this.memoryCounter.Enabled)
    {
      DrawableLabel label = this.labels[(int) this.memoryCounter.Anchor];
      if (label.newText.Length > 0)
        label.newText.Append(new string('\n', this.countersSpacing + 1));
      label.newText.Append((object) this.memoryCounter.text);
      label.dirty |= this.memoryCounter.dirty;
      this.memoryCounter.dirty = false;
      flag = true;
    }
    if (this.deviceInfoCounter.Enabled)
    {
      DrawableLabel label = this.labels[(int) this.deviceInfoCounter.Anchor];
      if (label.newText.Length > 0)
        label.newText.Append(new string('\n', this.countersSpacing + 1));
      label.newText.Append((object) this.deviceInfoCounter.text);
      label.dirty |= this.deviceInfoCounter.dirty;
      this.deviceInfoCounter.dirty = false;
      flag = true;
    }
    if (flag)
    {
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].CheckAndUpdate();
    }
    else
    {
      for (int index = 0; index < this.anchorsCount; ++index)
        this.labels[index].Clear();
    }
  }

  public void ConfigureCanvas()
  {
    if ((UnityEngine.Object) this.GetComponentInParent<Canvas>() != (UnityEngine.Object) null)
    {
      this.externalCanvas = true;
      RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
      if ((UnityEngine.Object) rectTransform == (UnityEngine.Object) null)
        rectTransform = this.gameObject.AddComponent<RectTransform>();
      UIUtils.ResetRectTransform(rectTransform);
    }
    GameObject gameObject = new GameObject("CountersCanvas", new System.Type[1]
    {
      typeof (Canvas)
    });
    gameObject.tag = this.gameObject.tag;
    gameObject.layer = this.gameObject.layer;
    gameObject.transform.SetParent(this.transform, false);
    this.canvas = gameObject.GetComponent<Canvas>();
    UIUtils.ResetRectTransform(gameObject.GetComponent<RectTransform>());
    if (this.externalCanvas)
      return;
    this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    this.canvas.pixelPerfect = this.pixelPerfect;
    this.canvas.sortingOrder = this.sortingOrder;
    this.canvasScaler = gameObject.AddComponent<CanvasScaler>();
    if (this.autoScale)
      this.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    else
      this.canvasScaler.scaleFactor = this.scaleFactor;
  }

  public void ConfigureLabels()
  {
    this.anchorsCount = Enum.GetNames(typeof (LabelAnchor)).Length;
    this.labels = new DrawableLabel[this.anchorsCount];
    for (int anchor = 0; anchor < this.anchorsCount; ++anchor)
      this.labels[anchor] = new DrawableLabel(this.canvas.gameObject, (LabelAnchor) anchor, new LabelEffect(this.background, this.backgroundColor, this.backgroundPadding), new LabelEffect(this.shadow, this.shadowColor, this.shadowDistance), new LabelEffect(this.outline, this.outlineColor, this.outlineDistance), this.labelsFont, this.fontSize, this.lineSpacing, this.paddingOffset);
  }

  public void DisposeInternal()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this);
    if (!((UnityEngine.Object) AFPSCounter.Instance == (UnityEngine.Object) this))
      return;
    AFPSCounter.Instance = (AFPSCounter) null;
  }

  public void ProcessHotKey()
  {
    if (this.hotKey == KeyCode.None || !Input.GetKeyDown(this.hotKey) || !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift))
      return;
    bool flag = true;
    if (this.hotKeyCtrl)
      flag = ((flag ? 1 : 0) & (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) ? 1 : (Input.GetKey(KeyCode.RightCommand) ? 1 : 0))) != 0;
    if (this.hotKeyAlt)
      flag = ((flag ? 1 : 0) & (Input.GetKey(KeyCode.LeftAlt) ? 1 : (Input.GetKey(KeyCode.RightAlt) ? 1 : 0))) != 0;
    if (this.hotKeyShift)
      flag = ((flag ? 1 : 0) & (Input.GetKey(KeyCode.LeftShift) ? 1 : (Input.GetKey(KeyCode.RightShift) ? 1 : 0))) != 0;
    if (!flag)
      return;
    this.SwitchCounter();
  }

  public bool CircleGestureMade()
  {
    bool flag = false;
    int num1 = this.gesturePoints.Count;
    if (Input.GetMouseButton(0))
    {
      Vector2 mousePosition = (Vector2) Input.mousePosition;
      if (num1 == 0 || (double) (mousePosition - this.gesturePoints[num1 - 1]).magnitude > 10.0)
      {
        this.gesturePoints.Add(mousePosition);
        ++num1;
      }
    }
    else if (Input.GetMouseButtonUp(0))
    {
      num1 = 0;
      this.gestureCount = 0;
      this.gesturePoints.Clear();
    }
    if (num1 < 10)
      return flag;
    float num2 = 0.0f;
    Vector2 zero = Vector2.zero;
    Vector2 rhs = Vector2.zero;
    for (int index = 0; index < num1 - 2; ++index)
    {
      Vector2 lhs = this.gesturePoints[index + 1] - this.gesturePoints[index];
      zero += lhs;
      float magnitude = lhs.magnitude;
      num2 += magnitude;
      if ((double) Vector2.Dot(lhs, rhs) < 0.0)
      {
        this.gesturePoints.Clear();
        this.gestureCount = 0;
        return flag;
      }
      rhs = lhs;
    }
    int num3 = (Screen.width + Screen.height) / 4;
    if ((double) num2 > (double) num3 && (double) zero.magnitude < (double) num3 / 2.0)
    {
      this.gesturePoints.Clear();
      ++this.gestureCount;
      if (this.gestureCount >= 2)
      {
        this.gestureCount = 0;
        flag = true;
      }
    }
    return flag;
  }

  public void SwitchCounter()
  {
    if (this.operationMode == OperationMode.Disabled)
    {
      this.OperationMode = OperationMode.Normal;
    }
    else
    {
      if (this.operationMode != OperationMode.Normal)
        return;
      this.OperationMode = OperationMode.Disabled;
    }
  }

  public void ActivateCounters()
  {
    this.fpsCounter.Activate();
    this.memoryCounter.Activate();
    this.deviceInfoCounter.Activate();
    if (!this.fpsCounter.Enabled && !this.memoryCounter.Enabled && !this.deviceInfoCounter.Enabled)
      return;
    this.UpdateTexts();
  }

  public void DeactivateCounters()
  {
    if ((UnityEngine.Object) AFPSCounter.Instance == (UnityEngine.Object) null)
      return;
    this.fpsCounter.Deactivate();
    this.memoryCounter.Deactivate();
    this.deviceInfoCounter.Deactivate();
  }

  public void RefreshForcedFrameRate() => this.RefreshForcedFrameRate(false);

  public void RefreshForcedFrameRate(bool disabling)
  {
    if (this.forceFrameRate && !disabling)
    {
      if (this.cachedVSync == -1)
      {
        this.cachedVSync = QualitySettings.vSyncCount;
        this.cachedFrameRate = Application.targetFrameRate;
        QualitySettings.vSyncCount = 0;
      }
      Application.targetFrameRate = this.forcedFrameRate;
    }
    else
    {
      if (this.cachedVSync == -1)
        return;
      QualitySettings.vSyncCount = this.cachedVSync;
      Application.targetFrameRate = this.cachedFrameRate;
      this.cachedVSync = -1;
    }
  }
}
