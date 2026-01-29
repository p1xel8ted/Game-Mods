// Decompiled with JetBrains decompiler
// Type: WoolhavenPlazaRotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class WoolhavenPlazaRotManager : BaseMonoBehaviour
{
  [SerializeField]
  public List<Sprite> rotSprites = new List<Sprite>();
  [SerializeField]
  public Sprite beatenYngyaSprite;
  [SerializeField]
  public Material plazaVolumetricMaterial;
  public SpriteRenderer _spriteRenderer;
  [CompilerGenerated]
  public static WoolhavenPlazaRotManager \u003CInstance\u003Ek__BackingField;

  public static WoolhavenPlazaRotManager Instance
  {
    get => WoolhavenPlazaRotManager.\u003CInstance\u003Ek__BackingField;
    set => WoolhavenPlazaRotManager.\u003CInstance\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    WoolhavenPlazaRotManager.Instance = this;
    this._spriteRenderer = this.GetComponent<SpriteRenderer>();
  }

  public void OnEnable() => this.SetCurrentPlazaRot();

  public void SetCurrentPlazaRot()
  {
    int winterServerity = DataManager.Instance.WinterServerity;
    if (DataManager.Instance.BeatenYngya)
      this.SetBeatenYngyaSprite();
    else if (winterServerity > 0)
      this.SetRotSprite(winterServerity);
    else
      this.ClearRot();
  }

  public void ClearRot()
  {
    this._spriteRenderer.sprite = (Sprite) null;
    this.plazaVolumetricMaterial.SetFloat("_MaskStength", 0.0f);
  }

  public void SetBeatenYngyaSprite()
  {
    this._spriteRenderer.sprite = this.beatenYngyaSprite;
    this.plazaVolumetricMaterial.SetFloat("_MaskStength", 1f);
    this.plazaVolumetricMaterial.SetTexture("_Mask", (Texture) this._spriteRenderer.sprite.texture);
  }

  public void SetRotSprite(int level)
  {
    --level;
    if (level < 0)
      level = 0;
    if (level >= this.rotSprites.Count)
      level = this.rotSprites.Count - 1;
    this._spriteRenderer.sprite = this.rotSprites[level];
    this.plazaVolumetricMaterial.SetTexture("_Mask", (Texture) this._spriteRenderer.sprite.texture);
    this.plazaVolumetricMaterial.SetFloat("_MaskStength", level > 0 ? 1f : 0.0f);
  }
}
