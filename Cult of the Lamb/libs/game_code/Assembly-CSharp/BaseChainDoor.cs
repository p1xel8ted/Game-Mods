// Decompiled with JetBrains decompiler
// Type: BaseChainDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class BaseChainDoor : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  public float Distance = 8f;
  public Interaction_BaseDungeonDoor Door1;
  public Interaction_BaseDungeonDoor Door2;
  public Interaction_BaseDungeonDoor Door3;
  public Interaction_BaseDungeonDoor Door4;
  public bool DoorActive;
  public static BaseChainDoor Instance;
  public Vector3 OffsetPosition;
  public bool Used;

  public void OnEnable()
  {
    BaseChainDoor.Instance = this;
    DataManager.Instance.DoorRoomChainProgress = Mathf.Clamp(DataManager.Instance.DoorRoomChainProgress, 0, 5);
    this.StartCoroutine(this.WaitForPlayer());
  }

  public IEnumerator WaitForPlayer()
  {
    BaseChainDoor baseChainDoor = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if (DataManager.Instance.BossesCompleted.Count == 0 || DataManager.Instance.DoorRoomChainProgress == 0)
    {
      baseChainDoor.Spine.AnimationState.SetAnimation(0, "closed", true);
    }
    else
    {
      baseChainDoor.Spine.AnimationState.SetAnimation(0, "broken" + DataManager.Instance.DoorRoomChainProgress.ToString(), true);
      baseChainDoor.DoorActive = DataManager.Instance.DoorRoomChainProgress >= 5;
    }
    bool waiting = true;
    while (waiting)
    {
      foreach (Component player in PlayerFarming.players)
      {
        if ((double) Vector3.Distance(player.transform.position, baseChainDoor.transform.position + baseChainDoor.OffsetPosition) < (double) baseChainDoor.Distance)
          waiting = false;
      }
      yield return (object) null;
    }
    baseChainDoor.Play();
  }

  public void Play() => this.StartCoroutine(this.PlayRoutine());

  public void PlayDoor1(System.Action CallBack)
  {
    this.StartCoroutine(this.PlayDoor1Routine(CallBack));
  }

  public IEnumerator PlayDoor1Routine(System.Action CallBack)
  {
    BaseChainDoor baseChainDoor = this;
    DataManager.Instance.DoorRoomChainProgress = -1;
    yield return (object) baseChainDoor.StartCoroutine(baseChainDoor.PlayRoutine());
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

  public IEnumerator PlayRoutine()
  {
    BaseChainDoor baseChainDoor = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    int bossesCompleted = 0;
    foreach (FollowerLocation followerLocation in DataManager.Instance.BossesCompleted)
    {
      switch (followerLocation)
      {
        case FollowerLocation.Dungeon1_5:
        case FollowerLocation.Dungeon1_6:
        case FollowerLocation.Boss_Yngya:
        case FollowerLocation.Boss_Wolf:
          continue;
        default:
          ++bossesCompleted;
          continue;
      }
    }
    bool doChainDoorRoutine = DataManager.Instance.DoorRoomChainProgress < bossesCompleted || DataManager.Instance.DoorRoomChainProgress == 4;
    if (doChainDoorRoutine)
    {
      SimpleSetCamera.DisableAll();
      Debug.Log((object) ("DataManager.Instance.DoorRoomChainProgress " + DataManager.Instance.DoorRoomChainProgress.ToString()).Colour(Color.red));
      ++DataManager.Instance.DoorRoomChainProgress;
      Debug.Log((object) ("DataManager.Instance.DoorRoomChainProgress " + DataManager.Instance.DoorRoomChainProgress.ToString()).Colour(Color.red));
      if (bossesCompleted > 0)
      {
        Debug.Log((object) "aaa".Colour(Color.red));
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(baseChainDoor.gameObject, 12f);
        if (DataManager.Instance.DoorRoomChainProgress != 5)
          AudioManager.Instance.PlayOneShot("event:/door/chain_break_sequence");
        else
          AudioManager.Instance.PlayOneShot("event:/door/chain_door_final");
        yield return (object) new WaitForSeconds(0.5f);
        baseChainDoor.Spine.AnimationState.SetAnimation(0, "break" + DataManager.Instance.DoorRoomChainProgress.ToString(), false);
        baseChainDoor.Spine.AnimationState.AddAnimation(0, "broken" + DataManager.Instance.DoorRoomChainProgress.ToString(), true, 0.0f);
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
    if (DataManager.Instance.DoorRoomChainProgress < 5 && DataManager.Instance.DoorRoomDoorsProgress != bossesCompleted)
    {
      if (bossesCompleted <= 0 && !baseChainDoor.Door1.Unlocked)
        TargetDoor = baseChainDoor.Door1;
      else if (bossesCompleted >= 1 && baseChainDoor.Door2.GetFollowerCount() && !baseChainDoor.Door2.Unlocked)
        TargetDoor = baseChainDoor.Door2;
      else if (bossesCompleted >= 2 && baseChainDoor.Door3.GetFollowerCount() && !baseChainDoor.Door3.Unlocked)
        TargetDoor = baseChainDoor.Door3;
      else if (bossesCompleted >= 3 && baseChainDoor.Door4.GetFollowerCount() && !baseChainDoor.Door4.Unlocked)
        TargetDoor = baseChainDoor.Door4;
    }
    if (DataManager.Instance.DoorRoomChainProgress >= 5)
      baseChainDoor.DoorActive = true;
    if ((UnityEngine.Object) TargetDoor != (UnityEngine.Object) null)
    {
      DataManager.Instance.DoorRoomDoorsProgress = bossesCompleted;
      TargetDoor.DoorLights.SetActive(false);
      GameManager.GetInstance().OnConversationNew();
      TargetDoor.SimpleSetCamera.Play();
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/door/cross_disappear");
      yield return (object) new WaitForSeconds(0.5f);
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
      if (DataManager.Instance.DoorRoomChainProgress < bossesCompleted)
        baseChainDoor.Play();
    }
  }

  public void Test(FollowerLocation Location)
  {
    DataManager.Instance.BossesCompleted.Add(Location);
    this.Play();
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.OffsetPosition, this.Distance, Color.yellow);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.DoorActive || !collision.gameObject.CompareTag("Player") || this.Used)
      return;
    this.Used = true;
    GameManager.NewRun("", false);
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon Final", 1f, "", new System.Action(this.FadeSave));
  }

  public void FadeSave() => SaveAndLoad.Save();
}
