// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerClearRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PlayerClearRubble : Interaction
{
  public Rubble rubble;
  public bool RequireUseOthersFirst;
  private bool Activating;
  public static System.Action<Rubble> PlayerActivatingStart;
  public static System.Action<Rubble> PlayerActivatingEnd;
  [SerializeField]
  private ParticleSystem _particleSystem;
  private string sString;
  private string sMine;
  private bool helpedFollower;
  private float size;

  private void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    this.HasSecondaryInteraction = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ClearRubble;
    this.sMine = ScriptLocalization.Interactions.MineBloodStone;
  }

  public override void GetLabel()
  {
    if (!this.RequireUseOthersFirst || this.RequireUseOthersFirst && DataManager.Instance.FirstTimeMine)
      this.Label = $"{this.sMine} {FontImageNames.GetIconByType(this.rubble.StructureInfo.LootToDrop)}";
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    DataManager.Instance.FirstTimeMine = true;
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.DoBuild());
  }

  private new void Update()
  {
    if (this.rubble.StructureInfo == null)
      return;
    if (this.Activating && !this.helpedFollower)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain.CurrentTask is FollowerTask_ClearRubble currentTask && currentTask.RubbleID == this.rubble.Structure.Structure_Info.ID && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
        {
          this.helpedFollower = true;
          break;
        }
      }
    }
    if (!this.rubble.StructureInfo.Prioritised || !((UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) this))
      return;
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = Color.yellow;
  }

  public override void OnBecomeCurrent()
  {
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = Color.white;
    base.OnBecomeCurrent();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (!this.rubble.StructureInfo.Prioritised)
    {
      this.rubble.StructureInfo.Prioritised = true;
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = Color.yellow;
    }
    else
    {
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = Color.white;
      this.rubble.StructureInfo.Prioritised = false;
    }
    this.ForceFollowersToUpdateRubble();
  }

  private void ForceFollowersToUpdateRubble()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.ClearRubble || allBrain.CurrentTaskType == FollowerTaskType.ClearWeeds || allBrain.CurrentTaskType == FollowerTaskType.Build)
        allBrain.CheckChangeTask();
    }
  }

  private void SimpleSpineAnimator_OnSpineEvent(string EventName)
  {
    Debug.Log((object) ("Eventname: " + EventName));
    if (!(EventName == "Chop"))
      return;
    this.rubble.StructureBrain.RemovalProgress += 6f + UpgradeSystem.Mining;
    this.rubble.StructureBrain.UpdateProgress();
    this.rubble.PlayerRubbleFX();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!this.Activating)
      return;
    if ((UnityEngine.Object) this.rubble._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this.rubble._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this.rubble._uiProgressIndicator = (UIProgressIndicator) null;
    }
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    AudioManager.Instance.PlayOneShot(SoundConstants.GetBreakSoundPathForMaterial(SoundConstants.SoundMaterial.Stone), this.transform.position);
    CameraManager.shakeCamera(0.3f);
    this.StopAllCoroutines();
    MMVibrate.StopRumble();
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (!this.helpedFollower)
      return;
    CultFaithManager.AddThought(Thought.Cult_HelpFollower);
  }

  private IEnumerator DoBuild()
  {
    Interaction_PlayerClearRubble playerClearRubble = this;
    playerClearRubble.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    playerClearRubble.state.facingAngle = Utils.GetAngle(playerClearRubble.state.transform.position, playerClearRubble.transform.position);
    System.Action<Rubble> playerActivatingStart = Interaction_PlayerClearRubble.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(playerClearRubble.rubble);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/chop-stone", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while (InputManager.Gameplay.GetInteractButtonHeld() && playerClearRubble.state.CURRENT_STATE == StateMachine.State.CustomAction0)
      yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(playerClearRubble.SimpleSpineAnimator_OnSpineEvent);
    if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAction0)
      playerClearRubble.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    System.Action<Rubble> playerActivatingEnd = Interaction_PlayerClearRubble.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(playerClearRubble.rubble);
    playerClearRubble.Activating = false;
  }
}
