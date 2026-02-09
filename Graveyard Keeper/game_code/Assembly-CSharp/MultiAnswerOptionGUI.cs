// Decompiled with JetBrains decompiler
// Type: MultiAnswerOptionGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class MultiAnswerOptionGUI : MonoBehaviour
{
  [HideInInspector]
  public UILabel label;
  [HideInInspector]
  public UI2DSprite ld;
  [HideInInspector]
  public UI2DSprite rd;
  [HideInInspector]
  public UI2DSprite lu;
  [HideInInspector]
  public UI2DSprite ru;
  [HideInInspector]
  public UI2DSprite back;
  public UIButton _button;
  public GamepadNavigationItem _gamepad_item;
  public UIWidget _widget;
  [Header("Mouse settings")]
  public int mouse_delta_size = 4;
  public Color mouse_focus_color;
  [Header("Gamepad settings")]
  public int gamepad_delta_size = 8;
  public Color gamepad_focus_color;
  public string _answer_id;
  public MultiAnswerGUI _answer_gui;
  public int _label_width;
  public int _focus_delta_size;
  public int _start_size;
  public Color _default_color;
  public UILabel label_2;
  public UIWidget button_widget;
  [Space(10f)]
  public UI2DSprite price_icon;
  public UIWidget price_widget;
  public UILabel price_label_n;
  public UI2DSprite price_quality_icon;
  public UIWidget price_lock_available;
  public UIWidget price_lock_locked;
  [Space(10f)]
  public UI2DSprite reward_icon;
  public UIWidget reward_widget;
  public UILabel reward_label;
  public UILabel reward_label_n;
  public UI2DSprite reward_quality_icon;
  [Space(10f)]
  public UIWidget lock_widget;
  public UIWidget lock_widget_locked;
  public UIWidget lock_widget_available;
  public UIWidget lock_widget_arrow;
  public UILabel lock_label;
  public UI2DSprite lock_quality_icon;
  [Space(10f)]
  public Color color_disabled;
  public Color color_icon_normal;
  public Color color_icon_not_enough;
  public Color color_text_normal;
  public Color color_text_not_enough;
  public const int DETAILED_OPTION_WIDTH = 150;
  public const int DETAILED_OPTION_HEIGHT = 28;
  public bool _can_be_picked = true;
  public AnswerVisualData _answer_data;
  public Color inside_color_normal;
  public Color inside_color_not_enough;
  public bool _inited;
  public bool _animating;
  public UIWidget _wgt;
  [Header("Multiple Price Lock")]
  public Transform multiple_price_content;
  public Transform multiple_lock_content;
  public MultiAnswerLock multiAnswerLockPrefab;
  public MultiAnswerPrice multiAnswerPricePrefab;
  public UITable table;

  public void Init()
  {
    this._inited = true;
    this.label = this.GetComponent<UILabel>();
    this._wgt = this.GetComponent<UIWidget>();
    foreach (UI2DSprite componentsInChild in this.GetComponentsInChildren<UI2DSprite>(true))
    {
      switch (componentsInChild.name)
      {
        case "back":
          this.back = componentsInChild;
          break;
        case "ld":
        case "left down":
          this.ld = componentsInChild;
          break;
        case "left up":
        case "lu":
          this.lu = componentsInChild;
          break;
        case "rd":
        case "right down":
          this.rd = componentsInChild;
          break;
        case "right up":
        case "ru":
          this.ru = componentsInChild;
          break;
      }
    }
    this._button = this.GetComponentInChildren<UIButton>(true);
    this._gamepad_item = this.GetComponent<GamepadNavigationItem>();
    this._widget = this.back.GetComponent<UIWidget>();
  }

  public void Update()
  {
    if (!this._inited)
      return;
    this.ld.color = this.rd.color = this.lu.color = this.ru.color = this.back.color;
    if (!this._animating)
      return;
    this._wgt.UpdateVisibility(true, true);
  }

  public void OnChosen()
  {
    if (!this._can_be_picked || (double) this._widget.alpha < 0.5)
    {
      Debug.Log((object) ("Reply option can't be clicked, _can_be_picked = " + this._can_be_picked.ToString()));
    }
    else
    {
      LazyInput.ClearAllKeysDown();
      this._can_be_picked = false;
      if (this._answer_data != null && this._answer_data.link_to_answer_data != null)
      {
        if (this._answer_data is MultipleAnswerVisualData)
        {
          for (int index = 0; index < this._answer_data.answer_visual_datas.Count; ++index)
            MainGame.me.player.RemoveSmartRes(this._answer_data.answer_visual_datas[index].link_to_answer_data.d_price);
        }
        else
          MainGame.me.player.RemoveSmartRes(this._answer_data.link_to_answer_data.d_price);
        if (this._answer_data.link_to_answer_data.HasAnyReward())
          MainGame.me.player.ReceiveSmartRes(this._answer_data.link_to_answer_data.d_reward);
      }
      this._answer_gui.OnChosen(this._answer_id);
      Sounds.OnGUIClick();
    }
  }

  public void OnFocused()
  {
    this._button.defaultColor = this._button.hover;
    this._widget.ChangeSize((float) (this._start_size + this._focus_delta_size), (float) this._widget.height, 0.1f);
    Sounds.OnGUIHover();
  }

  public void OnUnfocused()
  {
    this._button.defaultColor = this._default_color;
    this._widget.ChangeSize((float) this._start_size, (float) this._widget.height, 0.1f);
  }

  public void Show(
    AnswerVisualData answer,
    bool first_answer,
    bool last_answer,
    int width,
    MultiAnswerGUI answer_gui,
    float anim_time = 0.3f,
    float anim_delay = 0.0f)
  {
    if (!this._inited)
      this.Init();
    if (!first_answer)
      this.DeactivateCorners(true);
    if (!last_answer)
      this.DeactivateCorners(false);
    this._answer_id = answer.id;
    this._answer_gui = answer_gui;
    this._answer_data = answer;
    this._animating = false;
    this.lock_widget_locked.gameObject.SetActive(false);
    this.price_widget.gameObject.SetActive(false);
    this.reward_widget.gameObject.SetActive(false);
    this.multiple_lock_content.gameObject.SetActive(false);
    this.multiple_price_content.gameObject.SetActive(false);
    for (int index = 1; index < this.multiple_lock_content.childCount; ++index)
      Object.Destroy((Object) this.multiple_lock_content.GetChild(index).gameObject);
    for (int index = 1; index < this.multiple_price_content.childCount; ++index)
      Object.Destroy((Object) this.multiple_price_content.GetChild(index).gameObject);
    this.lock_label.color = this.color_text_normal;
    this.label.width = this._label_width = width;
    this.label.overflowMethod = UILabel.Overflow.ResizeHeight;
    this.label_2.text = this.label.text = answer.translation;
    int height = this.label.height;
    this._can_be_picked = answer.can_be_picked;
    if (answer.IsDetailed())
    {
      this.label.width = this._label_width = 150;
      bool flag1 = false;
      this.label_2.gameObject.SetActive(true);
      this.label.text = "";
      this.label.ProcessText();
      this._wgt.UpdateAnchors();
      this.label_2.overflowMethod = UILabel.Overflow.ResizeHeight;
      float y = LabelSizeCalculator.Calc(this.label_2, this.label_2.text).y;
      int num = 0;
      while ((double) (this.label.height + 3) < (double) y)
      {
        this.label.text += "\n";
        this.label.ProcessText();
        if (++num > 10)
          break;
      }
      bool flag2 = false;
      if (answer is MultipleAnswerVisualData)
      {
        MultipleAnswerVisualData answerVisualData1 = (MultipleAnswerVisualData) answer;
        for (int index = 0; index < answerVisualData1.answer_visual_datas.Count; ++index)
        {
          AnswerVisualData answerVisualData2 = answerVisualData1.answer_visual_datas[index];
          MultiAnswerLock multiAnswerLock = Object.Instantiate<MultiAnswerLock>(this.multiAnswerLockPrefab, this.multiple_lock_content);
          MultiAnswerPrice multiAnswerPrice = Object.Instantiate<MultiAnswerPrice>(this.multiAnswerPricePrefab, this.multiple_price_content);
          multiAnswerPrice.price_widget.gameObject.SetActive(true);
          multiAnswerLock.lock_widget.gameObject.SetActive(true);
          if (!string.IsNullOrEmpty(answerVisualData2.icon_price) && answerVisualData2.icon_price.StartsWith(":"))
          {
            multiAnswerLock.lock_widget_arrow.gameObject.SetActive(true);
            multiAnswerLock.lock_widget_locked.gameObject.SetActive(false);
            multiAnswerLock.lock_widget_available.gameObject.SetActive(false);
            this.multiple_lock_content.gameObject.SetActive(true);
            multiAnswerLock.lock_label.text = answerVisualData2.icon_price.Substring(2);
            if (answerVisualData2.inside_price_is_red)
              multiAnswerLock.lock_label.color = this.color_text_not_enough;
          }
          else
          {
            multiAnswerPrice.price_label_n.gameObject.SetActive(true);
            if (EasySpritesCollection.SetSpriteOrDisableGameObject(multiAnswerPrice.price_icon, this.FixIconName(answerVisualData2.icon_price)))
            {
              multiAnswerPrice.price_label_n.text = !answerVisualData2.inside_price_is_red || string.IsNullOrEmpty(answerVisualData2.price_txt) ? (answerVisualData2.n_price > 1 ? answerVisualData2.n_price.ToString() ?? "" : "") : answerVisualData2.price_txt;
              if (answerVisualData2.n_price == 0)
              {
                Debug.LogError((object) ("Price n = 0 for item = " + answerVisualData2.icon_price));
                multiAnswerPrice.price_label_n.text = "???";
              }
              multiAnswerPrice.price_label_n.color = answerVisualData2.inside_price_is_red ? this.inside_color_not_enough : this.inside_color_normal;
              flag1 = true;
              if (string.IsNullOrEmpty(answerVisualData2.icon_price_quality))
              {
                multiAnswerPrice.price_quality_icon.gameObject.SetActive(false);
              }
              else
              {
                multiAnswerPrice.price_quality_icon.gameObject.SetActive(true);
                multiAnswerPrice.price_quality_icon.sprite2D = EasySpritesCollection.GetSprite(answerVisualData2.icon_price_quality);
              }
              this.multiple_price_content.gameObject.SetActive(true);
              multiAnswerPrice.price_lock_available.gameObject.SetActive(false);
              multiAnswerPrice.price_lock_locked.gameObject.SetActive(false);
            }
            else
            {
              multiAnswerPrice.price_label_n.text = "";
              multiAnswerPrice.price_widget.SetActive(false);
            }
            multiAnswerLock.lock_widget_arrow.gameObject.SetActive(false);
            multiAnswerLock.lock_widget_locked.gameObject.SetActive(!this._can_be_picked && !flag2);
            multiAnswerLock.lock_widget_available.gameObject.SetActive(this._can_be_picked && !flag2);
            if (!string.IsNullOrEmpty(answerVisualData2.icon_lock) && answerVisualData2.icon_lock.StartsWith(":"))
            {
              this.multiple_lock_content.gameObject.SetActive(true);
              multiAnswerLock.lock_label.text = answerVisualData2.icon_lock.Substring(2);
            }
            else
            {
              if (!string.IsNullOrEmpty(answerVisualData2.icon_lock))
              {
                Debug.Log((object) ("lock icon = " + answerVisualData2.icon_lock));
                if (EasySpritesCollection.SetSpriteOrDisableGameObject(multiAnswerPrice.price_icon, this.FixIconName(answerVisualData2.icon_lock)))
                {
                  multiAnswerPrice.price_lock_locked.gameObject.SetActive(!this._can_be_picked);
                  multiAnswerPrice.price_lock_available.gameObject.SetActive(this._can_be_picked);
                  flag2 = true;
                  this.multiple_price_content.gameObject.SetActive(true);
                  multiAnswerPrice.price_widget.gameObject.SetActive(true);
                  multiAnswerPrice.price_label_n.text = answerVisualData2.n_lock <= 1 ? "" : answerVisualData2.n_lock.ToString() ?? "";
                  flag1 = true;
                  if (string.IsNullOrEmpty(answerVisualData2.icon_lock_quality))
                  {
                    multiAnswerPrice.price_quality_icon.gameObject.SetActive(false);
                  }
                  else
                  {
                    multiAnswerPrice.price_quality_icon.gameObject.SetActive(true);
                    multiAnswerPrice.price_quality_icon.sprite2D = EasySpritesCollection.GetSprite(answerVisualData2.icon_lock_quality);
                  }
                }
              }
              multiAnswerLock.lock_widget.gameObject.SetActive(false);
            }
          }
          multiAnswerPrice.SetBack(index == answerVisualData1.answer_visual_datas.Count - 1);
        }
      }
      else if (!string.IsNullOrEmpty(answer.icon_price) && answer.icon_price.StartsWith(":"))
      {
        this.lock_widget_arrow.gameObject.SetActive(true);
        this.lock_widget_locked.gameObject.SetActive(false);
        this.lock_widget_available.gameObject.SetActive(false);
        this.lock_widget.gameObject.SetActive(true);
        this.lock_label.text = answer.icon_price.Substring(2);
        if (answer.inside_price_is_red)
          this.lock_label.color = this.color_text_not_enough;
      }
      else
      {
        this.price_label_n.gameObject.SetActive(true);
        if (EasySpritesCollection.SetSpriteOrDisableGameObject(this.price_icon, this.FixIconName(answer.icon_price)))
        {
          this.price_label_n.text = !answer.inside_price_is_red || string.IsNullOrEmpty(answer.price_txt) ? (answer.n_price > 1 ? answer.n_price.ToString() ?? "" : "") : answer.price_txt;
          if (answer.n_price == 0)
          {
            Debug.LogError((object) ("Price n = 0 for item = " + answer.icon_price));
            this.price_label_n.text = "???";
          }
          this.price_label_n.color = answer.inside_price_is_red ? this.inside_color_not_enough : this.inside_color_normal;
          flag1 = true;
          if (string.IsNullOrEmpty(answer.icon_price_quality))
          {
            this.price_quality_icon.gameObject.SetActive(false);
          }
          else
          {
            this.price_quality_icon.gameObject.SetActive(true);
            this.price_quality_icon.sprite2D = EasySpritesCollection.GetSprite(answer.icon_price_quality);
          }
          this.price_widget.gameObject.SetActive(true);
          this.price_lock_available.gameObject.SetActive(false);
          this.price_lock_locked.gameObject.SetActive(false);
        }
        else
        {
          this.price_label_n.text = "";
          this.price_widget.gameObject.SetActive(false);
        }
        this.lock_widget_arrow.gameObject.SetActive(false);
        this.lock_widget_locked.gameObject.SetActive(!this._can_be_picked);
        this.lock_widget_available.gameObject.SetActive(this._can_be_picked);
        if (!string.IsNullOrEmpty(answer.icon_lock) && answer.icon_lock.StartsWith(":"))
        {
          this.lock_widget.gameObject.SetActive(true);
          this.lock_label.text = answer.icon_lock.Substring(2);
        }
        else
        {
          if (!string.IsNullOrEmpty(answer.icon_lock))
          {
            Debug.Log((object) ("lock icon = " + answer.icon_lock));
            if (EasySpritesCollection.SetSpriteOrDisableGameObject(this.price_icon, this.FixIconName(answer.icon_lock)))
            {
              this.price_lock_locked.gameObject.SetActive(!this._can_be_picked);
              this.price_lock_available.gameObject.SetActive(this._can_be_picked);
              this.price_widget.gameObject.SetActive(true);
              this.price_label_n.text = answer.n_lock <= 1 ? "" : answer.n_lock.ToString() ?? "";
              flag1 = true;
              if (string.IsNullOrEmpty(answer.icon_lock_quality))
              {
                this.price_quality_icon.gameObject.SetActive(false);
              }
              else
              {
                this.price_quality_icon.gameObject.SetActive(true);
                this.price_quality_icon.sprite2D = EasySpritesCollection.GetSprite(answer.icon_lock_quality);
              }
            }
          }
          this.lock_widget.gameObject.SetActive(false);
        }
      }
      if (!string.IsNullOrEmpty(answer.icon_reward) && answer.icon_reward.StartsWith(":"))
      {
        if (height >= 28)
          flag1 = true;
        this.reward_widget.gameObject.SetActive(true);
        this.reward_icon.gameObject.SetActive(false);
        this.reward_label.text = answer.icon_reward.Substring(1);
      }
      else
      {
        this.reward_label.text = "";
        if (EasySpritesCollection.SetSpriteOrDisableGameObject(this.reward_icon, this.FixIconName(answer.icon_reward)))
        {
          this.reward_widget.gameObject.SetActive(true);
          this.reward_label_n.text = answer.n_reward == 1 ? "" : "×" + answer.n_reward.ToString();
          flag1 = true;
        }
        else
        {
          this.reward_widget.gameObject.SetActive(false);
          this.reward_label_n.text = "";
        }
        if (string.IsNullOrEmpty(answer.icon_reward_quality))
        {
          this.reward_quality_icon.gameObject.SetActive(false);
        }
        else
        {
          this.reward_quality_icon.gameObject.SetActive(true);
          this.reward_quality_icon.sprite2D = EasySpritesCollection.GetSprite(answer.icon_reward_quality);
        }
      }
      Debug.Log((object) $"{this.label_2.text}, fh = {flag1.ToString()}");
      if (flag1)
      {
        if (this.label.height < 28)
          this.label.overflowMethod = UILabel.Overflow.ClampContent;
        this.GetComponent<UIWidget>().height = 28 >= height ? 28 : height;
      }
    }
    else
    {
      this.label_2.gameObject.SetActive(false);
      this.reward_widget.gameObject.SetActive(false);
      this.lock_widget.gameObject.SetActive(false);
      this.price_widget.gameObject.SetActive(false);
      this.multiple_lock_content.gameObject.SetActive(false);
      this.multiple_price_content.gameObject.SetActive(false);
    }
    this._widget.Update();
    this._start_size = this._widget.width;
    this._focus_delta_size = BaseGUI.for_gamepad ? this.gamepad_delta_size : this.mouse_delta_size;
    this._button.hover = BaseGUI.for_gamepad ? this.gamepad_focus_color : this.mouse_focus_color;
    this._default_color = this._button.defaultColor;
    this._animating = true;
    this.label.color = this._can_be_picked ? Color.black : this.color_disabled;
    this.label_2.color = this.label.color;
    this._wgt.ChangeAlpha(0.01f, 1f, anim_time, (GJCommons.VoidDelegate) (() => this._animating = false), anim_delay);
    this.table.repositionNow = true;
    this._gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnFocused), new GJCommons.VoidDelegate(this.OnUnfocused), new GJCommons.VoidDelegate(this.OnChosen));
  }

  public string FixIconName(string s) => s;

  public void FinishAnimation()
  {
    this._widget.GetComponent<TweenAlpha>().DestroyComponent();
    this.label.GetComponent<TweenAlpha>().DestroyComponent();
    this._widget.alpha = 1f;
    this.label.alpha = 1f;
  }

  public void DeactivateCorners(bool top)
  {
    if (top)
    {
      this.lu.Deactivate<UI2DSprite>();
      this.ru.Deactivate<UI2DSprite>();
    }
    else
    {
      this.ld.Deactivate<UI2DSprite>();
      this.rd.Deactivate<UI2DSprite>();
    }
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__62_0() => this._animating = false;
}
