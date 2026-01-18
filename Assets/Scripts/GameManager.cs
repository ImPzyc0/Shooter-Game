using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public Client client;
    
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public PlayerManager localPlayer; 
    public GameObject otherPlayerPrefab;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }
    
    /// <summary>Diese Methode wird für JEDEN Spieler aufgerufen, der dem Spiel beitritt (auch für den lokalen).</summary>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        PlayerManager _playerManager;
        
        // Wenn die ID mit unserer eigenen Client-ID übereinstimmt...
        if (_id == Client.instance.myId)
        {
            // ...dann instanziieren wir nichts Neues. Wir benutzen die Referenz 'localPlayer'.
            _playerManager = localPlayer;
            _playerManager.id = _id;
            _playerManager.username = _username;
            
            // Setze die Startposition, die vom Server kommt.
            // Der Spieler wird an den zufälligen Spawnpunkt teleportiert.
            _playerManager.transform.position = _position;
            _playerManager.transform.rotation = _rotation;
        }
        else
        {
            // Für alle anderen Spieler instanziieren wir wie gewohnt das Prefab.
            GameObject _otherPlayerGO = Instantiate(otherPlayerPrefab, _position, _rotation);
            _playerManager = _otherPlayerGO.GetComponent<PlayerManager>();
            _playerManager.id = _id;
            _playerManager.username = _username;
        }

        // Füge den Spieler (egal ob lokal oder remote) zum Dictionary hinzu.
        players.Add(_id, _playerManager);
    }
    
    

    private void Start()
    {
        
        client.ConnectToServer();
        
    }
    
    public void RemovePlayer(int _id)
    {
        if (players.ContainsKey(_id))
        {
            Destroy(players[_id].gameObject);
            players.Remove(_id);
        }
    }
    
}
