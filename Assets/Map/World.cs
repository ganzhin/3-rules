using Assets.Gameplay.Rules;
using Assets.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _lakePrefab;
    [SerializeField] private GameObject _grassPrefab;

    [SerializeField] private int _treeAmount;
    [SerializeField] private int _lakeAmount;
    [SerializeField] private int _grassAmount;

    [SerializeField] private GameObject[] _spawners;

    [SerializeField] private int _worldRadius;

    private void Start()
    {
        RespawnTrees();
    }

    public void RespawnTrees()
    {
        for (int i = 0; i < FindObjectsOfType<WorldElement>().Length; i++)
        {
            Destroy(FindObjectsOfType<WorldElement>()[i].gameObject);
        }

        for (int i = 0; i <= _treeAmount; i++)
        {
            Instantiate(_treePrefab, Random.insideUnitCircle * _worldRadius, Quaternion.identity);
        }

        for (int i = 0; i <= _lakeAmount; i++)
        {
            Instantiate(_lakePrefab, Random.insideUnitCircle * _worldRadius, Quaternion.identity);
        }

        for (int i = 0; i <= _grassAmount; i++)
        {
            Instantiate(_grassPrefab, Random.insideUnitCircle * _worldRadius, Quaternion.identity);
        }

        for (int i = 0; i <= 1 + Levels.Level; i++)
        {
            Instantiate(_spawners[Random.Range(0, _spawners.Length)], Random.insideUnitCircle * _worldRadius, Quaternion.identity);
        }
    }
}
