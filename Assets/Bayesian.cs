using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayesianAI : MonoBehaviour
{
    // Beliefs (Prior Probabilities) about the player's location in each room
    public float[] priorProbabilities = { 0.33f, 0.33f, 0.33f }; // equal prior probabilities for 3 rooms
    public float[] likelihoods = { 0.33f, 0.33f, 0.33f }; // initial likelihoods (based on room proximity)
    public float[] updatedProbabilities;

    // Room positions
    public Vector3[] roomPositions = new Vector3[3];

    void Start()
    {
        updatedProbabilities = new float[priorProbabilities.Length];
        Debug.Log("Initial Probabilities: " + string.Join(", ", priorProbabilities));
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Get the position of the mouse click in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = 0; // Ensure it’s on the same level as the rooms and AI

            // Update likelihoods based on the click position
            UpdateLikelihoods(mousePosition);

            // Update beliefs using Bayes' Theorem
            UpdateBeliefs();

            // Output the updated probabilities to console
            Debug.Log("Updated Probabilities: " + string.Join(", ", updatedProbabilities));
        }
    }

    // Function to update likelihoods based on click position
    void UpdateLikelihoods(Vector3 noisePosition)
    {
        for (int i = 0; i < roomPositions.Length; i++)
        {
            float distance = Vector3.Distance(noisePosition, roomPositions[i]);

            // Adjust likelihood based on distance; closer means higher likelihood
            if (distance < 5) // If the noise is close to the room
            {
                likelihoods[i] = 0.5f;
            }
            else if (distance < 10)
            {
                likelihoods[i] = 0.3f;
            }
            else
            {
                likelihoods[i] = 0.1f;
            }
        }
    }

    // Function to update the AI's belief based on Bayes' Theorem
    void UpdateBeliefs()
    {
        float totalEvidence = 0f;

        // Calculate the total evidence (normalizing factor)
        for (int i = 0; i < priorProbabilities.Length; i++)
        {
            totalEvidence += likelihoods[i] * priorProbabilities[i];
        }

        // Calculate the updated probabilities using Bayes' Theorem
        for (int i = 0; i < priorProbabilities.Length; i++)
        {
            updatedProbabilities[i] = (likelihoods[i] * priorProbabilities[i]) / totalEvidence;
        }
    }
}

