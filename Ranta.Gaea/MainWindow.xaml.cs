using Ranta.Gaea.Model;
using Ranta.Gaea.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ranta.Gaea
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(PrefixTextBox.Text) &&
                !string.IsNullOrEmpty(ProjectTextBox.Text) &&
                !string.IsNullOrEmpty(BrowseFolderTextBox.Text);
        }

        private void GenerateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                PreviewTextBox.Text = "正在处理...";

                var path = BrowseFolderTextBox.Text;

                var solution = PrepareSolution();

                Processor.Process(path, solution);

                PreviewTextBox.Text = "处理成功";
            }
            catch (Exception ex)
            {
                PreviewTextBox.Text = ex.ToString();
            }
        }

        private Solution PrepareSolution()
        {
            var solution = new Solution();
            solution.Guid = Guid.NewGuid();
            solution.Prefix = PrefixTextBox.Text;
            solution.Name = ProjectTextBox.Text;
            solution.FullName = string.Concat(PrefixTextBox.Text, ".", ProjectTextBox.Text);
            solution.ProjectList = new List<CSharpProject>();

            if (LibraryRadioButton.IsChecked != null && LibraryRadioButton.IsChecked.Value)//Library
            {
                solution.ProjectType = 1;

                var project = new CSharpProject();
                project.Guid = Guid.NewGuid();
                project.ProjectType = ProjectType.Net45;
                project.NeedTest = true;
                project.Name = string.Format("{0}.{1}", PrefixTextBox.Text, ProjectTextBox.Text);
                project.FullName = string.Format("{0}.{1}", PrefixTextBox.Text, ProjectTextBox.Text);
                solution.ProjectList.Add(project);

                var projectTest = new CSharpProject();
                projectTest.Guid = Guid.NewGuid();
                projectTest.ProjectType = ProjectType.Test;
                projectTest.Name = string.Format("{0}.{1}.Test", PrefixTextBox.Text, ProjectTextBox.Text);
                projectTest.FullName = string.Format("{0}.{1}.Test", PrefixTextBox.Text, ProjectTextBox.Text);
                solution.ProjectList.Add(projectTest);

                var project40 = new CSharpProject();
                project40.Guid = Guid.NewGuid();
                project40.ProjectType = ProjectType.Net40;
                project40.Compile = true;
                project40.Name = string.Format("{0}.{1}", PrefixTextBox.Text, ProjectTextBox.Text);
                project40.FullName = string.Format("{0}.{1}.Net40", PrefixTextBox.Text, ProjectTextBox.Text);
                solution.ProjectList.Add(project40);

                var project45 = new CSharpProject();
                project45.Guid = Guid.NewGuid();
                project45.ProjectType = ProjectType.Net45;
                project45.Compile = true;
                project45.Name = string.Format("{0}.{1}", PrefixTextBox.Text, ProjectTextBox.Text);
                project45.FullName = string.Format("{0}.{1}.Net45", PrefixTextBox.Text, ProjectTextBox.Text);
                solution.ProjectList.Add(project45);
            }
            else//Mvc
            {
                solution.ProjectType = 2;

                //mvc 使用.net45，暂时不支持.net40

                var projectMvc = new CSharpProject();
                projectMvc.Guid = Guid.NewGuid();
                projectMvc.ProjectType = ProjectType.Mvc;
                projectMvc.Name = string.Format("{0}.{1}", PrefixTextBox.Text, ProjectTextBox.Text);
                projectMvc.FullName = string.Format("{0}.{1}", PrefixTextBox.Text, ProjectTextBox.Text);
                solution.ProjectList.Add(projectMvc);
            }

            return solution;
        }
    }
}
