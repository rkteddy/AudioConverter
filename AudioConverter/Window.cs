using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Arbingersys.Audio.Aumplib;

namespace AudioConverter
{
    public partial class Window : Form
    {

        private string inputFile = "Not selected";
        private string outputFile = "Not selected";

        private Aumpel audioConverter = new Aumpel();
        private Aumpel.soundFormat inputFileFormat;
        private Aumpel.soundFormat outputFileFormat;

       

        public Window()
        {
            InitializeComponent();
        }

        /*
         * 抛出异常
         */
         private void ShowExceptionMsg(Exception e)
         {
            MessageBox.Show("Exception: " + e.Message, "Exception!", MessageBoxButtons.OK);
         }
        
        /*
         * 选择转换源文件
         */ 
        protected void sourceFileButton_Click(object sender, EventArgs e)
        {
            // 打开文件选择窗口
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "MP3 (*.mp3)|*.mp3|" +
                              "WAV (*.wav)|*.wav|";
            openFile.FileName = "";

            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;

            if (openFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // 设置输入文件格式
            try
            {
                inputFileFormat = audioConverter.CheckSoundFormat(openFile.FileName);
            }
            catch (Exception ex)
            {
                ShowExceptionMsg(ex);
                return;
            }

        }

        protected void destFileButton_Click(object sender, System.EventArgs e)
        {
            // 打开文件选择窗口
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "MP3 (*.mp3)|*.mp3|" +
                              "WAV (*.wav)|*.wav|" +
                              "AIFF (*.aiff)|*.aiff|" +
                              "AU (*.au)|*.au|" +
                              "All Files (*.*)|*.*|";

            if (saveFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // 设置输出文件格式
            destFileLabel.Text = outputFile = saveFile.FileName;
        }
    }
}
