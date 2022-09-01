

namespace Chernobog.Studio.Common
{
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;
    using System;
    
    public static class Utils
    {
        public static float Abs(float x) {
            return x >= 0 ? x : -x;
        }

        public static float2 Abs(float2 x)
        {
            x.x = x.x >= 0 ? x.x : -x.x;
            x.y = x.y >= 0 ? x.y : -x.y;
            return x;
        }
        
        public static float2[] BoundValuesFromCamera(Camera camera) {
            var screenAspect = Screen.width / (float)Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            return new float2[] { (Vector2)camera.transform.position, new float2(cameraHeight * screenAspect, cameraHeight) };
        }
        
        public static Bounds TransformBounds(this Transform _transform, Bounds _localBounds)
        {
            var center = _transform.TransformPoint(_localBounds.center);

            // transform the local extents' axes
            var extents = _localBounds.extents;
            var axisX = _transform.TransformVector(extents.x, 0, 0);
            var axisY = _transform.TransformVector(0, extents.y, 0);
            var axisZ = _transform.TransformVector(0, 0, extents.z);

            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }
        
        // Is Mouse over a UI Element? Used for ignoring World clicks through UI
        public static bool IsPointerOverUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true;
            } else {
                var pe = new PointerEventData(EventSystem.current);
                pe.position =  Input.mousePosition;
                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll( pe, hits );
                return hits.Count > 0;
            }
        }

        public static float4 ColorToFloat(Color color)
        {
            return new float4(
                color.r,
                color.g,
                color.b,
                color.a);
        }
        
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            var vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            var worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        
        // Generate random normalized direction
        public static Vector3 GetRandomDir() {
            return new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
        }
        

        public static Vector3 GetVectorFromAngle(int angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        public static int GetAngleFromVector(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static int GetAngleFromVector180(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, Vector3 vecRotation) {
            return ApplyRotationToVector(vec, GetAngleFromVectorFloat(vecRotation));
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
            return Quaternion.Euler(0,0,angle) * vec;
        }
        
        public static FunctionUpdater CreateMouseDraggingAction(Action<Vector3> onMouseDragging) {
            return CreateMouseDraggingAction(0, onMouseDragging);
        }
        
        
        // Get UI Position from World Position
        public static Vector2 GetWorldUIPosition(Vector3 worldPosition, Transform parent, Camera uiCamera, Camera worldCamera) {
            Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            Vector3 uiCameraWorldPosition = uiCamera.ScreenToWorldPoint(screenPosition);
            Vector3 localPos = parent.InverseTransformPoint(uiCameraWorldPosition);
            return new Vector2(localPos.x, localPos.y);
        }

        public static Vector3 GetWorldPositionFromUIZeroZ() {
            Vector3 vec = GetWorldPositionFromUI(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        // Get World Position from UI Position
        public static Vector3 GetWorldPositionFromUI() {
            return GetWorldPositionFromUI(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI(Camera worldCamera) {
            return GetWorldPositionFromUI(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    
        public static Vector3 GetWorldPositionFromUI_Perspective() {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Camera worldCamera) {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Vector3 screenPosition, Camera worldCamera) {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0f));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }

        public static FunctionUpdater CreateMouseDraggingAction(int mouseButton, Action<Vector3> onMouseDragging) {
            bool dragging = false;
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    dragging = true;
                }
                if (Input.GetMouseButtonUp(mouseButton)) {
                    dragging = false;
                }
                if (dragging) {
                    onMouseDragging(GetMouseWorldPosition());
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickFromToAction(Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition) {
            return CreateMouseClickFromToAction(0, 1, onMouseClickFromTo, onWaitingForToPosition);
        }

        public static FunctionUpdater CreateMouseClickFromToAction(int mouseButton, int cancelMouseButton, Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition) {
            int state = 0;
            Vector3 from = Vector3.zero;
            return FunctionUpdater.Create(() => {
                if (state == 1) {
                    if (onWaitingForToPosition != null) onWaitingForToPosition(from, GetMouseWorldPosition());
                }
                if (state == 1 && Input.GetMouseButtonDown(cancelMouseButton)) {
                    // Cancel
                    state = 0;
                }
                if (Input.GetMouseButtonDown(mouseButton) && !IsPointerOverUI()) {
                    if (state == 0) {
                        state = 1;
                        from = GetMouseWorldPosition();
                    } else {
                        state = 0;
                        onMouseClickFromTo(from, GetMouseWorldPosition());
                    }
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickAction(Action<Vector3> onMouseClick) {
            return CreateMouseClickAction(0, onMouseClick);
        }

        public static FunctionUpdater CreateMouseClickAction(int mouseButton, Action<Vector3> onMouseClick) {
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    onMouseClick(GetWorldPositionFromUI());
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateKeyCodeAction(KeyCode keyCode, Action onKeyDown) {
            return FunctionUpdater.Create(() => {
                if (Input.GetKeyDown(keyCode)) {
                    onKeyDown();
                }
                return false; 
            });
        }
        
        
        // Screen Shake
        public static void ShakeCamera(float intensity, float timer) {
            Vector3 lastCameraMovement = Vector3.zero;
            FunctionUpdater.Create(delegate () {
                timer -= Time.unscaledDeltaTime;
                Vector3 randomMovement = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
                Camera.main.transform.position = Camera.main.transform.position - lastCameraMovement + randomMovement;
                lastCameraMovement = randomMovement;
                return timer <= 0f;
            }, "CAMERA_SHAKE");
        }
    }
}