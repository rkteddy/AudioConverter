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
        private string outputFile = "Not set";

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
         * 输出格式选择
         */
        private void formatSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //outputFileFormat = formatSelectionBox.SelectedItem.ToString();
        }

        /*
         * 选择转换源文件
         */ 
        private void sourceFileButton_Click(object sender, EventArgs e)
        {
            // 打开选择文件窗口
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "MP3 (*.mp3)|*.mp3|WAV (*.wav)" + "*.wav|All Files (*.*)|*.*";
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
    }
}
