using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// public enum SoundActionType
// {
//     PLAY = 0,
//     PAUSE,
//     STOP
// }

public enum SoundType
{
    SOUND_FX = 0,
    SOUND_MUSIC
}

public enum SoundFXID
{
    NONE = 0,
    SOUNDFX_Arrow_Fly,
    SOUNDFX_Bow_Loading,
    SOUNDFX_Bow_Fail,
    SOUNDFX_Boss_Die,
    SOUNDFX_Boss_Hurt,
    SOUNDFX_Button_Click,
    SOUNDFX_Buy_Item,
    SOUNDFX_CoinPickup,
    SOUNDFX_Fail,
    SOUNDFX_Foot_Step,
    SOUNDFX_Foot_Step_Shoe,
    SOUNDFX_Jump,
    SOUNDFX_Pickup_Normal,
    SOUNDFX_Player_Hurt,
    SOUNDFX_Enemy_Hurt,
    SOUNDFX_Sword_Attack,
    SOUNDFX_Sword_Metal_Hit,
    SOUNDFX_Sword_Wood_Hit,
    SOUNDFX_Lose,
    SOUNDFX_Viruss_Shot,
    SOUNDFX_Attack_Cage,
    SOUNDFX_Cage_Broken,
    SOUNDFX_Rat_Hurt,
    SOUNDFX_Virus_Hurt,
    SOUNDFX_Power,
}

public enum SoundMusicID
{
    NONE = 0,
    BGM_Main,
    SOUND_BATTLE_MUSIC,
    SOUND_VICTORY_MUSIC,
    SOUND_LOSE_MUSIC
}

[CreateAssetMenu(menuName = "config/SoundConfig")]
public class SoundDataScriptableObject : ScriptableObject
{


    [Serializable]
    public class SoundFXDataItem
    {
        public SoundFXID SoundFXID;
        public bool IsMultipleSound = false;

        public AudioClip AudioClip;

        public List<AudioClip> AudioClips;
        public bool IsSequenceSound = false;
    }

    [Serializable]
    public struct SoundMusicDataItem
    {
        public SoundMusicID SoundMusicID;
        public AudioClip AudioClip;
    }

    public List<SoundFXDataItem> SoundFXItems;
    public List<SoundMusicDataItem> SoundMusicItems;

    public void Init()
    {
        _soundFXSequenceTime.Clear();
    }

    public class SequenceSoundData
    {
        public float LastTime;
        public int CurrentSequenceIndex;
    }

    private Dictionary<SoundFXID, SequenceSoundData> _soundFXSequenceTime = new Dictionary<SoundFXID, SequenceSoundData>();


    public AudioClip GetSoundFXAudioClip(SoundFXID soundId)
    {
        var _soundDataItem = SoundFXItems.FindLast(e => e.SoundFXID == soundId);


        if (_soundDataItem.IsMultipleSound == false)
        {
            if (_soundDataItem.AudioClip == null)
            {
                Debug.LogError("SoundFXID: " + soundId + " not found in SoundDataScriptableObject");
                return null;
            }

            return _soundDataItem.AudioClip;
        }
        else
        {
            if (_soundDataItem.IsSequenceSound == false) //random
            {
                return _soundDataItem.AudioClips[UnityEngine.Random.Range(0, _soundDataItem.AudioClips.Count)];
            }
            else
            {
                if (_soundFXSequenceTime.ContainsKey(soundId) == false)
                {
                    SequenceSoundData sequenceSoundData = new SequenceSoundData();
                    sequenceSoundData.LastTime = Time.time;
                    sequenceSoundData.CurrentSequenceIndex = 0;

                    _soundFXSequenceTime.Add(soundId, sequenceSoundData);
                    return _soundDataItem.AudioClips[0];
                }
                else
                {
                    var _sequenceSoundData = _soundFXSequenceTime[soundId];
                    float deltaTime = Time.time - _sequenceSoundData.LastTime;
                    if (deltaTime <= 1f)
                    {
                        int nextIndex = _sequenceSoundData.CurrentSequenceIndex + 1;
                        if (nextIndex >= _soundDataItem.AudioClips.Count - 1)
                        {
                            nextIndex = _soundDataItem.AudioClips.Count - 1;
                        }

                        _sequenceSoundData.CurrentSequenceIndex = nextIndex;
                        _sequenceSoundData.LastTime = Time.time;

                        // _soundFXSequenceTime[soundId] = _sequenceSoundData;
                        return _soundDataItem.AudioClips[nextIndex];
                    }
                    else
                    {
                        _sequenceSoundData.LastTime = Time.time;
                        _sequenceSoundData.CurrentSequenceIndex = 0;
                        return _soundDataItem.AudioClips[0];
                    }
                }
            }
        }
    }

    public AudioClip GetSoundMusicAudioClip(SoundMusicID soundId)
    {
        var _soundDataItem = SoundMusicItems.FindLast(e => e.SoundMusicID == soundId);
        if (_soundDataItem.AudioClip == null)
        {
            Debug.LogError("SoundMusicID: " + soundId + " not found in SoundDataScriptableObject");
            return null;
        }

        return _soundDataItem.AudioClip;
    }
}