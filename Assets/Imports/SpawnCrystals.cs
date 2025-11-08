using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class SpawnCrystals : MonoBehaviour
{
    private List<GameObject> crystals = new List<GameObject>();
    public GameObject[] allCrystals = new GameObject[6];
    public GameObject currentCrystal;
    public GameObject boundingBox; // The area to spawn crystals inside.

    // Important Stats //
    public int numCrystals; // Total crystals to place down :)
    public float distanceBetweenCrystals; // Space between crystals.
    public int scaleAmount; // Scale of the crystals. 30 or 100 is best.
    public int attemptCounter; // Take 10 attempts to place each crystal, else give up :(
    public int numColours; // The number of different types of colour materials.

    // Crystal Colours //
    public Material[] crystalMaterials = new Material[6]; // Chance of 6 different materials (red, yellow, blue, cyan, pink, green)
    
    private void Start()
    {
        PlaceCrystals(); // JUST ONCE
        // StartCoroutine(LoopCrystalPlacing()); // INFINITE RANDOMUnity 
    }

    private void PlaceCrystals()
    {
        float offsetX, offsetZ;

        // Get bounds of the invisible box collider, and fill it with crystals!!!
        Bounds bounds = boundingBox.GetComponent < Collider > ().bounds;

        for (int i = 0; i < numCrystals; i++)
        {
            attemptCounter = 0; // Stop accidentally causing an infinite while loop (learnt the hard way)
            offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
            offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

            Vector3 randomPosition = bounds.center + new Vector3(offsetX, -0.6f, offsetZ);

            while (!CheckIfLocationIsAvailable(randomPosition, distanceBetweenCrystals, crystals))
            {
                offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
                offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);
                randomPosition = bounds.center + new Vector3(offsetX, -0.6f, offsetZ);

                attemptCounter++;

                if (attemptCounter > 10) // The infinite while loop preventer.
                {
                    break;
                }
            }

            if (CheckIfLocationIsAvailable(randomPosition, distanceBetweenCrystals, crystals))
            {
                attemptCounter = 0;
                
                currentCrystal = allCrystals[Random.Range(0, 6)];
                GameObject crystal = Instantiate(currentCrystal, randomPosition, Quaternion.identity);
                
                crystal.transform.position = new Vector3(
                    crystal.transform.position.x,
                    crystal.transform.position.y, // Can minus from here to make them spawn underground or above ground.
                    crystal.transform.position.z);

                int randomRotationAmount = Random.Range(0, 361);
                
                crystal.transform.SetParent(boundingBox.transform);
                crystal.transform.rotation = Quaternion.Euler(-90f, transform.rotation.y, randomRotationAmount); // Rotate up (because imported from Blender).
                crystal.transform.localScale = new Vector3(scaleAmount , scaleAmount, scaleAmount);
                crystal.GetComponent<Renderer>().material = crystalMaterials[Random.Range(0, numColours)];
                crystals.Add(crystal);
            }
        }
        crystals.Clear();
    }

    public bool CheckIfLocationIsAvailable(Vector3 randomPosition, float radius, List<GameObject> existingCrystals)
    {
        // Check if this spot is free. 
        // Make a sphere, see if any objects are inside and have the tag "Crystal".
        // If there's a gap, we place another crystal.
        
        Vector3 boxBounds = new Vector3(radius, randomPosition.y, radius);
        Collider[] nearObjects = Physics.OverlapBox(randomPosition, boxBounds, Quaternion.identity);

        foreach (Collider ob in nearObjects)
        {
            if (ob.tag == "Crystal") // Don't place crystal on top of another crystal.
            {
                return false;
            }
        }

        foreach (GameObject crystal in existingCrystals) // Prevent crystals overlapping by using the radius.
        {
            if (crystal != null && Vector3.Distance(randomPosition, crystal.transform.position) < radius)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator LoopCrystalPlacing()
    {
        while (true)
        {
            // Wait before spawning
            yield return new WaitForSeconds(0.5f);
            
            PlaceCrystals();

            // Wait again before removing them
            yield return new WaitForSeconds(3f);

            // Destroy all current crystals
            foreach (Transform child in boundingBox.transform)
            {
                Destroy(child.gameObject);
            }
            crystals.Clear();
        }
    }
}
