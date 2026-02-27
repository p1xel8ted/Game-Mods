// Decompiled with JetBrains decompiler
// Type: UIWeaponCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools.UIInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class UIWeaponCards : UIInventoryController
{
  public GameObject ControlPromptsObject;
  public TextMeshProUGUI RedealTokenCount;
  private GameObject Player;
  private Canvas canvas;
  private System.Action PlayerCallBack;
  private System.Action ResetCallBack;
  public RectTransform CardStartingPosition;
  public List<UIWeaponCard> WeaponCards = new List<UIWeaponCard>();
  public float Offset = 100f;

  public static void Play(
    System.Action CallBack,
    System.Action PlayerCallBack,
    System.Action ResetCallBack,
    GameObject Player)
  {
    UIWeaponCards component = UnityEngine.Object.Instantiate<GameObject>(UnityEngine.Resources.Load<GameObject>("MMUIInventory/UI Weapon Cards"), GlobalCanvasReference.Instance).GetComponent<UIWeaponCards>();
    component.Callback = CallBack;
    component.PlayerCallBack = PlayerCallBack;
    component.ResetCallBack = ResetCallBack;
    component.Player = Player;
    component.PauseTimeSpeed = 1f;
    component.canvas = GlobalCanvasReference.CanvasInstance;
  }

  public override void StartUIInventoryController()
  {
    this.ControlPromptsObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.DealCards());
  }

  private IEnumerator DealCards()
  {
    UIWeaponCards uiWeaponCards = this;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    System.Action playerCallBack = uiWeaponCards.PlayerCallBack;
    if (playerCallBack != null)
      playerCallBack();
    yield return (object) new WaitForSeconds(1.3f);
    bool Redeal = false;
    if (DataManager.Instance.PLAYER_REDEAL_TOKEN > 0)
    {
      uiWeaponCards.ControlPromptsObject.SetActive(true);
      uiWeaponCards.RedealTokenCount.text = "x" + (object) DataManager.Instance.PLAYER_REDEAL_TOKEN;
    }
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      if (InputManager.UI.GetAcceptButtonDown())
      {
        Redeal = true;
        Timer = 1f;
      }
      yield return (object) null;
    }
    while (!Redeal && !InputManager.UI.GetAcceptButtonDown() && !InputManager.UI.GetCancelButtonDown() && (double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) < 0.30000001192092896 && (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) < 0.30000001192092896)
      yield return (object) null;
    uiWeaponCards.Close();
    if (DataManager.Instance.PLAYER_REDEAL_TOKEN > 0 && InputManager.UI.GetAcceptButtonDown() | Redeal)
    {
      System.Action resetCallBack = uiWeaponCards.ResetCallBack;
      if (resetCallBack != null)
        resetCallBack();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) uiWeaponCards.gameObject);
  }

  public override void UpdateUIInventoryController()
  {
  }

  public override void Close() => base.Close();
}
