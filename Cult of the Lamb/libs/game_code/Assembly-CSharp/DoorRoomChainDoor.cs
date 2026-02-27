// Decompiled with JetBrains decompiler
// Type: DoorRoomChainDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class DoorRoomChainDoor : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  public float Distance = 8f;
  public DeleteIfBossCompleted Door1;
  public DeleteIfBossCompleted Door2;
  public DeleteIfBossCompleted Door3;
  public DeleteIfBossCompleted Door4;
  public DeleteIfBossCompleted Door5;
  public bool DoorActive;
  public bool Used;

  public void Start()
  {
    if (DataManager.Instance.BossesCompleted.Count == 0)
    {
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    }
    else
    {
      this.Spine.AnimationState.SetAnimation(0, "broken" + DataManager.Instance.DoorRoomChainProgress.ToString(), true);
      this.DoorActive = DataManager.Instance.DoorRoomChainProgress >= 5;
    }
    if (DataManager.Instance.DoorRoomChainProgress >= DataManager.Instance.BossesCompleted.Count)
      return;
    this.Play();
  }

  public void Play() => this.StartCoroutine(this.PlayRoutine());

  public IEnumerator PlayRoutine()
  {
    DoorRoomChainDoor doorRoomChainDoor = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, doorRoomChainDoor.transform.position) > (double) doorRoomChainDoor.Distance)
      yield return (object) null;
    SimpleSetCamera.DisableAll();
    ++DataManager.Instance.DoorRoomChainProgress;
    if (DataManager.Instance.BossesCompleted.Count > 0)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(doorRoomChainDoor.gameObject, 12f);
      AudioManager.Instance.PlayOneShot("event:/door/chain_break_sequence");
      yield return (object) new WaitForSeconds(0.5f);
      doorRoomChainDoor.Spine.AnimationState.SetAnimation(0, "break" + DataManager.Instance.DoorRoomChainProgress.ToString(), false);
      doorRoomChainDoor.Spine.AnimationState.AddAnimation(0, "broken" + DataManager.Instance.DoorRoomChainProgress.ToString(), true, 0.0f);
      if (DataManager.Instance.DoorRoomChainProgress == 5)
        yield return (object) new WaitForSeconds(5.4666667f);
      else
        yield return (object) new WaitForSeconds(1.5f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    if (DataManager.Instance.DoorRoomChainProgress == 4)
    {
      doorRoomChainDoor.Play();
    }
    else
    {
      DeleteIfBossCompleted TargetDoor = (DeleteIfBossCompleted) null;
      if (DataManager.Instance.DoorRoomChainProgress < 5)
      {
        if (DataManager.Instance.BossesCompleted.Count <= 0)
        {
          TargetDoor = doorRoomChainDoor.Door1;
        }
        else
        {
          switch (DataManager.Instance.BossesCompleted[DataManager.Instance.BossesCompleted.Count - 1])
          {
            case FollowerLocation.Dungeon1_1:
              TargetDoor = doorRoomChainDoor.Door2;
              break;
            case FollowerLocation.Dungeon1_2:
              TargetDoor = doorRoomChainDoor.Door3;
              break;
            case FollowerLocation.Dungeon1_3:
              TargetDoor = doorRoomChainDoor.Door4;
              break;
            case FollowerLocation.Dungeon1_4:
              TargetDoor = doorRoomChainDoor.Door5;
              break;
          }
        }
      }
      else
        doorRoomChainDoor.DoorActive = true;
      if ((UnityEngine.Object) TargetDoor != (UnityEngine.Object) null)
      {
        GameManager.GetInstance().OnConversationNext(TargetDoor.CameraPosition, 8f);
        yield return (object) new WaitForSeconds(1f);
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        UnityEngine.Object.Destroy((UnityEngine.Object) TargetDoor.gameObject);
        AudioManager.Instance.PlayOneShot("event:/door/cross_disappear");
        if (!DataManager.Instance.DoorRoomBossLocksDestroyed.Contains(TargetDoor.Location))
          DataManager.Instance.DoorRoomBossLocksDestroyed.Add(TargetDoor.Location);
        yield return (object) new WaitForSeconds(1f);
        GameManager.GetInstance().OnConversationEnd();
        SimpleSetCamera.EnableAll();
      }
      else
      {
        GameManager.GetInstance().OnConversationEnd();
        SimpleSetCamera.EnableAll();
      }
      yield return (object) new WaitForSeconds(0.5f);
      if (DataManager.Instance.DoorRoomChainProgress < DataManager.Instance.BossesCompleted.Count)
        doorRoomChainDoor.Play();
    }
  }

  public void Test(FollowerLocation Location)
  {
    DataManager.Instance.BossesCompleted.Add(Location);
    this.Play();
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.Distance, Color.yellow);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.DoorActive || !collision.gameObject.CompareTag("Player") || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    if (DataManager.Instance.DeathCatBeaten)
      return;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon Final", 1f, "", (System.Action) null);
  }
}
