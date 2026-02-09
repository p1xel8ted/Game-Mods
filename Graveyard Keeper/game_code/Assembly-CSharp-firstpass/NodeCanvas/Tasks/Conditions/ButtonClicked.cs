// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.ButtonClicked
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("UGUI")]
public class ButtonClicked : ConditionTask
{
  [RequiredField]
  public BBParameter<Button> button;

  public override string info => $"Button {this.button.ToString()} Clicked";

  public override string OnInit()
  {
    this.button.value.onClick.AddListener(new UnityAction(this.OnClick));
    return (string) null;
  }

  public override bool OnCheck() => false;

  public void OnClick() => this.YieldReturn(true);
}
