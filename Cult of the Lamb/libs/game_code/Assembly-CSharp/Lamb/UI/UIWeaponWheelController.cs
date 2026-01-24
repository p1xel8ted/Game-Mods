// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIWeaponWheelController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIWeaponWheelController : UIRadialMenuBase<WeaponWheelItem, EquipmentType>
{
  [SerializeField]
  public RectTransform _container;
  public WeaponWheelItem _item;
  public static System.Action OnWheelHide;

  public override bool SelectOnHighlight => true;

  public override void Start()
  {
    base.Start();
    this._controlPrompts.HideAcceptButton();
    this._controlPrompts.HideCancelButton();
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this._container.position = (Vector3) (Vector2) Camera.main.WorldToScreenPoint(PlayerFarming.Instance.playerController.transform.position + new Vector3(0.0f, 2f, 0.0f));
  }

  public override void OnChoiceFinalized()
  {
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    System.Action onWheelHide = UIWeaponWheelController.OnWheelHide;
    if (onWheelHide == null)
      return;
    onWheelHide();
  }

  public override void MakeChoice(WeaponWheelItem item)
  {
    foreach (WeaponWheelItem wheelItem in this._wheelItems)
      wheelItem.SetSelected((UnityEngine.Object) item == (UnityEngine.Object) wheelItem);
    this._item = item;
  }

  public override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIWeaponWheelController weaponWheelController = this;
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
    this.\u003C\u003E2__current = (object) weaponWheelController._animator.YieldForAnimation("Hide");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public static bool CanShowWheel()
  {
    return !((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null) && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Attacking && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Dodging && !PlayerFarming.Instance.GoToAndStopping && LocationManager.LocationIsDungeon(PlayerFarming.Location) && DataManager.Instance.WeaponPool.Count > 1;
  }
}
