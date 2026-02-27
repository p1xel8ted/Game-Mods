// Decompiled with JetBrains decompiler
// Type: Rubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

#nullable disable
public class Rubble : BaseMonoBehaviour
{
  public static List<Rubble> Rubbles = new List<Rubble>();
  public List<Transform> ShakeTransforms;
  public Structure Structure;
  public Structures_Rubble _StructureInfo;
  [FormerlySerializedAs("ProgressIndicator")]
  public UIProgressIndicator _uiProgressIndicator;
  public RandomObjectPicker objectPick;
  [SerializeField]
  public ParticleSystem _particleSystem;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Rubble structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Rubble;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void OnEnable()
  {
    Rubble.Rubbles.Add(this);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnDisable()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structureBrain != null)
      this.structureBrain.OnRemovalProgressChanged -= new Action<int>(this.OnRemovalProgressChanged);
    Rubble.Rubbles.Remove(this);
  }

  public void Start()
  {
    if (!((UnityEngine.Object) this.objectPick != (UnityEngine.Object) null))
      return;
    this.objectPick.ObjectCreated += new UnityAction(this.ObjectCreated);
  }

  public void ObjectCreated()
  {
    foreach (Transform componentsInChild in this.objectPick.CreatedObject.GetComponentsInChildren<Transform>())
      this.ShakeTransforms.Add(componentsInChild);
  }

  public void OnBrainAssigned()
  {
    CircleCollider2D componentInChildren = this.GetComponentInChildren<CircleCollider2D>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      AstarPath.active.UpdateGraphs(componentInChildren.bounds);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structureBrain.OnRemovalProgressChanged += new Action<int>(this.OnRemovalProgressChanged);
    if (!this.structureBrain.Data.Destroyed)
      return;
    this.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    if (this.structureBrain == null || this.StructureInfo == null)
      return;
    if (this.StructureInfo.Destroyed && !this.structureBrain.ForceRemoved)
    {
      float num = 1f;
      if (this.StructureInfo.FollowerID != -1)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID);
        if (infoById != null)
        {
          FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
          if (brain != null)
            num = brain.ResourceHarvestingMultiplier;
        }
      }
      AudioManager.Instance.PlayOneShot(SoundConstants.GetBreakSoundPathForMaterial(SoundConstants.SoundMaterial.Stone), this.transform.position);
      if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null && (UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
      InventoryItem.Spawn(this.structureBrain.Data.LootToDrop, Mathf.RoundToInt((float) this.structureBrain.RubbleDropAmount * num), this.transform.position);
    }
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    if (this.structureBrain != null && this.structureBrain.ProgressFinished)
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Base:
          if (this.structureBrain.Data.PlacementRegionPosition != Vector3Int.zero)
          {
            this.structureBrain.RemoveFromGrid();
            break;
          }
          break;
        case FollowerLocation.DLC_ShrineRoom:
          StructureManager.RemoveStructure((StructureBrain) this.structureBrain);
          break;
      }
    }
    this.structureBrain.OnRemovalProgressChanged -= new Action<int>(this.OnRemovalProgressChanged);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void ShakeRubble()
  {
    float num = this.Structure.Type != StructureBrain.TYPES.RUBBLE_BIG ? 0.5f : 1f;
    if (this.ShakeTransforms.Count <= 0)
      return;
    for (int index = 0; index < this.ShakeTransforms.Count; ++index)
    {
      if ((UnityEngine.Object) this.ShakeTransforms[index] != (UnityEngine.Object) null && this.ShakeTransforms[index].gameObject.activeSelf)
      {
        if ((UnityEngine.Object) this.ShakeTransforms[index].transform == (UnityEngine.Object) null)
          break;
        this.ShakeTransforms[index].DOComplete();
        this.ShakeTransforms[index].DOShakePosition(0.5f * num, (Vector3) new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f) * num, 0.0f));
      }
    }
  }

  public void UpdateBar(int followerID)
  {
    float progress = this.structureBrain.RemovalProgress / this.structureBrain.RemovalDurationInGameMinutes;
    if ((double) progress == 0.0)
      return;
    if ((UnityEngine.Object) this._uiProgressIndicator == (UnityEngine.Object) null)
    {
      this._uiProgressIndicator = BiomeConstants.Instance.ProgressIndicatorTemplate.Spawn<UIProgressIndicator>(BiomeConstants.Instance.transform, this.transform.position + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position);
      this._uiProgressIndicator.Show(progress);
      this._uiProgressIndicator.OnHidden += (System.Action) (() => this._uiProgressIndicator = (UIProgressIndicator) null);
    }
    else
      this._uiProgressIndicator.SetProgress(progress);
  }

  public void PlayerRubbleFX()
  {
    CameraManager.shakeCamera(0.3f);
    this.ShakeRubble();
    Vector3 position = this.transform.position;
    AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(SoundConstants.SoundMaterial.Stone), position);
    this._particleSystem.Play();
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
  }

  public void RubbleFX()
  {
    this.ShakeRubble();
    Vector3 position = this.transform.position;
    AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(SoundConstants.SoundMaterial.Stone), position);
    this._particleSystem.Play();
  }

  public void OnRemovalProgressChanged(int followerID)
  {
    this.RubbleFX();
    this.UpdateBar(followerID);
  }

  [CompilerGenerated]
  public void \u003CUpdateBar\u003Eb__18_0()
  {
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }
}
