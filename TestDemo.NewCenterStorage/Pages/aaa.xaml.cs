﻿using System;
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
    /// aaa.xaml 的交互逻辑
    /// </summary>
    public partial class aaa : UserControl
    {
        public aaa()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("图层元素被点击");
            e.Handled = true;

        }
    }
}
