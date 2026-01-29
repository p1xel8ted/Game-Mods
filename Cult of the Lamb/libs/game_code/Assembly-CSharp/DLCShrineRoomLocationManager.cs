// Decompiled with JetBrains decompiler
// Type: DLCShrineRoomLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using System.Collections;
using System.Collections.Generic;
using Unify;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
[DefaultExecutionOrder(-50)]
public class DLCShrineRoomLocationManager : HubLocationManager
{
  public static DLCShrineRoomLocationManager Instance;
  public Transform Room;
  public GenerateRoom DLCShrineGenerateRoom;
  public CompositeCollider2D sceneryCollider;
  public Transform DoorStartPosition;
  public Transform DoorExitPosition;
  public Interaction_PurchasableFleece[] fleeces;
  public ColliderCulling2D colliderCulling2D;

  public static void CheckWoolhavenCompleteAchievement()
  {
    if ((UnityEngine.Object) DLCShrineRoomLocationManager.Instance == (UnityEngine.Object) null || DataManager.Instance == null)
      return;
    bool flag1 = false | DataManager.IsAllJobBoardsComplete;
    HashSet<int> intSet;
    using (CollectionPool<HashSet<int>, int>.Get(out intSet))
    {
      foreach (int unlockedFleece in DataManager.Instance.UnlockedFleeces)
        intSet.Add(unlockedFleece);
      bool flag2 = true;
      foreach (Interaction_PurchasableFleece fleece in DLCShrineRoomLocationManager.Instance.fleeces)
      {
        if (!intSet.Contains((int) fleece.FleeceType))
        {
          flag2 = false;
          break;
        }
      }
      flag1 |= flag2;
      if (!DataManager.Instance.NPCGhostGeneric11Rescued)
        flag1 = false;
    }
    if (!(flag1 | DataManager.GetNumberOfFullFlowerPots() >= 10))
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup(AchievementsWrapper.Tags.WOOLHAVEN_COMPLETE));
  }

  public override FollowerLocation Location => FollowerLocation.DLC_ShrineRoom;

  public override Transform UnitLayer => this.Room;

  public override Transform StructureLayer => this.Room;

  public override void Awake()
  {
    DLCShrineRoomLocationManager.Instance = this;
    base.Awake();
    this.DLCShrineGenerateRoom.OnSetCollider += new GenerateRoom.GenerateEvent(this.GenerateSceneryColliderDelayed);
    DLCRebuildableShop.OnRestored += new System.Action(this.GenerateSceneryCollider);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this.DLCShrineGenerateRoom != (UnityEngine.Object) null)
      this.DLCShrineGenerateRoom.OnSetCollider -= new GenerateRoom.GenerateEvent(this.GenerateSceneryColliderDelayed);
    DLCRebuildableShop.OnRestored -= new System.Action(this.GenerateSceneryCollider);
  }

  public override void Start()
  {
    base.Start();
    this.StartCoroutine((IEnumerator) this.SetupColliderCullingIE());
  }

  public IEnumerator SetupColliderCullingIE()
  {
    DLCShrineRoomLocationManager roomLocationManager = this;
    yield return (object) new WaitForSecondsRealtime(1f);
    if ((UnityEngine.Object) roomLocationManager.colliderCulling2D == (UnityEngine.Object) null)
    {
      roomLocationManager.colliderCulling2D = roomLocationManager.gameObject.AddComponent<ColliderCulling2D>();
      foreach (Collider2D componentsInChild in roomLocationManager.GetComponentsInChildren<Collider2D>())
      {
        if ((!(componentsInChild is PolygonCollider2D) || componentsInChild.TryGetComponent<CullableCollider>(out CullableCollider _)) && !((UnityEngine.Object) componentsInChild.GetComponent<Health>() != (UnityEngine.Object) null) && !((UnityEngine.Object) componentsInChild.GetComponent<NonCullableColldier>() != (UnityEngine.Object) null))
          roomLocationManager.colliderCulling2D.allColliders.Add(componentsInChild);
      }
      roomLocationManager.colliderCulling2D.players = PlayerFarming.players;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
      BiomeBaseManager.Instance.PlayAtmos();
    if (!DataManager.Instance.EnabledDLCMapHeart || DataManager.Instance.BeatenYngya)
      return;
    AudioManager.Instance.StopCurrentMusic();
  }

  public override void OnDisable() => base.OnDisable();

  public override void PositionPlayer(GameObject player = null)
  {
    base.PositionPlayer(player);
    PlayerFarming.Instance.GoToAndStop(PlayerFarming.Instance.transform.position + new Vector3(0.0f, 8f), IdleOnEnd: true);
  }

  public override IEnumerator PlaceStructures()
  {
    DLCShrineRoomLocationManager roomLocationManager = this;
    List<Vector2Int> vector2IntList = new List<Vector2Int>();
    bool flag1 = GameManager.AuthenticateMajorDLC();
    roomLocationManager.CheckExistingStructures();
    roomLocationManager.structuresRequirePlacing.Clear();
    Vector2[] points = new Vector2[0];
    PolygonCollider2D collider = (PolygonCollider2D) null;
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
    {
      collider = BiomeBaseManager.Instance.Room.Pieces[0].Collider;
      points = collider.points;
    }
    for (int index = roomLocationManager.StructuresData.Count - 1; index >= 0; --index)
    {
      global::StructuresData structuresData = roomLocationManager.StructuresData[index];
      if (structuresData.Type != StructureBrain.TYPES.NONE)
      {
        if (!DataManager.Instance.DLC_Cultist_Pack && (DataManager.CultistDLCStructures.Contains(structuresData.Type) || DataManager.CultistDLCStructures.Contains(structuresData.ToBuildType)) || !DataManager.Instance.DLC_Sinful_Pack && (DataManager.SinfulDLCStructures.Contains(structuresData.Type) || DataManager.SinfulDLCStructures.Contains(structuresData.ToBuildType)) || !DataManager.Instance.DLC_Pilgrim_Pack && (DataManager.PilgrimDLCStructures.Contains(structuresData.Type) || DataManager.PilgrimDLCStructures.Contains(structuresData.ToBuildType)) || !DataManager.Instance.DLC_Heretic_Pack && (DataManager.HereticDLCStructures.Contains(structuresData.Type) || DataManager.HereticDLCStructures.Contains(structuresData.ToBuildType)))
          roomLocationManager.StructuresData.RemoveAt(index);
        else if (!flag1 && (DataManager.MajorDLCStructures.Contains(structuresData.Type) || DataManager.MajorDLCStructures.Contains(structuresData.ToBuildType)))
          roomLocationManager.StructuresData.RemoveAt(index);
        else if (structuresData.DontLoadMe)
        {
          bool flag2 = false;
          foreach (Structure structure in Structure.Structures)
          {
            if (structuresData.Position == structure.transform.position)
            {
              structure.Brain = StructureBrain.GetOrCreateBrain(structuresData);
              structure.Brain.AddToGrid();
              flag2 = true;
              break;
            }
          }
          if (!flag2)
            roomLocationManager.StructuresData.RemoveAt(index);
        }
        else if (roomLocationManager.IsStructurePlaced(structuresData))
        {
          foreach (Structure structure in Structure.Structures)
          {
            if (structuresData.Position == structure.transform.position && structure.Type == structuresData.Type)
            {
              structure.Brain = StructureBrain.GetOrCreateBrain(structuresData);
              structure.Brain.AddToGrid();
              break;
            }
          }
        }
        else
        {
          if (structuresData.Location == FollowerLocation.None)
          {
            Debug.LogWarning((object) $"Placing Structure {structuresData.Type}.{structuresData.ID} with Location.None, updating to {roomLocationManager.Location}");
            structuresData.Location = roomLocationManager.Location;
          }
          if (vector2IntList.Contains(structuresData.GridTilePosition) && structuresData.GridTilePosition != new Vector2Int(-2147483647 /*0x80000001*/, -2147483647 /*0x80000001*/))
            roomLocationManager.StructuresData.RemoveAt(index);
          else if (!roomLocationManager.EnsureWithinBounds(structuresData.Position, points, collider))
          {
            roomLocationManager.StructuresData.RemoveAt(index);
          }
          else
          {
            vector2IntList.Add(structuresData.GridTilePosition);
            for (int x = 0; x < structuresData.Bounds.x; ++x)
            {
              for (int y = 0; y < structuresData.Bounds.y; ++y)
                vector2IntList.Add(structuresData.GridTilePosition + new Vector2Int(x, y));
            }
            roomLocationManager.structuresRequirePlacing.Add(structuresData);
            roomLocationManager.InstantiateStructureAsync(structuresData);
          }
        }
      }
    }
    while (roomLocationManager.structuresRequirePlacing.Count > 0)
      yield return (object) null;
    roomLocationManager.StructuresPlaced = true;
    StructureManager.StructuresPlaced structuresPlaced = StructureManager.OnStructuresPlaced;
    if (structuresPlaced != null)
      structuresPlaced();
  }

  public bool IsStructurePlaced(global::StructuresData structuresData)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if (structuresData.Position == structure.transform.position && structure.Type == structuresData.Type)
        return true;
    }
    return false;
  }

  public IEnumerator WaitForWoolhavenYngyaStatue(System.Action callback)
  {
    while ((UnityEngine.Object) WoolhavenYngyaStatue.Instance == (UnityEngine.Object) null || (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    return DLCShrineRoomLocationManager.Instance.DoorStartPosition.position;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    return DLCShrineRoomLocationManager.Instance.DoorExitPosition.position;
  }

  public void GenerateSceneryCollider() => this.sceneryCollider.GenerateGeometry();

  public void GenerateSceneryColliderDelayed()
  {
    this.StartCoroutine((IEnumerator) this.GenerateSceneryColliderDelayedIE());
  }

  public IEnumerator GenerateSceneryColliderDelayedIE()
  {
    yield return (object) new WaitForEndOfFrame();
    this.sceneryCollider.GenerateGeometry();
  }
}
