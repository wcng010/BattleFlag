using System;
using System.Collections.Generic;
using System.Linq;
using C_Script.Component;
using C_Script.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace C_Script.Base.Abstract
{
    public abstract class Entity : MonoBehaviour
    {
        private readonly List<EntityComponent> _components = new List<EntityComponent>();
        //数据获取
        public EntityDataSo EntityDataSo => _entityDataSo ? _entityDataSo : _entityDataSo = Instantiate(entityDataSo);
        [SerializeField]private EntityDataSo _entityDataSo;
        [SerializeField]private EntityDataSo entityDataSo;
        
        
        //添加组件
        public void AddComponent(EntityComponent component)
        {
            if (!_components.Contains(component))
            {
                _components.Add(component);
            }
        }
        //获取组件
        public T GetCoreComponent<T>() where T : EntityComponent
        {
            //查找列表中符合要求的CoreComponent
            var comp = _components.OfType<T>().FirstOrDefault();

            if (comp)
                return comp;

            comp = GetComponentInChildren<T>();

            if (comp)
                return comp;

            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
            return null;
        }
        
        public abstract void Move(Vector3 pos);
        public abstract void Attack(Entity entity);

        public virtual HealthComponent GetHealthComponent => GetCoreComponent<HealthComponent>();

        public virtual RangeComponent GetRangeComponent => GetCoreComponent<RangeComponent>();
        public virtual AttackRangeComponent GetAttackRangeComponent => GetCoreComponent<AttackRangeComponent>();

        public virtual MoveComponent GetMoveComponent => GetCoreComponent<MoveComponent>();

        public virtual RayComponent GetRayComponent => GetCoreComponent<RayComponent>();
    }
}
