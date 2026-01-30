// Decompiled with JetBrains decompiler
// Type: VFXObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public abstract class VFXObject : MonoBehaviour
{
  [SerializeField]
  public float _emissionDelay;
  [SerializeField]
  public bool _despawnOnStop = true;
  [SerializeField]
  public float _maxDuration = -1f;
  [SerializeField]
  public bool _followsTarget;
  public Action<VFXObject> OnStarted;
  public Action<VFXObject> OnEmitted;
  public Action<VFXObject> OnStopped;
  public Action<VFXObject> OnEnabled;
  public Action<VFXObject> OnDisabled;
  [SerializeField]
  public string SFX;
  [SerializeField]
  public RelicSubType relicType;
  [SerializeField]
  public MMVibrate.HapticTypes HapticType = MMVibrate.HapticTypes.None;
  public VFXObject _instantiatedFrom;
  public bool _initialized;
  public bool _playing;
  [SerializeField]
  public List<VFXMaterialOverride> _materialOverrides;
  public List<VFXMaterialOverride> _activeOverrides;
  public MaterialPropertyBlock _materialPropertyBlock;
  public Transform _targetTransform;
  public bool _playSFX = true;

  public float EmissionDelay => this._emissionDelay;

  public float MaxDuration => this._maxDuration;

  public bool FollowsTarget => this._followsTarget;

  public VFXObject InstantiatedFrom => this._instantiatedFrom;

  public bool Initialized => this._initialized;

  public bool Playing => this._playing;

  public virtual VFXObject SpawnVFX(
    Transform target,
    bool playOnSpawn,
    Transform parent = null,
    bool playSFX = true)
  {
    this._targetTransform = target;
    this._playSFX = playSFX;
    VFXObject vfxObject;
    if ((UnityEngine.Object) this._instantiatedFrom == (UnityEngine.Object) null)
    {
      vfxObject = this.Spawn<VFXObject>(this._followsTarget ? target : parent);
      this._instantiatedFrom = this;
      vfxObject._instantiatedFrom = this;
    }
    else
    {
      vfxObject = this._instantiatedFrom.Spawn<VFXObject>(this._followsTarget ? target : parent);
      vfxObject._instantiatedFrom = this._instantiatedFrom;
    }
    if (this._followsTarget)
      this.MatchPrefabLocalTransform(vfxObject.transform);
    vfxObject._targetTransform = target;
    if (playOnSpawn)
      vfxObject.PlayVFX();
    Debug.Log((object) vfxObject._instantiatedFrom);
    return vfxObject;
  }

  public virtual void Init()
  {
    this._initialized = true;
    this._activeOverrides = this._materialOverrides;
    this._materialPropertyBlock = new MaterialPropertyBlock();
  }

  public virtual void PlayVFX(float addEmissionDelay = 0.0f, PlayerFarming playerFarming = null, bool playSFX = true)
  {
    if (!this._initialized)
      this.Init();
    Action<VFXObject> onStarted = this.OnStarted;
    if (onStarted != null)
      onStarted(this);
    this._playSFX = playSFX;
    this._playing = true;
    this.UpdateMaterialOverrides();
    this.StartCoroutine((IEnumerator) this.DelayedEmit(this._emissionDelay + addEmissionDelay));
  }

  public virtual void AddMaterialOverride(VFXMaterialOverride vfxMaterialOverride)
  {
    this._activeOverrides.Add(vfxMaterialOverride);
  }

  public virtual void UpdateMaterialOverrides()
  {
    if (this._activeOverrides.Count == 0)
      return;
    this._materialPropertyBlock.Clear();
    foreach (VFXMaterialOverride materialOverride in this._materialOverrides)
      materialOverride.Apply(ref this._materialPropertyBlock);
  }

  public virtual void Emit()
  {
    if ((UnityEngine.Object) this._targetTransform != (UnityEngine.Object) null)
    {
      if (this._followsTarget)
        this.MatchPrefabLocalTransform(this.transform);
      else
        this.transform.SetPositionAndRotation(this._targetTransform.position, this._targetTransform.rotation);
    }
    if (!this.SFX.IsNullOrEmpty() && this._playSFX)
    {
      if (this.relicType == RelicSubType.Blessed)
        AudioManager.Instance.ToggleFilter("blessed", true);
      else if (this.relicType == RelicSubType.Dammed)
        AudioManager.Instance.ToggleFilter("dammed", true);
      AudioManager.Instance.PlayOneShot(this.SFX, this.gameObject);
    }
    if (this.HapticType != MMVibrate.HapticTypes.None)
      MMVibrate.Haptic(this.HapticType);
    Action<VFXObject> onEmitted = this.OnEmitted;
    if (onEmitted != null)
      onEmitted(this);
    if ((double) this._maxDuration <= 0.0)
      return;
    this.StartCoroutine((IEnumerator) this.DelayedStop(this._maxDuration));
  }

  public void MatchPrefabLocalTransform(Transform thisTransform)
  {
    Transform transform = this._instantiatedFrom.transform;
    thisTransform.localPosition = transform.localPosition;
    thisTransform.localRotation = transform.localRotation;
  }

  public virtual void StopVFX()
  {
    this.TriggerStopEvent();
    this.CancelVFX();
  }

  public void TriggerStopEvent()
  {
    if (this._playing)
    {
      Action<VFXObject> onStopped = this.OnStopped;
      if (onStopped != null)
        onStopped(this);
    }
    this._playing = false;
  }

  public virtual void CancelVFX()
  {
    this.StopAllCoroutines();
    if (this._playing)
    {
      Action<VFXObject> onStopped = this.OnStopped;
      if (onStopped != null)
        onStopped(this);
    }
    this._playing = false;
    if (!this._despawnOnStop)
      return;
    this.Recycle<VFXObject>();
  }

  public IEnumerator DelayedStop(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    if (this._playing)
      this.StopVFX();
  }

  public IEnumerator DelayedEmit(float delay)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    this.Emit();
  }

  public virtual void OnEnable()
  {
    Action<VFXObject> onEnabled = this.OnEnabled;
    if (onEnabled == null)
      return;
    onEnabled(this);
  }

  public virtual void OnDisable()
  {
    Action<VFXObject> onDisabled = this.OnDisabled;
    if (onDisabled == null)
      return;
    onDisabled(this);
  }
}
