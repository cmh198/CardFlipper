using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public Text timeTxt;
    float time = 30.0f;
    int trycount = 0;
    public GameObject EndCanvas;
    public Text Trytime;
    public GameObject failTxt;
    public GameObject card;
    public GameObject endTxt;
    public GameObject firstCard;
    public GameObject secondCard;
    public AudioClip match;
    public AudioSource audioSource;

    public static gameManager I;
    void Start()
    {
        Time.timeScale = 1.0f;

        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for(int i = 0; i<16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    void Update()
    {
        Trytime.text = "시도횟수: " + trycount.ToString();
        Debug.Log(trycount);
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time < 5)
        {
            timeTxt.color = Color.red;
        }
        
        if (time < 0)
        {
            EndCanvas.SetActive(true);
            EndCanvas.transform.Find("TryTimeTxt").gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    private void Awake()
    {
        I = this;
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        trycount += 1;
        Trytime.text = trycount.ToString();
        Debug.Log(trycount);
        
        if (firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            if (cardsLeft == 2)
            {
                EndCanvas.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            Invoke("fail", 0.0f);
            time -= 2.0f;
        }
        firstCard = null;
        secondCard = null;
        Invoke("failafter", 0.5f);
    }
    void fail()
    {
        failTxt.SetActive(true);
    }
    void failafter()
    {
        failTxt.SetActive(false);
    }
}
