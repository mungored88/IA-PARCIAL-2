using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NpcController : MonoBehaviour
{
	private AStar pathFinding;
	private PathNode[] AstarPath;
	public PathNode[] AstarPathGetter { get { return AstarPath; } }

	private int currentIndex = 0;

	[Header("Avoidance")]
	public LayerMask LayerFriends;
	public LayerMask LayerObst;
	private float avoidWeight = 50;
	private float separationWeight = 10;
	private float targetWeight = 10;
	private float radObst = 0.6f;
	private float radFlock = 5.0f;

	public float speed = 5.0f;
	[HideInInspector]
	public float rotationSpeed = 5.0f;

	private Vector3 direction;
	private Vector3 avoidance;
	private Vector3 targetVector;
	private Vector3 separationVector;

	private Collider closerObstacle;

	[HideInInspector]
	public List<Collider> obstacles;
	[HideInInspector]
	public List<Collider> friends;

	void Awake()
	{
		pathFinding = new AStar();
	}

	public void GoToPath()
	{
		if (AstarPath == null || currentIndex == AstarPath.Length)
			return;

		float dist = Vector3.Distance(AstarPath[currentIndex].transform.position, transform.position);

		if (dist >= 1)
			SmoothMovement();
		else
			currentIndex++;
	}

	public void GoToEnemyTarget(Transform targetEnemy)
	{
		GetFriendsAndObstacles();
		closerObstacle = GetCloserOb();

		separationVector = GetSeparation() * separationWeight;
		targetVector = (targetEnemy.position - transform.position) * targetWeight ;
		avoidance = GetObstacleAvoidance() * avoidWeight;

		direction = avoidance;
		direction += separationVector + targetVector;
		direction = new Vector3(direction.x, 0, direction.z);

		transform.forward = Vector3.Slerp(transform.forward, direction, rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //Para que jamas ni aunque colisione haga rotaciones raras.
		transform.position += transform.forward * Time.deltaTime * speed;
	}

	private void SmoothMovement()
	{
		GetFriendsAndObstacles();
		closerObstacle = GetCloserOb();
		avoidance = GetObstacleAvoidance() * avoidWeight;

		direction = avoidance + (AstarPath[currentIndex].transform.position - transform.position);

		direction = new Vector3(direction.x, 0, direction.z);

		transform.forward = Vector3.Slerp(transform.forward, direction, rotationSpeed * Time.deltaTime);
		transform.position += transform.forward * Time.deltaTime * speed;	
	}

	#region Avoidance Code
	private void GetFriendsAndObstacles()
	{
		obstacles.Clear();
		friends.Clear();

		friends.AddRange(Physics.OverlapSphere(transform.position, radFlock, LayerFriends));
		obstacles.AddRange(Physics.OverlapSphere(transform.position, radObst, LayerObst));
	}

	private Collider GetCloserOb()
	{
		if (obstacles.Count > 0)
			return obstacles.OrderBy(x => (x.transform.position - transform.position).magnitude).First();
		else
			return null;
	}

	private Vector3 GetObstacleAvoidance()
	{
		if (closerObstacle)
			return transform.position - closerObstacle.transform.position;
		else return Vector3.zero;
	}

	private Vector3 GetSeparation()
	{
		Vector3 separation = new Vector3();

		foreach (Collider thisFriend in friends)
		{
			Vector3 friendDistance = new Vector3();
			friendDistance = transform.position - thisFriend.transform.position;

			float magnitude = radFlock - friendDistance.magnitude;

			friendDistance.Normalize();
			friendDistance *= magnitude;

			separation += friendDistance;
		}
		return separation /= friends.Count;
	}
	#endregion;

	#region PathFindingCode
	public void MoveTo(Vector3 endPos)
	{
		currentIndex = 0;
		AstarPath = null;
		AstarPath = pathFinding.SearchPath(FindNearNode(transform.position), FindNearNode(endPos));
	}

	public PathNode FindNearNode(Vector3 pos)
	{
		Collider[] nearNodes = Physics.OverlapSphere(pos, 10);
		List<PathNode> nodesList = new List<PathNode>();

		foreach (Collider node in nearNodes)
			if (node.GetComponent<PathNode>())
				nodesList.Add(node.GetComponent<PathNode>());

		float dist = Mathf.Infinity;
		PathNode closest = null;

		foreach (var item in nodesList)
		{
			float ds = Vector3.Distance(pos, item.transform.position);
			if (ds < dist)
			{
				dist = ds;
				closest = item;
			}
		}
		return closest;
	}
	#endregion;
}
