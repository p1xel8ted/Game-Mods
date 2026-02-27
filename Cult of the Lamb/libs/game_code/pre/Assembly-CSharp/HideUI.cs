// Decompiled with JetBrains decompiler
// Type: HideUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HideUI : BaseMonoBehaviour
{
  private void Start() => UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);

  private void Update()
  {
    foreach (Canvas canvas in (Canvas[]) UnityEngine.Object.FindObjectsOfType((System.Type) typeof (Canvas)))
    {
      CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
      if ((UnityEngine.Object) canvasGroup == (UnityEngine.Object) null)
        canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
      if (canvas.gameObject.name != "Canvas - Temple Overlays" && canvas.gameObject.name != "Transition(Clone)")
        canvasGroup.alpha = 0.0f;
    }
  }

  public void ShowUI()
  {
    foreach (Component component in (Canvas[]) UnityEngine.Object.FindObjectsOfType((System.Type) typeof (Canvas)))
      component.GetComponent<CanvasGroup>().alpha = 1f;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
