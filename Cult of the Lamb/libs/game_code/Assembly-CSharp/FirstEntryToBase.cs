// Decompiled with JetBrains decompiler
// Type: FirstEntryToBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FirstEntryToBase : BaseMonoBehaviour
{
  public GameObject Rat1Indoctrinate;
  public GameObject Rat1Shrine;
  public GameObject Rat1Dungeon;
  public GameObject Rat2Food;
  public GameObject Rat2Faith;
  public GameObject Rat2BackToDungeon;
  public GameObject Rat3Night;
  public bool ForceFollowersToBuild;
  public GameObject UITutorialPrefab;
  public UIShrineTutorial UIShrineTutorial;

  public void Start()
  {
    this.HideAll();
    if (!DataManager.Instance.InTutorial)
      this.Rat1Indoctrinate.SetActive(true);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public void OnStructureAdded(StructuresData structure)
  {
    if (this.ForceFollowersToBuild && structure.Type == StructureBrain.TYPES.BUILD_SITE && structure.ToBuildType == StructureBrain.TYPES.SHRINE)
    {
      foreach (Follower follower in Follower.Followers)
        follower.Brain.CompleteCurrentTask();
      this.ForceFollowersToBuild = false;
    }
    if (DataManager.Instance.Tutorial_ReturnToDungeon || structure.Type != StructureBrain.TYPES.BUILD_SITE || structure.ToBuildType != StructureBrain.TYPES.TEMPLE)
      return;
    this.HideAll();
    Debug.Log((object) ("Rat2BackToDungeon " + this.Rat2BackToDungeon?.ToString()));
    if ((Object) this.Rat2BackToDungeon != (Object) null)
      this.Rat2BackToDungeon.SetActive(true);
    DataManager.Instance.Tutorial_ReturnToDungeon = true;
  }

  public void OnObjectiveComplete(string GroupID)
  {
    switch (GroupID)
    {
      case "Objectives/GroupTitles/RecruitFollower":
        this.HideAll();
        DataManager.Instance.AllowBuilding = true;
        DataManager.Instance.InTutorial = true;
        this.Rat1Shrine.SetActive(true);
        break;
      case "Objectives/GroupTitles/RepairTheShrine":
        this.CreateFollowers();
        this.HideAll();
        this.Rat1Dungeon.SetActive(true);
        break;
      case "Objectives/GroupTitles/GoToDungeon":
        this.HideAll();
        this.Rat2Food.SetActive(true);
        this.ForceFollowersToBuild = true;
        break;
      case "Objectives/GroupTitles/Food":
        this.HideAll();
        this.Rat2Faith.SetActive(true);
        break;
      default:
        int num = GroupID == "Objectives/GroupTitles/Faith" ? 1 : 0;
        break;
    }
  }

  public void OnDisable()
  {
  }

  public void HideAll()
  {
    if ((Object) this.Rat1Shrine != (Object) null)
      this.Rat1Shrine.SetActive(false);
    if ((Object) this.Rat1Indoctrinate != (Object) null)
      this.Rat1Indoctrinate.SetActive(false);
    if ((Object) this.Rat1Dungeon != (Object) null)
      this.Rat1Dungeon.SetActive(false);
    if ((Object) this.Rat2Food != (Object) null)
      this.Rat2Food.SetActive(false);
    if ((Object) this.Rat2Faith != (Object) null)
      this.Rat2Faith.SetActive(false);
    if ((Object) this.Rat2BackToDungeon != (Object) null)
      this.Rat2BackToDungeon.SetActive(false);
    if (!((Object) this.Rat3Night != (Object) null))
      return;
    this.Rat3Night.SetActive(false);
  }

  public void CreateFollowers()
  {
    FollowerManager.CreateNewRecruit(FollowerLocation.Base, BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
  }

  public void PlayShrineTutorial()
  {
    this.UIShrineTutorial = Object.Instantiate<GameObject>(this.UITutorialPrefab, GameObject.FindWithTag("Canvas").transform).GetComponent<UIShrineTutorial>();
  }

  public void ShowXPBar() => this.StartCoroutine(this.ShowXPBarRoutine());

  public IEnumerator ShowXPBarRoutine()
  {
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.Instance.XPBarTransitions.MoveBackInFunction();
  }
}
