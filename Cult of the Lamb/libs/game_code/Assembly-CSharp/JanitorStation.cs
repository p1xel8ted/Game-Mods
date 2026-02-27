// Decompiled with JetBrains decompiler
// Type: JanitorStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class JanitorStation : Interaction
{
  public static List<JanitorStation> JanitorStations = new List<JanitorStation>();
  public Structure Structure;
  public Structures_JanitorStation _StructureInfo;
  [SerializeField]
  public ItemGauge gauge;
  public int previousSoulCount;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_JanitorStation StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_JanitorStation;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    JanitorStation.JanitorStations.Add(this);
  }

  public override void Update()
  {
    base.Update();
    if (this.StructureBrain != null && this.previousSoulCount != this.StructureBrain.SoulCount)
    {
      this.gauge.SetPosition((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax);
      this.previousSoulCount = this.StructureBrain.SoulCount;
    }
    this.gauge.gameObject.SetActive(DataManager.Instance.ChoreXPLevel < 9 && DataManager.Instance.ChoreXPLevel_Coop < 9);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    bool flag = PlayerFarming.players.Count > 1 && DataManager.Instance.ChoreXPLevel_Coop < 9;
    if (this.StructureBrain != null && this.StructureBrain.SoulCount > 0 && DataManager.Instance.ChoreXPLevel < 9 | flag)
      this.Label = ScriptLocalization.Interactions.Collect;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine(this.OnInteractIE());
  }

  public IEnumerator OnInteractIE()
  {
    JanitorStation janitorStation = this;
    janitorStation.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    janitorStation.playerFarming.simpleSpineAnimator.Animate("Mop/collect", 0, true);
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", janitorStation.transform.position);
    AudioManager.Instance.PlayOneShot("event:/small_portal/open", janitorStation.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    int soulCount = janitorStation.StructureBrain.SoulCount;
    janitorStation.StructureBrain.SoulCount = 0;
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/material/mushroom_impact", janitorStation.transform.position);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/player_rise", janitorStation.transform.position);
    janitorStation._playerFarming.playerChoreXPBarController.AddChoreXP(janitorStation.playerFarming, (float) soulCount);
    yield return (object) new WaitForSeconds(0.5f);
    System.Action onCrownReturn = janitorStation.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    PlayerFarming.SetStateForAllPlayers();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    JanitorStation.JanitorStations.Remove(this);
  }
}
