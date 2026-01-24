// Decompiled with JetBrains decompiler
// Type: CanvasConstants
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CanvasConstants : BaseMonoBehaviour
{
  public CanvasGroup _canvasGroup;
  public GameObject BuildSiteProgressUIPrefab;
  public static CanvasConstants instance;

  public void OnEnable() => CanvasConstants.instance = this;

  public void OnDisable()
  {
    if (!((Object) CanvasConstants.instance == (Object) this))
      return;
    CanvasConstants.instance = (CanvasConstants) null;
  }

  public CanvasConstants() => CanvasConstants.instance = this;

  public void Hide() => this._canvasGroup.alpha = 0.0f;

  public void Show() => this._canvasGroup.alpha = 1f;
}
