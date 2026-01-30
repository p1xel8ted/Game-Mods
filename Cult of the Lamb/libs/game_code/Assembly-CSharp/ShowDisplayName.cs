// Decompiled with JetBrains decompiler
// Type: ShowDisplayName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShowDisplayName : MonoBehaviour
{
  [TermsPopup("")]
  public string PlaceName = "";
  public bool ShowPlaceName;
  public int holdTime = 3;
  public HUD_DisplayName.Positions position = HUD_DisplayName.Positions.Centre;

  public void Start() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.delay());

  public IEnumerator delay()
  {
    yield return (object) new WaitForSeconds(0.1f);
    if (this.ShowPlaceName)
      HUD_DisplayName.Play(this.PlaceName, this.holdTime, this.position);
    FollowerLocation location = PlayerFarming.Location;
    yield return (object) new WaitForSeconds(5f);
    if (!DataManager.Instance.NewLocationFaithReward.Contains(location) && location != FollowerLocation.Base)
    {
      switch (location)
      {
        case FollowerLocation.HubShore:
        case FollowerLocation.Hub1_RatauOutside:
        case FollowerLocation.Hub1_Sozo:
        case FollowerLocation.Dungeon_Decoration_Shop1:
        case FollowerLocation.Dungeon_Location_3:
        case FollowerLocation.Graveyard_Location:
          CultFaithManager.AddThought(Thought.Cult_DiscoveredNewLocation);
          DataManager.Instance.NewLocationFaithReward.Add(location);
          break;
      }
    }
    switch (location)
    {
      case FollowerLocation.HubShore:
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitFisherman);
        break;
      case FollowerLocation.Hub1_RatauOutside:
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitRatau);
        break;
      case FollowerLocation.Hub1_Sozo:
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitSozo);
        break;
      case FollowerLocation.Dungeon_Decoration_Shop1:
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitPlimbo);
        break;
      case FollowerLocation.Dungeon_Location_3:
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitMidas);
        break;
    }
  }
}
