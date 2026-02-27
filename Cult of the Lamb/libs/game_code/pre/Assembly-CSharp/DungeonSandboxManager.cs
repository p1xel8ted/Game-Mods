// Decompiled with JetBrains decompiler
// Type: DungeonSandboxManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DungeonSandboxManager : BaseMonoBehaviour
{
  public static DungeonSandboxManager Instance;
  [SerializeField]
  private AnimationCurve dungeonSizeCurse;
  [SerializeField]
  private DungeonSandboxManager.Dungeon[] dungeons;
  [SerializeField]
  private string[] miniBossRooms;
  private BiomeGenerator biomeGenerator;
  private GenerateRoom roomGenerator;
  private int maxLayer = 10;

  public int SandboxLayer { get; private set; }

  private void Awake()
  {
    DungeonSandboxManager.Instance = this;
    this.biomeGenerator = this.GetComponentInChildren<BiomeGenerator>();
    this.roomGenerator = this.GetComponentInChildren<GenerateRoom>();
    this.LayerIncremented();
  }

  public void SetDungeonType(FollowerLocation location)
  {
    this.roomGenerator.StartPieces = ((IEnumerable<IslandPiece>) this.GetIslandPieces(location)).ToList<IslandPiece>();
    this.roomGenerator.DecorationSetList = ((IEnumerable<GeneraterDecorations>) this.GetDecorationList(location)).ToList<GeneraterDecorations>();
    this.biomeGenerator.DungeonLocation = location;
    this.biomeGenerator.BossRoomPath = this.GetBossRoom();
    LocationManager.UpdateLocation();
  }

  public GeneraterDecorations[] GetDecorationList(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.decorations;
    }
    return new GeneraterDecorations[0];
  }

  public IslandPiece[] GetIslandPieces(FollowerLocation location)
  {
    foreach (DungeonSandboxManager.Dungeon dungeon in this.dungeons)
    {
      if (dungeon.Location == location)
        return dungeon.islandPieces;
    }
    return new IslandPiece[0];
  }

  public string GetBossRoom() => this.miniBossRooms[UnityEngine.Random.Range(0, this.miniBossRooms.Length)];

  public void LayerIncremented()
  {
    ++this.SandboxLayer;
    this.biomeGenerator.NumberOfRooms = Mathf.RoundToInt((float) (10.0 * (1.0 + (double) this.dungeonSizeCurse.Evaluate((float) this.SandboxLayer / (float) this.maxLayer))));
    GameManager.CurrentDungeonLayer = Mathf.Clamp(this.SandboxLayer, 1, 4);
    List<FollowerLocation> followerLocationList = new List<FollowerLocation>()
    {
      FollowerLocation.Dungeon1_1,
      FollowerLocation.Dungeon1_2,
      FollowerLocation.Dungeon1_3,
      FollowerLocation.Dungeon1_4
    };
    if (followerLocationList.Contains(this.biomeGenerator.DungeonLocation))
      followerLocationList.Remove(this.biomeGenerator.DungeonLocation);
    this.SetDungeonType(followerLocationList[UnityEngine.Random.Range(0, followerLocationList.Count)]);
  }

  [Serializable]
  private struct Dungeon
  {
    public FollowerLocation Location;
    public GeneraterDecorations[] decorations;
    public IslandPiece[] islandPieces;
  }
}
