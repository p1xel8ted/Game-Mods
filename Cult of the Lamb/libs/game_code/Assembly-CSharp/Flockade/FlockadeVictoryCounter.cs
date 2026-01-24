// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVictoryCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeVictoryCounter : MonoBehaviour, IFlockadeCounter
{
  public const string _LEFT_SIDE_POINT_SCORED_SOUND = "event:/dlc/ui/flockade_minigame/battle_phase_end_player_goldshield_tick";
  public const string _RIGHT_SIDE_POINT_SCORED_SOUND = "event:/dlc/ui/flockade_minigame/battle_phase_end_opponent_goldshield_tick";
  public const float _BETWEEN_VICTORIES_APPEARANCE_DISAPPEARANCE_DELAY = 0.0333333351f;
  [SerializeField]
  public FlockadeVictoryMarker[] _markers;
  public FlockadeGameBoardSide _side;
  public int _count;

  public void Configure(FlockadeGameBoardSide side) => this._side = side;

  public int Count
  {
    get => this._count;
    set
    {
      bool descending = value < this._count;
      int num1 = descending ? Mathf.Max(this._count - 1, 0) : this._count;
      int num2 = descending ? value : Mathf.Max(value - 1, 0);
      float interval = 0.0f;
      for (int index = num1; (descending ? (index >= num2 ? 1 : 0) : (index <= num2 ? 1 : 0)) != 0; index += descending ? -1 : 1)
      {
        this._markers[index].SetActive(!descending).PrependCallback((TweenCallback) (() =>
        {
          if (descending)
            return;
          AudioManager.Instance.PlayOneShot(this._side == FlockadeGameBoardSide.Left ? "event:/dlc/ui/flockade_minigame/battle_phase_end_player_goldshield_tick" : "event:/dlc/ui/flockade_minigame/battle_phase_end_opponent_goldshield_tick");
        })).PrependInterval(interval);
        interval += 0.0333333351f;
      }
      this._count = value;
    }
  }

  public int Maximum => this._markers.Length;
}
