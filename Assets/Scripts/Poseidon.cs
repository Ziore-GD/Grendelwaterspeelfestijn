using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Poseidon : MonoBehaviour {
    private static Poseidon poseidon;
    public static Poseidon Instance {
        get {
            if (poseidon == null) poseidon = FindObjectOfType<Poseidon> ();
            return poseidon;
        }
    }

    private float _currentWater = 100;
    private float _maxWater = 100;
    private Animator _anim;
    [SerializeField] private Slider _waterSlider;
    private bool[] sendmsg = new bool[3];
    void Awake () {
        _anim = GetComponent<Animator> ();
        _waterSlider.maxValue = _maxWater;
        _waterSlider.value = _currentWater;
    }

    public void DeltaWater (float delta) {
        _currentWater -= delta;
        _waterSlider.value = _currentWater;
        if (_currentWater <= _maxWater / 1.25f && !sendmsg[0]) {
            Announcer.Instance.Log ("Be carefull you are wasting water!");
            sendmsg[0] = true;
        }

        if (_currentWater <= _maxWater / 2 && !sendmsg[1]) {
            Announcer.Instance.Log ("Be more sustainable!");
            sendmsg[1] = true;
        }

        if (_currentWater <= 0) {
            OutOfWater ();
        }
    }

    private void OutOfWater () {
        if (!sendmsg[2]) {
            Announcer.Instance.Log ("You lost all my refreshements!");
            sendmsg[2] = true;
        }
    }
    public void Heal () {
        _currentWater = _maxWater;
        _waterSlider.value = _currentWater;
    }

    void OnTriggerEnter (Collider other) {
        Enemy e = other.GetComponent<Enemy> ();
        if (e != null) {
            DeltaWater (10);
            e.DeltaHealth (500);
        }
    }
}