// Decompiled with JetBrains decompiler
// Type: MMPageIndicators
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MMPageIndicators : MonoBehaviour
{
  [SerializeField]
  public MMPageIndicator[] _indicators;

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
