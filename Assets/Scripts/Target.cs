using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float speed = 50;
    public Rigidbody2D rb;
    public LayerMask layerMask;

    public GameObject[] fishList;
    public TMP_Text text, deduction, minusfv, plusfv;
    public Slider slider;
    public Sprite FrenzyBackground;
    public Sprite NormalBackground;

    public float timeLeft = 30;
    public float startValue = 0f;
    public float endValue = 10f;
    public float lerpSpeed = 1f;

    private float currentValue;

    public Timer timer;
    public Timer FrenzyTimer;
    public Score score;
    public Sprite[] rods;
    public SpriteRenderer rod;

    public bool canCast = true;
    public bool FrenzyTime = false;

    public SpriteRenderer Background;

    int FrenzyTrigger=0;

    void Start()
    {
        HideAllFish();
        deduction.gameObject.SetActive(false);
        minusfv.gameObject.SetActive(false);
        plusfv.gameObject.SetActive(false);
        rod.sprite = rods[0];
        currentValue = startValue;
    }

    void Update()
    {
        if (FrenzyTime)
        {
            FrenzyTimer.pause = false;
            timer.pause = true;
            Background.sprite = FrenzyBackground;
        }
        else {
            FrenzyTimer.pause = true;
            timer.pause = false;
            Background.sprite = NormalBackground;
        }

        if (timer.done == true)
        {
            SceneManager.LoadScene(0);
        }
        if (FrenzyTimer.done ==true)
        {
            FrenzyTime = false;
            FrenzyTimer.done = false;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, layerMask);
        if (text.text != "Wait...")
        {
            currentValue = Mathf.Lerp(startValue, endValue, Mathf.PingPong(Time.time * lerpSpeed, 1f));
            slider.value = currentValue;
        }
    }

    void Launch()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(speed * x, speed * y);
        //FindObjectOfType<AudioManager>().Play("Heal");
    

    }

    public void CatchFish()
    {
        if (text.text == "Casting")
        {
            if (canCast)
            {
                timer.pause = true;
                text.text = "Wait...";
                canCast = false;
                rb.velocity = Vector2.zero;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, layerMask);
                int fishIndex = 0;

                if (hit.collider != null)
                {
                    FindObjectOfType<AudioManager>().Play("Cast Fish");
                    string area = hit.collider.name;
                    switch (area)
                    {
                        case "Pond":
                            fishIndex = Random.Range(0, 4);
                            timer.GameTimer -= 10f;
                            deduction.gameObject.SetActive(true);
                            break;
                        case "Light Blue":
                            fishIndex = Random.Range(3, 7);
                            timer.GameTimer -= 5f;
                            minusfv.gameObject.SetActive(true);
                            break;
                        case "Violet":
                            fishIndex = Random.Range(6, 10);
                            break;
                        case "Dark Blue":
                            fishIndex = Random.Range(9, 13);
                            break;
                        case "Deep Blue":
                            timer.GameTimer += 1f;
                            plusfv.gameObject.SetActive(true);
                            timeLeft += fishIndex * 5;
                            break;
                    }

                    int totalIndex = CheckIndex(fishIndex + currentValue);
                    score.AddScore(totalIndex);
                    HideAllFish();
                    fishList[totalIndex].SetActive(true);
                }
                Invoke("Reset", 1f);  // Always reset after catching
            }
        }
        else if (text.text == "Catch again!" || text.text == "Frenzy Time!!!" || text.text == "Start!!!")
        {
            FindObjectOfType<AudioManager>().Play("Cast Fish");
            Launch();
            text.text = "Casting";
            HideAllFish();
            deduction.gameObject.SetActive(false);
            minusfv.gameObject.SetActive(false);
            plusfv.gameObject.SetActive(false);

            

            // Optionally you can add Invoke here too if needed
            // Invoke("Reset", 1f);
        }
    }

    void HideAllFish()
    {
        foreach (var fish in fishList)
        {
            fish.SetActive(false);
        }
    }
    void Reset()
    {
        deduction.gameObject.SetActive(false);
        minusfv.gameObject.SetActive(false);
        plusfv.gameObject.SetActive(false);
        if (FrenzyTime!=true) {
            FrenzyTrigger = Random.Range(0, 15);
        }
        if (FrenzyTrigger > 5)
        {
            FrenzyTime = true;
            text.text = "Frenzy Time!!!";

            // Start casting immediately during frenzy
            Launch();
            text.text = "Casting";
            canCast = true;
        }
        else
        {
            FrenzyTime = false;
            text.text = "Catch again!";
            canCast = true;
        }
    }


    int CheckIndex(float totalScore)
    {
        return Mathf.Min(13, (int)totalScore);
    }
}
