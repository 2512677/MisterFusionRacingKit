using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RGSK
{
    public class MenuChampionshipPanel : MonoBehaviour
    {

        public ChampionshipData[] championships;
        private int championshipIndex;
		public RoundInformation[] roundInformation;

        public Text championshipName;
        public Image championshipImage;
        public Image championshipIcon;
        public Button nextChampionshipButton;
        public Button previousChampionshipButton;
        public Button startChampionship;

        [Header("Back")]
        public Button backButton;
        public GameObject previousPanel;

        void Start()
        {
            //Add button listeners
            if (nextChampionshipButton != null)
            {
                nextChampionshipButton.onClick.AddListener(delegate { AddChampionship(1); });
            }

            if (previousChampionshipButton != null)
            {
                previousChampionshipButton.onClick.AddListener(delegate { AddChampionship(-1); });
            }

            if (startChampionship != null)
            {
                startChampionship.onClick.AddListener(delegate { StartChampionship(); });
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(delegate { Back(); });
            }

            ClearRoundInformation();
            AddChampionship(0);
        }


        public void AddChampionship(int direction)
        {
            // Move to the index in the direction of "direction"
            championshipIndex += direction;
            championshipIndex = Mathf.Clamp(championshipIndex, 0, championships.Length - 1);

            // Fill in the name
            if (championshipName != null)
            {
                championshipName.text = championships[championshipIndex].championshipName;
            }

            // Fill in the image
            if (championshipImage != null && championships[championshipIndex].championshipImage != null)
            {
                championshipImage.sprite = championships[championshipIndex].championshipImage;
            }

            // Fill in the Icon
            if (championshipIcon != null && championships[championshipIndex].championshipIcon != null)
            {
                championshipIcon.sprite = championships[championshipIndex].championshipIcon;
            }

            // Clear round information and information on the current championship index
            ClearRoundInformation();
            for (int i = 0; i < championships[championshipIndex].championshipRounds.Count; i++)
            {
                if (i > roundInformation.Length - 1)
                    break;

                if (roundInformation[i].roundNumber != null)
                {
                    roundInformation[i].roundNumber.text = "Round " + (i + 1);
                }

                if (roundInformation[i].trackName != null)
                {
                    roundInformation[i].trackName.text = championships[championshipIndex].championshipRounds[i].trackData.trackName;
                }

                if (roundInformation[i].raceType != null)
                {
                    // Use the GetDescription method to display the description in Russian
                    roundInformation[i].raceType.text = championships[championshipIndex].championshipRounds[i].raceType.GetDescription();
                }
            }

            // Disable the next button when we are on the last index
            if (nextChampionshipButton != null)
            {
                nextChampionshipButton.gameObject.SetActive(championshipIndex < championships.Length - 1);
            }

            // Disable the previous button when we are on the first index
            if (previousChampionshipButton != null)
            {
                previousChampionshipButton.gameObject.SetActive(championshipIndex > 0);
            }
        }


        public void StartChampionship()
        {
            //Create the championship
            GameObject go = new GameObject("Championship");
            ChampionshipManager newChampionship = go.AddComponent<ChampionshipManager>();
            newChampionship.championshipData = championships[championshipIndex];

            if(SceneController.instance != null)
            {
				SceneController.instance.LoadScene(newChampionship.CurrentRound().trackData.scene);
            }
			else 
			{
				Debug.LogWarning ("Cannont load the scene beacuse a SceneLoader was not found. Please add a SceneController to this scene.");
			}
        }


        public void Back()
        {
            if (previousPanel != null)
            {
                gameObject.SetActive(false);
                previousPanel.SetActive(true);
            }
        }


        void ClearRoundInformation()
        {
            for (int i = 0; i < roundInformation.Length; i++)
            {
                if (roundInformation[i].roundNumber != null)
                {
                    roundInformation[i].roundNumber.text = string.Empty;
                }

                if (roundInformation[i].trackName != null)
                {
                    roundInformation[i].trackName.text = string.Empty;
                }

                if (roundInformation[i].raceType != null)
                {
                    roundInformation[i].raceType.text = string.Empty;
                }
            }
        }


        [System.Serializable]
        public class RoundInformation
        {
            public Text roundNumber;
            public Text trackName;
            public Text raceType;
        }
    }
}