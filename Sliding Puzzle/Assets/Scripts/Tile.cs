using UnityEngine;

public class Tile : MonoBehaviour {
    public int id;
    private bool _inRightPlace = false;
    private Vector3 _targetPosition;
    private Vector3 _correctPosition;
    private SpriteRenderer _sprite;

    private void Awake() {
        _targetPosition = transform.position;
        _correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.05f);
        _sprite.color = (_targetPosition == _correctPosition) ? Color.green : Color.white;
        _inRightPlace = (_targetPosition == _correctPosition);
    }

    public Vector3 GetTargetPosition() => _targetPosition;
    public bool IsInRightPlace() => _inRightPlace;
    public void SetTargetPosition(Vector3 position) => _targetPosition = position;
}
