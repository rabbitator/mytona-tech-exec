using UnityEngine;

//Bad class
public class PowerupSpawner : MonoBehaviour
{
	[Range(0, 100)] public float HealthUpgradeWeight = 10;
	[Range(0, 100)] public float DamageUpgradeWeight = 10;
	[Range(0, 100)] public float MoveSpeedUpgradeWeight = 5;
	[Range(0, 100)] public float HealWeight = 25;
	[Range(0, 100)] public float WeaponChangeWeight = 2;
	[Range(0, 100)] public float RifleWeight = 25;
	[Range(0, 100)] public float AutomaticRifleWeight = 15;
	[Range(0, 100)] public float ShotgunWeight = 20;

	public PowerUp HealthPrefab;
	public PowerUp DamagePrefab;
	public PowerUp MoveSpeedPrefab;
	public HealthPack HealPrefab;
	public WeaponPowerUp RiflePrefab;
	public WeaponPowerUp AutomaticRifleWPrefab;
	public WeaponPowerUp ShotgunPrefab;

	private float[] weights;
	private float[] weaponWeights;
    
	private GameObject[] prefabs;
	private WeaponPowerUp[] weaponPrefabs;
    
	private void Awake()
	{
		weights = new float[5];
		weights[0] = HealthUpgradeWeight;
		weights[1] = weights[0] + DamageUpgradeWeight;
		weights[2] = weights[1] + MoveSpeedUpgradeWeight;
		weights[3] = weights[2] + HealWeight;
		weights[4] = weights[3] + WeaponChangeWeight;
        
		weaponWeights = new float[3];
		weaponWeights[0] = RifleWeight;
		weaponWeights[1] = weaponWeights[0] + AutomaticRifleWeight;
		weaponWeights[2] = weaponWeights[1] + ShotgunWeight;

		prefabs = new[]
		{
			HealthPrefab.gameObject, 
			DamagePrefab.gameObject, 
			MoveSpeedPrefab.gameObject, 
			HealPrefab.gameObject
		};
		weaponPrefabs = new[]
		{
			RiflePrefab,
			AutomaticRifleWPrefab,
			ShotgunPrefab
		};
        
		EventBus.Sub(Handle, EventBus.MOB_KILLED);
	}

	private void Handle()
	{
		Spawn(PickRandomPosition());
	}
	
	private Vector3 PickRandomPosition()
	{
		var vector3 = new Vector3();
		vector3.x = Random.value * 11 - 6;
		vector3.z = Random.value * 11 - 6;
		return vector3;
	}


	private void Spawn(Vector3 position)
	{
		var rand = Random.value * weights[4];
		int i = 0;
		while (i< 5 && weights[i] >= rand)
		{
			i++;
		}

		if (i < 4)
		{
			Instantiate(prefabs[Mathf.Min(3,i)], position, Quaternion.identity);
		}
		else
		{
			rand = Random.value * weaponWeights[2];
			i = 0;
			while (i < 3 && weaponWeights[Mathf.Min(2,i)] >= rand )
			{
				i++;
			}
			Instantiate(weaponPrefabs[Mathf.Min(2,i)], position, Quaternion.identity);
		}
	}
}