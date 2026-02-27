// Decompiled with JetBrains decompiler
// Type: Worshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (WorshipperInfoManager))]
public class Worshipper : TaskDoer
{
  public static List<Worshipper> worshippers = new List<Worshipper>();
  public SkeletonAnimation Spine;
  private GameObject NameOnHUD;
  private SimpleInventory inventory;
  public Interaction_AwaitRecruit interaction_AwaitRecruit;
  public ParticleSystem NewJobParticles;
  private Task WorshipLeaderTask;
  public bool GoToAndStopping;
  private Task GoToAndStopTask;
  private System.Action GoToAndCallback;
  private GameObject LookToObject;
  private GameObject GoToAndStopTargetPosition;
  public bool GivenPreachSoul;
  public bool BlessedToday;
  public bool BeenFed;
  private float HungerComplaint;
  public float Effeciency = 1f;
  public WorshipperInfoManager wim;
  [HideInInspector]
  public WorshipperBubble bubble;
  private float FacingAngle;
  private float Delay;
  private float Timer;
  private Vector3 TargetPosition;
  public bool BeingCarried;
  private ThrowWorshipper throwWorshipper;
  private List<string> EmotionalSpectrum = new List<string>()
  {
    "Emotions/emotion-angry",
    "Emotions/emotion-unhappy",
    "Emotions/emotion-nomal",
    "Emotions/emotion-happy"
  };
  private float TimedTimer;
  private System.Action TimedCallBack;
  public SimpleSpineAnimator simpleAnimator;
  private float TIME_END_OF_DAY = 1f;
  private float TIME_MORNING = 0.03f;
  public bool MORNING_ASLEEP = true;
  public bool EATEN_DINNNER;
  public bool TRAPPED;
  private float YellTimer;
  private bool _InRitual;
  private float AssignParticles;

  public float Faith
  {
    get => this.wim.v_i.Faith;
    set
    {
      if ((double) value != (double) this.wim.v_i.Faith)
        UITextPopUp.Create(((double) value > (double) this.Faith ? (object) "+" : (object) "").ToString() + (object) (float) ((double) value - (double) this.Faith), (double) value > (double) this.Faith ? Color.green : Color.red, this.gameObject, new Vector3(0.0f, 2f));
      this.wim.v_i.Faith = value;
    }
  }

  public float FearLove
  {
    get => this.wim.v_i.FearLove;
    set
    {
      if ((double) value != (double) this.wim.v_i.FearLove)
        UITextPopUp.Create(((double) value > (double) this.FearLove ? (object) "+" : (object) "").ToString() + (object) (float) ((double) value - (double) this.FearLove), (double) value > (double) this.FearLove ? Color.green : Color.red, this.gameObject, new Vector3(0.0f, 2f));
      this.wim.v_i.FearLove = Mathf.Clamp(value, 0.0f, 100f);
    }
  }

  private void OnDissentor(Villager_Info.StatusState State)
  {
    switch (State)
    {
      case Villager_Info.StatusState.Off:
        if (this.CurrentTask == null || this.CurrentTask.Type != Task_Type.DISSENTER)
          break;
        this.CurrentTask = (Task) null;
        break;
      case Villager_Info.StatusState.On:
        if (this.CurrentTask != null && this.CurrentTask.Type == Task_Type.DISSENTER)
          break;
        this.CurrentTask = (Task) new Task_Dissenter();
        this.CurrentTask.StartTask((TaskDoer) this, (GameObject) null);
        break;
    }
  }

  private void OnIllness(Villager_Info.StatusState Status)
  {
    switch (Status)
    {
      case Villager_Info.StatusState.Off:
        if (this.CurrentTask == null || this.CurrentTask.Type != Task_Type.ILL)
          break;
        this.CurrentTask = (Task) null;
        break;
      case Villager_Info.StatusState.On:
        if (this.CurrentTask != null && this.CurrentTask.Type == Task_Type.ILL)
          break;
        this.CurrentTask = (Task) new Task_Ill();
        this.CurrentTask.StartTask((TaskDoer) this, (GameObject) null);
        break;
      case Villager_Info.StatusState.Kill:
        this.DieFromIllness();
        break;
    }
  }

  private void OnStarve(Villager_Info.StatusState Status)
  {
    switch (Status)
    {
      case Villager_Info.StatusState.Off:
        if (this.CurrentTask == null || this.CurrentTask.Type != Task_Type.ILL)
          break;
        this.CurrentTask = (Task) null;
        break;
      case Villager_Info.StatusState.Kill:
        this.TimedAnimation("tantrum-hungry", 3.2f, new System.Action(this.DieFromStarvation));
        break;
    }
  }

  public bool GuaranteedGoodInteractions
  {
    get => this.wim.v_i.GuaranteedGoodInteractionsUntil >= DataManager.Instance.CurrentDayIndex;
  }

  public bool Motivated => this.wim.v_i.MotivatedUntil >= DataManager.Instance.CurrentDayIndex;

  private void DissentorUp() => this.wim.v_i.Dissentor += 10f;

  private void DissentorDown() => this.wim.v_i.Dissentor -= 10f;

  private void IllnessUp() => this.wim.v_i.Illness += 10f;

  private void IllnessDown() => this.wim.v_i.Illness -= 10f;

  private void DebugCurrentTask()
  {
    Debug.Log((object) $"current task: {(object) this.CurrentTask}  {(this.CurrentTask != null ? (object) this.CurrentTask.Type.ToString() : (object) "")}");
  }

  private void DebugCurrentState()
  {
    Debug.Log((object) ("Current state " + (object) this.state.CURRENT_STATE));
  }

  private void DieFromStarvation() => this.Die();

  private void DieFromIllness() => this.Die();

  public void Die()
  {
    this.CurrentTask = (Task) null;
    // ISSUE: variable of the null type
    __Null local = null;
    ((GameObject) local).GetComponent<Structure>();
    DeadWorshipper component = ((GameObject) local).GetComponent<DeadWorshipper>();
    component.StructureInfo.Dir = this.simpleAnimator.Dir;
    component.PlayAnimation = true;
    Worshipper.ClearDwelling(this);
    Worshipper.ClearJob(this);
    this.wim.v_i.Faith = 0.0f;
    this.wim.v_i.Starve = 0.0f;
    this.wim.OnDie();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void StartWorship(GameObject TargetObject)
  {
    if (this.CurrentTask != null && this.CurrentTask.Type == Task_Type.NONE)
      this.CurrentTask.ClearTask();
    this.WorshipLeaderTask = (Task) new Task_WorshipLeader();
    this.WorshipLeaderTask.StartTask((TaskDoer) this, TargetObject);
  }

  public void EndWorship()
  {
    this.WorshipLeaderTask.ClearTask();
    this.WorshipLeaderTask = (Task) null;
  }

  public override void Update()
  {
    this.wim.v_i.IncreaseDevotion(1f);
    if (!this.InConversation)
    {
      if (this.CurrentTask == null || this.CurrentTask.Type != Task_Type.IMPRISONED)
      {
        if (this.state.CURRENT_STATE != StateMachine.State.Sleeping)
          this.Sleep -= Time.deltaTime;
        if (!this.wim.v_i.Fasting)
        {
          this.wim.v_i.DecreaseHunger(1f);
          if ((double) this.Hunger <= -60.0)
            this.wim.v_i.Starve += Time.deltaTime;
        }
      }
      if (this.CurrentTask != null && this.CurrentTask.Type == Task_Type.IMPRISONED)
        this.wim.v_i.Dissentor -= Time.deltaTime;
    }
    if ((double) this.AssignParticles > 0.0 && (double) (this.AssignParticles -= Time.deltaTime) <= 0.0)
      this.NewJobParticles.Stop();
    if (this.GoToAndStopping)
    {
      this.GoToAndStopTask.TaskUpdate();
      base.Update();
    }
    else if (this.InRitual && this.state.CURRENT_STATE != StateMachine.State.TimedAction)
      base.Update();
    else if (this.WorshipLeaderTask != null)
    {
      this.WorshipLeaderTask.TaskUpdate();
      base.Update();
    }
    else
    {
      if (this.state.CURRENT_STATE == StateMachine.State.TimedAction)
      {
        this.TimedTimer -= Time.deltaTime;
        if ((double) this.TimedTimer < 0.0)
        {
          if (this.TimedCallBack != null)
          {
            this.TimedCallBack();
            return;
          }
        }
        else
        {
          base.Update();
          return;
        }
      }
      if (this.state.CURRENT_STATE == StateMachine.State.AwaitRecruit)
      {
        this.speed = 0.0f;
        this.moveVX = 0.0f;
        this.moveVY = 0.0f;
        this.vx = 0.0f;
        this.vy = 0.0f;
      }
      else if (this.state.CURRENT_STATE == StateMachine.State.SpawnIn)
      {
        double num = (double) (this.state.Timer += Time.deltaTime);
      }
      else if (this.state.CURRENT_STATE == StateMachine.State.SpawnOut)
      {
        if ((double) (this.state.Timer += Time.deltaTime) <= 2.0)
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else
      {
        if (this.state.CURRENT_STATE == StateMachine.State.BeingCarried || this.state.CURRENT_STATE == StateMachine.State.PickedUp || this.state.CURRENT_STATE == StateMachine.State.SacrificeRecruit || this.state.CURRENT_STATE == StateMachine.State.Recruited)
          return;
        if (this.TRAPPED && (double) (this.YellTimer += Time.deltaTime) > 12.0)
        {
          this.YellTimer = 0.0f;
          this.bubble.Play(WorshipperBubble.SPEECH_TYPE.HELP);
        }
        if (this.CurrentTask == null)
        {
          this.DoWorkOrWonder();
        }
        else
        {
          if ((double) this.Sleep <= 0.0 && !this.InConversation && this.CurrentTask.Type == Task_Type.NONE)
            this.SleepAtDwellingOrWonder();
          else if ((double) this.Hunger <= 0.0 && !this.InConversation && this.CurrentTask.Type == Task_Type.NONE && !this.wim.v_i.Fasting)
          {
            if (!this.wim.v_i.Complaint_Food)
            {
              this.bubble.Play(WorshipperBubble.SPEECH_TYPE.FOOD);
              this.TimedAnimation("tantrum-hungry", 3.2f, new System.Action(this.BackToIdle));
              this.wim.v_i.Complaint_Food = true;
            }
            else if ((double) (this.HungerComplaint += Time.deltaTime) > 600.0)
            {
              this.bubble.Play(WorshipperBubble.SPEECH_TYPE.FOOD);
              this.TimedAnimation("tantrum-hungry", 3.2f, new System.Action(this.BackToIdle));
            }
          }
          this.CurrentTask.TaskUpdate();
        }
        base.Update();
      }
    }
  }

  public float Sleep
  {
    set => this.wim.v_i.Sleep = Mathf.Max(0.0f, value);
    get => this.wim.v_i.Sleep;
  }

  public float Hunger
  {
    set => this.wim.v_i.Hunger = Mathf.Clamp(value, -100f, 100f);
    get => this.wim.v_i.Hunger;
  }

  public void GoToAndStop(
    GameObject TargetPosition,
    System.Action GoToAndCallback,
    GameObject LookToObject,
    bool ClearCurrentTaskAfterGoToAndStop)
  {
    if (this.GoToAndStopTask != null)
      this.GoToAndStopTask.ClearTask();
    this.ClearPaths();
    this.GoToAndStopTargetPosition = TargetPosition;
    this.GoToAndStopTask = (Task) new Task_GoToAndStop();
    this.GoToAndStopTask.StartTask((TaskDoer) this, TargetPosition);
    ((Task_GoToAndStop) this.GoToAndStopTask).ClearCurrentTaskAfterGoToAndStop = ClearCurrentTaskAfterGoToAndStop;
    this.GoToAndStopping = true;
    this.GoToAndCallback = GoToAndCallback;
    this.LookToObject = LookToObject;
  }

  public void EndGoToAndStop()
  {
    if ((UnityEngine.Object) this.GoToAndStopTargetPosition != (UnityEngine.Object) null)
      this.transform.position = this.GoToAndStopTargetPosition.transform.position;
    if (this.GoToAndStopTask != null)
      this.GoToAndStopTask.ClearTask();
    this.GoToAndStopTask = (Task) null;
    this.GoToAndStopping = false;
    if (this.GoToAndCallback != null)
      this.GoToAndCallback();
    if (!((UnityEngine.Object) this.LookToObject != (UnityEngine.Object) null))
      return;
    this.state.facingAngle = Utils.GetAngle(this.transform.position, this.LookToObject.transform.position);
  }

  public void Pray() => this.state.CURRENT_STATE = StateMachine.State.Worshipping;

  public void BackToIdle() => this.state.CURRENT_STATE = StateMachine.State.Idle;

  public void Inactive() => this.state.CURRENT_STATE = StateMachine.State.InActive;

  public void CrowdWorship() => this.state.CURRENT_STATE = StateMachine.State.CrowdWorship;

  public new virtual Task CurrentTask
  {
    get => this._CurrentTask;
    set
    {
      this.StopAllCoroutines();
      if (this._CurrentTask != value && this._CurrentTask != null)
        this._CurrentTask.ClearTask();
      this._CurrentTask = value;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Worshipper.worshippers.Add(this);
    this.NewJobParticles.Stop();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.CurrentTask = (Task) null;
    this.wim.v_i.OnDissenter -= new Villager_Info.StatusEffectEvent(this.OnDissentor);
    this.wim.v_i.OnIllness -= new Villager_Info.StatusEffectEvent(this.OnIllness);
    this.wim.v_i.OnStarve -= new Villager_Info.StatusEffectEvent(this.OnStarve);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Worshipper.worshippers.Remove(this);
    if (!((UnityEngine.Object) this.NameOnHUD != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.NameOnHUD);
  }

  private void Start()
  {
    this.Delay = UnityEngine.Random.Range(0.0f, 3f);
    this.Spine = this.GetComponentInChildren<SkeletonAnimation>();
    this.inventory = this.GetComponent<SimpleInventory>();
    this.bubble = this.GetComponentInChildren<WorshipperBubble>();
    this.simpleAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
    this.simpleAnimator.AnimationTrack = 1;
    this.Spine.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.SetEmotionAnimation);
    this.interaction_AwaitRecruit.enabled = false;
    this.wim = this.GetComponent<WorshipperInfoManager>();
    this.wim.v_i.OnDissenter += new Villager_Info.StatusEffectEvent(this.OnDissentor);
    this.wim.v_i.OnIllness += new Villager_Info.StatusEffectEvent(this.OnIllness);
    this.wim.v_i.OnStarve += new Villager_Info.StatusEffectEvent(this.OnStarve);
    if ((double) this.wim.v_i.Faith >= 40.0 || (double) this.wim.v_i.Dissentor >= (double) Villager_Info.DissentorThreshold || UnityEngine.Random.Range(0, 3) != 0)
      return;
    this.wim.v_i.Dissentor = 100f;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.health.HP = this.health.totalHP;
    --this.FearLove;
    this.wim.v_i.Dissentor -= 20f;
    this.simpleAnimator.FlashRedTint();
    if ((double) AttackLocation.x > (double) this.transform.position.x)
      this.Spine.AnimationState.SetAnimation(2, "hurt-front", false);
    else
      this.Spine.AnimationState.SetAnimation(2, "hurt-back", false);
    this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.Complete);
    this.wim.v_i.MotivatedUntil = DataManager.Instance.CurrentDayIndex;
    base.OnHit(Attacker, AttackLocation, AttackType);
  }

  private void Complete(TrackEntry trackEntry)
  {
    if (trackEntry.TrackIndex != 2)
      return;
    this.Spine.AnimationState.SetEmptyAnimation(2, 0.1f);
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.Complete);
  }

  public void ShowStats()
  {
  }

  public void HideStats()
  {
  }

  public void PickUp()
  {
    this.inventory.DropItem();
    this.BeingCarried = true;
    this.CurrentTask = (Task) null;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.PickedUp;
    if ((double) this.wim.v_i.FearLove <= (double) Villager_Info.FearThreshold)
      this.SetAnimation("picked-up-hate", true);
    else if ((double) this.wim.v_i.FearLove >= (double) Villager_Info.LoveThreshold)
      this.SetAnimation("picked-up-love", true);
    else
      this.SetAnimation("picked-up", true);
    this.MORNING_ASLEEP = false;
    this.ShowStats();
  }

  public IEnumerator LowerZ()
  {
    Worshipper worshipper = this;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.20000000298023224)
    {
      Vector3 position = worshipper.transform.position with
      {
        z = 0.0f
      };
      worshipper.transform.position = Vector3.Lerp(worshipper.transform.position, position, Timer / 0.1f);
      yield return (object) null;
    }
    Vector3 position1 = worshipper.transform.position with
    {
      z = 0.0f
    };
    worshipper.transform.position = position1;
  }

  public void DropMe()
  {
    this.transform.position = this.transform.position with
    {
      z = 0.0f
    };
    this.TimedAnimation("put-down", 0.3f, new System.Action(this.Dropped));
    this.HideStats();
  }

  private void Dropped()
  {
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.BeingCarried = false;
    this.TRAPPED = false;
  }

  public void RecoverFromThrow()
  {
    this.TimedAnimation("put-down", 0.3f, new System.Action(this.RecoveredFromThrow));
  }

  private void RecoveredFromThrow()
  {
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.BeingCarried = false;
  }

  public void ThrowMe(float Direction)
  {
    this.throwWorshipper = this.GetComponent<ThrowWorshipper>();
    this.throwWorshipper.FacingAngle = Direction;
    this.throwWorshipper.enabled = true;
    this.enabled = false;
    this.Spine.AnimationState.SetAnimation(0, "thrown", true);
    this.TRAPPED = false;
    Worshipper.ClearJob(this);
    this.FearLove -= 3f;
    this.HideStats();
  }

  public void GiveItem(InventoryItem.ITEM_TYPE itemType)
  {
    this.transform.position = this.transform.position with
    {
      z = 0.0f
    };
    this.TimedAnimation("put-down", 0.3f, (System.Action) (() => this.TimedAnimation("cheer", 1f, (System.Action) (() => this.Dropped()))));
    this.HideStats();
  }

  public void CapturedByBigSpider()
  {
    this.Spine.AnimationState.SetAnimation(0, "spider", true);
    this.BeingCarried = false;
    this.TRAPPED = true;
  }

  public void FreeFromHive()
  {
    this.DropMe();
    this.bubble.Play(WorshipperBubble.SPEECH_TYPE.LOVE);
  }

  private void SetEmotionAnimation(TrackEntry trackEntry)
  {
    if (trackEntry.TrackIndex != 1)
      return;
    if (this.wim.v_i.Brainwashed)
      this.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-enlightened", true);
    else if ((double) this.wim.v_i.Dissentor >= (double) Villager_Info.DissentorThreshold)
      this.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-dissenter", true);
    else if ((double) this.wim.v_i.Illness >= (double) Villager_Info.IllnessThreshold)
      this.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-sick", true);
    else
      this.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-normal", true);
  }

  public void TimedAnimation(string Animation, float Timer, System.Action Callback)
  {
    this.state.CURRENT_STATE = StateMachine.State.TimedAction;
    this.Spine.AnimationState.SetAnimation(1, Animation, true);
    this.TimedTimer = Timer;
    this.TimedCallBack = Callback;
  }

  public void SetAnimation(string Animation, bool Loop)
  {
    this.StartCoroutine((IEnumerator) this.SetAnimationAtEndOfFrame(Animation, Loop));
  }

  private IEnumerator SetAnimationAtEndOfFrame(string Animation, bool Loop)
  {
    yield return (object) new WaitForEndOfFrame();
    this.Spine.AnimationState.SetAnimation(1, Animation, Loop);
  }

  public void AddAnimation(string Animation, bool Loop)
  {
    this.StartCoroutine((IEnumerator) this.AddAnimationAtEndOfFrame(Animation, Loop));
  }

  private IEnumerator AddAnimationAtEndOfFrame(string Animation, bool Loop)
  {
    yield return (object) new WaitForEndOfFrame();
    this.Spine.AnimationState.AddAnimation(1, Animation, Loop, 0.0f);
  }

  public void ChangeStateAnimation(StateMachine.State s, string NewAnimation)
  {
    this.simpleAnimator.ChangeStateAnimation(s, NewAnimation);
  }

  public void ResetAnimationsToDefaults() => this.simpleAnimator.ResetAnimationsToDefaults();

  public bool InRitual
  {
    get => this._InRitual;
    set
    {
      if (value)
      {
        if (this.CurrentTask != null)
          this.CurrentTask.ClearTask();
        this.CurrentTask = (Task) null;
        this.ClearPaths();
      }
      this._InRitual = value;
    }
  }

  private void SleepAtDwellingOrWonder()
  {
    this.Sleep = 60f;
    this.bubble.Play(WorshipperBubble.SPEECH_TYPE.HOME);
    this.TimedAnimation("tantrum", 3.2f, new System.Action(this.BackToIdle));
    this.wim.v_i.Complaint_House = true;
  }

  private void DoWorkOrWonder()
  {
    WorkPlace workPlaceById = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    if (this.wim.v_i.WorkPlace != WorkPlace.NO_JOB)
    {
      workPlaceById.BeginJob((TaskDoer) this, this.wim.v_i.WorkPlaceSlot);
      this.CurrentTask = Task.GetTaskByType(workPlaceById.JobType);
      this.CurrentTask.StartTask((TaskDoer) this, (GameObject) null);
    }
    else if ((double) this.wim.v_i.Illness >= (double) Villager_Info.IllnessThreshold)
    {
      if (this.CurrentTask != null && this.CurrentTask.Type == Task_Type.ILL)
        return;
      this.CurrentTask = (Task) new Task_Ill();
      this.CurrentTask.StartTask((TaskDoer) this, (GameObject) null);
    }
    else
    {
      this.CurrentTask = (Task) new Task_IdleWorhipper();
      this.CurrentTask.StartTask((TaskDoer) this, (GameObject) null);
    }
  }

  private void Repath()
  {
    if ((double) (this.Timer += Time.deltaTime) <= 1.0)
      return;
    this.Timer = 0.0f;
    this.givePath(this.TargetPosition);
  }

  public static Worshipper GetAvailableWorshipper()
  {
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (!worshipper.InConversation && worshipper.CurrentTask != null && (worshipper.CurrentTask.Type == Task_Type.NONE || worshipper.CurrentTask.Type == Task_Type.SLEEP))
        return worshipper;
    }
    return (Worshipper) null;
  }

  public static List<Worshipper> GetAllAvailableWorshipper(bool IgnoreConversation = false)
  {
    List<Worshipper> availableWorshipper = new List<Worshipper>();
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (!worshipper.InConversation | IgnoreConversation && worshipper.CurrentTask != null && (worshipper.CurrentTask.Type == Task_Type.NONE || worshipper.CurrentTask.Type == Task_Type.SLEEP))
        availableWorshipper.Add(worshipper);
    }
    return availableWorshipper;
  }

  public void AssignJob(string WorkPlaceID, int WorkPlaceSlot)
  {
    if (this.wim.v_i.WorkPlace != WorkPlace.NO_JOB)
      Worshipper.ClearJob(this.wim.v_i.WorkPlace, this.wim.v_i.WorkPlaceSlot);
    this.wim.v_i.WorkPlace = WorkPlaceID;
    this.wim.v_i.WorkPlaceSlot = WorkPlaceSlot;
    this.MORNING_ASLEEP = false;
    this.PlayNewAssignParticles();
  }

  private void PlayNewAssignParticles()
  {
    this.NewJobParticles.Play();
    this.AssignParticles = 1f;
  }

  public static void ClearJob(string workplace, int workplaceslot)
  {
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (worshipper.wim.v_i.WorkPlace == workplace && worshipper.wim.v_i.WorkPlaceSlot == workplaceslot)
      {
        worshipper.wim.v_i.WorkPlace = WorkPlace.NO_JOB;
        worshipper.state.CURRENT_STATE = StateMachine.State.Idle;
        worshipper.Delay = 0.0f;
        worshipper.CurrentTask = (Task) null;
      }
    }
  }

  public static void ClearJob(Worshipper w)
  {
    w.wim.v_i.WorkPlace = WorkPlace.NO_JOB;
    w.state.CURRENT_STATE = StateMachine.State.Idle;
    w.Delay = 0.0f;
    w.CurrentTask = (Task) null;
  }

  public void AssignDwelling(Dwelling dwelling, int dwellingslot)
  {
    Worshipper.ClearDwelling(this);
    this.wim.v_i.DwellingSlot = dwellingslot;
    this.wim.v_i.DwellingClaimed = false;
    dwelling.SetBedImage(dwellingslot, Dwelling.SlotState.CLAIMED);
    this.MORNING_ASLEEP = true;
    if ((UnityEngine.Object) this.bubble != (UnityEngine.Object) null)
      this.bubble.Play(WorshipperBubble.SPEECH_TYPE.LOVE);
    this.PlayNewAssignParticles();
  }

  public static void ClearDwelling(string dwelling, int dwellingslot)
  {
  }

  public static void ClearDwelling(Worshipper w)
  {
  }

  public static Worshipper GetWorshipperByID(int ID)
  {
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (worshipper.wim.v_i.ID == ID)
        return worshipper;
    }
    return (Worshipper) null;
  }

  public static Worshipper GetWorshipperByInfo(Villager_Info v_i)
  {
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (worshipper.wim.v_i == v_i)
        return worshipper;
    }
    return (Worshipper) null;
  }

  public static Worshipper GetWorshipperByJobID(string ID)
  {
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (worshipper.wim.v_i != null && worshipper.wim.v_i.WorkPlace == ID)
        return worshipper;
    }
    return (Worshipper) null;
  }

  public static Worshipper GetWorshipperByDwellingID(string ID)
  {
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if (worshipper.wim.v_i != null && worshipper.wim.v_i.Dwelling == ID)
        return worshipper;
    }
    return (Worshipper) null;
  }
}
