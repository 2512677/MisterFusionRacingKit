using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Events;

namespace RGSK
{
    public class PlayerDataoriginal : MonoBehaviour
    {
        public static PlayerData instance;
        public static UnityAction OnPlayerLevelUp;

        [Header("Default Values")]
        public DataContainer defaultValues = new DataContainer();

        [Header("Player Data")]
        public DataContainer playerData = new DataContainer();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            //Load data from binary file
            LoadData();

            //Dont destroy this object as it should exist throughout the game
            DontDestroyOnLoad(gameObject);
        }


        public void AddPlayerCurrecny(float amount)
        {
            //Add to the player's currency
            playerData.playerCurrency += amount;

            //Save
            SaveData();
        }


        public void AddPlayerXP(float points)
        {
            //Add the points to the total xp
            playerData.totalPlayerXP += points;

            //Add the points to the current level's XP
            playerData.currentLevelXP += points;

            //Level up if the current level XP reaches the next level XP
            if (playerData.currentLevelXP >= playerData.nextLevelXP)
            {
                AddPlayerXPLevel();
            }

            //Save
            SaveData();
        }


        void AddPlayerXPLevel()
        {
            //Add the players XP level
            playerData.playerXPLevel++;

            //Reset the current level XP
            playerData.currentLevelXP = 0;

            //Multiply the next level XP by the 'nextLevelMultiplier' value
            playerData.nextLevelXP *= playerData.nextLevelXpMultiplier;
        }



        public void UnlockItem(string id)
        {
            //Check if the item is unlocked first
            if (IsItemUnlocked(id))
                return;

            //Unlock the item and save
            playerData.items.Add(id);
            SaveData();
        }


        public void SavePlayerName(string newName)
        {
            playerData.playerName = newName;
            SaveData();
        }


        public void SavePlayerNationality(Nationality newNationality)
        {
            playerData.playerNationality = newNationality;
            SaveData();
        }


        public void SaveSelectedVehicle(string id)
        {
            playerData.vehicleID = id;
            SaveData();
        }


        public bool IsItemUnlocked(string id)
        {
            return playerData.items.Contains(id);
        }


        void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savefile.dat");
            DataContainer data = new DataContainer();

            data.playerName = playerData.playerName;
            data.playerNationality = playerData.playerNationality;

            data.playerCurrency = playerData.playerCurrency;
            data.playerXPLevel = playerData.playerXPLevel;
            data.totalPlayerXP = playerData.totalPlayerXP;
            data.currentLevelXP = playerData.currentLevelXP;
            data.nextLevelXP = playerData.nextLevelXP;
            data.items = playerData.items;
            data.vehicleID = playerData.vehicleID;

            bf.Serialize(file, data);
            file.Close();
        }


        void LoadData()
        {
            if (!File.Exists(Application.persistentDataPath + "/savefile.dat"))
            {
                //File doesnt exist so create a new one
                ResetData();
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savefile.dat", FileMode.Open);

            DataContainer data = (DataContainer)bf.Deserialize(file);
            file.Close();

            playerData.playerName = data.playerName;
            playerData.playerNationality = data.playerNationality;

            playerData.playerCurrency = data.playerCurrency;
            playerData.playerXPLevel = data.playerXPLevel;
            playerData.totalPlayerXP = data.totalPlayerXP;
            playerData.currentLevelXP = data.currentLevelXP;
            playerData.nextLevelXP = data.nextLevelXP;
            playerData.items = data.items;
            playerData.vehicleID = data.vehicleID;
        }


        public void ResetData()
        {
            playerData.playerName = defaultValues.playerName;
            playerData.playerNationality = Nationality.Other;
            playerData.playerCurrency = defaultValues.playerCurrency;
            playerData.totalPlayerXP = defaultValues.totalPlayerXP;
            playerData.playerXPLevel = defaultValues.playerXPLevel;
            playerData.currentLevelXP = defaultValues.currentLevelXP;
            playerData.nextLevelXP = defaultValues.nextLevelXP;
            playerData.nextLevelXpMultiplier = defaultValues.nextLevelXpMultiplier;

            playerData.items = new List<string>();
            for (int i = 0; i < defaultValues.items.Count; i++)
            {
                UnlockItem(defaultValues.items[i]);
            }

            playerData.vehicleID = defaultValues.vehicleID;

            SaveData();

            Debug.Log("Player data has been reset!");
        }


        public void DeleteSaveFile()
        {
            if(File.Exists(Application.persistentDataPath + "/savefile.dat"))
            {
                File.Delete(Application.persistentDataPath + "/savefile.dat");
            }
        }
    }


    [Serializable]
    public class DataContainer
    {
        public string playerName;
        public Nationality playerNationality;

        public float playerCurrency;
        public float totalPlayerXP;
        public int playerXPLevel;
        public float currentLevelXP;
        public float nextLevelXP;
        public float nextLevelXpMultiplier;
        public List<string> items;
        public string vehicleID;
    }
}