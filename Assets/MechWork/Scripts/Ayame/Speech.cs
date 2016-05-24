using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Speech {
    [XmlElement("event")]
    public string Event;
    [XmlElement("image")]
    public string Image;
    [XmlElement("line")]
    public string Line;
}
