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
            string key = Guid.NewGuid().ToString();
            List<ClusteringModel> clusteringModels = new List<ClusteringModel>();
            for (int i = 0; i < 5; i++)
            {
                ClusteringModel clusteringModel = new ClusteringModel();
                clusteringModel.Level = 1;
                string uid2 = Guid.NewGuid().ToString();
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
                        string uid1 = Guid.NewGuid().ToString();
                        childClusteringModel.Clustering = new List<ClusteringModel>();
                        if (j < 1)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                ClusteringModel childKClusteringModel = new ClusteringModel();
                                childKClusteringModel.Level = 3;
                                childKClusteringModel.Name = "第三极" + k;
                                childKClusteringModel.Col = colum;
                                string uid = Guid.NewGuid().ToString();
                                childKClusteringModel.Clustering = new List<ClusteringModel>();
                                if (k < 2)
                                {

                                    ClusteringModel childKClusteringModel1 = new ClusteringModel();
                                    childKClusteringModel1.Level = 4;
                                    childKClusteringModel1.Name = "第四极" + k;
                                    childKClusteringModel1.Col = colum;
                                    childKClusteringModel1.Clustering = new List<ClusteringModel>();
                                    childKClusteringModel.Clustering.Add(childKClusteringModel1);
                                    string fd = Guid.NewGuid().ToString();
                                    for (int m = 0; m < 3; m++)
                                    {
                                        ClusteringModel childKClusteringModel1s = new ClusteringModel();
                                        childKClusteringModel1s.Level = 5;
                                        childKClusteringModel1s.Name = "第五极" + k;
                                        childKClusteringModel1s.Col = colum;
                                        childKClusteringModel1s.Clustering.Add(childKClusteringModel1);
                                        testClusters.Add(new TestCluster() { Name = childKClusteringModel1s.Name, ParentUid = fd, Level = childKClusteringModel1s.Level });
                                    }
                                    testClusters.Add(new TestCluster() { Name = childKClusteringModel1.Name, ParentUid = uid, Uid = fd, Level = childKClusteringModel1.Level });

                                }
                                testClusters.Add(new TestCluster() { Name = childKClusteringModel.Name, Uid = uid, ParentUid = uid1, Level = childKClusteringModel.Level });
                                childClusteringModel.Clustering.Add(childKClusteringModel);

                            }
                        }
                        testClusters.Add(new TestCluster() { Name = childClusteringModel.Name, Uid = uid1, ParentUid = uid2, Level = childClusteringModel.Level });
                        clusteringModel.Clustering.Add(childClusteringModel);
                    }
                }
                else
                {
                    clusteringModel.Name = "第一级AAAA" + i;
                    clusteringModel.Col = colum;


                }
                testClusters.Add(new TestCluster() { Name = clusteringModel.Name, Uid = uid2, ParentUid = key, IsRoot = true, Level = clusteringModel.Level });
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

            root.ItemsSource = testClusters;//.OrderBy(x=>x.Level).ToList();
        }
    }
}
