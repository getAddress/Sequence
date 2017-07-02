Sequence
========

A multiple instance and thread safe unique number sequence generator. 

Replicates the create sequence transact-SQL functionality (http://msdn.microsoft.com/en-us/library/ff878091.aspx) in a .Net assembly.

## What's the point?

The 'Create Sequence' command is not supported on Sql Azure:
(https://connect.microsoft.com/SQLServer/feedback/details/723601/support-native-sequences-in-sql-azure)

You can now use any Sql or NoSql database to store your sequence. Just implement IStateStore.

## When using Azure table storage as the store state

### Install
```
Install from Nuget:

PM> Install-Package getAddress.Sequence.Azure 
```

### Usage

```
static void Main(string[] args)
{
    Run().Wait();

    Console.Read();
}

private async static Task Run()
{

    var stateProvider = AzureStateProviderFactory.Get("" /* Your azure storage connection string*/, "mySequenceTable" /*or any table name*/);

    var sequence = await stateProvider.NewAsync(new SequenceOptions
    {
        Cycle = false,
        Increment = 1,
        MaxValue = 100,
        MinValue = 0,
        StartAt = 0
    });

    var sequenceKey = await stateProvider.AddAsync(sequence);

    var sequenceGenerator = new SequenceGenerator(stateProvider);

    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(await sequenceGenerator.NextAsync(sequenceKey));
    }


}
```
### Output

```
1
2
3
4
5
6
7
8
9
10

```

## When using SQL Server as the store state 

### Install

```
Install from Nuget:

PM> Install-Package getAddress.Sequence.SqlServer 
```

### Usage

```
static void Main(string[] args)
{
    Run().Wait();

    Console.Read();
}

private async static Task Run()
{

    var stateProvider = SqlServerStateProviderFactory.Get("" /* Your connection string*/);

    var sequence = await stateProvider.NewAsync(new SequenceOptions
    {
        Cycle = false,
        Increment = 1,
        MaxValue = 100,
        MinValue = 0,
        StartAt = 0
    });

    var sequenceKey = await stateProvider.AddAsync(sequence);

    var sequenceGenerator = new SequenceGenerator(stateProvider);

    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(await sequenceGenerator.NextAsync(sequenceKey));
    }


}
```

### Output

```
1
2
3
4
5
6
7
8
9
10

```
## When using Mongo as the store state

### Install

```
Install from Nuget:

PM> Install-Package getAddress.Sequence.Mongo 
```

### Usage

```
static void Main(string[] args)
{
    Run().Wait();

    Console.Read();
}

private async static Task Run()
{

    var stateProvider = MongoStateProviderFactory.Get("" /* Your connection string*/);

    var sequence = await stateProvider.NewAsync(new SequenceOptions
    {
        Cycle = false,
        Increment = 1,
        MaxValue = 100,
        MinValue = 0,
        StartAt = 0
    });

    var sequenceKey = await stateProvider.AddAsync(sequence);

    var sequenceGenerator = new SequenceGenerator(stateProvider);

    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(await sequenceGenerator.NextAsync(sequenceKey));
    }


}
```
### Output

```
1
2
3
4
5
6
7
8
9
10
```
