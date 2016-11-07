using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

    Transform[] buttons;
    Transform buttonHighlight;
    int currentButton;
    int numButtons;
    float[] inputYAxis, inputYAxisLast;

    public GameManager gameManager;

	// Use this for initialization
	void Start () {
        numButtons = 3;
        buttons = new Transform[numButtons];
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name == "Resume")
                buttons[0] = child;
            else if (child.name == "Volume")
                buttons[1] = child;
            else if (child.name == "MainMenu")
                buttons[2] = child;
            else if (child.name == "ButtonHighlight")
                buttonHighlight = child;
        }
        currentButton = 0;
        buttonHighlight.position = buttons[0].position;
        inputYAxis = new float[4];
        inputYAxisLast = new float[4];
        for (int i = 0; i <  4; i++)
            inputYAxisLast[i] = Input.GetAxis("P"+(i+1)+"LeftAxisY");

    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; i++)
            checkForInputs(i);
    }

    void checkForInputs(int pID)
    {
        inputYAxis[pID] = Input.GetAxis("P"+(pID+1)+"LeftAxisY");
        if (pID >= 2)
            inputYAxis[pID] += Input.GetAxisRaw("P" + (pID + 1) + "KbAxisY");

        if (inputYAxis[pID] > 0.5 && inputYAxisLast[pID] <= 0.5)
        {
            currentButton++;
            if (currentButton >= numButtons)
                currentButton = 0;
        }
        if (inputYAxis[pID] < -0.5 && inputYAxisLast[pID] >= -0.5)
        {
            currentButton--;
            if (currentButton < 0)
                currentButton = numButtons - 1;
        }
        buttonHighlight.position = buttons[currentButton].position;
        inputYAxisLast[pID] = inputYAxis[pID];

        if (Input.GetButtonDown("P"+(pID+1)+"BtnA"))
        {
            if (buttons[currentButton].name == "Resume")
            {
                gameManager.PauseGame();
            }
            else if (buttons[currentButton].name == "MainMenu")
            {
                GetComponent<SceneChanger>().GoToNextScene();
                gameManager.PauseGame();
            }
        }
    }
}
