# BDSA-Project-TheBearMinimum

## How to run the project without API

To run the project (from the master folder) run the following command:

```
dotnet run --project GitInsight --repo /path/to/your/repository
```

Optionally the `--mode` tag can be added, to run in a specific mode (Frequency mode or Author mode):

```
dotnet run --project GitInsight --repo /path/to/your/repository --mode Author
```

or
```
dotnet run --project GitInsight --repo /path/to/your/repository --mode Frequency
```



## How to run the program with docker

From the master folder run the following command:

```
docker compose up
```

Optionally add `-d`, so it will run in the background and not in the terminal
```
docker compose up -d
```

\
How to stop docker from running:
- in the terminal
```
ctrl + c
```

- in the background
```
docker compose down
```