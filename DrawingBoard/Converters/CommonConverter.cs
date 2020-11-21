using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace DrawingBoard.Converters
{
    public class CommonCoverter : IValueConverter
    {
        /// 转换器参数语法: key1,value1 key2,value2 ...   [other,defaultValue] [object>bool] [contains(:)]
        ///               -----------1----------------   -------2------------ ------3------ ---4---------
        /// 1:必填,为键值对,用空格分隔,不限键值对个数
        /// 2:选填,如果你想要1转成true,其他转成false: 1,true other,false ;也可以1和2转成true,其他转成flase: 1,true 2,true other,false
        /// 3:选填,像DataTrigger的value是object类型的,  如果用了此转换器需要手动指定转换类型. 例如  object>visibility 1,visible 2,hidden 
        /// 4:选填,判断字符串中是否包含，只有在string -> bool 时才生效
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Binding.DoNothing;
            #region 参数转dictionary
            string ObjectType = null;
            Dictionary<string, string> PStrDic = new Dictionary<string, string>();
            object result = null;
            var PStr = parameter as string ?? string.Empty;
            if (!((value is DateTime) && targetType == typeof(string)))
            {
                if (targetType == typeof(object) && Regex.IsMatch(PStr, @"object\>(\w+)"))
                    ObjectType = Regex.Match(PStr, @"object\>(\w+)").Groups[1].Value;
                if ((targetType == typeof(bool) || targetType == typeof(bool?) || ObjectType == "bool") && value is string v_str)
                {
                    var r = v_str.Contains(Regex.Match(PStr, @"\((.*)\)").Groups[1].Value);
                    return r;
                }
                foreach (Match m in Regex.Matches(PStr, @"(\w+)?\,[^\s]+"))
                {
                    var p_key = Regex.Match(m.Value, @"^(\w+)?").Value;
                    var p_value = Regex.Match(m.Value, @"(?<=\,)[^\s]+$").Value;
                    PStrDic.Add(p_key, p_value);
                }
            }
            #endregion
            //brush
            if (targetType == typeof(System.Windows.Media.Brush) || targetType == typeof(System.Windows.Media.SolidColorBrush) || ObjectType == "brush")
            {
                if (PStrDic.ContainsKey(value.ToString().ToLower()))
                {
                    var temp = PStrDic[value.ToString().ToLower()];
                    if (Regex.IsMatch(temp, @"^RS_"))
                    {
                        result = Application.Current.TryFindResource(Regex.Replace(temp, @"^RS_", m => string.Empty));
                    }
                    else if (Regex.IsMatch(temp, @"^#"))
                    {
                        result = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(temp));
                    }
                    else
                    {
                        //result = typeof(System.Drawing.Brushes ).GetProperties(System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic)
                    }
                }
                else
                {
                    result = PStrDic.Any(t => t.Key.ToLower() == "other") ? PStrDic.Single(t => t.Key.ToLower() == "other").Value : (object)Brushes.Transparent;
                }
            }

            // to visibility
            if (targetType == typeof(Visibility) || targetType == typeof(Visibility?) || ObjectType == "visibility")
            {
                if (PStrDic.ContainsKey(value.ToString().ToLower()))
                {
                    var temp = PStrDic[value.ToString().ToLower()].ToLower();
                    Visibility visibilityResult = Visibility.Collapsed;
                    switch (temp)
                    {
                        case "visible":
                            visibilityResult = Visibility.Visible; break;
                        case "hidden":
                            visibilityResult = Visibility.Hidden; break;
                        case "collapsed":
                            visibilityResult = Visibility.Collapsed; break;
                        default:
                            break;
                    }

                    return visibilityResult;
                }
                else
                {
                    result = PStrDic.Any(t => t.Key.ToLower() == "other") ? PStrDic.Single(t => t.Key.ToLower() == "other").Value : (object)Visibility.Collapsed;

                }
            }
            // to int
            if (targetType == typeof(int) || targetType == typeof(int?) || ObjectType == "int")
            {
                if (PStrDic.ContainsKey(value.ToString().ToLower()))
                {
                    var temp = PStrDic[value.ToString().ToLower()].ToLower();
                    result = int.Parse(temp);
                }
                else
                {
                    result = PStrDic.Any(t => t.Key.ToLower() == "other") ? PStrDic.Single(t => t.Key.ToLower() == "other").Value : (object)0;
                }
            }
            // to bool
            if (targetType == typeof(bool?) || targetType == typeof(bool) || ObjectType == "bool")
            {
                if (PStrDic.ContainsKey(value.ToString().ToLower()))
                {
                    var temp = PStrDic[value.ToString().ToLower()].ToLower();
                    result = bool.Parse(temp);
                }
                else
                {
                    result = PStrDic.Any(t => t.Key.ToLower() == "other") ? PStrDic.Single(t => t.Key.ToLower() == "other").Value : (object)false;
                }
            }
            // to string
            if (targetType == typeof(string) || ObjectType == "string")
            {
                if (PStrDic.ContainsKey(value.ToString()))
                {
                    var temp = PStrDic[value.ToString()];
                    result = temp;
                }
                else
                {
                    result = PStrDic.Any(t => t.Key.ToLower() == "other") ? PStrDic.Single(t => t.Key.ToLower() == "other").Value : value;
                }
            }
            //datetime
            if (targetType == typeof(string) && (value is DateTime dt))
            {
                if (dt == DateTime.MinValue)
                {
                    result = string.Empty;
                }
                else
                {
                    result = PStrDic.Any(t => t.Key.ToLower() == "other") ? PStrDic.Single(t => t.Key.ToLower() == "other").Value : (object)dt;
                }
            }


            return result ?? Binding.DoNothing;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
