using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhotonController : Photon.PunBehaviour
{
    public static PhotonController controller;

    private void Awake()
    {
        if(controller != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        controller = this;

        PhotonNetwork.automaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("Elements");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void joinGame()
    {
        if (PhotonNetwork.connected == true)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("room", options, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("게임룸 진입 완료");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected");
    }
}
