using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Class
{
    public abstract class MovableSmileMan : ISmileMan
    {
        
        #region References
        [SerializeField] private GameObject ParentOfPoints;
        [SerializeField] private float velocity = 3f;
        #endregion
        
        #region private variables
        private point[] points;
        private bool isMoving = false;
        private int indexOfPoint = 0;
        #endregion
        private void Awake()
        {
            points = ParentOfPoints.GetComponentsInChildren<point>();
        }


        #region virtual methods
        protected virtual void GetMove()
        {
            isMoving = true;
        }

        protected virtual void GetSmileManMove()
        {
            if (!isMoving) return;
            transform.position = Vector3.MoveTowards(transform.localPosition, points[indexOfPoint].gameObject.transform.localPosition, velocity * Time.deltaTime);
        }

        protected virtual void HandleMovement()
        {
             if (!Mathf.Approximately(transform.position.x, points[indexOfPoint].gameObject.transform.position.x) ||
                 !Mathf.Approximately(transform.position.z, points[indexOfPoint].gameObject.transform.position.z)) return;
            
            isMoving = false;
            indexOfPoint = (indexOfPoint + 1) % points.Count();
        }
        #endregion
    }
}