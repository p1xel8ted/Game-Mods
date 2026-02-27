// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ShowIfCultistEdition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class ShowIfCultistEdition : BaseMonoBehaviour
{
  [SerializeField]
  private bool _checkDataManager;

  public void Start() => this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());

  private IEnumerator WaitForDLCCheck()
  {
    ShowIfCultistEdition ifCultistEdition = this;
    yield return (object) null;
    if (GameManager.AuthenticateCultistDLC())
      ifCultistEdition.gameObject.SetActive(true);
    else
      ifCultistEdition.gameObject.SetActive(false);
  }
}
