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

    private float[] _wallList = new float[4];

    public int StuckCounter { get; set; }
    public float TickTimer { get; private set; }
    public bool WasMoving { get; set; }

    public Vector3 GoBackPosition { get; set; }
    public PointAccses CurrentPoint { get; set; }
    public PlayerController CanSeePlayer { get; private set; }
    public CapsuleCollider LookTrigger { get; private set; }

    public float MoveSpeed = 4f;
    public float RotSpeed = 180f;
    public float MaxSusicionLevel = 10f;
    public float HighestPoint = 2f;
    public float LowestPoint = 0.5f;

    [SerializeField]
    private float _triggerHeight;
    [SerializeField]
    private float _triggerWidth;
    [SerializeField]
    private float _triggerCenterY;

    [SerializeField]
    private GameObject _reactState;
    [SerializeField]
    private GameObject _chaseState;
    [SerializeField]
    private GameObject _lookState;

    private Vector3 _triggerCenter = Vector3.zero;
    private ISpyState _enemyState;
    private Light _lookArea;

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

        Light[] allLight = GetComponentsInChildren<Light>();
        for (int i = 0; i < allLight.Length; i++)
        {
            if (allLight[i].type == LightType.Spot)
            {
                _lookArea = allLight[i];
                break;
            }
        }
        LookTrigger = _lookArea.GetComponentInChildren<CapsuleCollider>();

        _reactState.SetActive(false);
        _chaseState.SetActive(false);
        _lookState.SetActive(false);

        UpdateRaycast();

        if (_enemyState != null)
            _enemyState.Setup(this);


        GoBackPosition = Vector3.zero;
        CurrentPoint = null;

        WasMoving = false;

        _triggerCenter.y = _triggerCenterY;
        LookTrigger.height = _triggerHeight;
        LookTrigger.radius = _triggerWidth;
        LookTrigger.center = _triggerCenter;

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
        UpdateLookDistance();

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
            RaycastDirectionList[i].Clear();

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
            List<Vector3> tempWallListHit = new List<Vector3>();

            //updating the north raycast list
            for (int u = 0; u < raycastHitArrayList[i].Length; u++)
            {
                RaycastHit currentHit = raycastHitArrayList[i][u];

                //finds every object that has layer wall on it
                if (currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    tempWallListHit.Add(currentHit.point);
                }
            }

            //Adds object to the right RaycastList

            //Adds if there are more then one wall
            if (tempWallListHit.Count > 1)
            {
                //temp list for all distances between all walls
                List<float> wallDistance = new List<float>();

                //the math for the distance
                for (int u = 0; u < tempWallListHit.Count; u++)
                {
                    wallDistance.Add(Vector3.Distance(transform.position, tempWallListHit[u]));
                }

                //Sorts the temp list so the closest wall is first
                wallDistance.Sort();
                _wallList[i] = wallDistance[0];

                //Adds all object with layer "Spy" and "Point" if the object is closer then the wall
                for (int u = 0; u < raycastHitArrayList[i].Length; u++)
                {
                    if (Vector3.Distance(transform.position, raycastHitArrayList[i][u].collider.gameObject.transform.position) < wallDistance[0])
                    {
                        RaycastDirectionList[i].Add(raycastHitArrayList[i][u].collider.gameObject);
                    }
                }
            }
            //Adds when there are no walls
            else if (tempWallListHit.Count < 1)
            {
                _wallList[i] = int.MaxValue;

                //adds the whole raycast list
                for (int u = 0; u < raycastHitArrayList[i].Length; u++)
                    RaycastDirectionList[i].Add(raycastHitArrayList[i][u].collider.gameObject);
            }
            //Adds when there is only one wall
            else
            {
                //calculating the distance between the wall add object
                float wallDistance = Vector3.Distance(transform.position, tempWallListHit[0]);

                _wallList[i] = wallDistance;

                //Adds all object with layer "Spy" and "Point" if the object is closer then the wall
                for (int u = 0; u < raycastHitArrayList[i].Length; u++)
                {
                    if (Vector3.Distance(transform.position, raycastHitArrayList[i][u].collider.gameObject.transform.position) < wallDistance)
                        RaycastDirectionList[i].Add(raycastHitArrayList[i][u].collider.gameObject);
                }
            }
        }
    }
    public void ConstantRaycastUpdate(Vector3 direction)
    {
        LayerMask mask = LayerMask.GetMask("Wall");
        RaycastHit[] allObject = Physics.RaycastAll(transform.position, direction, float.MaxValue, mask, QueryTriggerInteraction.Collide);

        List<float> wallListDist = new List<float>();
        float changedDist = 0f;

        foreach (var i in allObject)
        {
            wallListDist.Add(Vector3.Distance(transform.position, i.point));
        }

        if (wallListDist.Count > 1)
        {
            wallListDist.Sort();

            changedDist = wallListDist[0];
        }
        else if (wallListDist.Count < 1)
            changedDist = float.MaxValue;
        else
            changedDist = wallListDist[0];

        direction = direction.normalized;
        direction = new Vector3(Mathf.Round(direction.x), Mathf.Round(direction.y), Mathf.Round(direction.z));

        if (direction.Equals(Vector3.forward))
            _wallList[0] = changedDist;
        else if (direction.Equals(Vector3.right))
            _wallList[1] = changedDist;
        else if (direction.Equals(Vector3.back))
            _wallList[2] = changedDist;
        else if (direction.Equals(Vector3.left))
            _wallList[3] = changedDist;
    }

    public void UpdateLookDistance()
    {
        int lookDirection = Mathf.RoundToInt(transform.rotation.eulerAngles.y / 90f);
        float distanceCheck = 0f;

        switch (lookDirection)
        {
            case 0:
            case 4:
                if (lookDirection != 0)
                    lookDirection = 0;

                distanceCheck = _wallList[lookDirection];
                break;
            case 1:
                distanceCheck = _wallList[lookDirection];
                break;
            case 2:
                distanceCheck = _wallList[lookDirection];
                break;
            case 3:
                distanceCheck = _wallList[lookDirection];
                break;
            default:
                Debug.LogError("The distance check got outside of the bounds.");
                break;
        }

        if (distanceCheck <= _triggerHeight)
        {
            LookTrigger.height = distanceCheck;
            _triggerCenter.y = distanceCheck / 2f;
            _lookArea.range = distanceCheck + 1f;

            if (distanceCheck <= _triggerWidth)
                _triggerWidth = distanceCheck / 2f;
        }
        else
        {
            LookTrigger.height = _triggerHeight;
            LookTrigger.radius = _triggerWidth;
            _triggerCenter.y = _triggerCenterY;
            _lookArea.range = _triggerHeight + 1f;
        }
        
        LookTrigger.center = _triggerCenter;
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
