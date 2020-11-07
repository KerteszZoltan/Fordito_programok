using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Forditoprogramok
{
    class sourceHandler
    {
        private string path1, path2 = "";
        private string content = "";

        public string Path1
        {
            get { return this.path1; }
            set { this.path1 = value; }
        }

        public string Path2
        {
            get { return this.path2; }
            set { this.path2 = value; }
        }

        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        public sourceHandler(string path1, string path2)
        {
            this.path1 = path1;
            this.path2 = path2;
        }

        public void openFileToRead()
        {
            try
            {
                StreamReader SR = new StreamReader(File.OpenRead(this.path1));
                content = SR.ReadToEnd();

                //while (SR.Peek()>-1)
                //{
                //    s = SR.ReadLine();
                //}

                SR.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //replaceText("  "," ");
        public void replaceText(string from, string to) {
            while (content.Contains(from)) {
                content = content.Replace(from, to);
            }
        }


        List<string> symbolTable = new List<string>();
        int symbolIndex = 0;

        string changeVariableAndConstants(string varAndConstName)
        {
            symbolTable.Add(varAndConstName);
            symbolIndex += 1;
            string result = "00" + symbolIndex.ToString();
            return result.Substring(result.Length - 3);
        }

        Dictionary<string, string> fromTo = new Dictionary<string, string>();

       

        public void replaceContent()
        {


            var blockComment = @"/[*][\w\d\s]+[*]/";
            var lineComment = @"//.*?\n";

            string fromNum = @"([0-9]+)";
            string fromVar = @"([a-z-_]+)";

            content = Regex.Replace(content, blockComment, "  ");
            content = Regex.Replace(content, lineComment, "  ");


            content = Regex.Replace(content, fromNum, changeVariableAndConstants("$1"));
            content = Regex.Replace(content, fromVar, changeVariableAndConstants("$1"));

            foreach (var x in fromTo)
            {
                while (content.Contains(x.Key))
                {
                    content = content.Replace(x.Key, x.Value);
                }
            }

            fromTo.Add("\n", " ");
            fromTo.Add(" \n", " ");
            fromTo.Add("\n ", " ");
            fromTo.Add(@"\ ", " ");
            fromTo.Add("    ", " ");
            fromTo.Add(" {", "{");
            fromTo.Add("/", " ");
            fromTo.Add(" /", " ");
            fromTo.Add("/ ", " ");
            fromTo.Add(" }", "}");
            fromTo.Add("( ", "(");
            fromTo.Add("} ", "}");
            fromTo.Add("{ ", "{");
            fromTo.Add(" (", "(");
            fromTo.Add(" )", ")");
            fromTo.Add(") ", ")");
            fromTo.Add(" ;", ";");
            fromTo.Add("; ", ";");
            fromTo.Add("  ", " ");
            fromTo.Add(" =", "=");
            fromTo.Add("= ", "=");
            fromTo.Add("for", " 10 ");
            fromTo.Add("IF", " 11 ");
            fromTo.Add("while", " 12 ");
            fromTo.Add("case", " 13 ");
            fromTo.Add("switch", " 14 ");
            fromTo.Add("else", " 15 ");
            fromTo.Add("(", " 20 ");
            fromTo.Add("{", " 21 ");
            fromTo.Add(")", " 22 ");
            fromTo.Add("==", " 23 ");
            fromTo.Add("}", " 24 ");
            fromTo.Add("+", " 25 ");
            fromTo.Add("++", " 26 ");
            fromTo.Add("-", " 27 ");
            fromTo.Add("--", " 28 ");
            fromTo.Add("=", " 29 ");
            fromTo.Add("<", " 30 ");
            fromTo.Add("-=", " 31 ");
            fromTo.Add("+=", " 32 ");
            fromTo.Add("<=", " 33 ");
            fromTo.Add(">", " 34 ");
            fromTo.Add(">=", " 35 ");

            foreach (KeyValuePair<string, string> kvp in fromTo)
            {
                replaceText(kvp.Key, kvp.Value);
            }

        }
        public void openFileToWrite()
        {
            try
            {
                StreamWriter SW = new StreamWriter(File.Open(this.path2, FileMode.Create));
                SW.WriteLine(content);
                SW.Flush();
                SW.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
