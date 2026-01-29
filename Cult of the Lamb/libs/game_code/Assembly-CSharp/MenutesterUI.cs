// Decompiled with JetBrains decompiler
// Type: MenutesterUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class MenutesterUI : MonoBehaviour
{
  [SerializeField]
  public MenuTester _menuTester;
  [SerializeField]
  public RectTransform _buttonContainer;
  [SerializeField]
  public Button _buttonPrefab;

  public void Start() => this.GenerateMenuButtons();

  public void GenerateMenuButtons()
  {
    foreach (MethodInfo methodInfo in ((IEnumerable<MethodInfo>) typeof (MenuTester).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name.StartsWith("Test") && m.GetParameters().Length == 0 && m.ReturnType == typeof (void))).OrderBy<MethodInfo, string>((Func<MethodInfo, string>) (m => m.Name)).ToArray<MethodInfo>())
    {
      MethodInfo method = methodInfo;
      Button button = UnityEngine.Object.Instantiate<Button>(this._buttonPrefab, (Transform) this._buttonContainer);
      button.name = method.Name;
      button.GetComponentInChildren<Text>().text = method.Name.Replace("Test", "").Replace("Menu", "");
      button.onClick.AddListener((UnityAction) (() =>
      {
        method.Invoke((object) this._menuTester, (object[]) null);
        Debug.Log((object) ("Invoked: " + method.Name));
      }));
    }
  }
}
