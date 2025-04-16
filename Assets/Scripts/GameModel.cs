using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public class GameModel
{
    private static string _gameSaveFilename = "/vuduongthanh-save.json";
    private GameData _gameData;
    public GameData GameData => _gameData;
    public static GameModel Load(GameConfig config)
    {
        var gameModel = new GameModel();
        gameModel.Prepare(config);
        return gameModel;
    }
    void Prepare(GameConfig config)
    {
        LoadGameData(config);
    }
    private void SaveGame()
    {
        string gameSavePath = GetGameSavePath();
        string content = Newtonsoft.Json.JsonConvert.SerializeObject(_gameData);

        File.WriteAllText(gameSavePath, content);

        Debug.Log($"SaveGame to {gameSavePath}");
    }
    private GameData LoadGameData(GameConfig gameConfig)
    {
        try
        {
            if (_gameData == null)
            {
                string gameSavePath = GetGameSavePath();

                if (File.Exists(gameSavePath))
                {
#if UNITY_EDITOR
                    //make a copy of the save file for reproducibility for testing purposes
                    string copyFile = $"{Application.persistentDataPath}/vuduongthanh-save_copy.json";
                    File.Copy(gameSavePath, copyFile, true);

#endif
                    Debug.Log($"Loading game save : {gameSavePath}");

                    var bytes = File.ReadAllBytes(gameSavePath);
                    string text = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    try
                    {
                        _gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(text);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($">>> Parse GameSave error: {e.Message}");
                    }
                }
                else
                {
                    Debug.Log("Game save not found, starting a new game!");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load saved game due to: {ex}");
        }

        if (_gameData != null)
        {

        }

        //Init New Data
        if (_gameData == null /*|| AlwaysStartNewGame*/)
        {
            _gameData = new GameData();

            if (gameConfig != null)
            {
                _gameData.Coin = gameConfig.CoinDefault;
                _gameData.Arrow = gameConfig.ArrowDefault;
                _gameData.Health = gameConfig.HealthDefault;
                _gameData.Stamina = gameConfig.HealthDefault;
                _gameData.AngryEnergy = gameConfig.AngryEnergyDefault;
            }
        }

        SaveGame();
        return _gameData;
    }
    public static string GetGameSavePath()
    {
        return $"{Application.persistentDataPath}{_gameSaveFilename}";
    }

    public float CurrentHealth => _gameData.Health;
    public void IncreaseHealth(float value){
        _gameData.Health+=value;
        if(_gameData.Health>GameConfig.Load().HealthDefault){
            _gameData.Health = GameConfig.Load().HealthDefault;
        }
        SaveGame();
        PlayerHealthBar.Instance.OnHealthChange(_gameData.Health);
        if(_gameData.Health==GameConfig.Load().HealthDefault||_gameData.ItemHealth==0){
            MainHud.Instance.SetActiveUseItemHealthButton(false);
        }
    }
    public void DecreaseHealth(float value){
        _gameData.Health-=value;
        if(_gameData.Health<0){
            _gameData.Health = 0;
        }
        SaveGame();
        PlayerHealthBar.Instance.OnHealthChange(_gameData.Stamina);
        if(_gameData.Health<GameConfig.Load().HealthDefault&&_gameData.ItemHealth>0){
            MainHud.Instance.SetActiveUseItemHealthButton(true);
        }
    }
    public void SetHealth(float value){
        _gameData.Health = value;
        SaveGame();
        PlayerHealthBar.Instance.OnHealthChange(_gameData.Health);
        if(_gameData.Health==GameConfig.Load().HealthDefault||_gameData.ItemHealth==0){
            MainHud.Instance.SetActiveUseItemHealthButton(false);
        }
        if(_gameData.Health<GameConfig.Load().HealthDefault&&_gameData.ItemHealth>0){
            MainHud.Instance.SetActiveUseItemHealthButton(true);
        }
    }
    public float CurrentArmor => _gameData.Armor;
    public float CurrentStamina => _gameData.Stamina;
    public void SetStamina(float value){
        _gameData.Stamina = value;
        SaveGame();
        PlayerStaminaBar.Instance.OnStaminaChange(_gameData.Stamina);
        if(_gameData.Stamina==GameConfig.Load().StatminaDefault||_gameData.ItemStamina==0){
            MainHud.Instance.SetActiveUseStaminaItemButton(false);
        }
        if(_gameData.Stamina<GameConfig.Load().StatminaDefault&&_gameData.ItemStamina>0){
            MainHud.Instance.SetActiveUseStaminaItemButton(true);
        }
    }
    public void IncreaseStamina(float value){
        _gameData.Stamina+=value;
        if(_gameData.Stamina>GameConfig.Load().StatminaDefault){
            _gameData.Stamina = GameConfig.Load().StatminaDefault;
        }
        SaveGame();
        PlayerStaminaBar.Instance.OnStaminaChange(_gameData.Stamina);
        if(_gameData.Stamina==GameConfig.Load().StatminaDefault||_gameData.ItemStamina==0){
            MainHud.Instance.SetActiveUseStaminaItemButton(false);
        }
    }
    public void DecreaseStamina(float value){
        _gameData.Stamina-=value;
        if(_gameData.Stamina<0){
            _gameData.Stamina = 0;
        }
        SaveGame();
        PlayerStaminaBar.Instance.OnStaminaChange(_gameData.Stamina);
        if(_gameData.Stamina<GameConfig.Load().StatminaDefault&&_gameData.ItemStamina>0){
            MainHud.Instance.SetActiveUseStaminaItemButton(true);
        }
    }
    public float CurrentAngryEnergy => _gameData.AngryEnergy;
    public void IncreaseAngryEnergy(float value){
        _gameData.AngryEnergy+=value;
        if(_gameData.AngryEnergy>GameConfig.Load().MaxAngryEnergy){
            _gameData.AngryEnergy = GameConfig.Load().MaxAngryEnergy;
        }
        if(_gameData.AngryEnergy==GameConfig.Load().MaxAngryEnergy){
            MainHud.Instance.SetActiveButtonSkill(true);
        }
        Debug.Log("IncreaseAngryEnergy: value = "+value+" current = "+_gameData.AngryEnergy);
        SaveGame();
        MainHud.Instance.OnAngryEnergyChange();
    }
    public void SetAngryEnergy(float value){
        _gameData.AngryEnergy = value;
        SaveGame();
        MainHud.Instance.OnAngryEnergyChange();
    }
    public bool IsHaveBow => _gameData.IsHaveBow;
    public void UnlockBow(){
        _gameData.IsHaveBow = true;
        SaveGame();
        MainHud.Instance.OnUnlockBow();
    }
    public int CurrentItemHealth => GameData.ItemHealth;
    public void IncreaseItemHealth(int amount){
        GameData.ItemHealth+=amount;
        if(GameData.ItemHealth>GameConfig.Load().MaxCountItemHealth){
            GameData.ItemHealth = GameConfig.Load().MaxCountItemHealth;
        }
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
        if(_gameData.Health<GameConfig.Load().HealthDefault&&_gameData.ItemHealth>0){
            MainHud.Instance.SetActiveUseItemHealthButton(true);
        }
    }
    public bool TryIncreaseItemHealth(int amount){
        if(GameData.ItemHealth+amount>GameConfig.Load().MaxCountItemHealth){
            return false;
        }
        GameData.ItemHealth+=amount;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
        if(_gameData.Health<GameConfig.Load().HealthDefault&&_gameData.ItemHealth>0){
            MainHud.Instance.SetActiveUseItemHealthButton(true);
        }
        return true;
    }
    public void DecreaseItemHealth(int amount){
        if(GameData.ItemHealth==0){
            return;
        }
        GameData.ItemHealth-=amount;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
    }
    public int CurrentItemStamina => GameData.ItemStamina;
    public bool TryIncreaseItemStamina(int amount){
        if(GameData.ItemStamina+amount>GameConfig.Load().MaxCountItemStamina){
            return false;
        }
        GameData.ItemStamina+=amount;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
        if(_gameData.Stamina<GameConfig.Load().StatminaDefault&&_gameData.ItemStamina>0){
            MainHud.Instance.SetActiveUseStaminaItemButton(true);
        }
        return true;
    }
    public void DecreaseItemStamina(int amount){
        if(GameData.ItemStamina==0){
            return;
        }
        GameData.ItemStamina-=amount;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
    }
    public float CurrentCoin => _gameData.Coin;
    public void IncreaseCoin(int value){
        _gameData.Coin+=value;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
    }
    public void DecreaseCoin(int value){
        _gameData.Coin-=value;
        if(_gameData.Coin<0){
            _gameData.Coin = 0;
        }
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
    }
    public bool TryDecreaseCoin(int value){
        if(_gameData.Coin<value){
            return false;
        }
        _gameData.Coin-=value;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
        return true;
    }
    public float CurrentArrow => _gameData.Arrow;
    public bool TryIncreaseArrow(int amount){
        if(GameData.Arrow+amount>GameConfig.Load().MaxArrow){
            return false;
        }
        _gameData.Arrow+=amount;
        SaveGame();
        MainHud.Instance.UpdateResourceDisplay();
        return true;
    }
    public void DecreaseArrow(int amount){
        _gameData.Arrow-=amount;
        MainHud.Instance.UpdateResourceDisplay();
        SaveGame();
    }
    public int CurrentEquipLevel => GameData.EquipLevel;
    public void SetEquipLevel(int equipLevel){
        GameData.EquipLevel=equipLevel;
        SaveGame();
    }
}
