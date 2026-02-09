// Decompiled with JetBrains decompiler
// Type: MultiAnswerGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class MultiAnswerGUI : BaseBubbleGUI
{
  public const char LOCKABLE_PHRASE_FIRST_CHAR = '@';
  [Range(0.0f, 1f)]
  public float anim_time = 0.3f;
  [Range(0.0f, 0.3f)]
  public float anim_delay = 0.1f;
  [HideInInspector]
  public MultiAnswerOptionGUI answer_prefab;
  [HideInInspector]
  public SimpleUITable table;
  [HideInInspector]
  public GamepadNavigationController gamepad_controller;
  public static MultiAnswerGUI _me;
  public MultiAnswerGUI.MultiAnswerResult _on_chosen;
  public List<MultiAnswerOptionGUI> _answers;
  public bool _all_anims_finished;
  public bool _need_reposition;
  public static WorldGameObject talker_wgo;
  public static MultiAnswerGUI _current;

  public override void Init()
  {
    MultiAnswerGUI._me = this;
    this.answer_prefab = this.GetComponentInChildren<MultiAnswerOptionGUI>(true);
    this.answer_prefab.Init();
    this.answer_prefab.gameObject.SetActive(false);
    this.table = this.GetComponent<SimpleUITable>();
    this.gamepad_controller = this.GetComponent<GamepadNavigationController>();
    base.Init();
  }

  public void ShowAnswers(List<AnswerVisualData> answers, bool show_to_left)
  {
    this.gameObject.SetActive(true);
    BaseGUI.for_gamepad = LazyInput.gamepad_active;
    LazyInput.ClearKeyDown(GameKey.Select);
    LazyInput.ClearKey(GameKey.Select);
    List<string> stringList = new List<string>();
    List<AnswerVisualData> answerVisualDataList = new List<AnswerVisualData>();
    bool flag = false;
    this.table?.ClearHashes();
    foreach (AnswerVisualData answer in answers)
    {
      if (!string.IsNullOrEmpty(answer.id) && (answer.id[0] != '@' || MainGame.me.save.unlocked_phrases.Contains(answer.id)) && !MainGame.me.save.black_list_of_phrases.Contains(answer.id))
      {
        answer.translation = SpeechBubbleGUI.SpeechText(answer.id);
        stringList.Add(answer.translation);
        answerVisualDataList.Add(answer);
        if (answer.IsDetailed())
          flag = true;
        Debug.Log((object) $"Draw answer {answer.id}, {answer.translation}");
      }
    }
    if (answerVisualDataList.Count == 0)
    {
      Debug.LogError((object) "No answers");
      this._on_chosen("error");
    }
    else
    {
      Debug.Log((object) ("Showing multi-answer dialog with options count = " + answerVisualDataList.Count.ToString()));
      int width = 0;
      foreach (string text in stringList)
      {
        int num = Mathf.RoundToInt(LabelSizeCalculator.Calc(this.answer_prefab.label, text).x);
        if (num > width)
          width = num;
      }
      this._answers = new List<MultiAnswerOptionGUI>();
      for (int index = 0; index < answerVisualDataList.Count; ++index)
      {
        MultiAnswerOptionGUI multiAnswerOptionGui = this.answer_prefab.Copy<MultiAnswerOptionGUI>(name: $"#{index.ToString()}: {answerVisualDataList[index].id}");
        GJL.EnsureChildLabelsHasCorrectFont(multiAnswerOptionGui.gameObject, false);
        GJL.ApplyCustomFontSettings(multiAnswerOptionGui.gameObject);
        multiAnswerOptionGui.gameObject.SetActive(true);
        this._answers.Add(multiAnswerOptionGui);
      }
      if (flag)
        width = 150;
      else if (width % 2 != 0)
        ++width;
      float a = 0.0f;
      for (int index = 0; index < answerVisualDataList.Count; ++index)
      {
        float anim_delay = this.anim_delay * (float) (answerVisualDataList.Count - index - 1);
        if (a.EqualsTo(0.0f))
          a = (float) ((double) anim_delay + (double) this.anim_time + 0.019999999552965164);
        if ((Object) this._answers[index] == (Object) null)
          Debug.LogError((object) $"answer #{index.ToString()} is null");
        else
          this._answers[index].Show(answerVisualDataList[index], index == 0, index == answerVisualDataList.Count - 1, width, this, this.anim_time, anim_delay);
      }
      this._all_anims_finished = a.EqualsTo(0.0f);
      this.corners[0] = this._answers.LastElement<MultiAnswerOptionGUI>().ld.gameObject;
      this.corners[1] = this._answers.LastElement<MultiAnswerOptionGUI>().rd.gameObject;
      this.corners[2] = this._answers[0].lu.gameObject;
      this.corners[3] = this._answers[0].ru.gameObject;
      this.try_show_to_left = show_to_left;
      this.current_corner_index = -1;
      base.Init();
      this.gameObject.SetActive(true);
      GJL.ApplyCustomFontSettings(this.gameObject);
      this.table.Reposition();
      this.OnContentChanged();
      this.LateUpdate();
      this.Update();
      this._need_reposition = true;
      if (BaseGUI.for_gamepad)
        this.gamepad_controller.ReinitItems(false);
      GJTimer.AddTimer(a - this.anim_time, (GJTimer.VoidDelegate) (() =>
      {
        if (this._all_anims_finished)
          return;
        this._all_anims_finished = true;
        if (!BaseGUI.for_gamepad)
          return;
        this.gamepad_controller.FocusOnFirstActive();
      }));
    }
  }

  public override void LateUpdate()
  {
    if (!((Object) this.linked_tf != (Object) null))
      return;
    this.UpdateBubble(this.linked_tf.position, true);
  }

  public void Update()
  {
    if (this._need_reposition)
    {
      this.table.Reposition();
      this._need_reposition = true;
    }
    this.OnContentChanged();
    if (!BaseGUI.for_gamepad)
      return;
    if (LazyInput.GetKeyDown(GameKey.Up))
    {
      LazyInput.ClearKeyDown(GameKey.Up);
      if (this._all_anims_finished)
        this.gamepad_controller.Navigate(Direction.Up);
    }
    if (LazyInput.GetKeyDown(GameKey.Down))
    {
      LazyInput.ClearKeyDown(GameKey.Down);
      if (this._all_anims_finished)
        this.gamepad_controller.Navigate(Direction.Down);
    }
    if (!LazyInput.GetKeyDown(GameKey.Select))
      return;
    LazyInput.ClearAllKeysDown();
    if (this._all_anims_finished)
      this.gamepad_controller.SelectFocusedItem();
    else
      this.SkipAnimations();
  }

  public void SkipAnimations()
  {
    if (this._all_anims_finished)
      return;
    this._all_anims_finished = true;
    foreach (MultiAnswerOptionGUI answer in this._answers)
      answer.FinishAnimation();
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.FocusOnFirstActive();
  }

  public void OnChosen(string answer)
  {
    Debug.Log((object) ("MultiAnswer OnChosen: " + answer));
    this._on_chosen(answer);
    this.StartDisappear();
  }

  public static void ShowAnswers(
    List<AnswerVisualData> answers,
    Transform link,
    MultiAnswerGUI.MultiAnswerResult on_chosen,
    bool show_to_left = false,
    GJCommons.VoidDelegate on_disappeared = null,
    WorldGameObject talker = null)
  {
    if ((Object) MultiAnswerGUI._me == (Object) null)
    {
      Debug.LogError((object) "MultiAnswer.ShowAnswers error: _me is null");
    }
    else
    {
      MultiAnswerGUI multiAnswerGui;
      MultiAnswerGUI._current = multiAnswerGui = MultiAnswerGUI._me.Copy<MultiAnswerGUI>();
      MultiAnswerGUI.talker_wgo = talker;
      multiAnswerGui.linked_tf = link;
      multiAnswerGui.ShowAnswers(answers, show_to_left);
      multiAnswerGui._on_chosen = on_chosen;
      multiAnswerGui.on_disappeared = on_disappeared;
    }
  }

  public override void DestroyBubble()
  {
    if ((Object) this == (Object) MultiAnswerGUI._me)
    {
      Debug.LogError((object) "Error: trying to destroy _me MultiAnswerGUI");
    }
    else
    {
      base.DestroyBubble();
      MultiAnswerGUI._current = (MultiAnswerGUI) null;
    }
  }

  public static void HideAnyctive()
  {
    if (!((Object) MultiAnswerGUI._current != (Object) null))
      return;
    MultiAnswerGUI._current.DestroyBubble();
  }

  [CompilerGenerated]
  public void \u003CShowAnswers\u003Eb__15_0()
  {
    if (this._all_anims_finished)
      return;
    this._all_anims_finished = true;
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.FocusOnFirstActive();
  }

  public delegate void MultiAnswerResult(string chosen);
}
