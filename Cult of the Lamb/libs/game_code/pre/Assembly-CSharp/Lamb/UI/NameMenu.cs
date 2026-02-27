// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NameMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class NameMenu : UISubmenuBase
{
  public System.Action OnStartedEditingName;
  public Action<string> OnEndedEditingName;
  [SerializeField]
  private MMInputField _nameInputField;
  [SerializeField]
  private MMButton _randomiseButton;
  private Follower _follower;

  public bool Editing { private set; get; }

  private void Start()
  {
    this._nameInputField.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this._nameInputField.OnEndedEditing += new Action<string>(this.OnEndedEditing);
    this._randomiseButton.onClick.AddListener(new UnityAction(this.RandomiseName));
  }

  public void Configure(Follower follower)
  {
    this._follower = follower;
    this._nameInputField.text = this._follower.Brain.Info.Name;
  }

  private void OnStartedEditing()
  {
    this.Editing = true;
    System.Action startedEditingName = this.OnStartedEditingName;
    if (startedEditingName == null)
      return;
    startedEditingName();
  }

  private void OnEndedEditing(string text)
  {
    if (text != this._follower.Brain.Info.Name)
    {
      double num = (double) this._follower.SetBodyAnimation("Indoctrinate/indoctrinate-react", false);
      this._follower.AddBodyAnimation("pray", true, 0.0f);
    }
    this.Editing = false;
    Action<string> endedEditingName = this.OnEndedEditingName;
    if (endedEditingName == null)
      return;
    endedEditingName(text);
  }

  private void RandomiseName() => this._nameInputField.text = FollowerInfo.GenerateName();

  protected override void OnHideStarted()
  {
    Action<string> endedEditingName = this.OnEndedEditingName;
    if (endedEditingName == null)
      return;
    endedEditingName(this._nameInputField.text);
  }
}
