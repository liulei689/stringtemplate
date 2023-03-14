

using JinianNet.JNTemplate;
using StoneCodeGenerator.Lib;
using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
           comboBox1.DataSource = Directory.GetFiles("模板");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //测试1143424
            Engine.UseInterpretationEngine();
            Engine.Configure((c) =>
            {
                c.OutMode = OutMode.Auto;
            });
            var ddd = File.ReadAllText("Template\\1.txt");
            var template = Engine.CreateTemplate(ddd);
            List<sss> values = new List<sss>();
            var sss = File.ReadAllLines("3.txt");
            for (int i = 0; i < sss.Length; i++)
            {
                string dsdsa = sss[i].Replace("\t\t", "\t");
                string[] ddd3423 = dsdsa.Split("\t");
                if (ddd3423[1] == "String") ddd3423[1] = ddd3423[1].ToLower();
                if (ddd3423[1] == "Boolean") ddd3423[1] = "bool";
                values.Add(new sss { type = ddd3423[1], Name = ddd3423[0], despriton = ddd3423[2] });
            }
            template.Set("list", values);
            template.Set("ZhushiS", textBox1.Text);
            template.Set("Zhushi", textBox2.Text);
            template.Set("Name", textBox3.Text);
            var result = template.Render();
            File.WriteAllText("2.txt", result);
            Process vProcess = Process.Start("notepad.exe", "Template\\2.txt");
        }
        private void button1_Click2()
        {
            Engine.UseInterpretationEngine();
            Engine.Configure((c) =>
            {
                c.OutMode = OutMode.Auto;
            });
            var ddd = File.ReadAllText("C:\\Users\\guangbo\\Desktop\\logs.html");
            var template = Engine.CreateTemplate(ddd);
            List<htmllog> values = new List<htmllog>();
            var sss = File.ReadAllLines("logshonghui.txt");
            int falg = 0;
            for (int i = 0; i < sss.Length; i++)
            {
                if (sss[i].Contains("---"))
                {
                    falg = i;
                    string[] miancontent = sss[i-falg].Split('-');//基础信息
                    var db =new htmllog();
                    db.Vseon = "1";
                    for (int m = 0; m < 2 * falg - i; m++)
                    {
                        db.Detail.Add(sss[i - falg + 1]);
                    }
                    if(db!=null)
                    values.Add(db);
                }
                if (i == 0)
                {
                    string[] miancontent = sss[i].Split('-');
                    template.Set("Year", miancontent[0]);
                    template.Set("Day", "12332");
                    template.Set("Name", "12332");
                    continue;
                }
                string dsdsa = sss[i].Replace("\t\t", "\t");
                string[] ddd3423 = dsdsa.Split("\t");
            
            }
            template.Set("list", values);

            template.Set("Name", "12332");
            var result = template.Render();
            File.WriteAllText("C:\\Users\\guangbo\\Desktop\\logs2.html", result);
        }
        public class htmllog
        {
            public string? Day { get; set; }
            public string? Vseon { get; set; }
            public string? Vseodescr { get; set; }
            public List<string>? Detail { get; set; }

        }

        public class sss
        {
            public string? type { get; set; }
            public string? Name { get; set; }
            public string? despriton { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process vProcess = Process.Start("notepad.exe", "3.txt");
        }

        private  async void textBox4_TextChanged(object sender, EventArgs e)
        {
            //texts.Text =await StoneCode.GMethod(textBox4.Text);
    
        }
   

        private void button3_Click(object sender, EventArgs e)
        {

           // Clipboard.SetDataObject(result);
        }
    }
}