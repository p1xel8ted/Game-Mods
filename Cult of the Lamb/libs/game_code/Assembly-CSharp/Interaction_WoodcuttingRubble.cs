// Decompiled with JetBrains decompiler
// Type: Interaction_WoodcuttingRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_WoodcuttingRubble : TreeBase
{
  public UIProgressIndicator _uiProgressIndicator;
  public ParticleSystem TreeHitParticles;
  public string sLabelName;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  public List<PlayerFarming> activatingPlayers = new List<PlayerFarming>();
  public string chopWoodEventName = "Chop";
  public RandomObjectPicker objectPick;
  public bool[] buttonDown = new bool[2];
  public List<Transform> ShakeTransforms;
  public Vector2[] Shake = new Vector2[0];

  public override bool InactiveAfterStopMoving => false;

  public void Start()
  {
    this.FreezeCoopPlayersOnHoldToInteract = false;
    this.UpdateLocalisation();
    this.objectPick.ObjectCreated += new UnityAction(this.ObjectCreated);
  }

  public void ObjectCreated()
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
    if (this.StructureBrain == null)
      return;
    this.Label = this.StructureBrain.TreeChopped || this.StructureBrain.Data.IsSapling ? "" : this.sLabelName;
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
    if ((UnityEngine.Object) this._playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) this._playerFarming.Spine != (UnityEngine.Object) null)
      this._playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnDisableInteraction();
    TreeBase.Trees.Remove((TreeBase) this);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnTreeProgressChanged -= new System.Action<int>(this.OnTreeHit);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.OnChoppedDown);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
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

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == this.chopWoodEventName && this.StructureBrain != null)
      this.StructureBrain.TreeHit(1f + UpgradeSystem.Chopping);
    if (!(e.Data.Name == "swipe_1"))
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) trackEntry.TrackTime == (double) player.Spine.AnimationState.GetCurrent(0).TrackTime)
      {
        CameraManager.shakeCamera(0.1f, (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null ? Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position) : 0.1f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      }
    }
  }

  public void OnTreeHit(int followerID)
  {
    float Angle = (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null ? Utils.GetAngle(this.transform.position, this.playerFarming.gameObject.transform.position) : UnityEngine.Random.value * 360f;
    BiomeConstants.Instance.EmitHitImpactEffect(this.transform.position + Vector3.back * 0.5f, Angle);
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
    DataManager.Instance.FirstTimeChop = true;
    base.OnInteract(state);
    if ((UnityEngine.Object) this.interactorSkeletonAnimation == (UnityEngine.Object) null)
      this.interactorSkeletonAnimation = this.playerFarming.Spine;
    if ((UnityEngine.Object) this.interactorSkeletonAnimation == (UnityEngine.Object) null)
      return;
    this.activatingPlayers.Add(this._playerFarming);
    this._playerFarming.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.StartCoroutine((IEnumerator) this.DoWoodCutting(this._playerFarming));
  }

  public override void Update()
  {
    base.Update();
    if (this.activatingPlayers.Count <= 0)
      return;
    foreach (PlayerFarming activatingPlayer in this.activatingPlayers)
    {
      if (InputManager.Gameplay.GetInteractButtonUp(activatingPlayer) && SettingsManager.Settings.Accessibility.HoldActions || (UnityEngine.Object) activatingPlayer != (UnityEngine.Object) null && activatingPlayer.state.CURRENT_STATE == StateMachine.State.Meditate || !SettingsManager.Settings.Accessibility.HoldActions && InputManager.Gameplay.GetAnyButtonDownExcludingMouse(activatingPlayer) && !InputManager.Gameplay.GetInteractButtonDown(activatingPlayer))
        this.buttonDown[PlayerFarming.players.IndexOf(activatingPlayer)] = false;
    }
  }

  public void OnChoppedDown(bool dropLoot)
  {
    this.TreeHitParticles.Play();
    if (dropLoot)
    {
      int num1 = this.Structure.Structure_Info.LootCountToDrop;
      if (this.activatingPlayers.Count > 0)
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(InventoryItem.ITEM_TYPE.LOG, this.playerFarming) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position);
    }
    if (this.activatingPlayers.Count > 0)
    {
      for (int index = this.activatingPlayers.Count - 1; index >= 0; --index)
        this.EndChopping(this.activatingPlayers[index]);
    }
    AudioManager.Instance.PlayOneShot("event:/material/tree_break", this.transform.position);
    Debug.Log((object) "REMOVE TREEEEEE");
    this.StructureBrain.Remove();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
  }

  public void EndChopping(PlayerFarming player)
  {
    System.Action onCrownReturn = player.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      player.state.CURRENT_STATE = StateMachine.State.Idle;
    player.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public IEnumerator DoWoodCutting(PlayerFarming player)
  {
    Interaction_WoodcuttingRubble woodcuttingRubble = this;
    woodcuttingRubble.buttonDown[PlayerFarming.players.IndexOf(player)] = true;
    player.state.facingAngle = Utils.GetAngle(player.state.transform.position, woodcuttingRubble.transform.position);
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    player.simpleSpineAnimator.Animate("actions/chop-wood", 0, true);
    yield return (object) new WaitForSeconds(0.766666651f);
    while (woodcuttingRubble.buttonDown[PlayerFarming.players.IndexOf(player)] && player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    woodcuttingRubble.EndChopping(player);
  }

  [CompilerGenerated]
  public void \u003COnTreeHit\u003Eb__23_0()
  {
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }
}
