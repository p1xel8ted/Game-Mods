// Decompiled with JetBrains decompiler
// Type: BaseLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class BaseLocationManager : LocationManager
{
  public static BaseLocationManager Instance;
  [SerializeField]
  public Transform _unitLayer;
  [SerializeField]
  public Transform _structureLayer;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => true;

  public override Transform StructureLayer => this._structureLayer;

  public override void Awake()
  {
    base.Awake();
    BaseLocationManager.Instance = this;
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.ShowCultName);
    DoctrineUpgradeSystem.Initialise();
    FollowerTrait.Initialise();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    FollowerTrait.DeInitialise();
  }

  public void ShowCultName()
  {
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.ShowCultName);
    if (string.IsNullOrEmpty(DataManager.Instance.CultName))
      return;
    HUD_DisplayName.PlayTranslatedText(DataManager.Instance.CultName, 3, HUD_DisplayName.Positions.Centre);
  }

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
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
          startPosition = !((Object) Interaction_Temple.Instance == (Object) null) ? Interaction_Temple.Instance.ExitPosition.transform.position : BiomeBaseManager.Instance.PlayerSpawnLocation.transform.position;
          break;
        case FollowerLocation.DoorRoom:
        case FollowerLocation.DLC_DoorRoom:
          startPosition = BiomeBaseManager.Instance.PlayerReturnFromDoorRoomLocation.transform.position;
          break;
        case FollowerLocation.Endless:
          startPosition = BiomeBaseManager.Instance.PlayerReturnFromEndlessLocation.transform.position;
          break;
        case FollowerLocation.DLC_ShrineRoom:
          startPosition = BiomeBaseManager.Instance.DLC_ShrineRoomEntrance.position;
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

  public override void PrePlacingStructures()
  {
    this.StartCoroutine((IEnumerator) this.CombineCollidersIE());
  }

  public IEnumerator CombineCollidersIE()
  {
    while ((Object) BiomeBaseManager.Instance == (Object) null)
      yield return (object) null;
    BiomeBaseManager.Instance.CombineDLCRoomColliders();
  }

  public override void PostPlaceStructures()
  {
    StructureManager.PlaceRubble(this.Location);
    BiomeBaseManager.Instance.SpawnAnimals();
  }
}
