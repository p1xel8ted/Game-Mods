// Decompiled with JetBrains decompiler
// Type: UIDoctrineXPBarSmall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
