// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.SkeletonGhost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Spine.Unity.Examples;

[RequireComponent(typeof (SkeletonRenderer))]
public class SkeletonGhost : MonoBehaviour
{
  public const HideFlags GhostHideFlags = HideFlags.HideInHierarchy;
  public const string GhostingShaderName = "Spine/Special/SkeletonGhost";
  [Header("Animation")]
  public bool ghostingEnabled = true;
  [Tooltip("The time between invididual ghost pieces being spawned.")]
  [FormerlySerializedAs("spawnRate")]
  public float spawnInterval = 0.0333333351f;
  [Tooltip("Maximum number of ghosts that can exist at a time. If the fade speed is not fast enough, the oldest ghost will immediately disappear to enforce the maximum number.")]
  public int maximumGhosts = 10;
  public float fadeSpeed = 10f;
  [Header("Rendering")]
  public Shader ghostShader;
  public Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0);
  [Tooltip("Remember to set color alpha to 0 if Additive is true")]
  public bool additive = true;
  [Tooltip("0 is Color and Alpha, 1 is Alpha only.")]
  [Range(0.0f, 1f)]
  public float textureFade = 1f;
  [Header("Sorting")]
  public bool sortWithDistanceOnly;
  public float zOffset;
  public float nextSpawnTime;
  public SkeletonGhostRenderer[] pool;
  public int poolIndex;
  public SkeletonRenderer skeletonRenderer;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;
  public Dictionary<Material, Material> materialTable = new Dictionary<Material, Material>();

  public void Start()
  {
    this.maximumGhosts = 10;
    if (SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      this.maximumGhosts = 5;
    this.Initialize(false);
  }

  public void Initialize(bool overwrite)
  {
    if (!(this.pool == null | overwrite))
      return;
    if ((UnityEngine.Object) this.ghostShader == (UnityEngine.Object) null)
      this.ghostShader = Shader.Find("Spine/Special/SkeletonGhost");
    this.skeletonRenderer = this.GetComponent<SkeletonRenderer>();
    this.meshFilter = this.GetComponent<MeshFilter>();
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    this.nextSpawnTime = Time.time + this.spawnInterval;
    this.pool = new SkeletonGhostRenderer[this.maximumGhosts];
    for (int index = 0; index < this.maximumGhosts; ++index)
    {
      GameObject gameObject = new GameObject(this.gameObject.name + " Ghost", (System.Type[]) new System.Type[3]
      {
        typeof (SkeletonGhostRenderer),
        typeof (MeshRenderer),
        typeof (MeshFilter)
      });
      this.pool[index] = gameObject.GetComponent<SkeletonGhostRenderer>();
      gameObject.SetActive(false);
      gameObject.hideFlags = HideFlags.HideInHierarchy;
    }
    if (!(this.skeletonRenderer is IAnimationStateComponent skeletonRenderer))
      return;
    skeletonRenderer.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.OnEvent);
  }

  public void OnEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!e.Data.Name.Equals("Ghosting", StringComparison.Ordinal))
      return;
    this.ghostingEnabled = e.Int > 0;
    if ((double) e.Float > 0.0)
      this.spawnInterval = e.Float;
    if (string.IsNullOrEmpty(e.String))
      return;
    this.color = SkeletonGhost.HexToColor(e.String);
  }

  public void Ghosting(float val) => this.ghostingEnabled = (double) val > 0.0;

  public void Update()
  {
    if (!this.ghostingEnabled || this.poolIndex >= this.pool.Length || (double) Time.time < (double) this.nextSpawnTime)
      return;
    GameObject gameObject = this.pool[this.poolIndex].gameObject;
    Material[] sharedMaterials = this.meshRenderer.sharedMaterials;
    for (int index = 0; index < sharedMaterials.Length; ++index)
    {
      Material material1 = sharedMaterials[index];
      Material material2;
      if (!this.materialTable.ContainsKey(material1))
      {
        material2 = new Material(material1)
        {
          shader = this.ghostShader,
          color = Color.white
        };
        if (material2.HasProperty("_TextureFade"))
          material2.SetFloat("_TextureFade", this.textureFade);
        this.materialTable.Add(material1, material2);
      }
      else
        material2 = this.materialTable[material1];
      sharedMaterials[index] = material2;
    }
    Transform transform = gameObject.transform;
    transform.parent = this.transform;
    this.pool[this.poolIndex].Initialize(this.meshFilter.sharedMesh, sharedMaterials, this.color, this.additive, this.fadeSpeed, this.meshRenderer.sortingLayerID, this.sortWithDistanceOnly ? this.meshRenderer.sortingOrder : this.meshRenderer.sortingOrder - 1);
    transform.localPosition = new Vector3(0.0f, 0.0f, this.zOffset);
    transform.localRotation = Quaternion.identity;
    transform.localScale = Vector3.one;
    transform.parent = (Transform) null;
    ++this.poolIndex;
    if (this.poolIndex == this.pool.Length)
      this.poolIndex = 0;
    this.nextSpawnTime = Time.time + this.spawnInterval;
  }

  public void OnDestroy()
  {
    if (this.pool != null)
    {
      for (int index = 0; index < this.maximumGhosts; ++index)
      {
        if ((UnityEngine.Object) this.pool[index] != (UnityEngine.Object) null)
          this.pool[index].Cleanup();
      }
    }
    foreach (UnityEngine.Object @object in this.materialTable.Values)
      UnityEngine.Object.Destroy(@object);
  }

  public static Color32 HexToColor(string hex)
  {
    if (hex.Length < 6)
      return (Color32) Color.magenta;
    hex = hex.Replace("#", "");
    int r = (int) byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
    byte num1 = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
    byte num2 = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
    byte maxValue = byte.MaxValue;
    if (hex.Length == 8)
      maxValue = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
    int g = (int) num1;
    int b = (int) num2;
    int a = (int) maxValue;
    return new Color32((byte) r, (byte) g, (byte) b, (byte) a);
  }
}
