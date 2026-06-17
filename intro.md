# Business Logic Service (BLS)

The Business Logic Service (BLS) allows you to define, manage, and execute business rules, calculations, and alerts through a simple REST API.

In practice, you send data to the service in a format defined by your model, and BLS processes that data according to the configured business rules. The result is a structured response that can be used by other applications or business processes.

## Core Concepts

### Environment

An environment represents a complete set of business configuration, including:

* The data model that defines the structure of input and output data
* The business rule definitions used during processing

You can have multiple environments, allowing you to separate different business domains, projects, or stages of development.

Environment names are not case-sensitive.

### Ruleset

A ruleset is a version or branch of an environment's business rules and configuration.

Ruleset allow different versions of business logic to coexist, making it easier to test changes or maintain multiple rule sets.

## API Structure

Most API endpoints follow the pattern:

```text
{environment}/{ruleset}/{action}/...
```

Where:

* **environment** identifies the business configuration to use
* **ruleset** identifies the version of the business rules
* **action** specifies the operation to perform

Refer to the API documentation (OpenAPI/Swagger) for the complete list of available endpoints and parameters.

## Running Evaluations

To execute business logic, use the evaluation endpoint:

```http
POST {environment}/{ruleset}/evaluate
```

Provide the input data in the request body. The service validates the data, executes the relevant business rules, and returns the calculated results.

## Updating Models and Definitions

When models or business rule definitions are modified, BLS automatically validates the changes before applying them.

Invalid updates are rejected, helping ensure that existing models and definitions remain consistent and usable.

## Response Information

Responses may include additional metadata in the headers, such as:

* The source of the business definitions used for the request
* The version or revision of the definitions
* The processing time for the request

## Response Format

BLS returns responses in a standard format to ensure consistent integration across applications and services.
