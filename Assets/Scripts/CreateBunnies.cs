using UnityEngine;
using System.Collections;

public class CreateBunnies : MonoBehaviour {

	private GameObject character;
	public GameObject planet;
	public int numberOfSpawns = 10;

	// Use this for initialization
	void Start () {
		character = Resources.Load("Usagi") as GameObject;
		for (var i=0; i<=numberOfSpawns; i++) {
			Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x/2) + character.transform.localScale.y * 0.5f) + planet.transform.position;
			Quaternion spawnRotation = Quaternion.identity;
			GameObject newCharacter = Instantiate(character, spawnPosition, spawnRotation) as GameObject;
			newCharacter.transform.LookAt(planet.transform);
			newCharacter.transform.Rotate(-90, 0, 0);
		}
	}
}
