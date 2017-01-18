using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class MapGenerator : MonoBehaviour {
	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(1,100)]
	public int NumberOfIterations;

	[Range(0,100)]
	public int randomFillPercent;

	int [,] map;

	void Start(){
		GenerateMap ();
	}

	[BitStrap.Button]
	void GenerateMap(){
		map = new int[width, height]; 
		RandomFillMap ();

		for (int i = 0; i < NumberOfIterations; i++) {
			SmothMap (); 
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (map,1);
	}

	void RandomFillMap(){
		if (useRandomSeed) {
			int rnd = UnityEngine.Random.Range (1,int.MaxValue/10000000);
			seed = ((Time.realtimeSinceStartup+1)*rnd).ToString (); 

		}
		System.Random pseudoRandom = new System.Random (seed.GetHashCode());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width -1 || y == 0 || y == height-1) {
					map [x, y] = 1;
				}
				map [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
			}
		}
	}

	void SmothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbourWallTiles = GetSurroundingWallCount (x,y);
				if (neighbourWallTiles > 4 ) {
					map [x, y] = 1;
				}else if (neighbourWallTiles < 4) {
					map [x, y] = 0;
				}
			}
		}
	}

	int GetSurroundingWallCount(int gridX,int gridY){
		int wallCount = 0;
		for (int neightbourX = gridX -1; neightbourX <= gridX+1; neightbourX++) {
			for (int neightbourY = gridY -1; neightbourY <= gridY+1; neightbourY++) {
				if (neightbourX >= 0 && neightbourX < width && neightbourY >= 0 && neightbourY < height) {
					if (neightbourX != gridX || neightbourY != gridY) {
						wallCount += map [neightbourX, neightbourY];
					}
				} else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	void OnDrawGizmos(){
		/*
		if (map != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Gizmos.color = (map [x, y] == 1) ? Color.black : Color.white;
					Vector3 pos = new Vector3 (-width/2+x+.5f+this.transform.position.x,0,-height/2 +y+.5f+this.transform.position.z);
					Gizmos.DrawCube (pos, Vector3.one);
				}
			}
		}
		*/
	}
}
