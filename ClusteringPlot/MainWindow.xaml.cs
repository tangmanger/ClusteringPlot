using EasyPlot.Model;
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
    public class TestCluster : BaseClusterModel
    {
        public string Name { get; set; }


    }
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
            List<TestCluster> testClusters = new List<TestCluster>();
            int level = 1;
            int colum = 0;
            List<string> cols = new List<string>();
            List<ClusteringModel> clusteringModels = new List<ClusteringModel>();
            for (int i = 0; i < 5; i++)
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

                                    ClusteringModel childKClusteringModel1 = new ClusteringModel();
                                    childKClusteringModel1.Level = 4;
                                    childKClusteringModel1.Name = "第四极" + k;
                                    childKClusteringModel1.Col = colum;
                                    childKClusteringModel.Clustering.Add(childKClusteringModel1);
                                    testClusters.Add(new TestCluster() { Name = childKClusteringModel1.Name, Level = childKClusteringModel1.Level });

                                }
                                testClusters.Add(new TestCluster() { Name = childKClusteringModel.Name, Level = childKClusteringModel.Level });
                                childClusteringModel.Clustering.Add(childKClusteringModel);

                            }
                        }
                        testClusters.Add(new TestCluster() { Name = childClusteringModel.Name, Level = childClusteringModel.Level });
                        clusteringModel.Clustering.Add(childClusteringModel);
                    }
                }
                else
                {
                    clusteringModel.Name = "第一级AAAA" + i;
                    clusteringModel.Col = colum;
                    testClusters.Add(new TestCluster() { Name = clusteringModel.Name, Level = clusteringModel.Level });


                }
                clusteringModels.Add(clusteringModel);


            }
            ClusteringModel clusteringModel1 = new ClusteringModel();
            clusteringModel1.Clustering = clusteringModels;
            //aa.MaxTreeLevel = 2;
            //aa.Columns = cols;
            //aa.Clustering = clusteringModel1;
            //aa.ItemsSource = new List<ClusteringModel>() { clusteringModel1 };

            List<string> a = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                a.Add(i.ToString());
            }

            root.ItemsSource = testClusters.OrderBy(x=>x.Level).ToList();
        }
    }
}
