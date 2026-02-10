// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeFight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD;
using FMOD.Studio;
using Lamb.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (RectTransform))]
public class FlockadeFight : MonoBehaviour
{
  public FlockadeFight.FlockadeFightEvent OnHit;
  public const string _BACKGROUND_DISAPPEARANCE_SOUND = "event:/dlc/ui/flockade_minigame/battle_duel_end";
  public const string _DRAW_SOUND = "event:/dlc/ui/flockade_minigame/battle_duel_result_draw";
  public const string _LEFT_SIDE_WINS_SOUND = "event:/dlc/ui/flockade_minigame/battle_duel_result_player_victory";
  public const string _RIGHT_SIDE_WINS_SOUND = "event:/dlc/ui/flockade_minigame/battle_duel_result_opponent_victory";
  public const string _SPECIAL_ANTICIPATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_guardianshepherd_activate";
  public static int _ALPHA_SHADER_PROPERTY = Shader.PropertyToID("_AlphaMultiply");
  public const float _BACKGROUND_APPEARANCE_DURATION = 0.5f;
  public const float _BACKGROUND_DISAPPEARANCE_DURATION = 0.5f;
  public const float _BACKGROUND_DURATION = 0.8333333f;
  public const float _BACKGROUND_SPECIAL_DURATION = 6.66666651f;
  public const float _BLACK_OVERLAY_SPECIAL_DISAPPEARANCE_DURATION = 1f;
  public const float _GAME_PIECE_APPEARANCE_DURATION = 0.333333343f;
  public const float _GAME_PIECE_OPACITY_DISAPPEARANCE_DURATION = 0.433333337f;
  public const float _GAME_PIECE_DISAPPEARANCE_DURATION = 0.5f;
  public const float _GAME_PIECE_DURATION = 0.75f;
  public const float _GAME_PIECE_SPECIAL_APPEARANCE_DURATION = 1f;
  public const float _GAME_PIECE_SPECIAL_ANTICIPATION_DURATION = 2.5f;
  public const float _GAME_PIECE_SPECIAL_DURATION = 2f;
  public const float _DOODLE_APPEARANCE_DURATION = 0.166666672f;
  public const float _DOODLE_DURATION = 0.5833333f;
  public const float _DOODLE_SPECIAL_DURATION = 1.666667f;
  public const float _DOODLE_DISAPPEARANCE_DURATION = 0.5f;
  public const float _BACKGROUND_APPEARING_AT = 0.0f;
  public const float _BACKGROUND_APPEARED_AT = 0.5f;
  public const float _BACKGROUND_DISAPPEARING_AT = 1.33333325f;
  public const float _BACKGROUND_DISAPPEARED_AT = 1.83333325f;
  public const float _BACKGROUND_ESPECIALLY_DISAPPEARING_AT = 7.16666651f;
  public const float _BACKGROUND_ESPECIALLY_DISAPPEARED_AT = 7.66666651f;
  public const float _BLACK_OVERLAY_ESPECIALLY_APPEARING_AT = 5.16666651f;
  public const float _BLACK_OVERLAY_ESPECIALLY_DISAPPEARING_AT = 6.16666651f;
  public const float _GAME_PIECE_APPEARING_AT = 0.25f;
  public const float _GAME_PIECE_APPEARED_AT = 0.5833334f;
  public const float _GAME_PIECE_DISAPPEARING_AT = 1.33333337f;
  public const float _GAME_PIECE_DISAPPEARED_AT = 1.83333337f;
  public const float _GAME_PIECE_ESPECIALLY_APPEARING_AT_RELATIVELY_FROM_BACKGROUND_APPEARED = 1.16666663f;
  public const float _GAME_PIECE_ESPECIALLY_APPEARING_AT = 1.66666663f;
  public const float _GAME_PIECE_ESPECIALLY_APPEARED_AT = 2.66666651f;
  public const float _GAME_PIECE_ESPECIALLY_ANTICIPATED_AT = 5.16666651f;
  public const float _GAME_PIECE_ESPECIALLY_DISAPPEARING_AT = 7.16666651f;
  public const float _GAME_PIECE_ESPECIALLY_DISAPPEARED_AT = 7.66666651f;
  public const float _NEXT_DOODLE_APPEARING_AFTER = 0.0833333358f;
  public const float _CLIMAX_AT = 0.6666667f;
  public const float _CLIMAX_ESPECIALLY_AT = 5.333333f;
  public const float _DOODLE_APPEARING_AT = 0.5833334f;
  public const float _DOODLE_APPEARED_AT = 0.75000006f;
  public const float _DOODLE_DISAPPEARING_AT = 1.33333337f;
  public const float _DOODLE_DISAPPEARED_AT = 1.83333337f;
  public const float _DOODLE_ESPECIALLY_APPEARING_AT = 5.333333f;
  public const float _DOODLE_ESPECIALLY_APPEARED_AT = 5.49999952f;
  public const float _DOODLE_ESPECIALLY_DISAPPEARING_AT = 7.16666651f;
  public const float _DOODLE_ESPECIALLY_DISAPPEARED_AT = 7.66666651f;
  public const Ease _BACKGROUND_APPEARANCE_EASING = Ease.OutCubic;
  public const Ease _BACKGROUND_EASING = Ease.Linear;
  public const Ease _BACKGROUND_DISAPPEARANCE_EASING = Ease.InCubic;
  public const Ease _BLACK_OVERLAY_SPECIAL_DISAPPEARANCE_EASING = Ease.InCubic;
  public const Ease _GAME_PIECE_APPEARANCE_EASING = Ease.OutCirc;
  public const Ease _GAME_PIECE_EASING = Ease.Linear;
  public const Ease _GAME_PIECE_SPECIAL_APPEARANCE_EASING = Ease.Linear;
  public const Ease _GAME_PIECE_SPECIAL_EASING = Ease.InCubic;
  public const Ease _GAME_PIECE_OPACITY_DISAPPEARANCE_EASING = Ease.InSine;
  public const Ease _GAME_PIECE_DISAPPEARANCE_EASING = Ease.InCirc;
  public const Ease _DOODLE_APPEARANCE_EASING = Ease.OutCubic;
  public const Ease _DOODLE_EASING = Ease.Linear;
  public const Ease _DOODLE_DISAPPEARANCE_EASING = Ease.InCirc;
  public const float _BLACK_OVERLAY_SPECIAL_OPACITY = 0.6f;
  public const float _BLACK_OVERLAY_SPECIAL_DISAPPEARED_OPACITY = 0.0f;
  public const float _BACKGROUND_APPEARING_OPACITY = 0.0f;
  public const float _BACKGROUND_APPEARING_SCALE = 0.0f;
  public const float _BACKGROUND_APPEARING_TILT_ANGLE = -120f;
  public const float _BACKGROUND_APPEARED_SCALE = 0.98f;
  public const float _BACKGROUND_APPEARED_TILT_ANGLE = -3f;
  public const float _BACKGROUND_OPACITY = 1f;
  public const float _BACKGROUND_SCALE = 1f;
  public const float _BACKGROUND_TILT_ANGLE = 0.0f;
  public const float _BACKGROUND_DISAPPEARED_OPACITY = 0.0f;
  public const float _BACKGROUND_DISAPPEARED_SCALE = 0.0f;
  public const float _BACKGROUND_DISAPPEARED_TILT_ANGLE = 120f;
  public const float _CONTAINER_TILT_ANGLE = 15f;
  public const float _CONTAINER_TILT_ANGLE_DEFAULT_MULTIPLIER = 0.0f;
  public const float _CONTAINER_TILT_ANGLE_LOSING_MULTIPLIER = 1f;
  public const float _CONTAINER_TILT_ANGLE_WINNING_MULTIPLIER = -1f;
  public const float _DOODLE_APPEARING_SCALE = 0.25f;
  public const float _DOODLE_APPEARED_SCALE = 0.97f;
  public const float _DOODLE_SCALE = 1f;
  public const float _DOODLE_DISAPPEARED_SCALE = 0.0f;
  public const float _GAME_PIECE_APPEARING_OPACITY = 0.0f;
  public const float _GAME_PIECE_APPEARING_X_POSITION_MULTIPLIER = 2f;
  public const float _GAME_PIECE_LOSING_APPEARING_SCALE = 1.3f;
  public const float _GAME_PIECE_TIE_APPEARING_SCALE = 1.3f;
  public const float _GAME_PIECE_WINNING_APPEARING_SCALE = 0.8f;
  public const float _GAME_PIECE_APPEARED_X_POSITION_MULTIPLIER = 1f;
  public const float _GAME_PIECE_LOSING_APPEARED_SCALE = 0.83f;
  public const float _GAME_PIECE_TIE_APPEARED_SCALE = 0.93f;
  public const float _GAME_PIECE_WINNING_APPEARED_SCALE = 1.27f;
  public static Vector3 _GAME_PIECE_SPECIAL_SCALE = new Vector3(1f, 1f, 1f);
  public const float _GAME_PIECE_SPECIAL_ANTICIPATION_SHAKE_RANDOMNESS = 45f;
  public const float _GAME_PIECE_SPECIAL_ANTICIPATION_SHAKE_STRENGTH = 10f;
  public const int _GAME_PIECE_SPECIAL_ANTICIPATION_SHAKE_VIBRATO = 15;
  public const float _GAME_PIECE_DEFAULT_OPACITY = 1f;
  public const float _GAME_PIECE_LOSING_OPACITY = 0.5f;
  public const float _GAME_PIECE_LOSING_SCALE = 0.8f;
  public const float _GAME_PIECE_TIE_SCALE = 0.9f;
  public const float _GAME_PIECE_WINNING_SCALE = 1.3f;
  public const float _GAME_PIECE_X_POSITION_MULTIPLIER = 0.95f;
  public const float _GAME_PIECE_DISAPPEARED_OPACITY = 0.0f;
  public const float _GAME_PIECE_DISAPPEARED_SCALE = 0.0f;
  public const float _GAME_PIECE_DISAPPEARED_X_POSITION_MULTIPLIER = -1f;
  [SerializeField]
  public MMUIRadialGraphic _background;
  [SerializeField]
  public CanvasGroup _blackOverlay;
  [SerializeField]
  public FlockadeGamePiece _centerGamePiece;
  [SerializeField]
  public FlockadeFight.DoodleSlot[] _centerGamePieceDoodleSlots;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public Color _doodleColor;
  [SerializeField]
  public FlockadeGamePiece _leftGamePiece;
  [SerializeField]
  public FlockadeFight.DoodleSlot[] _leftGamePieceDoodleSlots;
  [SerializeField]
  public FlockadeGamePiece _rightGamePiece;
  [SerializeField]
  public FlockadeFight.DoodleSlot[] _rightGamePieceDoodleSlots;
  [SerializeField]
  public FlockadeFight.Doodle[] _tieDoodles;
  public RectTransform _backgroundRectTransform;
  public Dictionary<FlockadeFight.GamePiecePosition, Dictionary<FlockadeFight.DoodlePosition, FlockadeFight.DoodleSlot>> _doodleSlots;
  public Dictionary<FlockadeGamePiece, Vector2> _originPositions;
  public float _originOuterRadius;
  public float _originRadius;
  public RectTransform _rectTransform;

  public virtual void Awake()
  {
    this._rectTransform = this.GetComponent<RectTransform>();
    this._backgroundRectTransform = this._background.GetComponent<RectTransform>();
    this._doodleSlots = new Dictionary<FlockadeFight.GamePiecePosition, Dictionary<FlockadeFight.DoodlePosition, FlockadeFight.DoodleSlot>>()
    {
      {
        FlockadeFight.GamePiecePosition.Left,
        ((IEnumerable<FlockadeFight.DoodleSlot>) this._leftGamePieceDoodleSlots).ToDictionary<FlockadeFight.DoodleSlot, FlockadeFight.DoodlePosition>((Func<FlockadeFight.DoodleSlot, FlockadeFight.DoodlePosition>) (slot => slot.Position))
      },
      {
        FlockadeFight.GamePiecePosition.Right,
        ((IEnumerable<FlockadeFight.DoodleSlot>) this._rightGamePieceDoodleSlots).ToDictionary<FlockadeFight.DoodleSlot, FlockadeFight.DoodlePosition>((Func<FlockadeFight.DoodleSlot, FlockadeFight.DoodlePosition>) (slot => slot.Position))
      },
      {
        FlockadeFight.GamePiecePosition.Center,
        ((IEnumerable<FlockadeFight.DoodleSlot>) this._centerGamePieceDoodleSlots).ToDictionary<FlockadeFight.DoodleSlot, FlockadeFight.DoodlePosition>((Func<FlockadeFight.DoodleSlot, FlockadeFight.DoodlePosition>) (slot => slot.Position))
      }
    };
    this._originPositions = new Dictionary<FlockadeGamePiece, Vector2>()
    {
      {
        this._leftGamePiece,
        this._leftGamePiece.RectTransform.anchoredPosition
      },
      {
        this._rightGamePiece,
        this._rightGamePiece.RectTransform.anchoredPosition
      },
      {
        this._centerGamePiece,
        this._centerGamePiece.RectTransform.anchoredPosition
      }
    };
    this._originOuterRadius = this._background.OuterRadius;
    this._originRadius = this._background.Radius;
    this._background.material = new Material(this._background.material);
    this._background.material.SetFloat(FlockadeFight._ALPHA_SHADER_PROPERTY, 0.0f);
    this._background.Radius = this._background.OuterRadius = 0.0f;
    this._leftGamePiece.Configure();
    this._rightGamePiece.Configure();
    this._centerGamePiece.Configure();
  }

  public DG.Tweening.Sequence Animate(
    FlockadeGameBoard gameBoard,
    IFlockadeGamePiece.State leftGamePiece,
    IFlockadeGamePiece.State rightGamePiece,
    FlockadeFight.Result resolution,
    float y,
    DG.Tweening.Sequence onClimax = null)
  {
    bool flag1 = resolution == FlockadeFight.Result.WinAndDuelPhaseEnd || resolution == FlockadeFight.Result.DefeatAndDuelPhaseEnd;
    DG.Tweening.Sequence s1 = DOTween.Sequence().InsertCallback(0.0f, (TweenCallback) (() =>
    {
      RectTransform rectTransform = this._rectTransform;
      rectTransform.localPosition = rectTransform.localPosition + y * Vector3.up;
      RectTransform container = this._container;
      float num;
      switch (resolution)
      {
        case FlockadeFight.Result.Defeat:
          num = 1f;
          break;
        case FlockadeFight.Result.Win:
          num = -1f;
          break;
        default:
          num = 0.0f;
          break;
      }
      container.localRotation = Quaternion.Euler(0.0f, 0.0f, num * 15f);
    })).Insert(0.0f, (Tween) DOVirtual.Float(0.0f, 0.98f, 0.5f, new TweenCallback<float>(this.UpdateBackgroundScale)).SetEase<Tweener>(Ease.OutCubic)).Insert(0.0f, (Tween) this._backgroundRectTransform.DORotate(new Vector3(0.0f, 0.0f, -3f), 0.5f).From<Quaternion, Vector3, QuaternionOptions>(new Vector3(0.0f, 0.0f, -120f)).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutCubic)).Insert(0.0f, (Tween) DOVirtual.Float(0.0f, 1f, 0.5f, new TweenCallback<float>(this.UpdateBackgroundOpacity)).SetEase<Tweener>(Ease.OutCubic));
    float duration1 = flag1 ? 6.66666651f : 0.8333333f;
    s1.Insert(0.5f, (Tween) DOVirtual.Float(0.98f, 1f, duration1, new TweenCallback<float>(this.UpdateBackgroundScale)).SetEase<Tweener>(Ease.Linear)).Insert(0.5f, (Tween) this._backgroundRectTransform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), duration1).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.Linear));
    float atPosition1 = flag1 ? 7.16666651f : 1.33333325f;
    float atPosition2 = flag1 ? 7.66666651f : 1.83333325f;
    s1.InsertCallback(atPosition1, (TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_duel_end"))).Insert(atPosition1, (Tween) DOVirtual.Float(1f, 0.0f, 0.5f, new TweenCallback<float>(this.UpdateBackgroundScale)).SetEase<Tweener>(Ease.InCubic)).Insert(atPosition1, (Tween) this._backgroundRectTransform.DORotate(new Vector3(0.0f, 0.0f, 120f), 0.5f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InCubic)).Insert(atPosition1, (Tween) DOVirtual.Float(1f, 0.0f, 0.5f, new TweenCallback<float>(this.UpdateBackgroundOpacity)).SetEase<Tweener>(Ease.InCubic)).InsertCallback(atPosition2, (TweenCallback) (() =>
    {
      this._container.localRotation = Quaternion.identity;
      RectTransform rectTransform = this._rectTransform;
      rectTransform.localPosition = rectTransform.localPosition - y * Vector3.up;
    }));
    float atPosition3 = flag1 ? 1.66666663f : 0.25f;
    float duration2 = flag1 ? 1f : 0.333333343f;
    s1.InsertCallback(atPosition3, (TweenCallback) (() =>
    {
      switch (resolution)
      {
        case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
          this._centerGamePiece.Set(rightGamePiece);
          break;
        case FlockadeFight.Result.WinAndDuelPhaseEnd:
          this._centerGamePiece.Set(leftGamePiece);
          break;
        default:
          this._leftGamePiece.Set(leftGamePiece);
          this._rightGamePiece.Set(rightGamePiece);
          break;
      }
      FlockadeFight.SetStancesAndPlayAttackSounds(this._leftGamePiece, this._rightGamePiece, this._centerGamePiece, resolution);
    }));
    FlockadeGamePiece[] flockadeGamePieceArray1;
    if (!flag1)
      flockadeGamePieceArray1 = new FlockadeGamePiece[2]
      {
        this._leftGamePiece,
        this._rightGamePiece
      };
    else
      flockadeGamePieceArray1 = new FlockadeGamePiece[1]
      {
        this._centerGamePiece
      };
    foreach (FlockadeGamePiece flockadeGamePiece in flockadeGamePieceArray1)
    {
      FlockadeGamePiece gamePiece = flockadeGamePiece;
      FlockadeFight.Result result = (UnityEngine.Object) gamePiece == (UnityEngine.Object) this._rightGamePiece ? resolution.Reverse() : resolution;
      DG.Tweening.Sequence s2 = s1;
      float num = atPosition3;
      Image image = gamePiece.Image;
      float endValue1 = result != FlockadeFight.Result.Defeat ? 1f : 0.5f;
      double atPosition4 = (double) num;
      TweenerCore<Color, Color, ColorOptions> t1 = DOTweenModuleUI.DOFade(image, endValue1, duration2).From(0.0f).SetEase<TweenerCore<Color, Color, ColorOptions>>(flag1 ? Ease.Linear : Ease.OutCirc);
      s2.Insert((float) atPosition4, (Tween) t1);
      if (flag1)
      {
        s1.InsertCallback(atPosition3, (TweenCallback) (() => gamePiece.RectTransform.localScale = FlockadeFight._GAME_PIECE_SPECIAL_SCALE));
        break;
      }
      DG.Tweening.Sequence s3 = s1;
      float atPosition5 = atPosition3;
      Transform rectTransform = (Transform) gamePiece.RectTransform;
      float endValue2;
      switch (result)
      {
        case FlockadeFight.Result.Defeat:
          endValue2 = 0.83f;
          break;
        case FlockadeFight.Result.Win:
          endValue2 = 1.27f;
          break;
        default:
          endValue2 = 0.93f;
          break;
      }
      TweenerCore<Vector3, Vector3, VectorOptions> t2 = rectTransform.DOScale(endValue2, duration2);
      float fromValue;
      switch (result)
      {
        case FlockadeFight.Result.Defeat:
          fromValue = 1.3f;
          break;
        case FlockadeFight.Result.Win:
          fromValue = 0.8f;
          break;
        default:
          fromValue = 1.3f;
          break;
      }
      s3.Insert(atPosition5, (Tween) t2.From(fromValue).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc)).Insert(atPosition3, (Tween) gamePiece.RectTransform.DOAnchorPosX(this._originPositions[gamePiece].x * 1f, duration2).From<Vector2, Vector2, VectorOptions>(new Vector2(this._originPositions[gamePiece].x * 2f, gamePiece.RectTransform.anchoredPosition.y)).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc));
    }
    float atPosition6 = flag1 ? 2.66666651f : 0.5833334f;
    if (flag1)
    {
      Vector3 offset = Vector3.zero;
      s1.InsertCallback(atPosition6, (TweenCallback) (() =>
      {
        EventInstance eventInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/dlc/ui/flockade_minigame/piece_blessing_guardianshepherd_activate", (Transform) null);
        AudioManager.Instance.SetEventInstanceParameter(eventInstance, SoundParams.IsFlockadeFastForwardOn, gameBoard.IsFastForwarding ? 1f : 0.0f);
        gameBoard.FastForwarding += (Action<bool>) (isFastForwarding => AudioManager.Instance.SetEventInstanceParameter(eventInstance, SoundParams.IsFlockadeFastForwardOn, isFastForwarding ? 1f : 0.0f));
        int num = (int) eventInstance.setCallback((EVENT_CALLBACK) ((type, eventPtr, parameterPtr) =>
        {
          if (type == EVENT_CALLBACK_TYPE.STOPPED)
            gameBoard.FastForwarding -= (Action<bool>) (isFastForwarding => AudioManager.Instance.SetEventInstanceParameter(eventInstance, SoundParams.IsFlockadeFastForwardOn, isFastForwarding ? 1f : 0.0f));
          return RESULT.OK;
        }), EVENT_CALLBACK_TYPE.STOPPED);
        DOTween.Shake((DOGetter<Vector3>) (() => offset), (DOSetter<Vector3>) (value => offset = value), 2.5f, 10f, 15, 45f, fadeOut: false);
      })).Insert(atPosition6, (Tween) DOVirtual.Float(0.0f, 1f, 2.5f, (TweenCallback<float>) (progress => this._centerGamePiece.RectTransform.anchoredPosition = this._originPositions[this._centerGamePiece] + (Vector2) (progress * offset))).SetEase<Tweener>(Ease.InCubic)).InsertCallback(5.16666651f, (TweenCallback) (() =>
      {
        this._centerGamePiece.RectTransform.anchoredPosition = this._originPositions[this._centerGamePiece];
        this._centerGamePiece.Set(this._centerGamePiece.Configuration);
        this._centerGamePiece.SetStance(FlockadeGamePiece.Stance.Attacking);
      })).InsertCallback(5.16666651f, (TweenCallback) (() => this._blackOverlay.alpha = 0.6f)).Insert(6.16666651f, (Tween) this._blackOverlay.DOFade(0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic));
    }
    else
    {
      FlockadeGamePiece[] flockadeGamePieceArray2 = new FlockadeGamePiece[2]
      {
        this._leftGamePiece,
        this._rightGamePiece
      };
      foreach (FlockadeGamePiece key in flockadeGamePieceArray2)
      {
        FlockadeFight.Result result = (UnityEngine.Object) key == (UnityEngine.Object) this._rightGamePiece ? resolution.Reverse() : resolution;
        DG.Tweening.Sequence s4 = s1;
        float atPosition7 = atPosition6;
        Transform rectTransform = (Transform) key.RectTransform;
        float endValue;
        switch (result)
        {
          case FlockadeFight.Result.Defeat:
            endValue = 0.8f;
            break;
          case FlockadeFight.Result.Win:
            endValue = 1.3f;
            break;
          default:
            endValue = 0.9f;
            break;
        }
        s4.Insert(atPosition7, (Tween) rectTransform.DOScale(endValue, 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear)).Insert(atPosition6, (Tween) key.RectTransform.DOAnchorPosX(this._originPositions[key].x * 0.95f, 0.75f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.Linear)).Insert(atPosition6, DOVirtual.DelayedCall(0.0f, (TweenCallback) (() =>
        {
          FlockadeFight.FlockadeFightEvent onHit = this.OnHit;
          if (onHit == null)
            return;
          onHit();
        })));
      }
    }
    float atPosition8 = flag1 ? 7.16666651f : 1.33333337f;
    FlockadeGamePiece[] flockadeGamePieceArray3;
    if (!flag1)
      flockadeGamePieceArray3 = new FlockadeGamePiece[2]
      {
        this._leftGamePiece,
        this._rightGamePiece
      };
    else
      flockadeGamePieceArray3 = new FlockadeGamePiece[1]
      {
        this._centerGamePiece
      };
    foreach (FlockadeGamePiece key in flockadeGamePieceArray3)
    {
      s1.Insert(atPosition8, (Tween) DOTweenModuleUI.DOFade(key.Image, 0.0f, 0.433333337f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InSine)).Insert(atPosition8, (Tween) key.RectTransform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCirc));
      if (!flag1)
        s1.Insert(atPosition8, (Tween) key.RectTransform.DOAnchorPosX(this._originPositions[key].x * -1f, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InCirc));
      else
        break;
    }
    float atPosition9 = flag1 ? 7.66666651f : 1.83333337f;
    s1.InsertCallback(atPosition9, (TweenCallback) (() =>
    {
      this._leftGamePiece.Pop();
      this._rightGamePiece.Pop();
      this._centerGamePiece.Pop();
    }));
    if (onClimax != null)
    {
      float atPosition10 = flag1 ? 5.333333f : 0.6666667f;
      s1.Insert(atPosition10, (Tween) onClimax);
    }
    float atPosition11 = flag1 ? 5.333333f : 0.5833334f;
    s1.InsertCallback(atPosition11, (TweenCallback) (() => FlockadeFight.PlayResolutionSound(resolution)));
    FlockadeFight.GamePiecePosition onto;
    FlockadeFight.Doodle[] doodles = this.GetDoodles(leftGamePiece, rightGamePiece, resolution, out onto);
    for (int index = 0; index < doodles.Length; ++index)
    {
      FlockadeFight.Doodle doodle = doodles[index];
      bool flag2 = doodle.Position >= FlockadeFight.DoodlePosition.AboveOpponent;
      FlockadeFight.DoodlePosition key = flag2 ? doodle.Position - 5 : doodle.Position;
      bool flipX = flag2 ? !doodle.FlipX : doodle.FlipX;
      FlockadeFight.DoodleSlot doodleSlot = this._doodleSlots[flag2 ? onto.GetOpposing() : onto][key];
      float atPosition12 = atPosition11 + (float) index * 0.0833333358f;
      if (!string.IsNullOrEmpty(doodle.Sound))
        s1.InsertCallback(atPosition12, (TweenCallback) (() => AudioManager.Instance.PlayOneShot(doodle.Sound)));
      if (doodle.UseCustomAnimation)
      {
        FlockadeAnimation flockadeAnimation = UnityEngine.Object.Instantiate<FlockadeAnimation>(doodle.CustomAnimation, doodleSlot.RectTransform.parent);
        flockadeAnimation.Flip = (flipX, doodle.FlipY);
        s1.Insert(atPosition12, (Tween) flockadeAnimation.Play());
      }
      else
      {
        s1.InsertCallback(atPosition12, (TweenCallback) (() => doodleSlot.Image.sprite = doodle.Image)).Insert(atPosition12, (Tween) DOTweenModuleUI.DOColor(doodleSlot.Image, this._doodleColor, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic)).Insert(atPosition12, (Tween) doodleSlot.RectTransform.DOScale(FlockadeFight.GetScale(0.97f, flipX, doodle.FlipY), 0.166666672f).From<Vector3, Vector3, VectorOptions>(FlockadeFight.GetScale(0.25f, flipX, doodle.FlipY)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic));
        if (doodle.UseShowAddAnimation)
          s1.Insert(atPosition12, doodle.ShowAddAnimation.GetAddAnimation(doodleSlot.RectTransform, FlockadeFight.GetScale(1f, flipX, doodle.FlipY), 0.166666672f, Ease.OutCubic));
        float atPosition13 = flag1 ? 5.49999952f : 0.75000006f;
        float duration3 = flag1 ? 1.666667f : 0.5833333f;
        s1.Insert(atPosition13, (Tween) doodleSlot.RectTransform.DOScale(FlockadeFight.GetScale(1f, flipX, doodle.FlipY), duration3).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear));
        if (doodle.UseUpdateAddAnimation)
          s1.Insert(atPosition13, doodle.UpdateAddAnimation.GetAddAnimation(doodleSlot.RectTransform, FlockadeFight.GetScale(1f, flipX, doodle.FlipY), duration3, Ease.Linear));
        float atPosition14 = flag1 ? 7.16666651f : 1.33333337f;
        float atPosition15 = flag1 ? 7.66666651f : 1.83333337f;
        s1.Insert(atPosition14, (Tween) DOTweenModuleUI.DOColor(doodleSlot.Image, Color.clear, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InCirc)).Insert(atPosition14, (Tween) doodleSlot.RectTransform.DOScale(FlockadeFight.GetScale(0.0f, flipX, doodle.FlipY), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCirc)).InsertCallback(atPosition15, (TweenCallback) (() => doodleSlot.Image.sprite = (Sprite) null));
        if (doodle.UseHideAddAnimation)
          s1.Insert(atPosition14, doodle.HideAddAnimation.GetAddAnimation(doodleSlot.RectTransform, FlockadeFight.GetScale(1f, flipX, doodle.FlipY), 0.5f, Ease.InCirc));
        if (doodle.UseShowAddAnimation)
          s1.Insert(atPosition14 + 0.5f, doodle.ShowAddAnimation.ResetAnimationChanges(doodleSlot.RectTransform));
        if (doodle.UseUpdateAddAnimation)
          s1.Insert(atPosition14 + 0.5f, doodle.UpdateAddAnimation.ResetAnimationChanges(doodleSlot.RectTransform));
        if (doodle.UseHideAddAnimation)
          s1.Insert(atPosition14 + 0.5f, doodle.HideAddAnimation.ResetAnimationChanges(doodleSlot.RectTransform));
      }
    }
    return s1;
  }

  public static void SetStancesAndPlayAttackSounds(
    FlockadeGamePiece left,
    FlockadeGamePiece right,
    FlockadeGamePiece center,
    FlockadeFight.Result resolution)
  {
    switch (resolution)
    {
      case FlockadeFight.Result.Tie:
        left.SetStance(FlockadeGamePiece.Stance.Flinching);
        right.SetStance(FlockadeGamePiece.Stance.Flinching);
        AudioManager.Instance.PlayOneShot(left.Configuration.BaseConfiguration.AttackingSound);
        break;
      case FlockadeFight.Result.Defeat:
        left.SetStance(FlockadeGamePiece.Stance.Flinching);
        right.SetStance(FlockadeGamePiece.Stance.Attacking);
        AudioManager.Instance.PlayOneShot(right.Configuration.BaseConfiguration.AttackingSound);
        break;
      case FlockadeFight.Result.Win:
        left.SetStance(FlockadeGamePiece.Stance.Attacking);
        right.SetStance(FlockadeGamePiece.Stance.Flinching);
        AudioManager.Instance.PlayOneShot(left.Configuration.BaseConfiguration.AttackingSound);
        break;
    }
  }

  public static void PlayResolutionSound(FlockadeFight.Result resolution)
  {
    switch (resolution)
    {
      case FlockadeFight.Result.Defeat:
      case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
        AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_duel_result_opponent_victory");
        break;
      case FlockadeFight.Result.Win:
      case FlockadeFight.Result.WinAndDuelPhaseEnd:
        AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_duel_result_player_victory");
        break;
      default:
        AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_duel_result_draw");
        break;
    }
  }

  public FlockadeFight.Doodle[] GetDoodles(
    IFlockadeGamePiece.State leftGamePiece,
    IFlockadeGamePiece.State rightGamePiece,
    FlockadeFight.Result resolution,
    out FlockadeFight.GamePiecePosition onto)
  {
    FlockadeFight.GamePiecePosition gamePiecePosition;
    switch (resolution)
    {
      case FlockadeFight.Result.Defeat:
        gamePiecePosition = FlockadeFight.GamePiecePosition.Right;
        break;
      case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
      case FlockadeFight.Result.WinAndDuelPhaseEnd:
        gamePiecePosition = FlockadeFight.GamePiecePosition.Center;
        break;
      default:
        gamePiecePosition = FlockadeFight.GamePiecePosition.Left;
        break;
    }
    onto = gamePiecePosition;
    FlockadeFight.Doodle[] doodles;
    switch (resolution)
    {
      case FlockadeFight.Result.Defeat:
      case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
        doodles = ((IEnumerable<FlockadeFight.Doodle>) rightGamePiece.Configuration.BaseConfiguration.AttackDoodles).Concat<FlockadeFight.Doodle>(rightGamePiece.Blessing.Active ? (IEnumerable<FlockadeFight.Doodle>) rightGamePiece.Configuration.BlessingConfiguration.AttackDoodles : (IEnumerable<FlockadeFight.Doodle>) Array.Empty<FlockadeFight.Doodle>()).ToArray<FlockadeFight.Doodle>();
        break;
      case FlockadeFight.Result.Win:
      case FlockadeFight.Result.WinAndDuelPhaseEnd:
        doodles = ((IEnumerable<FlockadeFight.Doodle>) leftGamePiece.Configuration.BaseConfiguration.AttackDoodles).Concat<FlockadeFight.Doodle>(leftGamePiece.Blessing.Active ? (IEnumerable<FlockadeFight.Doodle>) leftGamePiece.Configuration.BlessingConfiguration.AttackDoodles : (IEnumerable<FlockadeFight.Doodle>) Array.Empty<FlockadeFight.Doodle>()).ToArray<FlockadeFight.Doodle>();
        break;
      default:
        doodles = this._tieDoodles;
        break;
    }
    return doodles;
  }

  public static Vector3 GetScale(float scale, bool flipX, bool flipY)
  {
    return new Vector3(scale * (flipX ? -1f : 1f), scale * (flipY ? -1f : 1f), scale);
  }

  public void UpdateBackgroundOpacity(float progress)
  {
    this._background.material.SetFloat(FlockadeFight._ALPHA_SHADER_PROPERTY, progress);
  }

  public void UpdateBackgroundScale(float progress)
  {
    this._background.Radius = progress * this._originRadius;
    this._background.OuterRadius = progress * this._originOuterRadius;
  }

  public delegate void FlockadeFightEvent();

  public enum Result
  {
    Unknown,
    Tie,
    Defeat,
    Win,
    DefeatAndDuelPhaseEnd,
    WinAndDuelPhaseEnd,
  }

  public enum DoodlePosition
  {
    Above,
    Behind,
    Over,
    AboveWeapon,
    UnderWeapon,
    AboveOpponent,
    BehindOpponent,
    OverOpponent,
    AboveOpponentWeapon,
    UnderOpponentWeapon,
  }

  [Serializable]
  public class Doodle
  {
    public bool UseCustomAnimation;
    public FlockadeAnimation CustomAnimation;
    public Sprite Image;
    public bool UseShowAddAnimation;
    public FlockadeAddAnimation ShowAddAnimation;
    public bool UseUpdateAddAnimation;
    public FlockadeAddAnimation UpdateAddAnimation;
    public bool UseHideAddAnimation;
    public FlockadeAddAnimation HideAddAnimation;
    public FlockadeFight.DoodlePosition Position;
    public bool FlipX;
    public bool FlipY;
    public string Sound;
  }

  [Serializable]
  public class DoodleSlot
  {
    public FlockadeFight.DoodlePosition Position;
    public RectTransform RectTransform;
    public Image Image;
  }

  public enum GamePiecePosition
  {
    Left,
    Right,
    Center,
  }
}
