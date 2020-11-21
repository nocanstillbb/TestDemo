using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace TestDemo.NewCenterStorage.Pages
{
    /// <summary>
    /// c_downloadfile_small.xaml 的交互逻辑
    /// </summary>
    public partial class c_downloadfile_small : UserControl
    {
        public c_downloadfile_small()
        {
            InitializeComponent();
        }
        //下载到本地文件
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new SaveFileDialog();
            if (ofd.ShowDialog() == true)
            {
                var result = BLL.NewCenterStorage.Instance.DownloadFile(tb_id.Text, ofd.FileName);

                if (result == 0)
                {
                    MessageBox.Show("下载成功");
                }
                else
                {
                    MessageBox.Show("下载失败");

                }
            }

        }

        //直接返回byte数组
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var result = BLL.NewCenterStorage.Instance.DownloadFileRtByte(tb_id.Text);

            if (result?.Any() == true)
            {
                MessageBox.Show("下载成功");
            }
            else
            {
                MessageBox.Show("下载失败");


            }

        }

        //流回调
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            var ofd = new SaveFileDialog();
            if (ofd.ShowDialog() == true)
            {
                var result = BLL.NewCenterStorage.Instance.DownloadFileByStream(tb_id.Text, ofd.FileName);

                if (result == 0)
                {
                    MessageBox.Show("下载成功");
                }
                else
                {
                    MessageBox.Show("下载失败");

                }
            }
        }
    }
}
