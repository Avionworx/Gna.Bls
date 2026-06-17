# Evaluation Endpoint

## Overview

The evaluation endpoint evaluates data against one or more definition from a specified ruleset and returns the resulting business logic evaluations (alerts and/or evaluations)

## Endpoint

```http
POST /{env}/{ruleset}/evaluate
```

## Path Parameters

| Parameter | Required | Description |
|------------|----------|-------------|
| `env` | Yes | Case-insensitive environment name. |
| `ruleset` | Yes | Name of the ruleset (blueprint) to evaluate. Defaults to `master`. |

---

## Evaluation Parameters

Evaluation parameters can be supplied in **either**:

- Query string parameters
- Request payload

This allows callers to keep evaluation configuration separate from the data payload or package everything into a single request body.

### Supported Parameters

| Parameter | Type | Description |
|------------|------|-------------|
| `commit` | string | Optional commit identifier. |
| `EvaluationPeriodStart` | datetime | Start of the evaluation period. Only required when it differs from the input data range. |
| `EvaluationPeriodEnd` | datetime | End of the evaluation period. Only required when it differs from the input data range. |
| `DataPeriodStart` | datetime | Optional lead-in start date for data considered during evaluation. |
| `DataPeriodEnd` | datetime | Optional lead-in end date for data considered during evaluation. |
| `label` / `Labels` | string[] | Labels (rule names) to evaluate. Example: `FDP`, `FDPlimit`. |
| `tag` / `Tags` | string[] | Tags used to select definitions for evaluation. Example: `GROUP_Roster`. |
| `role` / `Roles` | string[] | Roles to be used during evaluation. |
| `context` / `Contexts` | string[] | Contexts to be used during evaluation. |
| `Mode` | enum | Evaluation mode. Supported values: `default`, `alerts`, `calculations`. |

---

## Request Body

The endpoint accepts JSON input containing:

| Property | Type | Description |
|-----------|------|-------------|
| `Data` | array | Input records grouped into evaluation datasets. |
| `Keys` | array | Keys corresponding to the supplied data records. |
| `Labels` | string[] | Optional labels to evaluate. |
| `Roles` | string[] | Optional roles to use during evaluation. |
| `Tags` | string[] | Optional tags to use during evaluation. |
| `Contexts` | string[] | Optional contexts to use during evaluation. |
| `EvaluationPeriodStart` | datetime | Optional evaluation period start. |
| `EvaluationPeriodEnd` | datetime | Optional evaluation period end. |
| `DataPeriodStart` | datetime | Optional data period start. |
| `DataPeriodEnd` | datetime | Optional data period end. |

### Data Structure

`Data` contains one or more datasets. Each dataset consists of a collection of business objects that will be evaluated.

`Keys` contains identifiers matching the records in `Data`. These keys are returned in the evaluation results, allowing results to be correlated back to the original input records.

---

## Example: Parameters in Query String

```http
POST /AWX/master/evaluate?label=FDPlimit&label=FDP
Content-Type: application/json
```

```json
{
  "Data": [
    [
      {
        "Rank": "CAP",
        "ActingRank": "CAP",
        "EmploymentDate": "2020-01-01T00:00:00",
        "BirthDate": "1980-01-01T00:00:00",
        "IsActive": true,
        "IsDuty": true,
        "FlightDeckCount": 2,
        "Code": "Act0",
        "EmploymentPercentage": 100,
        "HomeBaseCode": "MMX",
        "AirlineCode": "AWX",
        "FlightNumber": 4100,
        "STD": "2026-06-09T06:00:00Z",
        "DepAirportCode": "MMX",
        "ServiceTypeCode": "J",
        "STA": "2026-06-09T06:55:00Z",
        "ArrAirportCode": "GOT",
        "Registration": "SE-DUU",
        "AcTypeCode": "221",
        "AcVersion": "221",
        "AcConfiguration": "Y135"
      },
      {
        "Rank": "CAP",
        "ActingRank": "CAP",
        "EmploymentDate": "2020-01-01T00:00:00",
        "BirthDate": "1980-01-01T00:00:00",
        "IsActive": true,
        "IsDuty": true,
        "FlightDeckCount": 2,
        "Code": "Act1",
        "EmploymentPercentage": 100,
        "HomeBaseCode": "MMX",
        "AirlineCode": "AWX",
        "FlightNumber": 4100,
        "Sequence": 1,
        "STD": "2026-06-09T07:10:00Z",
        "DepAirportCode": "GOT",
        "ServiceTypeCode": "J",
        "STA": "2026-06-09T09:15:00Z",
        "ArrAirportCode": "LLA",
        "Registration": "SE-DUU",
        "AcTypeCode": "221",
        "AcVersion": "221",
        "AcConfiguration": "Y135"
      }
    ]
  ],
  "Keys": [
    [
      "bb1aeec1-9a45-4670-9f86-77b25e8540e4",
      "af1d38a4-375d-48c3-ad26-431bb6a6aaed"
    ]
  ],
  "EvaluationPeriodStart": "2026-06-08T00:00:00Z",
  "EvaluationPeriodEnd": "2026-06-12T23:59:59.999Z"
}
```

---

## Example: Parameters in Request Body

Instead of using query parameters, labels and other evaluation options can be supplied directly in the payload.

```http
POST /AWX/master/evaluate
Content-Type: application/json
```

```json
{
  "Labels": [
    "FDPlimit",
    "FDP"
  ],
  "Roles": [],
  "Tags": [],
  "Contexts": [],
  "EvaluationPeriodStart": "2026-06-08T00:00:00Z",
  "EvaluationPeriodEnd": "2026-06-12T23:59:59.999Z",
  "Data": [...],
  "Keys": [...]
}
```

---

## Response

A successful request returns HTTP `200 OK` and an array of evaluation results.

```json
[
  {
    "key": "bb1aeec1-9a45-4670-9f86-77b25e8540e4",
    "index": "2026-06-09T06:00:00Z",
    "label": "FDP",
    "value": [
      "duration",
      "00:55:00"
    ],
    "cachedValue": [
    ]
  },
  {
    "key": "af1d38a4-375d-48c3-ad26-431bb6a6aaed",
    "index": "2026-06-09T07:10:00Z",
    "label": "FDP",
    "value": [
      "duration",
      "02:20:00"
    ],
    "cachedValue": [
      "duration",
      "02:20:00"
    ]
  },
  {
    "key": "bb1aeec1-9a45-4670-9f86-77b25e8540e4",
    "index": "2026-06-09T06:00:00Z",
    "label": "FDPlimit",
    "value": [
      "duration",
      "13:00:00"
    ]
  }
]
```

Each item in the response represents the result produced by a business rule evaluation and may contain:

- The evaluated rule or label
- Outcome or status
- Related keys from the input data
- Calculated values
- Alerts or violations
- Additional rule-specific metadata

---

## Common Usage Patterns

### Evaluate using specific definitions

```http
POST /AWX/master/evaluate?label=FDP&label=FDPlimit
```

Only the requested definitions are evaluated.

### Evaluate Using Tags

```http
POST /AWX/master/evaluate?tag=GROUP_Roster
```

Only definitions matching the supplied tag are considered.

### Run Alert Evaluation

```http
POST /AWX/master/evaluate?Mode=alerts&tag=GROUP_Roster&tag=GROUP_Document
```

Evaluate alerts only for given tag(s)

### Evaluate using different ruleset

```http
POST /AWX/UNION2027/evaluate?label=Duty
```

Executes the Duty evaluation in UNION2027 ruleset

---

## Notes

- Evaluation parameters such as labels, roles, tags, contexts, and evaluation periods can be provided either in the query string or in the request body.
- Multiple values can be supplied for query parameters by repeating the parameter:

```http
POST /AWX/master/evaluate?label=FDP&label=FDPlimit&tag=GROUP_Roster
```

- The request body supports both evaluation configuration and evaluation data, allowing all inputs to be sent in a single request when preferred.
- The endpoint accepts `application/json`, `text/plain`, and `application/octet-stream` payloads.