using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingPoint : Photon.MonoBehaviour
{
    [SerializeField]
    GameObject effect;
    [SerializeField]
    GameObject FIre_ResourceBuilding;
    [SerializeField]
    GameObject FIre_Worker;
    [SerializeField]
    GameObject Water_ResourceBuilding;
    [SerializeField]
    GameObject Water_Worker;
    [SerializeField]
    Text CountDownText;
    [SerializeField]
    GameObject Arrow;
    private int startingPoint;
    public static bool IsStart = false;
    bool stop = false;
    float Count = 4;
    GameObject arrow;
    Vector3 Pos;

    Color[] color = { Color.red, Color.blue };

    public void Restart()
    {
        IsStart = false;
        stop = false;
        Count = 4;
        enabled = true;
        CountDownText.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.GetPlayerNum() != -1 && PhotonNetwork.playerList.Length == 2)
        {
            if (!stop)
            {
                stop = true;
                PlaneManager.Map.GetComponent<BoxCollider>().enabled = false;
                startingPoint = (PlayerController.GetPlayerNum() * 10) + Random.Range(0, 2);
                if (PlayerController.GetTribe() == "Fire")
                {
                    CreateBasic(startingPoint, FIre_ResourceBuilding, FIre_Worker);
                }
                else if (PlayerController.GetTribe() == "Water")
                {
                    CreateBasic(startingPoint, Water_ResourceBuilding, Water_Worker);
                }
                transform.GetComponent<ScoreController>().enabled = true;
                transform.GetComponent<SelectUnit>().enabled = true;
                arrow = Instantiate(Arrow, Camera.main.transform.position + (Camera.main.transform.forward.normalized * 7), Quaternion.identity);
                arrow.GetComponent<Renderer>().material.color = color[PlayerController.GetPlayerNum() - 1];
                arrow.transform.LookAt(Pos);
                arrow.transform.Rotate(90, 0, 0);
            }
            Count = Count - Time.deltaTime;
            arrow.transform.position = Camera.main.transform.position + (Camera.main.transform.forward.normalized * 7);
            arrow.transform.LookAt(Pos);
            arrow.transform.Rotate(90, 0, 0);
            if (Count <= 0)
            {
                transform.GetComponent<StartingPoint>().enabled = false;
                CountDownText.text = "";
                CountDownText.enabled = false;
                Destroy(arrow);
            }
            else if (Count <= 1)
            {
                CountDownText.text = "Game Start!!";
                IsStart = true;
            }
            else
            {
                CountDownText.text = ((int)Count).ToString();
            }
        }
    }

    public void CreateBasic(int startPoint, GameObject Building, GameObject Worker)
    {
        if(startPoint == 10)
        {
            Pos = new Vector3(44.3f, 2, 43.8f);
        }
        else if(startPoint == 11)
        {
            Pos = new Vector3(44.3f, 2, -43.8f);
        }
        else if (startPoint == 20)
        {
            Pos = new Vector3(-44.3f, 2, -43.8f);
        }
        else if (startPoint == 21)
        {
            Pos = new Vector3(-44.3f, 2, 43.8f);
        }
        GameObject Effect = Instantiate(effect, Pos, new Quaternion(0,0,0,0));
        Effect.transform.Rotate(-90, 0, 0);
        var main = Effect.GetComponent<ParticleSystem>().main;
        main.startColor = color[PlayerController.GetPlayerNum()-1];
        GameObject building = PhotonNetwork.Instantiate(Building.name, Pos, Quaternion.identity, 0);
        GameObject worker = PhotonNetwork.Instantiate(Worker.name, Pos, Quaternion.identity, 0);
        building.transform.SetParent(PlaneManager.Map.transform, true);
        worker.transform.SetParent(PlaneManager.Map.transform, true);
        building.transform.localPosition = Pos;
        worker.transform.localPosition = Pos;
        building.GetComponent<BuildingController>().Condition = "Starting";
        worker.GetComponent<Status>().SetCondition("Starting");
    }
}
