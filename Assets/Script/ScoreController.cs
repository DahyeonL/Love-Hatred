using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    GameObject firework;

    [SerializeField]
    GameObject LoseEffect;

    [SerializeField]
    GameObject resulttext;

    [SerializeField]
    GameObject guidetext;

    [SerializeField]
    GameObject BasicCanvas;
    public static int MyBuildings = 1;
    float x = 1;
    float plus = -1;
    public static bool end_of_game = false;
    bool win_or_lose;
    float waitcount = 0;
    float waitcount2 = 0;

    public void Restart()
    {
        resulttext.SetActive(false);
        guidetext.SetActive(false);
        MyBuildings = 1;
        x = 1;
        plus = -1;
        end_of_game = false;
        waitcount = 0;
        waitcount2 = 0;
        transform.GetComponent<SelectUnit>().Restart();
        transform.GetComponent<ClickController>().Restart();
        transform.GetComponent<PlaneManager>().Restart();
        transform.GetComponent<BuildManager>().Restart();
        transform.GetComponent<PlayerController>().Restart();
        transform.GetComponent<StartingPoint>().Restart();
        transform.GetComponent<MainMenu>().Restart();
        transform.GetComponent<PhotonController>().Restart();
        enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    void Check()
    {

        if (end_of_game)
        {
            if(waitcount2 <= 0)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    end_of_game = false;
                    Restart();
                    return;
                }
            }
            else
            {
                waitcount2 = waitcount2 - Time.deltaTime;
            }
            if (win_or_lose)
            {
                settext();
                waitcount = waitcount - Time.deltaTime;
                if (waitcount <= 0)
                {
                    int num = Random.Range(1, 4);
                    for (int i = 0; i < num; i++)
                        SpawnFireWork();
                    waitcount = Random.Range(0.2f, 1.0f);
                }
            }
            else
            {
                settext();
                waitcount = waitcount - Time.deltaTime;
                if (waitcount <= 0)
                {
                    int num = Random.Range(1, 4);
                    for (int i = 0; i < num; i++)
                        SpawnLoseEffect();
                    waitcount = Random.Range(0.2f, 1.0f);
                }
            }
        }
        if (PhotonNetwork.playerList.Length == 1 && !end_of_game)
        {
            win_or_lose = true;
            end_of_game = true;
            resulttext.SetActive(true);
            guidetext.SetActive(true);
            ShowResult("승리!!");
            PhotonNetwork.LeaveRoom();
            BasicCanvas.SetActive(false);
            waitcount2 = 1;
            //transform.GetComponent<ScoreController>().enabled = false;
        }
        else if(MyBuildings < 1 && !end_of_game)
        {
            win_or_lose = false;
            end_of_game = true;
            resulttext.SetActive(true);
            guidetext.SetActive(true);
            ShowResult("패배!!");
            PhotonNetwork.LeaveRoom();
            BasicCanvas.SetActive(false);
            waitcount2 = 1;
            //transform.GetComponent<ScoreController>().enabled = false;
        }
    }

    void SpawnFireWork()
    {
        float r = Random.Range(-7.0f, 7.0f);
        float u = Random.Range(-5.0f, 5.0f);
        Instantiate(firework, Camera.main.transform.position + (Camera.main.transform.forward.normalized * 7) + (Camera.main.transform.right.normalized * r) + (Camera.main.transform.up.normalized * u), Quaternion.identity);
    }

    void SpawnLoseEffect()
    {
        float r = Random.Range(-7.0f, 7.0f);
        float u = Random.Range(-5.0f, 5.0f);
        Instantiate(LoseEffect, Camera.main.transform.position + (Camera.main.transform.forward.normalized * 7) + (Camera.main.transform.right.normalized * r) + (Camera.main.transform.up.normalized * u), Quaternion.identity);
    }

    void ShowResult(string str)
    {
        resulttext.GetComponent<Text>().text = str;
    }

    void settext()
    {
        if (x >= 1)
        {
            plus = -1;
        }
        else if (x <= 0)
        {
            plus = 1;
        }
        x = x + (Time.deltaTime * plus);
        guidetext.GetComponent<Text>().color = new Color(1, 1, 1, x);
    }
}
