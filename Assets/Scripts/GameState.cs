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
    private World _world;
    public World World
    {
        get => this._world;
    }
    private WorldStage _stage;
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
        this._player = new Player(GameConstant.PlayerInitialStat.DeepCopy());
        this._player.money = GameConstant.PlayerInitialMoney;
        this._player.ResetWeaponList();
        this._player.ResetEquipment();
    }

    public void StartWorld(WorldId worldId)
    {
        this._world = new World(worldId);
        WorldSoundController.WSC_instance.SoundState = (int)worldId;
        WorldSoundController.WSC_instance.WorldSoundControl();
        this._stage = this.World.GetStage(1);
    }

    public void MoveToNextStage()
    {
        this._stage = this.World.GetStage(this.Stage.Number + 1);
    }
}
