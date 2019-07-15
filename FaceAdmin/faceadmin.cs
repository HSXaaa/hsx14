using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace FaceAdmin
{
    public partial class faceadmin : Form
    {
        private string file;
        private string path;
        private string base64str;
        public object getJson { get; private set; }

        public faceadmin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //设置识别回调地址
            string faceurl = equipurl.Text;
            string callbackUrl = callbackurl.Text;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("callbackUrl", callbackUrl);
            string aa = HttpUitls.DoPost(faceurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }

        private string GetResponseString(HttpWebResponse httpWebResponse)
        {
            throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //获取设备序列号
            string url = eip.Text + ":8090/getDeviceKey";
            //将接口传入，这个HttpUitls的类，有兴趣可以研究下，也可以直接用就可以，不用管如何实现。
            string getJson = HttpUitls.Get(url);
            Root rt = JsonConvert.DeserializeObject<Root>(getJson);
            equipnum.Text = rt.data;

        }


        private void equipnum_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //创建人员
            string createurl = eip.Text + ":8090/person/create";
            string id = personid.Text;
            string name = uname.Text;
            string idcardNum = idcardnum.Text;
            Person ps = new Person();
            ps.id = id;
            ps.name = name;
            ps.idcardNum = idcardNum;
            string createperson = JsonConvert.SerializeObject(ps, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("person", createperson);
            string aa = HttpUitls.DoPost(createurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //删除人员
            string createurl = eip.Text + ":8090/person/delete";
            string id = personid.Text;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("id", id);
            string aa = HttpUitls.DoPost(createurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //查询人员
            string findurl = eip.Text + ":8090/person/find";
            string id = personid.Text;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("id", id);
            string aa = HttpUitls.DoPost(findurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //查询人脸
            string faceurl = eip.Text + ":8090/face/find";
            string personId = personid.Text;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("personId", personId);
            string aa = HttpUitls.DoPost(faceurl, ht);  //HttpRequest是自定义的一个类
            //MessageBox.Show(aa);
            Facepic pic = JsonConvert.DeserializeObject<Facepic>(aa);
            //MessageBox.Show(pic);
            for (int i = 0; i < pic.data.Count; i++)
            {
                string strImageURL = pic.data[i].path;
                //MessageBox.Show(strImageURL);
                string savepath = @"D:\Fx\" + pic.data[i].personId + ".jpg";
                WebClient mywebclient = new WebClient();
                try
                {
                    mywebclient.DownloadFile(strImageURL, savepath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                MessageBox.Show("照片查询成功");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //选择照片
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹"; //窗体标题
            dialog.Filter = "图片文件(*.jpg,*.png)|*.jpg;*.png"; //文件筛选
            //默认路径设置为我的电脑文件夹
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file= dialog.FileName;//文件夹路径
                path = file.Substring(file.LastIndexOf("\\") + 1); //格式化处理，提取文件名
                                                                    //File.Copy(file, Application.StartupPath + "\\cai\\" + lujin, false); //复制到的目录切记要加文件名！！！
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;  //使图像拉伸或收缩，以适合PictureBox
                this.PictureBox1.ImageLocation = file;

                //图片转为base64编码的字符串
                Bitmap bmp = new Bitmap(file);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                string base64string = Convert.ToBase64String(arr);
                base64str = base64string;
                //MessageBox.Show(base64string);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //创建人脸
            string faceurl = eip.Text + ":8090/face/create";
            string personId = personid.Text;
            string faceId = personid.Text;
            string base64string = base64str;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("personId", personId);
            ht.Add("faceId", faceId);
            ht.Add("imgBase64", base64string);
            string aa = HttpUitls.DoPost(faceurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //更新人脸
            string faceurl = eip.Text + ":8090/face/update";
            string personId = personid.Text;
            string faceId = personid.Text;
            string base64string = base64str;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("personId", personId);
            ht.Add("faceId", faceId);
            ht.Add("imgBase64", base64string);
            string aa = HttpUitls.DoPost(faceurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //删除人脸
            string faceurl = eip.Text + ":8090/face/delete";
            string faceId = personid.Text;
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            ht.Add("faceId", faceId);
            string aa = HttpUitls.DoPost(faceurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //远程开门
            string faceurl = eip.Text + ":8090/device/openDoorControl";
            Hashtable ht = new Hashtable();
            ht.Add("pass", "12345678");
            string aa = HttpUitls.DoPost(faceurl, ht);  //HttpRequest是自定义的一个类
            MessageBox.Show(aa);
        }
    }
}


