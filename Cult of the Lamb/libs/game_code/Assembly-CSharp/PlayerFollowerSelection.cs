// Decompiled with JetBrains decompiler
// Type: PlayerFollowerSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerFollowerSelection : BaseMonoBehaviour
{
  public SpriteRenderer Image;
  public StateMachine State;
  public PlayerFarming playerFarming;
  public float Distance = 2f;
  public static bool IsPlaying;
  public static PlayerFollowerSelection Instance;
  public List<Follower> SelectedFollowers = new List<Follower>();
  public UICommandFollowerWheel UICommandFollowerWheel;
  public Coroutine cScaleCirlce;

  public void OnEnable() => PlayerFollowerSelection.Instance = this;

  public void OnDisable()
  {
    if (!((UnityEngine.Object) PlayerFollowerSelection.Instance == (UnityEngine.Object) this))
      return;
    PlayerFollowerSelection.Instance = (PlayerFollowerSelection) null;
  }

  public void Start()
  {
    this.Image.transform.localScale = Vector3.zero;
    PlayerFollowerSelection.IsPlaying = false;
  }

  public IEnumerator IdleRoutine()
  {
    PlayerFollowerSelection followerSelection = this;
    while (true)
    {
      while ((double) Time.timeScale <= 0.0)
        yield return (object) null;
      while (followerSelection.playerFarming.GoToAndStopping)
        yield return (object) null;
      switch (followerSelection.State.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
          if (!InputManager.UI.GetPageNavigateLeftDown())
            break;
          goto label_7;
      }
      yield return (object) null;
    }
label_7:
    followerSelection.StartCoroutine((IEnumerator) followerSelection.SelectRoutine());
  }

  public IEnumerator SelectRoutine()
  {
    PlayerFollowerSelection followerSelection = this;
    PlayerFollowerSelection.IsPlaying = true;
    if (followerSelection.cScaleCirlce != null)
      followerSelection.StopCoroutine(followerSelection.cScaleCirlce);
    followerSelection.cScaleCirlce = followerSelection.StartCoroutine((IEnumerator) followerSelection.ScaleCirlce(0.0f, followerSelection.Distance, 0.3f));
    yield return (object) new WaitForSeconds(0.3f);
    while (InputManager.UI.GetPageNavigateLeftHeld() && !followerSelection.playerFarming.GoToAndStopping && !LetterBox.IsPlaying)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (!follower.Brain.FollowingPlayer && (double) Vector3.Distance(followerSelection.transform.position, follower.transform.position) < (double) followerSelection.Distance)
        {
          follower.Brain.FollowingPlayer = true;
          follower.Brain.CompleteCurrentTask();
          follower.PlayParticles();
          if (followerSelection.SelectedFollowers.Count <= 0)
            followerSelection.CreateUI();
          followerSelection.UICommandFollowerWheel.AddPortrait(follower.Brain.Info);
          followerSelection.SelectedFollowers.Add(follower);
        }
      }
      yield return (object) null;
    }
    if (followerSelection.cScaleCirlce != null)
      followerSelection.StopCoroutine(followerSelection.cScaleCirlce);
    followerSelection.cScaleCirlce = followerSelection.StartCoroutine((IEnumerator) followerSelection.ScaleCirlce(followerSelection.Image.transform.localScale.x, 0.0f, 0.3f));
    if (followerSelection.SelectedFollowers.Count <= 0)
    {
      PlayerFollowerSelection.IsPlaying = false;
      followerSelection.StartCoroutine((IEnumerator) followerSelection.IdleRoutine());
    }
    else
    {
      followerSelection.State.CURRENT_STATE = StateMachine.State.Map;
      GameManager.GetInstance().CameraSetTargetZoom(15f);
    }
  }

  public void CreateUI()
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(UnityEngine.Resources.Load("Prefabs/UI/UI Follower Command Wheel") as GameObject);
    float num = Screen.width < 0 || Screen.width > 1080 ? (Screen.width <= 1080 || Screen.width >= 2160 ? (Screen.width < 2160 ? 1f : 2f) : 1f) : 0.5f;
    gameObject.GetComponent<CanvasScaler>().scaleFactor = num;
    this.UICommandFollowerWheel = gameObject.GetComponent<UICommandFollowerWheel>();
    this.UICommandFollowerWheel.CallbackClose = (Action<UICommandFollowerWheel.ActivityChoice.AvailableCommands>) (activityChoice =>
    {
      this.State.CURRENT_STATE = StateMachine.State.Idle;
      GameManager.GetInstance().CameraResetTargetZoom();
      foreach (Follower selectedFollower in this.SelectedFollowers)
      {
        selectedFollower.Brain.FollowingPlayer = false;
        selectedFollower.Brain.Stats.WorkerBeenGivenOrders = true;
        selectedFollower.PlayParticles();
        switch (activityChoice)
        {
          case UICommandFollowerWheel.ActivityChoice.AvailableCommands.ChopTrees:
            if (selectedFollower.Brain.CurrentTaskType != FollowerTaskType.ChopTrees)
            {
              selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChopTrees());
              continue;
            }
            continue;
          case UICommandFollowerWheel.ActivityChoice.AvailableCommands.ClearWeeds:
            Interaction_Weed weed1 = (Interaction_Weed) null;
            float maxValue1 = float.MaxValue;
            foreach (Interaction_Weed weed2 in Interaction_Weed.Weeds)
            {
              if ((double) Vector3.Distance(weed2.transform.position, this.transform.position) < (double) maxValue1)
                weed1 = weed2;
            }
            if (selectedFollower.Brain.CurrentTaskType != FollowerTaskType.ClearWeeds)
            {
              selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ClearWeeds(weed1));
              continue;
            }
            continue;
          case UICommandFollowerWheel.ActivityChoice.AvailableCommands.ClearRubble:
            Rubble rubble1 = (Rubble) null;
            float maxValue2 = float.MaxValue;
            foreach (Rubble rubble2 in Rubble.Rubbles)
            {
              if ((double) Vector3.Distance(rubble2.transform.position, this.transform.position) < (double) maxValue2)
                rubble1 = rubble2;
            }
            if (selectedFollower.Brain.CurrentTaskType != FollowerTaskType.ClearRubble)
            {
              selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ClearRubble(rubble1));
              continue;
            }
            continue;
          default:
            selectedFollower.Brain.CompleteCurrentTask();
            continue;
        }
      }
      PlayerFollowerSelection.IsPlaying = false;
      this.SelectedFollowers.Clear();
      this.StartCoroutine((IEnumerator) this.IdleRoutine());
    });
  }

  public void Update()
  {
    this.Image.transform.Rotate(new Vector3(0.0f, 0.0f, 10f) * Time.deltaTime);
  }

  public IEnumerator ScaleCirlce(float Start, float Target, float Duration)
  {
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.Image.transform.localScale = Vector3.one * Mathf.SmoothStep(Start, Target, Progress / Duration);
      yield return (object) null;
    }
    this.Image.transform.localScale = Vector3.one * Target;
  }

  [CompilerGenerated]
  public void \u003CCreateUI\u003Eb__13_0(
    UICommandFollowerWheel.ActivityChoice.AvailableCommands activityChoice)
  {
    this.State.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().CameraResetTargetZoom();
    foreach (Follower selectedFollower in this.SelectedFollowers)
    {
      selectedFollower.Brain.FollowingPlayer = false;
      selectedFollower.Brain.Stats.WorkerBeenGivenOrders = true;
      selectedFollower.PlayParticles();
      switch (activityChoice)
      {
        case UICommandFollowerWheel.ActivityChoice.AvailableCommands.ChopTrees:
          if (selectedFollower.Brain.CurrentTaskType != FollowerTaskType.ChopTrees)
          {
            selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChopTrees());
            continue;
          }
          continue;
        case UICommandFollowerWheel.ActivityChoice.AvailableCommands.ClearWeeds:
          Interaction_Weed weed1 = (Interaction_Weed) null;
          float maxValue1 = float.MaxValue;
          foreach (Interaction_Weed weed2 in Interaction_Weed.Weeds)
          {
            if ((double) Vector3.Distance(weed2.transform.position, this.transform.position) < (double) maxValue1)
              weed1 = weed2;
          }
          if (selectedFollower.Brain.CurrentTaskType != FollowerTaskType.ClearWeeds)
          {
            selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ClearWeeds(weed1));
            continue;
          }
          continue;
        case UICommandFollowerWheel.ActivityChoice.AvailableCommands.ClearRubble:
          Rubble rubble1 = (Rubble) null;
          float maxValue2 = float.MaxValue;
          foreach (Rubble rubble2 in Rubble.Rubbles)
          {
            if ((double) Vector3.Distance(rubble2.transform.position, this.transform.position) < (double) maxValue2)
              rubble1 = rubble2;
          }
          if (selectedFollower.Brain.CurrentTaskType != FollowerTaskType.ClearRubble)
          {
            selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ClearRubble(rubble1));
            continue;
          }
          continue;
        default:
          selectedFollower.Brain.CompleteCurrentTask();
          continue;
      }
    }
    PlayerFollowerSelection.IsPlaying = false;
    this.SelectedFollowers.Clear();
    this.StartCoroutine((IEnumerator) this.IdleRoutine());
  }
}
