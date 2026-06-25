# Business Logic Service (BLS) Definitions

## Overview

Definitions describe how business-domain data is transformed into calculations and alerts (rule results). Definitions are stored in `*.yaml` files.

Definitions can be organized into folders as needed. Each definition must be stored in its own YAML file. The file name must match the definition name exactly (for example, `TotalPrice.yaml` must contain a definition named `TotalPrice`). File and definition names are case-sensitive.

There is no limit to the number of definitions within an environment. All file names and folder names must be unique within that environment.

## Definition Types

BLS supports four types of definitions:

### Calculation

A calculation is the default definition type. It is used to perform business logic transformations and can return values of any supported type.

```yaml
# TotalPrice.yaml
# Returns the total price based on item price and quantity.

name: TotalPrice
type: calculation

logic:
  - When: Amount > 0 AND ItemPrice > 0
    Return: Amount * ItemPrice
```

### Alert

An alert is a specialized form of calculation that evaluates business rules and typically returns a violation or warning message.

```yaml
# NoPriceAlert.yaml
# Returns an alert when ItemPrice or Amount is missing or invalid.

name: NoPriceAlert
type: alert

context:
  - Order

logic:
  - When: not ItemPrice OR ItemPrice < 0
    Return: $"{OrderId} wrong or missing ItemPrice ({ItemPrice})"
    ReturnAlso:
      OrderDate: OrderDate

  - When: not Amount OR Amount < 0
    Return: $"{OrderId} wrong or missing Amount ({Amount})"
    ReturnAlso:
      OrderDate: OrderDate
```

### Code

A code definition allows custom scripting using C# for advanced scenarios that cannot be expressed using standard calculation logic.

Once defined, a code definition can be used as a function from other definitions (for example, `GuidToInt(ActivityId)`).

```yaml
# Converts a Guid to an Integer.
# parameters[0] contains the first function argument.

type: code
name: GuidToInt

code: >-
  if (parameters.Length == 1)
  {
      if (parameters[0].GetAsString(out var g))
      {
          byte[] guidBytes = Guid.Parse(g).ToByteArray();
          return BitConverter.ToInt32(guidBytes, 0);
      }
  }

  return "?";
```

### Table

A table definition stores semi-static reference data in a tabular format.

```yaml
# Aircraft fuel consumption reference data.

type: table
name: FuelConsumptionTable

columns:
  - name: ACTYPE
    type: string
  - name: ACTAIL
    type: string
  - name: KGperBLH
    type: number

rows:
  - ACTYPE: A321
    KGperBLH: 2787

  - ACTYPE: A321N
    KGperBLH: 2296

  - ACTYPE: A339
    KGperBLH: 4612

  - KGperBLH: 2787
```


