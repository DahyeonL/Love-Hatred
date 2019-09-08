using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhotonController : Photon.PunBehaviour
{
    public static PhotonController controller;
    
    [SerializeField]
    GameObject Worker;
    [SerializeField]
    GameObject UnitCanvas;
    [SerializeField]
    GameObject ResourceCanvas;
    [SerializeField]
    GameObject BasicCanvas;

    public void Restart()
    {
        UnitCanvas.GetComponent<UnitCanvasController>().Restart();
        ResourceCanvas.GetComponent<UnitCanvasController>().Restart();
    }

    private void Awake()
    {
        if(controller != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        controller = this;

        PhotonNetwork.automaticallySyncScene = true;
    }
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings("Elements");
    }

    private void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("게임룸 진입 완료");
        PlayerController.SetPlayerNum(PhotonNetwork.playerList.Length);

        BasicCanvas.SetActive(true);
        UnitCanvas.GetComponent<UnitCanvasController>().UnitCanvasSetting();
        ResourceCanvas.GetComponent<UnitCanvasController>().UnitCanvasSetting();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected");
    }

    public void spawnFlama()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded) {
                obj.GetComponent<UnitCreateBuilding>().CreateFlama();
            }
        }
    }
    
    public void spawnCrepo()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateCrepo();
            }
        }
    }

    public void spawnFli()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateFli();
            }
        }
    }

    public void spawnSilba()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateSilba();
            }
        }
    }


    public void spawnGari()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateGari();
            }
        }
    }

    public void spawnHema()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateHema();
            }
        }
    }

    public void spawnSora()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateSora();
            }
        }
    }

    public void spawnCuro()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateCuro();
            }
        }
    }

    public void spawnRuru()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateRuru();
            }
        }
    }

    public void spawnBear()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateBear();
            }
        }
    }

    public void spawnDragon()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateDragon();
            }
        }
    }

    public void spawnFairy()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "UnitCreateBuilding" && !obj.GetComponent<UnitCreateBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<UnitCreateBuilding>().CreateFairy();
            }
        }
    }

    public void spawnFireWorker()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "ResourceBuilding" && !obj.GetComponent<ResourceBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<ResourceBuilding>().CreateFireWorker();
            }
        }
    }

    public void spawnWaterWorker()
    {
        foreach (GameObject obj in SelectUnit.SelectedBuilding)
        {
            if (obj.tag == "ResourceBuilding" && !obj.GetComponent<ResourceBuilding>().Creating && obj.GetComponent<BuildingStatus>().builded)
            {
                obj.GetComponent<ResourceBuilding>().CreateWaterWorker();
            }
        }
    }

    public void LeaveRoom()
    {
        ScoreController.MyBuildings = -1;
    }
}
