// Decompiled with JetBrains decompiler
// Type: LazyConsts
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class LazyConsts
{
  public const bool DEMO_BUILD = false;
  public const bool ALPHA_BUILD = false;
  public const float VERSION_INT = 1407f;
  public const int DLC_STORIES_VERSION_INT = 1200;
  public const float FIXED_DELTA_TIME = 0.0166666675f;
  public const float SLEEP_TIME_SCALE = 10f;
  public const int GRID_SIZE = 96 /*0x60*/;
  public const int GRID_SIZE_HALF = 48 /*0x30*/;
  public static Vector2 GRID_SIZE_VECTOR2 = new Vector2(96f, 96f);
  public static Vector2 GRID_SIZE_VECTOR2_08 = new Vector2(76.8f, 76.8f);
  public const float INTERACTIVE_DROPS_RADUIS = 1.8f;
  public const float INTERACTIVE_DROPS_MAGNET_SPEED_K = 1.3f;
  public const float DROPS_MAGNET_RADIUS = 270f;
  public const float TECHPOINT_COLLECT_DELAY = 0.7f;
  public const float TECH_POINTS_MAGNET_RADIUS = 200f;
  public const int DEFAULT_LAYER = 0;
  public const int INTERACTION_LAYER = 8;
  public const int CHAR_LAYER = 9;
  public const int DOCKS_LAYER = 11;
  public const int LIGHT_LAYER = 12;
  public const int NGUI_LAYER = 13;
  public const int DROP_LAYER = 14;
  public const int COMBAT_LAYERS = 15;
  public const int VISUAL_FX = 16 /*0x10*/;
  public const int CAMERA_BOUNDS_LAYER = 17;
  public const int GROUND_LAYER = 18;
  public const int ZONES_LAYER = 19;
  public const int GD_ZONES_LAYER = 21;
  public const int GD_LOGICS_LAYER = 22;
  public const int BUILD_COLLIDERS_LAYER = 23;
  public const int BUILD_IGNORE_LAYER = 28;
  public const int WALLS_MASK = 1;
  public const int CHAR_MASK = 512 /*0x0200*/;
  public const int CAMERA_BOUNDS_MASK = 131072 /*0x020000*/;
  public const int DROPS_MASK = 16384 /*0x4000*/;
  public const int OBJ_MASK = 66305;
  public const int ZONES_MASK = 524288 /*0x080000*/;
  public const int BUILD_COLLIDERS_MASK = 8388608 /*0x800000*/;
  public const int PROJECTILE_MASK = 66049;
  public const int BUILD_IGNORE_LAYER_MASK = 268435456 /*0x10000000*/;
  public const int NGUI_MASK = 8192 /*0x2000*/;
  public const float INTERACTIVE_DROPS_RADUIS_SQR = 3.23999977f;
  public static bool editor_updates_allowed = true;
  public static bool DYAK = true;
  public static float PI2 = 6.28318548f;
  public static float PI_DIV_2 = 1.57079637f;
  public static float PLAYER_SPEED = 3.3f;
  public const bool FORCE_MOBS_LAYER = true;
  public const bool REALTIME_UPDATE_GRAPH_FOR_MOBS = true;
  public const bool SHOW_AUTOCRAFT_ONLY_IF_PLAYER_IS_NEAR = false;
  public const int BODY_DURABILITY_THAT_COUNTS_AS_100 = 90;
  public const LazyConsts.GameSaveSerializer GAME_SERIALIZER = LazyConsts.GameSaveSerializer.Binary;
  public const bool DASH_AVAILABLE = false;
  public const bool CRAFT_TASK_SYSTEM = true;
  public const float ZOMBIE_RESURRECT_PERCENTAGE = 0.9f;
  public const bool BUILD_FOR_TRAILER = false;
  public const float CAMERA_FADE_TIME = 2f;
  public const string RAT_SPEED_ICON = "(speed)";
  public const string RAT_OBEDIENCE_ICON = "(obedience)";

  public static float VERSION => 1.407f;

  public enum GameSaveSerializer
  {
    JSON,
    Binary,
  }
}
