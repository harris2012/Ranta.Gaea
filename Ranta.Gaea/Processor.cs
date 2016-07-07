using Ranta.Gaea.Model;
using Ranta.Gaea.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranta.Gaea
{
    internal static class Processor
    {
        public static void Process(string outputFolder, Solution solution)
        {
            //if (Directory.Exists(outputFolder))
            //{
            //    Directory.Delete(outputFolder, true);
            //}
            //Directory.CreateDirectory(outputFolder);

            #region Solution
            var solutionPath = Path.Combine(outputFolder, string.Format("{0}.sln", solution.Name));
            var solutionTemplate = new SolutionTemplate { Solution = solution };
            var solutionContent = solutionTemplate.TransformText();
            File.WriteAllText(solutionPath, solutionContent, Encoding.UTF8);
            #endregion

            switch (solution.ProjectType)
            {
                case 1://Library
                    {
                        #region Nuspec
                        var nuspecFilePath = Path.Combine(outputFolder, string.Format("{0}.nuspec", solution.FullName));
                        var nuspecTemplate = new NuspecTemplate { Solution = solution };
                        var nuspecContent = nuspecTemplate.TransformText();
                        File.WriteAllText(nuspecFilePath, nuspecContent, Encoding.UTF8);
                        #endregion

                        #region Bat
                        var batFilePath = Path.Combine(outputFolder, string.Format("{0}.bat", solution.FullName));
                        var batTemplate = new BatTemplate { Solution = solution, ProjectList = solution.ProjectList };
                        var batContent = batTemplate.TransformText();
                        File.WriteAllText(batFilePath, batContent, Encoding.ASCII);
                        #endregion

                        #region AppConfigTransform
                        var appConfigFilePath = Path.Combine(outputFolder, "app.config.transform");
                        File.WriteAllBytes(appConfigFilePath, GaeaResource.app_config);
                        #endregion

                        #region WebConfigTransform
                        var webConfigFilePath = Path.Combine(outputFolder, "web.config.transform");
                        File.WriteAllBytes(webConfigFilePath, GaeaResource.web_config);
                        #endregion
                    }
                    break;
                default:
                    break;
            }

            if (solution.ProjectList != null && solution.ProjectList.Count > 0)
            {
                foreach (var project in solution.ProjectList)
                {
                    //create project folder
                    var projectFolderPath = Path.Combine(outputFolder, project.FullName);
                    Directory.CreateDirectory(projectFolderPath);

                    //create project file.
                    var projectFilePath = Path.Combine(projectFolderPath, string.Format("{0}.csproj", project.FullName));
                    var projectContent = string.Empty;
                    switch (project.ProjectType)
                    {
                        case ProjectType.Net40:
                            {
                                var projectTemplate = new CSharp40ProjectTemplate { Project = project };
                                projectContent = projectTemplate.TransformText();
                            }
                            break;
                        case ProjectType.Net45:
                            {
                                var projectTemplate = new CSharp45ProjectTemplate { Project = project };
                                projectContent = projectTemplate.TransformText();
                            }
                            break;
                        case ProjectType.Test:
                            {
                                var projectTemplate = new CSharpTestProjectTemplate { Project = project, ProjectToTest = solution.ProjectList.Where(v => v.NeedTest).ToArray() };
                                projectContent = projectTemplate.TransformText();
                            }
                            break;
                        case ProjectType.Mvc:
                            {
                                var projectTemplate = new CSharpMvcProjectTemplate { Project = project };
                                projectContent = projectTemplate.TransformText();
                            }
                            break;
                        default:
                            throw new NotSupportedException("FrameworkVersion 不受支持");
                    }
                    if (!string.IsNullOrEmpty(projectContent))
                    {
                        File.WriteAllText(projectFilePath, projectContent, Encoding.UTF8);
                    }

                    switch (project.ProjectType)
                    {
                        case ProjectType.Mvc:
                            {
                                var webConfigFilePath = Path.Combine(projectFolderPath, "Web.Config");
                                File.WriteAllText(webConfigFilePath, GaeaResource.mvc_web_config, Encoding.UTF8);

                                var webDebugConfigFilePath = Path.Combine(projectFolderPath, "Web.Debug.config");
                                File.WriteAllText(webDebugConfigFilePath, GaeaResource.mvc_web_debug_config, Encoding.UTF8);

                                var webReleaseConfigFilePath = Path.Combine(projectFolderPath, "Web.Release.config");
                                File.WriteAllText(webReleaseConfigFilePath, GaeaResource.mvc_web_release_config, Encoding.UTF8);

                                var packagesFilePath = Path.Combine(projectFolderPath, "packages.config");
                                File.WriteAllText(packagesFilePath, GaeaResource.mvc_packages, Encoding.UTF8);

                                var globalFilePath = Path.Combine(projectFolderPath, "Global.asax");
                                var globalTemplate = new CSharpMvcGlobalTemplate { Project = project };
                                var globalContent = globalTemplate.TransformText();
                                File.WriteAllText(globalFilePath, globalContent, Encoding.UTF8);

                                var globalCsFilePath = Path.Combine(projectFolderPath, "Global.asax.cs");
                                var globalCsTemplate = new CSharpMvcGlobalCsTemplate { Project = project };
                                var globalCsContent = globalCsTemplate.TransformText();
                                File.WriteAllText(globalCsFilePath, globalCsContent, Encoding.UTF8);

                                var appStartFolderPath = Path.Combine(projectFolderPath, "App_Start");
                                Directory.CreateDirectory(appStartFolderPath);

                                var filterConfigFilePath = Path.Combine(appStartFolderPath, "FilterConfig.cs");
                                var filterConfigTemplate = new CSharpMvcFilterConfigTemplate { Project = project };
                                var filterConfigContent = filterConfigTemplate.TransformText();
                                File.WriteAllText(filterConfigFilePath, filterConfigContent, Encoding.UTF8);

                                var routeConfigFilePath = Path.Combine(appStartFolderPath, "RouteConfig.cs");
                                var routeConfigTemplate = new CSharpMvcRouteConfigTemplate { Project = project };
                                var routeConfigContent = routeConfigTemplate.TransformText();
                                File.WriteAllText(routeConfigFilePath, routeConfigContent, Encoding.UTF8);

                                var controllersFolderPath = Path.Combine(projectFolderPath, "Controllers");
                                Directory.CreateDirectory(controllersFolderPath);

                                var homeControllerFilePath = Path.Combine(controllersFolderPath, "HomeController.cs");
                                var homeControllerTemplate = new CSharpMvcHomeControllerTemplate { Project = project };
                                var homeControllerContent = homeControllerTemplate.TransformText();
                                File.WriteAllText(homeControllerFilePath, homeControllerContent, Encoding.UTF8);

                                var viewsFolderPath = Path.Combine(projectFolderPath, "Views");
                                Directory.CreateDirectory(viewsFolderPath);

                                var viewWebConfigFilePath = Path.Combine(viewsFolderPath, "Web.Config");
                                File.WriteAllText(viewWebConfigFilePath, GaeaResource.mvc_view_web_config, Encoding.UTF8);

                                var viewStartFilePath = Path.Combine(viewsFolderPath, "_ViewStart.cshtml");
                                File.WriteAllBytes(viewStartFilePath, GaeaResource.mvc_views_viewstart);

                                var viewsSharedFolderPath = Path.Combine(viewsFolderPath, "Shared");
                                Directory.CreateDirectory(viewsSharedFolderPath);

                                var layoutFilePath = Path.Combine(viewsSharedFolderPath, "_Layout.cshtml");
                                File.WriteAllBytes(layoutFilePath, GaeaResource.mvc_views_shared_layout);

                                var viewsHomeFolderPath = Path.Combine(viewsFolderPath, "Home");
                                Directory.CreateDirectory(viewsHomeFolderPath);

                                var homeIndexFilePath = Path.Combine(viewsHomeFolderPath, "Index.cshtml");
                                File.WriteAllBytes(homeIndexFilePath, GaeaResource.mvc_views_home_index);
                            }
                            break;
                        default:
                            break;
                    }

                    var propertiesFolderPath = Path.Combine(projectFolderPath, "Properties");
                    Directory.CreateDirectory(propertiesFolderPath);

                    //create assembly info file.
                    var assemblyInfoFilePath = Path.Combine(projectFolderPath, "Properties", "AssemblyInfo.cs");
                    var assemblyInfoTemplate = new AssemblyInfoTemplate { Project = project };
                    var assemblyInfoContent = assemblyInfoTemplate.TransformText();
                    File.WriteAllText(assemblyInfoFilePath, assemblyInfoContent, Encoding.UTF8);

                }
            }
        }
    }
}
