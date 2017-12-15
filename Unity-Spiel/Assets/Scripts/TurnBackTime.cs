﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;

public class TurnBackTime : MonoBehaviour {

	public static bool isRewinding = false;

	public float t = 2F; //Rückspulzeit

	public GameObject Canvas;
	Canvas canvas;

	
	List<Vector3> positions;
	List<Quaternion> rotations;

    public Texture2D ZeitNormal;

	AudioSource AudioSource;

    void Start ()
	{
		positions = new List<Vector3>();
		rotations = new List<Quaternion>();
		Canvas = GameObject.Find("RewindCanvas");
		canvas = Canvas.GetComponentInChildren<Canvas>();
		canvas.enabled = false;
		AudioSource = Camera.main.GetComponent<AudioSource>();
	}

  /*  private void OnGUI()
    {
        GUI.DrawTexture(new Rect(50, 400, 100, 100), ZeitNormal);
    }*/


        void FixedUpdate()
	{
		if (isRewinding)
			Rewind();
		else
			Record();
	}

	void Record()
	{
		if (positions.Count > Mathf.Round(5F / Time.fixedDeltaTime))
		{
			positions.RemoveAt(positions.Count - 1); //alle Werte die älter als 5Sekunden sind werden gelöscht
			rotations.RemoveAt(rotations.Count - 1);
		}
			positions.Insert(0, transform.position); // neue Werte hinzufügen
			rotations.Insert(0, transform.rotation);
		

	}

	void Rewind()
	{
		if (positions.Count > 0)
		{
			transform.position = positions[0];
			positions.RemoveAt(0);
			transform.rotation = rotations[0];
			rotations.RemoveAt(0);
			//gameObject.GetComponent<NavMeshAgent>().ResetPath();
		}
	}

	void Update ()
	{

		// Wenn "R" gedrückt wird und die Fäigkeit nicht auf Cooldown ist, wird die zeit 3s zurückgespult
		if (Input.GetKeyDown(KeyCode.R) && !RewindCooldown.isOnCooldown)
		{
			StartCoroutine("RewindTime");
			RewindCooldown.isOnCooldown = true;
			RewindCooldown.timeRemaining = RewindCooldown.Cooldown;
			AudioSource.Play();
		}
	}

	IEnumerator RewindTime()
	{
		isRewinding = true;
		canvas.enabled = true;
		Time.timeScale = 0F;
		GameObject.Find("Player").GetComponent<HealthHandeler>().TakeDamage(-20F);
		yield return new WaitForSeconds(t);
		Time.timeScale = 1F;
		isRewinding = false;
		canvas.enabled = false;
	}
}
