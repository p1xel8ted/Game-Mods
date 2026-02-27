// Decompiled with JetBrains decompiler
// Type: BaseLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class BaseLocationManager : LocationManager
{
  public static BaseLocationManager Instance;
  [SerializeField]
  private Transform _unitLayer;
  [SerializeField]
  private Transform _structureLayer;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => true;

  public override Transform StructureLayer => this._structureLayer;

  protected override void Awake()
  {
    base.Awake();
    BaseLocationManager.Instance = this;
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.ShowCultName);
  }

  private void ShowCultName()
  {
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.ShowCultName);
    if (string.IsNullOrEmpty(DataManager.Instance.CultName))
      return;
    HUD_DisplayName.PlayTranslatedText(DataManager.Instance.CultName, 3, HUD_DisplayName.Positions.Centre);
  }

  protected override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    Vector3 startPosition;
    if ((Object) BiomeBaseManager.Instance == (Object) null)
    {
      Debug.LogWarning((object) "BiomeBaseManager.Instance == null ??");
      startPosition = Vector3.zero;
    }
    else
    {
      switch (prevLocation)
      {
        case FollowerLocation.Church:
          startPosition = !((Object) Interaction_Temple.Instance == (Object) null) ? Interaction_Temple.Instance.ExitPosition.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.0f) : BiomeBaseManager.Instance.PlayerSpawnLocation.transform.position;
          break;
        case FollowerLocation.DoorRoom:
          startPosition = BiomeBaseManager.Instance.PlayerReturnFromDoorRoomLocation.transform.position;
          break;
        default:
          startPosition = BiomeBaseManager.Instance.PlayerSpawnLocation.transform.position;
          break;
      }
    }
    return startPosition;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    return destLocation != FollowerLocation.Church ? BiomeBaseManager.Instance.PlayerSpawnLocation.transform.position : Interaction_Temple.Instance.Entrance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.0f);
  }

  protected override void PostPlaceStructures() => StructureManager.PlaceRubble(this.Location);
}
