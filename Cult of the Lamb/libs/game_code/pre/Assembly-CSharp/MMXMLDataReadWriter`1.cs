// Decompiled with JetBrains decompiler
// Type: MMXMLDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
public class MMXMLDataReadWriter<T> : MMDataReadWriterBase<T>
{
  private const string kSaveDirectory = "saves";

  public override void Write(T data, string filename, bool encrypt = true, bool backup = true)
  {
    try
    {
      Debug.Log((object) ("MMXMLDataReadWriter - Write File " + this.GetFilepath(filename)).Colour(Color.yellow));
      using (FileStream fileStream = new FileStream(this.GetFilepath(filename), FileMode.Create))
      {
        new XmlSerializer(typeof (T)).Serialize((Stream) fileStream, (object) data);
        fileStream.Close();
      }
      System.Action onWriteCompleted = this.OnWriteCompleted;
      if (onWriteCompleted == null)
        return;
      onWriteCompleted();
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      throw;
    }
  }

  public override void Read(string filename)
  {
    try
    {
      Debug.Log((object) ("MMDataReadWriter - Read File " + this.GetFilepath(filename)).Colour(Color.yellow));
      if (this.FileExists(filename))
      {
        T obj;
        using (FileStream fileStream = new FileStream(this.GetFilepath(filename), FileMode.Open))
        {
          using (StreamReader streamReader = new StreamReader((Stream) fileStream))
          {
            Debug.Log((object) "MMDataReadWriter - Read File - Successfully read file!".Colour(Color.magenta));
            obj = (T) new XmlSerializer(typeof (T)).Deserialize((TextReader) streamReader);
            fileStream.Close();
          }
        }
        Action<T> onReadCompleted = this.OnReadCompleted;
        if (onReadCompleted == null)
          return;
        onReadCompleted(obj);
      }
      else
      {
        Debug.Log((object) "MMDataReadWriter - Read File - File did not exist, creating new".Colour(Color.cyan));
        Directory.CreateDirectory(this.GetDirectory());
        System.Action onCreateDefault = this.OnCreateDefault;
        if (onCreateDefault == null)
          return;
        onCreateDefault();
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      throw;
    }
  }

  public override void Delete(string filename)
  {
    Debug.Log((object) ("MMXMLDataReadWriter - Delete " + this.GetFilepath(filename)).Colour(Color.yellow));
    if (!this.FileExists(filename))
      return;
    Debug.Log((object) ("MMXMLDataReadWriter - Deletion Successful " + this.GetFilepath(filename)).Colour(Color.yellow));
    File.Delete(this.GetFilepath(filename));
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete == null)
      return;
    deletionComplete();
  }

  public override bool FileExists(string filename)
  {
    Debug.Log((object) ("MMXMLDataReadWriter - File Exists? " + this.GetFilepath(filename)).Colour(Color.yellow));
    return File.Exists(this.GetFilepath(filename));
  }

  private string GetDirectory() => Path.Combine(Application.persistentDataPath, "saves");

  private string GetFilepath(string filename) => Path.Combine(this.GetDirectory(), filename);
}
