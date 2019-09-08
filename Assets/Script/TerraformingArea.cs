using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraformingArea : Photon.MonoBehaviour
{
    [SerializeField]
    GameObject TerraArea;

    Color[] color = { new Color(1, 0, 0, 0.15f), new Color(0, 0, 1, 0.15f) };
    bool x = true;
    // Start is called before the first frame update
    void Start()
    {
        
        if (transform.GetComponent<PhotonView>().isMine)
        {
            TerraArea.GetComponent<Renderer>().material.color = color[PlayerController.GetPlayerNum() - 1];
        }
        else
        {
            TerraArea.GetComponent<Renderer>().material.color = color[(PlayerController.GetPlayerNum()) % 2];
        }
    }

    public void OnTerra()
    {
        TerraArea.SetActive(true);
    }
    public void OffTerra()
    {
        TerraArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (x && !photonView.isMine && transform.GetComponent<Status>().GetVisible())
        {
            PlayerController.TerraBuildings.Add(gameObject);
            x = false;
        }
        else if (x && photonView.isMine && transform.GetComponent<BuildingStatus>().builded)
        {
            PlayerController.TerraBuildings.Add(gameObject);
            x = false;
        }
    }

    private void OnDestroy()
    {
        if(transform.GetComponent<Status>().GetHp() <= 0)
            PlayerController.TerraBuildings.Remove(gameObject);
    }
}
