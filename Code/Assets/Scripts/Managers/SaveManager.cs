using System;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public abstract class SaveManager
{
    public static void Save(object _obj, string filename)
    {
        XmlSerializer sr = new XmlSerializer(_obj.GetType());

        TextWriter writer = new StreamWriter(filename, false);

        sr.Serialize(writer, _obj);

        writer.Close();

        Debug.Log("Data Saved");
    }

    public static object Load(object _obj, string filename)
    {
        if(FileExists(filename))
        {
            XmlSerializer sr = new XmlSerializer(_obj.GetType());
                
            FileStream read = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            object info = (object)sr.Deserialize(read);

            read.Close();

            return info;
        }

        Debug.Log("Not Loaded");

        return new object();
    }

    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}
