using Services;

InterfaceProvider serviceProvider = new InterfaceProvider();
serviceProvider.AddStaticService<IProjectInfo, ProjectInfo>();

IProjectInfo projectInfo = serviceProvider.GetService<IProjectInfo>();

Console.WriteLine(projectInfo);