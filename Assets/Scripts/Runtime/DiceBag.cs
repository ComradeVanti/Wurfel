using System.Collections.Generic;
using System.Linq;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceBag
    {

        private readonly Dictionary<string, int> countsByDiceName;


        private IEnumerable<string> DiceNames =>
            countsByDiceName.Keys.Append("Normal");


        private DiceBag(Dictionary<string, int> countsByDiceName) =>
            this.countsByDiceName = countsByDiceName;

        public static DiceBag MakeDefault() =>
            new DiceBag(new Dictionary<string, int>
            {
                { "Boom", 5 },
                { "Bad", 5 },
                { "Bouncy", 5 }
            });

        public string GetRandom()
        {
            var diceName = DiceNames.Random();

            if (countsByDiceName.ContainsKey(diceName))
            {
                countsByDiceName[diceName]--;

                if (countsByDiceName[diceName] == 0)
                    countsByDiceName.Remove(diceName);
            }

            return diceName;
        }

    }

}