using UnityEngine;
using System.Collections;

public class Target {
	public TargetType targetType;
	public Checkpoint checkpoint;
	public Minion minion;
	
	public Target(Checkpoint checkpoint) {
		this.targetType = TargetType.checkpoint;
		this.checkpoint = checkpoint;
		this.minion = null;
	}
	public Target(Minion minion) {
		this.targetType = TargetType.minion;
		this.checkpoint = null;
		this.minion = minion;
	}
	
	public Vector3 GetPosition() {
		return targetType == TargetType.checkpoint ? checkpoint.transform.position : minion.transform.position;
	}
	
	public float GetDistance(Vector3 from) {
		return Vector3.Distance (from, GetPosition());
	}
	
	public enum TargetType { checkpoint, minion };
}

public class Checkpoint : MonoBehaviour {


}
