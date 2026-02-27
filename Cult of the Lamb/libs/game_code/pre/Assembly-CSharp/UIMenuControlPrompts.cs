// Decompiled with JetBrains decompiler
// Type: UIMenuControlPrompts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIMenuControlPrompts : BaseMonoBehaviour
{
  [Header("Menu")]
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private UIMenuBase _attachedMenu;
  [Header("Prompt Containers")]
  [SerializeField]
  private GameObject _acceptPromptContainer;
  [SerializeField]
  private GameObject _cancelPromptContainer;

  private void Start() => this.ForceRebuild();

  private void OnCancelButtonClicked() => this._attachedMenu.OnCancelButtonInput();

  public void ShowAcceptButton()
  {
    if (!((Object) this._acceptPromptContainer != (Object) null))
      return;
    this._acceptPromptContainer.gameObject.SetActive(true);
  }

  public void HideAcceptButton()
  {
    if (!((Object) this._acceptPromptContainer != (Object) null))
      return;
    this._acceptPromptContainer.gameObject.SetActive(false);
  }

  public void ShowCancelButton()
  {
    if (!((Object) this._cancelPromptContainer != (Object) null))
      return;
    this._cancelPromptContainer.gameObject.SetActive(true);
  }

  public void HideCancelButton()
  {
    if (!((Object) this._cancelPromptContainer != (Object) null))
      return;
    this._cancelPromptContainer.gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.ForceRebuild);
  }

  private void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.ForceRebuild);
  }

  private void ForceRebuild() => LayoutRebuilder.ForceRebuildLayoutImmediate(this._rectTransform);
}
