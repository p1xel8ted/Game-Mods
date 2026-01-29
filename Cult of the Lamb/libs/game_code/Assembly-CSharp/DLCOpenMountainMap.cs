// Decompiled with JetBrains decompiler
// Type: DLCOpenMountainMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DLCOpenMountainMap : EnterBuilding
{
  public Vector3 MovePlayerOnCancel = Vector3.down;
  public float FacePlayerOnCancel = 270f;
  public Transform GotoPoint;
  public Transform ReturnPoint;
  public bool triggered;

  public override void OnTriggerEnter2D(Collider2D collision)
  {
    this.playerFarming = collision.GetComponent<PlayerFarming>();
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && (PlayerFarming.IsAnyPlayerCarryingBody() || PlayerFarming.IsAnyPlayerInInteractionWithRanchable() || PlayerFarming.IsAnyPlayerChargingSnowball() || this.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive) || (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive || (UnityEngine.Object) MonoSingleton<UIManager>.Instance == (UnityEngine.Object) null || MMTransition.IsPlaying || this.triggered)
      return;
    this.triggered = true;
    this.playerFarming.EndGoToAndStop();
    SimulationManager.Pause();
    if (DataManager.Instance.DLCDungeonNodesCompleted.Count >= 49)
    {
      GameManager.NextDungeonLayer(4);
      GameManager.DungeonUseAllLayers = true;
      DataManager.Instance.IsMiniBoss = true;
      FollowerLocation location = (double) UnityEngine.Random.value < 0.5 ? FollowerLocation.Dungeon1_5 : FollowerLocation.Dungeon1_6;
      GameManager.NewRun("", false, location);
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, location == FollowerLocation.Dungeon1_5 ? "Dungeon5" : "Dungeon6", 1f, "", (System.Action) (() => SaveAndLoad.Save()));
    }
    else if (SeasonsManager.Active)
    {
      this.StartCoroutine((IEnumerator) this.OpenMap());
    }
    else
    {
      this.playerFarming.GoToAndStop(this.GotoPoint.position);
      DataManager.Instance.CurrentDLCNodeType = DungeonWorldMapIcon.NodeType.Shrine;
      DataManager.Instance.IsLambGhostRescue = false;
      GameManager.NextDungeonLayer(0);
      GameManager.NewRun("", false, FollowerLocation.Dungeon1_5);
      DataManager.Instance.CurrentLocation = FollowerLocation.Base;
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon5", 1f, "", new System.Action(SaveAndLoad.Save));
    }
  }

  public IEnumerator OpenMap()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DLCOpenMountainMap dlcOpenMountainMap = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets();
    dlcOpenMountainMap.playerFarming.GoToAndStop(dlcOpenMountainMap.GotoPoint.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) UIManager.LoadAssets(task, new System.Action(dlcOpenMountainMap.\u003COpenMap\u003Eb__6_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public void \u003COpenMap\u003Eb__6_0()
  {
    UIDLCMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowDLCMapMenu(this.playerFarming);
    mapMenuController.OnCancel = mapMenuController.OnCancel + (System.Action) (() =>
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.transform.position += this.MovePlayerOnCancel;
        player.state.facingAngle = this.FacePlayerOnCancel;
        Vector3 position = this.ReturnPoint.position with
        {
          x = this.transform.position.x
        };
        if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) player)
          player.GoToAndStop(position, IdleOnEnd: true, GoToCallback: (System.Action) (() => this.triggered = false), AbortGoToCallback: (System.Action) (() => this.triggered = false));
        else
          player.GoToAndStop(position, IdleOnEnd: true);
      }
      LambTownController.Instance.UpdateRevealInteraction();
    });
  }

  [CompilerGenerated]
  public void \u003COpenMap\u003Eb__6_1()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.transform.position += this.MovePlayerOnCancel;
      player.state.facingAngle = this.FacePlayerOnCancel;
      Vector3 position = this.ReturnPoint.position with
      {
        x = this.transform.position.x
      };
      if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) player)
        player.GoToAndStop(position, IdleOnEnd: true, GoToCallback: (System.Action) (() => this.triggered = false), AbortGoToCallback: (System.Action) (() => this.triggered = false));
      else
        player.GoToAndStop(position, IdleOnEnd: true);
    }
    LambTownController.Instance.UpdateRevealInteraction();
  }

  [CompilerGenerated]
  public void \u003COpenMap\u003Eb__6_2() => this.triggered = false;

  [CompilerGenerated]
  public void \u003COpenMap\u003Eb__6_3() => this.triggered = false;
}
