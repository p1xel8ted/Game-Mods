// Decompiled with JetBrains decompiler
// Type: Interaction_WoodcuttingRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_WoodcuttingRubble : TreeBase
{
  private UIProgressIndicator _uiProgressIndicator;
  public ParticleSystem TreeHitParticles;
  private string sLabelName;
  public bool Activated;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  public SkeletonAnimation skeletonAnimation;
  public string chopWoodEventName = "Chop";
  public RandomObjectPicker objectPick;
  public List<Transform> ShakeTransforms;
  private Vector2[] Shake = new Vector2[0];

  private void Start()
  {
    this.UpdateLocalisation();
    this.objectPick.ObjectCreated += new UnityAction(this.ObjectCreated);
  }

  private void ObjectCreated()
  {
    foreach (Transform componentsInChild in this.objectPick.CreatedObject.GetComponentsInChildren<Transform>())
      this.ShakeTransforms.Add(componentsInChild);
    this.Shake = new Vector2[this.ShakeTransforms.Count];
    for (int index = 0; index < this.ShakeTransforms.Count; ++index)
      this.Shake[index] = (Vector2) this.ShakeTransforms[index].transform.localPosition;
  }

  public void ShakeRubble()
  {
    float num = 0.5f;
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

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabelName = ScriptLocalization.Interactions.ChopWood;
  }

  public override void GetLabel()
  {
    if (!this.Activated)
    {
      if (this.StructureBrain == null)
        return;
      Debug.Log((object) ("StructureBrain.TreeChopped: ".Colour(Color.green) + this.StructureBrain.TreeChopped.ToString()));
      Debug.Log((object) ("StructureBrain.Data.IsSapling: ".Colour(Color.green) + this.StructureBrain.Data.IsSapling.ToString()));
      this.Label = this.StructureBrain.TreeChopped || this.StructureBrain.Data.IsSapling ? "" : this.sLabelName;
    }
    else
      this.Label = "";
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    TreeBase.Trees.Add((TreeBase) this);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    else
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public override void OnDisableInteraction()
  {
    if ((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnDisableInteraction();
    TreeBase.Trees.Remove((TreeBase) this);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnTreeProgressChanged -= new System.Action<int>(this.OnTreeHit);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.OnChoppedDown);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain.TreeChopped || this.StructureBrain.Data.IsSapling)
    {
      this.StructureBrain.Remove();
      this.enabled = false;
    }
    else
    {
      this.StructureBrain.OnTreeProgressChanged += new System.Action<int>(this.OnTreeHit);
      this.StructureBrain.OnTreeComplete += new System.Action<bool>(this.OnChoppedDown);
    }
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == this.chopWoodEventName && this.StructureBrain != null)
      this.StructureBrain.TreeHit(1f + UpgradeSystem.Chopping);
    if (!(e.Data.Name == "swipe_1"))
      return;
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
  }

  public void OnTreeHit(int followerID)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      float angle = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.gameObject.transform.position);
      BiomeConstants.Instance.EmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, angle);
    }
    this.TreeHitParticles.Play();
    this.ShakeRubble();
    AudioManager.Instance.PlayOneShot("event:/material/tree_chop", this.transform.position);
    this.gameObject.transform.DOShakePosition(0.1f, 0.1f, 13, 48.8f);
    float progress = this.StructureBrain.Data.Progress / this.StructureBrain.Data.ProgressTarget;
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

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    DataManager.Instance.FirstTimeChop = true;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.Activated = true;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnInteract(state);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoWoodCutting());
    this.Interactable = false;
  }

  public void OnChoppedDown(bool dropLoot)
  {
    this.TreeHitParticles.Play();
    if (dropLoot)
    {
      int num1 = this.Structure.Structure_Info.LootCountToDrop;
      if (this.Activated)
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(InventoryItem.ITEM_TYPE.LOG) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position);
    }
    if (this.Activated)
      this.EndChopping();
    AudioManager.Instance.PlayOneShot("event:/material/tree_break", this.transform.position);
    this.Interactable = false;
    Debug.Log((object) "REMOVE TREEEEEE");
    this.StructureBrain.Remove();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    if (!(bool) (UnityEngine.Object) this.skeletonAnimation)
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void EndChopping()
  {
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    this.Activated = false;
    this.Interactable = true;
    this.HasChanged = true;
  }

  private IEnumerator DoWoodCutting()
  {
    Interaction_WoodcuttingRubble woodcuttingRubble = this;
    woodcuttingRubble.state.facingAngle = Utils.GetAngle(woodcuttingRubble.state.transform.position, woodcuttingRubble.transform.position);
    woodcuttingRubble.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/chop-wood", 0, true);
    yield return (object) new WaitForSeconds(0.766666651f);
    while (InputManager.Gameplay.GetInteractButtonHeld() && woodcuttingRubble.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    woodcuttingRubble.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(woodcuttingRubble.HandleEvent);
    woodcuttingRubble.EndChopping();
  }
}
