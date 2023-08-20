using System.Collections.Generic;
using UnityEngine;
using Colyseus;


public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private const string GAME_ROOM_NAME = "state_handler";

    [field: SerializeField] public Skins _skins;
    [field: SerializeField] public LossCounter _lossCounter { get; private set; }
    [field: SerializeField] public SpawnPoints _spawnPoints { get; private set; }
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;

    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Instance.InitializeClient();
        Connect();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    private void LeaveRoom()
    {
        _room?.Leave();
    }

    private async void Connect() 
    {
        Vector3 spawnPosition;
        Vector3 spawnRotation;
        _spawnPoints.GetPoint(Random.Range(0, _spawnPoints.length), out spawnPosition, out spawnRotation);

        Dictionary<string, object> data = new Dictionary<string, object>() 
        {
            {"skins",  _skins.length},
            {"points", _spawnPoints.length },
            {"speed",_player.speed },
            {"hp", _player.maxHealth },
            {"pX", spawnPosition.x },
            {"pY", spawnPosition.y },
            {"pZ", spawnPosition.z },
            {"rY", spawnRotation.y },
        };

        _room = await Instance.client.JoinOrCreate<State>(GAME_ROOM_NAME, data);

        _room.OnStateChange += OnChange;

        _room.OnMessage<string>("Shoot", ApplyShoot);
        _room.OnMessage<string>("message", Chat.Instance.ApplyMessage);
    }

    private void ApplyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.LogError("Врага " + shootInfo.key + "нет, а он пытался стрелять");
            return;
        }

        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;

        state.players.ForEach((string key, Player player) => {

            if (key == _room.SessionId) CreatePlayer(player);

            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;


    }

    private void CreatePlayer(Player player)
    {

        var position = new Vector3(player.pX, player.pY, player.pZ);
        Quaternion rotation = Quaternion.Euler(0, player.rY, 0);

        var newPlayer = Instantiate(_player, position, rotation);

        player.OnChange += newPlayer.OnChange;
        _room.OnMessage<int>("restart", newPlayer.GetComponent<Controller>().Restart);

        _player.GetComponent<SetSkin>().Set(_skins.GetMaterial(player.skin));
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ); 
        var enemy = Instantiate(_enemy, position, Quaternion.identity);

        _enemy.GetComponent<SetSkin>().Set(_skins.GetMaterial(player.skin));

        enemy.Init(key, player);
        _enemies.Add(key, enemy);

    }

    private void RemoveEnemy(string key, Player player)
    {
        if (_enemies.ContainsKey(key) == false) return;
        var enemy = _enemies[key];
        enemy.Destroy();
        _enemies.Remove(key);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _room.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetClientSessionId()
    {
        return _room.SessionId;
    }
}
