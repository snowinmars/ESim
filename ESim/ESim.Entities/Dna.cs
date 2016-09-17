using ESim.Config;
using System.Text;

namespace ESim.Entities
{
    public class Dna
    {
        public Dna()
        {
            this.Values = new bool[Configuration.DnaSize];
        }

        public bool[] Values { get; private set; }

        public void Reset()
        {
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Configuration.DnaSize);

            foreach (bool v in this.Values)
            {
                sb.Append(v ? "1" : "0");
            }

            return sb.ToString();
        }
    }
}