// Decompiled with JetBrains decompiler
// Type: PerspectiveTextGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PerspectiveTextGUI : BaseGUI
{
  public GameObject camera_start_go;
  public GameObject camera_end_go;
  [Range(0.001f, 1f)]
  public float slide_speed = 0.1f;
  [Range(-90f, 90f)]
  public float camera_rot_x;
  public GameObject dark_back;
  public GameObject fade_obj;
  public UILabel ui_label;
  public string music_id;
  public float _slide_speed;
  public bool _is_sliding;
  public Camera _sliding_camera;
  public float _y;
  public float _z;
  public bool _fade_is_played;
  public bool _is_talking_now;
  public GJCommons.VoidDelegate _on_finished;
  public SmartSpeechEngine.VoiceID _voice = SmartSpeechEngine.VoiceID.Skull;
  public float _voice_volume = 0.3f;
  public const float _CAM_FIELD_OF_VEIW = 83f;
  public Vector3 _CAM_ROT = new Vector3(-50f, 0.0f, 0.0f);
  public float _TIME_SLIDE_COEFF = 0.160427809f;
  public float _CAM_FADE_TIME = 3f;
  public float _CAM_NEAR_CLIP_PLANE = 0.01f;
  public float _EASTERN_SPEED_COEFF = 0.25f;
  public int _syll_count;

  public void OpenSlidingText(GJCommons.VoidDelegate on_finished)
  {
    this.Open(false);
    this.InstantiateCameraAtPoint(new Vector3(0.0f, 1000f, 0.0f));
    GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
    {
      this._sliding_camera.transform.position = this.camera_start_go.transform.position;
      this.dark_back.gameObject.transform.eulerAngles = new Vector3(this.camera_rot_x, 0.0f, 0.0f);
      this._y = this._sliding_camera.transform.position.y;
      this._z = this._sliding_camera.transform.position.z;
      this._on_finished = on_finished;
      this._slide_speed = this.slide_speed;
      if (GJL.IsEastern())
        this.slide_speed *= this._EASTERN_SPEED_COEFF;
      this._is_sliding = true;
      this._fade_is_played = false;
      int width = Screen.width;
      if (width <= 1024 /*0x0400*/)
        this.ui_label.width = 210;
      else if (width <= 1280 /*0x0500*/)
        this.ui_label.width = 240 /*0xF0*/;
      GJTimer.AddTimer(2f, (GJTimer.VoidDelegate) (() => this._is_talking_now = true));
    }));
  }

  public void InstantiateCameraAtPoint(Vector3 position)
  {
    this._sliding_camera = Object.Instantiate<Camera>(MainGame.me.gui_cam);
    MainGame.me.gui_cam.gameObject.SetActive(false);
    this._sliding_camera.orthographic = false;
    this._sliding_camera.fieldOfView = 83f;
    this._sliding_camera.gameObject.transform.position = position;
    this._sliding_camera.gameObject.transform.eulerAngles = new Vector3(this.camera_rot_x, 0.0f, 0.0f);
    this._sliding_camera.nearClipPlane = this._CAM_NEAR_CLIP_PLANE;
    SmartAudioEngine.me.PlayOvrMusic(this.music_id);
  }

  public override void Update()
  {
    base.Update();
    if (this._is_sliding)
    {
      this._y -= Time.deltaTime * this.slide_speed;
      this._sliding_camera.transform.position = new Vector3(0.0f, this._y, this._z);
      this.fade_obj.transform.position = new Vector3(0.0f, this._y, 0.0f);
      if (!this._fade_is_played && (double) Mathf.Abs(this._y - this.camera_end_go.transform.position.y) < 0.1)
      {
        this._fade_is_played = true;
        this.PlayCameraFade((GJCommons.VoidDelegate) (() =>
        {
          this._is_talking_now = false;
          this._is_sliding = false;
          this.Hide(false);
          MainGame.me.gui_cam.gameObject.SetActive(true);
          this._sliding_camera.gameObject.Destroy();
          SmartAudioEngine.me.StopOvrMusic(this.music_id);
          this._on_finished.TryInvoke();
        }));
      }
    }
    if (!this._is_talking_now)
      return;
    SmartSpeechEngine.me.PlayVoiceSound(this._voice, this._voice_volume);
    int num = Random.Range(50, 120);
    ++this._syll_count;
    if (this._syll_count < num)
      return;
    this._syll_count = 0;
    this._is_talking_now = false;
    GJTimer.AddTimer(Random.Range(0.4f, 1.2f), (GJTimer.VoidDelegate) (() => this._is_talking_now = true));
  }

  public void PlayCameraFade(GJCommons.VoidDelegate on_finished_fade)
  {
    CameraTools.Fade((GJCommons.VoidDelegate) (() => CameraTools.UnFade(on_finished_fade, new float?(this._CAM_FADE_TIME))), new float?(this._CAM_FADE_TIME));
  }

  public void StartSlideCamera(GJCommons.VoidDelegate on_finished)
  {
    float duration = Vector3.Magnitude(this.camera_start_go.transform.position - this.camera_end_go.transform.position) * this._TIME_SLIDE_COEFF;
    this._sliding_camera.gameObject.transform.DOMove(this.camera_end_go.transform.position, duration);
    GJTimer.AddTimer(duration - this._CAM_FADE_TIME, (GJTimer.VoidDelegate) (() => CameraTools.Fade((GJCommons.VoidDelegate) (() => CameraTools.UnFade(on_finished, new float?(this._CAM_FADE_TIME))), new float?(this._CAM_FADE_TIME))));
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__27_0()
  {
    this._is_talking_now = false;
    this._is_sliding = false;
    this.Hide(false);
    MainGame.me.gui_cam.gameObject.SetActive(true);
    this._sliding_camera.gameObject.Destroy();
    SmartAudioEngine.me.StopOvrMusic(this.music_id);
    this._on_finished.TryInvoke();
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__27_1() => this._is_talking_now = true;
}
