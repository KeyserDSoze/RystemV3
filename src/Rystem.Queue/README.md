## Queue
You have to configure it in DI

	services.AddQueue<Sample>(x =>
        {
            x.MaximumBuffer = 1000;
            x.Actions.Add(items =>
            {
                foreach(var item in items)
                {
                    //do something
                }
                return Task.CompletedTask;
            });
            x.MaximumRetentionCronFormat = "*/3 * * * * *";
        });

For instance, in the example above you have a maximum queue lenght of 1000
after the build you have to warm up

    var app = builder.Build();
	await app.Services.WarmUpAsync();

and inject to use it
    
    var queue = _serviceProvider.GetService<IQueue<Sample>>()!;
    for (int i = 0; i < 100; i++)
        await queue.AddAsync(new Sample() { Id = i.ToString() });

In this example, after 1000 elements or 3 seconds the configured actions will be fired.