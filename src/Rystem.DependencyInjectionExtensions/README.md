### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

## Dependency injection extensions

### Warm up
When you use the DI pattern in your .Net application you could need a warm up after the build of your services. And with Rystem you can simply do it.

	builder.Services.AddWarmUp(() => somethingToDo());

and after the build use the warm up

	var app = builder.Build();
	await app.Services.WarmUpAsync();