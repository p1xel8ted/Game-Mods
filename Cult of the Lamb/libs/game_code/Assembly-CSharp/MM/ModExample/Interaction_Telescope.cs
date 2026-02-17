// Decompiled with JetBrains decompiler
// Type: MM.ModExample.Interaction_Telescope
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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

public class Interaction_Telescope : Interaction
{
  public override void GetLabel()
  {
    if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0 || DataManager.Instance.Followers.Count <= 0)
      this.Label = "";
    else
      this.Label = ScriptLocalization.Interactions.Look;
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
      followerSelectEntries.Add(new FollowerSelectEntry(follower));
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
        this.StartCoroutine((IEnumerator) this.TrackFollowerIE(followerById));
    });
    selectMenuController.OnCancel = selectMenuController.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      SimulationManager.UnPause();
      Time.timeScale = 1f;
    });
  }

  public IEnumerator TrackFollowerIE(Follower targetFollower)
  {
    Time.timeScale = 1f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(targetFollower.gameObject);
    yield return (object) new WaitForSecondsRealtime(2f);
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.UnPause();
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
      this.StartCoroutine((IEnumerator) this.TrackFollowerIE(followerById));
  }
}
