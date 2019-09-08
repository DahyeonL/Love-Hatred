using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject background;

    [SerializeField]
    GameObject image;

    [SerializeField]
    GameObject text;

    [SerializeField]
    GameObject blinder;
    
    bool selectFire = false;
    bool selectWater = false;
    bool selectEarth = false;
    bool start = false;
    bool back = false;
    float x = 1;
    float y = 1;
    int plus = -1;
    int plusy = -1;
    float waitcount = 0;
    bool notselect = true;

    public void Restart()
    {

    }

    private void Awake()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("Elements");
    }

    private string roomName = "myRoom";
    private Vector2 scrollPos = Vector2.zero;

    void settext()
    {
        if (x >= 1)
        {
            plus = -1;
        }
        else if(x <= 0)
        {
            plus = 1;
        }
        x = x + (Time.deltaTime * plus);
        text.GetComponent<Text>().color = new Color(1, 1, 1, x);
    }

    void OnGUI()
    {
        if(!start && waitcount < 1.2f)
        {
            waitcount = waitcount + Time.deltaTime;
            blinder.GetComponent<RawImage>().color = new Color(49/255f, 49 / 255f, 49 / 255f, 1 - (waitcount / 1.2f));
        }
        if (start)
        {
            if (!back && waitcount < 1.2)
            {
                waitcount = waitcount + Time.deltaTime;
                image.GetComponent<RawImage>().color = new Color(1, 1, 1, 1 - (waitcount / 1.2f));
            }
            else if(!back && waitcount >= 1.2)
            {
                back = true;
                waitcount = 0;
            }
            else
            {
                if (waitcount < 1.2f)
                {
                    waitcount = waitcount + Time.deltaTime;
                    background.GetComponent<RawImage>().color = new Color(0, 0, 0, 1 - (waitcount / 1.2f));
                }
                else
                {
                    transform.GetComponent<PlaneManager>().enabled = true;
                    Destroy(background);
                    if (PlaneManager.plane && !ScoreController.end_of_game)
                        ShowMainMenu();
                }
            }
        }
        else
        {
            Event e = Event.current;
            if (e.isMouse)
            {
                if (e.clickCount > 0)
                {
                    start = true;
                    Destroy(text);
                    waitcount = 0;
                }
            }
            if (PhotonNetwork.connected)
            {
                text.SetActive(true);
                settext();
            }
        }
    }

    public void OnConnectedToMaster()
    {
        // this method gets called by PUN, if "Auto Join Lobby" is off.
        // this demo needs to join the lobby, to show available rooms!

        PhotonNetwork.JoinLobby();  // this joins the "default" lobby
    }

    void ShowMainMenu()
    {
        GUI.skin.button.fontSize = 40;
        GUIStyle guistyle = new GUIStyle();

        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room

        GUILayout.BeginArea(new Rect(100, 10, Screen.width * 0.6f, Screen.height - 20));

        guistyle.fontSize = 100;
        guistyle.normal.textColor = Color.white;

        GUILayout.Label("Room List", guistyle);
        GUILayout.Space(40);
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            guistyle.fontSize = 50;
            guistyle.normal.textColor = Color.white;
            GUILayout.Label("..no games available..", guistyle);
        }
        else
        {
            guistyle.fontSize = 50;
            guistyle.normal.textColor = Color.white;
            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(game.Name, guistyle, GUILayout.Width(550));
                GUILayout.Label(game.PlayerCount + "  /  " + game.MaxPlayers, guistyle, GUILayout.Width(200));
                GUI.skin.button.fontSize = 40;
                if (GUILayout.Button("JOIN", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    if (selectFire || selectWater || selectEarth)
                        PhotonNetwork.JoinRoom(game.Name);
                    else
                    {
                        Debug.Log("종족을 선택해주세요.");
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.6f + 50, 10, Screen.width - 200, Screen.height - 20));

        GUILayout.Space(70);

        GUILayout.Label("CREATE Room", guistyle);

        GUILayout.Space(10);
        GUI.skin.textField.fontSize = 40;
        roomName = GUILayout.TextField(roomName, GUILayout.Height(50), GUILayout.Width(600));

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete", GUILayout.Width(300), GUILayout.Height(50)))
        {
            roomName = "";
        }

        if (GUILayout.Button("Create", GUILayout.Width(300), GUILayout.Height(50)))
        {
            if (selectFire || selectWater || selectEarth)
                PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
            else
            {
                Debug.Log("종족을 선택해주세요.");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(100);

        GUILayout.Label("JOIN Room", guistyle);

        GUILayout.Space(10);
        GUI.skin.textField.fontSize = 40;
        roomName = GUILayout.TextField(roomName, GUILayout.Height(50), GUILayout.Width(600));

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete", GUILayout.Width(300), GUILayout.Height(50)))
        {
            roomName = "";
        }

        if (GUILayout.Button("Join", GUILayout.Width(300), GUILayout.Height(50)))
        {
            if (selectFire || selectWater || selectEarth)
                PhotonNetwork.JoinRoom(roomName);
            else
            {
                Debug.Log("종족을 선택해주세요.");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(100);

        GUILayout.Label("RANDOM Room", guistyle);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Join", GUILayout.Height(50), GUILayout.Width(600)))
        {
            if (selectFire || selectWater || selectEarth)
                PhotonNetwork.JoinRandomRoom();
            else
            {
                Debug.Log("종족을 선택해주세요.");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(100);
        if (notselect)
        {
            if (y >= 1)
            {
                plusy = -1;
            }
            else if (y <= 0)
            {
                plusy = 1;
            }
            y = y + (Time.deltaTime * plusy);
            guistyle.normal.textColor = new Color(1, 1, 1, y);
        }
        else
        {
            guistyle.normal.textColor = new Color(1, 1, 1, 1);
        }
        GUILayout.Label("Select Tribe", guistyle);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (selectFire)
        {
            GUI.skin.button.normal.textColor = Color.red;
        }
        else
        {
            GUI.skin.button.normal.textColor = Color.white;
        }

        if (GUILayout.Button("Fire", GUILayout.Width(200), GUILayout.Height(50)))
        {
            PlayerController.SetTribe("Fire");
            notselect = false;
            selectFire = true;
            selectWater = false;
            selectEarth = false;
        }
        if (selectWater)
        {
            GUI.skin.button.normal.textColor = Color.blue;
        }
        else
        {
            GUI.skin.button.normal.textColor = Color.white;
        }
        if (GUILayout.Button("Water", GUILayout.Width(200), GUILayout.Height(50)))
        {
            PlayerController.SetTribe("Water");
            notselect = false;
            selectFire = false;
            selectEarth = false;
            selectWater = true;
        }
        if (selectEarth)
        {
            GUI.skin.button.normal.textColor = new Color(0.6f, 0.45f, 0.12f);
        }
        else
        {
            GUI.skin.button.normal.textColor = Color.white;
        }
        if (GUILayout.Button("Earth", GUILayout.Width(200), GUILayout.Height(50)))
        {
            PlayerController.SetTribe("Earth");
            notselect = false;
            selectEarth = true;
            selectFire = false;
            selectWater = false;
        }
        GUI.skin.button.normal.textColor = Color.white;
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
