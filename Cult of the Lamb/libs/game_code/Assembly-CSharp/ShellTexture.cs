// Decompiled with JetBrains decompiler
// Type: ShellTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
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
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        gameObjectList.Add(current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (UnityEngine.Object @object in gameObjectList)
      UnityEngine.Object.DestroyImmediate(@object);
    if ((UnityEngine.Object) this.shellSprite == (UnityEngine.Object) null || (UnityEngine.Object) this.shellMaterial == (UnityEngine.Object) null)
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
