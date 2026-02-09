// Decompiled with JetBrains decompiler
// Type: World
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class World : MonoBehaviour
{
  public GameObject zones;
  public static Vector3 _player_default_pos = Vector3.zero;
  public static bool _player_default_pos_set = false;

  public void FindAndRemovePlayerPrefab()
  {
    foreach (PlayerComponent componentsInChild in this.GetComponentsInChildren<PlayerComponent>(true))
    {
      if (!World._player_default_pos_set)
      {
        World._player_default_pos_set = true;
        World._player_default_pos = componentsInChild.gameObject.transform.localPosition;
        Debug.Log((object) ("Player default pos: " + World._player_default_pos.ToString()));
        if (!MainGame.loaded_from_scene_main)
          MainGame.me.save.player_position = World._player_default_pos;
      }
      Debug.Log((object) "Removing player prefab");
      Object.Destroy((Object) componentsInChild.gameObject);
    }
  }

  public static void InitWorldOnApplicationStart()
  {
    Debug.Log((object) nameof (InitWorldOnApplicationStart));
    MainGame.me.world = Object.FindObjectOfType<World>();
    if ((Object) MainGame.me.world == (Object) null)
      Debug.LogError((object) "Couldn't find the world");
    else
      MainGame.me.world_root = MainGame.me.world.transform;
  }

  public static Vector3 player_default_pos
  {
    get
    {
      if (World._player_default_pos_set)
        return World._player_default_pos;
      Debug.LogError((object) "Player default pos is not set");
      return Vector3.zero;
    }
  }

  public void LateUpdate()
  {
  }
}
