// Decompiled with JetBrains decompiler
// Type: PhotoInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PhotoInfoCard : UIInfoCardBase<PhotoModeManager.PhotoData>
{
  [SerializeField]
  public TMP_Text photoTitle;
  [SerializeField]
  public RawImage photoImage;

  public override void Configure(PhotoModeManager.PhotoData config)
  {
    if (config == null)
      return;
    this.photoTitle.text = config.PhotoName;
    this.photoImage.texture = (Texture) config.PhotoTexture;
  }

  public void Update()
  {
    if ((Object) this.photoImage.texture == (Object) null)
      this.CanvasGroup.alpha = 0.0f;
    else
      this.CanvasGroup.alpha = 1f;
  }
}
