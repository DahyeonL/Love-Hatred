using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStatus : MonoBehaviour
{
    private float half_width;
    private float half_height;
    private float half_thickness;
    private Vector3[] Edges = new Vector3[4];
    public bool ARMove = false;
    private Vector3 ConnectedEdgeVector;
    private string ConnectedEdgeString;
    private float time = 0f;

    void Start()
    {
        half_width = transform.GetComponent<Renderer>().bounds.size.x / 2f;
        half_height = transform.GetComponent<Renderer>().bounds.size.y / 2f;
        half_thickness = transform.GetComponent<Renderer>().bounds.size.z / 2f;
        Edges[0] = transform.position + new Vector3(0, half_height, half_thickness);
        Edges[1] = transform.position + new Vector3(half_width, half_height, 0);
        Edges[2] = transform.position + new Vector3(0, half_height, -half_thickness);
        Edges[3] = transform.position + new Vector3(-half_width, half_height, 0);
        transform.GetComponent<Renderer>().material.color = new Color(144f / 255f, 142f / 255f, 142f / 255f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdge();
    }

    public bool getARMove()
    {
        return ARMove;
    }

    public Vector3 getConnectedEdgeVector()
    {
        return ConnectedEdgeVector;
    }

    public string getConnectedEdgeString()
    {
        return ConnectedEdgeString;
    }

    void CheckEdge()
    {
        for(int i = 0; i < 4; i++)
        {
            Vector3 EdgeToScreen = Camera.main.WorldToScreenPoint(Edges[i]);
            Ray ray = Camera.main.ScreenPointToRay(EdgeToScreen);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if(i == 0)
                {
                    if (hitInfo.transform.name == "Bottom")
                    {
                        if(time < 1)
                        {
                            time = time + Time.deltaTime;
                            transform.GetComponent<Renderer>().material.color = new Color(144f / 255f, 142f / 255f, 142f / 255f) + new Color(6f * time / 255f, 113f * time / 255f, 113f * time / 255f);
                        }
                        else
                        {
                            ARMove = true;
                        }
                        ConnectedEdgeVector = Edges[i];
                        ConnectedEdgeString = "Top";
                        return;
                    }
                }
                else if (i == 1)
                {
                    if (hitInfo.transform.name == "Left")
                    {
                        if (time < 1)
                        {
                            time = time + Time.deltaTime;
                            transform.GetComponent<Renderer>().material.color = new Color(144f / 255f, 142f / 255f, 142f / 255f) + new Color(6f * time / 255f, 113f * time / 255f, 113f * time / 255f);
                        }
                        else
                        {
                            ARMove = true;
                        }
                        ConnectedEdgeVector = Edges[i];
                        ConnectedEdgeString = "Right";
                        return;
                    }
                }
                else if (i == 2)
                {
                    if (hitInfo.transform.name == "Top")
                    {
                        if (time < 1)
                        {
                            time = time + Time.deltaTime;
                            transform.GetComponent<Renderer>().material.color = new Color(144f / 255f, 142f / 255f, 142f / 255f) + new Color(6f * time / 255f, 113f * time / 255f, 113f * time / 255f);
                        }
                        else
                        {
                            ARMove = true;
                        }
                        ConnectedEdgeVector = Edges[i];
                        ConnectedEdgeString = "Bottom";
                        return;
                    }
                }
                else if (i == 3)
                {
                    if (hitInfo.transform.name == "Right")
                    {
                        if (time < 1)
                        {
                            time = time + Time.deltaTime;
                            transform.GetComponent<Renderer>().material.color = new Color(144f / 255f, 142f / 255f, 142f / 255f) + new Color(6f * time / 255f, 113f * time / 255f, 113f * time / 255f);
                        }
                        else
                        {
                            ARMove = true;
                        }
                        ConnectedEdgeVector = Edges[i];
                        ConnectedEdgeString = "Left";
                        return;
                    }
                }
            }
        }
        if (time <= 0)
        {
            ARMove = false;
        }
        else
        {
            time = time - Time.deltaTime;
            transform.GetComponent<Renderer>().material.color = new Color(144f / 255f, 142f / 255f, 142f / 255f) + new Color(6f * time / 255f, 113f * time / 255f, 113f * time / 255f);
        }
    }
}
