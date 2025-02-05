using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RGSK
{
    public class VehicleSelectionPaneloriginal : MonoBehaviour
    {
        private MenuVehicleInstantiator vehicleInstantiator;
        private IEnumerator moveBars;

        public Text vehicleName;

        [Header("Tech Specs")]
        public Image topSpeedBar;
        public Image accelerationBar;
        public Image handlingBar;
        public Image brakingBar;
        public Text vehicleMassText;
        public Text vehiclePowerText;
        public Text vehicleDescriptionText;

        public Button nextVehicle;
        public Button previousVehicle;
        public Button selectVehicle;
        public Button customizeVehicle;

        [Header("Back")]
        public Button backButton;
        public GameObject previousPanel;


        void Start()
        {
            vehicleInstantiator = FindObjectOfType<MenuVehicleInstantiator>();
            if(vehicleInstantiator == null)
                return;

            //Add button listeners
            if (nextVehicle != null)
            {
                nextVehicle.onClick.AddListener(delegate { vehicleInstantiator.CycleVehicles(1); });
                nextVehicle.onClick.AddListener(delegate { UpdateVehicleInformation(); });
            }

            if (previousVehicle != null)
            {
                previousVehicle.onClick.AddListener(delegate { vehicleInstantiator.CycleVehicles(-1); });
                previousVehicle.onClick.AddListener(delegate { UpdateVehicleInformation(); });
            }

            if(selectVehicle != null)
            {
                selectVehicle.onClick.AddListener(delegate { vehicleInstantiator.SetSelectedVehicle(); });
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(delegate { Back(); });
            }

            UpdateVehicleInformation();
        }


        public void UpdateVehicleInformation()
        {
            if (vehicleInstantiator == null)
                return;

            if (!vehicleInstantiator.HasVehicleDatabase())
                return;

            if(vehicleName != null)
            {
                vehicleName.text = vehicleInstantiator.GetVehicleData().vehicleName;
            }

            if(vehicleMassText != null)
            {
                //vehicleMassText.text = vehicleInstantiator.GetVehicleData().mass.ToString() + " Kg";
            }

            if (vehiclePowerText != null)
            {
                //vehiclePowerText.text = vehicleInstantiator.GetVehicleData().power.ToString() + "Hp";
            }

            if(vehicleDescriptionText != null)
            {
                //vehicleDescriptionText.text = vehicleInstantiator.GetVehicleData().vehicleDescription;
            }

            //Update the performance bars
            if (moveBars != null)
            {
                StopCoroutine(moveBars);
            }

            moveBars = MovePerformanceBars(
                vehicleInstantiator.GetVehicleData().topSpeed, 
                vehicleInstantiator.GetVehicleData().acceleration, 
                vehicleInstantiator.GetVehicleData().handling, 
                vehicleInstantiator.GetVehicleData().braking);

            StartCoroutine(moveBars);

            if(nextVehicle != null)
            {
                nextVehicle.gameObject.SetActive(!vehicleInstantiator.IsLastVehicleInList());
            }

            if (previousVehicle != null)
            {
                previousVehicle.gameObject.SetActive(!vehicleInstantiator.IsFirstVehicleInList());
            }
        }


        IEnumerator MovePerformanceBars(float speed, float accel, float handle, float brake)
        {
            float timer = 0;
            float lerpSpeed = 1;

            while (timer < lerpSpeed)
            {
                timer += Time.deltaTime;

                if(topSpeedBar != null)
                {
                    topSpeedBar.fillAmount = Mathf.Lerp(topSpeedBar.fillAmount, speed, timer / lerpSpeed);
                }

                if (accelerationBar != null)
                {
                    accelerationBar.fillAmount = Mathf.Lerp(accelerationBar.fillAmount, accel, timer / lerpSpeed);
                }

                if (handlingBar != null)
                {
                    handlingBar.fillAmount = Mathf.Lerp(handlingBar.fillAmount, handle, timer / lerpSpeed);
                }

                if (brakingBar != null)
                {
                    brakingBar.fillAmount = Mathf.Lerp(brakingBar.fillAmount, brake, timer / lerpSpeed);
                }

                yield return null;
            }
        }


		void RevertPlayerVehicle()
		{
			if (vehicleInstantiator == null)
				return;

            if (!vehicleInstantiator.HasVehicleDatabase())
                return;

            //Revert the player vehicle if the selected vehicle does not match the saved vehicle
            if (PlayerData.instance.playerData.vehicleID != vehicleInstantiator.GetVehicleData ().uniqueID)
				vehicleInstantiator.RevertPlayerVehicle ();
		}


        public void Back()
        {
			RevertPlayerVehicle ();

            if (previousPanel != null)
            {
                gameObject.SetActive(false);
                previousPanel.SetActive(true);
            }
        }


		void OnEnable()
		{
			UpdateVehicleInformation ();
		}
    }
}
