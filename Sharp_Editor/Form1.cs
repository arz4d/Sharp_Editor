
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Security.AccessControl;
namespace Sharp_Editor
{
    public partial class Form1 : Form
    {

        string path = "noPath";
        string encytpedpath = "";
        Task[] tasks = new Task[1];
      
        int xx = 0;

       
        public Form1()
        {
            InitializeComponent();
            newTabCreate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
       
        public void newTabCreate()
        {
            TabPage tp = new TabPage();
            RichTextBox rtb = new RichTextBox();
            tp.AutoScroll = true;
            tabControl1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            rtb.WordWrap = false;
            Label mainlabel = new Label();
            Label lb1 = new Label();
            Label lb2 = new Label();
            Label lb3 = new Label();
            Label lb4 = new Label();
            rtb.Dock = DockStyle.Fill;
            mainlabel.Dock = DockStyle.Bottom;
            lb1.Dock = DockStyle.Left;
            lb2.Dock = DockStyle.Left;
            lb3.Dock = DockStyle.Left;
            lb4.Dock = DockStyle.Left;
            mainlabel.Text = "Total Counts ";
            lb1.Text = "Word Count is : 0";
            lb2.Text = "  Line Count is : 0";
            lb3.Text = "Letter Count is : 0";
            lb4.Text = "Total letters : 0";
            mainlabel.Controls.Add(lb1);
            mainlabel.Controls.Add(lb2);
            mainlabel.Controls.Add(lb3);
            mainlabel.Controls.Add(lb4);
            tp.Controls.Add(mainlabel);
            tp.Controls.Add(rtb);
            tp.Text = "untitled *";
            tabControl1.TabPages.Add(tp);
            rtb.Select();
            rtb.TextChanged += rtb_TextChanged;
        }

       




        public void closeTab()
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        public RichTextBox myrtb()
        {
            try
            {
                TabPage tp = tabControl1.SelectedTab;
                RichTextBox rtb = tp.Controls[1] as RichTextBox;
                return rtb;
            }
            catch (InvalidOperationException ee)
            {
                Console.WriteLine(ee);
            }
            catch (IndexOutOfRangeException ee)
            {
                Console.WriteLine(ee);
            }
            return null;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void encryptFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Users\AnverMAM\Desktop";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            try
            {

                byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(myrtb().Text);
                string encryptedText = Convert.ToBase64String(toEncodeAsBytes);
                if (!(tp.Text).Contains("Encrypted"))
                {
                    if ((tp.Text).Contains("untitled *"))
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string newFilename = saveFileDialog1.FileName;
                            String[] substrings = newFilename.Split('.');
                            newFilename = substrings[0];
                            newFilename += "Encrypted.";
                            newFilename += substrings[1];
                            File.WriteAllText(newFilename, encryptedText);
                            path = newFilename;
                            encytpedpath = path;
                            String[] substrings2 = path.Split('\\');
                            int noofStrings = substrings2.Length;
                            tp.Text = substrings2[noofStrings - 1];



                        }
                    }
                    else
                    {
                        MessageBox.Show("Dont Encrypt an existing normal file !. Create a new one");
                    }

                }
                else
                {
                    if (tp.Tag != null)
                    {
                        encytpedpath = (string)tp.Tag;
                    }
                    File.WriteAllText(encytpedpath, encryptedText);
                }

            }

            catch (NullReferenceException ee)
            {
                Console.WriteLine(ee);
            }

        }

        private void autosave(TabPage tp, string text)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (!tp.Text.Equals("untitled *"))
            {
                try
                {

                    path = (string)tp.Tag;
                    File.WriteAllText(path, text);


                }
                catch (ArgumentException ee)
                {
                    Console.WriteLine(ee);
                }
                catch (NullReferenceException ee)
                {
                    Console.WriteLine(ee);
                }
                catch (IOException ee)
                {
                    Console.WriteLine(ee);
                }

            }

        }

       

        private void saveAll(TabPage tabpg, string text)
        {

            try
            {
                Parallel.ForEach(text, tabControl1 =>
                 {
                     try
                     {
                         path = (string)tabpg.Tag;
                         File.WriteAllText(path, text);
                     }
                     catch(IOException ee)
                     {
                         Console.WriteLine(ee);
                     }
                 });
            }
            catch (IOException aaa)
            {
                Console.WriteLine(aaa);
            }


        }
        

        /*
           private void saveAll(TabPage tabpg, string text)
        {
            try
            {
            

                path = (string)tabpg.Tag;
                File.WriteAllText(path, text);
            }
            catch (ArgumentNullException aaa)
            {
                Console.WriteLine(aaa);
            }

        }

             */


        public delegate void closeTabsDelegate();

     
       

        public delegate void spellCheckDelegate();
        private void spellCheck()
        {

            if (this.InvokeRequired)
            {

                this.Invoke(new spellCheckDelegate(spellchecker));
            }
            else
            {

                spellchecker();
            }

        }
        private void spellchecker()
        {
            TabPage tp = tabControl1.SelectedTab;
            Label mainlabel = tp.Controls[0] as Label;
            string lastWord = myrtb().Text.Split(' ').Last();
            lastWord = lastWord.Replace("\n", string.Empty);

            string[] output = lastWord.Split(' ');

            string a = "";

            string[] words = myrtb().Text.Split(' ');
            int countWords = lastWord.Count(x => { return !Char.IsWhiteSpace(x); });
            int countwordstotal = 0;
            try
            {
                foreach (string firstletter in output)
                {

                    a = firstletter[0].ToString();
                    a = a.ToUpper();
                }
                StreamReader sr = new StreamReader(@"C:\Users\AnverMAM\Desktop\words.txt");
                Parallel.For(0, words.Length, i =>
                  {
                 
                
                    countwordstotal += words[i].Length;   //(int i = 0; i < words.Length; i++)

                  });
            mainlabel.Controls[3].Text = "Total Letters: " + countwordstotal;
                myrtb().SelectionStart = countwordstotal + words.Length;
                myrtb().SelectionLength = countWords;
                myrtb().SelectionColor = Color.Red;
                if (countWords > 1)
                {
                    string curval;
                    string chk = "false";

                    while ((curval = sr.ReadLine()) != null)
                    {

                        if (countWords == curval.Length && curval.Equals(lastWord, StringComparison.InvariantCultureIgnoreCase) && curval != null)
                        {

                            myrtb().SelectionStart = countwordstotal - countWords + words.Length - 1;

                            myrtb().SelectionLength = countWords;

                            int covr = myrtb().SelectionLength;
                            myrtb().SelectionColor = Color.Black;
                            string nnn = myrtb().SelectedText;
                            myrtb().SelectionStart = countwordstotal + words.Length;
                            chk = "true";

                            break;

                        }
                    }

                    if (chk.Equals("false"))
                    {
                        myrtb().SelectionStart = countwordstotal - countWords + words.Length - 1;

                        myrtb().SelectionLength = countWords;
                        int covr2 = myrtb().SelectionLength;
                        myrtb().SelectionColor = Color.Red;

                        myrtb().SelectionStart = countwordstotal + words.Length;

                    }

                }


            }
            catch (NullReferenceException ee)
            {
                Console.WriteLine(ee);
            }
            catch (IndexOutOfRangeException ee)
            {
                Console.WriteLine(ee);

            }
            catch (FileNotFoundException ee)
            {
                Console.WriteLine(ee);
            }
            catch (ArgumentNullException ee)
            {
                Console.WriteLine(ee);
            }
            catch (ArgumentException ee)
            {
                Console.WriteLine(ee);

            }
            catch (DirectoryNotFoundException ee)
            {
                Console.WriteLine(ee);

            }


        }




      
        private void sToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            TabPage tp = tabControl1.SelectedTab;
            if ((tp.Text).Equals("untitled *"))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, myrtb().Text);
                    path = saveFileDialog1.FileName;
                    tp.Tag = path;

                    String[] substrings = path.Split('\\');
                    int noofStrings = substrings.Length;
                    tp.Text = substrings[noofStrings - 1];
                    File.WriteAllLines(path, myrtb().Lines);

                }
            }
            else
            {
                try
                {
                    path = (string)tp.Tag;
                    File.WriteAllText(path, myrtb().Text);
                    File.WriteAllLines(path, myrtb().Lines);

                }
                catch (ArgumentException ee)
                {
                    Console.WriteLine(ee);
                }
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void undoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            myrtb().Undo();
        }

        private void redoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            bool vall = myrtb().CanRedo;
            myrtb().Redo();
        }

        private void boldToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string styleoffont = myrtb().SelectionFont.Style.ToString();

            if (styleoffont.Equals("Bold"))
            {
                myrtb().SelectionFont = new Font(myrtb().Font, FontStyle.Regular);
            }
            else
            {
                myrtb().SelectionFont = new Font(myrtb().Font, FontStyle.Bold);
            }
        }

        private void italicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string styleoffont = myrtb().SelectionFont.Style.ToString();

            if (styleoffont.Equals("Italic"))
            {
                myrtb().SelectionFont = new Font(myrtb().Font, FontStyle.Regular);
            }
            else
            {
                myrtb().SelectionFont = new Font(myrtb().Font, FontStyle.Italic);
            }
        }

        private void underlineToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string styleoffont = myrtb().SelectionFont.Style.ToString();

            if (styleoffont.Equals("Underline"))
            {
                myrtb().SelectionFont = new Font(myrtb().Font, FontStyle.Regular);
            }
            else
            {
                myrtb().SelectionFont = new Font(myrtb().Font, FontStyle.Underline);
            }
        }

        private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            myrtb().Copy();
        }

        private void pasteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            myrtb().Paste();
        }

        private void cutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            myrtb().Cut();
        }

        private void selectAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            myrtb().SelectAll();
        }

        private void clearToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            myrtb().Text = "";
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\";

            openFileDialog1.Title = "Open text Files";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            TabPage tp = tabControl1.SelectedTab;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                if (openFileDialog1.FileName.Contains("Encrypted"))
                {
                    MessageBox.Show("You are trying to open an encrypted file");
                    string val = sr.ReadToEnd();
                    byte[] encodedDataAsBytes = Convert.FromBase64String(val);
                    string decryptedValue = ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
                    myrtb().Text = decryptedValue;
                    path = openFileDialog1.FileName;
                    tp.Tag = path;
                    String[] substrings = path.Split('\\');
                    int noofStrings = substrings.Length;
                    tp.Text = substrings[noofStrings - 1];
                    sr.Close();
                }
                else
                {
                    myrtb().Text = sr.ReadToEnd();
                    path = openFileDialog1.FileName;
                    tp.Tag = path;

                    String[] substrings = path.Split('\\');
                    int noofStrings = substrings.Length;
                    tp.Text = substrings[noofStrings - 1];
                    File.ReadAllLines(path);

                    sr.Close();
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                newTabCreate();
            }
            catch (ArgumentOutOfRangeException ee)
            {
                Console.WriteLine(ee);
            }
            catch (ArgumentException ee)
            {
                Console.WriteLine(ee);
            }
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void saveAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            TabControl.TabPageCollection tabcollection = tabControl1.TabPages;
            foreach (TabPage tabpage in tabcollection)
            {
                if (!(tabpage.Text).Equals("untitled *"))
                {
                    RichTextBox rtb = tabpage.Controls[1] as RichTextBox;
                    string text = rtb.Text;
                    var t2 = new Task(() => saveAll(tabpage, text));
                    t2.Start();
                    Task.WaitAll(t2);
                }
                else
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        path = saveFileDialog1.FileName;
                        tabpage.Tag = path;

                        String[] substrings = path.Split('\\');
                        int noofStrings = substrings.Length;
                        tabpage.Text = substrings[noofStrings - 1];
                        RichTextBox rtbnew = tabpage.Controls[1] as RichTextBox;
                        string text = rtbnew.Text;
                        File.WriteAllText(saveFileDialog1.FileName, myrtb().Text);
                        File.WriteAllLines(path, myrtb().Lines);

                    }
                }
            }
        }



        /*
          private void saveAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            TabControl.TabPageCollection tabcollection = tabControl1.TabPages;
            foreach (TabPage tabpage in tabcollection)
            {
                if (!(tabpage.Text).Equals("untitled *"))
                {
                    RichTextBox rtb = tabpage.Controls[1] as RichTextBox;
                    string text = rtb.Text;
                    var t2 = new Task(() => saveAll(tabpage, text));
                    t2.Start();
                    Task.WaitAll(t2);
                }
                else
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        path = saveFileDialog1.FileName;
                        tabpage.Tag = path;

                        String[] substrings = path.Split('\\');
                        int noofStrings = substrings.Length;
                        tabpage.Text = substrings[noofStrings - 1];
                        RichTextBox rtbnew = tabpage.Controls[1] as RichTextBox;
                        string text = rtbnew.Text;
                        File.WriteAllText(saveFileDialog1.FileName, myrtb().Text);
                        File.WriteAllLines(path, myrtb().Lines);

                    }
                }
            }
        }

             
             */

        private void rtb_TextChanged(object sender, EventArgs e)
        {
            TabPage tp = tabControl1.SelectedTab;
            Label mainlabel = tp.Controls[0] as Label;

            int numberoflines = myrtb().Lines.Count();

            string lastWord = myrtb().Text.Split(' ').Last();
            lastWord = lastWord.Replace("\n", string.Empty);

            int countWords = lastWord.Count(x => { return !Char.IsWhiteSpace(x); });

            mainlabel.Controls[0].Text = "Letter Count : " + countWords;
            mainlabel.Controls[1].Text = "  Line Count : " + numberoflines;
            mainlabel.Controls[2].Text = "Word Count : " + Regex.Matches(((RichTextBox)sender).Text, @"[\S]+").Count.ToString();

            if (xx != countWords)
            {
                xx = countWords;
                tasks[0] = Task.Run(() => spellCheck());

                string text = myrtb().Text;
                var t1autoSave = new Task(() => autosave(tp, text));
                t1autoSave.Start();

                Task.WaitAll(t1autoSave);

            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sharp Editor V 1.0 by ARZ4D (CB006647)");
        }
    }
}
