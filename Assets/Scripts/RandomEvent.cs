using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class RandomEvent 
{
    public RandomEventType randomEventType;
    public static void PositiveEvent(Text name,Text description)
    {
        int tempInt;
        switch (Random.Range(0,8)) //case 추가할때 Range 범위도 늘려주자.
        {

            case 0:
                tempInt = 5;
                name.text = $"요리사 이진혁의 맛있는 식사시간";
                description.text = "\"맛있네요!\"" + "\n\n" + $"[공격력 {tempInt} 증가]";
                GameState.Instance.player.baseStat.attack += tempInt;
                break;
            case 1:
                tempInt = 20;
                name.text = $"요리사 이진혁의 배부른 식사시간";
                description.text = "\"맛있네요!\"" + "\n\n"+$"[최대체력 {tempInt} 증가]";
                GameState.Instance.player.baseStat.maxHp += tempInt;
                break;
            case 2:
                tempInt = 3;
                name.text = $"요리사 이진혁의 든든한 식사시간";
                description.text = "\"맛있네요!\"" + "\n\n" + $"[방어력 {tempInt} 증가]";
                GameState.Instance.player.baseStat.defense += tempInt;
                break;
            case 3:
                tempInt = 5;
                name.text = $"요리사 이진혁의 달콤한 식사시간";
                description.text = "\"맛있네요!\"" + "\n\n" + $"[스피드 {tempInt} 증가]";
                GameState.Instance.player.baseStat.speed += tempInt;
                break;
            case 4:
                tempInt = 50;
                name.text = $"운수 좋은 날";
                description.text = "바닥에 반짝이는 뭔가가 있다." + "\n\n" + $"[+{tempInt} 코인 ]";
                GameState.Instance.player.money += tempInt;
                break;
            case 5:
                name.text = $"직원 휴게실";
                description.text = "어쩐지 이공간은 안전한 것 같다." + "\n\n" + $"[모든 체력 회복]";
                GameState.Instance.player.Heal(GameState.Instance.player.baseStat.maxHp);
                break;
            case 6: //TODO : 아직 무적이 구현안됬음. 구현하고 내용 집어넣자.
                name.text = $"행운의 징표";
                description.text = "(아직구현안됨)와! 네잎클로버!" + "\n\n" + $"[다음 전투에서 무적(보스제외)]";
                break;
            case 7:
                name.text = $"테마파크 분수";
                description.text = "정화되는 기분이 든다." + "\n\n" + $"[디버프 제거]";
                GameState.Instance.player.Dispel();
                break;
            default:
                break;
        }
    }
    public static void NeuturalityEvent(Text name, Text description)
    {
        int tempInt;
        switch (Random.Range(0,2)) //case 추가할때 Range 범위도 늘려주자.
        {
            case 0: //TODO : 4가지의 확률에 맞춰 구현하기(장비,아티펙트,재화,티켓 획득)
                name.text = $"풍선 다트";
                description.text = "(아직구현안됨)얍!" + "\n\n";
                switch (CustomRandom<int>.WeightedChoice(new List<int> { 0, 1, 2, 3, 4 }, new List<double> { 0.2f, 0.08f, 0.3f, 0.02f, 0.4f }, new System.Random()))
                {
                    case 0:
                        description.text += $"[장비 획득]";
                        break;
                    case 1:
                        description.text += $"[아티펙트 획득]";
                        break;
                    case 2:
                        description.text += $"[코인 랜덤 획득]";
                        break;
                    case 3:
                        description.text += $"[티켓  랜덤 획득]";
                        break;
                    case 4:
                        description.text += $"[꽝]";
                        break;
                    default:
                        break;
                }
                ;
                break;
            case 1:
                name.text = $"성난 마술사 이진혁의 장난";
                description.text = "(아직구현안됨)\"...이건 제 아티펙트가 아닌 것 같은데요?\"" + "\n\n" + $"[아티펙트 강제 랜덤변경]";
                tempInt = GameState.Instance.player.ArtifactsCount();
                if (tempInt != 0)
                {
                    GameState.Instance.player.RemoveAtArtifact(Random.Range(0, tempInt));
                    GameState.Instance.player.SetEquipment(JsonDB.GetArtifact("artifact0")); //TODO: 나중에 랜덤으로 넣는 것이 필요!!
                }
                break;
            default:
                break;
        }
    }
    public static void NegativeEvent(Text name, Text description)
    {
        int tempInt;
        switch (Random.Range(0, 9)) //case 추가할때 Range 범위도 늘려주자.
        {
            case 0: 
                name.text = $"갑작스러운 소나기";
                description.text = "비를 피해 잠시 건물로 들어가 쉬던 주인공은 옆에 무기를 두고 쉬다가, 비가 그쳐 급히 챙겨나왔다\n\"어... 근데 무기 하나가 어디갔지?\""
                    + "\n\n" + $"[무기 1개 맨주먹으로 변경]";
                var weaponList = GameState.Instance.player.GetWeaponList();
                weaponList.RemoveAt(Random.Range(0, 10)); //장착무기 랜덤 제거
                weaponList.Add(JsonDB.GetWeapon($"bare_fist")); //맨주먹 추가
                var sortList =weaponList.OrderBy(x => x.id).ToList();
                GameState.Instance.player.SetWeaponList(sortList);
                break;
            case 1:
                name.text = $"사소한 불운";
                description.text = "신성한 기운이 빠져나가는 기분이 든다." + "\n\n" + $"[버프 해제]";
                GameState.Instance.player.DispelBuff();
                break;
            case 2:
                tempInt = 20;
                name.text = $"식중독";
                description.text = "상한 음식을 먹었다." + "\n\n" + $"[최대 체력 {tempInt} 감소]";
                GameState.Instance.player.baseStat.maxHp =System.Math.Max(1, GameState.Instance.player.baseStat.maxHp-tempInt);
                if (GameState.Instance.player.hp > GameState.Instance.player.GetStat().maxHp) GameState.Instance.player.hp = GameState.Instance.player.GetStat().maxHp;
                break;
            case 3:
                tempInt = 3;
                name.text = $"감기몸살";
                description.text = "몸이 으슬으슬 떨린다." + "\n\n" + $"[방어력 {tempInt} 감소]";
                GameState.Instance.player.baseStat.defense = System.Math.Max(0, GameState.Instance.player.baseStat.defense - tempInt);
                break;
            case 4:
                tempInt = 5;
                name.text = $"끈적끈적";
                description.text = "껌을 밟았다." + "\n\n" + $"[스피드 {tempInt} 감소]";
                GameState.Instance.player.baseStat.speed = System.Math.Max(0, GameState.Instance.player.baseStat.speed - tempInt);
                break;
            case 5:
                tempInt = 5;
                name.text = $"환경미화";
                description.text = "청소를 하다가 손을 다쳤다." + "\n\n" + $"[공격력 {tempInt} 감소]";
                GameState.Instance.player.baseStat.attack = System.Math.Max(0, GameState.Instance.player.baseStat.attack - tempInt);
                break;
            case 6: 
                name.text = $"소매치기";
                description.text = "\"내 아티펙트 돌려줘!\"" + "\n\n" + $"[아티펙트 손실]";
                tempInt = GameState.Instance.player.ArtifactsCount();
<<<<<<< HEAD
                GameState.Instance.player.ReMoveAtArtifact(Random.Range(0,tempInt));
=======
                GameState.Instance.player.RemoveAtArtifact(Random.Range(0,tempInt));
>>>>>>> main
                break;
            case 7:
                name.text = $"소매치기";
                description.text = "\"내 돈 돌려줘!\"" + "\n\n" + $"[코인 전액 감소]";
                GameState.Instance.player.money = 0;
                break;
            case 8:
                tempInt = 50;
                name.text = $"수상한 누군가";
                description.text = "\"믿어도 되는거죠...?\"" + "\n\n" + $"[-{tempInt} 코인]";
                GameState.Instance.player.money = System.Math.Max(0, GameState.Instance.player.money - tempInt);
                break;
            default:
                break;
        }
    }
}
