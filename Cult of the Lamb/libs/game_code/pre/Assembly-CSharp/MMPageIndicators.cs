// Decompiled with JetBrains decompiler
// Type: MMPageIndicators
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MMPageIndicators : MonoBehaviour
{
  [SerializeField]
  private MMPageIndicator[] _indicators;

  public void SetNumPages(int pages)
  {
    for (int index = 0; index < this._indicators.Length; ++index)
      this._indicators[index].gameObject.SetActive(index < pages);
  }

  public void SetPage(int page)
  {
    for (int index = 0; index < this._indicators.Length; ++index)
    {
      if (index == page)
        this._indicators[index].Activate();
      else
        this._indicators[index].Deactivate();
    }
  }
}
