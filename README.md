Sequence
========

A unique number sequence generator. 

Replicates the create sequence transact-SQL functionality (http://msdn.microsoft.com/en-us/library/ff878091.aspx) in a .Net assembly.

<h2>When using Azure table storage to store state</h2>

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

