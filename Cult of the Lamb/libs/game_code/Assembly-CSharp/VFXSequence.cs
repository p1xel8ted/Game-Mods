// Decompiled with JetBrains decompiler
// Type: VFXSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class VFXSequence
{
  public VFXObject[] ActivationVFXObjects;
  public VFXObject[] ImpactVFXObjects;
  public Action<VFXObject> OnActivation;
  public Transform[] _targets;
  public Action<VFXObject, int> OnImpact;
  public System.Action OnComplete;

  public Transform[] Targets => this._targets;

  public VFXSequence(
    VFXAbilitySequenceData data,
    Transform caster,
    Transform[] targets,
    Transform vfxParent = null,
    float emissionDelay = 0.0f,
    bool onlyImpact = false)
  {
    this._targets = targets;
    if ((UnityEngine.Object) caster == (UnityEngine.Object) null)
      caster = PlayerFarming.Instance.transform;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(caster.gameObject);
    if (!onlyImpact)
    {
      if (data.ActivationAnimationName != null && data.Animate)
      {
        Debug.Log((object) ("VFXSequence: Activating anim " + data.ActivationAnimationName));
        farmingComponent.TimedAction(data.AnimationDuration, (System.Action) (() => Debug.Log((object) ("VFXSequence: Animation Ended " + data.ActivationAnimationName))), data.ActivationAnimationName);
      }
      this.ActivationVFXObjects = new VFXObject[data.ActivationVFXObjects.Length];
      for (int index = 0; index < data.ActivationVFXObjects.Length; ++index)
      {
        this.ActivationVFXObjects[index] = data.ActivationVFXObjects[index].SpawnVFX(caster, false, vfxParent);
        this.ActivationVFXObjects[index].transform.SetPositionAndRotation(caster.position, Quaternion.identity);
        if (index == 0)
          this.ActivationVFXObjects[index].OnEmitted += new Action<VFXObject>(this.OnActivationStarted);
        this.ActivationVFXObjects[index].PlayVFX();
      }
    }
    else
      emissionDelay = 0.0f;
    if (targets == null || !((UnityEngine.Object) data.ImpactVFXObject != (UnityEngine.Object) null))
      return;
    this.ImpactVFXObjects = new VFXObject[targets.Length];
    for (int index = 0; index < targets.Length; ++index)
    {
      this.ImpactVFXObjects[index] = data.ImpactVFXObject.SpawnVFX(targets[index].transform, false, vfxParent);
      this.ImpactVFXObjects[index].OnEmitted += new Action<VFXObject>(this.OnImpactTriggered);
      if (!this.ImpactVFXObjects[index].Playing)
        this.ImpactVFXObjects[index].PlayVFX(emissionDelay * (float) index);
    }
  }

  public void OnImpactTriggered(VFXObject vfxObject)
  {
    vfxObject.OnEmitted -= new Action<VFXObject>(this.OnImpactTriggered);
    Action<VFXObject, int> onImpact = this.OnImpact;
    if (onImpact != null)
      onImpact(vfxObject, this.ImpactVFXObjects.IndexOf<VFXObject>(vfxObject));
    if (!((UnityEngine.Object) vfxObject == (UnityEngine.Object) this.ImpactVFXObjects[this.ImpactVFXObjects.Length - 1]))
      return;
    System.Action onComplete = this.OnComplete;
    if (onComplete == null)
      return;
    onComplete();
  }

  public void OnActivationStarted(VFXObject vfxObject)
  {
    vfxObject.OnEmitted -= new Action<VFXObject>(this.OnActivationStarted);
    Action<VFXObject> onActivation = this.OnActivation;
    if (onActivation == null)
      return;
    onActivation(vfxObject);
  }
}
