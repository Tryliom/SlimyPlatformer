using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _musicGameObject;
    [SerializeField] private GameObject _sfxGameObject;

    [Header("Music clips")] 
    [SerializeField] private AudioClip _musicCalm;
    [SerializeField] private AudioClip _musicSand;
    [SerializeField] private AudioClip _musicCastle;
    [SerializeField] private AudioClip _musicChallenge;
    [SerializeField] private AudioClip _musicHell;
    [SerializeField] private AudioClip _musicCredits;
    [SerializeField] private AudioClip _musicMenu;
    [SerializeField] private AudioClip _musicHub;
    
    [Header("SFX clips")]
    [SerializeField] private AudioClip _sfxJump;
    [SerializeField] private AudioClip _sfxDash;
    [SerializeField] private AudioClip _sfxDeath;
    [SerializeField] private AudioClip _sfxCheckpoint;
    [SerializeField] private AudioClip _sfxButton;
    [SerializeField] private AudioClip _sfxGlue;
    [SerializeField] private AudioClip _sfxDoorUnlocked;
    [SerializeField] private List<AudioClip> _sfxCoin;
    // Use sfx stars in order until the player go on ground
    [SerializeField] private List<AudioClip> _sfxStars;

    private AudioSource _musicAudioSource;
    private AudioSource _sfxAudioSource;
    
    private readonly List<string> _calmMusicLevels = new List<string>()
    {
        "1-1", "1-2", "1-3", "Ending", "True ending"
    };
    private readonly List<string> _sandMusicLevels = new List<string>()
    {
        "1-4"
    };
    private readonly List<string> _castleMusicLevels = new List<string>()
    {
        "1-5", "1-Secret"
    };
    private readonly List<string> _challengeMusicLevels = new List<string>()
    {
        "1-1a", "1-3a", "1-4a"
    };
    
    private int _currentStarSfxIndex = 0;
    private string _lastStarId = "";
    
    // Start is called before the first frame update
    private void Start()
    {
        _musicAudioSource = _musicGameObject.GetComponent<AudioSource>();
        _sfxAudioSource = _sfxGameObject.GetComponent<AudioSource>();
        
        _musicAudioSource.volume = 0.5f;
        _sfxAudioSource.volume = 1f;
        
        StartMusicForLevel();
    }

    private void StartMusicForLevel()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        
        if (_calmMusicLevels.Contains(sceneName))
        {
            _musicAudioSource.clip = _musicCalm;
        }
        else if (_sandMusicLevels.Contains(sceneName))
        {
            _musicAudioSource.clip = _musicSand;
        }
        else if (_castleMusicLevels.Contains(sceneName))
        {
            _musicAudioSource.clip = _musicCastle;
        }
        else if (_challengeMusicLevels.Contains(sceneName))
        {
            _musicAudioSource.clip = _musicChallenge;
        }
        else if (sceneName == "Hell")
        {
            _musicAudioSource.clip = _musicHell;
        }
        else if (sceneName == "Exit")
        {
            _musicAudioSource.clip = _musicCredits;
        }
        else if (sceneName == "MainMenu")
        {
            _musicAudioSource.clip = _musicMenu;
        }
        else if (sceneName == "Hub")
        {
            _musicAudioSource.clip = _musicHub;
        }
        
        _musicAudioSource.Play();
        _musicAudioSource.loop = true;
    }
    
    public void ResetStarSfxIndex()
    {
        _currentStarSfxIndex = 0;
        _lastStarId = "";
    }
    
    public void PlayStarSfx(string starId)
    {
        if (_lastStarId.Length != 0 && _lastStarId != starId)
        {
            _currentStarSfxIndex++;
        }
        
        if (_currentStarSfxIndex >= _sfxStars.Count)
        {
            _currentStarSfxIndex = 0;
        }
        
        if (_currentStarSfxIndex < _sfxStars.Count)
        {
            _sfxAudioSource.PlayOneShot(_sfxStars[_currentStarSfxIndex]);
        }
        
        _lastStarId = starId;
    }
    
    public void PlayJumpSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxJump);
    }
    
    public void PlayDashSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxDash);
    }
    
    public void PlayCoinSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxCoin[Random.Range(0, _sfxCoin.Count)]);
    }

    public void PlayDeathSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxDeath);
    }
    
    public void PlayCheckpointSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxCheckpoint);
    }
    
    public void PlayButtonSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxButton);
    }
    
    public void PlayGlueSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxGlue);
    }
    
    public void PlayDoorUnlockedSfx()
    {
        _sfxAudioSource.PlayOneShot(_sfxDoorUnlocked);
    }
}
