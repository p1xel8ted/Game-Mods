// Decompiled with JetBrains decompiler
// Type: AddOutline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityFx.Outline;

#nullable disable
public class AddOutline : MonoBehaviour
{
  protected OutlineEffect Outliner;
  public Interaction interaction;
  public int Layer = 2;
  public GameObject OutlineTarget;
  private bool removed;

  private void OnEnable()
  {
    if ((Object) this.Outliner == (Object) null)
      this.Outliner = Camera.main.GetComponent<OutlineEffect>();
    if ((Object) this.Outliner != (Object) null)
      this.Outliner.OutlineLayers[this.Layer].Add((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.interaction.indicateHighlightEnd.AddListener(new UnityAction(this.Highlighted));
    this.interaction.indicateHighlight.AddListener(new UnityAction(this.HighlightedRemoved));
  }

  private void Highlighted()
  {
    Debug.Log((object) nameof (Highlighted));
    if ((Object) this.Outliner != (Object) null)
      this.Outliner.OutlineLayers[this.Layer].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.Outliner.OutlineLayers[this.Layer].Remove(this.gameObject);
  }

  private void HighlightedRemoved()
  {
    Debug.Log((object) "HighlightRemoved");
    this.Outliner.OutlineLayers[this.Layer].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.Outliner.OutlineLayers[this.Layer].Remove(this.gameObject);
  }

  private void OnDisable()
  {
    if ((Object) this.Outliner != (Object) null)
      this.Outliner.OutlineLayers[this.Layer].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
    this.interaction.indicateHighlightEnd.RemoveListener(new UnityAction(this.Highlighted));
    this.interaction.indicateHighlight.RemoveListener(new UnityAction(this.HighlightedRemoved));
  }
}
