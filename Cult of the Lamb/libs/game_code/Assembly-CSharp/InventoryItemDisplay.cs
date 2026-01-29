// Decompiled with JetBrains decompiler
// Type: InventoryItemDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class InventoryItemDisplay : BaseMonoBehaviour
{
  public Dictionary<InventoryItem.ITEM_TYPE, Sprite> Images = new Dictionary<InventoryItem.ITEM_TYPE, Sprite>();
  [SerializeField]
  public List<InventoryItemDisplay.MyDictionaryEntry> ItemImages;
  public Dictionary<InventoryItem.ITEM_TYPE, Sprite> myDictionary;
  public List<Sprite> DoctrineStoneSprites;
  public Sprite sprite;
  public SpriteRenderer spriteRenderer;
  public Image image;
  public Image outline;
  public bool IsScaling;

  public List<InventoryItemDisplay.MyDictionaryEntry> imgs => this.ItemImages;

  public void Awake() => this.GetItemImages();

  public void GetItemImages()
  {
    this.myDictionary = new Dictionary<InventoryItem.ITEM_TYPE, Sprite>();
    foreach (InventoryItemDisplay.MyDictionaryEntry itemImage in this.ItemImages)
      this.myDictionary.Add(itemImage.key, itemImage.value);
  }

  public void Start()
  {
    if (!((UnityEngine.Object) this.image == (UnityEngine.Object) null) || !((UnityEngine.Object) this.spriteRenderer == (UnityEngine.Object) null))
      return;
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
  }

  public void DoScale()
  {
    if (this.IsScaling)
      return;
    this.IsScaling = true;
    Transform transform = this.spriteRenderer.transform;
    transform.DOKill();
    transform.localScale = Vector3.zero;
    transform.DOScale(1f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.IsScaling = false));
  }

  public void SetImage(Sprite sprite, bool doScale = true)
  {
    this.StopAllCoroutines();
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    if ((UnityEngine.Object) this.spriteRenderer != (UnityEngine.Object) null)
    {
      this.spriteRenderer.sprite = sprite;
      if (doScale)
        this.DoScale();
    }
    if ((UnityEngine.Object) this.image != (UnityEngine.Object) null)
    {
      this.image.enabled = true;
      this.image.sprite = (Sprite) null;
      this.image.preserveAspect = true;
      this.image.sprite = sprite;
    }
    if (!((UnityEngine.Object) this.outline != (UnityEngine.Object) null))
      return;
    this.outline.enabled = true;
    this.outline.sprite = (Sprite) null;
    this.outline.preserveAspect = true;
    this.outline.sprite = sprite;
  }

  public void SetImage(InventoryItem.ITEM_TYPE Type, bool andScale = true)
  {
    if (this.myDictionary == null)
      this.Awake();
    this.StopAllCoroutines();
    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    if ((UnityEngine.Object) this.spriteRenderer != (UnityEngine.Object) null)
    {
      switch (Type)
      {
        case InventoryItem.ITEM_TYPE.NONE:
          this.spriteRenderer.sprite = (Sprite) null;
          break;
        case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
          this.spriteRenderer.sprite = this.DoctrineStoneSprites[Mathf.Clamp(DataManager.Instance.CompletedDoctrineStones, 0, this.DoctrineStoneSprites.Count - 1)];
          break;
        default:
          if (this.myDictionary.ContainsKey(Type))
          {
            this.spriteRenderer.sprite = this.myDictionary[Type];
            break;
          }
          break;
      }
      if (andScale && this.gameObject.activeInHierarchy)
        this.DoScale();
    }
    if ((UnityEngine.Object) this.image != (UnityEngine.Object) null)
    {
      this.image.enabled = true;
      this.image.sprite = (Sprite) null;
      this.image.preserveAspect = true;
      switch (Type)
      {
        case InventoryItem.ITEM_TYPE.NONE:
          this.image.enabled = false;
          break;
        case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
          this.image.sprite = this.DoctrineStoneSprites[Mathf.Clamp(DataManager.Instance.CompletedDoctrineStones, 0, this.DoctrineStoneSprites.Count - 1)];
          break;
        default:
          if (!this.myDictionary.ContainsKey(Type))
          {
            this.image.sprite = (Sprite) null;
            Debug.Log((object) Type);
            break;
          }
          this.image.sprite = this.myDictionary[Type];
          break;
      }
    }
    if (!((UnityEngine.Object) this.outline != (UnityEngine.Object) null))
      return;
    this.outline.enabled = true;
    this.outline.sprite = (Sprite) null;
    this.outline.preserveAspect = true;
    if (Type == InventoryItem.ITEM_TYPE.NONE)
      this.outline.enabled = false;
    else if (Type == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      this.outline.sprite = this.DoctrineStoneSprites[Mathf.Clamp(DataManager.Instance.CompletedDoctrineStones, 0, this.DoctrineStoneSprites.Count - 1)];
    else
      this.outline.sprite = this.myDictionary[Type];
  }

  public Sprite GetImage(InventoryItem.ITEM_TYPE Type)
  {
    if (this.myDictionary == null)
      this.GetItemImages();
    if (Type == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      return this.DoctrineStoneSprites[Mathf.Clamp(DataManager.Instance.CompletedDoctrineStones, 0, this.DoctrineStoneSprites.Count - 1)];
    Sprite sprite;
    return this.myDictionary.TryGetValue(Type, out sprite) ? sprite : (Sprite) null;
  }

  [CompilerGenerated]
  public void \u003CDoScale\u003Eb__15_0() => this.IsScaling = false;

  [Serializable]
  public class MyDictionaryEntry
  {
    public InventoryItem.ITEM_TYPE key;
    public Sprite value;
  }
}
