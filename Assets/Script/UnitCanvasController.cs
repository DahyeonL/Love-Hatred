using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCanvasController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Fire;
    [SerializeField]
    List<GameObject> Water;

    public void Restart()
    {
        foreach (GameObject button in Fire)
        {
            button.gameObject.SetActive(false);
        }
        foreach (GameObject button in Water)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void UnitCanvasSetting()
    {
        if(PlayerController.GetTribe() == "Fire")
        {
            foreach(GameObject button in Fire)
            {
                button.gameObject.SetActive(true);
            }
        }
        else if(PlayerController.GetTribe() == "Water")
        {
            foreach (GameObject button in Water)
            {
                button.gameObject.SetActive(true);
            }
        }
    }
}
