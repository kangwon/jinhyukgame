using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerPrefs = UnityEngine.PlayerPrefs;

class GameState
{
    private Player _player;
    public Player player
    {
        get => this._player;
    }

    public World World;
    public WorldStage Stage;

    private int _globalSeed;
    public int GlobalSeed
    {
        get
        {
            if (_globalSeed == 0)
            {
                string PREF_KEY = "GlobalSeed";
                if (!PlayerPrefs.HasKey(PREF_KEY))
                {
                    var rand = new Random();
                    PlayerPrefs.SetInt(PREF_KEY, rand.Next(100000000));
                }
                _globalSeed = PlayerPrefs.GetInt(PREF_KEY);
            }
            return _globalSeed;
        }
    }
    
    private static readonly GameState instance = new GameState();
    static GameState() {}
    private GameState()
    {
        this._player = new Player
        (
            new Stat()
            {
                maxHp = 20,
                attack = 10,
                defense = 3,
                speed = 12,
                startSpeedGauge = 1,
                evasion = 0.05f,
                critical = 0.05f,
            }
        );
     //        this._player.SetEquipment(JsonDB.GetEquipment($"weapon_000"));
    }
    public static GameState Instance  
    {  
        get => instance;
    }
}
