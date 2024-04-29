using UnityEngine;

namespace C_Script.Component
{
    public class RayComponent:EntityComponent
    {
        [SerializeField] private Transform footPoint;

        private Vector3 _point = Vector3.zero;
        //向脚底发射射线，返回检测地板的方块名字
        public string GetGridName()
        {
            var ray = new Ray(footPoint.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 2f,1<<LayerMask.NameToLayer("Ground")))
            {
                return hitInfo.collider.transform.parent.name;
            }
            return default;
        }
        
        public RaycastHit GetFootGridHit()
        {
            Ray ray = new Ray(footPoint.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 2f,1<<LayerMask.NameToLayer("Ground")))
            {
                return hitInfo;
            }
            return default;
        }
        
        public RaycastHit GetGridUpDown(Vector3 point)
        {
            Vector3 temp = new Vector3(point.x, point.y+5, point.z);
            RaycastHit hitInfo;
            Ray ray = new Ray(temp, Vector3.down);
            if (Physics.Raycast(ray, out hitInfo, 10f,1<<LayerMask.NameToLayer("friend")))
            {
                return default;
            }
            if (Physics.Raycast(ray, out hitInfo, 10f, 1 << LayerMask.NameToLayer("Enemy")))
            {
                return default;
            }
            if (Physics.Raycast(ray, out hitInfo, 10f,1<<LayerMask.NameToLayer("Ground")))
            {
                return hitInfo;
            }
            return default;
        }
        
        public RaycastHit GetPlayerGrid(Vector3 point)
        {
            Vector3 temp = new Vector3(point.x, point.y+5, point.z);
            RaycastHit hitInfo;
            Ray ray = new Ray(temp, Vector3.down);
            if (Physics.Raycast(ray, out hitInfo, 10f,1<<LayerMask.NameToLayer("Ground")))
            {
                return hitInfo;
            }
            return default;
        }


        public RaycastHit GetEmptyGrid(Vector3 point)
        {
            int randomX = Random.value < 0.5 ? -1 : 1;
            int randomY = Random.value < 0.5 ? -1 : 1; 
            
            Vector3 temp = new Vector3(point.x+randomX*2.41f, point.y+5, point.z+randomY*2.19f);
            RaycastHit hitInfo;
            Ray ray = new Ray(temp, Vector3.down);
            if (Physics.Raycast(ray, out hitInfo, 10f,1<<LayerMask.NameToLayer("friend")))
            {
                return default;
            }
            if (Physics.Raycast(ray, out hitInfo, 10f, 1 << LayerMask.NameToLayer("Enemy")))
            {
                return default;
            }
            if (Physics.Raycast(ray, out hitInfo, 10f, 1 << LayerMask.NameToLayer("Ground")))
            {
                return hitInfo;
            }
            return default;
        }



        private void Update()
        {
            if (_point != Vector3.zero)
            {
                Debug.DrawLine(_point,_point+Vector3.up*5f,Color.red);
            }
        }
    }
}