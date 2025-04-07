using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public void SetHealth(float value){
        _gameData.Health = value;
        SaveGame();
    }
    public float CurrentArmor => _gameData.Armor;
    public float CurrentStamina => _gameData.Stamina;
}
