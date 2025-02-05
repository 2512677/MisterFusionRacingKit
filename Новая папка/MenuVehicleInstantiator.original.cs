using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RGSK
{
    public class MenuVehicleInstantiator.original : MonoBehaviour
    {
        public VehicleDatabase vehicleDatabase { get { return GlobalSettings.Instance.vehicleDatabase; } }

        [HideInInspector]
        public List<MenuVehicle> menuVehicles;
        private int vehicleIndex;

        void Start()
        {
            InstantiateVehicles();
            LoadPlayerVehicle();
        }


        void InstantiateVehicles()
        {
            if (vehicleDatabase == null)
            {
                Debug.Log("A Vehicle Database has not been assigned");
                return;
            }

            //Loop through all the vehicles and instantiate them
            for (int i = 0; i < vehicleDatabase.vehicles.Length; i++)
            {
                GameObject vehicle = (GameObject)Instantiate(vehicleDatabase.vehicles[i].menuVehicle, transform.position, transform.rotation, transform);
                menuVehicles.Add(new MenuVehicle(vehicle, vehicleDatabase.vehicles[i]));

                //Deactivate instantiated vehicles
                vehicle.SetActive(false);
            }
        }


        void LoadPlayerVehicle()
        {
            if (PlayerData.instance == null)
            {
                //If no data found, just enable the first vehicle in the list
                if (menuVehicles.Count > 0)
                {
                    menuVehicles[0].vehicle.SetActive(true);
                }

                return;
            }

            //If saved vehicle string is empty, enable the fist vehicle and return
            if (PlayerData.instance.playerData.vehicleID == string.Empty && menuVehicles.Count > 0)
            {
                menuVehicles[0].vehicle.SetActive(true);
                PlayerData.instance.playerData.vehicleID = menuVehicles[0].vehicleData.uniqueID;
                return;
            }

            for (int i = 0; i < menuVehicles.Count; i++)
            {
                //Load vehicle colors
                if (menuVehicles[i].vehicleData.bodyMaterial != null)
                {
                    menuVehicles[i].vehicleData.bodyMaterial.color =
                        LoadVehicleColor(menuVehicles[i].vehicleData.uniqueID, menuVehicles[i].vehicleData.bodyMaterial.color);
                }

                //Select the player's vehicle
                if (menuVehicles[i].vehicleData.uniqueID == PlayerData.instance.playerData.vehicleID)
                {                  
                    vehicleIndex = i;
                    menuVehicles[i].vehicle.SetActive(true);
                }
            }
        }


        public void CycleVehicles(int direction)
        {
            //Deactivate the current vehicle
            menuVehicles[vehicleIndex].vehicle.SetActive(false);

            //Set the index to the next/prev vehicle
            vehicleIndex += direction;
            vehicleIndex = Mathf.Clamp(vehicleIndex, 0, menuVehicles.Count - 1);

            //Activate the newly selected vehicle
            menuVehicles[vehicleIndex].vehicle.SetActive(true);
        }


        public void SetSelectedVehicle()
        {
            if (PlayerData.instance != null)
            {
                //Save the selected vehicle
                PlayerData.instance.SaveSelectedVehicle(GetVehicleData().uniqueID);
            }
        }


        public void SetSelectedVehicleColor(Color color)
        {
            if(GetVehicleData().bodyMaterial != null)
            {
                GetVehicleData().bodyMaterial.color = color;
                SaveSelectedVehicleColor(color);
            }
        }


        public void SaveSelectedVehicleColor(Color color)
        {
            PlayerPrefs.SetFloat(GetVehicleData().uniqueID + "_colorR", color.r);
            PlayerPrefs.SetFloat(GetVehicleData().uniqueID + "_colorG", color.g);
            PlayerPrefs.SetFloat(GetVehicleData().uniqueID + "_colorB", color.b);
        }


        public Color LoadVehicleColor(string id, Color defaultColor)
        {
            float r = PlayerPrefs.GetFloat(id + "_colorR", defaultColor.r);
            float g = PlayerPrefs.GetFloat(id + "_colorG", defaultColor.g);
            float b = PlayerPrefs.GetFloat(id + "_colorB", defaultColor.b);

            return new Color(r, g, b);
        }


		public void RevertPlayerVehicle()
		{
			for (int i = 0; i < menuVehicles.Count; i++) 
			{
				if (menuVehicles [i].vehicleData.uniqueID == PlayerData.instance.playerData.vehicleID) 
				{                  
					vehicleIndex = i;
					menuVehicles [i].vehicle.SetActive (true);
				} 
				else 
				{
					menuVehicles[i].vehicle.SetActive(false);
				}
			}
		}


        public VehicleDatabase.VehicleData GetVehicleData()
        {
            if (menuVehicles.Count == 0)
                return null;

            return menuVehicles[vehicleIndex].vehicleData;
        }


        public bool IsLastVehicleInList()
        {
            return vehicleIndex == menuVehicles.Count - 1;
        }


        public bool IsFirstVehicleInList()
        {
            return vehicleIndex == 0;
        }


        public bool HasVehicleDatabase()
        {
            return vehicleDatabase != null;
        }


        [System.Serializable]
        public class MenuVehicle
        {
            public GameObject vehicle;
            public VehicleDatabase.VehicleData vehicleData;

            public MenuVehicle(GameObject _vehicle, VehicleDatabase.VehicleData _vehicleData)
            {
                vehicle = _vehicle;
                vehicleData = _vehicleData;
            }
        }
    }
}