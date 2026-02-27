// Decompiled with JetBrains decompiler
// Type: Rubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

#nullable disable
public class Rubble : BaseMonoBehaviour
{
  public static List<Rubble> Rubbles = new List<Rubble>();
  public List<Transform> ShakeTransforms;
  private Vector2[] Shake = new Vector2[0];
  public Structure Structure;
  private Structures_Rubble _StructureInfo;
  [FormerlySerializedAs("ProgressIndicator")]
  public UIProgressIndicator _uiProgressIndicator;
  public RandomObjectPicker objectPick;
  [SerializeField]
  private ParticleSystem _particleSystem;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Rubble StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Rubble;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void OnEnable()
  {
    Rubble.Rubbles.Add(this);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  private void OnDisable()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
      this.StructureBrain.OnRemovalProgressChanged -= new Action<int>(this.OnRemovalProgressChanged);
    Rubble.Rubbles.Remove(this);
  }

  private void Start() => this.objectPick.ObjectCreated += new UnityAction(this.ObjectCreated);

  private void ObjectCreated()
  {
    foreach (Transform componentsInChild in this.objectPick.CreatedObject.GetComponentsInChildren<Transform>())
      this.ShakeTransforms.Add(componentsInChild);
    this.Shake = new Vector2[this.ShakeTransforms.Count];
    for (int index = 0; index < this.ShakeTransforms.Count; ++index)
      this.Shake[index] = (Vector2) this.ShakeTransforms[index].transform.localPosition;
  }

  private void OnBrainAssigned()
  {
    CircleCollider2D componentInChildren = this.GetComponentInChildren<CircleCollider2D>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      AstarPath.active.UpdateGraphs(componentInChildren.bounds);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnRemovalProgressChanged += new Action<int>(this.OnRemovalProgressChanged);
  }

  private void OnDestroy()
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
      InventoryItem.Spawn(this.StructureBrain.Data.LootToDrop, Mathf.RoundToInt((float) this.StructureBrain.RubbleDropAmount * num), this.transform.position);
    }
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    this.StructureBrain.OnRemovalProgressChanged -= new Action<int>(this.OnRemovalProgressChanged);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
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
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
  }

  private void RubbleFX()
  {
    this.ShakeRubble();
    Vector3 position = this.transform.position;
    AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(SoundConstants.SoundMaterial.Stone), position);
    this._particleSystem.Play();
  }

  private void OnRemovalProgressChanged(int followerID)
  {
    this.RubbleFX();
    this.UpdateBar(followerID);
  }
}
