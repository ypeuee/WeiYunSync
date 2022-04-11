using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MainApp.Views
{
    /// <summary>
    /// UcLogs.xaml 的交互逻辑
    /// </summary>
    public partial class UcLog : UserControl
    {
        public UcLog()
        {
            InitializeComponent();


            InitData();
        }

        private void InitData()
        {

            TreeViewOrg.ItemsSource = TreeviewDataInit.Instance.OrgList;
        }

    }


    public class TreeviewDataInit
    {
        private static TreeviewDataInit dataInit;

        public static TreeviewDataInit Instance
        {
            get
            {
                if (dataInit == null)
                    dataInit = new TreeviewDataInit();
                return dataInit;
            }
        }
        private TreeviewDataInit()
        {
            OrgList = new ObservableCollection<OrgModel>()
            {
                new OrgModel()
                {
                    //IsGrouping = true,
                    DisplayName = "同步于2022/4/8 14:59:38",
                    Children = new ObservableCollection<OrgModel>()
                    {
                        new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3

                    },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3
                    },
                },
                },
                new OrgModel()
                {
                    //IsGrouping = true,
                    DisplayName = "同步于2022/4/8 14:59:38",
                    Children = new ObservableCollection<OrgModel>()
                    {
                        new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3

                    },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count=3
                    },
                },
                }
            };
        }

        public ObservableCollection<OrgModel> OrgList { get; set; }

    }

    public class OrgModel
    {
        public bool IsGrouping { get { return Children != null && Children.Count > 0; } }
        public ObservableCollection<OrgModel> Children { get; set; }
        public string DisplayName { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public int Count { get; set; }
    }


    public class BoolToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
