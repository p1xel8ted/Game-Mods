// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.InterceptEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Returns true when the selected event is triggered on the selected agent.\nYou can use this for both GUI and 3D objects.\nPlease make sure that Unity Event Systems are setup correctly")]
[Category("UGUI")]
public class InterceptEvent : ConditionTask<Transform>
{
  public EventTriggerType eventType;

  public override string info => $"{this.eventType.ToString()} on {this.agentInfo}";

  public override string OnInit()
  {
    this.RegisterEvent("On" + this.eventType.ToString());
    return (string) null;
  }

  public override bool OnCheck() => false;

  public void OnPointerEnter(PointerEventData eventData) => this.YieldReturn(true);

  public void OnPointerExit(PointerEventData eventData) => this.YieldReturn(true);

  public void OnPointerDown(PointerEventData eventData) => this.YieldReturn(true);

  public void OnPointerUp(PointerEventData eventData) => this.YieldReturn(true);

  public void OnPointerClick(PointerEventData eventData) => this.YieldReturn(true);

  public void OnDrag(PointerEventData eventData) => this.YieldReturn(true);

  public void OnDrop(BaseEventData eventData) => this.YieldReturn(true);

  public void OnScroll(PointerEventData eventData) => this.YieldReturn(true);

  public void OnUpdateSelected(BaseEventData eventData) => this.YieldReturn(true);

  public void OnSelect(BaseEventData eventData) => this.YieldReturn(true);

  public void OnDeselect(BaseEventData eventData) => this.YieldReturn(true);

  public void OnMove(AxisEventData eventData) => this.YieldReturn(true);

  public void OnSubmit(BaseEventData eventData) => this.YieldReturn(true);
}
