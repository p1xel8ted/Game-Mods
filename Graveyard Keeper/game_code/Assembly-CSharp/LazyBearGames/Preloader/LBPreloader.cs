// Decompiled with JetBrains decompiler
// Type: LazyBearGames.Preloader.LBPreloader
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace LazyBearGames.Preloader;

public class LBPreloader : MonoBehaviour
{
  public bool autostart = true;
  public float fade_time = 0.35f;
  public SpriteRenderer fading_sprite;
  public List<LBPreloaderLogo> logos = new List<LBPreloaderLogo>();
  public int _cur_logo;
  public LBPreloader.State _state;
  public float _time;
  public UnityEngine.Object _cur_obj;
  public LBPreloader.PreloaderCoroutine _preloader_coroutine;
  public IEnumerator _enumerator;
  public static LBPreloader _me;
  public bool _coroutine_can_be_left_unfinished;
  public bool _is_animation_in_progress;
  public Action _on_finished_logos;

  public void Start()
  {
    if (this.autostart)
      this.StartAnimations();
    LBPreloader._me = this;
  }

  public void Update()
  {
    if (this._state == LBPreloader.State.Paused)
      return;
    this._time += Time.deltaTime;
    float num = Mathf.Min(1f, this._time / this.fade_time);
    Color color = this.fading_sprite.color;
    switch (this._state)
    {
      case LBPreloader.State.Paused:
        break;
      case LBPreloader.State.FadeIn:
        if ((double) this._time >= (double) this.fade_time)
        {
          this._state = LBPreloader.State.ShowingLogo;
          this._time = 0.0f;
        }
        color.a = 1f - num;
        this.fading_sprite.color = color;
        break;
      case LBPreloader.State.FadeOut:
        if ((double) this._time >= (double) this.fade_time)
          this.ProceedToNextLogo();
        color.a = num;
        this.fading_sprite.color = color;
        break;
      case LBPreloader.State.ShowingLogo:
        if (!this._is_animation_in_progress)
          this.ProcessCoroutineStep();
        if ((double) this._time < (double) this.logos[this._cur_logo].time)
          break;
        this._state = LBPreloader.State.FadeOut;
        this._time = 0.0f;
        break;
      case LBPreloader.State.FinishingCoroutine:
        this.ProcessCoroutineStep();
        if (this._preloader_coroutine != null)
          break;
        this.OnAllLogosShown();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void ProcessCoroutineStep()
  {
    if (this._preloader_coroutine == null)
      return;
    if (this._enumerator == null)
    {
      this._enumerator = this._preloader_coroutine();
    }
    else
    {
      if (this._enumerator.MoveNext())
        return;
      Debug.Log((object) "Finished preloader coroutine");
      this._preloader_coroutine = (LBPreloader.PreloaderCoroutine) null;
      this._enumerator = (IEnumerator) null;
    }
  }

  public void SetOnFinishedDelegate(Action on_finished) => this._on_finished_logos = on_finished;

  public void SetPreloaderCouroutine(
    LBPreloader.PreloaderCoroutine coroutine,
    bool coroutine_can_be_left_unfinished = false)
  {
    if (coroutine_can_be_left_unfinished)
      Debug.LogError((object) "coroutine_can_be_left_unfinished==true is not implemented!");
    this._preloader_coroutine = coroutine;
    this._enumerator = (IEnumerator) null;
    this._coroutine_can_be_left_unfinished = coroutine_can_be_left_unfinished;
  }

  public void StartAnimations()
  {
    this._cur_logo = -1;
    this._is_animation_in_progress = false;
    this.ProceedToNextLogo();
  }

  public void ProceedToNextLogo()
  {
    if (this._cur_obj != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy(this._cur_obj);
    if (++this._cur_logo >= this.logos.Count)
    {
      this.OnAllLogosShown();
    }
    else
    {
      this._state = LBPreloader.State.FadeIn;
      this._time = 0.0f;
      if (this.logos[this._cur_logo].obj is Texture2D)
      {
        GameObject gameObject = new GameObject("sprite");
        gameObject.transform.SetParent(this.transform, false);
        this._cur_obj = (UnityEngine.Object) gameObject;
        Texture2D texture = this.logos[this._cur_logo].obj as Texture2D;
        gameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, (float) texture.width, (float) texture.height), new Vector2(0.5f, 0.5f), 1f);
      }
      else
        this._cur_obj = UnityEngine.Object.Instantiate(this.logos[this._cur_logo].obj, this.transform, false);
      (this._cur_obj as GameObject).transform.localScale = Vector3.one * this.logos[this._cur_logo].scale;
    }
  }

  public void OnAllLogosShown()
  {
    if (!this._coroutine_can_be_left_unfinished && this._preloader_coroutine != null)
    {
      this._state = LBPreloader.State.FinishingCoroutine;
    }
    else
    {
      this._state = LBPreloader.State.Paused;
      if (this._on_finished_logos == null)
        return;
      this._on_finished_logos();
    }
  }

  public static void OnAnimationStarted()
  {
    if ((UnityEngine.Object) LBPreloader._me == (UnityEngine.Object) null)
      Debug.LogError((object) "Calling a static LBPreloader.OnAnimationStarted() method without previously running initialize.");
    else
      LBPreloader._me._is_animation_in_progress = true;
  }

  public static void OnAnimationStopped()
  {
    if ((UnityEngine.Object) LBPreloader._me == (UnityEngine.Object) null)
      Debug.LogError((object) "Calling a static LBPreloader.OnAnimationStopped() method without previously running initialize.");
    else
      LBPreloader._me._is_animation_in_progress = false;
  }

  public enum State
  {
    Paused,
    FadeIn,
    FadeOut,
    ShowingLogo,
    FinishingCoroutine,
  }

  public delegate IEnumerator PreloaderCoroutine();
}
