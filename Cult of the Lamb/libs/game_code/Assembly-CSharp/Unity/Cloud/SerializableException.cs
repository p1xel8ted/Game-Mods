// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.SerializableException
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud;

public class SerializableException
{
  [CompilerGenerated]
  public string \u003CDetailedProblemIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CFullText\u003Ek__BackingField;
  [CompilerGenerated]
  public SerializableException \u003CInnerException\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CMessage\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CProblemIdentifier\u003Ek__BackingField;
  [CompilerGenerated]
  public List<SerializableStackFrame> \u003CStackTrace\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CType\u003Ek__BackingField;

  public SerializableException()
  {
  }

  public SerializableException(Exception exception)
  {
    this.Message = exception.Message;
    this.FullText = exception.ToString();
    this.Type = exception.GetType().FullName;
    this.StackTrace = new List<SerializableStackFrame>();
    foreach (StackFrame frame in new System.Diagnostics.StackTrace(exception, true).GetFrames())
      this.StackTrace.Add(new SerializableStackFrame(frame));
    if (this.StackTrace.Count > 0)
    {
      SerializableStackFrame serializableStackFrame = this.StackTrace[0];
      this.ProblemIdentifier = $"{this.Type} at {serializableStackFrame.DeclaringType}.{serializableStackFrame.MethodName}";
    }
    else
      this.ProblemIdentifier = this.Type;
    if (this.StackTrace.Count > 1)
    {
      SerializableStackFrame serializableStackFrame1 = this.StackTrace[0];
      SerializableStackFrame serializableStackFrame2 = this.StackTrace[1];
      this.DetailedProblemIdentifier = $"{this.Type} at {serializableStackFrame1.DeclaringType}.{serializableStackFrame1.MethodName} from {serializableStackFrame2.DeclaringType}.{serializableStackFrame2.MethodName}";
    }
    if (exception.InnerException == null)
      return;
    this.InnerException = new SerializableException(exception.InnerException);
  }

  public string DetailedProblemIdentifier
  {
    get => this.\u003CDetailedProblemIdentifier\u003Ek__BackingField;
    set => this.\u003CDetailedProblemIdentifier\u003Ek__BackingField = value;
  }

  public string FullText
  {
    get => this.\u003CFullText\u003Ek__BackingField;
    set => this.\u003CFullText\u003Ek__BackingField = value;
  }

  public SerializableException InnerException
  {
    get => this.\u003CInnerException\u003Ek__BackingField;
    set => this.\u003CInnerException\u003Ek__BackingField = value;
  }

  public string Message
  {
    get => this.\u003CMessage\u003Ek__BackingField;
    set => this.\u003CMessage\u003Ek__BackingField = value;
  }

  public string ProblemIdentifier
  {
    get => this.\u003CProblemIdentifier\u003Ek__BackingField;
    set => this.\u003CProblemIdentifier\u003Ek__BackingField = value;
  }

  public List<SerializableStackFrame> StackTrace
  {
    get => this.\u003CStackTrace\u003Ek__BackingField;
    set => this.\u003CStackTrace\u003Ek__BackingField = value;
  }

  public string Type
  {
    get => this.\u003CType\u003Ek__BackingField;
    set => this.\u003CType\u003Ek__BackingField = value;
  }
}
