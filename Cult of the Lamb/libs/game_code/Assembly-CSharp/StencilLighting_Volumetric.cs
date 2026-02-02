// Decompiled with JetBrains decompiler
// Type: StencilLighting_Volumetric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
[ExecuteInEditMode]
public class StencilLighting_Volumetric : BaseMonoBehaviour, IStencilLighting
{
  public bool useAdditiveColor;
  [SerializeField]
  [ColorUsage(false, true)]
  public Color AdditiveColor = Color.grey;
  [Range(0.0f, 1f)]
  public float AdditiveScale = 1f;
  public bool hideChild = true;
  public static int overrideColorID = Shader.PropertyToID("_Color");
  public Transform m_targetObj;
  public Renderer m_renderer;
  public MeshRenderer m_DecalMeshRenderer;
  public MeshFilter m_meshFilter;
  public MaterialPropertyBlock m_propBlock;
  public bool isVisible;
  public float visibilityTimer;
  public int materialID = -1;
  public Material sharedMaterial;

  public MaterialPropertyBlock PropBlock
  {
    get
    {
      if (this.m_propBlock == null)
        this.m_propBlock = new MaterialPropertyBlock();
      return this.m_propBlock;
    }
  }

  public void UpdateLight() => this.Init();

  public void Init()
  {
    MeshRenderer[] componentsInChildren = this.gameObject.transform.GetComponentsInChildren<MeshRenderer>(true);
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      if ((Object) componentsInChildren[index].transform != (Object) this.gameObject.transform)
      {
        componentsInChildren[index].transform.gameObject.SetActive(this.useAdditiveColor);
        if (this.useAdditiveColor)
        {
          componentsInChildren[index].transform.localScale = new Vector3(this.AdditiveScale, this.AdditiveScale, this.AdditiveScale);
          this.ApplyProperties((Renderer) componentsInChildren[index]);
          break;
        }
        break;
      }
      this.m_DecalMeshRenderer = componentsInChildren[index];
    }
    this.m_renderer = (Renderer) this.m_DecalMeshRenderer;
    this.m_targetObj = this.m_renderer.transform;
    this.m_meshFilter = this.m_targetObj.GetComponent<MeshFilter>();
    this.NegativeScaleFix();
  }

  public void NegativeScaleFix()
  {
    Vector3 vector3 = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    if ((double) this.transform.lossyScale.x < 0.0)
      vector3.x *= -1f;
    if ((double) this.transform.lossyScale.y < 0.0)
      vector3.y *= -1f;
    if ((double) this.transform.lossyScale.z < 0.0)
      vector3.z *= -1f;
    this.transform.localScale = vector3;
  }

  public void ApplyProperties(Renderer renderer)
  {
    this.PropBlock.Clear();
    this.PropBlock.SetColor(StencilLighting_Volumetric.overrideColorID, this.AdditiveColor);
    if (!((Object) renderer != (Object) null))
      return;
    renderer.SetPropertyBlock(this.PropBlock, 0);
  }

  public void Awake()
  {
    this.Init();
    this.ApplyProperties(this.m_renderer);
    if (this.gameObject.layer != 15 && this.gameObject.layer != 20)
      this.gameObject.layer = 20;
    if (!Application.isPlaying || SettingsManager.Settings == null || !SettingsManager.Settings.Game.PerformanceMode || !(SceneManager.GetActiveScene().name == "Base Biome 1") || !((Object) this.GetComponentInParent<DoorRoomManager>() == (Object) null))
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void UpdateVisibility()
  {
  }

  public void OnDisable()
  {
  }

  public void Update() => this.UpdateVisibility();

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
  }

  public int GetMaterialID()
  {
    if (this.materialID == -1)
    {
      if ((Object) this.sharedMaterial == (Object) null)
        this.sharedMaterial = this.m_renderer.sharedMaterial;
      if ((Object) this.sharedMaterial != (Object) null)
        this.materialID = this.sharedMaterial.GetInstanceID();
    }
    return this.materialID;
  }

  public int GetGameObjectID() => this.m_targetObj.GetInstanceID();

  public MeshFilter GetMeshFilter() => this.m_meshFilter;

  public Material GetSharedMaterial()
  {
    if ((Object) this.sharedMaterial == (Object) null)
      this.sharedMaterial = this.m_renderer.sharedMaterial;
    return this.sharedMaterial;
  }

  public Transform GetTransform() => this.m_targetObj.transform;
}
