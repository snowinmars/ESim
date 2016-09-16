using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Threading;
using ESim.Config;
using Mathos.Converter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandS.Algorithm.CommonNamespace;
using SandS.Algorithm.Library.PositionNamespace;

namespace ESim.Entities
{
    public static class ColorComparer
    {
        public static double Compare(Color lhs, Color rhs)
        {
            int dR = lhs.R - rhs.R;
            int dG = lhs.G - rhs.G;
            int dB = lhs.B - rhs.B;

            return Math.Sqrt(dR * dR * 0.2126 + dG * dG * 0.7152 + dB * dB * 0.0722);
        }
    }

    public static class A
    {
        public static T ClosestTo<T>(this IList<T> inputCollection, T target)
            where T : IComparable
        {
            T closest = default(T);
            int minDifference = int.MaxValue;
            foreach (var element in inputCollection)
            {
                int difference = element.CompareTo(target);

                if (minDifference > difference)
                {
                    minDifference = (int)difference;
                    closest = element;
                }
            }

            return closest;
        }

        public static KeyValuePair<T, int> ClosestTo<T>(this IList<T> inputCollection, T target, IComparer<T> comparer)
        {
            T closest = default(T);
            int minDifference = int.MaxValue;

            int pos = 0;

            for (int i = 0; i < inputCollection.Count; i++)
            {
                T element = inputCollection[i];

                int difference = comparer.Compare(element, target);

                if (minDifference > difference)
                {
                    minDifference = (int) difference;
                    closest = element;
                    pos = i;
                }
            }

            return new KeyValuePair<T, int>(closest, pos);
        }
    }

    public class World
    {
        private void DestroySomeCreatures()
        {
            var oc = this.Creatures.OrderBy(c => ColorComparer.Compare(this.Color, c.Color)).ToList();

            for (int i = this.Creatures.Count - 1; i > this.Creatures.Count - Configuration.KillCount; i--)
            {
                oc[i].Kill();
            }
        }

        private readonly Queue<Creature> creatureReproductionQueue;

        public World(Color color)
        {
            this.Color = color;
            this.Size = Configuration.WorldSize;

            this.Creatures = this.InitCreatures();
            this.creatureReproductionQueue = new Queue<Creature>(this.Size.X * this.Size.Y);
        }

        private IList<Creature> InitCreatures()
        {
            int v = this.Size.X * this.Size.Y;

            List<Creature> creatures = new List<Creature>(v);

            for (int i = 0; i < v; i++)
            {
                Position position = this.GetPosition(i);

                var creature = new Creature(position);

                creatures.Add(creature);
            }

            return creatures;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Position GetPosition(int i)
        {
            string a = i.From(Base.Base10).To(Base.Base4);

            if (a.Length == 1)
            {
                a = "0" + a;
            }

            char l = a[0];
            char r = a[1];

            int x = Int32.Parse(new string(l, 1));
            int y = Int32.Parse(new string(r, 1));

            return new Position(x, y);
        }

        public Position Size { get; set; }

        public IList<Creature> Creatures { get; private set; }

        public Color Color { get; set; }

        public void Update(GameTime gameTime)
        {
            this.DestroySomeCreatures();

            this.FillQueue();

            this.ProcessQueue();

            this.FillSpace();
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
                creature.Spawn();
            }
        }

        private void ProcessQueue()
        {
            while (this.creatureReproductionQueue.Count > 2)
            {
                Creature father = this.creatureReproductionQueue.Dequeue();
                Creature mother = this.creatureReproductionQueue.Dequeue();
                Creature child = World.MakeChild(mother, father);

                var firstDead = this.Creatures.First(c => !c.IsAlive);
                firstDead.Dna = child.Dna;
                firstDead.Spawn();
                firstDead.RefreshColor();

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

        private static Creature MakeChild(Creature mother, Creature father)
        {
            var child = new Creature(mother.Position);

            child.Dna = new Dna();

            for (int i = 0; i < child.Dna.Values.Length; i++)
            {
                child.Dna.Values[i] = mother.Dna.Values[i] | father.Dna.Values[i];
            }

            child.RefreshColor();

            child.IsAlive = true;

            return child;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void QueueChild(Creature creature)
        {
            this.creatureReproductionQueue.Enqueue(creature);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Configuration.DefaultWorldTexture, Vector2.Zero, this.Color);

            foreach (var creature in this.Creatures.Where(creature => creature.IsAlive))
            {
                creature.Draw(gameTime, spriteBatch);
            }
        }
    }
}
