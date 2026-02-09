// Decompiled with JetBrains decompiler
// Type: BaseTutorialGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseTutorialGUI : BaseGUI
{
  public TutorialItemGUI[] items;
  public UITable ui_table;
  public SimpleUITable simple_ui_table;
  public string back_btn_text = "back";
  [Tooltip("gui elemets which should be hidden when any window is opened above this window")]
  public List<GameObject> hideable_controls;
  public List<bool> controlls_states = new List<bool>();

  public override void Init()
  {
    this.items = this.GetComponentsInChildren<TutorialItemGUI>(true);
    foreach (TutorialItemGUI tutorialItemGui in this.items)
      tutorialItemGui.Init(this);
    this.ui_table = this.GetComponentInChildren<UITable>(true);
    this.simple_ui_table = this.GetComponentInChildren<SimpleUITable>(true);
    base.Init();
  }

  public override void Open()
  {
    base.Open();
    TutorialItemGUI tutorialItemGui1 = (TutorialItemGUI) null;
    TutorialItemGUI tutorialItemGui2 = (TutorialItemGUI) null;
    foreach (TutorialItemGUI tutorialItemGui3 in this.items)
    {
      tutorialItemGui3.Show();
      if (tutorialItemGui3.gameObject.activeSelf)
      {
        if ((Object) tutorialItemGui1 == (Object) null)
          tutorialItemGui1 = tutorialItemGui3;
        tutorialItemGui2 = tutorialItemGui3;
      }
    }
    if ((Object) tutorialItemGui1 != (Object) null && (Object) tutorialItemGui2 != (Object) null)
    {
      tutorialItemGui1.gamepad_item.SetCustomDirectionItem(tutorialItemGui2.gamepad_item, Direction.Up);
      tutorialItemGui2.gamepad_item.SetCustomDirectionItem(tutorialItemGui1.gamepad_item, Direction.Down);
    }
    foreach (TutorialItemGUI tutorialItemGui4 in this.items)
    {
      if (!((Object) tutorialItemGui4.gamepad_frame == (Object) null))
        tutorialItemGui4.gamepad_frame.Deactivate<UIWidget>();
    }
    this.Reposition();
    this.UpdatePixelPerfect();
    if (!this.is_shown || !BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(true);
  }

  public override void Hide(bool play_sound = true)
  {
    foreach (TutorialItemGUI tutorialItemGui in this.items)
      tutorialItemGui.OnOut(false);
    this.Reposition();
    base.Hide(play_sound);
  }

  public void Reposition()
  {
    if ((Object) this.simple_ui_table != (Object) null)
      this.simple_ui_table.Reposition();
    if (!((Object) this.ui_table != (Object) null))
      return;
    this.ui_table.Reposition();
  }

  public virtual void UpdatTip(bool select_active)
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Back(this.back_btn_text));
  }

  public void SetControllsActive(bool active)
  {
    this.hideable_controls.RemoveUnityNulls<GameObject>();
    if (active)
    {
      if (this.controlls_states.Count == 0)
        return;
      for (int index = 0; index < this.hideable_controls.Count; ++index)
        this.hideable_controls[index].SetActive(this.controlls_states[index]);
    }
    else
    {
      this.controlls_states.Clear();
      foreach (GameObject hideableControl in this.hideable_controls)
      {
        this.controlls_states.Add(hideableControl.activeSelf);
        hideableControl.Deactivate();
      }
    }
  }

  public override void OnAboveWindowClosed()
  {
    this.InitPlatformDependentStuff();
    if (!this.is_shown || !BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(false);
    this.gamepad_controller.RestoreFocus();
  }
}
