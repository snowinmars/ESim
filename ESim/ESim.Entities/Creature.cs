using ESim.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandS.Algorithm.CommonNamespace;
using SandS.Algorithm.Library.BitwiseNamespace;
using SandS.Algorithm.Library.PositionNamespace;
using System.Linq;

namespace ESim.Entities
{
    public class Creature
    {
        public Creature(Position position)
        {
            this.Position = position;
            this.Dna = new Dna();
            this.IsAlive = true;
            this.RefreshColor();
        }

        public Color Color { get; private set; }
        public Dna Dna { get; set; }
        public bool IsAlive { get; set; }
        public Position Position { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var texturePosition = this.Position * Configuration.CreatureTextureSize;

            spriteBatch.Draw(Configuration.DefaultCreatureTexture, texturePosition.ToVector2(), this.Color);

            if (Configuration.HaveToDrawText)
            {
                spriteBatch.DrawString(Configuration.DefaultApplicationFont, this.Dna.ToString(), texturePosition.ToVector2(), Color.White);
            }
        }

        public void Kill()
        {
            this.IsAlive = false;
        }

        public void Mutate()
        {
            if (!this.WillOrganismMutate())
            {
                return;
            }

            this.MutateImpl();
        }

        public void Spawn()
        {
            this.IsAlive = true;
        }

        public bool WillHaveChild()
        {
            return CommonValues.Random.Next(0, Configuration.WillHaveChildMax) == 0;
        }

        internal void RefreshColor()
        {
            int r = (int)BitwiseOperation.BitsToNumber(this.Dna.Values.Take(8).ToArray()); // TODO refactor
            int g = (int)BitwiseOperation.BitsToNumber(this.Dna.Values.Skip(8).Take(8).ToArray());
            int b = (int)BitwiseOperation.BitsToNumber(this.Dna.Values.Skip(16).Take(8).ToArray());

            this.Color = new Color(r, g, b, 255);
        }

        private void MutateImpl()
        {
            bool was = false;

            for (int i = 0; i < this.Dna.Values.Length; i++)
            {
                if (this.WillNucleotideMutate())
                {
                    this.Dna.Values[i] = !this.Dna.Values[i];
                    was = true;
                }
            }

            if (was)
            {
                this.RefreshColor(); // TODO refactor
            }
        }

        private bool WillNucleotideMutate()
        {
            return CommonValues.Random.Next(0, Configuration.WillNucleotideMutateMax) == 0;
        }

        private bool WillOrganismMutate()
        {
            return CommonValues.Random.Next(0, Configuration.WillOrganismMutateMax) == 0;
        }
    }
}