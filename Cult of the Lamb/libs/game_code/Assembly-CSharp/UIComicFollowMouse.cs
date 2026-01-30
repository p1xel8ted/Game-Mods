// Decompiled with JetBrains decompiler
// Type: UIComicFollowMouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using UnityEngine;

#nullable disable
public class UIComicFollowMouse : MonoBehaviour
{
  [SerializeField]
  public UIComicSegment choice1;
  [SerializeField]
  public Vector3 choice1Position;
  [SerializeField]
  public UIComicSegment choice2;
  [SerializeField]
  public Vector3 choice2Position;
  public UIComicSegment previousSelected;
  public UIComicPage page;

  public void Awake() => this.page = this.GetComponentInParent<UIComicPage>();

  public void Update()
  {
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null || !((Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable != (Object) null) || !((Object) this.page != (Object) null) || this.page.Animating)
      return;
    UIComicSegment component = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<UIComicSegment>();
    if (!((Object) component != (Object) null) || !((Object) this.previousSelected != (Object) component))
      return;
    this.transform.DOKill();
    ((RectTransform) this.transform).DOAnchorPos((Vector2) ((Object) component == (Object) this.choice1 ? this.choice1Position : this.choice2Position), 0.1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine);
    this.previousSelected = component;
  }
}
