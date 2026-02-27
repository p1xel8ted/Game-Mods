// Decompiled with JetBrains decompiler
// Type: ShellTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ShellTexture : BaseMonoBehaviour
{
  [SerializeField]
  public Sprite shellSprite;
  [SerializeField]
  public Material shellMaterial;
  [SerializeField]
  public Color shellColour = Color.white;
  [SerializeField]
  public float shellSpacing = 0.1f;
  [SerializeField]
  public int shellCount = 10;

  public void GenerateShells()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (Transform transform in this.transform)
      gameObjectList.Add(transform.gameObject);
    foreach (Object @object in gameObjectList)
      Object.DestroyImmediate(@object);
    if ((Object) this.shellSprite == (Object) null || (Object) this.shellMaterial == (Object) null)
    {
      Debug.LogWarning((object) "Shell sprite or material is not assigned.");
    }
    else
    {
      for (int index = 0; index < this.shellCount; ++index)
      {
        GameObject gameObject = new GameObject($"Shell_{index}");
        gameObject.transform.SetParent(this.transform);
        Vector3 zero = Vector3.zero;
        zero.z -= (float) index * this.shellSpacing;
        gameObject.transform.localPosition = zero;
        gameObject.transform.localScale = Vector3.one;
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(this.shellColour.r, this.shellColour.g, this.shellColour.b, this.shellColour.a * (float) (1.0 - (double) index / (double) this.shellCount));
        spriteRenderer.sprite = this.shellSprite;
        spriteRenderer.material = this.shellMaterial;
      }
    }
  }
}
