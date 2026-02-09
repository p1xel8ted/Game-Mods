// Decompiled with JetBrains decompiler
// Type: SpriteText
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpriteText : MonoBehaviour
{
  public SpriteRenderer prefab;
  public float padding = 0.104166664f;
  public string sprite_prefix = "sprfont_";

  public void SetText(string txt)
  {
    foreach (SpriteRenderer componentsInChild in this.transform.GetComponentsInChildren<SpriteRenderer>(true))
    {
      if ((Object) componentsInChild != (Object) this.prefab)
        Object.Destroy((Object) componentsInChild.gameObject);
    }
    this.prefab.gameObject.SetActive(false);
    float x = 0.0f;
    for (int index = txt.Length - 1; index >= 0; --index)
    {
      char ch = txt[index];
      SpriteRenderer spriteRenderer = Object.Instantiate<SpriteRenderer>(this.prefab, this.prefab.transform.parent);
      spriteRenderer.gameObject.SetActive(true);
      spriteRenderer.transform.localPosition = new Vector3(x, 0.0f);
      x -= this.padding;
      spriteRenderer.sprite = EasySpritesCollection.GetSprite(this.sprite_prefix + ch.ToString());
    }
  }
}
