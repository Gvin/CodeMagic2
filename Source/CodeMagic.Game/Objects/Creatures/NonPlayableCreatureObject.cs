using System.Linq;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.CreaturesLogic;
using CodeMagic.Game.CreaturesLogic.Strategies;

namespace CodeMagic.Game.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : CreatureObject, INonPlayableCreatureObject
    {
        private string _logicPattern;

        protected NonPlayableCreatureObject()
        {
            Logic = new Logic();
            TurnsCounter = 0;
        }

        public float TurnsCounter { get; set; }

        public string LogicPattern
        {
            get => _logicPattern;
            set
            {
                _logicPattern = value;

                if (!string.IsNullOrEmpty(_logicPattern))
                {
                    var configurator = StandardLogicFactory.GetConfigurator(_logicPattern);
                    configurator.Configure(Logic);
                }
                else
                {
                    Logic.SetInitialStrategy(new StandStillStrategy());
                }
            }
        }

        public override int MaxHealth { get; set; }

        protected virtual float NormalSpeed => 1f;

        private float Speed
        {
            get
            {
                if (Statuses.Contains(FrozenObjectStatus.StatusType))
                {
                    return NormalSpeed * FrozenObjectStatus.SpeedMultiplier;
                }

                return NormalSpeed;
            }
        }

        protected Logic Logic { get; }

        public override void Update(Point position)
        {
            base.Update(position);

            TurnsCounter += 1;
            if (TurnsCounter >= Speed)
            {
                Logic.Update(this, position);
                TurnsCounter -= Speed;
            }
        }

        public virtual void Attack(Point position, Point targetPosition, IDestroyableObject target)
        {
        }

        public override void OnDeath(Point position)
        {
            base.OnDeath(position);

            var remains = GenerateRemains();
            if (remains != null)
            {
                CurrentGame.Map.AddObject(position, remains);
            }

            var loot = GenerateLoot();
            if (loot != null && loot.Any())
            {
                foreach (var item in loot)
                {
                    CurrentGame.Map.AddObject(position, item);
                }
            }
        }

        protected virtual IMapObject GenerateRemains()
        {
            return null;
        }

        protected virtual IItem[] GenerateLoot()
        {
            return null;
        }
    }
}