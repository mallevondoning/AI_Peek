using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<List<GameObject>> RaycastDirectionList = new List<List<GameObject>>();
    public List<GameObject> RaycastListNorth { get; private set; }
    public List<GameObject> RaycastListEast { get; private set; }
    public List<GameObject> RaycastListSouth { get; private set; }
    public List<GameObject> RaycastListWest { get; private set; }

    public int StuckCounter { get; set; }
    public float TickTimer { get; private set; }
    public bool HasMoved { get; set; }
    public bool WasMoving { get; set; }

    public Vector3 GoBackPosition { get; set; }
    public PointAccses CurrentPoint { get; set; }
    public PlayerController CanSeePlayer { get; private set; }
    public MeshCollider LookArea { get; private set; }

    public float MoveSpeed = 4f;
    public float RotSpeed = 180f;
    public float MaxSusicionLevel = 10f;
    public float HighestPoint = 2f;
    public float LowestPoint = 0.5f;

    [SerializeField]
    private GameObject _reactState;
    [SerializeField]
    private GameObject _chaseState;
    [SerializeField]
    private GameObject _lookState;

    private ISpyState _enemyState;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        _enemyState = new Wait();

        RaycastDirectionList = new List<List<GameObject>>();

        RaycastListNorth = new List<GameObject>();
        RaycastDirectionList.Add(RaycastListNorth);

        RaycastListEast = new List<GameObject>();
        RaycastDirectionList.Add(RaycastListEast);

        RaycastListSouth = new List<GameObject>();
        RaycastDirectionList.Add(RaycastListSouth);

        RaycastListWest = new List<GameObject>();
        RaycastDirectionList.Add(RaycastListWest);

        LookArea = GetComponentInChildren<MeshCollider>();

        _reactState.SetActive(false);
        _chaseState.SetActive(false);
        _lookState.SetActive(false);

        if (_enemyState != null)
        {
            UpdateRaycast();
            _enemyState.Setup(this);
        }

        GoBackPosition = Vector3.zero;
        CurrentPoint = null;

        HasMoved = true;
        WasMoving = false;

        TickTimer = Time.deltaTime;
    }

    void Tick()
    {
        _enemyState.Tick(this);

        ISpyState newState = _enemyState.Transition(this);
        if (newState != null) 
        { 
            _enemyState = newState;
            _enemyState.Setup(this);
        }
    }
    IEnumerator TickLoop()
    {
        Tick();

        yield return new WaitForSeconds(TickTimer);
    }

    void Update()
    {
        StartCoroutine(TickLoop());
    }

    public void UpdateRaycast()
    {
        List<RaycastHit[]> raycastHitArrayList = new List<RaycastHit[]>();
        
        //mask for the raycast
        var mask = LayerMask.GetMask("Wall", "Point");

        for (int i = 0; i < RaycastDirectionList.Count; i++)
        {
            RaycastDirectionList[i].Clear();
        }

        //north looking raycast
        raycastHitArrayList.Add(Physics.RaycastAll(transform.position, Vector3.forward, int.MaxValue, mask, QueryTriggerInteraction.Collide));
        //east looking raycast
        raycastHitArrayList.Add(Physics.RaycastAll(transform.position, Vector3.right, int.MaxValue, mask, QueryTriggerInteraction.Collide));
        //south looking raycast
        raycastHitArrayList.Add(Physics.RaycastAll(transform.position, Vector3.back, int.MaxValue, mask, QueryTriggerInteraction.Collide));
        //west looking raycast
        raycastHitArrayList.Add(Physics.RaycastAll(transform.position, Vector3.left, int.MaxValue, mask, QueryTriggerInteraction.Collide));

        //adds relevant object to all Raycast Direction Lists 

        //Loop for every direction 
        for (int i = 0; i < RaycastDirectionList.Count; i++)
        {
            List<GameObject> tempWallList = new List<GameObject>();

            //updating the north raycast list
            for (int u = 0; u < raycastHitArrayList[i].Length; u++)
            {
                RaycastHit currentHit = raycastHitArrayList[i][u];

                //finds every object that has layer wall on it
                if (currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    tempWallList.Add(currentHit.collider.gameObject);
                }
            }

            //Adds object to the right RaycastList

            //Adds if there are more then one wall
            if (tempWallList.Count > 1)
            {
                //temp list for all distances between all walls
                List<float> wallDistance = new List<float>();

                //the math for the distance
                for (int u = 0; u < tempWallList.Count; u++)
                {
                    wallDistance.Add(Vector3.Distance(transform.position, tempWallList[u].transform.position));
                }

                //Sorts the temp list so the closest wall is first
                wallDistance.Sort();

                //Adds all object with layer "Spy" and "Point" if the object is closer then the wall
                for (int u = 0; u < raycastHitArrayList[i].Length; u++)
                {
                    if (Vector3.Distance(transform.position, raycastHitArrayList[i][u].collider.gameObject.transform.position) < wallDistance[0])
                        RaycastDirectionList[i].Add(raycastHitArrayList[i][u].collider.gameObject);
                }
            }
            //Adds when there are no walls
            else if (tempWallList.Count < 1)
            {
                //adds the whole raycast list
                for (int u = 0; u < raycastHitArrayList[i].Length; u++)
                    RaycastDirectionList[i].Add(raycastHitArrayList[i][u].collider.gameObject);
            }
            //Adds when there is only one wall
            else
            {
                //calculating the distance between the wall add object
                float wallDistance = Vector3.Distance(transform.position, tempWallList[0].transform.position);

                //Adds all object with layer "Spy" and "Point" if the object is closer then the wall
                for (int u = 0; u < raycastHitArrayList[i].Length; u++)
                {
                    if (Vector3.Distance(transform.position, raycastHitArrayList[i][u].collider.gameObject.transform.position) < wallDistance)
                        RaycastDirectionList[i].Add(raycastHitArrayList[i][u].collider.gameObject);
                }
            }
        }
    }

    public void SetReactActive(bool isOn)
    {
        _reactState.SetActive(isOn);
    }
    public void SetChaseActive(bool isOn)
    {
        _chaseState.SetActive(isOn);
    }
    public void SetLookActive(bool isOn)
    {
        _lookState.SetActive(isOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerCheck = other.GetComponent<PlayerController>();

        if (playerCheck != null)
            CanSeePlayer = playerCheck;
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerController playerCheck = other.GetComponent<PlayerController>();

        if (playerCheck != null)
            CanSeePlayer = null;
    }
}
