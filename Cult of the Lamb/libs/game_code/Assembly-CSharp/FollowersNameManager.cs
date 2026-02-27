// Decompiled with JetBrains decompiler
// Type: FollowersNameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class FollowersNameManager : MonoBehaviour
{
  public static FollowersNameManager Instance;
  public List<FollowersNameManager.Nameplate> nameplates = new List<FollowersNameManager.Nameplate>();
  public int textWidth = 192 /*0xC0*/;
  public int textHeight = 16 /*0x10*/;
  public int padding = 8;
  public int columns = 10;
  public int maxAtlasWidth = 1024 /*0x0400*/;
  public int maxAtlasHeight = 1024 /*0x0400*/;
  public RenderTexture renderRT;
  public Texture2D atlasTexture;
  public Texture2D tmpReadBuffer;
  public int atlasWidth;
  public int atlasHeight;
  public TextMeshPro renderTMP;
  public TMP_SubMesh renderTMPSubMesh;
  public GameObject tmpContainer;
  public Camera renderCam;
  public bool isInitialized;
  public bool isGenerating;
  public bool isDirty;
  public static Rect cachedReadRect = new Rect(0.0f, 0.0f, 192f, 16f);

  public int GetNewIndex => this.nameplates.Count;

  public void AddNameplate(FollowersNameManager.Nameplate nameplate)
  {
    this.nameplates.Add(nameplate);
    this.isDirty = true;
  }

  public void RemoveNameplate(int index)
  {
    if (!this.isInitialized || index < 0 || index >= this.nameplates.Count)
      return;
    this.nameplates[index] = (FollowersNameManager.Nameplate) null;
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void CreateInstance()
  {
    GameObject target = new GameObject("TextToAtlasManager");
    target.AddComponent<FollowersNameManager>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
  }

  public void Awake() => FollowersNameManager.Instance = this;

  public void Initialize()
  {
    this.InitRenderer();
    this.GenerateAtlas();
  }

  public void LateUpdate()
  {
    if (SettingsManager.Settings != null && !SettingsManager.Settings.Game.ShowFollowerNames)
      return;
    this.isGenerating = false;
    if (!this.isDirty)
      return;
    if (this.isInitialized)
      this.GenerateAtlas();
    else
      this.Initialize();
    this.isDirty = false;
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.tmpContainer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.tmpContainer);
    if ((UnityEngine.Object) FollowersNameManager.Instance != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) FollowersNameManager.Instance.gameObject);
    FollowersNameManager.Instance = (FollowersNameManager) null;
  }

  public void InitRenderer()
  {
    if ((UnityEngine.Object) this.renderRT == (UnityEngine.Object) null)
      this.renderRT = new RenderTexture(this.textWidth, this.textHeight, 0);
    if ((UnityEngine.Object) this.tmpReadBuffer == (UnityEngine.Object) null)
      this.tmpReadBuffer = new Texture2D(this.textWidth, this.textHeight, TextureFormat.ARGB32, false);
    this.isInitialized = true;
    if (!((UnityEngine.Object) this.renderCam == (UnityEngine.Object) null))
      return;
    this.CreateCamera();
  }

  public void CreateCamera()
  {
    GameObject gameObject = new GameObject("TMP_Render_Camera");
    gameObject.hideFlags = HideFlags.HideAndDontSave;
    this.renderCam = gameObject.AddComponent<Camera>();
    this.renderCam.orthographic = true;
    this.renderCam.orthographicSize = 0.1f;
    this.renderCam.clearFlags = CameraClearFlags.Color;
    this.renderCam.backgroundColor = Color.clear;
    this.renderCam.cullingMask = LayerMask.GetMask("TMPUI");
    this.renderCam.targetTexture = this.renderRT;
    this.renderCam.transform.position = new Vector3(0.0f, 0.0f, -10f);
    this.renderCam.enabled = false;
  }

  public void CreateTMPComponent()
  {
    this.tmpContainer = new GameObject("TMP_Renderer");
    this.renderTMP = this.tmpContainer.AddComponent<TextMeshPro>();
    this.renderTMP.gameObject.layer = LayerMask.NameToLayer("TMPUI");
    this.renderTMP.rectTransform.sizeDelta = new Vector2((float) this.textWidth, (float) this.textHeight);
    this.renderTMP.transform.position = Vector3.zero;
    this.renderTMPSubMesh = this.renderTMP.GetComponentInChildren<TMP_SubMesh>();
  }

  public void Regenerate()
  {
    this.GenerateAtlas();
    this.isGenerating = true;
  }

  public void GenerateAtlas()
  {
    if (this.isGenerating || !this.isInitialized || this.nameplates.Count == 0)
      return;
    if ((UnityEngine.Object) this.tmpContainer == (UnityEngine.Object) null)
      this.CreateTMPComponent();
    int count = this.nameplates.Count;
    int num1 = Mathf.CeilToInt((float) count / (float) this.columns);
    int num2 = this.columns * (this.textWidth + this.padding);
    int num3 = this.textHeight + this.padding;
    int num4 = num1 * num3;
    this.atlasWidth = this.maxAtlasWidth;
    this.atlasHeight = this.maxAtlasHeight;
    if ((UnityEngine.Object) this.atlasTexture == (UnityEngine.Object) null | (num2 > this.atlasWidth || num4 > this.atlasHeight))
    {
      if (num2 > this.maxAtlasWidth || num4 > this.maxAtlasHeight)
      {
        Debug.LogError((object) "Required atlas size exceeds supported max (4096x4096). Consider reducing text size or using multiple atlases.");
        return;
      }
      this.atlasWidth = Mathf.Clamp(num2, this.maxAtlasWidth, 4096 /*0x1000*/);
      this.atlasHeight = Mathf.Clamp(num4, this.maxAtlasHeight, 4096 /*0x1000*/);
      if ((UnityEngine.Object) this.atlasTexture != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.atlasTexture);
      this.atlasTexture = new Texture2D(this.atlasWidth, this.atlasHeight, TextureFormat.DXT5Crunched, false);
      this.atlasTexture.name = "FollowerNamesAtlas_Texture";
    }
    this.ClearAtlas();
    for (int index = 0; index < count; ++index)
      this.UpdateNameplate(index);
    this.atlasTexture.Apply(false, false);
    this.StartCoroutine(this.RemoveCont());
  }

  public IEnumerator RemoveCont()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) this.tmpContainer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.tmpContainer);
  }

  public void Unload()
  {
    if ((UnityEngine.Object) this.atlasTexture != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.atlasTexture);
    if ((UnityEngine.Object) this.tmpContainer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.tmpContainer);
    if ((UnityEngine.Object) this.renderRT != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.renderRT);
    if ((UnityEngine.Object) this.renderCam != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.renderCam);
    this.nameplates.Clear();
    this.isInitialized = false;
  }

  public void ClearAtlas()
  {
    Color32[] colors = new Color32[this.atlasWidth * this.atlasHeight];
    for (int index = 0; index < colors.Length; ++index)
      colors[index] = new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 0);
    this.atlasTexture.SetPixels32(colors);
  }

  public void UpdateNameplate(int index)
  {
    if (!this.isInitialized || index < 0 || index >= this.nameplates.Count)
      return;
    FollowersNameManager.Nameplate nameplate = this.nameplates[index];
    if (nameplate == null)
      return;
    TMPHelperUtilities.CopyTMPToTMP(nameplate.TMPTextSource, this.renderTMP);
    this.renderTMP.ForceMeshUpdate(false, false);
    this.renderCam.Render();
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = this.renderRT;
    this.tmpReadBuffer.ReadPixels(FollowersNameManager.cachedReadRect, 0, 0);
    this.tmpReadBuffer.Apply();
    RenderTexture.active = active;
    int num1 = index % this.columns;
    int num2 = index / this.columns;
    int num3 = this.textWidth + this.padding;
    int x = num1 * num3;
    int y = this.atlasHeight - (num2 + 1) * (this.textHeight + this.padding);
    this.atlasTexture.SetPixels(x, y, this.textWidth, this.textHeight, this.tmpReadBuffer.GetPixels());
    Sprite sprite = Sprite.Create(this.atlasTexture, new Rect((float) x, (float) y, (float) this.textWidth, (float) this.textHeight), new Vector2(0.5f, 0.5f), 100f);
    nameplate.SpriteRendererSource.sprite = sprite;
    nameplate.TMPTextSource.gameObject.SetActive(false);
  }

  [Serializable]
  public class Nameplate
  {
    public TMP_Text TMPTextSource;
    public SpriteRenderer SpriteRendererSource;
  }
}
