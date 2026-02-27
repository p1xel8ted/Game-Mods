// Decompiled with JetBrains decompiler
// Type: ShowDisplayName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void Start() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.delay());

  private IEnumerator delay()
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
