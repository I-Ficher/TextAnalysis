using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace TextAnalyzer
{
    public partial class Form1 : Form
    {
        public bool change;
        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.DefaultExt = "*.txt";
            SaveFile.Filter = "Text Files(*.txt)|*.txt";
            if(SaveFile.ShowDialog()==System.Windows.Forms.DialogResult.OK && SaveFile.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(SaveFile.FileName, true))
                {
                    sw.WriteLine(richTextBox1.Text);
                    sw.Close();
                }
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(openFile.FileName);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (richTextBox1.Modified == true)
            {
                
                label1.Text = string.Format("Кол-во символов: {0}", richTextBox1.Text.Length.ToString().Split(' ', '\n', '\t'));

                int count = richTextBox1.Text.Split(' ', '\n', '\t').Length;
                
                label2.Text = string.Format("Кол-во слов: {0}",Convert.ToString(count));

                string wordWithMaxLength = string.Empty;
                string[] words = richTextBox1.Text.Split(' ','\n', '\t');
                for (int i = 0; i < words.Length; i++)
                    if (words[i].Length > wordWithMaxLength.Length)
                        wordWithMaxLength = words[i];
               
                label3.Text = string.Format(wordWithMaxLength);

                
                int[] words_freq = new int[words.Length];           //это массив из целых чисел. индекс массива это номер слова в тексте. пока пустой
                for (int i = 0; i < words.Length; i++)              //цикл с количеством пробегов, равным числу слов в тексте
                {
                    if (words_freq[i] >= 0)                         //если число повторений слова неотрицательно, то... (объяснение отрицательных значений см ниже)
                    {
                        words_freq[i] = 1;                          //ставим 1, потому что слово новое и повторяется минимум 1 раз
                        for (int j = i + 1; j < words.Length; j++)  //сравниваем слово с каждым следующим словом в тексте
                        {
                            if (words[i] == words[j])               //если слова одинаковые, то
                            {
                                words_freq[i]++;                    //счетчик повторений увеличивает число на 1
                                words_freq[j] = -99;                //слово-повтор учитывать в дальнейшем не нужно, для исключения из учета ему присваивается
                            }                                       //отрицательное значение
                        }
                    }
                }
                int[] words_top = new int[10];                      //это массив, содержащий в себе топ-10 повторений (само их количество)
                string[] words_toplist = new string[10];            //это массив, содержащий в себе слова из топ-10 (соотносится с words_top)
                int temp = int.MaxValue;
                bool flag = true;
                for (int i = 0; i < 10; i++)                        //заходим в цикл 10 раз для выявления 10 самых частых слов
                {
                    for (int j = 0; j < words_freq.Length; j++)     //каждый раз пробегаем полное количество слов
                    {
                        if (words_freq[j] > words_top[i] && words_freq[j] <= temp)           //если слово по частоте попадается больше, чем то, которое в топе уже сидит, то
                        {
                            if (i == 0 && words[j] != "")                             //для первого слова просто
                            {
                                words_top[i] = words_freq[j];       //ставим новое наибольшее число в words_top и
                                words_toplist[i] = words[j];        //ставим соответствующее слово в toplist
                            }
                            else
                            {                                       //для всех последующих слов
                                for (int k = 0; k < i; k++)         //в этом цикле проверяем, чтобы слова не совпадали, чтобы не поместить одно и то же слово дважды
                                {
                                    if (words[j] == words_toplist[k])   //если слова совпали, то
                                    {
                                        flag = false;                   //флаг становится ложью и
                                        continue;                       //цикл заканчивается
                                    }
                                }
                                if (flag == true && words[j] != "")                       //если флаг остался правдой, то
                                {
                                    words_top[i] = words_freq[j];       //ставим следующее по списку наибольшее число в words_top и
                                    words_toplist[i] = words[j];        //ставим соответствующее слово в toplist
                                }
                            }
                            flag = true;
                        }
                    }
                    temp = words_top[i];

                }
                label4.Text = string.Format("Слово {0} встречается {1} раз\nСлово {2} встречается {3} раз\nСлово {4} встречается {5} раз\nСлово {6} встречается {7} раз\nСлово {8} встречается {9} раз\nСлово {10} встречается {11} раз\nСлово {12} встречается {13} раз\nСлово {14} встречается {15} раз\nСлово {16} встречается {17} раз\nСлово {18} встречается {19} раз", words_toplist[0], words_top[0], words_toplist[1], words_top[1], words_toplist[2], words_top[2], words_toplist[3], words_top[3], words_toplist[4], words_top[4], words_toplist[5], words_top[5], words_toplist[6], words_top[6], words_toplist[7], words_top[7], words_toplist[8], words_top[8], words_toplist[9], words_top[9]);


                /*
                string all_text = string.Join("", chars);
                char[] alltext = all_text.ToCharArray();
                
                
                for (int x = 0; x < alltext.Length; x++)
                {
                    if (chr[0] == alltext[x])
                    {
                        
                    }
                }*/
                /*string[] chars = richTextBox1.Text.Length.ToString().Split(' ', '\n', '\t');
                int char_count = 0;
                string ch = textBox1.Text;
                char[] chr = ch.ToCharArray();
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    if (richTextBox1.Lines[i].Contains(ch)) // проверяем содержится символ в данной строке
                    {
                        char_count++;
                    }
                }
                double percent = 100 * char_count / chars.Length;
                // if (chars==textBox1.Text)
                //textBox1.Text
                label5.Text = string.Format("Символ {0} составляет {1}% от текста", ch, Convert.ToString(percent));*/


            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = richTextBox1.Text;
            if (str.IndexOf(textBox1.Text) != -1)
            {
                
                richTextBox1.SelectionStart = str.IndexOf(textBox1.Text);
                richTextBox1.SelectionLength = textBox1.Text.Length;
                richTextBox1.SelectionBackColor = Color.Red;
                
            }
            else
            {
                MessageBox.Show("Не обнаружено!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Transparent;
        }
    }
}
