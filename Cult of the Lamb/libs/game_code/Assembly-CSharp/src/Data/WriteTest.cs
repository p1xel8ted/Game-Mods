// Decompiled with JetBrains decompiler
// Type: src.Data.WriteTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace src.Data;

public class WriteTest : BaseMonoBehaviour
{
  public const string kMMJsonTestFilename = "mmJsonTestFile";
  public const string kUnifyTestFilename = "unifyTestFile";
  public MMDataReadWriterBase<SerializeTest> _mmJsonTest = (MMDataReadWriterBase<SerializeTest>) new MMJsonDataReadWriter<SerializeTest>();
  public MMDataReadWriterBase<SerializeTest> _unifyTest = (MMDataReadWriterBase<SerializeTest>) new UnifyDataReadWriter<SerializeTest>();
  [SerializeField]
  public SerializeTest _mmTestData = new SerializeTest();
  [SerializeField]
  public SerializeTest _unifyTestData = new SerializeTest();

  public void Start()
  {
    this._mmJsonTest.OnCreateDefault += (System.Action) (() => this._mmTestData = new SerializeTest());
    this._mmJsonTest.OnReadCompleted += (Action<SerializeTest>) (data => this._mmTestData = data);
    this._unifyTest.OnCreateDefault += (System.Action) (() => this._unifyTestData = new SerializeTest());
    this._unifyTest.OnReadCompleted += (Action<SerializeTest>) (data => this._unifyTestData = data);
  }

  public void Write()
  {
    this._mmJsonTest.Write(this._mmTestData, "mmJsonTestFile");
    this._unifyTest.Write(this._unifyTestData, "unifyTestFile");
  }

  public void Read()
  {
  }

  public void Delete()
  {
    this._mmJsonTest.Delete("mmJsonTestFile");
    this._unifyTest.Delete("unifyTestFile");
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_0() => this._mmTestData = new SerializeTest();

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_1(SerializeTest data) => this._mmTestData = data;

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_2() => this._unifyTestData = new SerializeTest();

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_3(SerializeTest data) => this._unifyTestData = data;
}
