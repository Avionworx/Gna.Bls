# Data Model

The data model defines how business domain data is structured and stored within the service.

The service includes a built-in default GnA data model.

 * [`GnA model(json)`](./gna.model.json) 
 * [`GnA model(yaml)`](./gna.model.yaml) 


Alternatively, you can define a custom data model in a YAML file named `.model.yaml`.

## YAML Model Example

```yaml
name: OrderModel

properties:
  Orders:
    type: array
    items:
      properties:
        Id:
          type: integer
          key: 0

        TimeStamp:
          type: datetime
          index: 0

        OrderLines:
          type: array
          items:
            properties:
              ProductId:
                type: integer

              Quantity:
                type: number

              ProductPrice:
                type: number

  FreeFreightLimit:
    type: number
```

## JSON Schema Support

The service supports a JSON Schema–like format for model definitions.

You can convert models between YAML and JSON formats using the following endpoint:

```http
GET/POST ./{env}/{blueprint}/Model
```

### JSON Model Example

```json
{
  "type": ["object", "null"],
  "properties": {
    "Orders": {
      "type": ["array", "null"],
      "items": {
        "type": ["object", "null"],
        "properties": {
          "Id": {
            "type": ["string", "integer", "null"],
            "$key": 0
          },
          "TimeStamp": {
            "type": ["string", "null"],
            "format": "date-time",
            "$index": 0
          },
          "OrderLines": {
            "type": ["array", "null"],
            "items": {
              "$ref": "#/properties/Orders/items"
            }
          },
          "ProductId": {
            "type": ["string", "integer", "null"]
          },
          "Quantity": {
            "type": ["string", "number", "null"]
          },
          "ProductPrice": {
            "type": ["string", "number", "null"]
          }
        },
        "additionalProperties": false
      }
    },
    "FreeFreightLimit": {
      "type": ["string", "number", "null"]
    }
  },
  "additionalProperties": false
}
```

> **Note**
>
> The service follows JSON Schema conventions where possible, but it also introduces additional requirements and limitations described below.

## Model Constraints

### Single Model per Environment

Each environment can contain only one data model.

### Reserved Keywords

Certain keywords are reserved and cannot be used as property names.
Since the service generates and processes .NET types internally, C# language keywords are reserved and cannot be used as model property names.
For a complete and up-to-date list, see the [reserved keywords](./reservedkeywords.md)  documentation.

Examples include:

```text
abstract
base
bool
class
decimal
enum
event
float
int
interface
namespace
object
private
public
string
struct
void
while
...
```

### Property Name Consistency

Properties with the same name must always use the same type throughout the model.

For example:

```yaml
Customer:
  properties:
    Id:
      type: integer

Supplier:
  properties:
    Id:
      type: integer   # Valid
```

```yaml
Customer:
  properties:
    Id:
      type: integer

Supplier:
  properties:
    Id:
      type: string    # Invalid
```

## Supported Data Types

The service supports the following property types:

| Model Type | JSON Schema Representation                        |
| ---------- | ------------------------------------------------- |
| string     | string                                            |
| array      | array                                             |
| boolean    | boolean                                           |
| integer    | integer                                           |
| number     | number                                            |
| datetime   | string with `format: date` or `format: date-time` |
| duration   | string with `format: time`                        |
| guid       | string with `format: uuid`                        |

## Nullability

All properties are implicitly nullable.

There is no need to explicitly mark properties as nullable in the YAML model definition.

## Keys and Indexes

A valid model must define:

* At least one key property (`key` or `$key`)
* At least one index property (`index` or `$index`)

Example:

```yaml
Id:
  type: integer
  key: 0

TimeStamp:
  type: datetime
  index: 0
```

## Array Restrictions

Arrays can only be defined at levels 1 and 2 of the model hierarchy.

### Level 1 Arrays

First-level arrays are interpreted as data context collections.

Example:

```yaml
properties:
  Orders:
    type: array
```

### Level 2 Arrays

Arrays nested within a first-level collection are supported.

Example:

```yaml
Orders:
  type: array
  items:
    properties:
      OrderLines:
        type: array
```

### Unsupported

Arrays nested deeper than level 2 are not supported.

```yaml
Orders:
  type: array
  items:
    properties:
      OrderLines:
        type: array
        items:
          properties:
            SerialNumbers:
              type: array   # Not supported
```
 
