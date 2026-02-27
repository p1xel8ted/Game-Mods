// Decompiled with JetBrains decompiler
// Type: Interaction_UpgradeBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private bool Activating;
  private string sString;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public StructureBrain StructureBrain => this.Structure.Brain;

  private void Start()
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

  public override void OnBecomeCurrent()
  {
  }

  public override void OnBecomeNotCurrent()
  {
  }

  private void SetPrefabs()
  {
    int index = -1;
    while (++index < this.BuildingObjects.Count)
      this.BuildingObjects[index].SetActive(index == this.StructureBrain.Data.UpgradeLevel);
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.StructureBrain.Data.UpgradeLevel != 0)
      return;
    this.Activating = true;
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
    PlayerFarming.Instance.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.UpgradeBuilding(state)));
  }

  private void UpgradeBuilding(StateMachine player)
  {
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
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
