using UnityEngine;
using UnityEngine.UI;


namespace RGSK
{
    public class RaceRewardsPanel : MonoBehaviour
    {
        [Header("UI Text fields for showing rewards")]
        public Text moneyText;
        public Text xpText;
        public Text speedBoostText; // Новое поле для отображения spb
        public Text itemsText; // Новое поле для отображения списка предметов

        private void OnEnable()
        {
            if (RaceManager.instance != null && RaceManager.instance.playerStatistics != null)
            {
                int playerPosition = RaceManager.instance.playerStatistics.Position;
                UpdateRewardsUI(playerPosition);
            }
            else
            {
                Debug.LogError("RaceRewardsPanel: Невозможно определить позицию игрока! RaceManager или playerStatistics имеет значение null.");
            }
        }

        public void UpdateRewardsUI(int playerPosition)
        {
            if (RaceRewards.Instance == null)
            {
                Debug.LogError("RaceRewardsPanel: RaceRewards.Instance is null! Негде брать массив наград.");
                moneyText.text = "0";
                xpText.text = "0";
                speedBoostText.text = "0";
                itemsText.text = "";
                return;
            }

            var rewardsArray = RaceRewards.Instance.currentRewards;
            if (rewardsArray == null || rewardsArray.Length == 0)
            {
                Debug.LogError("RaceRewardsPanel: currentRewards не задан или пуст.");
                moneyText.text = "0";
                xpText.text = "0";
                speedBoostText.text = "0";
                itemsText.text = "";
                return;
            }

            if (playerPosition < 1 || playerPosition > rewardsArray.Length)
            {
                Debug.LogWarning($"RaceRewardsPanel: playerPosition {playerPosition} выходит за границы наград.");
                moneyText.text = "0";
                xpText.text = "0";
                speedBoostText.text = "0";
                itemsText.text = "";
                return;
            }

            var reward = rewardsArray[playerPosition - 1];

            // Обновляем UI
            moneyText.text = reward.currency.ToString();
            xpText.text = reward.xp.ToString();
            speedBoostText.text = reward.speedBoost.ToString(); // Показываем spb

            // Формируем строку для списка предметов
            if (reward.items != null && reward.items.Length > 0)
            {
                itemsText.text = string.Join(", ", reward.items); // Объединяем предметы через запятую
            }
            else
            {
                itemsText.text = "";
            }

            // Сохраняем награды в PlayerData
            PlayerData.instance.AddCurrency(reward.currency);
            PlayerData.instance.AddXP(reward.xp);
            PlayerData.instance.playerData.speedBoost += reward.speedBoost;

            if (reward.items != null && reward.items.Length > 0)
            {
                foreach (var item in reward.items)
                {
                    PlayerData.instance.AddItem(item); // Метод для добавления предметов
                }
            }

            PlayerData.instance.SaveData();

            Debug.Log($"Игрок получил speedBoost: {reward.speedBoost}, предметы: {itemsText.text}, деньги: {reward.currency}, опыт: {reward.xp}");
        }
    }
}
