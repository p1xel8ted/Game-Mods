// Decompiled with JetBrains decompiler
// Type: VFXAbilitySequenceData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/VFX Ability Sequence")]
public class VFXAbilitySequenceData : ScriptableObject
{
  [SerializeField]
  public bool _animate = true;
  [SerializeField]
  public string _activationAnimationName;
  [SerializeField]
  public float _animationDuration = 1f;
  [SerializeField]
  public VFXObject[] _activationVFXObjects;
  [SerializeField]
  public VFXObject _impactVFXObject;
  [SerializeField]
  public float _multipleImpactDelay;

  public bool Animate => this._animate;

  public string ActivationAnimationName => this._activationAnimationName;

  public float AnimationDuration => this._animationDuration;

  public VFXObject[] ActivationVFXObjects => this._activationVFXObjects;

  public VFXObject ImpactVFXObject => this._impactVFXObject;

  public void TestSequence(int targetNumber, bool targetSelf, PlayerFarming playerFarming)
  {
    if (!Application.isPlaying)
      return;
    Transform[] targetTransforms = new Transform[targetSelf ? 1 : targetNumber];
    if (targetSelf)
    {
      targetTransforms[0] = playerFarming.transform;
      this.PlayNewSequence(targetTransforms[0], targetTransforms);
    }
    else
    {
      for (int index = 0; index < targetNumber; ++index)
      {
        GameObject gameObject = new GameObject();
        targetTransforms[index] = gameObject.transform;
        targetTransforms[index].position = playerFarming.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 5f);
      }
      VFXSequence sequence = this.PlayNewSequence(playerFarming.transform, targetTransforms);
      sequence.OnComplete += (System.Action) (() =>
      {
        sequence.OnComplete -= (System.Action) (() =>
        {
          // ISSUE: unable to decompile the method.
        });
        for (int index = 0; index < targetTransforms.Length; ++index)
          UnityEngine.Object.Destroy((UnityEngine.Object) targetTransforms[index].gameObject);
      });
    }
  }

  public VFXSequence PlayNewSequence(
    Transform caster,
    Transform[] targets,
    Transform vfxParent = null,
    bool onlyImpact = false)
  {
    return new VFXSequence(this, caster, targets, vfxParent, this._multipleImpactDelay, onlyImpact);
  }
}
