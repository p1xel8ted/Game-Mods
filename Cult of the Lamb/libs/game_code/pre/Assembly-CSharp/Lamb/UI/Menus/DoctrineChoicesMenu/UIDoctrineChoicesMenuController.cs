// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.DoctrineChoicesMenu.UIDoctrineChoicesMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Menus.DoctrineChoicesMenu;

public class UIDoctrineChoicesMenuController : 
  UIChoiceMenuBase<UIDoctrineChoiceInfoBox, DoctrineResponse>
{
  private int _cachedMMConversationSortingOrder;
  private Canvas _mmConversationCanvas;

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    if (!((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null) || !MMConversation.mmConversation.TryGetComponent<Canvas>(out this._mmConversationCanvas))
      return;
    this._cachedMMConversationSortingOrder = this._mmConversationCanvas.sortingOrder;
    if (!((UnityEngine.Object) this._canvas != (UnityEngine.Object) null))
      return;
    this._mmConversationCanvas.sortingOrder = this._canvas.sortingOrder + 1;
  }

  protected override void OnHideCompleted()
  {
    if ((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(true);
    if ((UnityEngine.Object) this._mmConversationCanvas != (UnityEngine.Object) null)
      this._mmConversationCanvas.sortingOrder = this._cachedMMConversationSortingOrder;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  protected override void Configure()
  {
    this._infoBox1.Configure(MMConversation.CURRENT_CONVERSATION.DoctrineResponses[0], new Vector2(-660f, 0.0f), new Vector2(-1160f, 0.0f));
    if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1)
    {
      this._infoBox2.Configure(MMConversation.CURRENT_CONVERSATION.DoctrineResponses[1], new Vector2(660f, 0.0f), new Vector2(1160f, 0.0f));
    }
    else
    {
      this._controlPrompts.HideAcceptButton();
      this._infoBox1.RectTransform.localPosition = (Vector3) new Vector2(0.0f, -540f);
      this._infoBox2.gameObject.SetActive(false);
      if (!((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null))
        return;
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(false);
    }
  }

  protected override void MadeChoice(UIChoiceInfoCard<DoctrineResponse> infoCard)
  {
    if (!((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null))
      return;
    if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1)
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(true);
    System.Action action = (System.Action) null;
    if ((UnityEngine.Object) infoCard == (UnityEngine.Object) this._infoBox1 && MMConversation.CURRENT_CONVERSATION.DoctrineResponses[0] != null)
      action = MMConversation.CURRENT_CONVERSATION.DoctrineResponses[0].Callback;
    if ((UnityEngine.Object) infoCard == (UnityEngine.Object) this._infoBox2 && MMConversation.CURRENT_CONVERSATION.DoctrineResponses[1] != null)
      action = MMConversation.CURRENT_CONVERSATION.DoctrineResponses[1].Callback;
    CultFaithManager.AddThought(Thought.Cult_NewDoctrine);
    MMConversation.mmConversation.Close();
    if (action == null)
      return;
    action();
  }
}
