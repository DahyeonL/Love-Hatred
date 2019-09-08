using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreateBuilding : MonoBehaviour
{
    [SerializeField]
    GameObject Flama;
    [SerializeField]
    GameObject Crepo;
    [SerializeField]
    GameObject Silba;
    [SerializeField]
    GameObject Fli;
    [SerializeField]
    GameObject Ruru;
    [SerializeField]
    GameObject Dragon;

    [SerializeField]
    GameObject Gari;
    [SerializeField]
    GameObject Curo;
    [SerializeField]
    GameObject Hema;
    [SerializeField]
    GameObject Sora;
    [SerializeField]
    GameObject Bear;
    [SerializeField]
    GameObject Fairy;


    public bool Creating = false;
    float CreatingTime;
    GameObject CreatingObject;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Point = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Creating)
        {
            CreatingTime = CreatingTime - Time.deltaTime;
            if(CreatingTime <= 0)
            {
                GameObject obj = PhotonNetwork.Instantiate(CreatingObject.name, transform.position, Quaternion.identity, 0);
                obj.GetComponent<Status>().SetCondition("Create");
                Creating = false;
            }
        }
    }
    
    public void Selected()
    {

    }

    public void CreateFlama()
    {
        if (PlayerController.GetResource() < Flama.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Flama.GetComponent<Status>().GetTime();
        CreatingObject = Flama;
        PlayerController.RemoveResource(Flama.GetComponent<Status>().GetCost());
    }

    public void CreateCrepo()
    {
        if (PlayerController.GetResource() < Crepo.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Crepo.GetComponent<Status>().GetTime();
        CreatingObject = Crepo;
        PlayerController.RemoveResource(Crepo.GetComponent<Status>().GetCost());
    }

    public void CreateSilba()
    {
        
        if (PlayerController.GetResource() < Silba.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Silba.GetComponent<Status>().GetTime();
        CreatingObject = Silba;
        PlayerController.RemoveResource(Silba.GetComponent<Status>().GetCost());
    }

    public void CreateFli()
    {
        if (PlayerController.GetResource() < Fli.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Fli.GetComponent<Status>().GetTime();
        CreatingObject = Fli;
        PlayerController.RemoveResource(Fli.GetComponent<Status>().GetCost());
    }

    public void CreateRuru()
    {

        if (PlayerController.GetResource() < Ruru.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Ruru.GetComponent<Status>().GetTime();
        CreatingObject = Ruru;
        PlayerController.RemoveResource(Ruru.GetComponent<Status>().GetCost());
    }

    public void CreateDragon()
    {
        if (PlayerController.GetResource() < Dragon.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Dragon.GetComponent<Status>().GetTime();
        CreatingObject = Dragon;
        PlayerController.RemoveResource(Dragon.GetComponent<Status>().GetCost());
    }



    public void CreateGari()
    {
        if (PlayerController.GetResource() < Gari.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Gari.GetComponent<Status>().GetTime();
        CreatingObject = Gari;
        PlayerController.RemoveResource(Gari.GetComponent<Status>().GetCost());
    }

    public void CreateCuro()
    {
        if (PlayerController.GetResource() < Curo.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Curo.GetComponent<Status>().GetTime();
        CreatingObject = Curo;
        PlayerController.RemoveResource(Curo.GetComponent<Status>().GetCost());
    }

    public void CreateHema()
    {
        if (PlayerController.GetResource() < Hema.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Hema.GetComponent<Status>().GetTime();
        CreatingObject = Hema;
        PlayerController.RemoveResource(Hema.GetComponent<Status>().GetCost());
    }

    public void CreateSora()
    {
        if (PlayerController.GetResource() < Sora.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Sora.GetComponent<Status>().GetTime();
        CreatingObject = Sora;
        PlayerController.RemoveResource(Sora.GetComponent<Status>().GetCost());
    }

    public void CreateBear()
    {
        if (PlayerController.GetResource() < Bear.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Bear.GetComponent<Status>().GetTime();
        CreatingObject = Bear;
        PlayerController.RemoveResource(Bear.GetComponent<Status>().GetCost());
    }

    public void CreateFairy()
    {
        if (PlayerController.GetResource() < Fairy.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Fairy.GetComponent<Status>().GetTime();
        CreatingObject = Fairy;
        PlayerController.RemoveResource(Fairy.GetComponent<Status>().GetCost());
    }
}
