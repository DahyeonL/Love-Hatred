using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    [SerializeField]
    GameObject MainCanvas;
    [SerializeField]
    GameObject UnitCanvas;
    [SerializeField]
    GameObject ResourceCanvas;
    [SerializeField]
    GameObject BuildingCanvas;


    public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0, 0, 0, 0);
    private Vector3 startClick = -Vector3.one;
    public static bool isUnit = false;
    public static bool isBuilding = false;
    public static bool isDragged = false;
    public static RaycastHit hitInfo;
    public static List<GameObject> SelectedUnit = new List<GameObject>();
    public static List<GameObject> SelectedBuilding = new List<GameObject>();

    public void Restart()
    {
        selection = new Rect(0, 0, 0, 0);
        startClick = -Vector3.one;
        isUnit = false;
        isBuilding = false;
        isDragged = false;
        SelectedUnit = new List<GameObject>();
        SelectedBuilding = new List<GameObject>();
        MainCanvas.SetActive(false);
        transform.GetComponent<SelectUnit>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UnitCanvas.SetActive(false);
        ResourceCanvas.SetActive(false);
        foreach (GameObject obj in PlayerController.TerraBuildings)
        {
            if (obj == null)
            {
                PlayerController.TerraBuildings.Remove(obj);
            }
            else
                obj.GetComponent<TerraformingArea>().OffTerra();
        }
        if (SelectedBuilding.Count > 0)
        {
            
            foreach (GameObject building in SelectedBuilding)
            {
                if (building.tag == "UnitCreateBuilding")
                {
                    MainCanvas.SetActive(false);
                    BuildingCanvas.SetActive(false);
                    UnitCanvas.SetActive(true);
                }
                else if(building.tag == "ResourceBuilding")
                {
                    MainCanvas.SetActive(false);
                    BuildingCanvas.SetActive(false);
                    ResourceCanvas.SetActive(true);
                    foreach (GameObject obj in PlayerController.TerraBuildings)
                    {
                        obj.GetComponent<TerraformingArea>().OnTerra();
                    }
                }
                else if(building.tag == "TerraformingBuilding")
                {
                    foreach(GameObject obj in PlayerController.TerraBuildings)
                    {
                        obj.GetComponent<TerraformingArea>().OnTerra();
                    }
                }
            }
        }
        else
        {
            MainCanvas.SetActive(true);
            UnitCanvas.SetActive(false);
            ResourceCanvas.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            selection.size = new Vector2(0, 0);
            startClick = Input.mousePosition;
            isUnit = false;
            isBuilding = false;
            isDragged = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selection.width < 0)
            {
                selection.x += selection.width;
                selection.width = -selection.width;
            }
            if (selection.height < 0)
            {
                selection.y += selection.height;
                selection.height = -selection.height;
            }
            startClick = -Vector3.one;
            if (Mathf.Abs(selection.size.x * selection.size.y) < 100)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    int l = hitInfo.transform.gameObject.layer;
                    if (l == 11)
                    {
                        isUnit = true;
                        isBuilding = false;
                    }
                    else if(l == 10)
                    {
                        isBuilding = true;
                        isUnit = false;
                    }
                    else
                    {
                        isUnit = false;
                        isBuilding = false;
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
            if (Mathf.Abs(selection.size.x * selection.size.y) > 100)
            {
                isDragged = true;
            }
        }
    }
    private void OnGUI()
    {
        if (startClick != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionHighlight);
        }
    }
    public static float InvertMouseY(float y)
    {
        return Screen.height - y;
    }
}
