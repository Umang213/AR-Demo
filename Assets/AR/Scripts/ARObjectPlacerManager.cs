using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARObjectPlacerManager : MonoBehaviour {
	[Header("AR Components")]
	public ARRaycastManager arRaycastManager;
	public ARPlaneManager arPlaneManager;
	public Camera arCamera;

	[Header("Components")]
	public GameObject placementIndicator;
	public GameObject arInstructionPanel;
	public Button placeButton;
	public Button restartButton;
	public Button quitButton;


	[Header("Prefabs")]
	public GameObject objectPrefab;

	private GameObject instantiatedObject;
	internal Pose placementPose;
	internal bool isPlacementPoseValid = false;
	internal bool isObjectPlaced = false;

	private void Awake() {


		InitializeUI();
	}

	private void Update() {
		if (!isObjectPlaced) {
			HandlePlacement();
		}
	}

	private void InitializeUI() {
		arInstructionPanel.SetActive(true);
		placementIndicator.SetActive(false);
		placeButton.gameObject.SetActive(false);
		restartButton.gameObject.SetActive(false);
	}

	private void HandlePlacement() {
		UpdatePlacementPose();
		UpdatePlacementIndicator();

		if (isPlacementPoseValid) {
			placeButton.gameObject.SetActive(true);
		} else {
			placeButton.gameObject.SetActive(false);
		}
	}

	private void UpdatePlacementPose() {
		var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		var hits = new List<ARRaycastHit>();
		_ = arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

		isPlacementPoseValid = hits.Count > 0;

		if (isPlacementPoseValid) {
			placementPose = hits[0].pose;
			arInstructionPanel.SetActive(false);
			placementIndicator.SetActive(true);
		} else {
			arInstructionPanel.SetActive(true);
			placementIndicator.SetActive(false);
		}
	}

	private void UpdatePlacementIndicator() {
		if (isPlacementPoseValid) {
			placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
		}
	}



	public void AssignPrefabs(GameObject obj) {
		//AR_Animator.Instance.AssignTargetObject(obj);
		//AR_Scale.Instance.AssignTargetObject(obj);
		//AR_Rotation.Instance.AssignTargetObject(obj);
	}


}
