using UnityEngine;

public class Tile : MonoBehaviour {
    private Vector3 _targetPosition;
    private Vector3 _correctPosition;
    private SpriteRenderer _sprite;

    private void Start() {
        _targetPosition = transform.position;
        _correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.05f);
        //_sprite.color = (_targetPosition == _correctPosition) ? Color.green : Color.white;
    }

    public Vector3 GetTargetPosition() => _targetPosition;
    public void SetTargetPosition(Vector3 position) => _targetPosition = position;
}
