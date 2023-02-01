using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Weights")]
    [SerializeField, Range(0, 100)]
    private float _healthUpgradeWeight = 10;
    [SerializeField, Range(0, 100)]
    private float _damageUpgradeWeight = 10;
    [SerializeField, Range(0, 100)]
    private float _moveSpeedUpgradeWeight = 5;
    [SerializeField, Range(0, 100)]
    private float _healWeight = 25;
    [SerializeField, Range(0, 100)]
    private float _weaponChangeWeight = 2;
    [SerializeField, Range(0, 100)]
    private float _rifleWeight = 25;
    [SerializeField, Range(0, 100)]
    private float _automaticRifleWeight = 15;
    [SerializeField, Range(0, 100)]
    private float _shotgunWeight = 20;

    [Space, Header("Prefabs")]
    [SerializeField]
    private PowerUp _healthPrefab;
    [SerializeField]
    private PowerUp _damagePrefab;
    [SerializeField]
    private PowerUp _moveSpeedPrefab;
    [SerializeField]
    private HealthPack _healPrefab;
    [SerializeField]
    private WeaponPowerUp _riflePrefab;
    [SerializeField]
    private WeaponPowerUp _automaticRifleWPrefab;
    [SerializeField]
    private WeaponPowerUp _shotgunPrefab;

    private PlayerWeapon.WeaponType _currentWeapon;

    private const float FieldSideLength = 12.0f;

    private void Awake()
    {
        EventBus.Sub(MobKilledHandler, EventBus.MOB_KILLED);
        Player.Instance.OnWeaponChange += PlayerWeaponChangeHandler;
    }

    private void OnDestroy()
    {
        EventBus.Unsub(MobKilledHandler, EventBus.MOB_KILLED);
        Player.Instance.OnWeaponChange -= PlayerWeaponChangeHandler;
    }

    private void MobKilledHandler()
    {
        Spawn(PickRandomPosition());
    }

    private void PlayerWeaponChangeHandler(object _, PlayerWeapon.WeaponType type)
    {
        _currentWeapon = type;
    }

    private Vector3 PickRandomPosition()
    {
        var position = new Vector3
        {
            x = FieldSideLength * (Random.value - 0.5f),
            z = FieldSideLength * (Random.value - 0.5f)
        };

        return position;
    }

    private void Spawn(Vector3 position)
    {
        GameObject pickedPrefab;
        do pickedPrefab = WeightIndexToPrefab(GetRandomIndexRespectWeights());
        while (PrefabIsWeapon(pickedPrefab, _currentWeapon));

        Instantiate(pickedPrefab, position, Quaternion.identity);
    }

    private int GetRandomIndexRespectWeights()
    {
        var allWeights = new[]
        {
            _healthUpgradeWeight,
            _damageUpgradeWeight,
            _moveSpeedUpgradeWeight,
            _healWeight,
            _weaponChangeWeight,
            _rifleWeight,
            _automaticRifleWeight,
            _shotgunWeight
        };

        var ranges = new List<float>(new float[allWeights.Length]);

        var sum = allWeights.Sum();
        ranges[0] = allWeights[0] / sum;
        for (var i = 1; i < ranges.Count; i++)
        {
            ranges[i] = ranges[i - 1] + allWeights[i] / sum;
        }

        var random = Random.value;
        var weightIndex = -1;

        for (var i = 0; i < ranges.Count; i++)
        {
            if (random > ranges[i]) continue;

            weightIndex = i;
            break;
        }

        return weightIndex;
    }

    private GameObject WeightIndexToPrefab(int weightIndex)
    {
        return weightIndex switch
        {
            0 => _healthPrefab.gameObject,
            1 => _damagePrefab.gameObject,
            2 => _moveSpeedPrefab.gameObject,
            3 => _healPrefab.gameObject,
            // Not sure what this for
            4 => GetRandomWeapon(),
            5 => GetWeaponPrefab(PlayerWeapon.WeaponType.Rifle),
            6 => GetWeaponPrefab(PlayerWeapon.WeaponType.AutomaticRifle),
            7 => GetWeaponPrefab(PlayerWeapon.WeaponType.Shotgun),
            _ => throw new ArgumentException($"Cannot map {weightIndex} weight index!")
        };
    }

    private GameObject GetWeaponPrefab(PlayerWeapon.WeaponType type)
    {
        return type switch
        {
            PlayerWeapon.WeaponType.Rifle => _riflePrefab.gameObject,
            PlayerWeapon.WeaponType.Shotgun => _automaticRifleWPrefab.gameObject,
            PlayerWeapon.WeaponType.AutomaticRifle => _shotgunPrefab.gameObject,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private bool PrefabIsWeapon(GameObject prefab, PlayerWeapon.WeaponType type)
    {
        return type switch
        {
            PlayerWeapon.WeaponType.Rifle => prefab.Equals(_riflePrefab.gameObject),
            PlayerWeapon.WeaponType.Shotgun => prefab.Equals(_shotgunPrefab.gameObject),
            PlayerWeapon.WeaponType.AutomaticRifle => prefab.Equals(_automaticRifleWPrefab.gameObject),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private GameObject GetRandomWeapon()
    {
        return GetWeaponPrefab((PlayerWeapon.WeaponType)Random.Range(0, 3));
    }
}