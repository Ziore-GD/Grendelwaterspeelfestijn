using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class TurretStats {
    [SerializeField] private float _value;
    [SerializeField] private int _upgradePrice = 25;
    [SerializeField] private float _upgradeValue = 1;

    public float Value {
        get {
            return _value;
        }
    }
    public int UpgradePrice {
        get {
            return _upgradePrice;
        }
    }
    public void Upgrade () {
        _value += _upgradeValue;
        _upgradePrice += 25;
    }
}
public class Turret : MonoBehaviour {
    [SerializeField] private TurretStats _aggroRange;
    [SerializeField] private TurretStats _suctionSpeed;
    [SerializeField] private TurretStats _suctionEffectiveness;
    [SerializeField] private bool _activated;
    [SerializeField] private GameObject _attackPrefab;
    private ParticleSystem _attackEffect;
    private List<Enemy> Enemies = new List<Enemy> ();
    private float cd = 0;
    [SerializeField] private Enemy _target = null;
    private Button _button;
    private SphereCollider _collider;

    void Awake () {
        _attackEffect = Instantiate (_attackPrefab, transform.position + new Vector3 (0, 1.5f, .5f), Quaternion.identity, transform).GetComponent<ParticleSystem> ();
        _collider = GetComponent<SphereCollider> ();
        _button = transform.GetComponentInChildren<Button> ();
        _button.onClick.AddListener (() => UpgradeToggle ());
        SetRange ();
    }

    private Enemy GetEnemy () {
        Enemy e = null;
        if (Enemies.Count > 0) {
            for (int i = 0; i < Enemies.Count; i++) {
                if (!Enemies[i].IsDead) {
                    if (Vector3.Distance (transform.position, Enemies[i].transform.position) <= _aggroRange.Value)
                        return Enemies[i];
                } else {
                    Enemies.RemoveAt (i);
                }
            }
        }
        return e;
    }

    void Update () {
        if (_activated) {
            if (cd > _suctionSpeed.Value) {
                if (_target != null && !_target.IsDead) {
                    transform.LookAt (_target.transform, Vector3.up);
                    _attackEffect.Play ();
                    _target.DeltaHealth (_suctionEffectiveness.Value);
                    cd = 0;
                } else {
                    _target = GetEnemy ();
                    _attackEffect.Stop ();
                }
            } else {
                cd += Time.deltaTime;
            }
        }
    }

    private void SetRange () {
        _collider.radius = _aggroRange.Value;
    }

    void OnTriggerEnter (Collider other) {
        Enemy e = other.GetComponent<Enemy> ();
        if (e != null) {
            Enemies.Add (e);
        }
    }
    void OnTriggerExit (Collider other) {
        Enemy e = other.GetComponent<Enemy> ();
        if (e != null) {
            Enemies.Remove (e);
        }
    }
    void UpgradeToggle () {
        UIManager.Instance.OpenUpgradeWindow(this);
    }
    public TurretStats GetStats (int i) {
        TurretStats stats = new TurretStats();
        if (i == 0) {
            stats = _aggroRange;
        } else if (i == 1) {
            stats = _suctionSpeed;
        } else if (i == 2) {
            stats = _suctionEffectiveness;
        }
        return stats;
    }
}