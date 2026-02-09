// Decompiled with JetBrains decompiler
// Type: UIAtlas
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  public Material material;
  [SerializeField]
  [HideInInspector]
  public List<UISpriteData> mSprites = new List<UISpriteData>();
  [SerializeField]
  [HideInInspector]
  public float mPixelSize = 1f;
  [SerializeField]
  [HideInInspector]
  public UIAtlas mReplacement;
  [SerializeField]
  [HideInInspector]
  public UIAtlas.Coordinates mCoordinates;
  [SerializeField]
  [HideInInspector]
  public List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();
  public int mPMA = -1;
  public Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

  public Material spriteMaterial
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.material : this.mReplacement.spriteMaterial;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        this.mReplacement.spriteMaterial = value;
      else if ((UnityEngine.Object) this.material == (UnityEngine.Object) null)
      {
        this.mPMA = 0;
        this.material = value;
      }
      else
      {
        this.MarkAsChanged();
        this.mPMA = -1;
        this.material = value;
        this.MarkAsChanged();
      }
    }
  }

  public bool premultipliedAlpha
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.premultipliedAlpha;
      if (this.mPMA == -1)
      {
        Material spriteMaterial = this.spriteMaterial;
        this.mPMA = !((UnityEngine.Object) spriteMaterial != (UnityEngine.Object) null) || !((UnityEngine.Object) spriteMaterial.shader != (UnityEngine.Object) null) || !spriteMaterial.shader.name.Contains("Premultiplied") ? 0 : 1;
      }
      return this.mPMA == 1;
    }
  }

  public List<UISpriteData> spriteList
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.spriteList;
      if (this.mSprites.Count == 0)
        this.Upgrade();
      return this.mSprites;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        this.mReplacement.spriteList = value;
      else
        this.mSprites = value;
    }
  }

  public Texture texture
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.texture;
      return !((UnityEngine.Object) this.material != (UnityEngine.Object) null) ? (Texture) null : this.material.mainTexture;
    }
  }

  public float pixelSize
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mPixelSize : this.mReplacement.pixelSize;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.pixelSize = value;
      }
      else
      {
        float num = Mathf.Clamp(value, 0.25f, 4f);
        if ((double) this.mPixelSize == (double) num)
          return;
        this.mPixelSize = num;
        this.MarkAsChanged();
      }
    }
  }

  public UIAtlas replacement
  {
    get => this.mReplacement;
    set
    {
      UIAtlas uiAtlas = value;
      if ((UnityEngine.Object) uiAtlas == (UnityEngine.Object) this)
        uiAtlas = (UIAtlas) null;
      if (!((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) uiAtlas))
        return;
      if ((UnityEngine.Object) uiAtlas != (UnityEngine.Object) null && (UnityEngine.Object) uiAtlas.replacement == (UnityEngine.Object) this)
        uiAtlas.replacement = (UIAtlas) null;
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        this.MarkAsChanged();
      this.mReplacement = uiAtlas;
      if ((UnityEngine.Object) uiAtlas != (UnityEngine.Object) null)
        this.material = (Material) null;
      this.MarkAsChanged();
    }
  }

  public UISpriteData GetSprite(string name)
  {
    if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      return this.mReplacement.GetSprite(name);
    if (!string.IsNullOrEmpty(name))
    {
      if (this.mSprites.Count == 0)
        this.Upgrade();
      if (this.mSprites.Count == 0)
        return (UISpriteData) null;
      if (this.mSpriteIndices.Count != this.mSprites.Count)
        this.MarkSpriteListAsChanged();
      int index1;
      if (this.mSpriteIndices.TryGetValue(name, out index1))
      {
        if (index1 > -1 && index1 < this.mSprites.Count)
          return this.mSprites[index1];
        this.MarkSpriteListAsChanged();
        return !this.mSpriteIndices.TryGetValue(name, out index1) ? (UISpriteData) null : this.mSprites[index1];
      }
      int index2 = 0;
      for (int count = this.mSprites.Count; index2 < count; ++index2)
      {
        UISpriteData mSprite = this.mSprites[index2];
        if (!string.IsNullOrEmpty(mSprite.name) && name == mSprite.name)
        {
          this.MarkSpriteListAsChanged();
          return mSprite;
        }
      }
    }
    return (UISpriteData) null;
  }

  public string GetRandomSprite(string startsWith)
  {
    if (this.GetSprite(startsWith) != null)
      return startsWith;
    List<UISpriteData> spriteList = this.spriteList;
    List<string> stringList = new List<string>();
    foreach (UISpriteData uiSpriteData in spriteList)
    {
      if (uiSpriteData.name.StartsWith(startsWith))
        stringList.Add(uiSpriteData.name);
    }
    return stringList.Count <= 0 ? (string) null : stringList[UnityEngine.Random.Range(0, stringList.Count)];
  }

  public void MarkSpriteListAsChanged()
  {
    this.mSpriteIndices.Clear();
    int index = 0;
    for (int count = this.mSprites.Count; index < count; ++index)
      this.mSpriteIndices[this.mSprites[index].name] = index;
  }

  public void SortAlphabetically()
  {
    this.mSprites.Sort((Comparison<UISpriteData>) ((s1, s2) => s1.name.CompareTo(s2.name)));
  }

  public BetterList<string> GetListOfSprites()
  {
    if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      return this.mReplacement.GetListOfSprites();
    if (this.mSprites.Count == 0)
      this.Upgrade();
    BetterList<string> listOfSprites = new BetterList<string>();
    int index = 0;
    for (int count = this.mSprites.Count; index < count; ++index)
    {
      UISpriteData mSprite = this.mSprites[index];
      if (mSprite != null && !string.IsNullOrEmpty(mSprite.name))
        listOfSprites.Add(mSprite.name);
    }
    return listOfSprites;
  }

  public BetterList<string> GetListOfSprites(string match)
  {
    if ((bool) (UnityEngine.Object) this.mReplacement)
      return this.mReplacement.GetListOfSprites(match);
    if (string.IsNullOrEmpty(match))
      return this.GetListOfSprites();
    if (this.mSprites.Count == 0)
      this.Upgrade();
    BetterList<string> listOfSprites = new BetterList<string>();
    int index1 = 0;
    for (int count = this.mSprites.Count; index1 < count; ++index1)
    {
      UISpriteData mSprite = this.mSprites[index1];
      if (mSprite != null && !string.IsNullOrEmpty(mSprite.name) && string.Equals(match, mSprite.name, StringComparison.OrdinalIgnoreCase))
      {
        listOfSprites.Add(mSprite.name);
        return listOfSprites;
      }
    }
    string[] strArray = match.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
    for (int index2 = 0; index2 < strArray.Length; ++index2)
      strArray[index2] = strArray[index2].ToLower();
    int index3 = 0;
    for (int count = this.mSprites.Count; index3 < count; ++index3)
    {
      UISpriteData mSprite = this.mSprites[index3];
      if (mSprite != null && !string.IsNullOrEmpty(mSprite.name))
      {
        string lower = mSprite.name.ToLower();
        int num = 0;
        for (int index4 = 0; index4 < strArray.Length; ++index4)
        {
          if (lower.Contains(strArray[index4]))
            ++num;
        }
        if (num == strArray.Length)
          listOfSprites.Add(mSprite.name);
      }
    }
    return listOfSprites;
  }

  public bool References(UIAtlas atlas)
  {
    if ((UnityEngine.Object) atlas == (UnityEngine.Object) null)
      return false;
    if ((UnityEngine.Object) atlas == (UnityEngine.Object) this)
      return true;
    return (UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null && this.mReplacement.References(atlas);
  }

  public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
  {
    if ((UnityEngine.Object) a == (UnityEngine.Object) null || (UnityEngine.Object) b == (UnityEngine.Object) null)
      return false;
    return (UnityEngine.Object) a == (UnityEngine.Object) b || a.References(b) || b.References(a);
  }

  public void MarkAsChanged()
  {
    if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      this.mReplacement.MarkAsChanged();
    UISprite[] active1 = NGUITools.FindActive<UISprite>();
    int index1 = 0;
    for (int length = active1.Length; index1 < length; ++index1)
    {
      UISprite uiSprite = active1[index1];
      if (UIAtlas.CheckIfRelated(this, uiSprite.atlas))
      {
        UIAtlas atlas = uiSprite.atlas;
        uiSprite.atlas = (UIAtlas) null;
        uiSprite.atlas = atlas;
      }
    }
    UIFont[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (UIFont)) as UIFont[];
    int index2 = 0;
    for (int length = objectsOfTypeAll.Length; index2 < length; ++index2)
    {
      UIFont uiFont = objectsOfTypeAll[index2];
      if (UIAtlas.CheckIfRelated(this, uiFont.atlas))
      {
        UIAtlas atlas = uiFont.atlas;
        uiFont.atlas = (UIAtlas) null;
        uiFont.atlas = atlas;
      }
    }
    UILabel[] active2 = NGUITools.FindActive<UILabel>();
    int index3 = 0;
    for (int length = active2.Length; index3 < length; ++index3)
    {
      UILabel uiLabel = active2[index3];
      if ((UnityEngine.Object) uiLabel.bitmapFont != (UnityEngine.Object) null && UIAtlas.CheckIfRelated(this, uiLabel.bitmapFont.atlas))
      {
        UIFont bitmapFont = uiLabel.bitmapFont;
        uiLabel.bitmapFont = (UIFont) null;
        uiLabel.bitmapFont = bitmapFont;
      }
    }
  }

  public bool Upgrade()
  {
    if ((bool) (UnityEngine.Object) this.mReplacement)
      return this.mReplacement.Upgrade();
    if (this.mSprites.Count != 0 || this.sprites.Count <= 0 || !(bool) (UnityEngine.Object) this.material)
      return false;
    Texture mainTexture = this.material.mainTexture;
    int width = (UnityEngine.Object) mainTexture != (UnityEngine.Object) null ? mainTexture.width : 512 /*0x0200*/;
    int height = (UnityEngine.Object) mainTexture != (UnityEngine.Object) null ? mainTexture.height : 512 /*0x0200*/;
    for (int index = 0; index < this.sprites.Count; ++index)
    {
      UIAtlas.Sprite sprite = this.sprites[index];
      Rect outer = sprite.outer;
      Rect inner = sprite.inner;
      if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
      {
        NGUIMath.ConvertToPixels(outer, width, height, true);
        NGUIMath.ConvertToPixels(inner, width, height, true);
      }
      this.mSprites.Add(new UISpriteData()
      {
        name = sprite.name,
        x = Mathf.RoundToInt(outer.xMin),
        y = Mathf.RoundToInt(outer.yMin),
        width = Mathf.RoundToInt(outer.width),
        height = Mathf.RoundToInt(outer.height),
        paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width),
        paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width),
        paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height),
        paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height),
        borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin),
        borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax),
        borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax),
        borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin)
      });
    }
    this.sprites.Clear();
    return true;
  }

  [Serializable]
  public class Sprite
  {
    public string name = "Unity Bug";
    public Rect outer = new Rect(0.0f, 0.0f, 1f, 1f);
    public Rect inner = new Rect(0.0f, 0.0f, 1f, 1f);
    public bool rotated;
    public float paddingLeft;
    public float paddingRight;
    public float paddingTop;
    public float paddingBottom;

    public bool hasPadding
    {
      get
      {
        return (double) this.paddingLeft != 0.0 || (double) this.paddingRight != 0.0 || (double) this.paddingTop != 0.0 || (double) this.paddingBottom != 0.0;
      }
    }
  }

  public enum Coordinates
  {
    Pixels,
    TexCoords,
  }
}
