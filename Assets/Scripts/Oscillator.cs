using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period;

    Vector3 startingPos;
    const float tau = Mathf.PI * 2;

    // Use this for initialization
    void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (period <= Mathf.Epsilon)
            return;
        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles * tau);
        float fixedSinWave = (rawSinWave + 1f) / 2f; // New range is [0, 1]
        Vector3 offset = movementVector * fixedSinWave;
        transform.position = startingPos + offset;
	}
}
