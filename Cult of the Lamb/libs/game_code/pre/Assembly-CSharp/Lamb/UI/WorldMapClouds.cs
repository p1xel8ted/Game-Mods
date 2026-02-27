// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WorldMapClouds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class WorldMapClouds : MonoBehaviour
{
  [SerializeField]
  private FollowerLocation _location;
  [SerializeField]
  private CanvasGroup _canvasGroup;

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
