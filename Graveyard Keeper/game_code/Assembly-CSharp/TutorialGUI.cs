// Decompiled with JetBrains decompiler
// Type: TutorialGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TutorialGUI : BaseGUI
{
  public GJCommons.VoidDelegate _on_closed;
  public List<GameObject> _children = new List<GameObject>();
  [Space(20f)]
  public string test_tutorial_id = "";

  public void TestTutorialID() => this.Open(this.test_tutorial_id);

  public override void Init()
  {
    this.Deactivate<TutorialGUI>();
    base.Init();
    this._children.Clear();
    foreach (object obj in this.transform)
    {
      GameObject gameObject = (obj as Transform).gameObject;
      if (!gameObject.name.StartsWith("("))
        this._children.Add(gameObject);
    }
  }

  public void Open(string id, GJCommons.VoidDelegate on_closed = null)
  {
    if (!BaseGUI.for_gamepad && id == "controls")
      id = "controls_pc";
    Debug.Log((object) ("Open tutorial window id = " + id));
    this.Open();
    this._on_closed = on_closed;
    MainGame.me.player.SetParam("tut_shown_" + id, 1f);
    foreach (GameObject child in this._children)
    {
      child.SetActive(child.name == id);
      if (child.name == id)
      {
        ControlsGUI component = child.GetComponent<ControlsGUI>();
        if ((Object) component != (Object) null)
          component.just_opened = true;
        if (BaseGUI.for_gamepad)
        {
          ButtonTipsStr componentInChildren = this.GetComponentInChildren<ButtonTipsStr>();
          if ((Object) componentInChildren != (Object) null)
            componentInChildren.Print(GameKeyTip.Select("ok"));
        }
      }
    }
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override bool OnPressedSelect()
  {
    this.OnClosePressed();
    return true;
  }

  public override void Hide(bool play_hide_sound = true)
  {
    base.Hide(play_hide_sound);
    if (this._on_closed == null)
      return;
    GJCommons.VoidDelegate onClosed = this._on_closed;
    this._on_closed = (GJCommons.VoidDelegate) null;
    onClosed();
  }
}
