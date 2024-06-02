using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class CatapultSpawner : MonoBehaviour
{
    [SerializeField] private CatapultView m_catapultPrefab;
	[SerializeField] private StaticHandGesture grabGesture;
    [SerializeField] private ProjectileProperties projectileProperties;
    void Start()
    {
        SpawnCatapult();
    }
	
	private void SpawnCatapult()
	{
		CatapultModel model = new CatapultModel(projectileProperties , this.transform, grabGesture);
		CatapultController controller = new CatapultController(model , m_catapultPrefab);
	}

}

#region Catapult Attributes
  [System.Serializable]

public struct ProjectileProperties
{
    public Vector3 direction;
    public float initialSpeed;
    public Vector3 initialPos;
    public float mass, drag;

    public ProjectileProperties(Vector3 dir, float speed, Vector3 pos, float mass, float drag)
    {
        this.direction = dir;
        this.initialSpeed = speed;
        this.initialPos = pos;
        this.mass = mass;
        this.drag = drag;
    }
}
#endregion
