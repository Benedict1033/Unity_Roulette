using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public static Pointer Instance;
    public GameObject resultPanel;
    public Text resultTitle;
    public Text resultText;
    RaycastHit2D hitInfo;

    //public AudioSource resultSound;
    private void Awake()
    {
        Instance = this;
    }

    public void startRayCast()
    {
        hitInfo = Physics2D.Raycast(transform.position, transform.right, 800);

        if (hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            Invoke("showResult", 1.2f);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * 800, Color.green);
        }
    }

    public void showResult()
    {
        //resultSound.Play();
        resultPanel.SetActive(true);
        resultTitle.text = Roulette.Instance.titleText.text;
        resultText.text = hitInfo.collider.transform.GetChild(0).GetComponent<Text>().text;

    }
}

