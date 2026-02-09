// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WorldMapClouds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class WorldMapClouds : MonoBehaviour
{
  [SerializeField]
  public FollowerLocation _location;
  [SerializeField]
  public CanvasGroup _canvasGroup;

  public FollowerLocation Location => this._location;

  public void Hide() => this.gameObject.SetActive(false);

  public IEnumerator DoHide()
  {
    while ((double) this._canvasGroup.alpha > 0.0)
    {
      this._canvasGroup.alpha -= Time.unscaledDeltaTime;
      yield return (object) null;
    }
  }
}
