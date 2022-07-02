using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyzer
{
    class WordCounter
    {
        public string word { get; set; }
        public int frequency { get; set; }

        public WordCounter(string word, int frequency)
        {
            this.word = word;
            this.frequency = frequency;
        }
    }
}
