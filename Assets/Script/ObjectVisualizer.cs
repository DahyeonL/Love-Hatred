using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisualizer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Child = new List<GameObject>();
    
    public void SetInvisible()
    {
        foreach(GameObject obj in Child)
        {
            obj.SetActive(false);
        }
    }

    public void SetVisible()
    {
        foreach (GameObject obj in Child)
        {
            obj.SetActive(true);
        }
    }
}
