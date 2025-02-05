using UnityEngine;
using System.Collections.Generic;

namespace RGSK
{
    // Атрибут позволяет создавать объект данных чемпионата через меню в Unity Editor
    [CreateAssetMenu(fileName = "Новый чемпионат", menuName = "MRFE/New Championship Data", order = 1)]
    public class ChampionshipData : ScriptableObject
    {
        public string championshipName;                // Название чемпионата
        public Sprite championshipImage;               // Изображение чемпионата для UI
        public Sprite championshipIcon;                // Иконка чемпионата для отображения в списках
        public List<ChampionshipRound> championshipRounds = new List<ChampionshipRound>(); // Список раундов чемпионата
        public List<ChampionshipRacer> championshipRacers = new List<ChampionshipRacer>(); // Список участников чемпионата
        public int[] championshipPoints;               // Массив очков для чемпионата
    }
}
