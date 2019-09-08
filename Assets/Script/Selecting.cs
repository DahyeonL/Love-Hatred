using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecting : Photon.MonoBehaviour
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
        if (!photonView.isMine)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectUnit.InvertMouseY(camPos.y);
            contain = SelectUnit.selection.Contains(camPos, true);
        }
        if (contain)
        {
            if (transform.gameObject.layer == 11 && !SelectUnit.SelectedUnit.Contains(gameObject))
            {
                selected = true;
                SelectUnit.SelectedUnit.Add(gameObject);
            }
            else if (transform.gameObject.layer == 10)
            {
                if (!SelectUnit.SelectedBuilding.Contains(gameObject))
                {
                    if (SelectUnit.SelectedUnit.Count == 0)
                    {
                        selected = true;
                        SelectUnit.SelectedBuilding.Add(gameObject);
                    }
                    else
                    {
                        selected = false;
                    }
                }
                else
                {
                    if (SelectUnit.SelectedUnit.Count > 0)
                    {
                        selected = false;
                        SelectUnit.SelectedBuilding.Remove(gameObject);
                    }
                }
            }
        }
        else if (SelectUnit.isDragged == true)
        {
            selected = false;
            if (gameObject.layer == 11)
                SelectUnit.SelectedUnit.Remove(gameObject);
            else
                SelectUnit.SelectedBuilding.Remove(gameObject);
            /*if (transform.gameObject.layer == 11)
            {
                SelectUnit.SelectedUnit.Remove(gameObject);
            }
            else if (SelectUnit.SelectedUnit.Count == 0)
            {
                SelectUnit.SelectedBuilding.Remove(gameObject);
            }*/
        }
        else if (SelectUnit.isUnit)
        {
            selected = false;
            if (gameObject.layer == 11)
                SelectUnit.SelectedUnit.Remove(gameObject);
            else
                SelectUnit.SelectedBuilding.Remove(gameObject);
            if (gameObject.layer == 11 && SelectUnit.hitInfo.collider.transform.position == transform.position) // 수정 하기 (핸드폰 단일선택 불가 문제)
            {
                selected = true;
                SelectUnit.SelectedUnit.Add(gameObject);
            }
        }
        else if (SelectUnit.isBuilding)
        {
            selected = false;
            if (gameObject.layer == 11)
                SelectUnit.SelectedUnit.Remove(gameObject);
            else
                SelectUnit.SelectedBuilding.Remove(gameObject);
            if (gameObject.layer == 10 && SelectUnit.hitInfo.collider.transform.position == transform.position) // 수정 하기 (핸드폰 단일선택 불가 문제)
            {
                selected = true;
                SelectUnit.SelectedBuilding.Add(gameObject);
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
