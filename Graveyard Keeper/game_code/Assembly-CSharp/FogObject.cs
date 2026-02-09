// Decompiled with JetBrains decompiler
// Type: FogObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FogObject : MonoBehaviour
{
  public Vector2 scr_pos;
  public Vector2 tile_pos;
  public const int SIZE_X = 300;
  public const int SIZE_Y = 300;
  public const float OBJECT_WIDTH_IN_TILES = 6f;
  public const float OBJECT_HEIGHT_IN_TILES = 0.3f;
  public const int FOGFIELD_WIDTH_IN_OBJECTS = 6;
  public const int FOGFIELD_HEIGHT_IN_OBJECTS = 63 /*0x3F*/;
  public Vector3 TILES_X_VECTOR = (Vector3) new Vector2(36f, 0.0f);
  public Vector3 TILES_Y_VECTOR = (Vector3) new Vector2(0.0f, 18.9000015f);
  public static Transform _fog_parent = (Transform) null;
  public RoundAndSortComponent round_and_sort;
  public bool round_and_sort_set;
  public Material _mat;
  public static float BORDER_X = 1f;
  public static float BORDER_Y = 6f;

  public void Update()
  {
    this.scr_pos = (Vector2) MainGame.me.world_cam.WorldToScreenPoint(this.transform.position);
    this.tile_pos = new Vector2(this.scr_pos.x / 6f, this.scr_pos.y / 0.3f) / 96f;
    bool flag = false;
    if ((double) this.tile_pos.x < -(double) FogObject.BORDER_X)
    {
      this.transform.localPosition += this.TILES_X_VECTOR;
      flag = true;
    }
    else if ((double) this.tile_pos.x > 6.0 - (double) FogObject.BORDER_X)
    {
      this.transform.localPosition -= this.TILES_X_VECTOR;
      flag = true;
    }
    if ((double) this.tile_pos.y < -(double) FogObject.BORDER_Y)
    {
      this.transform.localPosition += this.TILES_Y_VECTOR;
      flag = true;
    }
    else if ((double) this.tile_pos.y > 63.0 - (double) FogObject.BORDER_Y)
    {
      this.transform.localPosition -= this.TILES_Y_VECTOR;
      flag = true;
    }
    if (!flag || !this.round_and_sort_set)
      return;
    this.round_and_sort.DoUpdateStuff();
  }

  public static void InitFog(FogObject prefab)
  {
    if ((Object) prefab == (Object) null)
    {
      Debug.LogError((object) "Fog object not found on a scene");
    }
    else
    {
      FogObject._fog_parent = Fog.SpawnNewFog();
      ChunkedGameObject chunkedGameObject = prefab.GetComponent<ChunkedGameObject>();
      if ((Object) chunkedGameObject == (Object) null)
        chunkedGameObject = prefab.gameObject.AddComponent<ChunkedGameObject>();
      chunkedGameObject.always_active = true;
      for (int index1 = 0; index1 < 6; ++index1)
      {
        for (int index2 = 0; index2 < 63 /*0x3F*/; ++index2)
        {
          FogObject fo = Object.Instantiate<FogObject>(prefab);
          fo.transform.SetParent(FogObject._fog_parent, false);
          fo.round_and_sort = fo.GetComponent<RoundAndSortComponent>();
          fo.round_and_sort_set = (Object) fo.round_and_sort != (Object) null;
          Fog.me.OnNewFogObjectCreated(fo);
          fo.transform.localPosition = new Vector3(6f * (float) index1, 0.3f * (float) index2);
        }
      }
    }
  }
}
