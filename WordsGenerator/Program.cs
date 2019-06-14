using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace SharpTest
{
    class Program
    {
        static List<string> name = new List<string>();
        static Dictionary<string, int> count = new Dictionary<string, int>();
        static Random r = new Random();
        static void WriteString(BinaryWriter bw, string s)
        {
            for (int i = 0; i < s.Length; i++)
                bw.Write(s[i]);
            bw.Write('\0');
        }
        static string ReadString(BinaryReader br)
        {
            string s = "";
            char c;
            while ((c = br.ReadChar()) != '\0')
                s += c;
            return s;
        }
        static void WriteInMemory()
        {
            using (FileStream file = new FileStream("bin", FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(file))
                foreach (var v in count)
                {
                    WriteString(bw, v.Key);
                    bw.Write(v.Value);
                }
        }
        static void ReadFromMemory()
        {
            count.Clear();
            using (FileStream file = new FileStream("bin", FileMode.Open))
            using (BinaryReader br = new BinaryReader(file))
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    count.Add(ReadString(br), br.ReadInt32());
                }
        }

        static void Add(string s)
        {
            if (!count.ContainsKey(s))
                count.Add(s, 1);
            else
                count[s]++;
        }
        static void CalculateCombinations(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                string s,b;
                while ((s = r.ReadLine()) != null)
                {
                    s = s.ToLowerInvariant();
                    b = "";
                    int i = 0;
                    while (i<s.Length && 'а' <= s[i] && s[i] <= 'я')
                        b += s[i++];
                    name.Add(b);
                }
            }
            for (int i = 0; i < name.Count; i++)
            {
                if (name[i].Length > 0)
                    Add(name[i][0] + "");
                if (name[i].Length > 1)
                    Add(name[i][0] + "" + name[i][1]);
                for (int j = 2; j < name[i].Length; j++)
                    Add(name[i][j - 2] + "" + name[i][j - 1] + "" + name[i][j]);
            }
            List<string> list = new List<string>();
            foreach (var x in count)
                if (x.Value < 3)
                    list.Add(x.Key);
            foreach (string s in list)
                count.Remove(s);
        }
        static int Get(string s)
        {
            if (count.ContainsKey(s))
                return count[s];
            else
                return 0;
        }
        static string Next(string res)
        {

            int s = 0;
            for (char c = 'а'; c <= 'я'; c++) 
                s += Get(res + c);
            if (s < 5)
                return null;
            s = r.Next() % s + 1;
            for (char c = 'а'; c <= 'я'; c++)
                if (s > Get(res + c))
                    s -= Get(res + c);
                else
                    return res + c;
            return null;
        }
        static string Generate(int l)
        {
            string buf = "", s = "";
            for (int i = 0; i < l; i++)
            {
                if ((buf = Next(buf)) == null)
                    return null;
                s += buf[buf.Length - 1];
                if (buf.Length == 3)
                    buf = buf[1] + "" + buf[2];
            }
            return s;
        }
        static string NextWord()
        {
            int l = 4 + r.Next() % 10;
            string ans = null;
            while ((ans = Generate(l)) == null) ;
            return ans;
        }
        static void ShowDictionary()
        {
            foreach(var v in count)
            {
                Console.WriteLine(v.Key + " " + v.Value);
            }
        }
        static void Main(string[] args)
        {
            //CalculateCombinations("dictionary.txt");
            //CalculateCombinations("States.txt");            
            //CalculateCombinations("Tolkin.txt");            
            //WriteInMemory();
            ReadFromMemory();
            PrintInFile("Output.txt");
            for (int i = 0; i < 40; i++)
            {
                Console.WriteLine(NextWord());
            }
        }
        static void PrintInFile(string path)
        {
            int Count = 10000;
            using (StreamWriter r = new StreamWriter(path,false))
            {
                for (int i = 0; i < Count; i++)
                    r.WriteLine(NextWord());
            }
        }
    }
}
