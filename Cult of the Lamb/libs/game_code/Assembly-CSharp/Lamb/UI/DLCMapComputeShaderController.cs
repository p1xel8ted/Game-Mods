// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DLCMapComputeShaderController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class DLCMapComputeShaderController : MonoBehaviour
{
  [Header("UI Stuff")]
  [SerializeField]
  public ComputeShader _computeShaderTest;
  [SerializeField]
  public RectTransform _rootViewport;
  [SerializeField]
  public RawImage _rawImage;
  [SerializeField]
  public UpgradeTreeConfiguration _upgradeTreeConfiguration;
  [SerializeField]
  public List<TierLockIcon> _tierLocks;
  [SerializeField]
  public UIDLCMapSideTransitioner _nodeContainer;
  [SerializeField]
  public bool _playerUpgrades;
  [Header("Effects Properties")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _radius = 100f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _smoothStepMin;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _smoothStepMax = 1f;
  [SerializeField]
  [Range(0.0f, 5f)]
  public float _multiply = 1f;
  [SerializeField]
  [Range(800f, 2500f)]
  public float _height = 1000f;
  [SerializeField]
  [Range(0.1f, 1000f)]
  public float _verticalFalloff = 100f;
  [SerializeField]
  [Range(0.1f, 1000f)]
  public float _tierFalloff = 20f;
  [SerializeField]
  public float _emptyTierPosition = 200f;
  public const int kResolutionScale = 2;
  public Vector2Int _resolution;
  public DLCMapNodeData[] _data;
  public ComputeBuffer _nodeBuffer;
  public RenderTexture _renderTexture;
  public Material _effectsMaterial;
  public int _offsetProperty;
  public int _horizontalFixProperty;
  public int _verticalFixProperty;
  public Vector2 _offset;
  public UpgradeTreeNode.TreeTier _highestTier;
  public float _tierPosition;
  public Dictionary<UpgradeTreeNode.TreeTier, float> _tierPositions = new Dictionary<UpgradeTreeNode.TreeTier, float>();
  public List<DungeonWorldMapIcon> _treeNodes = new List<DungeonWorldMapIcon>();
  public int _updateKernel;

  public float _targetTierPosition
  {
    get
    {
      if (this._tierPositions.Count == 0)
        return 0.0f;
      UpgradeTreeNode.TreeTier key = this._playerUpgrades ? DataManager.Instance.CurrentPlayerUpgradeTreeTier : DataManager.Instance.CurrentUpgradeTreeTier;
      float num1;
      if (key < this._highestTier)
      {
        float tierPosition1 = this._tierPositions[key];
        UpgradeTreeNode.TreeTier treeTier = key + 1;
        float tierPosition2 = this._tierPositions[treeTier];
        num1 = Mathf.Lerp(tierPosition1, tierPosition2, this._upgradeTreeConfiguration.NormalizedProgressToNextTier(treeTier));
      }
      else
      {
        int num2 = 5;
        float t = 1f - Mathf.Clamp((float) (this._upgradeTreeConfiguration.NumRequiredNodesForTier(this._highestTier) + num2 - this._upgradeTreeConfiguration.NumUnlockedUpgrades()) / (float) (this._upgradeTreeConfiguration.GetConfigForTier(this._highestTier).NumRequiredToUnlock + num2), 0.0f, 1f);
        num1 = this._tierLocks.Count <= 0 ? t : Mathf.Lerp(this._tierPositions[this._highestTier], this._height, t);
      }
      return num1 - this._tierFalloff * 0.5f;
    }
  }

  public IReadOnlyList<DungeonWorldMapIcon> TreeNodes
  {
    get => (IReadOnlyList<DungeonWorldMapIcon>) this._treeNodes;
  }

  public UpgradeTreeConfiguration UpgradeTreeConfiguration => this._upgradeTreeConfiguration;

  public List<TierLockIcon> TierLockIcons => this._tierLocks;

  public void OnEnable()
  {
  }

  public void InitialiseBuffers()
  {
    this._data = new DLCMapNodeData[this._treeNodes.Count];
    this._nodeBuffer = new ComputeBuffer(this._data.Length, this._data.Length * DLCMapNodeData.Size());
  }

  public void OnDisable()
  {
    if (this._nodeBuffer == null)
      return;
    this._nodeBuffer.Release();
    this._nodeBuffer = (ComputeBuffer) null;
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this._resolution = new Vector2Int(Screen.width / 2, Screen.height / 2);
    if ((UnityEngine.Object) this._renderTexture == (UnityEngine.Object) null)
    {
      this._renderTexture = new RenderTexture(this._resolution.x, this._resolution.y, 24);
      this._renderTexture.enableRandomWrite = true;
      this._renderTexture.filterMode = FilterMode.Bilinear;
      this._renderTexture.Create();
      this._rawImage.texture = (Texture) this._renderTexture;
      this._updateKernel = this._computeShaderTest.FindKernel("Update");
      Shader.SetGlobalTexture("_MountainMask", (Texture) this._renderTexture);
    }
    if ((UnityEngine.Object) this._effectsMaterial == (UnityEngine.Object) null)
    {
      this._effectsMaterial = new Material(this._rawImage.material);
      this._offsetProperty = Shader.PropertyToID("_ScrollOffset");
      this._horizontalFixProperty = Shader.PropertyToID("_HorizontalFix");
      this._verticalFixProperty = Shader.PropertyToID("_VerticalFix");
      if (this._resolution.x > this._resolution.y)
      {
        this._effectsMaterial.SetFloat(this._horizontalFixProperty, (float) this._resolution.x / (float) this._resolution.y);
        this._effectsMaterial.SetFloat(this._verticalFixProperty, 1f);
      }
      else
      {
        this._effectsMaterial.SetFloat(this._horizontalFixProperty, 1f);
        this._effectsMaterial.SetFloat(this._verticalFixProperty, (float) this._resolution.y / (float) this._resolution.x);
      }
      this._rawImage.material = this._effectsMaterial;
    }
    this._highestTier = this._upgradeTreeConfiguration.HighestTier();
    this._tierPositions.Add(UpgradeTreeNode.TreeTier.Tier1, -this._height);
    foreach (TierLockIcon tierLock in this._tierLocks)
      this._tierPositions.Add(tierLock.Tier, tierLock.RectTransform.anchoredPosition.y);
    this._tierPosition = this._targetTierPosition;
  }

  public void Update()
  {
    if ((UnityEngine.Object) this._renderTexture == (UnityEngine.Object) null)
      return;
    float num = (float) (2.0 / ((UnityEngine.Object) this._rootViewport != (UnityEngine.Object) null ? (double) this._rootViewport.localScale.x : 1.0));
    for (int index = 0; index < this._treeNodes.Count; ++index)
    {
      this._data[index].Position = this._treeNodes[index].RectTransform.position;
      this._data[index].Position /= 2f;
      this._data[index].RadiusScale = this._treeNodes[index].GoopRadiusScale;
      this._data[index].Influence = 1f;
      if (this._treeNodes[index].Button.Interactable)
        this._data[index].Influence = 0.0f;
    }
    if (this._nodeBuffer != null)
    {
      this._nodeBuffer.SetData((Array) this._data);
      this._computeShaderTest.SetBuffer(0, "TestBuffer", this._nodeBuffer);
      this._computeShaderTest.SetInt("BufferSize", this._nodeBuffer.count);
      this._computeShaderTest.SetTexture(this._updateKernel, "Result", (Texture) this._renderTexture);
      this._computeShaderTest.SetFloat("SmoothStepMin", this._smoothStepMin);
      this._computeShaderTest.SetFloat("SmoothStepMax", this._smoothStepMax);
      this._computeShaderTest.SetFloat("Time", Time.deltaTime);
      this._computeShaderTest.SetFloat("Radius", this._radius / num);
      this._computeShaderTest.SetFloat("Multiply", this._multiply);
      this._computeShaderTest.SetVector("MousePosition", (Vector4) (Input.mousePosition / num));
    }
    if (this._tierLocks.Count > 0)
    {
      this._tierPosition -= (this._tierPosition - this._targetTierPosition) * Time.unscaledDeltaTime;
      this._computeShaderTest.SetFloat("TierPosition", this._rootViewport.TransformPoint((Vector3) new Vector2(0.0f, this._tierPosition)).y / 2f);
      this._computeShaderTest.SetFloat("TierFalloff", this._tierFalloff / num);
    }
    else
    {
      this._computeShaderTest.SetFloat("TierPosition", this._emptyTierPosition);
      this._computeShaderTest.SetFloat("TierFalloff", this._tierFalloff / num);
    }
    float y1 = this._rootViewport.TransformPoint((Vector3) new Vector2(0.0f, this._rootViewport.rect.yMax - this._height)).y;
    float y2 = this._rootViewport.TransformPoint((Vector3) new Vector2(0.0f, this._rootViewport.rect.yMax)).y;
    this._computeShaderTest.SetFloat("VerticalExtentMin", y1 / 2f);
    this._computeShaderTest.SetFloat("VerticalExtentMax", y2 / 2f);
    this._computeShaderTest.SetFloat("VerticalFalloff", this._verticalFalloff / num);
    this._computeShaderTest.Dispatch(this._updateKernel, this._renderTexture.width, this._renderTexture.height, 1);
    this._offset = (Vector2) this._rootViewport.position;
    this._effectsMaterial.SetVector(this._offsetProperty, (Vector4) -this._offset);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this._effectsMaterial != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this._effectsMaterial);
    if (!((UnityEngine.Object) this._renderTexture != (UnityEngine.Object) null))
      return;
    this._renderTexture.Release();
    UnityEngine.Object.Destroy((UnityEngine.Object) this._renderTexture);
    this._renderTexture = (RenderTexture) null;
  }

  public void GatherNodes()
  {
    this._treeNodes.Clear();
    IEnumerator enumerator1 = (IEnumerator) this._nodeContainer.InsideContainer.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
      {
        Transform current = (Transform) enumerator1.Current;
        DungeonWorldMapIcon component;
        if (!((UnityEngine.Object) current.gameObject == (UnityEngine.Object) null) && current.gameObject.TryGetComponent<DungeonWorldMapIcon>(out component))
          this._treeNodes.Add(component);
      }
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this._nodeContainer.OutsideContainer.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
      {
        Transform current = (Transform) enumerator2.Current;
        DungeonWorldMapIcon component;
        if (!((UnityEngine.Object) current.gameObject == (UnityEngine.Object) null) && current.gameObject.TryGetComponent<DungeonWorldMapIcon>(out component))
          this._treeNodes.Add(component);
      }
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    this.InitialiseBuffers();
  }
}
