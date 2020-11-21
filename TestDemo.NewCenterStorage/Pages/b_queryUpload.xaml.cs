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
    /// b_queryUpload.xaml 的交互逻辑
    /// </summary>
    public partial class b_queryUpload : UserControl
    {
        public b_queryUpload()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = BLL.NewCenterStorage.Instance.GetFileInfo(tb_id.Text);
            if (result != null)
            {
                MessageBox.Show("查询文件信息成功");

            }
            else
            {
                MessageBox.Show("查询文件信息失败");

            }
        }
    }
}
