using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Debug=UnityEngine.Debug;

class GameState
{
    private Player _player;
    public Player player
    {
        get => this._player;
    }
    public World _world;
    public World World
    {
        get => this._world;
    }
    public WorldStage _stage;
    public WorldStage Stage
    {
        get => this._stage;
    }
    
    private static readonly GameState instance = new GameState();
    static GameState() {}
    private GameState() {}
    public static GameState Instance  
    {  
        get => instance;
    }

    public void ResetPlayer()
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
        this._player.ResetWeaponList();
        this._player.ResetEquipment();
    }

    public void StartWorld(int number, string name)
    {
        this._world = new World(number, name);
        this._stage = this.World.GetStage(1);
    }

    public void MoveToNextStage()
    {
        this._stage = this.World.GetStage(this.Stage.Number + 1);
    }
}
