// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeComputeShaderController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeTreeComputeShaderController : MonoBehaviour
{
  [Header("UI Stuff")]
  [SerializeField]
  private ComputeShader _computeShaderTest;
  [SerializeField]
  private RectTransform _rootViewport;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private RawImage _rawImage;
  [SerializeField]
  private List<UpgradeTreeNode> _treeNodes;
  [SerializeField]
  private UpgradeTreeConfiguration _upgradeTreeConfiguration;
  [SerializeField]
  private List<TierLockIcon> _tierLocks;
  [SerializeField]
  private bool _playerUpgrades;
  [Header("Effects Properties")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  private float _radius = 100f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float _smoothStepMin;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float _smoothStepMax = 1f;
  [SerializeField]
  [Range(0.0f, 5f)]
  private float _multiply = 1f;
  [SerializeField]
  [Range(800f, 2500f)]
  private float _height = 1000f;
  [SerializeField]
  [Range(0.1f, 1000f)]
  private float _verticalFalloff = 100f;
  [SerializeField]
  [Range(0.1f, 1000f)]
  private float _tierFalloff = 20f;
  private const int kResolutionScale = 2;
  private Vector2Int _resolution;
  private NodeData[] _data;
  private ComputeBuffer _nodeBuffer;
  private RenderTexture _renderTexture;
  private Material _effectsMaterial;
  private int _offsetProperty;
  private int _horizontalFixProperty;
  private int _verticalFixProperty;
  private Vector2 _offset;
  private UpgradeTreeNode.TreeTier _highestTier;
  private float _tierPosition;
  private Dictionary<UpgradeTreeNode.TreeTier, float> _tierPositions = new Dictionary<UpgradeTreeNode.TreeTier, float>();
  private int _updateKernel;

  private float _targetTierPosition
  {
    get
    {
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
        num1 = Mathf.Lerp(this._tierPositions[this._highestTier], this._height, 1f - Mathf.Clamp((float) (this._upgradeTreeConfiguration.NumRequiredNodesForTier(this._highestTier) + num2 - this._upgradeTreeConfiguration.NumUnlockedUpgrades()) / (float) (this._upgradeTreeConfiguration.GetConfigForTier(this._highestTier).NumRequiredToUnlock + num2), 0.0f, 1f));
      }
      return num1 - this._tierFalloff * 0.5f;
    }
  }

  public List<UpgradeTreeNode> TreeNodes => this._treeNodes;

  public UpgradeTreeConfiguration UpgradeTreeConfiguration => this._upgradeTreeConfiguration;

  public List<TierLockIcon> TierLockIcons => this._tierLocks;

  private void OnEnable()
  {
    this._data = new NodeData[this._treeNodes.Count];
    this._nodeBuffer = new ComputeBuffer(this._data.Length, this._data.Length * NodeData.Size());
  }

  private void OnDisable()
  {
    if (this._nodeBuffer == null)
      return;
    this._nodeBuffer.Release();
    this._nodeBuffer = (ComputeBuffer) null;
  }

  private IEnumerator Start()
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

  private void Update()
  {
    if ((UnityEngine.Object) this._renderTexture == (UnityEngine.Object) null)
      return;
    float num1 = (float) (2.0 / ((UnityEngine.Object) this._rootViewport != (UnityEngine.Object) null ? (double) this._rootViewport.localScale.x : 1.0));
    for (int index = 0; index < this._treeNodes.Count; ++index)
    {
      this._data[index].Position = this._treeNodes[index].RectTransform.position;
      this._data[index].Position /= 2f;
      this._data[index].Influence = this._treeNodes[index].UnlockedWeight;
    }
    this._nodeBuffer.SetData((Array) this._data);
    this._computeShaderTest.SetBuffer(0, "TestBuffer", this._nodeBuffer);
    this._computeShaderTest.SetInt("BufferSize", this._nodeBuffer.count);
    this._computeShaderTest.SetTexture(this._updateKernel, "Result", (Texture) this._renderTexture);
    this._computeShaderTest.SetFloat("SmoothStepMin", this._smoothStepMin);
    this._computeShaderTest.SetFloat("SmoothStepMax", this._smoothStepMax);
    this._computeShaderTest.SetFloat("Time", Time.deltaTime);
    this._computeShaderTest.SetFloat("Radius", this._radius / num1);
    this._computeShaderTest.SetFloat("Multiply", this._multiply);
    this._computeShaderTest.SetVector("MousePosition", (Vector4) (Input.mousePosition / num1));
    this._tierPosition -= (this._tierPosition - this._targetTierPosition) * Time.unscaledDeltaTime;
    this._computeShaderTest.SetFloat("TierPosition", this._scrollRect.content.TransformPoint((Vector3) new Vector2(0.0f, this._tierPosition)).y / 2f);
    this._computeShaderTest.SetFloat("TierFalloff", this._tierFalloff / num1);
    float y1 = this._scrollRect.content.TransformPoint((Vector3) new Vector2(0.0f, -this._height)).y;
    float y2 = this._scrollRect.content.TransformPoint((Vector3) new Vector2(0.0f, this._height)).y;
    this._computeShaderTest.SetFloat("VerticalExtentMin", y1 / 2f);
    this._computeShaderTest.SetFloat("VerticalExtentMax", y2 / 2f);
    this._computeShaderTest.SetFloat("VerticalFalloff", this._verticalFalloff / num1);
    this._computeShaderTest.Dispatch(this._updateKernel, this._renderTexture.width, this._renderTexture.height, 1);
    this._offset = (Vector2) this._scrollRect.content.position;
    ref float local1 = ref this._offset.x;
    double num2 = (double) local1;
    Rect rect = this._scrollRect.viewport.rect;
    double width = (double) rect.width;
    local1 = (float) (num2 / width);
    ref float local2 = ref this._offset.y;
    double num3 = (double) local2;
    rect = this._scrollRect.viewport.rect;
    double height = (double) rect.height;
    local2 = (float) (num3 / height);
    this._effectsMaterial.SetVector(this._offsetProperty, (Vector4) -this._offset);
  }

  private void OnDestroy()
  {
    if ((UnityEngine.Object) this._effectsMaterial != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this._effectsMaterial);
    if (!((UnityEngine.Object) this._renderTexture != (UnityEngine.Object) null))
      return;
    this._renderTexture.Release();
    UnityEngine.Object.Destroy((UnityEngine.Object) this._renderTexture);
    this._renderTexture = (RenderTexture) null;
  }
}
