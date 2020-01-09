using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private static UIManager _manager;
    private int visnovcounter = 0;
    public static UIManager Instance {
        get {
            if (_manager == null) _manager = FindObjectOfType<UIManager> ();
            return _manager;
        }
    }

    [SerializeField] private Button _nextWaveButton, _poseidonBtn, _upgradeRangeBtn, _upgradeSpeedBtn, _upgradeEffectivenessBtn, _nextButton;
    [SerializeField] private GameObject _inGamePanel, _buildingPanel, _upgradePanel, _visnovPanel;
    [SerializeField] private Image _poseidonImage;
    [SerializeField] private Text _moneyText;
    [SerializeField] private ParticleSystem _earthquakeFX;
    private int _money = 100;
    private Turret _selectedTurret;

    void Awake () {
        _nextWaveButton.onClick.AddListener (() => StartWave ());
        _poseidonBtn.onClick.AddListener (() => PoseidonAtk ());
        _upgradeRangeBtn.onClick.AddListener (() => TryUpgrade (0));
        _upgradeSpeedBtn.onClick.AddListener (() => TryUpgrade (1));
        _upgradeEffectivenessBtn.onClick.AddListener (() => TryUpgrade (2));
        _nextButton.onClick.AddListener (() => Visnov());
        _inGamePanel.SetActive (false);
        _upgradePanel.SetActive (false);
        _visnovPanel.SetActive (false);
        _moneyText.text = "Poseidon Coins : " + _money;
    }

    private void StartWave () {
        Spawnscript.Instance.NextWave ();
        ToggleUI ();
    }

    public void ToggleUI () {
        _upgradePanel.SetActive (!_buildingPanel.activeSelf);
        _buildingPanel.SetActive (!_buildingPanel.activeSelf);
        _inGamePanel.SetActive (!_inGamePanel.activeSelf);
    }
    public void OpenUpgradeWindow (Turret turret) {
        _selectedTurret = turret;
        _upgradePanel.SetActive (true);
        _upgradeRangeBtn.GetComponentInChildren<Text> ().text = "+ Range : " + _selectedTurret.GetStats (0).UpgradePrice;
        _upgradeSpeedBtn.GetComponentInChildren<Text> ().text = "+ Speed : " + _selectedTurret.GetStats (1).UpgradePrice;
        _upgradeEffectivenessBtn.GetComponentInChildren<Text> ().text = "+ Effectiveness : " + _selectedTurret.GetStats (2).UpgradePrice;
    }

    public void FinishedWave () {
        _visnovPanel.SetActive (true);
        _inGamePanel.SetActive(false);

        if (visnovcounter < 4) {
            switch (visnovcounter) {
                case 0:
                    _visnovPanel.GetComponentInChildren<Text> ().text = "Thank you for saving my water!";
                    _poseidonImage.sprite = Resources.Load<Sprite>("Sprites/happy pos");
                    break;
                case 1:
                    _visnovPanel.GetComponentInChildren<Text> ().text = "It makes me angry when people waste water...";
                    _poseidonImage.sprite = Resources.Load<Sprite>("Sprites/angry pos");
                    break;
                case 2:
                    _visnovPanel.GetComponentInChildren<Text> ().text = "By the way, did you know peeing in the shower saves water?";
                    _poseidonImage.sprite = Resources.Load<Sprite>("Sprites/happy pos");
                    break;
                case 3:
                    _visnovPanel.GetComponentInChildren<Text> ().text = "Anyway, here's some money to upgrade your stuff!";
                    _poseidonImage.sprite = Resources.Load<Sprite>("Sprites/giving pos");
                    break;
                default:
                    break;
            }
        } 
        else {
            visnovcounter = 0;
            _visnovPanel.SetActive (false);
            _inGamePanel.SetActive (true);            

            ToggleUI ();
            _money += 100;
            _moneyText.text = "Poseidon Coins : " + _money;
        }
    }
    private void TryUpgrade (int i) {
        TurretStats stats = _selectedTurret.GetStats (i);
        if (stats.UpgradePrice <= _money) {
            _money -= stats.UpgradePrice;
            stats.Upgrade ();
            _moneyText.text = "Poseidon Coins : " + _money;
            OpenUpgradeWindow (_selectedTurret);
        }
    }
    private void PoseidonAtk () {
        Spawnscript.Instance.StunAll ();
        _earthquakeFX.Play ();
        Camera.main.GetComponent<CameraShake> ().DoShake ();
        Invoke ("Unstun", 3);
    }
    private void Unstun () {
        Spawnscript.Instance.UnStunAll ();
        _earthquakeFX.Stop ();
    }

    private void Visnov () {

        visnovcounter++;
        FinishedWave ();
        
    }
}