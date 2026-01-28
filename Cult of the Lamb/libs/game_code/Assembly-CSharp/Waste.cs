// Decompiled with JetBrains decompiler
// Type: Waste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class Waste : BaseMonoBehaviour
{
  public static List<Waste> Wastes = new List<Waste>();
  public List<Transform> ShakeTransforms;
  public Vector2[] Shake = new Vector2[0];
  public Structure Structure;
  public Structures_Waste structureInfo;
  [FormerlySerializedAs("ProgressIndicator")]
  public UIProgressIndicator _uiProgressIndicator;
  [SerializeField]
  public ParticleSystem _particleSystem;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Waste StructureBrain
  {
    get
    {
      if (this.structureInfo == null && (UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
        this.structureInfo = this.Structure.Brain as Structures_Waste;
      return this.structureInfo;
    }
    set => this.structureInfo = value;
  }

  public void OnEnable()
  {
    Waste.Wastes.Add(this);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
    {
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
      if (this.Structure.Brain != null)
        this.OnBrainAssigned();
    }
    this.ObjectCreated();
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
    {
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
      if (this.StructureBrain != null)
        this.StructureBrain.OnRemovalProgressChanged -= new Action<int>(this.OnRemovalProgressChanged);
    }
    Waste.Wastes.Remove(this);
  }

  public void OnDestroy()
  {
    if (this.StructureBrain == null || this.StructureInfo == null)
      return;
    if (this.StructureInfo.Destroyed && !this.StructureBrain.ForceRemoved)
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
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
      InventoryItem.Spawn(this.StructureBrain.Data.LootToDrop, Mathf.RoundToInt((float) this.StructureBrain.DropAmount * num), this.transform.position);
    }
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    this.StructureBrain.OnRemovalProgressChanged -= new Action<int>(this.OnRemovalProgressChanged);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnRemovalProgressChanged += new Action<int>(this.OnRemovalProgressChanged);
  }

  public void ObjectCreated()
  {
    for (int index1 = 0; index1 < this.ShakeTransforms.Count; ++index1)
    {
      IEnumerator enumerator = (IEnumerator) this.ShakeTransforms[index1].transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.ShakeTransforms.Add((Transform) enumerator.Current);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      this.Shake = new Vector2[this.ShakeTransforms.Count];
      for (int index2 = 0; index2 < this.ShakeTransforms.Count; ++index2)
        this.Shake[index2] = (Vector2) this.ShakeTransforms[index2].transform.localPosition;
    }
  }

  public void ShakeRubble()
  {
    float num = this.Structure.Type != global::StructureBrain.TYPES.RUBBLE_BIG ? 0.5f : 1f;
    if (this.ShakeTransforms.Count <= 0)
      return;
    for (int index = 0; index < this.ShakeTransforms.Count; ++index)
    {
      if ((UnityEngine.Object) this.ShakeTransforms[index] != (UnityEngine.Object) null && this.ShakeTransforms[index].gameObject.activeSelf)
      {
        this.ShakeTransforms[index].DOKill();
        this.ShakeTransforms[index].transform.localPosition = (Vector3) this.Shake[index];
        this.ShakeTransforms[index].DOShakePosition(0.5f * num, (Vector3) new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f) * num, 0.0f));
      }
    }
  }

  public void UpdateBar(int followerID)
  {
    float progress = this.StructureBrain.RemovalProgress / this.StructureBrain.RemovalDurationInGameMinutes;
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
