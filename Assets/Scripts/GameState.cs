using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class GameState
{
    private Player _player;
    public Player player
    {
        get => this._player;
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
                critical =0.05f,
            }
        ) ;
    }
    public static GameState Instance  
    {  
        get => instance;
    }
}
