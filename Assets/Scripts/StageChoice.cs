﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageChoice : MonoBehaviour
{
    StageCard selectedCard;

    public static Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    
    GameObject CardSelectPanel;
    GameObject NpcPanel;
    GameObject NpcPanel_Merchant;
    GameObject NpcPanel_Healer;
    GameObject NpcPanel_Enchanter;
    GameObject BuffPanel;
    GameObject ChestPanel;
    GameObject BattlePanel;
    GameObject RandomPanel;
    GameObject WeaponPopupView;
    GameObject WeaponChangePanel;
    
    WorldSelectPanelController WorldSelect;

    Text WorldText;
    Text StageText;

    Text CardText1;
    Text CardText2;
    Text CardText3;

    void Start()
    {
        CardSelectPanel = GameObject.Find("CardSelectPanel");
        NpcPanel = GameObject.Find("NpcPanel");
        NpcPanel_Merchant = GameObject.Find("NpcPanel_Merchant");
        NpcPanel_Healer = GameObject.Find("NpcPanel_Healer");
        NpcPanel_Enchanter = GameObject.Find("NpcPanel_Enchanter");
        BuffPanel = GameObject.Find("BuffPanel");
        ChestPanel = GameObject.Find("ChestPanel");
        BattlePanel = GameObject.Find("BattlePlayerAttackPanel");
        RandomPanel = GameObject.Find("RandomPanel");
        WeaponChangePanel = GameObject.Find("WeaponChangePanel");
        WeaponPopupView = GameObject.Find("WeaponPopupView/WeaponPopupScreen");

        WorldSelect = GameObject.Find("WorldSelectPanel").GetComponent<WorldSelectPanelController>();

        WorldText = GameObject.Find("World Text").GetComponent<Text>();
        StageText = GameObject.Find("Stage Text").GetComponent<Text>();

        CardText1 = GameObject.Find("Card1 Text").GetComponent<Text>();
        CardText2 = GameObject.Find("Card2 Text").GetComponent<Text>();
        CardText3 = GameObject.Find("Card3 Text").GetComponent<Text>();

        GameState.Instance.ResetPlayer();
        GameState.Instance.StartWorld(GameConstant.InitialWorldId);
        ActivatePannel();
    }

    void Update()
    {
        var stageCards = GameState.Instance.Stage.Cards;
        CardText1.text = stageCards[0].Type.ToString();
        CardText2.text = stageCards[1].Type.ToString();
        CardText3.text = stageCards[2].Type.ToString();
    }

    public void OnClickCard(int index)
    {
        selectedCard = GameState.Instance.Stage.Cards[index];
        ActivatePannel();
    }

    public void MoveToNextStage()
    {
        selectedCard = null;
        GameState.Instance.MoveToNextStage();
        GameState.Instance.player.Heal((int)(GameState.Instance.player.GetStat().maxHp * GameState.Instance.player.GetStat().stageHpDrain));
        GameState.Instance.player.HpOver();
        ActivatePannel();
    }
    
    public void MoveToNextWorld()
    {
        DeactiveAllPanel();

        WorldId[] worldIds = GameState.Instance.World.GetNextWorldIds();
        if (worldIds.Length == 1)
            AfterWorldSelected(worldIds[0]);
        else if (worldIds.Length == 2)
            WorldSelect.DisplayPanel(worldIds[0], worldIds[1], AfterWorldSelected);
        else
            throw new InvalidOperationException($"Invalid number of world ids; {worldIds.Length}");

        void AfterWorldSelected(WorldId selectedWorld)
        {
            selectedCard = null;
            GameState.Instance.StartWorld(selectedWorld);
            GameState.Instance.player.Heal(GameState.Instance.player.GetStat().maxHp);
            GameState.Instance.player.HpOver();
            UpdateGamePanel();
        }
    }

    void ActivatePannel()
    {
        DeactiveAllPanel();
        UpdateGamePanel();
    }

    void DeactiveAllPanel()
    {
        CardSelectPanel.SetActive(false);
        NpcPanel.SetActive(false);
        NpcPanel_Merchant.SetActive(false);
        NpcPanel_Healer.SetActive(false);
        NpcPanel_Enchanter.SetActive(false);
        BuffPanel.SetActive(false);
        ChestPanel.SetActive(false);
        BattlePanel.SetActive(false);
        RandomPanel.SetActive(false);
        WeaponPopupView.SetActive(false);
    }

    public void UpdateGamePanel()
    {
        WorldText.text = GameState.Instance.World.Id.ToString();
        StageText.text = $"Stage {GameState.Instance.Stage.Number}";
        
        switch (selectedCard?.Type ?? null)
        {
            case null:
                CardSelectPanel.SetActive(true);
                CardSelectPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Monster:
                var battleController = BattlePanel.GetComponent<BattleController>();
                var battlePlayerAttackPanelController = BattlePanel.GetComponent<BattlePlayerAttackPanelController>();
                battleController.MonsterCard = (selectedCard as MonsterCard);
                battlePlayerAttackPanelController.MonsterCard = (selectedCard as MonsterCard);
                BattlePanel.SetActive(true);
                BattlePanel.transform.localPosition = PanelDisplayPosition;
                GameState.Instance.player.AddLocation(Location.Monster);
                break;
            case CardType.Boss:
                var battleController2 = BattlePanel.GetComponent<BattleController>();
                var battlePlayerAttackPanelController2 = BattlePanel.GetComponent<BattlePlayerAttackPanelController>();
                battleController2.MonsterCard = (selectedCard as MonsterCard);
                battlePlayerAttackPanelController2.MonsterCard = (selectedCard as MonsterCard);
                battlePlayerAttackPanelController2.BossCard = (selectedCard as BossCard);
                BattlePanel.SetActive(true);
                BattlePanel.transform.localPosition = PanelDisplayPosition;
                GameState.Instance.player.AddLocation(Location.Monster);
                break;
            case CardType.Chest:
                var chestController = ChestPanel.GetComponent<ChestPanelController>();
                chestController.ChestCard = (selectedCard as ChestCard);
                ChestPanel.SetActive(true);
                ChestPanel.transform.localPosition = PanelDisplayPosition;
                GameState.Instance.player.AddLocation(Location.Chest);
                break;
            case CardType.Buff:
                var buffController = BuffPanel.GetComponent<BuffPanelController>();
                buffController.BuffCard = (selectedCard as BuffCard);
                BuffPanel.SetActive(true);
                BuffPanel.transform.localPosition = PanelDisplayPosition;
                GameState.Instance.player.AddLocation(Location.Buff);
                break;
            case CardType.Random:
                var RandomController = RandomPanel.GetComponent<RandomPanelController>();
                RandomController.RandomCard = (selectedCard as RandomCard);
                RandomPanel.SetActive(true);
                RandomPanel.transform.localPosition = PanelDisplayPosition;
                GameState.Instance.player.AddLocation(Location.Random);
                break;
            case CardType.Npc:
                var NpcController = NpcPanel.GetComponent<NpcPanelController>();
                NpcController.NpcCard = (selectedCard as NpcCard);
                NpcPanel.SetActive(true);
                NpcPanel.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Merchant.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Healer.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Enchanter.transform.localPosition = PanelDisplayPosition;
                GameState.Instance.player.AddLocation(Location.Npc);
                break;
        }
    }
}
