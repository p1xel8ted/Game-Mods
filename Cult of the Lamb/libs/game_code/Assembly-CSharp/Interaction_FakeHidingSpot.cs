// Decompiled with JetBrains decompiler
// Type: Interaction_FakeHidingSpot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_FakeHidingSpot : Interaction
{
  [SerializeField]
  public Objectives_FindChildren.ChildLocation childLocation;
  [SerializeField]
  public InventoryItem.ITEM_TYPE dropItem;
  public bool searched;

  public override void Update()
  {
    base.Update();
    this.Interactable = this.IsObjectiveActive();
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.IsObjectiveActive())
      this.Label = ScriptLocalization.UI.Search;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine(this.SearchRoutine());
    this.searched = true;
  }

  public IEnumerator SearchRoutine()
  {
    Interaction_FakeHidingSpot interactionFakeHidingSpot = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFakeHidingSpot.gameObject, 8f);
    GameManager.GetInstance().CameraZoom(6f, 3f);
    EventInstance introInstance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/intro/intro_bass");
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().CameraZoom(8f, 0.1f);
    AudioManager.Instance.StopOneShotInstanceEarly(introInstance, STOP_MODE.IMMEDIATE);
    AudioManager.Instance.PlayOneShot("event:/Stings/gamble_lose", interactionFakeHidingSpot.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionFakeHidingSpot.gameObject);
    InventoryItem.Spawn(interactionFakeHidingSpot.dropItem, 1, interactionFakeHidingSpot.transform.position);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public bool IsObjectiveActive()
  {
    if (this.searched)
      return false;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_FindChildren objectivesFindChildren && this.childLocation == objectivesFindChildren.Location)
        return !objectivesFindChildren.IsComplete && (objectivesFindChildren.Location != Objectives_FindChildren.ChildLocation.SporeGrotto || TimeManager.CurrentPhase != DayPhase.Night) && (objectivesFindChildren.Location != Objectives_FindChildren.ChildLocation.SmugglersSanctuary || TimeManager.CurrentPhase == DayPhase.Night) && (objectivesFindChildren.Location != Objectives_FindChildren.ChildLocation.MidasCave || SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard);
    }
    return false;
  }
}
