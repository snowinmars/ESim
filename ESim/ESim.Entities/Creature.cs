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
using SandS.Algorithm.Library.BitwiseNamespace;

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
            return CommonValues.Random.Next(0, Configuration.WillHaveChildMax) == 0;
        }

        public void Mutate()
        {
            if (!this.WillOrganismMutate())
            {
                return;
            }

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
                this.RefreshColor();
            }
        }

        internal void RefreshColor()
        {
            int r = (int)BitwiseOperation.BitsToNumber(this.Dna.Values.Take(8).ToArray());
            int g = (int)BitwiseOperation.BitsToNumber(this.Dna.Values.Skip(8).Take(8).ToArray());
            int b = (int)BitwiseOperation.BitsToNumber(this.Dna.Values.Skip(16).Take(8).ToArray());

            this.Color = new Color(r, g, b, 255);
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
