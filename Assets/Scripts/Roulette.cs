using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    #region Variable

    // static and private variable will not show in inspector
    [Header("static variable")]
    public static Roulette Instance;

    [Header("private variable")]
    Image img;
    string[] inputText;
    float inputNum;
    int speed;
    int inputState;
    int emptyState;
    public bool CN;

    // public variable will show in inspector
    [Header("public variable")]
    public GameObject contentContainer;
    public GameObject warningPanel;
    public GameObject yesBtn;
    public GameObject okBtn;
    public GameObject obstaclePanel;
    public GameObject userInputContainer;
    public GameObject rouletteContainer;
    public GameObject rouletteIcon;
    public GameObject inputIcon;
    public GameObject nextBtn;
    public int randomA = 1000;
    public int randomB = 2000;

    public GameObject startButton;

    public Text titleWrap;
    public Text numOfSpin;

    //cn
    public Text numOfOption;
    public Text nextText;
    public Text confirmText;
    public Text startText;
    public Text spinText;
    public Text againText;
    public Text yesText;
    public Text noText;
    //public Text maxText;

    [Header("UI")]
    public InputField userInput;
    public InputField userInputField;
    public InputField titleText;
    public Image pieces;
    public Text warningText;
    public Text cnOrEng;
    #endregion

    #region Main Code

    private void Awake()
    {
        Instance = this;

    }

    void Update()
    {
        // If speed bigger than 0 ready to stop 
        if (speed > 0)
        {
            obstaclePanel.SetActive(true);
            stopSpin();
        }

        // start spin
        transform.Rotate(0, 0, speed * Time.deltaTime);

        titleWrap.horizontalOverflow = HorizontalWrapMode.Wrap;

        if (CN)
        {
            if (titleWrap.text.Length > 8)
            {
                titleText.GetComponent<ScrollRect>().enabled = true;
            }
            else if (titleWrap.text.Length < 8)
            {
                titleText.GetComponent<ScrollRect>().enabled = false;
            }
        }
        else
        {
            if (titleWrap.text.Length > 15)
            {
                titleText.GetComponent<ScrollRect>().enabled = true;
            }
            else if (titleWrap.text.Length < 15)
            {
                titleText.GetComponent<ScrollRect>().enabled = false;
            }
        }

        // playerPrefs 
        numOfSpin.text = PlayerPrefs.GetInt("numOfSpin").ToString();
    }

    #endregion

    #region Helper Code

    public void setupInput()
    {

        nextBtn.SetActive(true);

        // Destroy the previous InputField before instantiate the new InputField 
        for (int i = 0; i < contentContainer.transform.childCount; i++)
        {
            Destroy(contentContainer.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // Check Input Content
        try
        {
            // Convert string to float then reset input content
            inputNum = float.Parse(userInput.text);
            userInput.text = "";

            // Check if input out of range 2-10
            if (inputNum < 2 || inputNum > 10)
            {
                startButton.SetActive(false);

                warningPanel.SetActive(true);
                if (CN)
                    warningText.text = " 你輸入的内容不是 2-10 之間的整數 ";
                else
                    warningText.text = " Your input is out of range 2-10 ";
            }
            // No problem , ready to instantiate InputField
            else if (inputNum >= 2 && inputNum <= 10)
            {
                // Instantiate The InputField
                for (int i = 0; i < inputNum; i++)
                {
                    InputField inputField = Instantiate(userInputField, contentContainer.transform.position, contentContainer.transform.rotation);
                    inputField.transform.SetParent(contentContainer.transform);
                    inputField.transform.localScale = new Vector3(1, 1, 1);
                    inputField.transform.name = "InputField " + i;

                    int sum = i + 1;

                    if (CN)
                    {
                        inputField.placeholder.GetComponent<Text>().text = "請輸入你的選項 " + sum;
                    }
                    else 
                    { 
                        inputField.placeholder.GetComponent<Text>().text = "Enter your option " + sum;

                    }
                }

                startButton.SetActive(true);
            }

        }
        catch
        {
            startButton.SetActive(false);

            // Check if input is empty
            if (userInput.text == "")
            {
                warningPanel.SetActive(true);
                if (CN)
                    warningText.text = " 你的輸入不能為空白 ";
                else
                    warningText.text = " Input can't be Empty ";
            }
            // Check if input not integer
            else
            {
                warningPanel.SetActive(true);
                if (CN)
                    warningText.text = " 只有 2-10 之間的整數是可以被接受的 ";
                else
                    warningText.text = " Only integer 2-10 acceptable ";
            }

            userInput.text = "";
        }

    }

    public void Spin()
    {


        if (transform.childCount > 0 && titleText.text != "")
        {
            speed = Random.Range(randomA, randomB);

            //
            int numOfSpin = PlayerPrefs.GetInt("numOfSpin");
            numOfSpin++;
            PlayerPrefs.SetInt("numOfSpin", numOfSpin);
            // 

            startButton.SetActive(false);
        }
        else
        {
            warningPanel.SetActive(true);
            if (CN)
                warningText.text = " 你必須先輸入標題 ";
            else
                warningText.text = " Your must fill the title first ";
        }
    }

    public void roulettePieces()
    {
        // Image Fill Amount
        pieces.fillAmount = 1;
        pieces.fillAmount = pieces.fillAmount / inputNum;

        for (int j = 0; j < inputNum; j++)
        {
            // Instantiate the rolette pieces
            img = Instantiate(pieces, transform.position, transform.rotation);
            img.transform.SetParent(transform);
            img.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            img.transform.eulerAngles = new Vector3(img.transform.eulerAngles.x, img.transform.eulerAngles.y, img.transform.eulerAngles.z + (360 / inputNum) * j);

            // Instantiate the User Input Text
            transform.GetChild(j).GetChild(0).name = "User Input " + j;
            Text userInput = GameObject.Find("User Input " + j).GetComponent<Text>();
            userInput.text = inputText[j].ToString();
            userInput.transform.eulerAngles = new Vector3(0, 0, userInput.transform.eulerAngles.z + -((360 / inputNum) / 2));

            setCollider();
        }
    }

    public void checkText()
    {
        try
        {
            // Initialize the array space
            inputText = new string[(int)inputNum];

            // Store all user input text into array
            for (int i = 0; i < inputNum; i++)
            {
                InputField str = GameObject.Find("InputField " + i).GetComponent<InputField>();

                inputText[i] = str.text.ToString().Trim();
            }

            checkTextRepeated();

            checkUserInput();
        }
        catch
        {

        }
    }

    void checkTextRepeated()
    {
        inputState = 0;
        emptyState = 0;

        for (int i = 0; i < inputText.Length; i++)
        {
            if (inputText[i] == "")
            {
                emptyState++;
                if (CN)
                    inputText[i] = "無";
                else
                    inputText[i] = "none";
            }
            else if (inputText[i] != "")
            {
                inputState++;

                for (int j = 0; j < inputNum; j++)
                {
                    for (int k = j + 1; k < inputNum; k++)
                    {
                        if (inputText[k] != "" && inputText[k] != "none" && inputText[k] != "無")
                        {
                            if (inputText[j] == inputText[k])
                            {
                                inputState = 20;
                            }
                        }
                    }
                }
            }
        }
    }

    void checkUserInput()
    {
        // all empty
        if (emptyState == inputText.Length)
        {
            if (CN)
                warningText.text = " 你不能所有内容都為空白，至少要輸入2個或以上的内容 ";
            else
                warningText.text = " Your options can't be all empty , at least 2 option be fill ";
        }
        // input less than 1
        else if (inputState == 1)
        {
            if (CN)
                warningText.text = " 至少輸入2個或更多的選項 ";
            else
                warningText.text = " At least 2 option be fill ";
        }
        // have repeated
        else if (inputState > 10)
        {
            if (CN)
                warningText.text = "你不能輸入重複的内容 ";
            else
                warningText.text = " Your option can't be repeated ";
        }
        // some empty
        else if (inputState >= 2 && emptyState >= 1)
        {
            yesBtn.SetActive(true);
            okBtn.SetActive(false);
            if (CN)
                warningText.text = " 你有部分内容是空白的，確定要繼續嗎 ？ ";
            else
                warningText.text = " Your some option is unfill , are you sure you want to continue ? ";
        }
    }

    void stopSpin()
    {
        speed--;

        if (speed >= 0 && speed <= 20)
        {
            Pointer.Instance.startRayCast();
        }

        if (speed <= 0)
        {
            speed = 0;
        }
    }

    public void again()
    {
        //titleText.GetComponent<ScrollRect>().enabled = false;

        Pointer.Instance.resultPanel.SetActive(false);
        titleText.text = "";
        nextBtn.SetActive(true);
        obstaclePanel.SetActive(false);



        try
        {
            // Destroy all existing InputField and Roulette pieces
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
                //Destroy(contentContainer.transform.GetChild(i).gameObject);
            }

            //obstaclePanel.SetActive(false);
        }
        catch
        {

        }

        try
        {
            // Destroy all existing InputField and Roulette pieces
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(contentContainer.transform.GetChild(i).gameObject);
            }

            //obstaclePanel.SetActive(false);
        }
        catch
        {

        }
    }

    public void againtemp()
    {
        nextBtn.SetActive(true);

        try
        {
            // Destroy all existing InputField and Roulette pieces
            for (int i = 0; i < transform.childCount; i++)
            {
                //Destroy(gameObject.transform.GetChild(i).gameObject);
                Destroy(contentContainer.transform.GetChild(i).gameObject);
            }

            obstaclePanel.SetActive(false);
        }
        catch
        {

        }
    }

    public void closeWarningPanel()
    {
        warningPanel.SetActive(false);
        yesBtn.SetActive(false);
        okBtn.SetActive(true);
    }

    public void confirm()
    {
        yesBtn.SetActive(false);
        okBtn.SetActive(true);
        warningPanel.SetActive(false);
        inputIcon.SetActive(true);
        rouletteIcon.SetActive(false);
        nextBtn.SetActive(false);
        startButton.SetActive(false);

        RectTransform a = userInputContainer.transform.GetComponent<RectTransform>();
        a.localPosition = new Vector3(1080, -100, 0);
        RectTransform b = rouletteContainer.transform.GetComponent<RectTransform>();
        b.localPosition = new Vector3(0, -100, 0);

        roulettePieces();
    }

    public void setCollider()
    {
        if (inputNum == 2)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[0] = new Vector2(0f, 50f);
            colliderpoints[1] = new Vector2(-50f, 0f);
            colliderpoints[2] = new Vector2(0f, -50f);
            colliderpoints[3] = new Vector2(0f, 50f);
            bsa.points = colliderpoints;
        }
        else
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[1] = new Vector2(0f, -50f);
            colliderpoints[0] = new Vector2(0f, 0f);
            colliderpoints[3] = new Vector2(0f, 0f);
            bsa.points = colliderpoints;
        }

        if (inputNum == 3)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-43.5f, 25f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 4)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-50f, 0f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 5)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-47.2f, -15.3f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 6)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-43f, -24.8f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 7)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-39f, -31.1f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 8)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-35f, -35f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 9)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-32f, -38.1f);
            bsa.points = colliderpoints;
        }
        else if (inputNum == 10)
        {
            EdgeCollider2D bsa = img.gameObject.GetComponent<EdgeCollider2D>();
            Vector2[] colliderpoints;
            colliderpoints = bsa.points;
            colliderpoints[2] = new Vector2(-29f, -40f);
            bsa.points = colliderpoints;
        }
    }

    public void startBtn()
    {
        userInput.text = "";
        checkText();

        if (contentContainer.transform.childCount != 0)
        {
            if (inputState <= 10 && emptyState == 0)
            {
                roulettePieces();

                RectTransform a = userInputContainer.transform.GetComponent<RectTransform>();
                a.localPosition = new Vector3(1080, -100, 0);
                RectTransform b = rouletteContainer.transform.GetComponent<RectTransform>();
                b.localPosition = new Vector3(0, -100, 0);

                inputIcon.SetActive(true);
                rouletteIcon.SetActive(false);
                nextBtn.SetActive(false);

            }
            else if (inputState == 1 || inputState > 10 || emptyState > 0)
            {
                warningPanel.SetActive(true);
            }
        }
    }

    public void switchPage()
    {
        titleWrap.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -130, titleWrap.rectTransform.rect.width);

        againtemp();

        rouletteIcon.SetActive(true);
        inputIcon.SetActive(false);
        RectTransform a = userInputContainer.transform.GetComponent<RectTransform>();
        a.localPosition = new Vector3(0, -100, 0);
        RectTransform b = rouletteContainer.transform.GetComponent<RectTransform>();
        b.localPosition = new Vector3(1080, -100, 0);
    }

    public void nextPage()
    {
        titleWrap.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -130, titleWrap.rectTransform.rect.width);

        string title = titleText.text.Trim();

        if (title != "" && titleWrap.text.Length <= 20)
        {
            rouletteIcon.SetActive(true);
            inputIcon.SetActive(false);
            RectTransform a = userInputContainer.transform.GetComponent<RectTransform>();
            a.localPosition = new Vector3(0, -100, 0);
            RectTransform b = rouletteContainer.transform.GetComponent<RectTransform>();
            b.localPosition = new Vector3(1080, -100, 0);
        }
        else if (titleWrap.text.Length >= 20)
        {
            titleText.text = "";

            warningPanel.SetActive(true);
            if (CN)
                warningText.text = " 標題無法超過 20 字 ";
            else
                warningText.text = " Your title can't more than 20 words ";
        }
        else
        {
            titleText.text = "";


            warningPanel.SetActive(true);
            if (CN)
                warningText.text = " 你必須先輸入標題 ";
            else
                warningText.text = " You must fill the title first ";
        }
    }

    public void backRoulette()
    {
        inputIcon.SetActive(true);
        rouletteIcon.SetActive(false);
        RectTransform a = userInputContainer.transform.GetComponent<RectTransform>();
        a.localPosition = new Vector3(1080, -100, 0);
        RectTransform b = rouletteContainer.transform.GetComponent<RectTransform>();
        b.localPosition = new Vector3(0, -100, 0);
    }

    public void chinese()
    {
        titleWrap.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -130, titleWrap.rectTransform.rect.width);

        CN = !CN;
        if (CN)
        {
            cnOrEng.text = "CN";
            titleText.placeholder.GetComponent<Text>().text = "請輸入你的標題";
            numOfOption.text = "請輸入選項的數量";
            nextText.text = "下一步";
            confirmText.text = "確定";
            startText.text = "開始";
            spinText.text = "旋轉";
            againText.text = "再一次";
            yesText.text = "是";
            noText.text = "否";

            userInput.placeholder.GetComponent<Text>().text = "上限 :10";

            //maxText.text = "上限 :10";


            try
            {
                for (int i = 0; i < contentContainer.transform.childCount; i++)
                {
                    InputField str = GameObject.Find("InputField " + i).GetComponent<InputField>();
                    Text strstr = str.placeholder.GetComponent<Text>();
                    int sum = i + 1;
                    strstr.text = "請輸入你的選項 " + sum;
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    Text noneText = transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                    if (noneText.text == "none")
                    {
                        noneText.text = "無";
                    }

                }
            }
            catch { }
        }
        else
        {
            cnOrEng.text = "ENG";
            titleText.placeholder.GetComponent<Text>().text = "Enter your title";
            numOfOption.text = "Number of options";
            nextText.text = "Next";
            confirmText.text = "Confirm";
            startText.text = "Start";
            spinText.text = "Spin";
            againText.text = "Again";
            yesText.text = "yes";
            noText.text = "no";
            //maxText.text = "Max :10";

            userInput.placeholder.GetComponent<Text>().text = "Max :10";


            try
            {
                for (int i = 0; i < contentContainer.transform.childCount; i++)
                {
                    InputField str = GameObject.Find("InputField " + i).GetComponent<InputField>();
                    Text strstr = str.placeholder.GetComponent<Text>();
                    int sum = i + 1;
                    strstr.text = "Enter your option " + sum;
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    Text noneText = transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                    if (noneText.text == "無")
                    {
                        noneText.text = "none";
                    }

                }
            }
            catch { }
        }


    }




    #endregion
}
