// Decompiled with JetBrains decompiler
// Type: src.Data.WriteTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace src.Data;

public class WriteTest : BaseMonoBehaviour
{
  private const string kMMJsonTestFilename = "mmJsonTestFile";
  private const string kUnifyTestFilename = "unifyTestFile";
  private MMDataReadWriterBase<SerializeTest> _mmJsonTest = (MMDataReadWriterBase<SerializeTest>) new MMJsonDataReadWriter<SerializeTest>();
  private MMDataReadWriterBase<SerializeTest> _unifyTest = (MMDataReadWriterBase<SerializeTest>) new UnifyDataReadWriter<SerializeTest>();
  [SerializeField]
  private SerializeTest _mmTestData = new SerializeTest();
  [SerializeField]
  private SerializeTest _unifyTestData = new SerializeTest();

  private void Start()
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
}
