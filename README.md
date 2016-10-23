# Coniferous Log

A lightweight logging platform that supports arbitrary back ends, called sinks.

## Supported Sinks

The currently supported sinks are:

  * a console sink with coloured log levels
  * an in memory sink

More are to follow.

## Example

The most basic of set ups:

```csharp
ConsoleSink sink = new ConsoleSink("example", new CultureInfo("en-UK"));
ILogger log = new LoggerGenerator().SetMinumumLevel(LogLevel.Verbose)
                                   .SetBroadband(sink)
                                   .Generate();
log.Trace((Info i) => i.Push("Hello World!"));
```
This will print "HelloWorld!" to the console.

