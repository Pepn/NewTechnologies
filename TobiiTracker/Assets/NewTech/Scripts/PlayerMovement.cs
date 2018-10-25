using UnityEngine;
using Tobii.Gaming;


[RequireComponent(typeof(GazeAware))]
public class PlayerMovement : MonoBehaviour {

	public float speed;
	public Camera cam;

	GazeAware gazeAware;
	Transform transform;

	Vector3 vel;

	bool colliding = false;

	// Use this for initialization
	void Start () {
		
		gazeAware = GetComponent<GazeAware>();
		transform = GetComponent<Transform>();

	}
	
	// Update is called once per frame
	void Update () {

		if (!colliding)
			MouseMovement();

	}

	void OnCollisionEnter(Collision other)
	{
		gameObject.transform.position -= vel;
		colliding = true;
	}

	void OnCollisionExit(Collision other)
	{
		colliding = false;
	}

	void MouseMovement()
	{
		Vector2 mousePos = new Vector2();
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

       	Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
 		point.y = transform.position.y;

		vel = (point - transform.position);
		vel.Normalize();
		vel *= speed * Time.deltaTime;

		transform.position += vel;
	}

	void TobiiMovement()
	{
		// don't update velocity if player looks at the player
		if (gazeAware.HasGazeFocus) return;

		GazePoint gazePoint = TobiiAPI.GetGazePoint();
		if (gazePoint.IsRecent())
		{
			Vector2 screenPoint = gazePoint.Screen;
			Vector3 point = cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, cam.nearClipPlane));

			point.y = transform.position.y;

			Vector3 vel = (point - transform.position);
			vel.Normalize();
			vel *= speed * Time.deltaTime;

			transform.position += vel;
		}
	}

}
