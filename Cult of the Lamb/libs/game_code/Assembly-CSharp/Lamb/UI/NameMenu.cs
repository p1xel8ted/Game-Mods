// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NameMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class NameMenu : UISubmenuBase
{
  public System.Action OnStartedEditingName;
  public Action<string> OnEndedEditingName;
  [SerializeField]
  public MMInputField _nameInputField;
  [SerializeField]
  public MMButton _randomiseButton;
  [CompilerGenerated]
  public bool \u003CEditing\u003Ek__BackingField;
  public Follower _follower;

  public bool Editing
  {
    set => this.\u003CEditing\u003Ek__BackingField = value;
    get => this.\u003CEditing\u003Ek__BackingField;
  }

  public void Start()
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

  public void OnStartedEditing()
  {
    this.Editing = true;
    System.Action startedEditingName = this.OnStartedEditingName;
    if (startedEditingName == null)
      return;
    startedEditingName();
  }

  public void OnEndedEditing(string text)
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

  public void RandomiseName() => this._nameInputField.text = FollowerInfo.GenerateName();

  public override void OnHideStarted()
  {
    Action<string> endedEditingName = this.OnEndedEditingName;
    if (endedEditingName == null)
      return;
    endedEditingName(this._nameInputField.text);
  }
}
