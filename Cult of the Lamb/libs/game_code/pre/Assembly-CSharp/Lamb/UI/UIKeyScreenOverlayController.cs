// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIKeyScreenOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIKeyScreenOverlayController : UIMenuBase
{
  private const string kFlashMaterialID = "_FillColor";
  private const string kFillColorLerpFadeID = "_FillColorLerpFade";
  [Header("Key Screen")]
  [SerializeField]
  private KeyPiece[] _keyPieces;
  [SerializeField]
  private RectTransform _keyContainer;
  [SerializeField]
  private Material _flashMaterial;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [Header("Complete Key")]
  [SerializeField]
  private RectTransform _completeKeyContainer;
  [SerializeField]
  private Image _completeKey;
  [SerializeField]
  private GameObject _outline;
  [SerializeField]
  private RectTransform _glow;
  [Header("Total Keys")]
  [SerializeField]
  private RectTransform _totalKeysContainer;
  [SerializeField]
  private Image _totalKeysImage;
  [SerializeField]
  private TextMeshProUGUI _totalKeys;
  private KeyPiece _targetPiece;
  private Material _flashMaterialInstance;
  private Color _flashColor = new Color(1f, 1f, 1f, 0.0f);
  private static readonly int FillColorLerpFade = Shader.PropertyToID("_FillColorLerpFade");
  private static readonly int FillColor = Shader.PropertyToID("_FillColor");

  private Color _flashColorImpl
  {
    get => this._flashColor;
    set
    {
      this._flashColor = value;
      this._flashMaterialInstance.SetFloat(UIKeyScreenOverlayController.FillColorLerpFade, this._flashColor.a);
      this._flashMaterialInstance.SetColor(UIKeyScreenOverlayController.FillColor, this._flashColor);
    }
  }

  protected override void OnShowStarted()
  {
    this._controlPrompts.HideAcceptButton();
    int currentKeyPieces = DataManager.Instance.CurrentKeyPieces;
    if (currentKeyPieces != 0)
    {
      for (int index = 0; index < this._keyPieces.Length; ++index)
      {
        if (currentKeyPieces > index)
        {
          if (currentKeyPieces - 1 == index)
            this._targetPiece = this._keyPieces[index];
        }
        else
          this._keyPieces[index].gameObject.SetActive(false);
      }
      this._totalKeysContainer.gameObject.SetActive(Inventory.TempleKeys > 0);
      this._totalKeys.text = $"x{Inventory.TempleKeys}";
    }
    else
    {
      this._targetPiece = this._keyPieces.LastElement<KeyPiece>();
      if (Inventory.TempleKeys == 1)
        this._totalKeysContainer.gameObject.SetActive(false);
      else
        this._totalKeys.text = $"x{Inventory.TempleKeys - 1}";
    }
    this._flashMaterialInstance = new Material(this._flashMaterial);
    this._flashMaterialInstance.SetFloat(UIKeyScreenOverlayController.FillColorLerpFade, 0.0f);
    foreach (KeyPiece keyPiece in this._keyPieces)
      keyPiece.SetMaterial(this._flashMaterialInstance);
    this._completeKey.material = this._flashMaterialInstance;
    this._targetPiece.PrepareForAttach();
  }

  protected override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIKeyScreenOverlayController overlayController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      overlayController.StartCoroutine((IEnumerator) overlayController.RunKeyScreen());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    this.\u003C\u003E2__current = (object) overlayController.\u003C\u003En__0();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator RunKeyScreen()
  {
    UIKeyScreenOverlayController overlayController = this;
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) overlayController._targetPiece.Attach();
    overlayController._keyContainer.DOShakePosition(0.6f, (Vector3) (overlayController._targetPiece.MoveVector * 25f)).SetEase<Tweener>(Ease.OutSine).SetUpdate<Tweener>(true);
    overlayController.Flash(0.35f);
    if (Inventory.KeyPieces == 0)
    {
      yield return (object) new WaitForSecondsRealtime(1.5f);
      if (!overlayController._totalKeysContainer.gameObject.activeSelf)
        overlayController._totalKeysContainer.gameObject.SetActive(true);
      overlayController._flashMaterialInstance.color = Color.white;
      overlayController._completeKeyContainer.gameObject.SetActive(true);
      overlayController._glow.DOScale(Vector3.one * 1.25f, 1f).SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      overlayController._keyContainer.DOShakePosition(0.5f, Vector3.right * 35f).SetEase<Tweener>(Ease.OutSine).SetUpdate<Tweener>(true);
      overlayController._totalKeysImage.material = overlayController._flashMaterialInstance;
      UIManager.PlayAudio("event:/temple_key/become_whole");
      yield return (object) new WaitForSecondsRealtime(0.4f);
      overlayController.Flash(1.75f);
      overlayController._totalKeys.text = $"x{Inventory.TempleKeys}";
      yield return (object) new WaitForSecondsRealtime(1f);
    }
    else
      yield return (object) new WaitForSecondsRealtime(1f);
    overlayController._controlPrompts.ShowAcceptButton();
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    overlayController.Hide();
  }

  private void Flash(float time)
  {
    this._flashColorImpl = Color.white;
    this.StartCoroutine((IEnumerator) this.DoFlash(time));
  }

  private IEnumerator DoFlash(float time)
  {
    float t = 0.0f;
    while ((double) t < (double) time)
    {
      t += Time.unscaledDeltaTime;
      this._flashColorImpl = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0.0f), t / time);
      yield return (object) null;
    }
  }

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);

  protected override void OnDestroy()
  {
    base.OnDestroy();
    Object.Destroy((Object) this._flashMaterialInstance);
    this._flashMaterialInstance = (Material) null;
  }
}
