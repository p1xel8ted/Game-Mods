// Decompiled with JetBrains decompiler
// Type: AddOutline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityFx.Outline;

#nullable disable
public class AddOutline : MonoBehaviour
{
  public OutlineEffect Outliner;
  public Interaction interaction;
  public int Layer = 2;
  public GameObject OutlineTarget;
  public bool removed;

  public void OnEnable()
  {
    if ((Object) this.Outliner == (Object) null)
      this.Outliner = Camera.main.GetComponent<OutlineEffect>();
    if ((Object) this.Outliner != (Object) null)
      this.Outliner.OutlineLayers[this.Layer].Add((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.interaction.indicateHighlightEnd.AddListener(new UnityAction(this.Highlighted));
    this.interaction.indicateHighlight.AddListener(new UnityAction(this.HighlightedRemoved));
  }

  public void Highlighted()
  {
    Debug.Log((object) nameof (Highlighted));
    if ((Object) this.Outliner != (Object) null)
      this.Outliner.OutlineLayers[this.Layer].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.Outliner.OutlineLayers[this.Layer].Remove(this.gameObject);
  }

  public void HighlightedRemoved()
  {
    Debug.Log((object) "HighlightRemoved");
    this.Outliner.OutlineLayers[this.Layer].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.Outliner.OutlineLayers[this.Layer].Remove(this.gameObject);
  }

  public void OnDisable()
  {
    if ((Object) this.Outliner != (Object) null)
      this.Outliner.OutlineLayers[this.Layer].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.interaction.indicateHighlightEnd.RemoveListener(new UnityAction(this.Highlighted));
    this.interaction.indicateHighlight.RemoveListener(new UnityAction(this.HighlightedRemoved));
  }
}
