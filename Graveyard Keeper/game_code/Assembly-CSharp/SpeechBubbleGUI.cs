// Decompiled with JetBrains decompiler
// Type: SpeechBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class SpeechBubbleGUI : BaseBubbleGUI
{
  public static Dictionary<long, SpeechBubbleGUI> all = new Dictionary<long, SpeechBubbleGUI>();
  [SerializeField]
  [HideInInspector]
  public UILabel _label;
  public const float LETTER_ANIM_APPEAR_TIME = 0.02f;
  public const float TOTAL_TIME = 1.5f;
  public const float ONE_SYMBOL_TIME = 0.08449999f;
  public const float EASTERN_SYMBOL_K = 4f;
  public static SpeechBubbleGUI _me;
  public bool _animating;
  public bool _disappearing;
  public bool _use_world_cam = true;
  public float _last_time;
  public float _hold_time;
  public int _prev_w;
  public long _speaker_id;
  public SpeechBubbleGUI.SpeechBubbleType _type;
  public SmartSpeechEngine.VoiceID _voice;
  public UnityEngine.Sprite spr_corner_talk;
  public UnityEngine.Sprite spr_corner_think;
  public UnityEngine.Sprite spr_back_say;
  public UnityEngine.Sprite spr_back_square;
  public Color color_normal;
  public Color color_player;
  public static int _last_depth = 0;
  public static char[] RANDOM_CHARACTERS = new char[22]
  {
    '╣',
    '║',
    '╗',
    '╝',
    '╜',
    '╛',
    '┐',
    '└',
    '┴',
    '┬',
    '├',
    '┼',
    '╞',
    '╟',
    '╚',
    '╔',
    '╩',
    '╦',
    '╠',
    '╬',
    '╧',
    '╨'
  };
  public static List<char> NON_CHANGABLE_CHARS = new List<char>()
  {
    ' ',
    '\r',
    '\n',
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    '[',
    ']',
    ')'
  };

  public override void Init()
  {
    SpeechBubbleGUI._me = this;
    this._label = this.GetComponentInChildren<UILabel>(true);
    base.Init();
    BaseGUI.on_window_opened += (BaseGUI.OnAnyWindowStateChanged) (obj =>
    {
      if (SpeechBubbleGUI.all.Count == 0)
        return;
      foreach (SpeechBubbleGUI speechBubbleGui in new List<SpeechBubbleGUI>((IEnumerable<SpeechBubbleGUI>) SpeechBubbleGUI.all.Values))
        speechBubbleGui.ForceHide(true);
    });
  }

  public void ShowMessage(
    string msg,
    bool show_to_left = false,
    Color? color = null,
    SmartSpeechEngine.VoiceID voice = SmartSpeechEngine.VoiceID.None)
  {
    this.gameObject.SetActive(true);
    this.try_show_to_left = show_to_left;
    this._voice = voice;
    this.txt = this._label.text = SpeechBubbleGUI.SpeechText(msg);
    foreach (GameObject corner in this.corners)
      corner.GetComponentInChildren<UI2DSprite>().sprite2D = this._type == SpeechBubbleGUI.SpeechBubbleType.InfoBox ? (UnityEngine.Sprite) null : (this._type == SpeechBubbleGUI.SpeechBubbleType.Talk ? this.spr_corner_talk : this.spr_corner_think);
    if ((Object) this.back_spr != (Object) null)
      this.back_spr.sprite2D = this._type == SpeechBubbleGUI.SpeechBubbleType.InfoBox ? this.spr_back_square : this.spr_back_say;
    this.OnContentChanged();
    this.StartAppear();
    this._animating = true;
    this._last_time = GJCommons.GetTicksInSeconds();
    this._hold_time = SpeechBubbleGUI.CalculateWaitTime(this.txt.Length);
    if (color.HasValue)
      this.SetColor(color.Value);
    this.LateUpdate();
    this.Update();
  }

  public void Update()
  {
    if (MainGame.paused)
      return;
    this.DoAnimations();
  }

  public void DoAnimations()
  {
    bool flag1 = LazyInput.GetKeyDown(GameKey.Back) || LazyInput.GetKeyDown(GameKey.Select) || Input.GetMouseButtonDown(0);
    if (flag1)
      LazyInput.ClearAllKeysDown();
    float a = GJCommons.GetTicksInSeconds() - this._last_time;
    if (flag1 && a.EqualsTo(0.0f))
      flag1 = false;
    if (!this._animating)
    {
      if ((double) this._hold_time > 0.0)
        this._hold_time -= RealTime.deltaTime;
      if (flag1)
        this._hold_time = -1f;
      if (this._disappearing || (double) this._hold_time > 0.0)
        return;
      this._disappearing = true;
      this.StartDisappear();
    }
    else
    {
      bool flag2 = false;
      bool flag3 = false;
      int num1;
      bool flag4;
      do
      {
        num1 = Mathf.FloorToInt(a / 0.02f);
        flag4 = false;
        if (flag1)
          num1 = this.txt.Length;
        else if (this._prev_w < num1 - 1)
        {
          num1 = ++this._prev_w;
          flag4 = true;
        }
        if (num1 >= this.txt.Length)
        {
          num1 = this.txt.Length;
          this._animating = false;
          break;
        }
        if (num1 < 0)
          num1 = 0;
        if (num1 > 0)
        {
          int num2 = (int) this.txt[num1 - 1];
          if (num2 == 40)
            flag2 = true;
          if (num2 == 91)
            flag3 = true;
          bool flag5 = false;
          if (num2 == 41)
          {
            flag2 = false;
            flag5 = true;
          }
          if (num2 == 93)
          {
            flag3 = false;
            flag5 = true;
          }
          if (flag5 && num1 < this.txt.Length && this.txt[num1] == '[')
            flag3 = true;
        }
        if (flag2 | flag3)
        {
          this._last_time -= 0.02f;
          a += 0.02f;
        }
      }
      while (flag2 | flag3 | flag4);
      this._prev_w = num1;
      this._label.text = $"{this.txt.Substring(0, num1)}[c][00000000]{this.txt.Substring(num1).Replace("[-][/c]", "")}[-][/c]";
      float volume = 1f;
      if ((Object) this.linked_tf != (Object) null)
        volume = Sounds.CalcSoundVolume(new Vector2?((Vector2) this.linked_tf.position));
      SmartSpeechEngine.me.PlayVoiceSound(this._voice, volume);
    }
  }

  public void ForceHide(bool without_anims = false)
  {
    this._animating = false;
    this._hold_time = -1f;
    if (this._disappearing)
      return;
    this._disappearing = true;
    if (without_anims)
      this.alpha_anim_time = 0.0f;
    this.StartDisappear();
  }

  public override void DestroyBubble()
  {
    base.DestroyBubble();
    if (!SpeechBubbleGUI.all.ContainsKey(this._speaker_id) || !((Object) SpeechBubbleGUI.all[this._speaker_id] == (Object) this))
      return;
    SpeechBubbleGUI.all.Remove(this._speaker_id);
  }

  public static float CalculateWaitTime(int message_len)
  {
    float num = (float) message_len * 0.08449999f;
    if (GJL.IsEastern())
      num *= 4f;
    return 1.5f + num;
  }

  public override void LateUpdate()
  {
    if (!((Object) this.linked_tf != (Object) null))
      return;
    this.UpdateBubble(this.linked_tf.position, this._use_world_cam);
  }

  public static void ShowMessage(
    long speaker_id,
    string txt,
    Transform link,
    GJCommons.VoidDelegate on_disappeared = null,
    bool show_to_left = false,
    bool use_world_cam = true,
    SpeechBubbleGUI.SpeechBubbleType type = SpeechBubbleGUI.SpeechBubbleType.Talk,
    bool is_player = false,
    SmartSpeechEngine.VoiceID voice = SmartSpeechEngine.VoiceID.None)
  {
    if ((Object) SpeechBubbleGUI._me == (Object) null)
      return;
    if (SpeechBubbleGUI.all.ContainsKey(speaker_id))
    {
      SpeechBubbleGUI.all[speaker_id].ForceHide();
      SpeechBubbleGUI.all.Remove(speaker_id);
    }
    SpeechBubbleGUI speechBubbleGui = SpeechBubbleGUI._me.Copy<SpeechBubbleGUI>();
    SpeechBubbleGUI._last_depth += 3;
    GJL.EnsureChildLabelsHasCorrectFont(speechBubbleGui.gameObject, false);
    GJL.ApplyCustomFontSettings(speechBubbleGui.gameObject);
    speechBubbleGui.linked_tf = link;
    speechBubbleGui._speaker_id = speaker_id;
    speechBubbleGui._use_world_cam = use_world_cam;
    speechBubbleGui._type = type;
    if (type == SpeechBubbleGUI.SpeechBubbleType.InfoBox)
      speechBubbleGui.try_bottom_center = true;
    speechBubbleGui.ShowMessage(txt, show_to_left, new Color?(is_player ? SpeechBubbleGUI._me.color_player : SpeechBubbleGUI._me.color_normal), voice);
    speechBubbleGui.on_disappeared = on_disappeared;
    if ((Object) speechBubbleGui._label != (Object) null)
      speechBubbleGui._label.depth = SpeechBubbleGUI._last_depth + 2;
    if ((Object) speechBubbleGui.back_spr != (Object) null)
      speechBubbleGui.back_spr.depth = SpeechBubbleGUI._last_depth;
    foreach (GameObject corner in speechBubbleGui.corners)
    {
      UIWidget component = corner.GetComponent<UIWidget>();
      if ((Object) component != (Object) null)
        component.depth = SpeechBubbleGUI._last_depth + 1;
    }
    SpeechBubbleGUI.all.Add(speaker_id, speechBubbleGui);
  }

  public static string SpeechText(string s)
  {
    s = LocalizedLabel.ColorizeTags(GJL.L(s), LocalizedLabel.TextColor.SpeechBubble);
    StringBuilder stringBuilder = new StringBuilder();
    float num = PlayerComponent.GetTextObfuscationChance() * 100f;
    if ((double) num < 5.0)
      return s;
    bool flag = false;
    foreach (char ch in s)
    {
      if (ch == '(')
        flag = true;
      if (ch == ')')
        flag = false;
      if (flag || SpeechBubbleGUI.NON_CHANGABLE_CHARS.Contains(ch) || (double) Random.Range(0, 100) > (double) num)
        stringBuilder.Append(ch);
      else
        stringBuilder.Append(SpeechBubbleGUI.RANDOM_CHARACTERS[NGUITools.RandomRange(0, SpeechBubbleGUI.RANDOM_CHARACTERS.Length - 1)]);
    }
    return stringBuilder.ToString();
  }

  public void SetColor(Color c)
  {
    this.back_spr.color = c;
    foreach (GameObject corner in this.corners)
      corner.GetComponentInChildren<UI2DSprite>().color = c;
  }

  public enum SpeechBubbleType
  {
    Talk,
    Think,
    InfoBox,
  }
}
