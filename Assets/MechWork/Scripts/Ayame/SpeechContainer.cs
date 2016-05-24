using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("SpeechCollection")]
public class SpeechContainer {
    [XmlArray("Speeches")]
    [XmlArrayItem("Speech")]
    public List<Speech> speeches = new List<Speech>();

    public static SpeechContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(SpeechContainer));
        StringReader reader = new StringReader(_xml.text);
        SpeechContainer speeches = serializer.Deserialize(reader) as SpeechContainer;
        reader.Close();
        return speeches;
    }
}
