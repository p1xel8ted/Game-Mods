// Decompiled with JetBrains decompiler
// Type: Interaction_MissionShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_MissionShrine : Interaction
{
  [SerializeField]
  private GameObject missionAvailableEffect;
  [SerializeField]
  private GameObject container;
  private Structures_MissionShrine _StructureInfo;
  private string key;

  public Structure Structure { get; private set; }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_MissionShrine StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_MissionShrine;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Structure = this.GetComponentInParent<Structure>();
  }

  protected override void Update()
  {
    base.Update();
    this.Interactable = this.StructureInfo != null && DataManager.Instance.MissionShrineUnlocked;
    this.missionAvailableEffect.SetActive(this.StructureInfo != null && DataManager.Instance.AvailableMissions.Count > 0);
    this.container.SetActive(this.Interactable);
    if ((double) DataManager.Instance.NewMissionDayTimestamp == -1.0 || (double) TimeManager.CurrentDay < (double) DataManager.Instance.NewMissionDayTimestamp)
      return;
    if (this.CanAddNewMission())
      this.AddNewMission();
    else
      DataManager.Instance.NewMissionDayTimestamp = (float) (TimeManager.CurrentDay + 1);
    for (int index = DataManager.Instance.ActiveMissions.Count - 1; index >= 0; --index)
    {
      if ((double) TimeManager.TotalElapsedGameTime >= (double) DataManager.Instance.ActiveMissions[index].ExpiryTimestamp)
        MissionManager.RemoveMission(DataManager.Instance.ActiveMissions[index]);
    }
    for (int index = DataManager.Instance.AvailableMissions.Count - 1; index >= 0; --index)
    {
      if ((double) TimeManager.TotalElapsedGameTime >= (double) DataManager.Instance.AvailableMissions[index].ExpiryTimestamp)
        DataManager.Instance.AvailableMissions.RemoveAt(index);
    }
  }

  public void AddNewMission()
  {
    int num = 3 - (DataManager.Instance.AvailableMissions.Count + DataManager.Instance.ActiveMissions.Count);
    for (int index = 0; index < num; ++index)
      MissionManager.AddMission(MissionManager.MissionType.Bounty, Random.Range(1, 4), this.IsGoldenMission());
    DataManager.Instance.NewMissionDayTimestamp = (float) (TimeManager.CurrentDay + 1);
  }

  private bool IsGoldenMission()
  {
    if (DataManager.Instance.LastGoldenMissionDay == -1)
      DataManager.Instance.LastGoldenMissionDay = TimeManager.CurrentDay;
    bool flag = false;
    if (0.05000000074505806 * (double) (TimeManager.CurrentDay - DataManager.Instance.LastGoldenMissionDay) >= (double) Random.Range(0.0f, 1f))
      flag = true;
    if (flag)
      DataManager.Instance.LastGoldenMissionDay = TimeManager.CurrentDay;
    return flag;
  }

  private bool CanAddNewMission()
  {
    return DataManager.Instance.AvailableMissions.Count + DataManager.Instance.ActiveMissions.Count < 3;
  }

  public override void OnInteract(StateMachine state) => base.OnInteract(state);

  public override void GetLabel() => this.Label = this.Interactable ? this.key : "";

  public override void UpdateLocalisation() => base.UpdateLocalisation();
}
