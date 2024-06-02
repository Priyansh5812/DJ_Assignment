using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class CatapultSpawner : MonoBehaviour
{
    [SerializeField] private CatapultView m_catapultPrefab;
	[SerializeField] private StaticHandGesture grabGesture;
    [SerializeField] private ProjectileProperties projectileProperties;
    [SerializeField] private Projectile projectilePrefab;
    void Start()
    {
        SpawnCatapult();
    }
	
	private void SpawnCatapult()
	{
		CatapultModel model = new CatapultModel(projectileProperties , this.transform, grabGesture , projectilePrefab);
		CatapultController controller = new CatapultController(model , m_catapultPrefab);
	}

}

#region Catapult Attributes
  [System.Serializable]

public struct ProjectileProperties
{
    [Header("Projectile")]
    public float initialSpeed;
    public float mass, drag;

    [Header("Trajectory")]
    public int maxPoints;
    public float rayOverlap;

    public ProjectileProperties(Vector3 dir, float speed, Vector3 pos, float mass, float drag, int maxPoints, float rayOverlap)
    {
        this.initialSpeed = speed;
        this.mass = mass;
        this.drag = drag;
        this.maxPoints = maxPoints;
        this.rayOverlap = rayOverlap;
    }
}


#endregion
