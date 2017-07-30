using System;
using System.IO;
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

        public static int soundFileSize = 0;

        public Window()
        {
            InitializeComponent();
        }

        /*
         * 转换进度
         */
         private static void ReportStatus(int totalBytes,
                                          int processedBytes,
                                          Aumpel aumpelObj)
        {
            convertProgressBar.Value =
                (int)(((float)processedBytes / (float)totalBytes) * 100);
        }

        /*
         * 解码进度
         */
         private static bool ReportStatusMad(uint frameCount, uint byteCount,
                                             ref MadlldlibWrapper.mad_header mh)
        {
            convertProgressBar.Value =
                (int)(((float)byteCount / (float)soundFileSize) * 100);

            return true;
        }
        /*
         * 抛出异常
         */
         private void ShowExceptionMsg(Exception e)
         {
            MessageBox.Show("Exception: " + e.Message, "Exception!",
                             MessageBoxButtons.OK);
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

        private void convertButton_Click(object sender, EventArgs e)
        {

            // 设置转换格式
            switch (formatBox.SelectedItem.ToString())
            {
                case "WAV":
                    outputFileFormat = Aumpel.soundFormat.WAV;
                    break;
                case "MP3":
                    outputFileFormat = Aumpel.soundFormat.MP3;
                    break;
                case "AU":
                    outputFileFormat = Aumpel.soundFormat.AU;
                    break;
                case "AIFF":
                    outputFileFormat = Aumpel.soundFormat.AIFF;
                    break;
                default:
                    MessageBox.Show("You must select a valid type to convert to.",
                                    "Error",
                                    MessageBoxButtons.OK);
                    return;
            }

            // 转换为MP3（Aumpel库）
            if ((int)outputFileFormat == (int)Aumpel.soundFormat.MP3)
            {
                try
                {
                    Aumpel.Reporter defaultCallback = new Aumpel.Reporter(ReportStatus);

                    audioConverter.Convert(inputFile, (int)inputFileFormat,
                                           outputFile, (int)outputFileFormat,
                                           defaultCallback);

                    convertProgressBar.Value = 0;

                    destFileLabel.Text = outputFile = "";
                    sourceFileLabel.Text = inputFile = "";

                    MessageBox.Show("Conversion finished.",
                                    "Done",
                                    MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    ShowExceptionMsg(ex);
                    return;
                }
            }

            // MP3转换为其他（Madlldlib库）
            else if ((int)inputFileFormat == (int)Aumpel.soundFormat.MP3)
            {
                try
                {
                    MadlldlibWrapper.Callback defaultCallback =
                        new MadlldlibWrapper.Callback(ReportStatusMad);

                    // 确定文件大小
                    FileInfo f = new FileInfo(inputFile);
                    soundFileSize = (int)f.Length;

                    audioConverter.Convert(inputFile,
                                           outputFile,
                                           outputFileFormat,
                                           defaultCallback);
                    convertProgressBar.Value = 0;

                    destFileLabel.Text = outputFile = "";
                    sourceFileLabel.Text = inputFile = "";

                    MessageBox.Show("Conversion finifshed.",
                                     "Done.", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    ShowExceptionMsg(ex);
                    return;
                }
            }

            // 源文件非MP3的转换
            else
            {
                try
                {
                    Aumpel.Reporter defaultCallback =
                        new Aumpel.Reporter(ReportStatus);

                    audioConverter.Convert(inputFile, (int)inputFileFormat,
                                           outputFile, (int)(outputFileFormat | Aumpel.soundFormat.PCM_16),
                                           defaultCallback);

                    convertProgressBar.Value = 0;

                    destFileLabel.Text = outputFile = "";
                    sourceFileLabel.Text = inputFile = "";

                    MessageBox.Show("Conversion finished.",
                                    "Done.",
                                    MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    ShowExceptionMsg(ex);
                    return;
                }
            }
        }

        // 运行
        public static void Main()
        {
            Application.Run(new Window());
        }
    }
}
