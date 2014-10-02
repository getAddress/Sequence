Sequence
========

A multiple instance and thread safe unique number sequence generator. 

Replicates the create sequence transact-SQL functionality (http://msdn.microsoft.com/en-us/library/ff878091.aspx) in a .Net assembly.


<h2>When using Azure table storage as the store state</h2>

<h3>Install</h3>
```
Install from Nuget:

PM> Install-Package getAddress.Sequence.Azure 
```

<h3>Usage</h3>

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
<h3>Output</h3>
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

<h2>When using SQL Server as the store state</h2>

<h3>Install</h3>
```
Install from Nuget:

PM> Install-Package getAddress.Sequence.SqlServer 
```

<h3>Usage</h3>

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
<h3>Output</h3>
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
