// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ShowIfSpecialEdition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using TMPro;
using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class ShowIfSpecialEdition : BaseMonoBehaviour
{
  public Localize _localize;
  public TMP_Text _text;

  public void Start()
  {
    this._localize = this.GetComponent<Localize>();
    this._text = this.GetComponent<TMP_Text>();
    this._text.text = "";
    this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());
  }

  public IEnumerator WaitForDLCCheck()
  {
    ShowIfSpecialEdition ifSpecialEdition = this;
    yield return (object) new WaitUntil((Func<bool>) (() => SessionManager.instance.HasStarted));
    yield return (object) GameManager.WaitForTime(0.1f, (System.Action) null);
    if (GameManager.AuthenticatePilgrimDLC())
    {
      ifSpecialEdition._localize.Term = "UI/DLC/PilgrimPack";
      ifSpecialEdition._text.text = LocalizationManager.GetTranslation("UI/DLC/PilgrimPack");
    }
    else if (GameManager.AuthenticateSinfulDLC())
    {
      ifSpecialEdition._localize.Term = "UI/DLC/SinfulEdition";
      ifSpecialEdition._text.text = LocalizationManager.GetTranslation("UI/DLC/SinfulEdition");
    }
    else if (GameManager.AuthenticateHereticDLC())
    {
      ifSpecialEdition._localize.Term = "UI/DLC/HereticEdition";
      ifSpecialEdition._text.text = LocalizationManager.GetTranslation("UI/DLC/HereticEdition");
    }
    else if (GameManager.AuthenticateCultistDLC())
    {
      ifSpecialEdition._localize.Term = "UI/DLC/CultistEdition";
      ifSpecialEdition._text.text = LocalizationManager.GetTranslation("UI/DLC/CultistEdition");
    }
    else
      ifSpecialEdition.gameObject.SetActive(false);
  }
}
