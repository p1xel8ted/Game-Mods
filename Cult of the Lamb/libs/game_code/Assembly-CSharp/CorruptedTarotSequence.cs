// Decompiled with JetBrains decompiler
// Type: CorruptedTarotSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CorruptedTarotSequence : MonoBehaviour
{
  public GameObject lightning;
  public SkeletonGraphic originalSkeletonGraphic;
  public int numberOfDuplicates = 7;
  public List<GameObject> duplicatedSkeletonGraphics = new List<GameObject>();
  public List<Vector3> cardStartPos = new List<Vector3>();
  public string clipAttachmentPrefix = "Card";
  public float pushDistance = 5f;
  public float pushDuration = 1f;

  public void Start()
  {
    if ((Object) this.originalSkeletonGraphic == (Object) null)
    {
      Debug.LogError((object) "Original SkeletonGraphic is not set.");
    }
    else
    {
      foreach (GameObject duplicatedSkeletonGraphic in this.duplicatedSkeletonGraphics)
      {
        this.cardStartPos.Add(duplicatedSkeletonGraphic.transform.position);
        duplicatedSkeletonGraphic.transform.localPosition = Vector3.zero;
      }
    }
  }

  public void ExplosionSequence()
  {
    int index = 0;
    this.lightning.gameObject.SetActive(true);
    foreach (GameObject duplicatedSkeletonGraphic in this.duplicatedSkeletonGraphics)
    {
      GameObject g = duplicatedSkeletonGraphic;
      g.transform.DOKill();
      new Material(g.GetComponent<SkeletonGraphic>().material).DOFloat(0.0f, "_FillColorLerpFade", 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      g.transform.DOMove(this.cardStartPos[index], 0.65f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => g.transform.DOShakePosition(Random.Range(0.33f, 0.55f), 0.66f, fadeOut: false).SetUpdate<Tweener>(true).SetLoops<Tweener>(999)));
      ++index;
    }
  }

  public void DuplicateAndSetClipping()
  {
    this.duplicatedSkeletonGraphics.Clear();
    for (int index = 1; index <= this.numberOfDuplicates; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.originalSkeletonGraphic.gameObject, this.originalSkeletonGraphic.transform.parent);
      gameObject.name = this.clipAttachmentPrefix + index.ToString();
      gameObject.transform.localPosition = new Vector3(this.originalSkeletonGraphic.transform.localPosition.x, this.originalSkeletonGraphic.transform.localPosition.y, this.originalSkeletonGraphic.transform.localPosition.z);
      this.duplicatedSkeletonGraphics.Add(gameObject);
      SkeletonGraphic component = gameObject.GetComponent<SkeletonGraphic>();
      component.MatchRectTransformWithBounds();
      component.UpdateInterval = 0;
      this.SetClipping(component, this.clipAttachmentPrefix + index.ToString());
    }
    this.originalSkeletonGraphic.enabled = false;
  }

  public void SetClip()
  {
    this.SetClipping(this.gameObject.GetComponent<SkeletonGraphic>(), this.gameObject.name);
  }

  public void SetClipping(SkeletonGraphic skeletonGraphic, string clipAttachmentName)
  {
    string slotName = "Card";
    Attachment attachment = skeletonGraphic.Skeleton.GetAttachment(slotName, clipAttachmentName);
    if (attachment == null)
    {
      Debug.LogError((object) $"Clip attachment not found: {clipAttachmentName} in slot: {slotName}");
    }
    else
    {
      Slot slot = skeletonGraphic.Skeleton.FindSlot(slotName);
      if (slot == null)
        Debug.LogError((object) ("Slot not found: " + slotName));
      else
        slot.Attachment = attachment;
    }
  }
}
