using ESim.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandS.Algorithm.Library.PositionNamespace;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ESim.Entities
{
    public class World
    {
        private readonly Queue<Creature> creatureReproductionQueue;
        public long generationCount { get; internal set; }

        public World(Color color)
        {
            this.Color = color;
            this.Size = Configuration.WorldSize;
            this.generationCount = 0;

            this.Creatures = this.InitCreatures();
            this.creatureReproductionQueue = new Queue<Creature>(this.Size.X * this.Size.Y);
        }

        public Color Color { get; set; }

        public IList<Creature> Creatures { get; private set; }

        public Position Size { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Configuration.DefaultWorldTexture, Vector2.Zero, this.Color);

            foreach (var creature in this.Creatures.Where(creature => creature.IsAlive))
            {
                creature.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            this.DestroyLessAdaptedCreatures();

            this.FillQueue();

            this.ProcessQueue();

            this.FillSpace();

            ++this.generationCount;
        }

        private static Dna HybridizateDna(Creature mother, Creature father, Creature child)
        {
            Dna dna = new Dna();

            for (int i = 0; i < child.Dna.Values.Length; i++)
            {
                dna.Values[i] = mother.Dna.Values[i] & father.Dna.Values[i];
            }

            return dna;
        }

        private static Creature MakeChild(Creature mother, Creature father)
        {
            Creature child = new Creature(mother.Position);
            child.Dna = HybridizateDna(mother, father, child);
            child.IsAlive = true;

            child.RefreshColor(); // TODO refactor

            return child;
        }

        private void DestroyLessAdaptedCreatures()
        {
            IList<Creature> lessAdaptedCreatures = this.Creatures
                                                        .OrderBy(c => ColorComparer.Compare(this.Color, c.Color)) // I can't use IComparer overload here: see comment in ColorComparer class
                                                        .ToList();

            for (int i = this.Creatures.Count - 1;
                    i > this.Creatures.Count - Configuration.KillCount;
                    i--)
            {
                lessAdaptedCreatures[i].Kill();
            }
        }

        private void FillQueue()
        {
            foreach (var creature in this.Creatures.Where(creature => creature.IsAlive))
            {
                creature.Mutate();

                if (this.Creatures.Any(c => !c.IsAlive))
                {
                    this.QueueChild(creature);
                }
            }
        }

        private void FillSpace()
        {
            IEnumerable<Creature> creatures = this.Creatures.Where(creature => !creature.IsAlive);

            foreach (var creature in creatures)
            {
                creature.Dna.Reset();
                creature.RefreshColor();
                creature.Spawn();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Position GetPosition(int value)
        {
            return new Position
            {
                X = value / Configuration.WorldSize.X,
                Y = value % Configuration.WorldSize.X
            };
        }

        private IList<Creature> InitCreatures()
        {
            int amount = this.Size.X * this.Size.Y;

            IList<Creature> creatures = new List<Creature>(amount);

            for (int i = 0; i < amount; i++)
            {
                Position position = this.GetPosition(i);

                Creature creature = new Creature(position);

                creatures.Add(creature);
            }

            return creatures;
        }

        private void ProcessQueue()
        {
            while (this.creatureReproductionQueue.Count > 2)
            {
                Creature father = this.creatureReproductionQueue.Dequeue();
                Creature mother = this.creatureReproductionQueue.Dequeue();

                for (int i = 0; i < Configuration.HowManyChildren; i++)
                {
                    Creature child = SpawnChild(father, mother);
                }

                if (!father.WillHaveChild())
                {
                    father.Kill();
                }

                if (!mother.WillHaveChild())
                {
                    mother.Kill();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void QueueChild(Creature creature)
        {
            this.creatureReproductionQueue.Enqueue(creature);
        }

        private Creature SpawnChild(Creature father, Creature mother)
        {
            Creature child = World.MakeChild(mother, father);

            for (int i = 0; i < Configuration.HowManyTimesCreatureMutateAfterBirth; i++)
            {
                child.Mutate();
            }

            Creature firstDead = this.Creatures.FirstOrDefault(c => !c.IsAlive);

            if (firstDead == null)
            {
                return null;
            }

            firstDead.Dna = child.Dna;
            firstDead.Spawn();
            firstDead.RefreshColor();

            return child;
        }
    }
}