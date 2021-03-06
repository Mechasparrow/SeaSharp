using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SeaSharp_UI.Entities;

namespace SeaSharp_UI
{
    public enum WorldEventType
    {
        ENTITY_CHANGE,
        ENTITY_COLLISION
    }

    public class WorldEventArgs : EventArgs
    {
        public World OccuringWorld;
        public WorldEventType WorldEventType;
        public List<AbstractEntity> affectedEntites;
    }

    public class World
    {
        private List<AbstractEntity> entities;

        public event EventHandler<WorldEventArgs> WorldUpdated;

        public World()
        {
            entities = new List<AbstractEntity>();
        }

        public void AddEntity(AbstractEntity entity)
        {
            entities.Add(entity);

            entity.PropertyChanged += HandleEntityUpdate;
            WorldUpdated += entity.HandleWorldUpdate;

            EmitWorldUpdate(WorldEventType.ENTITY_CHANGE);
        }

        public void RemoveEntity(AbstractEntity entity)
        {
            entities.Remove(entity);

            entity.PropertyChanged -= HandleEntityUpdate;
            WorldUpdated -= entity.HandleWorldUpdate;

            entity.Destroy();

            EmitWorldUpdate(WorldEventType.ENTITY_CHANGE);
        }

        private void EmitWorldUpdate(WorldEventType worldEventType, List<AbstractEntity> abstractEntities = null) {

            EventHandler<WorldEventArgs> worldUpdated = WorldUpdated;

            WorldEventArgs worldEventArgs = new WorldEventArgs
            {
                OccuringWorld = this,
                WorldEventType = worldEventType,
                affectedEntites = abstractEntities != null ? abstractEntities : new List<AbstractEntity>()
            };

            if (worldUpdated != null)
            {
                worldUpdated.Invoke(this, worldEventArgs);
            }
        }



        public void HandleEntityUpdate(object sender, PropertyChangedEventArgs e)
        {
            if (sender.GetType() == typeof(Creature))
            {
                Creature creature = sender as Creature;
                AbstractEntity targetingEntity = creature.TargetingEntity;

                if (targetingEntity == null)
                {
                    return;
                }

                Rect targetRect = new Rect(new Point(targetingEntity.X, targetingEntity.Y), new Size(targetingEntity.EntitySize, targetingEntity.EntitySize));
                

                Rect creatureRect = new Rect(new Point(creature.X, creature.Y), new Size(creature.EntitySize, creature.EntitySize));
               

                bool collisionOccured = targetRect.IntersectsWith(creatureRect);
                
                if (collisionOccured)
                {

                    EmitWorldUpdate(WorldEventType.ENTITY_COLLISION, new List<AbstractEntity>()
                    {
                        creature,
                        targetingEntity
                    });
                }
            }
        }


        public List<ConsumableEntity> FindConsumableEntities()
        {
            return entities
                .Where(entity => entity.GetType().BaseType == typeof(ConsumableEntity))
                .Select(entity => entity as ConsumableEntity)
                .ToList();
        }

    }
}
