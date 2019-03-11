using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0, 0, 0, 0);
    private Vector3 startClick = -Vector3.one;
    public static bool isUnit = false;
    public static bool isDragged = false;
    public static RaycastHit hitInfo;
    public static List<GameObject> Selected = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selection.size = new Vector2(0, 0);
            startClick = Input.mousePosition;
            isUnit = false;
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
                    }
                    else
                    {
                        isUnit = false;
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
