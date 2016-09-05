using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESim.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandS.Algorithm.CommonNamespace;
using SandS.Algorithm.Library.PositionNamespace;

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

        public Position Position { get; set; }

        public bool IsAlive { get; set; }

        public Dna Dna { get; set; }

        public void Kill()
        {
            this.IsAlive = false;
        }

        public void Spawn()
        {
            this.IsAlive = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var texturePosition = this.Position* Configuration.CreatureTextureSize;

            spriteBatch.Draw(Configuration.DefaultCreatureTexture, texturePosition.ToVector2(), this.Color);
            spriteBatch.DrawString(Configuration.DefaultApplicationFont, this.Dna.ToString(), texturePosition.ToVector2(), Color.White);
        }

        public Color Color { get; private set; }

        public bool WillHaveChild()
        {
            return CommonValues.Random.Next(0, 4) == 0;
        }

        public void Mutate()
        {
            if (!this.WillOrganismMutate())
            {
                return;
            }

            bool[] nucleotides = this.Dna.Values;
            bool was = false;

            for (int i = 0; i < nucleotides.Length; i++)
            {
                if (this.WillNucleotideMutate())
                {
                    nucleotides[i] = !nucleotides[i];
                    was = true;
                }
            }

            this.RefreshColor();
        }

        internal void RefreshColor()
        {
            int r = 0;

            for (int i = 0; i < 8; i++)
            {
                int v = this.Dna.Values[i] ? 1 : 0;
                r += v * (2 ^ i);
            }

            int g = 0;

            for (int i = 0; i < 8; i++)
            {
                int v = this.Dna.Values[i + 8] ? 1 : 0;
                g += v * (2 ^ i);
            }

            int b = 0;

            for (int i = 0; i < 8; i++)
            {
                int v = this.Dna.Values[i + 16] ? 1 : 0;
                b += v * (2 ^ i);
            }

            this.Color = new Color(r, g, b, 255);
        }

        private bool WillNucleotideMutate()
        {
            return CommonValues.Random.Next(0, 36) == 0;
        }

        private bool WillOrganismMutate()
        {
            return CommonValues.Random.Next(0, 16) == 0;
        }
    }
}
