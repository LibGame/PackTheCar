using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SWS;

public class PathCreator : MonoBehaviour
{
    public static PathCreator instance;
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    GameObject walker;
    bool candraw;
    string ParkedCar;

    int CarParked;

   
    bool PathCreated = false;
    [Header("Paint")]
    public GameObject[] Paint;

    [Header("No of Cars")]
    public int Cars;

    string getName,getCarName;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        
        walker = GameObject.FindGameObjectWithTag("Player");
         candraw = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (!PathCreated) {
            if (Input.GetMouseButtonDown(0))
            {
                points.Clear();
                Ray rayCheck = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit HitInfoCar;
                if (Physics.Raycast(rayCheck, out HitInfoCar)) {

                    if (HitInfoCar.collider.gameObject.CompareTag("Stay"))
                    {
                      getName = (HitInfoCar.collider.gameObject.name);
                       
                            candraw = true;
                            foreach (GameObject go in Paint)
                            {
                                if (go.name == getName + "PAINT")
                                {
                                    go.SetActive(true);
                                    break;
                                }
                            }
                        

                    }
                  
                }
            }


            if (candraw && (Input.GetMouseButton(0)))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit HitInfo;
                if (Physics.Raycast(ray, out HitInfo))
                {
                    if (DistanceToLastPoint(HitInfo.point) > 0.5f)
                    {

                        points.Add(HitInfo.point);

                    }


                }


            }
            else if (candraw && (Input.GetMouseButtonUp(0)))
            {
                DrawPathForRed();
                foreach (GameObject go in Paint)
                {
                    if (go.name == getName + "PAINT")
                    {
                        go.SetActive(false);
                        break;
                    }
                }
                //PathCreated = true;
                candraw = false;

            }

    }
}
    private float DistanceToLastPoint(Vector3 point) {

        if (!points.Any())
            return Mathf.Infinity;
        return Vector3.Distance(points.Last(), point);

    }

    void DrawPathForRed()
    {

       
            //create path manager game object
            GameObject newPath = new GameObject(getName+"Path");
            PathManager path = newPath.AddComponent<PathManager>();

            //declare waypoint positions
            Vector3[] positions = points.ToArray();
            Transform[] waypoints = new Transform[positions.Length];

            //instantiate waypoints
            for (int i = 0; i < positions.Length; i++)
            {
                GameObject newPoint = new GameObject("Waypoint " + i);
                waypoints[i] = newPoint.transform;
                waypoints[i].position = positions[i];
            }

            //assign waypoints to path
            path.Create(waypoints, true);
           GameObject.Find(getName+"CAR").GetComponent<splineMove>().SetPath(WaypointManager.Paths[path.name]);

    }



    public void Score() {

        CarParked++;
        if (CarParked == Cars) {
            LevelCompleted();
        }
    }


    public void LevelCompleted() {
        GameManager.instance.LevelCompleted();
    }


    public void CarStopped(bool inside) {
        Debug.Log("CarStopped "+inside);
        if (!inside)
        {
            GameManager.instance.GameOver();
        }
    }

  

  



}
