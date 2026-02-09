// Decompiled with JetBrains decompiler
// Type: IllustrationsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class IllustrationsGUI : BaseGUI
{
  public Transform illustrations_folder;
  public UILabel text;
  public UI2DSprite dark_back;
  public bool is_open;

  public override void Init() => base.Init();

  public override void OnClosePressed()
  {
  }

  public new void Open(bool with_dark_back = true)
  {
    base.Open(false);
    this.text.alpha = 0.0f;
    this.text.text = string.Empty;
    this.ShowIllustration(string.Empty);
    this.dark_back.alpha = 0.0f;
    if (with_dark_back)
      DOTween.To((DOGetter<float>) (() => this.dark_back.alpha), (DOSetter<float>) (x => this.dark_back.alpha = x), 1f, 0.5f);
    this.is_open = true;
  }

  public void Hide()
  {
    this.SetText(string.Empty, out float _);
    this.ShowIllustration(string.Empty);
    DOTween.To((DOGetter<float>) (() => this.dark_back.alpha), (DOSetter<float>) (x => this.dark_back.alpha = x), 0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.Hide(false)));
    this.is_open = false;
  }

  public void ShowIllustration(string illustration_name)
  {
    GameObject object_to_hide = (GameObject) null;
    GameObject object_to_show = (GameObject) null;
    for (int index = 0; index < this.illustrations_folder.childCount; ++index)
    {
      Transform child = this.illustrations_folder.GetChild(index);
      if (!((Object) child == (Object) null))
      {
        GameObject gameObject = child.gameObject;
        if (gameObject.name == illustration_name && !gameObject.activeInHierarchy)
          object_to_show = gameObject;
        else if (gameObject.name != illustration_name && gameObject.activeInHierarchy)
          object_to_hide = gameObject;
      }
      else
        break;
    }
    if ((Object) object_to_hide != (Object) null)
    {
      UI2DSprite spr_to_hide = object_to_hide.GetComponent<UI2DSprite>();
      if ((Object) spr_to_hide == (Object) null)
        Debug.LogError((object) ("Not found sprite to hide on object " + object_to_hide.name));
      else
        DOTween.To((DOGetter<float>) (() => spr_to_hide.alpha), (DOSetter<float>) (x => spr_to_hide.alpha = x), 0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          object_to_hide.SetActive(false);
          spr_to_hide.alpha = 1f;
          if (!((Object) object_to_show != (Object) null))
            return;
          UI2DSprite spr_to_show = object_to_show.GetComponent<UI2DSprite>();
          if ((Object) spr_to_show == (Object) null)
          {
            Debug.LogError((object) ("Not found sprite to show on object " + object_to_show.name));
          }
          else
          {
            object_to_show.SetActive(true);
            spr_to_show.alpha = 0.0f;
            DOTween.To((DOGetter<float>) (() => spr_to_show.alpha), (DOSetter<float>) (x => spr_to_show.alpha = x), 1f, 0.5f);
          }
        }));
    }
    else
    {
      if (!((Object) object_to_show != (Object) null))
        return;
      UI2DSprite spr_to_show = object_to_show.GetComponent<UI2DSprite>();
      if ((Object) spr_to_show == (Object) null)
      {
        Debug.LogError((object) ("Not found sprite to show on object " + object_to_show.name));
      }
      else
      {
        object_to_show.SetActive(true);
        spr_to_show.alpha = 0.0f;
        DOTween.To((DOGetter<float>) (() => spr_to_show.alpha), (DOSetter<float>) (x => spr_to_show.alpha = x), 1f, 0.5f);
      }
    }
  }

  public void SetText(string new_text, out float hold_time)
  {
    new_text = GJL.L(new_text);
    hold_time = SpeechBubbleGUI.CalculateWaitTime(new_text.Length);
    if (!string.IsNullOrEmpty(this.text.text))
    {
      hold_time += 0.5f;
      DOTween.To((DOGetter<float>) (() => this.text.alpha), (DOSetter<float>) (x => this.text.alpha = x), 0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.text.text = new_text;
        DOTween.To((DOGetter<float>) (() => this.text.alpha), (DOSetter<float>) (x => this.text.alpha = x), 1f, 0.5f);
      }));
    }
    else
    {
      this.text.alpha = 0.0f;
      this.text.text = new_text;
      DOTween.To((DOGetter<float>) (() => this.text.alpha), (DOSetter<float>) (x => this.text.alpha = x), 1f, 0.5f);
    }
  }

  [CompilerGenerated]
  public float \u003COpen\u003Eb__6_0() => this.dark_back.alpha;

  [CompilerGenerated]
  public void \u003COpen\u003Eb__6_1(float x) => this.dark_back.alpha = x;

  [CompilerGenerated]
  public float \u003CHide\u003Eb__7_0() => this.dark_back.alpha;

  [CompilerGenerated]
  public void \u003CHide\u003Eb__7_1(float x) => this.dark_back.alpha = x;

  [CompilerGenerated]
  public void \u003CHide\u003Eb__7_2() => this.Hide(false);
}
