using RGSK;
using UnityEngine;
using UnityEngine.UI;

public class RaceRewards : MonoBehaviour
{
    public static RaceRewards Instance;

    // Текущий массив наград (для 1-го, 2-го, 3-го места)
    public Rewards[] currentRewards;

    [System.Serializable]
    public class Rewards
    {
        public int currency;
        public int xp;
        public int speedBoost;
        public string[] items;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (CareerData.pendingRewards != null && CareerData.pendingRewards.Length > 0)
        {
            currentRewards = CareerData.pendingRewards;
        }
        else
        {
            currentRewards = null;
        }

        // УБРАТЬ или закомментировать строчку "CareerData.pendingRewards = null;"
    }


    public Rewards[] GetRewards()
    {
        return currentRewards;
    }

    public void SetRewards(Rewards[] rewards)
    {
        currentRewards = rewards;
    }
}
