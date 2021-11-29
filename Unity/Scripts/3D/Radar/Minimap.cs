using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Minimap : MonoBehaviour
{
    public bool ScanTerrain = false;
	// find the ship object we want to show on the map
	public string PlayerName = "Player";
	public GameObject playerObject;
	public Texture aTexture;
	Vector2 mapSideNorthStartPoint;
	Vector2 mapSideNorthStopPoint;
	Vector2 mapSideSouthStartPoint;
	Vector2 mapSideSouthStopPoint;
	Vector2 mapSideWestStartPoint;
	Vector2 mapSideWestStopPoint;
	Vector2 mapSideEastStartPoint;
	Vector2 mapSideEastStopPoint;
	float backgroundImageHeight = 10;
	float backgroundImageWidth = 10;
	public Radar Radar;
	// world boundaries
	//float WORLD_MIN_X = -10.0f;awake
	//float WORLD_MIN_Z = -10.0f;
	//float WORLD_MAX_X = 10.0f;
	//float WORLD_MAX_Z = 10.0f;

	float mapWidth; 
	float mapHeight;
	Vector2 mapCenter;
	Vector2 mapStart;
	Vector2 mapSize;

	DateTime beaconCheckTime = DateTime.Now;
	DateTime sonarCheckTime = DateTime.Now;
	   
	// Use this for initialization
	void Start()
    {
        Radar = GameObject.FindObjectOfType<Radar>();
        if (playerObject == null)
            playerObject = GameObject.FindGameObjectWithTag("Player");
        InitMapSettings();
    }

    private void InitMapSettings()
    {
        mapWidth = Screen.width * 0.5f;   // 20% of width
        mapHeight = Screen.height * 0.5f;


        mapSize = new Vector2(mapWidth, mapHeight);
        //Vector3 mapStart = new Vector2(Screen.width - mapSize.x - 10, Screen.height - mapSize.y - 10);
        mapStart = new Vector2(10, Screen.height - mapSize.y - 10);


        // calculate center of mini-map in screen coordinates
        mapCenter = new Vector2(mapStart.x + mapSize.x / 2, mapStart.y + mapSize.y / 2);

        mapSideNorthStartPoint = new Vector2(mapStart.x, mapStart.y);
        mapSideNorthStopPoint = new Vector2(mapStart.x + mapWidth, mapStart.y);
        mapSideSouthStartPoint = new Vector2(mapStart.x, mapStart.y + mapHeight);
        mapSideSouthStopPoint = new Vector2(mapStart.x + mapWidth, mapStart.y + mapHeight);
        mapSideWestStartPoint = new Vector2(mapStart.x, mapStart.y);
        mapSideWestStopPoint = new Vector2(mapStart.x, mapStart.y + mapHeight);
        mapSideEastStartPoint = new Vector2(mapStart.x + mapWidth, mapStart.y);
        mapSideEastStopPoint = new Vector2(mapStart.x + mapWidth, mapStart.y + mapHeight);

        backgroundImageHeight = mapHeight;
        backgroundImageWidth = mapHeight;
    }

    // Update is called once per frame
    void Update()
	{
		if(mapWidth != Screen.width * 0.5f)
        {
			InitMapSettings();
        }
	}

	List<Vector2> radarPointsCache = new List<Vector2>();
	// OnGUI is called once per frame after other rendering is done
	void OnGUI()
	{
        if(playerObject != null && Radar!=null)
		//if ((DateTime.Now - beaconCheckTime).TotalMilliseconds > 0)
		{
			// create a mini-map showing the ship and target
			if (aTexture != null)
				GUI.DrawTexture(new Rect(mapCenter.x - (backgroundImageHeight / 2), mapCenter.y - (backgroundImageWidth / 2), backgroundImageHeight, backgroundImageWidth), aTexture, ScaleMode.ScaleToFit, true, 1.0F);


			// build and draw Rect holding map screen boundaries
			Rect mapBorder = new Rect(mapStart, mapSize);
			DrawShapes.DrawRectangle(mapBorder, Color.black, 2);

			//// build a Rect to represent the player on the map
			Rect playerRect = new Rect(mapCenter.x, mapCenter.y, 5, 5);

			// draw player as a small red rectangle
			DrawShapes.DrawRectangle(playerRect, Color.blue, 2);

			GUIStyle style1 = new GUIStyle();
			style1.richText = true;
			GUI.Label(new Rect(mapCenter.x, mapCenter.y+5, 100, 20), "<color='blue'>" + PlayerName + "</color>", style1);


			// add code here to draw the target on the mini-map...
			MapBeacon[] beacons = GameObject.FindObjectsOfType<MapBeacon>();

			foreach (MapBeacon b in beacons)

			{
				//draw the beacon on the map.
				//rotate it so we are always facing forward
				float fYRot = Radar.gameObject.transform.eulerAngles.y;//playerObject.transform.eulerAngles.y
				var radarDistance = (int)Vector3.Distance(b.transform.position, playerObject.transform.position);
				var circleSize = 15 - (radarDistance / 15);
				if (circleSize < 5)
					circleSize = 5;
				//Vector2 v2 = new Vector2(mapCenter.x+b.mapX, mapCenter.y+b.mapY) * Quaternion.Euler(0, fYRot,0);
				//Vector3 v2 = new Vector3(mapCenter.x + b.mapX, 0, mapCenter.y + b.mapY);
				//var rotatedVector3 = RotatePointAroundPivot(v2, new Vector3(mapCenter.x, 0, mapCenter.y), fYRot);
				Vector2 v2 = (new Vector2(mapCenter.x + b.mapX, mapCenter.y + b.mapY));
				var rotatedVector = DrawShapes.RotatePointAroundPivot(v2, mapCenter, fYRot);
				//Vector2 rotatedVector2 = new Vector2(rotatedVector3.x, rotatedVector3.y);
				//draw an arrow pointing towards the beacon if the beacon is off the map.

				GUIStyle style = new GUIStyle();
				style.richText = true;
				Vector2 pointOfIntersection = new Vector2();
				//are we intersecting the north border?
				if (LineIntersection((mapSideNorthStartPoint), (mapSideNorthStopPoint), mapCenter, rotatedVector, ref pointOfIntersection))
				{
					GUI.Label(new Rect(pointOfIntersection.x, pointOfIntersection.y, 100, 20), "<color='red'>" + radarDistance + " " + b.MapText + "</color>", style);

					var dir = mapCenter - rotatedVector; //a vector pointing from pointA to pointB
					var rot = Quaternion.LookRotation(dir, Vector3.forward).eulerAngles; //calc a rotation that
																						 //Debug.Log(string.Format("{0},{1},{2}",rot.x,rot.y,rot.z));
																						 //draw a cyan circle at the point of intersection
					DrawShapes.DrawCircle(pointOfIntersection, circleSize, Color.red, 2);
					//DrawShapes.DrawTriangle(pointOfIntersection, Color.cyan, 2, 10, rot.z);
				}
				else if (LineIntersection(mapSideSouthStartPoint, (mapSideSouthStopPoint), mapCenter, rotatedVector, ref pointOfIntersection))
				{
					GUI.Label(new Rect(pointOfIntersection.x, pointOfIntersection.y, 100, 20), "<color='red'>" + radarDistance + " " + b.MapText + "</color>", style);

					//draw a cyan circle at the point of intersection
					DrawShapes.DrawCircle(pointOfIntersection, circleSize, Color.red, 2);
					//DrawShapes.DrawTriangle(pointOfIntersection, Color.cyan, 2, 10, fYRot);
				}
				else if (LineIntersection((mapSideWestStartPoint), (mapSideWestStopPoint), mapCenter, rotatedVector, ref pointOfIntersection))
				{
					GUI.Label(new Rect(pointOfIntersection.x, pointOfIntersection.y, 100, 20), "<color='red'>" + radarDistance + " " + b.MapText + "</color>", style);

					//draw a cyan circle at the point of intersection
					DrawShapes.DrawCircle(pointOfIntersection, circleSize, Color.red, 2);
					//DrawShapes.DrawTriangle(pointOfIntersection, Color.cyan, 2, 10, fYRot);
				}
				else if (LineIntersection((mapSideEastStartPoint), (mapSideEastStopPoint), mapCenter, rotatedVector, ref pointOfIntersection))
				{
					GUI.Label(new Rect(pointOfIntersection.x, pointOfIntersection.y, 100, 20), "<color='red'>" + radarDistance + " " + b.MapText + "</color>", style);

					//draw a cyan circle at the point of intersection
					DrawShapes.DrawCircle(pointOfIntersection, circleSize, Color.red, 2);
					//DrawShapes.DrawTriangle(pointOfIntersection, Color.cyan, 2, 10, fYRot);
				}
				else
				{
					GUI.Label(new Rect(rotatedVector.x, rotatedVector.y, 100, 20), "<color='red'>" + radarDistance + " " + b.MapText + "</color>", style);

					//draw the beacon as a circle
					DrawShapes.DrawCircle(rotatedVector, circleSize, Color.red, 2);
				}
			}


			for (int i= 0; i < radarPointsCache.Count;i++)
			{
				Vector2 p = radarPointsCache[i];
				if (p != Vector2.zero)
				{
					DrawShapes.DrawCircle(p, 4, Color.yellow, 4);
					////was the one before us null?  If not, then draw a line between the points.
					//if (i > 0 && radarPointsCache[i - 1] != Vector2.zero)
					//	DrawShapes.DrawLine(radarPointsCache[i - 1], p, Color.yellow, 4);
				}
			}

			beaconCheckTime = DateTime.Now;

		}
		if ((DateTime.Now - sonarCheckTime).TotalMilliseconds > 50)
		{
			if (Radar != null && ScanTerrain)
			{
				List<Vector2> radarCache = new List<Vector2>();
				List<Vector2> points = Radar.ScanTerrain();
				foreach (Vector2 p in points)
				{
					if (p != Vector2.zero)
					{
						float fYRot = Radar.transform.eulerAngles.y;// playerObject.transform.eulerAngles.y;
						var mapX = p.x - Radar.transform.position.x;
						var mapY = p.y - Radar.transform.position.z;
						Vector2 v2 = (new Vector2(mapCenter.x + mapX, mapCenter.y + mapY));
						var rotatedVector = DrawShapes.RotatePointAroundPivot(v2, mapCenter, fYRot);
						radarCache.Add(rotatedVector);
					}
					else
					{
						radarCache.Add(Vector2.zero);
					}
				}
				radarPointsCache = radarCache;
			}
			sonarCheckTime = DateTime.Now;
		}

	}

	
	//Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
	//{
	//	var dir = point - pivot; // get point direction relative to pivot
	//	dir = Quaternion.Euler(180, angle, 0) * dir; // rotate it
	//	Vector3 pointNew = dir + pivot; // calculate rotated point
	//	return pointNew; // return it
	//}

	////Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
	////Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
	////same plane, use ClosestPointsOnTwoLines() instead.
	//public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
	//{

	//	Vector3 lineVec3 = linePoint2 - linePoint1;
	//	Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
	//	Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

	//	float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

	//	//is coplanar, and not parrallel
	//	if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
	//	{
	//		float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
	//		intersection = linePoint1 + (lineVec1 * s);
	//		return true;
	//	}
	//	else
	//	{
	//		intersection = Vector3.zero;
	//		return false;
	//	}
	//}

	public static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 intersection)
	{

		float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num/*,offset*/;

		float x1lo, x1hi, y1lo, y1hi;



		Ax = p2.x - p1.x;

		Bx = p3.x - p4.x;



		// X bound box test/

		if (Ax < 0)
		{

			x1lo = p2.x; x1hi = p1.x;

		}
		else
		{

			x1hi = p2.x; x1lo = p1.x;

		}



		if (Bx > 0)
		{

			if (x1hi < p4.x || p3.x < x1lo) return false;

		}
		else
		{

			if (x1hi < p3.x || p4.x < x1lo) return false;

		}



		Ay = p2.y - p1.y;

		By = p3.y - p4.y;



		// Y bound box test//

		if (Ay < 0)
		{

			y1lo = p2.y; y1hi = p1.y;

		}
		else
		{

			y1hi = p2.y; y1lo = p1.y;

		}



		if (By > 0)
		{

			if (y1hi < p4.y || p3.y < y1lo) return false;

		}
		else
		{

			if (y1hi < p3.y || p4.y < y1lo) return false;

		}



		Cx = p1.x - p3.x;

		Cy = p1.y - p3.y;

		d = By * Cx - Bx * Cy;  // alpha numerator//

		f = Ay * Bx - Ax * By;  // both denominator//



		// alpha tests//

		if (f > 0)
		{

			if (d < 0 || d > f) return false;

		}
		else
		{

			if (d > 0 || d < f) return false;

		}



		e = Ax * Cy - Ay * Cx;  // beta numerator//



		// beta tests //

		if (f > 0)
		{

			if (e < 0 || e > f) return false;

		}
		else
		{

			if (e > 0 || e < f) return false;

		}



		// check if they are parallel

		if (f == 0) return false;

		// compute intersection coordinates //

		num = d * Ax; // numerator //

		//    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;   // round direction //

		//    intersection.x = p1.x + (num+offset) / f;
		intersection.x = p1.x + num / f;



		num = d * Ay;

		//    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;

		//    intersection.y = p1.y + (num+offset) / f;
		intersection.y = p1.y + num / f;



		return true;

	}
}
