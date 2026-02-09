// Decompiled with JetBrains decompiler
// Type: CornerTalkGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CornerTalkGUI : BaseGUI
{
  public int offscreen_x;
  public float appearing_duration = 0.3f;
  public float appearing_delay = 0.2f;
  public float disappearing_duration = 0.3f;
  public UI2DSprite speaker_gfx;
  [NonSerialized]
  public bool appeared;
  public Transform _corner_point_transform;

  public override void Init()
  {
    this.appeared = false;
    this._corner_point_transform = this.GetComponentInChildren<BubbleCornerPoint>(true).transform;
    this.speaker_gfx.transform.localPosition = Vector3.right * (float) this.offscreen_x;
    base.Init();
  }

  public void Say(
    string locale,
    GJCommons.VoidDelegate on_complete,
    string sprite = "",
    SmartSpeechEngine.VoiceID voice = SmartSpeechEngine.VoiceID.None)
  {
    if (!this.is_shown)
      this.Open();
    if (string.IsNullOrEmpty(sprite))
      sprite = "ui_phone_call_red_eye";
    this.speaker_gfx.sprite2D = EasySpritesCollection.GetSprite(sprite);
    if (this.appeared)
      this.ShowMessage(locale, on_complete, voice);
    else
      this.speaker_gfx.ChangePos(this.speaker_gfx.transform.localPosition, this.speaker_gfx.transform.localPosition with
      {
        x = 0.0f
      }, this.appearing_duration, (GJCommons.VoidDelegate) (() => this.ShowMessage(locale, on_complete, voice)), this.appearing_delay);
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.speaker_gfx.ChangePos(this.speaker_gfx.transform.localPosition, this.speaker_gfx.transform.localPosition with
    {
      x = (float) this.offscreen_x
    }, this.disappearing_duration, (GJCommons.VoidDelegate) (() => base.Hide(false)));
  }

  public void ShowMessage(
    string locale,
    GJCommons.VoidDelegate on_complete,
    SmartSpeechEngine.VoiceID voice = SmartSpeechEngine.VoiceID.None)
  {
    SpeechBubbleGUI.ShowMessage((long) this.GetInstanceID(), locale, this._corner_point_transform, on_complete, true, false, voice: voice);
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__9_0() => base.Hide(false);
}
