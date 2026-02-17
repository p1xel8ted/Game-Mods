// Decompiled with JetBrains decompiler
// Type: FollowerTask_AssistPlayerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class FollowerTask_AssistPlayerBase : FollowerTask
{
  public bool _helpingPlayer;
  public Follower _follower;

  public override FollowerLocation Location => this._brain.Location;

  public override bool BlockTaskChanges => this._helpingPlayer;

  public override bool BlockReactTasks => this._helpingPlayer;

  public virtual float AssistRange => 8f;

  public sealed override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this._helpingPlayer && PlayerFarming.Location != this._brain.Location)
    {
      if (new List<FollowerLocation>(LocationManager.LocationsInState(LocationState.Active)).Count <= 0)
        return;
      this.OnPlayerLocationChange();
    }
    else
      this.AssistPlayerTick(deltaGameTime);
  }

  public abstract void AssistPlayerTick(float deltaGameTime);

  public virtual void OnPlayerLocationChange() => this.End();

  public virtual bool EndIfPlayerIsDistant()
  {
    PlayerFarming instance = PlayerFarming.Instance;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null) || !((UnityEngine.Object) instance != (UnityEngine.Object) null) || (double) Vector3.Distance(instance.transform.position, followerById.transform.position) <= (double) this.AssistRange)
      return false;
    this.End();
    return true;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (!this._helpingPlayer)
      return;
    this._follower = follower;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    Interaction_PlayerBuild.PlayerActivatingStart += new Action<BuildSitePlot>(this.PlayerActivatingBuildSite);
    Interaction_Berries.PlayerActivatingStart += new Action<Interaction_Berries>(this.PlayerActivatingBerries);
    Interaction_Weed.PlayerActivatingStart += new Action<Interaction_Weed>(this.PlayerActivatingWeed);
    Interaction_PlayerClearRubble.PlayerActivatingStart += new Action<Rubble>(this.PlayerActivatingRubble);
    WeightPlateManager.OnPlayerActivatingWeightPlate += new WeightPlateManager.PlayerActivatingWeightPlate(this.PlayerActivatingAssistRitual);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    this._follower = (Follower) null;
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    Interaction_PlayerBuild.PlayerActivatingStart -= new Action<BuildSitePlot>(this.PlayerActivatingBuildSite);
    Interaction_Berries.PlayerActivatingStart -= new Action<Interaction_Berries>(this.PlayerActivatingBerries);
    Interaction_Weed.PlayerActivatingStart -= new Action<Interaction_Weed>(this.PlayerActivatingWeed);
    Interaction_PlayerClearRubble.PlayerActivatingStart -= new Action<Rubble>(this.PlayerActivatingRubble);
    WeightPlateManager.OnPlayerActivatingWeightPlate -= new WeightPlateManager.PlayerActivatingWeightPlate(this.PlayerActivatingAssistRitual);
  }

  public void OnChangeRoom()
  {
    this._follower.ClearPath();
    this._follower.transform.position = PlayerFarming.Instance.transform.position;
    this.End();
  }

  public void PlayerActivatingBuildSite(BuildSitePlot buildSite)
  {
    this._brain.TransitionToTask((FollowerTask) new FollowerTask_Build(buildSite));
  }

  public void PlayerActivatingBerries(Interaction_Berries berries)
  {
    if (this._brain.CurrentTaskType == FollowerTaskType.Forage)
      return;
    this._brain.TransitionToTask((FollowerTask) new FollowerTask_Forage());
  }

  public void PlayerActivatingTree(Interaction_Woodcutting tree)
  {
    if (this._brain.CurrentTaskType == FollowerTaskType.ChopTrees)
      return;
    this._brain.TransitionToTask((FollowerTask) new FollowerTask_ChopTrees());
  }

  public void PlayerActivatingWeed(Interaction_Weed weed)
  {
    if (this._brain.CurrentTaskType == FollowerTaskType.ClearWeeds)
      return;
    this._brain.TransitionToTask((FollowerTask) new FollowerTask_ClearWeeds(weed));
  }

  public void PlayerActivatingRubble(Rubble rubble)
  {
    if (this._brain.CurrentTaskType == FollowerTaskType.ClearRubble)
      return;
    this._brain.TransitionToTask((FollowerTask) new FollowerTask_ClearRubble(rubble));
  }

  public void PlayerActivatingAssistRitual(List<WeightPlate> WeightPlates)
  {
    if (this._brain.CurrentTaskType == FollowerTaskType.AssistRitual)
      return;
    WeightPlate WeightPlate = (WeightPlate) null;
    float num1 = float.MaxValue;
    foreach (WeightPlate weightPlate in WeightPlates)
    {
      if (!weightPlate.Reserved)
      {
        float num2 = Vector3.Distance(weightPlate.transform.position, this._follower.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          WeightPlate = weightPlate;
        }
      }
    }
    if (!((UnityEngine.Object) WeightPlate != (UnityEngine.Object) null))
      return;
    WeightPlate.Reserved = true;
    this._brain.TransitionToTask((FollowerTask) new FollowerTask_AssistRitual(WeightPlate));
  }
}
