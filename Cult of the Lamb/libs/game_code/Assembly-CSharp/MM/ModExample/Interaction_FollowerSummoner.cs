// Decompiled with JetBrains decompiler
// Type: MM.ModExample.Interaction_FollowerSummoner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace MM.ModExample;

public class Interaction_FollowerSummoner : Interaction
{
  public override void GetLabel()
  {
    if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0 || DataManager.Instance.Followers.Count <= 0)
      this.Label = "";
    else
      this.Label = ScriptLocalization.Interactions.SummonDemon;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    state.CURRENT_STATE = StateMachine.State.InActive;
    Time.timeScale = 0.0f;
    SimulationManager.Pause();
    this.SelectFollower();
  }

  public void SelectFollower()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!DataManager.Instance.Followers_Recruit.Contains(follower.Brain._directInfoAccess))
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
    }
    UIFollowerSelectMenuController selectMenuController = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    selectMenuController.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    selectMenuController.OnFollowerSelected = selectMenuController.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      {
        GameManager.GetInstance().OnConversationEnd();
        SimulationManager.UnPause();
        Time.timeScale = 1f;
      }
      else
        this.StartCoroutine((IEnumerator) this.SummonFollowerIE(followerById));
    });
    selectMenuController.OnCancel = selectMenuController.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      SimulationManager.UnPause();
      Time.timeScale = 1f;
    });
  }

  public IEnumerator SummonFollowerIE(Follower follower)
  {
    Interaction_FollowerSummoner followerSummoner = this;
    Time.timeScale = 1f;
    GameManager.GetInstance().OnConversationNew();
    follower.HideAllFollowerIcons();
    yield return (object) new WaitForSeconds(0.5f);
    follower.Brain.CurrentTask?.Abort();
    follower.ClearPath();
    follower.transform.position = followerSummoner.transform.position;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("summon-demon-reverse", false);
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    yield return (object) new WaitForSeconds(1.5f);
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.UnPause();
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CSelectFollower\u003Eb__2_0(FollowerInfo followerInfo)
  {
    Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
    if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
    {
      GameManager.GetInstance().OnConversationEnd();
      SimulationManager.UnPause();
      Time.timeScale = 1f;
    }
    else
      this.StartCoroutine((IEnumerator) this.SummonFollowerIE(followerById));
  }
}
