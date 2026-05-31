# Currency Exchange Console Application

A simple .NET console application for converting between currencies using ISO currency codes.

## Features

* Convert between currencies using the format:

```text
<SOURCE_CURRENCY>/<TARGET_CURRENCY> <AMOUNT>
```

Example:

```text
EUR/USD 100
```
---

## Usage

Run the application and enter a conversion request:

```text
EUR/USD 100
```

Output:

```text
112.19
```

### Input Format

```text
<SOURCE_CURRENCY>/<TARGET_CURRENCY> <AMOUNT>
```

Where:

| Parameter       | Description              |
| --------------- | ------------------------ |
| SOURCE_CURRENCY | Source ISO currency code |
| TARGET_CURRENCY | Target ISO currency code |
| AMOUNT          | Amount to convert        |

Examples:

```text
EUR/USD 100
GBP/CHF 50
JPY/EUR 10000
```

---

## Repository choice

Application includes two implementations for repository:

### ExchangeRepository

Uses the Frankfurter API to retrieve current exchange rates.

### DummyExchangeRepository

Provides predefined in-memory exchange rates.

Responsibilities:

* No external dependencies.
* Deterministic behavior.
* Useful for testing and development.

Example rates:

```csharp
EUR -> 743.94
USD -> 663.11
GBP -> 852.85
...
```
Repository selection is configured through the ServiceCollectionBuilder extension method.

### Use Frankfurter API

```csharp
services.AddHttpClient<IExchangeRepository, ExchangeRepository>(client =>
{
    client.BaseAddress = new Uri("https://api.frankfurter.dev/v2/");
});
```

or comment it out and uncomment to  
### Use dummy repository

```csharp
services.AddScoped<IExchangeRepository, DummyExchangeRepository>();
```


## Requirements

* .NET 10


Enter a conversion request when prompted:

```text
EUR/USD 100
```
