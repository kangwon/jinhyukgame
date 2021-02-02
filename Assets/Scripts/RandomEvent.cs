using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class RandomEvent
{
    RandomPanelController randomPanelController =GameObject.Find("Canvas").transform.Find("RandomPanel").gameObject.GetComponent<RandomPanelController>();
    public void PositiveEvent(Text name,Text description,RandomCard randomCard)
    {
        int tempInt;
        switch (CustomRandom<int>.Choice(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, GameState.Instance.World.Random)) //case 추가할때 범위도 늘려주자.
        {

            case 0:
                tempInt = 5;
                name.text = $"모리스 셰프의 격렬한 음식";
                description.text = "요리대회 우승자 모리스의 특제요리! 먹기만했을 뿐인데... 화가납니다!!" + "\n\n" + $"[공격력 {tempInt} 증가]";
                GameState.Instance.player.baseStat.attack += tempInt;
                break;
            case 1:
                tempInt = 20;
                name.text = $"모리스 셰프의 든든한 음식";
                description.text = "요리대회 우승자 모리스의 특제요리! 먹기만하면 체력이 쑥쑥!" + "\n\n"+$"[최대체력 {tempInt} 증가]";
                GameState.Instance.player.baseStat.maxHp += tempInt;
                break;
            case 2:
                tempInt = 3;
                name.text = $"모리스 셰프의 강인한 음식";
                description.text = "요리대회 우승자 모리스의 특제요리! 먹기만하면 몸이 단단해진다구!" + "\n\n" + $"[방어력 {tempInt} 증가]";
                GameState.Instance.player.baseStat.defense += tempInt;
                break;
            case 3:
                tempInt = 5;
                name.text = $"모리스 셰프의 민첩한 음식";
                description.text = "요리대회 우승자 모리스의 특제요리! 먹기만하면 발이 빨라져요!" + "\n\n" + $"[스피드 {tempInt} 증가]";
                GameState.Instance.player.baseStat.speed += tempInt;
                break;
            case 4:
                tempInt = 50;
                name.text = $"운수 좋은 날";
                description.text = "햇볕은 쨍쩅, 코인한닢은 반짝! 오늘은 왠지 운수가 좋다!" + "\n\n" + $"[+{tempInt} 코인 ]";
                GameState.Instance.player.money += tempInt;
                break;
            case 5:
                name.text = $"휴식공간";
                description.text = "\"들어와서 쉬는건 마음대로지만, 나가는것도 마음대로란다.\"" + "\n\n" + $"[모든 체력 회복]";
                GameState.Instance.player.Heal(GameState.Instance.player.baseStat.maxHp);
                break;
            case 6: //TODO : 아직 무적이 구현안됬음. 구현하고 내용 집어넣자.
                name.text = $"네잎클로버";
                description.text = "무심코 내려다본 발아래에 네잎클로버가 있었다. \"기분 좋은 하루가 될것 같은걸?\"" + "\n\n" + $"[다음 전투에서 무적(보스제외)]";
                break;
            case 7:
                name.text = $"성스러운 분수";
                description.text = "넓은 광장 가운데에 있던 분수에서 튄 물방울이 시원하고도 아름답다" + "\n\n" + $"[디버프 제거]";
                GameState.Instance.player.Dispel();
                break; 
            case 8:
                name.text = $"세계수 카오마이의 은총";
                description.text = "세계수의 따뜻한 마음이 느껴진다" + "\n\n" + $"[버프 선택 획득]";
                foreach (var buff in JsonDB.GetBuffs())
                {
                    randomPanelController.CreateButton(buff);
                }
                break;
            case 9:
                name.text = $"카발라의 축복(등급)";
                description.text = "카발라의 축복" + "\n\n" + $"[선택장비 등급 상승]";
                CreateButtonEquipments(true);
                break;
            case 10:
                name.text = $"카발라의 축복(수식어)";
                description.text = "카발라의 축복" + "\n\n" + $"[선택장비 수식어 상승]";
                CreateButtonEquipments(false);
                break;
            default:
                break;
        }
    }
    public void NeuturalityEvent(Text name, Text description, RandomCard randomCard)
    {
        int tempInt;
        switch (CustomRandom<int>.Choice(new List<int> { 0, 1, 2, 3 }, GameState.Instance.World.Random)) //case 추가할때 범위도 늘려주자.
        {
            case 0: //TODO : 4가지의 확률에 맞춰 구현하기(티켓 획득)
                name.text = $"풍선 다트";
                description.text = "얍!" + "\n\n";
                switch (CustomRandom<int>.WeightedChoice(new List<int> { 0, 1, 2, 3, 4 }, new List<double> { 0.2f, 0.08f, 0.3f, 0.02f, 0.4f }, GameState.Instance.World.Random))
                {
                    case 0:
                        description.text += $"[장비 획득]";
                        if (randomCard.equipment.type == "weapon")
                        {
                            GameState.Instance.player.SetEquipment(randomCard.equipment);
                            if (10 < GameState.Instance.player.GetWeaponList().Count)
                                GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject.Find("Canvas").transform.Find("EquipmentChangePanel").gameObject.GetComponent<EquipmentChangePanelController>().DisplayPanel(randomCard.equipment, 
                                (e)=>{ 
                                    GameState.Instance.player.SetEquipment(e);
                                });
                        }
                        break;
                    case 1:
                        description.text += $"[아티펙트 획득]";
                        GameState.Instance.player.SetEquipment(randomCard.artifact); 
                        if (3 < GameState.Instance.player.ArtifactsCount())
                            GameObject.Find("Canvas").transform.Find("ArtifactChangePanel").gameObject.SetActive(true);
                            break;
                    case 2:
                        description.text += $"[{randomCard.money}코인 획득]";
                        GameState.Instance.player.money += randomCard.money;
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
                name.text = $"카발라의 잔혹한 장난";
                description.text = "\"마술 하난 보여줄께! 그거 이리 내!\"" + "\n\n" + $"[아티펙트 강제 랜덤변경]";
                tempInt = GameState.Instance.player.ArtifactsCount();
                if (tempInt != 0)
                    GameState.Instance.player.ChangeAtArtifact(Random.Range(0, tempInt), randomCard.artifact);
                break;
            case 2:
                name.text = $"카발라의 소소한 장난";
                description.text = "\"마술 하나 보여줄까? 물건 하나만 줘볼래?\"" + "\n\n" + $"[아티펙트 선택 랜덤변경]";
                var index = 0;
                foreach(var artifact in GameState.Instance.player.GetArtifacts())
                {
                    randomPanelController.CreateButton(artifact,index);
                    index++;
                }
                break;
            case 3:
                name.text = $"인형 뽑기";
                description.text = $"\"단 한번의 기회! 실력을 보여주지!\""+$"{randomCard.money}코인 필요";
                tempInt = randomCard.money;
                randomPanelController.CreateButtonGamble(tempInt, tempInt, CustomRandom<bool>.Choice(new List<bool> { false, true }, GameState.Instance.World.Random));
                break;
            default:
                break;
        }
    }
    public void NegativeEvent(Text name, Text description, RandomCard randomCard)
    {
        int tempInt;
        switch (CustomRandom<int>.Choice(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10, 11, 12}, GameState.Instance.World.Random)) //case 추가할때 범위도 늘려주자.
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
                description.text = "\"앗! 길가의 네잎클로버를 밟아버렸다! 괜히 찝찝한걸..\"" + "\n\n" + $"[버프 해제]";
                GameState.Instance.player.DispelBuff();
                break;
            case 2:
                tempInt = 20;
                name.text = $"식중독";
                description.text = "식은땀이 난다.. \"아까 먹은 음식이 문제였을까..\"" + "\n\n" + $"[최대 체력 {tempInt} 감소]";
                GameState.Instance.player.baseStat.maxHp =System.Math.Max(1, GameState.Instance.player.baseStat.maxHp-tempInt);
                if (GameState.Instance.player.hp > GameState.Instance.player.GetStat().maxHp) GameState.Instance.player.hp = GameState.Instance.player.GetStat().maxHp;
                break;
            case 3:
                tempInt = 3;
                name.text = $"감기몸살";
                description.text = "몸이 으슬으슬 떨린다.... \"엣취!\"" + "\n\n" + $"[방어력 {tempInt} 감소]";
                GameState.Instance.player.baseStat.defense = System.Math.Max(0, GameState.Instance.player.baseStat.defense - tempInt);
                break;
            case 4:
                tempInt = 5;
                name.text = $"끈적끈적";
                description.text = "\"에잇! 이게뭐야! 껌을 밟았잖아?!\"" + "\n\n" + $"[스피드 {tempInt} 감소]";
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
                GameState.Instance.player.RemoveAtArtifact(Random.Range(0,tempInt));
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
            case 9:
                name.text = $"가시덤불";
                description.text = "\"가시덤불 \"" + "\n\n" + $"[체력 손실]";
                tempInt=(int)(GameState.Instance.player.GetStat().maxHp * 0.05);
                GameState.Instance.player.hp = System.Math.Max(0, GameState.Instance.player.hp - tempInt);
                if (GameState.Instance.player.hp == 0)
                    randomPanelController.ShowGameOver();
                break;
            case 10:
                name.text = $"함정!";
                description.text = "\"함정 \"" + "\n\n" + $"[체력 손실]";
                tempInt = System.Math.Max(1, (int)(GameState.Instance.player.hp * 0.15));
                GameState.Instance.player.hp = System.Math.Max(0, GameState.Instance.player.hp - tempInt);
                if (GameState.Instance.player.hp == 0)
                    randomPanelController.ShowGameOver();
                break;
            case 11:
                name.text = $"정밀 감정";
                description.text = "이 장비는 그렇게 좋은 아이템은 아니네요..." + "\n\n" + $"[장비 등급 하락]";
                Dictionary<int,Equipment> rankDownEquipments = new Dictionary<int, Equipment>{ };
                var rankIndex = 0;
                //장비 등급 하락이 가능한 무기,장비들을 dictionary에 모은다.
                foreach (var weapon in GameState.Instance.player.GetWeaponList()) 
                {
                    if (!((weapon.rank == Rank.none) || (weapon.rank == Rank.common)))
                        rankDownEquipments.Add(rankIndex, weapon);
                    rankIndex++;
                }
                List<Equipment> tempEquipments =new List<Equipment> { GameState.Instance.player.GetHelmet(), GameState.Instance.player.GetArmor(), GameState.Instance.player.GetShoes() };
                foreach (var tempEquipment in tempEquipments )
                {
                   if(!((tempEquipment.rank == Rank.none) || (tempEquipment.rank == Rank.common)))
                        rankDownEquipments.Add(rankIndex, tempEquipment);
                    rankIndex++;
                }
                if (rankDownEquipments.Count != 0)
                {
                    tempInt = CustomRandom<int>.Choice(new List<int>(rankDownEquipments.Keys), GameState.Instance.World.Random); //dictionary에 있는 장비를 랜덤으로 하나 고른다
                    if (tempInt < 10) //무기일 때
                    {
                        var weapons = GameState.Instance.player.GetWeaponList();
                        weapons.RemoveAt(tempInt);
                        weapons.Add(GetDownEquipments(rankDownEquipments[tempInt].ToWeapon((WeaponType)int.Parse(rankDownEquipments[tempInt].id.Substring(7, 1))), true));
                        GameState.Instance.player.SetWeaponList(weapons.OrderBy(x => x.id).ToList());
                    }
                    else
                    {
                        GameState.Instance.player.SetEquipment(GetDownEquipments(rankDownEquipments[tempInt],true));
                    }
                    description.text += $"\n{ rankDownEquipments[tempInt].name}의 등급 하락";
                }
                break;
              case 12:
                name.text = $"첨벙!";
                description.text = "물 웅덩이를 미처 피하지 못했다." + "\n\n" + $"[장비 수식어 하락]";
                Dictionary<int, Equipment> prefixDownEquipments = new Dictionary<int, Equipment> { };
                var prefixIndex = 0;
                //장비 수식어 하락이 가능한 무기,장비들을 dictionary에 모은다.
                foreach (var weapon in GameState.Instance.player.GetWeaponList())
                {
                    if (!((weapon.prefix == Prefix.none) || (weapon.prefix == Prefix.broken)))
                        prefixDownEquipments.Add(prefixIndex, weapon);
                    prefixIndex++;
                }
                List<Equipment> tempEquipments2 = new List<Equipment> { GameState.Instance.player.GetHelmet(), GameState.Instance.player.GetArmor(), GameState.Instance.player.GetShoes() };
                foreach (var tempEquipment in tempEquipments2)
                {
                    if (!((tempEquipment.prefix == Prefix.none) || (tempEquipment.prefix == Prefix.broken)))
                        prefixDownEquipments.Add(prefixIndex, tempEquipment);
                    prefixIndex++;
                }
                if (prefixDownEquipments.Count != 0)
                {
                    tempInt = CustomRandom<int>.Choice(new List<int>(prefixDownEquipments.Keys), GameState.Instance.World.Random); //dictionary에 있는 장비를 랜덤으로 하나 고른다
                    if (tempInt < 10) //무기일 때
                    {
                        var weapons = GameState.Instance.player.GetWeaponList();
                        weapons.RemoveAt(tempInt);
                        weapons.Add(GetDownEquipments(prefixDownEquipments[tempInt].ToWeapon((WeaponType)int.Parse(prefixDownEquipments[tempInt].id.Substring(7, 1))), false));
                        GameState.Instance.player.SetWeaponList(weapons.OrderBy(x => x.id).ToList());
                    }
                    else
                    {
                        GameState.Instance.player.SetEquipment(GetDownEquipments(prefixDownEquipments[tempInt], false));
                    }
                    description.text += $"\n{prefixDownEquipments[tempInt].name}의 수식어 하락";
                }
                break;
            default:
                break;
        }
    }
    void CreateButtonEquipments(bool isRank) //랭크올리는거면 true, 수식어면 false
    {
        var index = 0;
        foreach (var weapon in GameState.Instance.player.GetWeaponList())
        {           
            randomPanelController.CreateButton(weapon,isRank,index);
            index++;
        }
        randomPanelController.CreateButton(GameState.Instance.player.GetHelmet(),isRank);
        randomPanelController.CreateButton(GameState.Instance.player.GetArmor(), isRank);
        randomPanelController.CreateButton(GameState.Instance.player.GetShoes(), isRank);
    }
    Equipment GetDownEquipments(Equipment nowEquipment,bool isRank)
    {
        var tempRank = (int)nowEquipment.rank;
        var tempPrefix = (int)nowEquipment.prefix;
        if (isRank) tempRank--;
        else tempPrefix--;
        switch (nowEquipment)
        {
            case Helmet h:
                return JsonDB.GetEquipment($"{h.id.Substring(0, 8)}{tempRank}{tempPrefix}");
            case Armor a:
                return JsonDB.GetEquipment($"{a.id.Substring(0, 7)}{tempRank}{tempPrefix}");
            case Shoes s:
                return JsonDB.GetEquipment($"{s.id.Substring(0, 7)}{tempRank}{tempPrefix}");
            default:
                throw new System.NotImplementedException($"Invalid equipment type: {nowEquipment.GetType().ToString()}");
        }
    }
    Weapon GetDownEquipments(Weapon nowWeapon, bool isRank)
    {
        var tempRank = (int)nowWeapon.rank;
        var tempPrefix = (int)nowWeapon.prefix;
        if (isRank) tempRank--;
        else tempPrefix--;
        return JsonDB.GetWeapon($"{nowWeapon.id.Substring(0, 8)}{tempRank}{tempPrefix}");
    }
}
