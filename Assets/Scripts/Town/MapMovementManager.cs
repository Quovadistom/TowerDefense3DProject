using UnityEngine;
using UnityEngine.EventSystems;

public class MapMovementManager : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 m_movementBounds;
    [SerializeField] private Vector2 m_scalingBounds;
    [SerializeField] private float m_moveAfterSeconds = 1;

    private float m_elapsedTime = 0;
    private Camera m_camera;
    private float m_cameraAngle;
    private Vector3 m_oldPosition;
    private float m_fingerDistance = 0;
    private bool m_resetRequired;
    private bool m_allowMove;

    private Vector3 m_calculatedPosition;
    private Vector3 m_calculatedScale;

    public void OnPointerDown(PointerEventData eventData)
    {
        m_allowMove = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_allowMove = false;
    }

    private void Awake()
    {
        m_camera = Camera.main;
        m_cameraAngle = m_camera.transform.parent.localEulerAngles.x;
    }

    private void Update()
    {
        if (!m_allowMove)
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || m_resetRequired)
            {
                m_oldPosition = m_camera.ScreenToWorldPoint(touch.position);
                m_resetRequired = false;
            }

            Vector3 newPosition = m_camera.ScreenToWorldPoint(touch.position);

            m_elapsedTime += Time.deltaTime;

            if (m_elapsedTime >= m_moveAfterSeconds && m_oldPosition != newPosition)
            {
                Vector3 deltaPosition = newPosition - m_oldPosition;
                float cameraAngleCompensation = Mathf.Cos(Mathf.Deg2Rad * (90 - m_cameraAngle));

                m_calculatedPosition = transform.position + new Vector3(deltaPosition.x, 0, Mathf.Sign(touch.deltaPosition.y) * new Vector2(deltaPosition.y, deltaPosition.z).magnitude / cameraAngleCompensation);
            }

            m_oldPosition = newPosition;
        }
        else if (Input.touchCount == 2)
        {
            Touch input1 = Input.GetTouch(0);
            Touch input2 = Input.GetTouch(1);

            if (input1.phase == TouchPhase.Began || input2.phase == TouchPhase.Began)
            {
                m_fingerDistance = Vector2.Distance(input1.position, input2.position);
            }

            float newDistance = Vector2.Distance(input1.position, input2.position);

            m_calculatedScale = transform.localScale + new Vector3(newDistance - m_fingerDistance, newDistance - m_fingerDistance, newDistance - m_fingerDistance) * 0.01f;

            m_fingerDistance = newDistance;

            if (input1.phase == TouchPhase.Ended || input2.phase == TouchPhase.Ended)
            {
                m_resetRequired = true;
            }
        }
        else
        {
            m_elapsedTime = 0;
        }


        transform.localScale = new Vector3(Mathf.Clamp(m_calculatedScale.x, m_scalingBounds.x, m_scalingBounds.y),
            Mathf.Clamp(m_calculatedScale.y, m_scalingBounds.x, m_scalingBounds.y),
            Mathf.Clamp(m_calculatedScale.z, m_scalingBounds.x, m_scalingBounds.y));

        transform.position = new Vector3(Mathf.Clamp(m_calculatedPosition.x, -m_movementBounds.x * transform.localScale.x, m_movementBounds.x * transform.localScale.x), 0,
                    Mathf.Clamp(m_calculatedPosition.z, -m_movementBounds.y * transform.localScale.x, m_movementBounds.y * transform.localScale.x));
    }
}
