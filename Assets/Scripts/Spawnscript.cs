using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnscript : MonoBehaviour {
    private Wave[] waves = new Wave[0];
    private List<Enemy> enemies = new List<Enemy> ();
    private GameObject enemyObject;
    float xis = 2;
    [SerializeField] private int currentWave = 1;
    [SerializeField] private float spawnCD = 1;
    [SerializeField] float upperLimit, lowerLimit;
    private static Spawnscript _spawner;
    public static Spawnscript Instance {
        get {
            if (_spawner == null) _spawner = FindObjectOfType<Spawnscript> ();
            return _spawner;
        }
    }

    private Wave GetWave {
        get {
            return waves[currentWave - 1];
        }
    }
    public void NextWave () {
        currentWave++;
        StartCoroutine (Spawning ());
    }

    void Start () {
        waves = Resources.LoadAll<Wave> ("Wave");
        enemyObject = Resources.Load<GameObject> ("Prefabs/Enemy");
        xis = transform.position.x;
    }

    IEnumerator Spawning () {
        int i = 0;
        while (i < GetWave.SpawnAmount) {
            GetEnemy ();
            i++;
            yield return new WaitForSeconds (GetWave.SpawnCD);
        }
        StartCoroutine (EndWave ());
    }

    IEnumerator EndWave () {
        bool finished = false;
        while (!finished) {
            bool nope = false;
            foreach (Enemy e in enemies) {
                if (!e.IsDead) {
                    nope = true;
                }
            }
            if (!nope) {
                finished = true;
            }
            yield return new WaitForSeconds (1f);
        }
        //UIManager.Instance.ToggleUI ();
        UIManager.Instance.FinishedWave ();
        Poseidon.Instance.Heal ();
    }

    Enemy GetEnemy () {
        Enemy e = null;
        foreach (Enemy b in enemies) {
            if (b.IsDead) {
                e = b;
            }
        }
        if (e == null) {
            e = Instantiate (GetWave.Enemies[Random.Range (0, GetWave.Enemies.Length)].gameObject).GetComponent<Enemy> ();
            enemies.Add (e);
        }
        Vector3 spawnPos = new Vector3 (xis, Random.Range (2, 5), transform.position.z + Random.Range (lowerLimit, upperLimit));
        e.transform.position = spawnPos;
        e.SetHealth (currentWave * 2);
        e.enabled = true;
        e.Show ();
        return e;
    }
    public void StunAll () {
        foreach (Enemy e in enemies) {
            e.enabled = false;
            e.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    public void UnStunAll () {
        foreach (Enemy e in enemies) {
            e.enabled = true;
        }
    }
}