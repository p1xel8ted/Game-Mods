// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUITextMeshProGlyphHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Text Mesh Pro Glyph Helper")]
[RequireComponent(typeof (TextMeshProUGUI))]
public class UnityUITextMeshProGlyphHelper : MonoBehaviour
{
  [Tooltip("Enter text into this field and not in the TMPro Text field directly. Text will be parsed for special tags, and the final result will be passed on to the Text Mesh Pro Text component. See the documentation for special tag format.")]
  [SerializeField]
  [TextArea(3, 10)]
  public string _text;
  [Tooltip("Optional reference to an object that defines options. If blank, the global default options will be used.")]
  [SerializeField]
  public ControllerElementGlyphSelectorOptionsSOBase _options;
  [Tooltip("Options that control how Text Mesh Pro displays Sprites.")]
  [SerializeField]
  public UnityUITextMeshProGlyphHelper.TMProSpriteOptions _spriteOptions = UnityUITextMeshProGlyphHelper.TMProSpriteOptions.Default;
  [Tooltip("Optional material for Sprites. If blank, the default material will be used.\nMaterial is instantiated for each Sprite Asset, so making changes to values in the base material later will not affect Sprites. Changing the base material at runtime will copy only certain properties from the new material to Sprite materials.")]
  [SerializeField]
  public Material _baseSpriteMaterial;
  [Tooltip("If enabled, local values such as Sprite color will be used instead of the value on the base material.")]
  [SerializeField]
  public bool _overrideSpriteMaterialProperties = true;
  [Tooltip("These properties will override the properties on the Sprite material if Override Sprite Material Properties is enabled.")]
  [SerializeField]
  public UnityUITextMeshProGlyphHelper.SpriteMaterialProperties _spriteMaterialProperties = UnityUITextMeshProGlyphHelper.SpriteMaterialProperties.Default;
  [NonSerialized]
  public TextMeshProUGUI _tmProText;
  [NonSerialized]
  public string _textPrev;
  [NonSerialized]
  public StringBuilder _processTagSb = new StringBuilder();
  [NonSerialized]
  public StringBuilder _tempSb = new StringBuilder();
  [NonSerialized]
  public StringBuilder _tempSb2 = new StringBuilder();
  [NonSerialized]
  public UnityUITextMeshProGlyphHelper.Asset _primaryAsset;
  [NonSerialized]
  public List<UnityUITextMeshProGlyphHelper.Asset> _assignedAssets = new List<UnityUITextMeshProGlyphHelper.Asset>();
  [NonSerialized]
  public List<UnityUITextMeshProGlyphHelper.Asset> _assetsPool = new List<UnityUITextMeshProGlyphHelper.Asset>();
  [NonSerialized]
  public List<ActionElementMap> _tempAems = new List<ActionElementMap>();
  [NonSerialized]
  public List<Sprite> _tempGlyphs = new List<Sprite>();
  [NonSerialized]
  public List<UnityUITextMeshProGlyphHelper.Asset> _dirtyAssets = new List<UnityUITextMeshProGlyphHelper.Asset>();
  [NonSerialized]
  public List<string> _tempKeys = new List<string>();
  [NonSerialized]
  public List<UnityUITextMeshProGlyphHelper.GlyphOrText> _glyphsOrTextTemp = new List<UnityUITextMeshProGlyphHelper.GlyphOrText>();
  [NonSerialized]
  public List<UnityUITextMeshProGlyphHelper.Asset> _currentlyUsedAssets = new List<UnityUITextMeshProGlyphHelper.Asset>();
  [NonSerialized]
  public List<UnityUITextMeshProGlyphHelper.Tag> _currentTags = new List<UnityUITextMeshProGlyphHelper.Tag>();
  [NonSerialized]
  public Dictionary<string, string> _tempStringDictionary = new Dictionary<string, string>();
  [NonSerialized]
  public bool _initialized;
  [NonSerialized]
  public bool _rebuildRequired;
  [NonSerialized]
  public Texture2D _stubTexture;
  public UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag> __controllerElementTagPool;
  public UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag> __actionTagPool;
  public UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag> __playerTagPool;
  [NonSerialized]
  public Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> __tagHandlers;
  public static string[] __s_displayTypeNames;
  public static UnityUITextMeshProGlyphHelper.DisplayType[] __s_displayTypeValues;
  public static string[] __s_axisRangeNames;
  public static AxisRange[] __s_axisRangeValues;

  public UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag> controllerElementTagPool
  {
    get
    {
      return this.__controllerElementTagPool == null ? (this.__controllerElementTagPool = new UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag>()) : this.__controllerElementTagPool;
    }
  }

  public UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag> actionTagPool
  {
    get
    {
      return this.__actionTagPool == null ? (this.__actionTagPool = new UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag>()) : this.__actionTagPool;
    }
  }

  public UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag> playerTagPool
  {
    get
    {
      return this.__playerTagPool == null ? (this.__playerTagPool = new UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag>()) : this.__playerTagPool;
    }
  }

  public Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> tagHandlers
  {
    get
    {
      if (this.__tagHandlers != null)
        return this.__tagHandlers;
      Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> dictionary = new Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler>();
      dictionary.Add("rewiredelement", new UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler(this.ProcessTag_ControllerElement));
      dictionary.Add("rewiredaction", new UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler(this.ProcessTag_Action));
      dictionary.Add("rewiredplayer", new UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler(this.ProcessTag_Player));
      Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> tagHandlers = dictionary;
      this.__tagHandlers = dictionary;
      return tagHandlers;
    }
  }

  public virtual string text
  {
    get => this._text;
    set
    {
      this._text = value;
      this.RequireRebuild();
    }
  }

  public virtual ControllerElementGlyphSelectorOptionsSOBase options
  {
    get => this._options;
    set
    {
      this._options = value;
      this.RequireRebuild();
    }
  }

  public virtual UnityUITextMeshProGlyphHelper.TMProSpriteOptions spriteOptions
  {
    get => this._spriteOptions;
    set
    {
      this._spriteOptions = value;
      int count = this._assignedAssets.Count;
      for (int index1 = 0; index1 < count; ++index1)
      {
        int spriteCount = this._assignedAssets[index1].spriteAsset.spriteCount;
        for (int index2 = 0; index2 < spriteCount; ++index2)
        {
          UnityUITextMeshProGlyphHelper.ITMProSprite sprite = this._assignedAssets[index1].spriteAsset.GetSprite(index2);
          if (sprite != null && !((UnityEngine.Object) sprite.sprite == (UnityEngine.Object) null))
          {
            Rect rect = sprite.sprite.rect;
            sprite.xOffset = rect.width * this._spriteOptions.offsetSizeMultiplier.x + this._spriteOptions.extraOffset.x;
            sprite.yOffset = rect.height * this._spriteOptions.offsetSizeMultiplier.y + this._spriteOptions.extraOffset.y;
            sprite.xAdvance = rect.width * this._spriteOptions.xAdvanceWidthMultiplier + this._spriteOptions.extraXAdvance;
            sprite.scale = this._spriteOptions.scale;
          }
        }
        TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(true, (UnityEngine.Object) this._assignedAssets[index1].spriteAsset.GetSpriteAsset());
      }
    }
  }

  public virtual Material baseSpriteMaterial
  {
    get => this._baseSpriteMaterial;
    set
    {
      this._baseSpriteMaterial = value;
      Material sourceMaterial = (UnityEngine.Object) this._baseSpriteMaterial != (UnityEngine.Object) null ? this._baseSpriteMaterial : this._primaryAsset.material;
      this.ForEachAsset((Action<UnityUITextMeshProGlyphHelper.Asset>) (asset =>
      {
        UnityUITextMeshProGlyphHelper.CopyMaterialProperties(sourceMaterial, asset.material);
        if (this._overrideSpriteMaterialProperties)
          UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
        TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
      }));
    }
  }

  public virtual bool overrideSpriteMaterialProperties
  {
    get => this._overrideSpriteMaterialProperties;
    set
    {
      this._overrideSpriteMaterialProperties = value;
      if (value)
      {
        this.ForEachAsset((Action<UnityUITextMeshProGlyphHelper.Asset>) (asset =>
        {
          UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
          TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
        }));
      }
      else
      {
        Material sourceMaterial = (UnityEngine.Object) this._baseSpriteMaterial != (UnityEngine.Object) null ? this._baseSpriteMaterial : this._primaryAsset.material;
        this.ForEachAsset((Action<UnityUITextMeshProGlyphHelper.Asset>) (asset =>
        {
          UnityUITextMeshProGlyphHelper.CopyMaterialProperties(sourceMaterial, asset.material);
          TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
        }));
      }
    }
  }

  public virtual UnityUITextMeshProGlyphHelper.SpriteMaterialProperties spriteMaterialProperties
  {
    get => this._spriteMaterialProperties;
    set
    {
      this._spriteMaterialProperties = value;
      if (!this._overrideSpriteMaterialProperties)
        return;
      this.ForEachAsset((Action<UnityUITextMeshProGlyphHelper.Asset>) (asset =>
      {
        UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
        TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
      }));
    }
  }

  public virtual void OnEnable() => this.Initialize();

  public virtual void Start() => this.MainUpdate();

  public virtual void Update()
  {
    if (!ReInput.isReady)
      return;
    this.MainUpdate();
  }

  public virtual void OnDestroy()
  {
    if (this._primaryAsset != null)
    {
      if ((UnityEngine.Object) this._tmProText != (UnityEngine.Object) null && (UnityEngine.Object) this._tmProText.spriteAsset == (UnityEngine.Object) this._primaryAsset.spriteAsset.GetSpriteAsset())
        this._tmProText.spriteAsset = (TMP_SpriteAsset) null;
      this._primaryAsset.Destroy();
      this._primaryAsset = (UnityUITextMeshProGlyphHelper.Asset) null;
    }
    for (int index = 0; index < this._assignedAssets.Count; ++index)
    {
      if (this._assignedAssets[index] != null)
        this._assignedAssets[index].Destroy();
    }
    this._assignedAssets.Clear();
    for (int index = 0; index < this._assetsPool.Count; ++index)
    {
      if (this._assetsPool[index] != null)
        this._assetsPool[index].Destroy();
    }
    this._assetsPool.Clear();
    if ((UnityEngine.Object) this._stubTexture != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._stubTexture);
      this._stubTexture = (Texture2D) null;
    }
    for (int index = 0; index < this._currentTags.Count; ++index)
      this._currentTags[index].ReturnToPool();
  }

  public virtual void ForceUpdate()
  {
    if (!ReInput.isReady)
      return;
    this._rebuildRequired = true;
    this.Update();
  }

  public virtual ControllerElementGlyphSelectorOptions GetOptionsOrDefault()
  {
    if ((UnityEngine.Object) this._options != (UnityEngine.Object) null && this._options.options == null)
    {
      Debug.LogError((object) $"Rewired: Options missing on {typeof (ControllerElementGlyphSelectorOptions).Name}. Global default options will be used instead.");
      return ControllerElementGlyphSelectorOptions.defaultOptions;
    }
    return !((UnityEngine.Object) this._options != (UnityEngine.Object) null) ? ControllerElementGlyphSelectorOptions.defaultOptions : this._options.options;
  }

  public bool Initialize()
  {
    if (this._initialized)
      return true;
    this._tmProText = this.GetComponent<TextMeshProUGUI>();
    this._stubTexture = new Texture2D(1, 1);
    this.CreatePrimaryAsset();
    this._initialized = true;
    return true;
  }

  public void MainUpdate()
  {
    bool flag = false;
    int count1 = this._currentTags.Count;
    for (int index = 0; index < count1; ++index)
    {
      UnityUITextMeshProGlyphHelper.Tag currentTag = this._currentTags[index];
      switch (currentTag.tagType)
      {
        case UnityUITextMeshProGlyphHelper.Tag.TagType.ControllerElement:
          UnityUITextMeshProGlyphHelper.ControllerElementTag controllerElementTag = (UnityUITextMeshProGlyphHelper.ControllerElementTag) currentTag;
          this._glyphsOrTextTemp.Clear();
          this.TryGetControllerElementGlyphsOrText((UnityUITextMeshProGlyphHelper.ControllerElementTag) currentTag, this._glyphsOrTextTemp);
          if (!UnityUITextMeshProGlyphHelper.IsEqual(this._glyphsOrTextTemp, controllerElementTag.glyphsOrText))
          {
            flag = true;
            break;
          }
          break;
        case UnityUITextMeshProGlyphHelper.Tag.TagType.Action:
          UnityUITextMeshProGlyphHelper.ActionTag tag1 = (UnityUITextMeshProGlyphHelper.ActionTag) currentTag;
          string result1;
          this.TryGetActionDisplayName(tag1, out result1);
          if (!string.Equals(tag1.displayName, result1, StringComparison.Ordinal))
          {
            flag = true;
            break;
          }
          break;
        case UnityUITextMeshProGlyphHelper.Tag.TagType.Player:
          UnityUITextMeshProGlyphHelper.PlayerTag tag2 = (UnityUITextMeshProGlyphHelper.PlayerTag) currentTag;
          string result2;
          this.TryGetPlayerDisplayName(tag2, out result2);
          if (!string.Equals(tag2.displayName, result2, StringComparison.Ordinal))
          {
            flag = true;
            break;
          }
          break;
        default:
          throw new NotImplementedException();
      }
    }
    if (!string.Equals(this._text, this._textPrev, StringComparison.Ordinal))
    {
      this._textPrev = this._text;
      flag = true;
    }
    string newText;
    if ((flag || this._rebuildRequired) && this.ParseText(this._textPrev, out newText))
      this._tmProText.text = newText;
    int count2 = this._dirtyAssets.Count;
    if (count2 <= 0)
      return;
    for (int index = 0; index < count2; ++index)
      this._dirtyAssets[index].spriteAsset.UpdateLookupTables();
    this._dirtyAssets.Clear();
  }

  public bool ParseText(string text, out string newText)
  {
    newText = (string) null;
    UnityUITextMeshProGlyphHelper.Tag.Clear(this._currentTags);
    this._currentlyUsedAssets.Clear();
    bool text1 = false;
    while (this.ProcessNextTag(ref text, this._processTagSb))
    {
      text1 = true;
      newText = text;
    }
    this.RemoveUnusedAssets();
    if (this._rebuildRequired)
      this._rebuildRequired = false;
    return text1;
  }

  public bool ProcessNextTag(ref string text, StringBuilder sb)
  {
    int num1 = 0;
    UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler attributesHandler = (UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler) null;
    int count = -1;
    try
    {
      for (int index = 0; index < text.Length; ++index)
      {
        char c = text[index];
        switch (num1)
        {
          case 0:
            if (c == '<')
            {
              count = index;
              sb.Length = 0;
              num1 = 1;
              break;
            }
            break;
          case 1:
            if (UnityUITextMeshProGlyphHelper.IsValidTagNameChar(c))
            {
              sb.Append(char.ToLowerInvariant(c));
              break;
            }
            if (char.IsWhiteSpace(c))
            {
              if (sb.Length > 0)
              {
                if (this.tagHandlers.TryGetValue(sb.ToString(), out attributesHandler))
                {
                  sb.Length = 0;
                  num1 = 2;
                  break;
                }
                num1 = 0;
                --index;
                break;
              }
              break;
            }
            num1 = 0;
            --index;
            break;
          case 2:
            int num2 = text.IndexOf('>', index);
            if (num2 < 0)
              throw new Exception("Malformed tag.");
            string replacement;
            if (!attributesHandler(text, index, num2 - index, out replacement))
              throw new Exception("Error parsing attributes.");
            sb.Length = 0;
            if (count > 0)
              sb.Append(text, 0, count);
            sb.Append(replacement);
            int startIndex = num2 + 1;
            if (startIndex < text.Length)
              sb.Append(text, startIndex, text.Length - startIndex);
            text = sb.ToString();
            return true;
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    return false;
  }

  public bool ProcessTag_ControllerElement(
    string text,
    int startIndex,
    int count,
    out string replacement)
  {
    UnityUITextMeshProGlyphHelper.ControllerElementTag result;
    if (!UnityUITextMeshProGlyphHelper.ControllerElementTag.TryParseString(text, startIndex, count, this._tempSb, this._tempSb2, this._tempStringDictionary, this.controllerElementTagPool, out result))
    {
      replacement = (string) null;
      return false;
    }
    this._currentTags.Add((UnityUITextMeshProGlyphHelper.Tag) result);
    result.glyphsOrText.Clear();
    if (!this.TryGetControllerElementGlyphsOrText(result, result.glyphsOrText))
    {
      replacement = (string) null;
      return true;
    }
    this.TryCreateTMProString(result.glyphsOrText, out replacement);
    return true;
  }

  public bool ProcessTag_Action(string text, int startIndex, int count, out string replacement)
  {
    UnityUITextMeshProGlyphHelper.ActionTag result;
    if (!UnityUITextMeshProGlyphHelper.ActionTag.TryParseString(text, startIndex, count, this._tempSb, this._tempSb2, this._tempStringDictionary, this.actionTagPool, out result))
    {
      replacement = (string) null;
      return false;
    }
    this._currentTags.Add((UnityUITextMeshProGlyphHelper.Tag) result);
    this.TryGetActionDisplayName(result, out replacement);
    return true;
  }

  public bool ProcessTag_Player(string text, int startIndex, int count, out string replacement)
  {
    UnityUITextMeshProGlyphHelper.PlayerTag result;
    if (!UnityUITextMeshProGlyphHelper.PlayerTag.TryParseString(text, startIndex, count, this._tempSb, this._tempSb2, this._tempStringDictionary, this.playerTagPool, out result))
    {
      replacement = (string) null;
      return false;
    }
    this._currentTags.Add((UnityUITextMeshProGlyphHelper.Tag) result);
    this.TryGetPlayerDisplayName(result, out replacement);
    return true;
  }

  public bool TryCreateTMProString(
    List<UnityUITextMeshProGlyphHelper.GlyphOrText> glyphs,
    out string result)
  {
    StringBuilder tempSb = this._tempSb;
    tempSb.Length = 0;
    int count = glyphs.Count;
    for (int index = 0; index < count; ++index)
    {
      string glyphKey = glyphs[index].glyphKey;
      if ((UnityEngine.Object) glyphs[index].sprite != (UnityEngine.Object) null && !string.IsNullOrEmpty(glyphKey) && this.TryAssignSprite(glyphs[index].sprite, glyphKey))
        UnityUITextMeshProGlyphHelper.WriteSpriteKey(tempSb, glyphKey);
      else
        tempSb.Append(glyphs[index].name);
      if (index < count - 1)
        tempSb.Append(" ");
    }
    result = tempSb.ToString();
    return !string.IsNullOrEmpty(result);
  }

  public bool TryGetControllerElementGlyphsOrText(
    UnityUITextMeshProGlyphHelper.ControllerElementTag tag,
    List<UnityUITextMeshProGlyphHelper.GlyphOrText> results)
  {
    if (tag == null)
      return false;
    this._tempAems.Clear();
    ActionElementMap aemResult1;
    ActionElementMap aemResult2;
    if (!GlyphTools.TryGetActionElementMaps(tag.playerId, tag.actionId, tag.actionRange, this.GetOptionsOrDefault(), this._tempAems, out aemResult1, out aemResult2))
      return false;
    if (aemResult1 != null && aemResult2 != null)
    {
      UnityUITextMeshProGlyphHelper.GlyphOrText glyphOrText = new UnityUITextMeshProGlyphHelper.GlyphOrText();
      this._tempAems.Clear();
      this._tempAems.Add(aemResult1);
      this._tempAems.Add(aemResult2);
      object result1;
      string result2;
      if (UnityUITextMeshProGlyphHelper.IsGlyphAllowed(tag.type) && ActionElementMap.TryGetCombinedElementIdentifierGlyph((IList<ActionElementMap>) this._tempAems, out result1) && ActionElementMap.TryGetCombinedElementIdentifierFinalGlyphKey((IList<ActionElementMap>) this._tempAems, out result2))
      {
        glyphOrText.glyphKey = result2;
        glyphOrText.sprite = result1 as Sprite;
        results.Add(glyphOrText);
        return true;
      }
      string result3;
      if (UnityUITextMeshProGlyphHelper.IsTextAllowed(tag.type) && ActionElementMap.TryGetCombinedElementIdentifierName((IList<ActionElementMap>) this._tempAems, out result3))
      {
        glyphOrText.name = result3;
        results.Add(glyphOrText);
        return true;
      }
    }
    this._tempGlyphs.Clear();
    this._tempKeys.Clear();
    int num1 = 0 | (UnityUITextMeshProGlyphHelper.TryGetGlyphsOrText(aemResult1, tag.type, this._tempGlyphs, this._tempKeys, results) ? 1 : 0);
    this._tempGlyphs.Clear();
    this._tempKeys.Clear();
    int num2 = UnityUITextMeshProGlyphHelper.TryGetGlyphsOrText(aemResult2, tag.type, this._tempGlyphs, this._tempKeys, results) ? 1 : 0;
    return (num1 | num2) != 0;
  }

  public bool TryGetActionDisplayName(
    UnityUITextMeshProGlyphHelper.ActionTag tag,
    out string result)
  {
    if (tag == null)
    {
      result = (string) null;
      return false;
    }
    InputAction action = ReInput.mapping.GetAction(tag.actionId);
    if (action == null)
    {
      result = (string) null;
      return false;
    }
    result = action.GetDisplayName(tag.actionRange);
    tag.displayName = result;
    return true;
  }

  public bool TryGetPlayerDisplayName(
    UnityUITextMeshProGlyphHelper.PlayerTag tag,
    out string result)
  {
    if (tag == null)
    {
      result = (string) null;
      return false;
    }
    Player player = ReInput.players.GetPlayer(tag.playerId);
    if (player == null)
    {
      result = (string) null;
      return false;
    }
    result = player.descriptiveName;
    tag.displayName = result;
    return true;
  }

  public bool TryAssignSprite(Sprite sprite, string key)
  {
    UnityUITextMeshProGlyphHelper.Asset asset = this.GetOrCreateAsset(sprite);
    if (asset == null)
      return false;
    UnityUITextMeshProGlyphHelper.ITMProSpriteAsset spriteAsset = asset.spriteAsset;
    if (!spriteAsset.Contains(key))
    {
      Rect rect = sprite.rect;
      UnityUITextMeshProGlyphHelper.ITMProSprite sprite1 = UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CreateSprite();
      sprite1.width = rect.width;
      sprite1.height = rect.height;
      sprite1.position = new Vector2(rect.x, rect.y);
      sprite1.xOffset = rect.width * this._spriteOptions.offsetSizeMultiplier.x + this._spriteOptions.extraOffset.x;
      sprite1.yOffset = rect.height * this._spriteOptions.offsetSizeMultiplier.y + this._spriteOptions.extraOffset.y;
      sprite1.xAdvance = rect.width * this._spriteOptions.xAdvanceWidthMultiplier + this._spriteOptions.extraXAdvance;
      sprite1.scale = this._spriteOptions.scale;
      sprite1.pivot = new Vector2(rect.width * -0.5f, rect.height * 0.5f);
      sprite1.name = key;
      sprite1.hashCode = TMP_TextUtilities.GetSimpleHashCode(key);
      sprite1.sprite = sprite;
      spriteAsset.AddSprite(sprite1);
      this.SetDirty(asset);
    }
    if (!this._currentlyUsedAssets.Contains(asset))
      this._currentlyUsedAssets.Add(asset);
    return true;
  }

  public void RequireRebuild() => this._rebuildRequired = true;

  public void CreatePrimaryAsset()
  {
    if (this._primaryAsset != null)
      return;
    this._primaryAsset = new UnityUITextMeshProGlyphHelper.Asset((Material) null);
    this._tmProText.spriteAsset = this._primaryAsset.spriteAsset.GetSpriteAsset();
  }

  public UnityUITextMeshProGlyphHelper.Asset GetOrCreateAsset(Sprite sprite)
  {
    if ((UnityEngine.Object) sprite == (UnityEngine.Object) null || (UnityEngine.Object) sprite.texture == (UnityEngine.Object) null)
      return (UnityUITextMeshProGlyphHelper.Asset) null;
    int count1 = this._assignedAssets.Count;
    for (int index = 0; index < count1; ++index)
    {
      if (this._assignedAssets[index] != null && (UnityEngine.Object) this._assignedAssets[index].spriteAsset.spriteSheet == (UnityEngine.Object) sprite.texture)
        return this._assignedAssets[index];
    }
    UnityUITextMeshProGlyphHelper.Asset asset = (UnityUITextMeshProGlyphHelper.Asset) null;
    int count2 = this._assetsPool.Count;
    for (int index = 0; index < count2; ++index)
    {
      if (this._assetsPool[index] != null)
      {
        asset = this._assetsPool[index];
        this._assetsPool.RemoveAt(index);
        break;
      }
    }
    if (asset == null)
      asset = this.CreateAsset();
    asset.spriteAsset.spriteSheet = (Texture) sprite.texture;
    asset.material.SetTexture(ShaderUtilities.ID_MainTex, (Texture) sprite.texture);
    List<TMP_SpriteAsset> tmpSpriteAssetList = this._primaryAsset.spriteAsset.GetSpriteAsset().fallbackSpriteAssets;
    if (tmpSpriteAssetList == null)
    {
      tmpSpriteAssetList = new List<TMP_SpriteAsset>();
      this._primaryAsset.spriteAsset.GetSpriteAsset().fallbackSpriteAssets = tmpSpriteAssetList;
    }
    tmpSpriteAssetList.Add(asset.spriteAsset.GetSpriteAsset());
    this._assignedAssets.Add(asset);
    return asset;
  }

  public UnityUITextMeshProGlyphHelper.Asset CreateAsset()
  {
    UnityUITextMeshProGlyphHelper.Asset asset = new UnityUITextMeshProGlyphHelper.Asset(this._baseSpriteMaterial);
    if (this._overrideSpriteMaterialProperties)
      UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
    return asset;
  }

  public void RemoveUnusedAssets()
  {
    int num = 0;
    for (int index = this._assignedAssets.Count - 1; index >= 0; --index)
    {
      UnityUITextMeshProGlyphHelper.Asset assignedAsset = this._assignedAssets[index];
      if (assignedAsset != null && !this._currentlyUsedAssets.Contains(assignedAsset))
      {
        if (num >= 2)
        {
          this._primaryAsset.spriteAsset.GetSpriteAsset().fallbackSpriteAssets.Remove(assignedAsset.spriteAsset.GetSpriteAsset());
          assignedAsset.spriteAsset.spriteSheet = (Texture) null;
          assignedAsset.spriteAsset.Clear();
          assignedAsset.material.SetTexture(ShaderUtilities.ID_MainTex, (Texture) this._stubTexture);
          this._assetsPool.Add(assignedAsset);
          this._assignedAssets.RemoveAt(index);
        }
        else
          ++num;
      }
    }
  }

  public void SetDirty(UnityUITextMeshProGlyphHelper.Asset asset)
  {
    if (this._dirtyAssets.Contains(asset))
      return;
    this._dirtyAssets.Add(asset);
  }

  public void ForEachAsset(
    Action<UnityUITextMeshProGlyphHelper.Asset> callback)
  {
    if (callback == null)
      return;
    int count1 = this._assignedAssets.Count;
    for (int index = 0; index < count1; ++index)
    {
      if (this._assignedAssets[index] != null)
        callback(this._assignedAssets[index]);
    }
    int count2 = this._assetsPool.Count;
    for (int index = 0; index < count2; ++index)
    {
      if (this._assetsPool[index] != null)
        callback(this._assetsPool[index]);
    }
  }

  public static int shaderPropertyId_color => Shader.PropertyToID("_Color");

  public static string[] s_displayTypeNames
  {
    get
    {
      return UnityUITextMeshProGlyphHelper.__s_displayTypeNames == null ? (UnityUITextMeshProGlyphHelper.__s_displayTypeNames = Enum.GetNames(typeof (UnityUITextMeshProGlyphHelper.DisplayType))) : UnityUITextMeshProGlyphHelper.__s_displayTypeNames;
    }
  }

  public static UnityUITextMeshProGlyphHelper.DisplayType[] s_displayTypeValues
  {
    get
    {
      return UnityUITextMeshProGlyphHelper.__s_displayTypeValues == null ? (UnityUITextMeshProGlyphHelper.__s_displayTypeValues = (UnityUITextMeshProGlyphHelper.DisplayType[]) Enum.GetValues(typeof (UnityUITextMeshProGlyphHelper.DisplayType))) : UnityUITextMeshProGlyphHelper.__s_displayTypeValues;
    }
  }

  public static string[] s_axisRangeNames
  {
    get
    {
      return UnityUITextMeshProGlyphHelper.__s_axisRangeNames == null ? (UnityUITextMeshProGlyphHelper.__s_axisRangeNames = Enum.GetNames(typeof (AxisRange))) : UnityUITextMeshProGlyphHelper.__s_axisRangeNames;
    }
  }

  public static AxisRange[] s_axisRangeValues
  {
    get
    {
      return UnityUITextMeshProGlyphHelper.__s_axisRangeValues == null ? (UnityUITextMeshProGlyphHelper.__s_axisRangeValues = (AxisRange[]) Enum.GetValues(typeof (AxisRange))) : UnityUITextMeshProGlyphHelper.__s_axisRangeValues;
    }
  }

  public static void ParseAttributes(
    string text,
    int startIndex,
    int count,
    StringBuilder sbKey,
    StringBuilder sbValue,
    Dictionary<string, string> results)
  {
    if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex >= text.Length)
      return;
    results.Clear();
    sbKey.Length = 0;
    sbValue.Length = 0;
    bool flag = true;
    int num1 = startIndex + count - 1;
    int num2 = 0;
    try
    {
      for (int index = startIndex; index < startIndex + count; ++index)
      {
        char c = text[index];
        switch (num2)
        {
          case 0:
            if (UnityUITextMeshProGlyphHelper.IsValidKeyChar(c))
            {
              num2 = 1;
              --index;
              sbKey.Length = 0;
              break;
            }
            break;
          case 1:
            if (c == '=')
            {
              if (sbKey.Length == 0)
                throw new Exception("Key was blank.");
              num2 = 2;
              break;
            }
            if (UnityUITextMeshProGlyphHelper.IsValidKeyChar(c))
            {
              sbKey.Append(char.ToLowerInvariant(c));
              break;
            }
            if (!char.IsWhiteSpace(c))
              throw new Exception("Error parsing key.");
            break;
          case 2:
            if ((flag = c == '"') || UnityUITextMeshProGlyphHelper.IsValidNonQuotedValueChar(c))
            {
              if (!flag)
                --index;
              sbValue.Length = 0;
              num2 = 3;
              break;
            }
            break;
          case 3:
            if (flag && c == '"' || !flag && (index == num1 || char.IsWhiteSpace(c)))
            {
              if (!flag && index == num1)
                sbValue.Append(c);
              if (sbValue.Length == 0)
                throw new Exception("Value was blank.");
              if (results == null)
                results = new Dictionary<string, string>();
              results.Add(sbKey.ToString(), sbValue.ToString());
              num2 = 0;
              break;
            }
            sbValue.Append(c);
            break;
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
  }

  public static bool IsValidKeyChar(char c) => char.IsLetterOrDigit(c) || c == '_';

  public static bool IsValidTagNameChar(char c) => char.IsLetterOrDigit(c) || c == '_';

  public static bool IsValidNonQuotedValueChar(char c) => char.IsDigit(c);

  public static bool IsEqual(
    List<UnityUITextMeshProGlyphHelper.GlyphOrText> a,
    List<UnityUITextMeshProGlyphHelper.GlyphOrText> b)
  {
    if (a.Count != b.Count)
      return false;
    for (int index = 0; index < a.Count; ++index)
    {
      if (a[index] != b[index])
        return false;
    }
    return true;
  }

  public static void WriteSpriteKey(StringBuilder sb, string key)
  {
    sb.Append("<sprite name=\"");
    sb.Append(key);
    sb.Append("\">");
  }

  public static bool TryGetGlyphsOrText(
    ActionElementMap aem,
    UnityUITextMeshProGlyphHelper.DisplayType displayType,
    List<Sprite> glyphs,
    List<string> keys,
    List<UnityUITextMeshProGlyphHelper.GlyphOrText> results)
  {
    if (aem == null || glyphs == null || results == null)
      return false;
    UnityUITextMeshProGlyphHelper.GlyphOrText glyphOrText;
    if (UnityUITextMeshProGlyphHelper.IsGlyphAllowed(displayType) && aem.GetElementIdentifierGlyphs<Sprite>((ICollection<Sprite>) glyphs) > 0)
    {
      aem.GetElementIdentifierFinalGlyphKeys((ICollection<string>) keys);
      if (keys.Count != glyphs.Count)
      {
        Debug.LogError((object) "Rewired: Glyph key count does not match glyph count.");
      }
      else
      {
        int count = glyphs.Count;
        for (int index = 0; index < count; ++index)
        {
          glyphOrText = new UnityUITextMeshProGlyphHelper.GlyphOrText();
          glyphOrText.glyphKey = keys[index];
          glyphOrText.sprite = glyphs[index];
          results.Add(glyphOrText);
        }
        if (count > 0)
          return true;
      }
    }
    if (!UnityUITextMeshProGlyphHelper.IsTextAllowed(displayType))
      return false;
    glyphOrText = new UnityUITextMeshProGlyphHelper.GlyphOrText();
    glyphOrText.name = aem.elementIdentifierName;
    results.Add(glyphOrText);
    return true;
  }

  public static bool IsGlyphAllowed(
    UnityUITextMeshProGlyphHelper.DisplayType displayType)
  {
    return displayType == UnityUITextMeshProGlyphHelper.DisplayType.Glyph || displayType == UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
  }

  public static bool IsTextAllowed(
    UnityUITextMeshProGlyphHelper.DisplayType displayType)
  {
    return displayType == UnityUITextMeshProGlyphHelper.DisplayType.Text || displayType == UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
  }

  public static void CopyMaterialProperties(Material source, Material destination)
  {
    if ((UnityEngine.Object) source == (UnityEngine.Object) null || (UnityEngine.Object) destination == (UnityEngine.Object) null)
      return;
    destination.shader = source.shader;
    if (source.shaderKeywords != null)
    {
      string[] destinationArray = new string[source.shaderKeywords.Length];
      Array.Copy((Array) source.shaderKeywords, (Array) destinationArray, source.shaderKeywords.Length);
      destination.shaderKeywords = destinationArray;
    }
    else
      destination.shaderKeywords = (string[]) null;
    if (source.HasProperty(UnityUITextMeshProGlyphHelper.shaderPropertyId_color) && destination.HasProperty(UnityUITextMeshProGlyphHelper.shaderPropertyId_color))
      destination.color = source.color;
    destination.renderQueue = source.renderQueue;
    destination.globalIlluminationFlags = source.globalIlluminationFlags;
  }

  public static void CopySpriteMaterialPropertiesToMaterial(
    UnityUITextMeshProGlyphHelper.SpriteMaterialProperties properties,
    Material material)
  {
    if ((UnityEngine.Object) material == (UnityEngine.Object) null || !material.HasProperty(UnityUITextMeshProGlyphHelper.shaderPropertyId_color))
      return;
    material.color = properties.color;
  }

  [CompilerGenerated]
  public void \u003Cset_overrideSpriteMaterialProperties\u003Eb__51_0(
    UnityUITextMeshProGlyphHelper.Asset asset)
  {
    UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
    TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
  }

  [CompilerGenerated]
  public void \u003Cset_spriteMaterialProperties\u003Eb__54_0(
    UnityUITextMeshProGlyphHelper.Asset asset)
  {
    UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
    TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
  }

  public delegate bool ParseTagAttributesHandler(
    string text,
    int startIndex,
    int count,
    out string replacement);

  public abstract class Tag
  {
    public UnityUITextMeshProGlyphHelper.Tag.TagType tagType;
    public UnityUITextMeshProGlyphHelper.Tag.Pool _pool;

    public Tag(UnityUITextMeshProGlyphHelper.Tag.TagType tagType) => this.tagType = tagType;

    public UnityUITextMeshProGlyphHelper.Tag.Pool pool
    {
      get => this._pool;
      set => this._pool = value;
    }

    public void ReturnToPool()
    {
      if (this._pool == null)
        return;
      this._pool.Return(this);
    }

    public abstract void Clear();

    public static void Clear(List<UnityUITextMeshProGlyphHelper.Tag> list)
    {
      int count = list.Count;
      for (int index = 0; index < count; ++index)
      {
        if (list[index] != null)
          list[index].ReturnToPool();
      }
      list.Clear();
    }

    public enum TagType
    {
      ControllerElement,
      Action,
      Player,
    }

    public abstract class Pool
    {
      public abstract bool Return(UnityUITextMeshProGlyphHelper.Tag obj);
    }

    public sealed class Pool<T> : UnityUITextMeshProGlyphHelper.Tag.Pool where T : UnityUITextMeshProGlyphHelper.Tag, new()
    {
      public List<T> _list;

      public Pool() => this._list = new List<T>();

      public T Get()
      {
        if (this._list.Count == 0)
        {
          T obj = new T();
          if ((object) obj != null)
            obj.pool = (UnityUITextMeshProGlyphHelper.Tag.Pool) this;
          return obj;
        }
        int index = this._list.Count - 1;
        T obj1 = this._list[index];
        this._list.RemoveAt(index);
        return obj1;
      }

      public override bool Return(UnityUITextMeshProGlyphHelper.Tag obj)
      {
        if (!(obj is T obj1) || obj1.pool != this)
          return false;
        obj1.Clear();
        if (this._list.Contains(obj1))
          return false;
        this._list.Add(obj1);
        return true;
      }
    }
  }

  public sealed class ControllerElementTag : UnityUITextMeshProGlyphHelper.Tag
  {
    public UnityUITextMeshProGlyphHelper.DisplayType type;
    public int playerId;
    public int actionId;
    public AxisRange actionRange;
    public List<UnityUITextMeshProGlyphHelper.GlyphOrText> _glyphsOrText;

    public List<UnityUITextMeshProGlyphHelper.GlyphOrText> glyphsOrText => this._glyphsOrText;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(typeof (UnityUITextMeshProGlyphHelper.ControllerElementTag).Name);
      stringBuilder.Append(": ");
      stringBuilder.Append("type = ");
      stringBuilder.Append((object) this.type);
      stringBuilder.Append(", playerId = ");
      stringBuilder.Append(this.playerId);
      stringBuilder.Append(", actionId = ");
      stringBuilder.Append(this.actionId);
      stringBuilder.Append(", actionRange = ");
      stringBuilder.Append((object) this.actionRange);
      return stringBuilder.ToString();
    }

    public ControllerElementTag()
      : base(UnityUITextMeshProGlyphHelper.Tag.TagType.ControllerElement)
    {
      this._glyphsOrText = new List<UnityUITextMeshProGlyphHelper.GlyphOrText>();
      this.Clear();
    }

    public override void Clear()
    {
      this.type = UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
      this.playerId = -1;
      this.actionId = -1;
      this.actionRange = AxisRange.Full;
      this._glyphsOrText.Clear();
    }

    public static bool TryParseString(
      string text,
      int startIndex,
      int count,
      StringBuilder sb1,
      StringBuilder sb2,
      Dictionary<string, string> workDictionary,
      UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag> pool,
      out UnityUITextMeshProGlyphHelper.ControllerElementTag result)
    {
      result = (UnityUITextMeshProGlyphHelper.ControllerElementTag) null;
      if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex + count >= text.Length)
        return false;
      UnityUITextMeshProGlyphHelper.ParseAttributes(text, startIndex, count, sb1, sb2, workDictionary);
      if (workDictionary.Count == 0)
        return false;
      result = pool.Get();
      try
      {
        string str;
        if (workDictionary.TryGetValue("type", out str))
        {
          bool flag = false;
          for (int index = 0; index < UnityUITextMeshProGlyphHelper.s_displayTypeNames.Length; ++index)
          {
            if (string.Equals(str, UnityUITextMeshProGlyphHelper.s_displayTypeNames[index], StringComparison.OrdinalIgnoreCase))
            {
              result.type = UnityUITextMeshProGlyphHelper.s_displayTypeValues[index];
              flag = true;
              break;
            }
          }
          if (!flag)
            throw new Exception("Invalid type: " + str);
        }
        else
          result.type = UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
        if (workDictionary.TryGetValue("playerid", out str))
        {
          result.playerId = int.Parse(str);
          if (ReInput.players.GetPlayer(result.playerId) == null)
            throw new Exception("Invalid Player Id: " + result.playerId.ToString());
        }
        else
        {
          if (!workDictionary.TryGetValue("playername", out str))
            throw new Exception("Player name/id missing.");
          result.playerId = (ReInput.players.GetPlayer(str) ?? throw new Exception("Invalid Player name: " + str)).id;
        }
        if (workDictionary.TryGetValue("actionid", out str))
        {
          result.actionId = int.Parse(str);
          if (ReInput.mapping.GetAction(result.actionId) == null)
            throw new Exception("Invalid Action Id: " + result.actionId.ToString());
        }
        else
        {
          if (!workDictionary.TryGetValue("actionname", out str))
            throw new Exception("Action name/id missing.");
          result.actionId = (ReInput.mapping.GetAction(str) ?? throw new Exception("Invalid Action name: " + str)).id;
        }
        if (workDictionary.TryGetValue("actionrange", out str))
        {
          bool flag = false;
          for (int index = 0; index < UnityUITextMeshProGlyphHelper.s_axisRangeNames.Length; ++index)
          {
            if (string.Equals(str, UnityUITextMeshProGlyphHelper.s_axisRangeNames[index], StringComparison.OrdinalIgnoreCase))
            {
              result.actionRange = UnityUITextMeshProGlyphHelper.s_axisRangeValues[index];
              flag = true;
              break;
            }
          }
          if (!flag)
            throw new Exception("Invalid Action Range: " + str);
        }
        else
          result.actionRange = AxisRange.Full;
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        result.ReturnToPool();
        return false;
      }
    }
  }

  public sealed class ActionTag : UnityUITextMeshProGlyphHelper.Tag
  {
    public int actionId;
    public AxisRange actionRange;
    public string _displayName;

    public string displayName
    {
      get => this._displayName;
      set => this._displayName = value;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(typeof (UnityUITextMeshProGlyphHelper.ControllerElementTag).Name);
      stringBuilder.Append(": ");
      stringBuilder.Append("actionId = ");
      stringBuilder.Append(this.actionId);
      stringBuilder.Append(", actionRange = ");
      stringBuilder.Append((object) this.actionRange);
      return stringBuilder.ToString();
    }

    public ActionTag()
      : base(UnityUITextMeshProGlyphHelper.Tag.TagType.Action)
    {
      this.Clear();
    }

    public override void Clear()
    {
      this.actionId = -1;
      this.actionRange = AxisRange.Full;
      this._displayName = (string) null;
    }

    public static bool TryParseString(
      string text,
      int startIndex,
      int count,
      StringBuilder sb1,
      StringBuilder sb2,
      Dictionary<string, string> workDictionary,
      UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag> pool,
      out UnityUITextMeshProGlyphHelper.ActionTag result)
    {
      result = (UnityUITextMeshProGlyphHelper.ActionTag) null;
      if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex + count >= text.Length)
        return false;
      UnityUITextMeshProGlyphHelper.ParseAttributes(text, startIndex, count, sb1, sb2, workDictionary);
      if (workDictionary.Count == 0)
        return false;
      result = pool.Get();
      try
      {
        string str;
        if (workDictionary.TryGetValue("id", out str) || workDictionary.TryGetValue("actionid", out str))
        {
          result.actionId = int.Parse(str);
          if (ReInput.mapping.GetAction(result.actionId) == null)
            throw new Exception("Invalid Action Id: " + result.actionId.ToString());
        }
        else
        {
          if (!workDictionary.TryGetValue("name", out str) && !workDictionary.TryGetValue("actionname", out str))
            throw new Exception("Action name/id missing.");
          result.actionId = (ReInput.mapping.GetAction(str) ?? throw new Exception("Invalid Action name: " + str)).id;
        }
        if (workDictionary.TryGetValue("range", out str) || workDictionary.TryGetValue("actionrange", out str))
        {
          bool flag = false;
          for (int index = 0; index < UnityUITextMeshProGlyphHelper.s_axisRangeNames.Length; ++index)
          {
            if (string.Equals(str, UnityUITextMeshProGlyphHelper.s_axisRangeNames[index], StringComparison.OrdinalIgnoreCase))
            {
              result.actionRange = UnityUITextMeshProGlyphHelper.s_axisRangeValues[index];
              flag = true;
              break;
            }
          }
          if (!flag)
            throw new Exception("Invalid Action Range: " + str);
        }
        else
          result.actionRange = AxisRange.Full;
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        result.ReturnToPool();
        return false;
      }
    }
  }

  public sealed class PlayerTag : UnityUITextMeshProGlyphHelper.Tag
  {
    public int playerId;
    public string _displayName;

    public string displayName
    {
      get => this._displayName;
      set => this._displayName = value;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(typeof (UnityUITextMeshProGlyphHelper.ControllerElementTag).Name);
      stringBuilder.Append(": ");
      stringBuilder.Append("playerId = ");
      stringBuilder.Append(this.playerId);
      return stringBuilder.ToString();
    }

    public PlayerTag()
      : base(UnityUITextMeshProGlyphHelper.Tag.TagType.Player)
    {
      this.Clear();
    }

    public override void Clear()
    {
      this.playerId = -1;
      this._displayName = (string) null;
    }

    public static bool TryParseString(
      string text,
      int startIndex,
      int count,
      StringBuilder sb1,
      StringBuilder sb2,
      Dictionary<string, string> workDictionary,
      UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag> pool,
      out UnityUITextMeshProGlyphHelper.PlayerTag result)
    {
      result = (UnityUITextMeshProGlyphHelper.PlayerTag) null;
      if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex + count >= text.Length)
        return false;
      UnityUITextMeshProGlyphHelper.ParseAttributes(text, startIndex, count, sb1, sb2, workDictionary);
      if (workDictionary.Count == 0)
        return false;
      result = pool.Get();
      try
      {
        string str;
        if (workDictionary.TryGetValue("id", out str) || workDictionary.TryGetValue("playerid", out str))
        {
          result.playerId = int.Parse(str);
          if (ReInput.players.GetPlayer(result.playerId) == null)
            throw new Exception("Invalid Player Id: " + result.playerId.ToString());
        }
        else
        {
          if (!workDictionary.TryGetValue("name", out str) && !workDictionary.TryGetValue("playername", out str))
            throw new Exception("Player name/id missing.");
          result.playerId = (ReInput.players.GetPlayer(str) ?? throw new Exception("Invalid Player name: " + str)).id;
        }
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        result.ReturnToPool();
        return false;
      }
    }
  }

  public struct GlyphOrText : IEquatable<UnityUITextMeshProGlyphHelper.GlyphOrText>
  {
    public string glyphKey;
    public Sprite sprite;
    public string name;

    public override bool Equals(object obj)
    {
      return obj is UnityUITextMeshProGlyphHelper.GlyphOrText glyphOrText && string.Equals(glyphOrText.glyphKey, this.glyphKey, StringComparison.Ordinal) && (UnityEngine.Object) glyphOrText.sprite == (UnityEngine.Object) this.sprite && string.Equals(glyphOrText.name, this.name, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
      return ((17 * 29 + this.glyphKey.GetHashCode()) * 29 + ((object) this.sprite).GetHashCode()) * 29 + this.name.GetHashCode();
    }

    public bool Equals(UnityUITextMeshProGlyphHelper.GlyphOrText other)
    {
      return string.Equals(other.glyphKey, this.glyphKey, StringComparison.Ordinal) && (UnityEngine.Object) other.sprite == (UnityEngine.Object) this.sprite && string.Equals(other.name, this.name, StringComparison.Ordinal);
    }

    public static bool operator ==(
      UnityUITextMeshProGlyphHelper.GlyphOrText a,
      UnityUITextMeshProGlyphHelper.GlyphOrText b)
    {
      return string.Equals(a.glyphKey, b.glyphKey, StringComparison.Ordinal) && (UnityEngine.Object) a.sprite == (UnityEngine.Object) b.sprite && string.Equals(a.name, b.name, StringComparison.Ordinal);
    }

    public static bool operator !=(
      UnityUITextMeshProGlyphHelper.GlyphOrText a,
      UnityUITextMeshProGlyphHelper.GlyphOrText b)
    {
      return !(a == b);
    }
  }

  public class Asset
  {
    public uint id;
    public UnityUITextMeshProGlyphHelper.ITMProSpriteAsset _spriteAsset;
    public Material _material;
    public static uint s_idCounter;
    public static Shader __tmProShader;

    public UnityUITextMeshProGlyphHelper.ITMProSpriteAsset spriteAsset => this._spriteAsset;

    public Material material => this._material;

    public Asset(Material baseMaterial)
    {
      this.id = UnityUITextMeshProGlyphHelper.Asset.s_idCounter++;
      this._spriteAsset = UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CreateSpriteAsset();
      TMP_SpriteAsset spriteAsset = this._spriteAsset.GetSpriteAsset();
      spriteAsset.name = $"{typeof (UnityUITextMeshProGlyphHelper).Name} SpriteAsset {this.id.ToString()}";
      spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteAsset.name);
      this._material = UnityUITextMeshProGlyphHelper.Asset.CreateMaterial(baseMaterial, this.id);
      if (this._spriteAsset == null)
        return;
      spriteAsset.material = this.material;
      spriteAsset.materialHashCode = TMP_TextUtilities.GetSimpleHashCode(this.material.name);
    }

    public static Material CreateMaterial(Material baseMaterial, uint id)
    {
      Material material = (UnityEngine.Object) baseMaterial != (UnityEngine.Object) null ? new Material(baseMaterial) : new Material(UnityUITextMeshProGlyphHelper.Asset.tmProShader);
      material.name = $"{typeof (UnityUITextMeshProGlyphHelper).Name} Material {id.ToString()}";
      material.hideFlags = HideFlags.HideInHierarchy;
      return material;
    }

    public void Destroy()
    {
      if (this._spriteAsset != null)
      {
        this._spriteAsset.Destroy();
        this._spriteAsset = (UnityUITextMeshProGlyphHelper.ITMProSpriteAsset) null;
      }
      if (!((UnityEngine.Object) this._material != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this._material);
      this._material = (Material) null;
    }

    public static Shader tmProShader
    {
      get
      {
        if ((UnityEngine.Object) UnityUITextMeshProGlyphHelper.Asset.__tmProShader == (UnityEngine.Object) null)
        {
          ShaderUtilities.GetShaderPropertyIDs();
          UnityUITextMeshProGlyphHelper.Asset.__tmProShader = Shader.Find("TextMeshPro/Sprite");
        }
        return UnityUITextMeshProGlyphHelper.Asset.__tmProShader;
      }
    }
  }

  [Serializable]
  public struct TMProSpriteOptions : IEquatable<UnityUITextMeshProGlyphHelper.TMProSpriteOptions>
  {
    [Tooltip("Scale.")]
    [SerializeField]
    public float _scale;
    [Tooltip("This value will be multiplied by the Sprite width and height and applied to offset.")]
    [SerializeField]
    public Vector2 _offsetSizeMultiplier;
    [Tooltip("An extra offset that is cumulative with Offset Size Multiplier.")]
    [SerializeField]
    public Vector2 _extraOffset;
    [Tooltip("This value will be multiplied by the Sprite width applied to X Advance.")]
    [SerializeField]
    public float _xAdvanceWidthMultiplier;
    [Tooltip("An extra offset that is cumulative with X Advance Width Multiplier.")]
    [SerializeField]
    public float _extraXAdvance;

    public float scale
    {
      get => this._scale;
      set => this._scale = value;
    }

    public Vector2 offsetSizeMultiplier
    {
      get => this._offsetSizeMultiplier;
      set => this._offsetSizeMultiplier = value;
    }

    public Vector2 extraOffset
    {
      get => this._extraOffset;
      set => this._extraOffset = value;
    }

    public float xAdvanceWidthMultiplier
    {
      get => this._xAdvanceWidthMultiplier;
      set => this._xAdvanceWidthMultiplier = value;
    }

    public float extraXAdvance
    {
      get => this._extraXAdvance;
      set => this._extraXAdvance = value;
    }

    public static UnityUITextMeshProGlyphHelper.TMProSpriteOptions Default
    {
      get
      {
        return new UnityUITextMeshProGlyphHelper.TMProSpriteOptions()
        {
          scale = 1.5f,
          extraOffset = new Vector2(),
          offsetSizeMultiplier = new Vector2(0.0f, 0.75f),
          xAdvanceWidthMultiplier = 1f
        };
      }
    }

    public override bool Equals(object obj)
    {
      return obj is UnityUITextMeshProGlyphHelper.TMProSpriteOptions proSpriteOptions && (double) proSpriteOptions._scale == (double) this._scale && proSpriteOptions._offsetSizeMultiplier == this._offsetSizeMultiplier && proSpriteOptions._extraOffset == this._extraOffset && (double) proSpriteOptions._xAdvanceWidthMultiplier == (double) this._xAdvanceWidthMultiplier && (double) proSpriteOptions._extraXAdvance == (double) this._extraXAdvance;
    }

    public override int GetHashCode()
    {
      return ((((17 * 29 + this._scale.GetHashCode()) * 29 + this._offsetSizeMultiplier.GetHashCode()) * 29 + this._extraOffset.GetHashCode()) * 29 + this._xAdvanceWidthMultiplier.GetHashCode()) * 29 + this._extraXAdvance.GetHashCode();
    }

    public bool Equals(
      UnityUITextMeshProGlyphHelper.TMProSpriteOptions other)
    {
      return (double) other._scale == (double) this._scale && other._offsetSizeMultiplier == this._offsetSizeMultiplier && other._extraOffset == this._extraOffset && (double) other._xAdvanceWidthMultiplier == (double) this._xAdvanceWidthMultiplier && (double) other._extraXAdvance == (double) this._extraXAdvance;
    }

    public static bool operator ==(
      UnityUITextMeshProGlyphHelper.TMProSpriteOptions a,
      UnityUITextMeshProGlyphHelper.TMProSpriteOptions b)
    {
      return (double) a._scale == (double) b._scale && a._offsetSizeMultiplier == b._offsetSizeMultiplier && a._extraOffset == b._extraOffset && (double) a._xAdvanceWidthMultiplier == (double) b._xAdvanceWidthMultiplier && (double) a._extraXAdvance == (double) b._extraXAdvance;
    }

    public static bool operator !=(
      UnityUITextMeshProGlyphHelper.TMProSpriteOptions a,
      UnityUITextMeshProGlyphHelper.TMProSpriteOptions b)
    {
      return !(a == b);
    }
  }

  [Serializable]
  public struct SpriteMaterialProperties
  {
    [Tooltip("Sprite material color.")]
    [SerializeField]
    public Color _color;

    public Color color
    {
      get => this._color;
      set => this._color = value;
    }

    public static UnityUITextMeshProGlyphHelper.SpriteMaterialProperties Default
    {
      get
      {
        return new UnityUITextMeshProGlyphHelper.SpriteMaterialProperties()
        {
          _color = Color.white
        };
      }
    }
  }

  public interface ITMProSprite
  {
    uint id { get; set; }

    float width { get; set; }

    float height { get; set; }

    float xOffset { get; set; }

    float yOffset { get; set; }

    float xAdvance { get; set; }

    Vector2 position { get; set; }

    Vector2 pivot { get; set; }

    float scale { get; set; }

    string name { get; set; }

    uint unicode { get; set; }

    int hashCode { get; set; }

    Sprite sprite { get; set; }
  }

  public interface ITMProSpriteAsset
  {
    int spriteCount { get; }

    Texture spriteSheet { get; set; }

    TMP_SpriteAsset GetSpriteAsset();

    UnityUITextMeshProGlyphHelper.ITMProSprite GetSprite(int index);

    void AddSprite(UnityUITextMeshProGlyphHelper.ITMProSprite sprite);

    bool Contains(string spriteName);

    void Clear();

    void UpdateLookupTables();

    void Destroy();
  }

  public static class TMProAssetVersionHelper
  {
    public static bool _isVersionSupportedChecked;

    public static bool CheckVersionSupported()
    {
      bool flag = UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.CheckVersionSupported();
      if (UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper._isVersionSupportedChecked)
        return flag;
      int num = flag ? 1 : 0;
      UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper._isVersionSupportedChecked = true;
      return flag;
    }

    public static UnityUITextMeshProGlyphHelper.ITMProSprite CreateSprite()
    {
      return !UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CheckVersionSupported() ? (UnityUITextMeshProGlyphHelper.ITMProSprite) new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0() : (UnityUITextMeshProGlyphHelper.ITMProSprite) new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0();
    }

    public static UnityUITextMeshProGlyphHelper.ITMProSpriteAsset CreateSpriteAsset()
    {
      return !UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CheckVersionSupported() ? (UnityUITextMeshProGlyphHelper.ITMProSpriteAsset) new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0.TMPro_SpriteAsset() : (UnityUITextMeshProGlyphHelper.ITMProSpriteAsset) new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteAsset();
    }
  }

  public class TMProSprite_AssetV1_0_0 : UnityUITextMeshProGlyphHelper.ITMProSprite
  {
    public TMP_Sprite spriteInfo;

    public TMProSprite_AssetV1_0_0() => this.spriteInfo = new TMP_Sprite();

    public uint id
    {
      get => (uint) this.spriteInfo.id;
      set => this.spriteInfo.id = (int) value;
    }

    public float width
    {
      get => this.spriteInfo.width;
      set => this.spriteInfo.width = value;
    }

    public float height
    {
      get => this.spriteInfo.height;
      set => this.spriteInfo.height = value;
    }

    public float xOffset
    {
      get => this.spriteInfo.xOffset;
      set => this.spriteInfo.xOffset = value;
    }

    public float yOffset
    {
      get => this.spriteInfo.yOffset;
      set => this.spriteInfo.yOffset = value;
    }

    public float xAdvance
    {
      get => this.spriteInfo.xAdvance;
      set => this.spriteInfo.xAdvance = value;
    }

    public Vector2 position
    {
      get => new Vector2(this.spriteInfo.x, this.spriteInfo.y);
      set
      {
        this.spriteInfo.x = value.x;
        this.spriteInfo.y = value.y;
      }
    }

    public Vector2 pivot
    {
      get => this.spriteInfo.pivot;
      set => this.spriteInfo.pivot = value;
    }

    public float scale
    {
      get => this.spriteInfo.scale;
      set => this.spriteInfo.scale = value;
    }

    public string name
    {
      get => this.spriteInfo.name;
      set => this.spriteInfo.name = value;
    }

    public uint unicode
    {
      get => (uint) this.spriteInfo.unicode;
      set => this.spriteInfo.unicode = (int) value;
    }

    public int hashCode
    {
      get => this.spriteInfo.hashCode;
      set => this.spriteInfo.hashCode = value;
    }

    public Sprite sprite
    {
      get => this.spriteInfo.sprite;
      set => this.spriteInfo.sprite = value;
    }

    public class TMPro_SpriteAsset : UnityUITextMeshProGlyphHelper.ITMProSpriteAsset
    {
      public TMP_SpriteAsset _spriteAsset;
      public List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0> _sprites;

      public int spriteCount => this._sprites.Count;

      public Texture spriteSheet
      {
        get => this._spriteAsset.spriteSheet;
        set => this._spriteAsset.spriteSheet = value;
      }

      public TMPro_SpriteAsset()
      {
        this._spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        this._spriteAsset.hideFlags = HideFlags.DontSave;
        if (this._spriteAsset.spriteInfoList == null)
          this._spriteAsset.spriteInfoList = new List<TMP_Sprite>();
        this._sprites = new List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0>();
      }

      public TMP_SpriteAsset GetSpriteAsset() => this._spriteAsset;

      public UnityUITextMeshProGlyphHelper.ITMProSprite GetSprite(int index)
      {
        return (uint) index >= (uint) this._sprites.Count ? (UnityUITextMeshProGlyphHelper.ITMProSprite) null : (UnityUITextMeshProGlyphHelper.ITMProSprite) this._sprites[index];
      }

      public void AddSprite(UnityUITextMeshProGlyphHelper.ITMProSprite sprite)
      {
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0 proSpriteAssetV100 = sprite as UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0;
        if (sprite == null)
          throw new ArgumentException();
        proSpriteAssetV100.spriteInfo.id = this._spriteAsset.spriteInfoList.Count;
        this._spriteAsset.spriteInfoList.Add(proSpriteAssetV100.spriteInfo);
        this._sprites.Add(proSpriteAssetV100);
      }

      public void Clear()
      {
        this._spriteAsset.spriteInfoList.Clear();
        this._sprites.Clear();
      }

      public bool Contains(string spriteName)
      {
        int count = this._sprites.Count;
        for (int index = 0; index < count; ++index)
        {
          if (string.Equals(this._sprites[index].name, spriteName, StringComparison.Ordinal))
            return true;
        }
        return false;
      }

      public void UpdateLookupTables() => this._spriteAsset.UpdateLookupTables();

      public void Destroy()
      {
        if ((UnityEngine.Object) this._spriteAsset == (UnityEngine.Object) null)
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this._spriteAsset);
        this._spriteAsset = (TMP_SpriteAsset) null;
      }
    }
  }

  public class TMProSprite_AssetV1_1_0 : UnityUITextMeshProGlyphHelper.ITMProSprite
  {
    public UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph _spriteGlyph;
    public UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter _spriteCharacter;
    public static bool? s_isVersionSupported;

    public TMProSprite_AssetV1_1_0()
    {
      this._spriteGlyph = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph();
      this._spriteCharacter = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter();
      this._spriteCharacter.glyph = this._spriteGlyph.source;
    }

    public UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph spriteGlyph
    {
      get => this._spriteGlyph;
    }

    public UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter spriteCharacter
    {
      get => this._spriteCharacter;
    }

    public uint id
    {
      get => this._spriteGlyph.source.index;
      set
      {
        this._spriteGlyph.source.index = value;
        this._spriteCharacter.glyphIndex = value;
      }
    }

    public float width
    {
      get => this._spriteGlyph.source.metrics.width;
      set
      {
        this._spriteGlyph.source.metrics = this._spriteGlyph.source.metrics with
        {
          width = value
        };
        this._spriteGlyph.source.glyphRect = this._spriteGlyph.source.glyphRect with
        {
          width = (int) value
        };
      }
    }

    public float height
    {
      get => this._spriteGlyph.source.metrics.height;
      set
      {
        this._spriteGlyph.source.metrics = this._spriteGlyph.source.metrics with
        {
          height = value
        };
        this._spriteGlyph.source.glyphRect = this._spriteGlyph.source.glyphRect with
        {
          height = (int) value
        };
      }
    }

    public float xOffset
    {
      get => this._spriteGlyph.source.metrics.horizontalBearingX;
      set
      {
        this._spriteGlyph.source.metrics = this._spriteGlyph.source.metrics with
        {
          horizontalBearingX = value
        };
      }
    }

    public float yOffset
    {
      get => this._spriteGlyph.source.metrics.horizontalBearingY;
      set
      {
        this._spriteGlyph.source.metrics = this._spriteGlyph.source.metrics with
        {
          horizontalBearingY = value
        };
      }
    }

    public float xAdvance
    {
      get => this._spriteGlyph.source.metrics.horizontalAdvance;
      set
      {
        this._spriteGlyph.source.metrics = this._spriteGlyph.source.metrics with
        {
          horizontalAdvance = value
        };
      }
    }

    public Vector2 position
    {
      get
      {
        GlyphRect glyphRect = this._spriteGlyph.source.glyphRect;
        return new Vector2((float) glyphRect.x, (float) glyphRect.y);
      }
      set
      {
        this._spriteGlyph.source.glyphRect = this._spriteGlyph.source.glyphRect with
        {
          x = (int) value.x,
          y = (int) value.y
        };
      }
    }

    public Vector2 pivot
    {
      get => new Vector2();
      set
      {
      }
    }

    public float scale
    {
      get => this._spriteCharacter.scale;
      set => this._spriteCharacter.scale = value;
    }

    public string name
    {
      get => this._spriteCharacter.name;
      set => this._spriteCharacter.name = value;
    }

    public uint unicode
    {
      get => this._spriteCharacter.unicode;
      set => this._spriteCharacter.unicode = value;
    }

    public int hashCode
    {
      get => 0;
      set
      {
      }
    }

    public Sprite sprite
    {
      get => this._spriteGlyph.sprite;
      set => this._spriteGlyph.sprite = value;
    }

    public static bool CheckVersionSupported()
    {
      if (UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported.HasValue)
        return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported.Value;
      try
      {
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter proSpriteCharacter = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter();
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph tmProSpriteGlyph = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph();
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteAsset tmProSpriteAsset = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteAsset();
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported = new bool?(true);
      }
      catch
      {
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported = new bool?(false);
      }
      return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported.Value;
    }

    public class TMPro_SpriteCharacter
    {
      public const string typeFullName = "TMPro.TMP_SpriteCharacter";
      public object _source;
      public PropertyInfo _glyph;
      public PropertyInfo _unicode;
      public PropertyInfo _name;
      public PropertyInfo _scale;
      public PropertyInfo _glyphIndex;
      public static System.Type s_type;

      public object source => this._source;

      public Glyph glyph
      {
        get => (Glyph) this._glyph.GetValue(this._source);
        set => this._glyph.SetValue(this._source, (object) value);
      }

      public uint unicode
      {
        get => (uint) this._unicode.GetValue(this._source);
        set
        {
          if (value == 0U)
            value = 65534U;
          this._unicode.SetValue(this._source, (object) value);
        }
      }

      public string name
      {
        get => (string) this._name.GetValue(this._source);
        set => this._name.SetValue(this._source, (object) value);
      }

      public float scale
      {
        get => (float) this._scale.GetValue(this._source);
        set => this._scale.SetValue(this._source, (object) value);
      }

      public uint glyphIndex
      {
        get => (uint) this._glyphIndex.GetValue(this._source);
        set => this._glyphIndex.SetValue(this._source, (object) value);
      }

      public TMPro_SpriteCharacter()
      {
        System.Type reflectedType = UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter.GetReflectedType();
        this._source = !(reflectedType == (System.Type) null) ? Activator.CreateInstance(reflectedType) : throw new ArgumentNullException("type");
        if (this._source == null)
          throw new ArgumentNullException(nameof (source));
        this._glyph = reflectedType.GetProperty(nameof (glyph), BindingFlags.Instance | BindingFlags.Public);
        if (this._glyph == (PropertyInfo) null)
          throw new ArgumentNullException(nameof (glyph));
        this._unicode = reflectedType.GetProperty(nameof (unicode), BindingFlags.Instance | BindingFlags.Public);
        if (this._unicode == (PropertyInfo) null)
          throw new ArgumentNullException(nameof (unicode));
        this._name = reflectedType.GetProperty(nameof (name), BindingFlags.Instance | BindingFlags.Public);
        if (this._name == (PropertyInfo) null)
          throw new ArgumentNullException(nameof (name));
        this._scale = reflectedType.GetProperty(nameof (scale), BindingFlags.Instance | BindingFlags.Public);
        if (this._scale == (PropertyInfo) null)
          throw new ArgumentNullException(nameof (scale));
        this._glyphIndex = reflectedType.GetProperty(nameof (glyphIndex), BindingFlags.Instance | BindingFlags.Public);
        if (this._glyphIndex == (PropertyInfo) null)
          throw new ArgumentNullException(nameof (glyphIndex));
      }

      public static System.Type GetReflectedType()
      {
        if (UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter.s_type != (System.Type) null)
          return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter.s_type;
        System.Type[] types = typeof (TMP_SpriteAsset).Assembly.GetTypes();
        if (types == null)
          return (System.Type) null;
        for (int index = 0; index < types.Length; ++index)
        {
          if (string.Equals(types[index].FullName, "TMPro.TMP_SpriteCharacter"))
          {
            UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter.s_type = types[index];
            break;
          }
        }
        return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter.s_type;
      }
    }

    public class TMPro_SpriteGlyph
    {
      public const string typeFullName = "TMPro.TMP_SpriteGlyph";
      public Glyph _source;
      public FieldInfo _sprite;
      public static System.Type s_type;

      public Glyph source => this._source;

      public Sprite sprite
      {
        get => (Sprite) this._sprite.GetValue((object) this._source);
        set => this._sprite.SetValue((object) this._source, (object) value);
      }

      public TMPro_SpriteGlyph()
      {
        System.Type reflectedType = UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.GetReflectedType();
        this._source = !(reflectedType == (System.Type) null) ? (Glyph) Activator.CreateInstance(reflectedType) : throw new ArgumentNullException("type");
        if (this._source == null)
          throw new ArgumentNullException("glyph");
        this._sprite = reflectedType.GetField(nameof (sprite), BindingFlags.Instance | BindingFlags.Public);
        if (this._sprite == (FieldInfo) null)
          throw new ArgumentNullException(nameof (sprite));
        UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.Initialize(this._source);
      }

      public static System.Type GetReflectedType()
      {
        if (UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.s_type != (System.Type) null)
          return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.s_type;
        System.Type[] types = typeof (TMP_SpriteAsset).Assembly.GetTypes();
        if (types == null)
          return (System.Type) null;
        for (int index = 0; index < types.Length; ++index)
        {
          if (string.Equals(types[index].FullName, "TMPro.TMP_SpriteGlyph"))
          {
            UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.s_type = types[index];
            break;
          }
        }
        return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.s_type;
      }

      public static void Initialize(Glyph glyph)
      {
        glyph.scale = 1f;
        glyph.atlasIndex = 0;
      }
    }

    public class TMPro_SpriteAsset : UnityUITextMeshProGlyphHelper.ITMProSpriteAsset
    {
      public PropertyInfo _spriteCharacterTable;
      public PropertyInfo _spriteGlyphTable;
      public IList _spriteCharacterTableList;
      public IList _spriteGlyphTableList;
      public List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0> _sprites;
      public TMP_SpriteAsset _spriteAsset;

      public int spriteCount => this._sprites.Count;

      public Texture spriteSheet
      {
        get => this._spriteAsset.spriteSheet;
        set => this._spriteAsset.spriteSheet = value;
      }

      public TMPro_SpriteAsset()
      {
        this._spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        this._spriteAsset.hideFlags = HideFlags.DontSave;
        System.Type type = typeof (TMP_SpriteAsset);
        PropertyInfo propertyInfo = !(type == (System.Type) null) ? type.GetProperty("version", BindingFlags.Instance | BindingFlags.Public) : throw new ArgumentNullException("type");
        if (propertyInfo == (PropertyInfo) null)
          throw new ArgumentNullException("version");
        propertyInfo.SetValue((object) this._spriteAsset, (object) "1.1.0");
        this._spriteCharacterTable = type.GetProperty("spriteCharacterTable", BindingFlags.Instance | BindingFlags.Public);
        this._spriteCharacterTableList = !(this._spriteCharacterTable == (PropertyInfo) null) ? (IList) this._spriteCharacterTable.GetValue((object) this._spriteAsset) : throw new ArgumentNullException("spriteCharacterTable");
        if (this._spriteCharacterTableList == null)
          throw new ArgumentNullException("spriteCharacterTableList");
        this._spriteGlyphTable = type.GetProperty("spriteGlyphTable", BindingFlags.Instance | BindingFlags.Public);
        this._spriteGlyphTableList = !(this._spriteGlyphTable == (PropertyInfo) null) ? (IList) this._spriteGlyphTable.GetValue((object) this._spriteAsset) : throw new ArgumentNullException("spriteGlyphTable");
        if (this._spriteGlyphTableList == null)
          throw new ArgumentNullException("spriteGlyphTableList");
        this._sprites = new List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0>();
      }

      public TMP_SpriteAsset GetSpriteAsset() => this._spriteAsset;

      public UnityUITextMeshProGlyphHelper.ITMProSprite GetSprite(int index)
      {
        return (uint) index >= (uint) this._sprites.Count ? (UnityUITextMeshProGlyphHelper.ITMProSprite) null : (UnityUITextMeshProGlyphHelper.ITMProSprite) this._sprites[index];
      }

      public void AddSprite(UnityUITextMeshProGlyphHelper.ITMProSprite sprite)
      {
        if (!(sprite is UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0 proSpriteAssetV110))
          throw new ArgumentException();
        proSpriteAssetV110.id = (uint) this._spriteCharacterTableList.Count;
        this._spriteCharacterTableList.Add(proSpriteAssetV110.spriteCharacter.source);
        this._spriteGlyphTableList.Add((object) proSpriteAssetV110.spriteGlyph.source);
        this._sprites.Add(proSpriteAssetV110);
      }

      public void Clear()
      {
        this._spriteCharacterTableList.Clear();
        this._spriteGlyphTableList.Clear();
        this._sprites.Clear();
      }

      public bool Contains(string spriteName)
      {
        int count = this._sprites.Count;
        for (int index = 0; index < count; ++index)
        {
          if (string.Equals(this._sprites[index].name, spriteName, StringComparison.Ordinal))
            return true;
        }
        return false;
      }

      public void UpdateLookupTables() => this._spriteAsset.UpdateLookupTables();

      public void Destroy()
      {
        if ((UnityEngine.Object) this._spriteAsset == (UnityEngine.Object) null)
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this._spriteAsset);
        this._spriteAsset = (TMP_SpriteAsset) null;
      }
    }
  }

  public enum DisplayType
  {
    Glyph,
    Text,
    GlyphOrText,
  }
}
