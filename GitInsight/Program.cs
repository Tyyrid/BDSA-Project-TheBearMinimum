


//dotnet run --repo 'path'
//hvis path indeholder mellemrum, skal path skrives: "path"
// --mode author for author mode
// --mode frequency for frequency mode (default)

FluentArgsBuilder.New()
    .DefaultConfigs()
    .Parameter<DirectoryInfo>("-r", "--repo")
        .WithValidation(repo => repo.Exists, "Directory does not exist")
        .IsRequired()
    .Parameter<Mode>("-m", "--mode")
        .WithDescription("Which display mode to use.")
        .IsOptional()
    .Call(mode => repoInfo =>
    {

        DBService dBService = new DBService(repoInfo.ToString());


        
        /* 
            case Mode.Frequency:
                

                break;
        } */

        return Task.CompletedTask;
    })
    .Parse(args);

enum Mode { Frequency, Author }
