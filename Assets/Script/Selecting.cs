using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecting : MonoBehaviour
{
    //Selecting
    private bool contain = false;
    public bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectUnit.InvertMouseY(camPos.y);
            contain = SelectUnit.selection.Contains(camPos, true);
        }
        if (contain)
        {
            selected = true;
            SelectUnit.Selected.Add(gameObject);
        }
        else if (SelectUnit.isDragged == true)
        {
            selected = false;
            SelectUnit.Selected.Remove(gameObject);
        }
        else if (SelectUnit.isUnit == true)
        {
            selected = false;
            SelectUnit.Selected.Remove(gameObject);
            if (SelectUnit.hitInfo.collider.transform.position == transform.position)
            {
                selected = true;
                SelectUnit.Selected.Add(gameObject);
            }
        }
        if(selected == true)
        {
            transform.GetComponentInChildren<HPBar>().visible();
        }
        else
        {
            transform.GetComponentInChildren<HPBar>().invisible();
        }
    }
}
