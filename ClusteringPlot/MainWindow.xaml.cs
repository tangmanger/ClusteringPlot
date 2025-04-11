using EasyPlot.Series;
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

namespace ClusteringPlot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int level = 1;
            int colum = 0;
            List<string> cols = new List<string>();
            List<ClusteringModel> clusteringModels = new List<ClusteringModel>();
            for (int i = 0; i < 3; i++)
            {
                ClusteringModel clusteringModel = new ClusteringModel();
                clusteringModel.Level = 1;
                if (i < 2)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (clusteringModel.Clustering == null)
                            clusteringModel.Clustering = new List<ClusteringModel>();
                        ClusteringModel childClusteringModel = new ClusteringModel();
                        childClusteringModel.Level = 2;
                        childClusteringModel.Name = "第二级" + j;
                        childClusteringModel.Col = colum;
                        childClusteringModel.Clustering = new List<ClusteringModel>();
                        if (j < 1)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                ClusteringModel childKClusteringModel = new ClusteringModel();
                                childKClusteringModel.Level = 3;
                                childKClusteringModel.Name = "第三极" + k;
                                childKClusteringModel.Col = colum;
                                childKClusteringModel.Clustering = new List<ClusteringModel>();
                                if (k < 2)
                                {
                                    for (int m = 0; m < 2; m++)
                                    {
                                        ClusteringModel childKClusteringModel1 = new ClusteringModel();
                                        childKClusteringModel1.Level = 4;
                                        childKClusteringModel1.Name = "第四极" + k;
                                        childKClusteringModel1.Col = colum;
                                        childKClusteringModel.Clustering.Add(childKClusteringModel1);
                                    }
                                }
                                childClusteringModel.Clustering.Add(childKClusteringModel);

                            }
                        }

                        clusteringModel.Clustering.Add(childClusteringModel);
                    }
                }
                else
                {
                    clusteringModel.Name = "第一级AAAA" + i;
                    clusteringModel.Col = colum;



                }
                clusteringModels.Add(clusteringModel);


            }
            ClusteringModel clusteringModel1 = new ClusteringModel();
            clusteringModel1.Clustering = clusteringModels;
            aa.MaxTreeLevel = 2;
            aa.Columns = cols;
            aa.Clustering = clusteringModel1;
        }
    }
}
