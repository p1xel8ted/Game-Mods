// Decompiled with JetBrains decompiler
// Type: UIDoctrineXPBarSmall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
