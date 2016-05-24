using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeechLoader : MonoBehaviour
{

    const string path = "speech";
    public SpeechContainer sc;

    public List<AEvent> A_Events; // List containing all Ayame events

    void Awake()
    {
        sc = SpeechContainer.Load(path);

        A_Events = new List<AEvent>();

        // Cause the XML is written in event order
        // Speeches are clustered together by their event
        int speechIndex = 0;
        while (speechIndex < sc.speeches.Count)
        {
            if (speechIndex == 0 || !sc.speeches[speechIndex].Event.Equals(A_Events[A_Events.Count - 1].name))
            {
                // First speech or Event doesn't already exist
                AEvent newEvent = new AEvent();
                newEvent.name = sc.speeches[speechIndex].Event;
                newEvent.lines.Add(sc.speeches[speechIndex].Line);
                newEvent.images.Add(sc.speeches[speechIndex].Image);
                A_Events.Add(newEvent);
            }
            else
            {
                // Event already exists
                A_Events[A_Events.Count - 1].lines.Add(sc.speeches[speechIndex].Line);
                A_Events[A_Events.Count - 1].images.Add(sc.speeches[speechIndex].Image);
            }
            speechIndex++;
        }

        Debug.Log(A_Events.Count);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
