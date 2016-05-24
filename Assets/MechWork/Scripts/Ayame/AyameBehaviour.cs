using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AyameBehaviour : MonoBehaviour {

    public Sprite[] ayameImages;
    public int eventIndex;

    public Text ayameBox;
    public Image ayameImage;
    public string textToShow;
    public float timeBetweenCharacters;
    public float timeBetweenLines;
    private float sentinel = 0;
    private bool textComplete = false;
    private int noOfCharacters = 0, untilIndex = 0;
    private int currentLine = -1; // -1 if not speaking
    private bool imageUpdated = false;

    private bool eventIsPlaying = false;
    private SpeechLoader sc;
    private AEvent currentEvent;
    private bool isWindowOpened = false;
    
    private Animator canvasAnimator;

    void Awake()
    {
        sc = GetComponent<SpeechLoader>();
        canvasAnimator = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventIndex++;
            eventIsPlaying = true;
            currentEvent = sc.A_Events[eventIndex];
            currentLine = 0;

            for (int i = 0; i < ayameImages.Length; i++)
            {
                if (ayameImages[i].name.ToLower().Equals(currentEvent.images[currentLine]))
                {
                    ayameImage.sprite = ayameImages[i];
                    imageUpdated = true;
                    break;
                }
            }
        }

        if (eventIsPlaying)
        {
            // Open window, if it isn't
            if (!canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opened"))
            {
                canvasAnimator.SetBool("IsOpened", true);
            }
            else
            {
                // Make Ayame say the appropriate lines
                #region Handling Ayame's line
                ayameBox.text = ""; // Reset every frame
                textComplete = (untilIndex == (currentEvent.lines[currentLine].Length));
                if (!textComplete)
                {
                    sentinel += Time.deltaTime;
                    if (sentinel >= timeBetweenCharacters)
                    {
                        untilIndex++;
                        sentinel = 0;
                    }
                }
                else
                {
                    // Once a line is complete
                    sentinel += Time.deltaTime;
                    if (sentinel >= timeBetweenLines)
                    {
                        if (currentLine < currentEvent.lines.Count - 1)
                        {
                            currentLine++;
                            imageUpdated = false;
                        }
                        else
                        {
                            Debug.Log("Ayame event end");
                            currentLine = -1;
                            eventIsPlaying = false;
                            textComplete = false;
                        }
                        //textComplete = false;
                        sentinel = 0;
                        untilIndex = 0;

                    }
                }

                for (int i = 0; i < untilIndex; i++)
                {
                    ayameBox.text += currentEvent.lines[currentLine][i].ToString();
                }
                #endregion

                if (!imageUpdated)
                {
                    for (int i = 0; i < ayameImages.Length; i++)
                    {
                        if (ayameImages[i].name.ToLower().Equals(currentEvent.images[currentLine]))
                        {
                            ayameImage.sprite = ayameImages[i];
                            imageUpdated = true;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            // Close window, if it isn't
            // Open window, if it isn't
            if (!canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed"))
            {
                canvasAnimator.SetBool("IsOpened", false);
                ayameBox.text = "";
            }
        }
	}
}
