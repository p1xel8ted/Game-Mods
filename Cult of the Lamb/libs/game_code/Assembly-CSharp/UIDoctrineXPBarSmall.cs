// Decompiled with JetBrains decompiler
// Type: UIDoctrineXPBarSmall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDoctrineXPBarSmall : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;
  public Image ProgressBar;

  public void Play(float Progress, string SermonNameAndLevel)
  {
    this.Text.text = SermonNameAndLevel;
    this.ProgressBar.transform.localScale = new Vector3(Progress, 1f);
  }
}
