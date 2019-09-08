using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctom : MonoBehaviour
{
    [SerializeField]
    List<GameObject> effects;

    public int preowner = 0;
    public int owner = 0;
    public bool player1 = false;
    public bool player2 = false;
    public bool isowned = false;
    private Color[] color = { Color.white, Color.red, Color.blue };

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Renderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        check();
        foreach(GameObject obj in effects)
        {
            var main = obj.GetComponent<ParticleSystem>().main;
            main.startColor = color[owner];
        }
    }

    public void check()
    {
        player1 = false;
        player2 = false;
        isowned = false;
        Collider[] colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 10, 0), transform.position - new Vector3(0, 10, 0), 10f);
        foreach (Collider col in colliders)
        {
            if ((col.gameObject.tag == "TerraformingBuilding" || col.gameObject.tag == "ResourceBuilding") && col.GetComponent<BuildingStatus>().builded)
            {
                int playernum = col.GetComponent<PhotonView>().viewID / 1000;
                if(playernum == 1)
                {
                    player1 = true;
                }
                else
                {
                    player2 = true;
                }
            }
        }
        if (player1)
            isowned = !isowned;
        if (player2)
            isowned = !isowned;

        preowner = owner;
        if(isowned && player1)
        {
            owner = 1;
        }
        else if(isowned && player2)
        {
            owner = 2;
        }
        else
        {
            owner = 0;
        }

        if(preowner != owner)
        {
            if(preowner == PlayerController.GetPlayerNum())
            {
                PlayerController.SetSanctomCount(PlayerController.GetSanctomCount() - 1);
            }
            else if(owner == PlayerController.GetPlayerNum())
            {
                PlayerController.SetSanctomCount(PlayerController.GetSanctomCount() + 1);
            }
        }
    }
}
