using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace LrcChecker {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            string path;
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog()) {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
                path = dialog.FileName;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var lrc in Directory.EnumerateFiles(path, "*.lrc", SearchOption.AllDirectories)) {
                using (StreamReader sr = new StreamReader(lrc,Encoding.Default, true)) {
                    var nothing = sr.ReadToEnd();
                    if (nothing.EndsWith(Environment.NewLine)) {
                        sb.AppendLine("最後の行が空白! : " + lrc);
                    }
                   if (sr.CurrentEncoding != Encoding.UTF8) {
                        sb.AppendLine("UTF8ではない!: " + lrc);
                        continue;
                   }
                }
                List<string> tester = new List<string>();
                foreach (var line in File.ReadLines(lrc, Encoding.Default)) {
                    tester.Add(line.Remove(0, 10));
                }
                int total = tester.Count();
                if (String.IsNullOrEmpty(tester[0])) {
                    sb.AppendLine("最初の行が空行: " + lrc);
                }

                for (int i = 0; i < total; i++) {                  
                    if (tester[i] != tester[i].Trim()) {
                        sb.AppendLine("第" + (i + 1) + "行の前後に空白がある" + lrc + " " + tester[i]);
                    }
                }
               // sb.AppendLine(lrc);
            }
            if (sb.Length == 0) {
                sb.AppendLine("おめでとうございます");
            }
            string content = sb.ToString();
            string desktoppath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            File.WriteAllText(Path.Combine(desktoppath, "report.txt"), content, Encoding.UTF8);
            MessageBox.Show("complete");
        }



        void checkfirstline() {

        }

        void checknotover38byte() {

        }

        void checknotendwithblank() {

        }
    }
}
