// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SandboxFleeceItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SandboxFleeceItem : MonoBehaviour
{
  [SerializeField]
  public RectTransform _shakeContainer;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public FleeceIconMapping _fleeceIconMapping;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public GameObject _lockedContainer;
  public int _fleeceIndex;
  public bool _unlocked;
  public Vector2 _origin;

  public MMButton Button => this._button;

  public bool Unlocked => this._unlocked;

  public int FleeceIndex => this._fleeceIndex;

  public void Configure(int index)
  {
    this._fleeceIndex = index;
    this._origin = this._shakeContainer.anchoredPosition;
    this._unlocked = DataManager.Instance.UnlockedFleeces.Contains(this._fleeceIndex);
    if (this._fleeceIndex != 0 && !DungeonSandboxManager.HasFinishedAnyWithDefaultFleece())
      this._unlocked = false;
    this._button.Confirmable = this._unlocked;
    this._lockedContainer.SetActive(!this._unlocked);
    this._fleeceIconMapping.GetImage(this._fleeceIndex, this._icon);
    this._button.OnConfirmDenied += new System.Action(this.Shake);
  }

  public void Shake()
  {
    this._shakeContainer.DOKill();
    this._shakeContainer.localScale = (Vector3) Vector2.one;
    this._shakeContainer.anchoredPosition = this._origin;
    this._shakeContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }
}
