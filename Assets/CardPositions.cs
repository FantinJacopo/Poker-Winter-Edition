using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class CardPosition
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float xRotation { get; set; }
        public float yRotation { get; set; }
        public float zRotation { get; set; }
        /*public float xRotationDegrees => xRotation * Mathf.Rad2Deg;
        public float yRotationDegrees => yRotation * Mathf.Rad2Deg;
        public float zRotationDegrees => zRotation * Mathf.Rad2Deg;*/
        public CardPosition(float x, float y, float z, float xRotation, float yRotation, float zRotation)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.xRotation = xRotation;
            this.yRotation = yRotation;
            this.zRotation = zRotation;
        }
        public CardPosition(Vector3 position, Vector3 rotation)
        {
            x = position.x;
            y = position.y;
            z = position.z;
            xRotation = rotation.x;
            yRotation = rotation.y;
            zRotation = rotation.z;
        }
        public CardPosition(Vector3 position, Quaternion rotation)
        {
            x = position.x;
            y = position.y;
            z = position.z;
            xRotation = rotation.eulerAngles.x;
            yRotation = rotation.eulerAngles.y;
            zRotation = rotation.eulerAngles.z;
        }
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
        public Vector3 ToVectorRotation()
        {
            return new Vector3(xRotation, yRotation, zRotation);
        }
    }
}
