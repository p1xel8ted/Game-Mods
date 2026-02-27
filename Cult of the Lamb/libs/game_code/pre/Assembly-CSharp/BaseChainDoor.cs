// Decompiled with JetBrains decompiler
// Type: BaseChainDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class BaseChainDoor : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  private float Distance = 8f;
  public Interaction_BaseDungeonDoor Door1;
  public Interaction_BaseDungeonDoor Door2;
  public Interaction_BaseDungeonDoor Door3;
  public Interaction_BaseDungeonDoor Door4;
  private bool DoorActive;
  public static BaseChainDoor Instance;
  public Vector3 OffsetPosition;
  private bool Used;

  private void OnEnable()
  {
    BaseChainDoor.Instance = this;
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  private IEnumerator WaitForPlayer()
  {
    BaseChainDoor baseChainDoor = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, baseChainDoor.transform.position + baseChainDoor.OffsetPosition) > (double) baseChainDoor.Distance)
      yield return (object) null;
    baseChainDoor.Play();
  }

  private void Start()
  {
    if (DataManager.Instance.BossesCompleted.Count == 0 || DataManager.Instance.DoorRoomChainProgress == 0)
    {
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    }
    else
    {
      this.Spine.AnimationState.SetAnimation(0, "broken" + (object) DataManager.Instance.DoorRoomChainProgress, true);
      this.DoorActive = DataManager.Instance.DoorRoomChainProgress >= 5;
    }
  }

  private void Play() => this.StartCoroutine((IEnumerator) this.PlayRoutine());

  public void PlayDoor1(System.Action CallBack)
  {
    this.StartCoroutine((IEnumerator) this.PlayDoor1Routine(CallBack));
  }

  private IEnumerator PlayDoor1Routine(System.Action CallBack)
  {
    BaseChainDoor baseChainDoor = this;
    DataManager.Instance.DoorRoomChainProgress = -1;
    yield return (object) baseChainDoor.StartCoroutine((IEnumerator) baseChainDoor.PlayRoutine());
    yield return (object) new WaitForSeconds(0.5f);
    System.Action action = CallBack;
    if (action != null)
      action();
  }

  public void BlockAllDoors()
  {
    this.Door1.Block();
    this.Door2.Block();
    this.Door3.Block();
    this.Door4.Block();
  }

  public void UnblockAllDoors()
  {
    this.Door1.Unblock();
    this.Door2.Unblock();
    this.Door3.Unblock();
    this.Door4.Unblock();
  }

  private IEnumerator PlayRoutine()
  {
    BaseChainDoor baseChainDoor = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    bool doChainDoorRoutine = DataManager.Instance.DoorRoomChainProgress < DataManager.Instance.BossesCompleted.Count || DataManager.Instance.DoorRoomChainProgress == 4;
    if (doChainDoorRoutine)
    {
      SimpleSetCamera.DisableAll();
      Debug.Log((object) ("DataManager.Instance.DoorRoomChainProgress " + (object) DataManager.Instance.DoorRoomChainProgress).Colour(Color.red));
      ++DataManager.Instance.DoorRoomChainProgress;
      Debug.Log((object) ("DataManager.Instance.DoorRoomChainProgress " + (object) DataManager.Instance.DoorRoomChainProgress).Colour(Color.red));
      if (DataManager.Instance.BossesCompleted.Count > 0)
      {
        Debug.Log((object) "aaa".Colour(Color.red));
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(baseChainDoor.gameObject, 12f);
        if (DataManager.Instance.DoorRoomChainProgress != 5)
          AudioManager.Instance.PlayOneShot("event:/door/chain_break_sequence");
        else
          AudioManager.Instance.PlayOneShot("event:/door/chain_door_final");
        yield return (object) new WaitForSeconds(0.5f);
        baseChainDoor.Spine.AnimationState.SetAnimation(0, "break" + (object) DataManager.Instance.DoorRoomChainProgress, false);
        baseChainDoor.Spine.AnimationState.AddAnimation(0, "broken" + (object) DataManager.Instance.DoorRoomChainProgress, true, 0.0f);
        if (DataManager.Instance.DoorRoomChainProgress == 5)
          yield return (object) new WaitForSeconds(5.4666667f);
        else
          yield return (object) new WaitForSeconds(1.5f);
        Debug.Log((object) "finished chain!".Colour(Color.red));
      }
      yield return (object) new WaitForSeconds(0.5f);
      if (DataManager.Instance.DoorRoomChainProgress == 4)
      {
        baseChainDoor.Play();
        yield break;
      }
    }
    Debug.Log((object) "A");
    Interaction_BaseDungeonDoor TargetDoor = (Interaction_BaseDungeonDoor) null;
    if (DataManager.Instance.DoorRoomChainProgress < 5 && DataManager.Instance.DoorRoomDoorsProgress != DataManager.Instance.BossesCompleted.Count)
    {
      if (DataManager.Instance.BossesCompleted.Count <= 0 && !baseChainDoor.Door1.Unlocked)
        TargetDoor = baseChainDoor.Door1;
      else if (DataManager.Instance.BossesCompleted.Count >= 1 && baseChainDoor.Door2.GetFollowerCount() && !baseChainDoor.Door2.Unlocked)
        TargetDoor = baseChainDoor.Door2;
      else if (DataManager.Instance.BossesCompleted.Count >= 2 && baseChainDoor.Door3.GetFollowerCount() && !baseChainDoor.Door3.Unlocked)
        TargetDoor = baseChainDoor.Door3;
      else if (DataManager.Instance.BossesCompleted.Count >= 3 && baseChainDoor.Door4.GetFollowerCount() && !baseChainDoor.Door4.Unlocked)
        TargetDoor = baseChainDoor.Door4;
    }
    if (DataManager.Instance.DoorRoomChainProgress >= 5)
      baseChainDoor.DoorActive = true;
    if ((UnityEngine.Object) TargetDoor != (UnityEngine.Object) null)
    {
      DataManager.Instance.DoorRoomDoorsProgress = DataManager.Instance.BossesCompleted.Count;
      if ((UnityEngine.Object) TargetDoor == (UnityEngine.Object) baseChainDoor.Door1)
        TargetDoor.DoorLights.SetActive(false);
      GameManager.GetInstance().OnConversationNew();
      TargetDoor.SimpleSetCamera.Play();
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/door/cross_disappear");
      yield return (object) new WaitForSeconds(0.5f);
      if ((UnityEngine.Object) TargetDoor == (UnityEngine.Object) baseChainDoor.Door1)
        TargetDoor.FadeDoorLight();
      yield return (object) new WaitForSeconds(2f);
      GameManager.GetInstance().OnConversationEnd();
      TargetDoor.SimpleSetCamera.Reset();
      SimpleSetCamera.EnableAll();
    }
    else if (doChainDoorRoutine)
    {
      GameManager.GetInstance().OnConversationEnd();
      SimpleSetCamera.EnableAll();
    }
    if (doChainDoorRoutine)
    {
      yield return (object) new WaitForSeconds(0.5f);
      if (DataManager.Instance.DoorRoomChainProgress < DataManager.Instance.BossesCompleted.Count)
        baseChainDoor.Play();
    }
  }

  private void Test(FollowerLocation Location)
  {
    DataManager.Instance.BossesCompleted.Add(Location);
    this.Play();
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.OffsetPosition, this.Distance, Color.yellow);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.DoorActive || !(collision.gameObject.tag == "Player") || this.Used)
      return;
    this.Used = true;
    GameManager.NewRun("", false);
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon Final", 1f, "", new System.Action(this.FadeSave));
  }

  private void FadeSave() => SaveAndLoad.Save();
}
