// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICurseWheelController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UICurseWheelController : UIRadialMenuBase<CurseWheelItem, EquipmentType>
{
  [SerializeField]
  private RectTransform _container;
  private CurseWheelItem _item;
  public static System.Action OnWheelHide;

  protected override bool SelectOnHighlight => true;

  protected override void Start()
  {
    base.Start();
    this._controlPrompts.HideAcceptButton();
    this._controlPrompts.HideCancelButton();
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this._container.position = (Vector3) (Vector2) Camera.main.WorldToScreenPoint(PlayerFarming.Instance.playerController.transform.position + new Vector3(0.0f, 2f, 0.0f));
  }

  protected override void OnChoiceFinalized()
  {
  }

  protected override void MakeChoice(CurseWheelItem item)
  {
    foreach (CurseWheelItem wheelItem in this._wheelItems)
      wheelItem.SetSelected((UnityEngine.Object) item == (UnityEngine.Object) wheelItem);
    this._item = item;
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    System.Action onWheelHide = UICurseWheelController.OnWheelHide;
    if (onWheelHide != null)
      onWheelHide();
    if (!((UnityEngine.Object) this._item != (UnityEngine.Object) null))
      return;
    Action<EquipmentType> onItemChosen = this.OnItemChosen;
    if (onItemChosen == null)
      return;
    onItemChosen(this._item.CurseType);
  }

  protected override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UICurseWheelController curseWheelController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) curseWheelController._animator.YieldForAnimation("Hide");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public static bool CanShowWheel()
  {
    return !((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null) && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Dodging && !PlayerFarming.Instance.GoToAndStopping && LocationManager.LocationIsDungeon(PlayerFarming.Location) && DataManager.Instance.EnabledSpells;
  }
}
