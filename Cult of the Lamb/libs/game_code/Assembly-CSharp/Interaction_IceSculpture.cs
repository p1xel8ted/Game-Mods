// Decompiled with JetBrains decompiler
// Type: Interaction_IceSculpture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_IceSculpture : Interaction
{
  public static List<Interaction_IceSculpture> IceSculptures = new List<Interaction_IceSculpture>();
  public System.Action OnStateChagned;
  public Structure structure;
  public Structures_IceSculpture iceSculptureBrain;
  [SerializeField]
  public SpriteRenderer visual;
  [Tooltip("Should contains 3 stages.")]
  [SerializeField]
  public List<Sprite> stageSprites;
  [SerializeField]
  public ParticleSystem iceHitParticle;
  [Range(0.0f, 2f)]
  public int currentState;
  public string sString;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public bool Activating
  {
    get => this.iceSculptureBrain != null && this.iceSculptureBrain.ReservedByPlayer;
    set => this.iceSculptureBrain.ReservedByPlayer = value;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.BreakIce;
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void OnEnable()
  {
    base.OnEnable();
    this.OnStateChagned += new System.Action(this.OnStateChange);
    Interaction_IceSculpture.IceSculptures.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.OnStateChagned -= new System.Action(this.OnStateChange);
    Interaction_IceSculpture.IceSculptures.Remove(this);
  }

  public void Start()
  {
    this.UpdateLocalisation();
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
      if (this.structure.Brain == null)
        return;
      this.OnBrainAssigned();
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.StructureInfo == null || !this.StructureInfo.Destroyed || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      return;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/icesculpture/ice_destroy", this.transform.position);
    for (int index = 0; index < 3; ++index)
    {
      Vector3 position = this.transform.position + Vector3.down + (Vector3) UnityEngine.Random.insideUnitCircle.normalized * 0.2f + Vector3.back * 2f;
      BiomeConstants.Instance.SpawnPuffEffect(position, this.transform.parent);
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.DoClean());
  }

  public void LateUpdate()
  {
    if (this.iceSculptureBrain == null)
      return;
    this.UpdateState();
    this.UpdateVisuals();
  }

  public IEnumerator DoClean()
  {
    Interaction_IceSculpture interactionIceSculpture = this;
    interactionIceSculpture.Activating = true;
    interactionIceSculpture.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionIceSculpture.state.facingAngle = Utils.GetAngle(interactionIceSculpture.state.transform.position, interactionIceSculpture.transform.position);
    yield return (object) new WaitForEndOfFrame();
    float multiplier = 1f;
    interactionIceSculpture.playerFarming.simpleSpineAnimator.Animate("actions/chop-stone", 0, true);
    yield return (object) new WaitForSeconds(0.25f);
    AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(SoundConstants.SoundMaterial.Glass), interactionIceSculpture.transform.position);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    float ChoreDuration = DataManager.GetChoreDuration(interactionIceSculpture.playerFarming);
    yield return (object) new WaitForSeconds(ChoreDuration / 2f);
    if (interactionIceSculpture.StructureInfo.Type != StructureBrain.TYPES.POOP_PET)
      interactionIceSculpture._playerFarming.playerChoreXPBarController.AddChoreXP(interactionIceSculpture.playerFarming, multiplier);
    yield return (object) new WaitForSeconds(ChoreDuration / 2f);
    interactionIceSculpture.iceSculptureBrain.Remove();
    ++DataManager.Instance.itemsCleaned;
    System.Action onCrownReturn = interactionIceSculpture.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    interactionIceSculpture.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionIceSculpture.Activating = false;
  }

  public void OnBrainAssigned()
  {
    this.iceSculptureBrain = (Structures_IceSculpture) this.structure.Brain;
    this.UpdateVisuals();
  }

  public void UpdateVisuals() => this.visual.sprite = this.stageSprites[this.currentState];

  public void UpdateState()
  {
    if ((double) this.iceSculptureBrain.Data.Progress >= 1.0 && this.currentState < 2)
    {
      this.currentState = 2;
      System.Action onStateChagned = this.OnStateChagned;
      if (onStateChagned == null)
        return;
      onStateChagned();
    }
    else
    {
      if ((double) this.iceSculptureBrain.Data.Progress < 0.5 || this.currentState >= 1)
        return;
      this.currentState = 1;
      System.Action onStateChagned = this.OnStateChagned;
      if (onStateChagned == null)
        return;
      onStateChagned();
    }
  }

  public void OnStateChange()
  {
    BiomeConstants.Instance.EmitSnowImpactVFX(this.transform.position);
    BiomeConstants.Instance.SpawnPuffEffect(this.transform.position + Vector3.down, this.transform);
  }

  public void Hit(Vector3 position)
  {
    this.iceHitParticle.transform.position = position;
    this.iceHitParticle.Play();
    this.transform.DOShakePosition(0.1f, 0.1f, 5, fadeOut: false);
  }
}
