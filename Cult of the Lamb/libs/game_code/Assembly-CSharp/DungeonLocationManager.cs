// Decompiled with JetBrains decompiler
// Type: DungeonLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class DungeonLocationManager : LocationManager
{
  [SerializeField]
  public Transform _unitLayer;
  [CompilerGenerated]
  public FollowerLocation \u003C_location\u003Ek__BackingField = FollowerLocation.None;

  public override FollowerLocation Location => this._location;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => true;

  public override Transform StructureLayer => this._unitLayer;

  public FollowerLocation _location
  {
    get => this.\u003C_location\u003Ek__BackingField;
    set => this.\u003C_location\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    this._location = !(bool) (Object) BiomeGenerator.Instance ? FollowerLocation.Dungeon1_1 : BiomeGenerator.Instance.DungeonLocation;
    base.Awake();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
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

  public void OnChangeRoom()
  {
    this.StartCoroutine((IEnumerator) this.PlaceRoomStructuresRoutine());
  }

  public IEnumerator PlaceRoomStructuresRoutine()
  {
    DungeonLocationManager dungeonLocationManager = this;
    if (dungeonLocationManager.SupportsStructures)
    {
      yield return (object) new WaitForEndOfFrame();
      yield return (object) dungeonLocationManager.StartCoroutine((IEnumerator) dungeonLocationManager.PlaceStructures());
      yield return (object) new WaitForEndOfFrame();
      dungeonLocationManager.PostPlaceStructures();
    }
  }
}
