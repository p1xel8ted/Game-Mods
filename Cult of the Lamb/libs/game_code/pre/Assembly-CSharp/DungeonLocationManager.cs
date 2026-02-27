// Decompiled with JetBrains decompiler
// Type: DungeonLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class DungeonLocationManager : LocationManager
{
  [SerializeField]
  private Transform _unitLayer;
  private FollowerLocation _location = FollowerLocation.None;

  public override FollowerLocation Location => this._location;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => true;

  public override Transform StructureLayer => this._unitLayer;

  protected override void Awake()
  {
    this._location = !(bool) (Object) BiomeGenerator.Instance ? FollowerLocation.Dungeon1_1 : BiomeGenerator.Instance.DungeonLocation;
    base.Awake();
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  protected override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    Vector3 startPosition = Vector3.zero;
    if ((Object) PlayerFarming.Instance != (Object) null)
      startPosition = PlayerFarming.Instance.transform.position;
    return startPosition;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    IslandPiece islandPiece = (IslandPiece) null;
    foreach (IslandPiece piece in GenerateRoom.Instance.Pieces)
    {
      if (piece.IsDoor)
      {
        islandPiece = piece;
        break;
      }
    }
    return !((Object) islandPiece != (Object) null) ? PlayerFarming.Instance.transform.position : islandPiece.transform.position;
  }

  private void OnChangeRoom()
  {
    this.StartCoroutine((IEnumerator) this.PlaceRoomStructuresRoutine());
  }

  private IEnumerator PlaceRoomStructuresRoutine()
  {
    DungeonLocationManager dungeonLocationManager = this;
    if (dungeonLocationManager.SupportsStructures)
    {
      yield return (object) new WaitForEndOfFrame();
      yield return (object) dungeonLocationManager.StartCoroutine((IEnumerator) dungeonLocationManager.PlaceStructures());
      dungeonLocationManager.PostPlaceStructures();
    }
  }
}
