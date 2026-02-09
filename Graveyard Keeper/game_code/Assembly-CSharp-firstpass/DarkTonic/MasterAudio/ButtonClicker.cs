// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.ButtonClicker
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[AddComponentMenu("Dark Tonic/Master Audio/Button Clicker")]
public class ButtonClicker : MonoBehaviour
{
  public const float SmallSizeMultiplier = 0.9f;
  public const float LargeSizeMultiplier = 1.1f;
  public bool resizeOnClick = true;
  public bool resizeClickAllSiblings;
  public bool resizeOnHover;
  public bool resizeHoverAllSiblings;
  public string mouseDownSound = string.Empty;
  public string mouseUpSound = string.Empty;
  public string mouseClickSound = string.Empty;
  public string mouseOverSound = string.Empty;
  public string mouseOutSound = string.Empty;
  public Vector3 _originalSize;
  public Vector3 _smallerSize;
  public Vector3 _largerSize;
  public Transform _trans;
  public Dictionary<Transform, Vector3> _siblingClickScaleByTransform = new Dictionary<Transform, Vector3>();
  public Dictionary<Transform, Vector3> _siblingHoverScaleByTransform = new Dictionary<Transform, Vector3>();

  public void Awake()
  {
    this._trans = this.transform;
    this._originalSize = this._trans.localScale;
    this._smallerSize = this._originalSize * 0.9f;
    this._largerSize = this._originalSize * 1.1f;
    Transform parent = this._trans.parent;
    if (this.resizeOnClick && this.resizeClickAllSiblings && (Object) parent != (Object) null)
    {
      for (int index = 0; index < parent.transform.childCount; ++index)
      {
        Transform child = parent.transform.GetChild(index);
        this._siblingClickScaleByTransform.Add(child, child.localScale);
      }
    }
    if (!this.resizeOnHover || !this.resizeHoverAllSiblings || (Object) parent == (Object) null)
      return;
    for (int index = 0; index < parent.transform.childCount; ++index)
    {
      Transform child = parent.transform.GetChild(index);
      this._siblingHoverScaleByTransform.Add(child, child.localScale);
    }
  }

  public void OnPress(bool isDown)
  {
    if (isDown)
    {
      if (!this.enabled)
        return;
      DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(this.mouseDownSound);
      if (!this.resizeOnClick)
        return;
      this._trans.localScale = this._smallerSize;
      Dictionary<Transform, Vector3>.Enumerator enumerator = this._siblingClickScaleByTransform.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<Transform, Vector3> current = enumerator.Current;
        Transform key = current.Key;
        current = enumerator.Current;
        Vector3 vector3 = current.Value * 0.9f;
        key.localScale = vector3;
      }
    }
    else
    {
      if (this.enabled)
        DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(this.mouseUpSound);
      if (!this.resizeOnClick)
        return;
      this._trans.localScale = this._originalSize;
      Dictionary<Transform, Vector3>.Enumerator enumerator = this._siblingClickScaleByTransform.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<Transform, Vector3> current = enumerator.Current;
        Transform key = current.Key;
        current = enumerator.Current;
        Vector3 vector3 = current.Value;
        key.localScale = vector3;
      }
    }
  }

  public void OnClick()
  {
    if (!this.enabled)
      return;
    DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(this.mouseClickSound);
  }

  public void OnHover(bool isOver)
  {
    if (isOver)
    {
      if (!this.enabled)
        return;
      DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(this.mouseOverSound);
      if (!this.resizeOnHover)
        return;
      this._trans.localScale = this._largerSize;
      Dictionary<Transform, Vector3>.Enumerator enumerator = this._siblingHoverScaleByTransform.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<Transform, Vector3> current = enumerator.Current;
        Transform key = current.Key;
        current = enumerator.Current;
        Vector3 vector3 = current.Value * 1.1f;
        key.localScale = vector3;
      }
    }
    else
    {
      if (this.enabled)
        DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget(this.mouseOutSound);
      if (!this.resizeOnHover)
        return;
      this._trans.localScale = this._originalSize;
      Dictionary<Transform, Vector3>.Enumerator enumerator = this._siblingHoverScaleByTransform.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<Transform, Vector3> current = enumerator.Current;
        Transform key = current.Key;
        current = enumerator.Current;
        Vector3 vector3 = current.Value;
        key.localScale = vector3;
      }
    }
  }
}
