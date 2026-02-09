// Decompiled with JetBrains decompiler
// Type: CameraTools
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public static class CameraTools
{
  public static Com.LuisPedroFonseca.ProCamera2D.ProCamera2D _pro_cam;
  public static ProCamera2DCinematics _cinematics;
  public static List<Transform> _cinematic_targets = new List<Transform>();
  public static ProCamera2DTransitionsFX _transitions_fx;
  public static ProCamera2DLetterbox _letterbox;
  public static ProCamera2DNumericBoundaries _boundaries;
  public static Transform _tf;
  public static bool _cached;
  public static bool _playing_transition;
  public static List<CameraTarget> _stored_camera_targets = new List<CameraTarget>();
  public static bool _camera_flyed_to_target = false;

  public static Com.LuisPedroFonseca.ProCamera2D.ProCamera2D pro_cam
  {
    get
    {
      if (!CameraTools._cached)
        CameraTools.CacheComponents();
      return CameraTools._pro_cam;
    }
  }

  public static Transform tf
  {
    get
    {
      if (!CameraTools._cached)
        CameraTools.CacheComponents();
      return CameraTools._tf;
    }
  }

  public static ProCamera2DCinematics cinematics
  {
    get
    {
      if (!CameraTools._cached)
        CameraTools.CacheComponents();
      return CameraTools._cinematics;
    }
  }

  public static ProCamera2DTransitionsFX transitions_fx
  {
    get
    {
      if (!CameraTools._cached)
        CameraTools.CacheComponents();
      return CameraTools._transitions_fx;
    }
  }

  public static ProCamera2DLetterbox letterbox
  {
    get
    {
      if (!CameraTools._cached)
        CameraTools.CacheComponents();
      return CameraTools._letterbox;
    }
  }

  public static ProCamera2DNumericBoundaries boundaries
  {
    get
    {
      if (!CameraTools._cached)
        CameraTools.CacheComponents();
      return CameraTools._boundaries;
    }
  }

  public static void ReCache()
  {
    CameraTools._cached = false;
    CameraTools.CacheComponents();
  }

  public static void CacheComponents()
  {
    CameraTools._pro_cam = MainGame.me.GetComponent<Com.LuisPedroFonseca.ProCamera2D.ProCamera2D>();
    CameraTools._tf = CameraTools._pro_cam.transform;
    CameraTools._cinematics = CameraTools._tf.GetComponent<ProCamera2DCinematics>();
    CameraTools._transitions_fx = CameraTools._tf.GetComponent<ProCamera2DTransitionsFX>();
    CameraTools._letterbox = CameraTools._tf.GetComponent<ProCamera2DLetterbox>() ?? CameraTools._tf.gameObject.AddComponent<ProCamera2DLetterbox>();
    CameraTools._boundaries = CameraTools._tf.GetComponent<ProCamera2DNumericBoundaries>();
    CameraTools._cached = true;
  }

  public static void PlayCinematics(
    WorldGameObject obj,
    float ease_in_duration = 1f,
    float hold_duration = 1f,
    GJCommons.VoidDelegate on_ended = null)
  {
    CameraTools.PlayCinematics(obj.tf, ease_in_duration, hold_duration, on_ended);
  }

  public static void PlayCinematics(
    Transform tf,
    float ease_in_duration = 1f,
    float hold_duration = 1f,
    GJCommons.VoidDelegate on_ended = null)
  {
    if (CameraTools.cinematics.IsPlaying)
    {
      Debug.LogError((object) "Cinematics already playing!");
      on_ended.TryInvoke();
    }
    else
    {
      if (!CameraTools._cinematic_targets.Contains(tf))
      {
        CameraTools._cinematic_targets.Add(tf);
        CameraTools.cinematics.AddCinematicTarget(tf, ease_in_duration, hold_duration);
      }
      CameraTools.cinematics.OnCinematicFinished = new UnityEvent();
      CameraTools.cinematics.OnCinematicFinished.AddListener((UnityAction) (() =>
      {
        CameraTools.StopCinematics();
        on_ended.TryInvoke();
      }));
      CameraTools.cinematics.Play();
    }
  }

  public static void StopCinematics()
  {
    foreach (Transform cinematicTarget in CameraTools._cinematic_targets)
      CameraTools.cinematics.RemoveCinematicTarget(cinematicTarget);
    CameraTools._cinematic_targets.Clear();
    CameraTools.cinematics.Stop();
  }

  public static void PlayFade(
    bool fade_in,
    GJCommons.VoidDelegate on_complete = null,
    float? time = null,
    Color? color = null)
  {
    if (CameraTools._playing_transition)
    {
      Debug.LogError((object) "Can't PlayFade: Cinematics already playing!");
      on_complete.TryInvoke();
    }
    else if ((UnityEngine.Object) CameraTools.transitions_fx == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "null transition fx!");
    }
    else
    {
      CameraTools._playing_transition = true;
      Color color1 = color ?? Color.black;
      if (fade_in)
      {
        CameraTools.transitions_fx.BackgroundColorExit = color1;
        CameraTools.transitions_fx.UpdateTransitionsColor();
        CameraTools.transitions_fx.TransitionExit(time);
        CameraTools.transitions_fx.OnTransitionExitEnded = (System.Action) (() =>
        {
          CameraTools._playing_transition = false;
          on_complete.TryInvoke();
        });
      }
      else
      {
        CameraTools.transitions_fx.BackgroundColorEnter = color1;
        CameraTools.transitions_fx.UpdateTransitionsColor();
        CameraTools.transitions_fx.TransitionEnter(time);
        CameraTools.transitions_fx.OnTransitionEnterEnded = (System.Action) (() =>
        {
          CameraTools._playing_transition = false;
          on_complete.TryInvoke();
        });
      }
    }
  }

  public static void TweenLetterbox(bool show)
  {
    Debug.Log((object) ("Camera letterbox = " + show.ToString()));
    CameraTools.letterbox.TweenTo(show ? CameraTools.cinematics.LetterboxAmount : 0.0f, CameraTools.cinematics.LetterboxAnimDuration);
  }

  public static void Fade(GJCommons.VoidDelegate on_complete = null, float? time = null, Color? color = null)
  {
    CameraTools.PlayFade(true, on_complete, time, color);
  }

  public static void UnFade(GJCommons.VoidDelegate on_complete = null, float? time = null, Color? color = null)
  {
    CameraTools.PlayFade(false, on_complete, time, color);
  }

  public static void MoveToPos(Vector2 pos)
  {
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.MoveCameraInstantlyToPosition(pos);
  }

  public static void StoreCameraTargets()
  {
    CameraTools._stored_camera_targets = new List<CameraTarget>((IEnumerable<CameraTarget>) CameraTools.pro_cam.CameraTargets);
    Debug.Log((object) ("Store camera targets: Count = " + CameraTools._stored_camera_targets.Count.ToString()));
  }

  public static void RestoreCameraTargets(float duration = 0.7f)
  {
    if (CameraTools._stored_camera_targets == null || CameraTools._stored_camera_targets.Count == 0)
    {
      CameraTools.AddToCameraTargets(MainGame.me.player.transform, duration: duration);
    }
    else
    {
      CameraTools.pro_cam.CameraTargets = CameraTools._stored_camera_targets;
      if (CameraTools.pro_cam.CameraTargets.Count > 0)
        CameraTools.pro_cam.CameraTargets[0].TargetInfluence = 1f;
    }
    CameraTools._stored_camera_targets = new List<CameraTarget>();
    Debug.Log((object) ("Restore camera targets.Count = " + CameraTools.pro_cam.CameraTargets.Count.ToString()));
  }

  public static void AddToCameraTargets(Transform transform, bool remove_others = true, float duration = 0.7f)
  {
    if (remove_others)
    {
      while (CameraTools.pro_cam.CameraTargets.Count > 0)
        CameraTools.pro_cam.CameraTargets.RemoveAt(0);
    }
    if ((double) Mathf.Abs(duration) < 0.01)
      CameraTools.MoveToPos((Vector2) transform.position);
    CameraTools.pro_cam.AddCameraTarget(transform, 10000f, 10000f, duration);
  }

  public static void CameraCinematicFly(
    Transform transform,
    GJCommons.VoidDelegate on_finished,
    float duration = 0.7f)
  {
    if (!CameraTools._camera_flyed_to_target)
    {
      CameraTools._camera_flyed_to_target = true;
      CameraTools.StoreCameraTargets();
    }
    CameraTools.pro_cam.CameraTargets[0].TargetInfluence = 1f;
    CameraTarget cameraTarget = new CameraTarget()
    {
      TargetTransform = transform,
      TargetInfluenceH = 0.0f,
      TargetInfluenceV = 0.0f,
      TargetOffset = Vector2.zero
    };
    CameraTools.pro_cam.CameraTargets.Add(cameraTarget);
    CameraTools.pro_cam.StartCoroutine(CameraTools.pro_cam.AdjustTargetInfluenceRoutine(cameraTarget, 1f, 1f, duration));
    CameraTools.pro_cam.StartCoroutine(CameraTools.pro_cam.AdjustTargetInfluenceRoutine(CameraTools.pro_cam.CameraTargets[0], 0.0f, 0.0f, duration, true));
    GJTimer.AddTimer(duration + 1f, new GJTimer.VoidDelegate(((ExtentionTools) on_finished).TryInvoke));
  }

  public static void RemoveFromCameraTargets(Transform transform, float duration = 0.7f)
  {
    CameraTools.pro_cam.RemoveCameraTarget(transform, duration);
  }

  public static void CameraFlyTo(
    Transform target,
    GJCommons.VoidDelegate on_finished = null,
    float duration = 0.7f)
  {
    if (!CameraTools._camera_flyed_to_target)
    {
      CameraTools._camera_flyed_to_target = true;
      CameraTools.StoreCameraTargets();
    }
    else
      Debug.Log((object) "Camera change target object second time!");
    if ((double) Mathf.Abs(duration) < 0.01)
    {
      CameraTools.AddToCameraTargets(target, duration: duration);
      on_finished.TryInvoke();
    }
    else
    {
      CameraTools.AddToCameraTargets(target, duration: duration);
      GJTimer.AddTimer(1f, new GJTimer.VoidDelegate(((ExtentionTools) on_finished).TryInvoke));
    }
  }

  public static void CameraFlyBack(GJCommons.VoidDelegate on_finished = null, float duration = 0.7f)
  {
    if (CameraTools._camera_flyed_to_target)
    {
      CameraTools._camera_flyed_to_target = false;
      CameraTools.RestoreCameraTargets();
    }
    else
      Debug.LogError((object) "Call CameraFlyBack when camera not changed target!");
    GJTimer.AddTimer(1f, new GJTimer.VoidDelegate(((ExtentionTools) on_finished).TryInvoke));
  }

  public static void Init() => CameraTools._camera_flyed_to_target = false;
}
