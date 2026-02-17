// Decompiled with JetBrains decompiler
// Type: InputTextFocusOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.IO;
using TMPro;
using UnityEngine;

#nullable disable
public class InputTextFocusOnStart : BaseMonoBehaviour
{
  public TMP_InputField TMP_InputField;

  public void Start() => this.TMP_InputField.ActivateInputField();

  public void SaveEmail()
  {
    if (MMTransition.IsPlaying)
      return;
    string text = this.TMP_InputField.text;
    StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/emails.txt", true);
    streamWriter.Write(text + ", ");
    streamWriter.Close();
  }
}
