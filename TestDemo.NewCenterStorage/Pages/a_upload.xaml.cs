using IOTMP.Common.NewCenterStoreClient;
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
    /// a_upload.xaml 的交互逻辑
    /// </summary>
    public partial class a_upload : UserControl
    {
        public a_upload()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                var result = BLL.NewCenterStorage.Instance.UploadFile(tb_id.Text, ofd.FileName);

                if (result == 0)
                {
                    MessageBox.Show("上传成功");
                }
                else
                {
                    MessageBox.Show("上传失败");

                }
            }
        }
    }
}
