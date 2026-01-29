// Decompiled with JetBrains decompiler
// Type: ScrollBarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class ScrollBarController : BaseMonoBehaviour
{
  public Scrollbar scrollbar;
  public CanvasGroup canvasGroup;
  public float SelectionDelay;
  public bool released;
  public string HorizontalNavAxisName = "Horizontal";
  public string VerticalNavAxisName = "Vertical";
  public float ButtonDownDelay;
  public float ScrollBarSpeedMultiplier = 1f;

  public void Start() => EventSystem.current.SetSelectedGameObject(this.scrollbar.gameObject);

  public void Update()
  {
    if (!this.canvasGroup.interactable || (double) this.canvasGroup.alpha == 0.0 || (Object) EventSystem.current.currentSelectedGameObject != (Object) this.scrollbar.gameObject)
      return;
    this.SelectionDelay -= Time.unscaledDeltaTime;
    if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) <= 0.20000000298023224 && (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) <= 0.20000000298023224)
      this.SelectionDelay = 0.0f;
    if ((double) this.SelectionDelay >= 0.0)
      return;
    if ((double) InputManager.UI.GetVerticalAxis() < -0.34999999403953552 && (double) this.scrollbar.value > 0.0)
      this.scrollbar.value -= 0.033f * this.ScrollBarSpeedMultiplier;
    if ((double) InputManager.UI.GetVerticalAxis() <= 0.34999999403953552 || (double) this.scrollbar.value >= 1.0)
      return;
    this.scrollbar.value += 0.033f * this.ScrollBarSpeedMultiplier;
  }
}
