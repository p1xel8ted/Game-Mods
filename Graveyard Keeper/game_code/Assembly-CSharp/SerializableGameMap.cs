// Decompiled with JetBrains decompiler
// Type: SerializableGameMap
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SerializableGameMap
{
  [SerializeField]
  public List<SerializableWGO> _wgos = new List<SerializableWGO>();
  [SerializeField]
  public List<MobSpawner.SerializableSpawner> _spawners = new List<MobSpawner.SerializableSpawner>();
  [SerializeField]
  public List<SerializedTechPointDrop> _tech_drops = new List<SerializedTechPointDrop>();

  public List<SerializableWGO> wgos => this.wgos;

  public string ToJSON(bool at_game_start = false)
  {
    this.SaveSceneToMe(at_game_start);
    return JsonUtility.ToJson((object) this, true);
  }

  public byte[] ToBinary(bool at_game_start = false)
  {
    this.SaveSceneToMe(at_game_start);
    byte[] binary = SmartSerializer.Serialize<SerializableGameMap>(this);
    Debug.Log((object) ("SerializableGameMap length = " + binary.Length.ToString()));
    return binary;
  }

  public void FromJSON(string json) => JsonUtility.FromJsonOverwrite(json, (object) this);

  public void FromBinary(byte[] data)
  {
    SmartSerializer.DeserializeInto<SerializableGameMap>(this, data);
  }

  public void SaveSceneToMe(bool at_game_start = false)
  {
    this._wgos.Clear();
    WorldGameObject[] componentsInChildren = MainGame.me.world_root.gameObject.GetComponentsInChildren<WorldGameObject>(true);
    Debug.Log((object) ("SaveSceneToMe, count = " + componentsInChildren.Length.ToString()));
    foreach (WorldGameObject wgo in componentsInChildren)
    {
      if (!wgo.is_player && !(wgo.name == "Player") && !(wgo.obj_id == "0"))
      {
        if (at_game_start)
          wgo.SetParam("hp_inited", 0.0f);
        this._wgos.Add(SerializableWGO.FromWGO(wgo));
      }
    }
    foreach (SimplifiedWGO componentsInChild in MainGame.me.world_root.gameObject.GetComponentsInChildren<SimplifiedWGO>(true))
      this._wgos.Add(componentsInChild.swgo);
    this._spawners.Clear();
    foreach (MobSpawner componentsInChild in MainGame.me.world.GetComponentsInChildren<MobSpawner>(true))
      this._spawners.Add(componentsInChild.ToSerializable());
    this._tech_drops.Clear();
    foreach (TechPointDrop drop in TechPointDrop.all)
    {
      SerializedTechPointDrop serializedTechPointDrop = new SerializedTechPointDrop();
      serializedTechPointDrop.FromTechPointDrop(drop);
      this._tech_drops.Add(serializedTechPointDrop);
    }
  }

  public void RestoreSceneToInitialState()
  {
    Debug.Log((object) nameof (RestoreSceneToInitialState));
    this.FromBinary(GameLoader.initial_map_bin);
    this.RestoreScene();
  }

  public void RestoreScene()
  {
    Debug.Log((object) ("RestoreScene, wgos count = " + this._wgos.Count.ToString()));
    this.ClearSceneMap();
    WorldMap.RescanGDPoints();
    foreach (SerializableWGO wgo in this._wgos)
      WorldGameObject.InstantiateWGOPrefab().RestoreFromSerializedObject(wgo);
    foreach (MobSpawner componentsInChild in MainGame.me.world.GetComponentsInChildren<MobSpawner>(true))
    {
      IntVector2 v = new IntVector2((Vector2) componentsInChild.transform.position);
      foreach (MobSpawner.SerializableSpawner spawner in this._spawners)
      {
        if (spawner.coords.EqualsTo(v))
        {
          componentsInChild.FromSerializable(spawner);
          break;
        }
      }
    }
    ObjectDynamicShadow.InstantiateAllAdditionalShadows();
  }

  public void DeserializeTechPoints()
  {
    if (this._tech_drops == null)
    {
      Debug.LogError((object) "TechPoints list is null!");
    }
    else
    {
      Debug.Log((object) $"Deserializing tech points, Count = {this._tech_drops.Count}");
      foreach (SerializedTechPointDrop techDrop in this._tech_drops)
        TechPointDrop.Spawn(GUIElements.me.tech_points_spawner.prefab, techDrop.type).transform.position = (Vector3) techDrop.pos;
    }
  }

  public void ClearSceneMap()
  {
    SaveSlotsMenuGUI.PrepareScene();
    foreach (WorldGameObject componentsInChild in MainGame.me.world_root.GetComponentsInChildren<WorldGameObject>(true))
    {
      if (!((UnityEngine.Object) componentsInChild.GetComponent<PlayerComponent>() != (UnityEngine.Object) null))
      {
        if (componentsInChild.name.Contains("Player"))
          Debug.LogError((object) ("Deleting player: " + componentsInChild.name));
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild.gameObject);
      }
    }
    foreach (Component componentsInChild in MainGame.me.world_root.GetComponentsInChildren<SimplifiedWGO>(true))
      UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild.gameObject);
    ChunkManager.ClearChunksList();
    foreach (ChunkedGameObject componentsInChild in MainGame.me.world_root.GetComponentsInChildren<ChunkedGameObject>(true))
      componentsInChild.ResetAtTheBeginning();
  }
}
