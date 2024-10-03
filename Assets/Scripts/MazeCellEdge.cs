using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {

	public MazeCell cell, otherCell;

	public MazeDirection direction;
	[SerializeField] GameObject mapMarker;

	public virtual void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}
    public virtual void OnPlayerEntered() { ShowMapMarker(); }

    public virtual void OnPlayerExited() { }

	public void ShowMapMarker()
	{
        if (mapMarker != null) mapMarker.SetActive(true);
    }
}