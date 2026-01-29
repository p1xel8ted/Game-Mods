// Decompiled with JetBrains decompiler
// Type: Interaction_UpgradeBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_UpgradeBuilding : Interaction
{
  public Structure Structure;
  public List<GameObject> BuildingObjects = new List<GameObject>();
  public Transform PlayerPosition;
  public string LocTermInteraction;
  public bool Activating;
  public string sString;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public StructureBrain StructureBrain => this.Structure.Brain;

  public void Start()
  {
    this.ContinuouslyHold = true;
    this.SetPrefabs();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = LocalizationManager.GetTranslation(this.LocTermInteraction);
  }

  public override void GetLabel()
  {
    this.Label = this.StructureBrain.Data.UpgradeLevel != 0 || this.Activating ? "" : this.sString;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
  }

  public void SetPrefabs()
  {
    int index = -1;
    while (++index < this.BuildingObjects.Count)
      this.BuildingObjects[index].SetActive(index == this.StructureBrain.Data.UpgradeLevel);
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.StructureBrain.Data.UpgradeLevel != 0)
      return;
    this.Activating = true;
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 8f);
    this.playerFarming.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.UpgradeBuilding(state)));
  }

  public void UpgradeBuilding(StateMachine player)
  {
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 6f);
    ++this.StructureBrain.Data.UpgradeLevel;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      MMTransition.ResumePlay();
      this.SetPrefabs();
      if ((UnityEngine.Object) player != (UnityEngine.Object) null)
      {
        player.transform.position = this.PlayerPosition.position;
        player.CURRENT_STATE = StateMachine.State.InActive;
      }
      GameManager.GetInstance().OnConversationEnd();
    }));
  }
}
