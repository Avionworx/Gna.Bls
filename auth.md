# BLS Authentication & Authorization Quick Start Guide

## Overview

The Business Logic Service (BLS) uses:

* **API Key** for initial authentication
* **JWT Token** for subsequent requests
* Optional **authorization rules** to control access to resources and operations

---

## Step 1: Obtain an Access Token

Before calling BLS endpoints, you must exchange your API key for a JWT access token.
Put your api key in the request header (X-API-KEY)

### Request

```http
POST {host}://{serviceUrl}/{env}/login?email={your-email}

X-API-KEY: {your-api-key}
```

### Example

```http
POST https://yourbls.com/prod/login?email=john.doe@company.com

X-API-KEY: abc123xyz
```

### Response

The service returns a JWT token:

```text
eyJhbGciOi...
```

---

## Step 2: Use the JWT Token

Include the received token in the `Authorization` header of every subsequent request.

### Request Header

```http
Authorization: Bearer {jwt-token}
```

### Example

```http
Authorization: Bearer eyJhbGciOi...
```

---

## Authentication Flow

```text
+-----------+
| API Key   |
+-----------+
      |
      v
POST /login?email=user@company.com
      |
      v
+-----------+
| JWT Token |
+-----------+
      |
      v
Authorization: Bearer <token>
      |
      v
BLS API Requests
```

---

## Trusted Email Domains

Depending on the BLS deployment configuration:

* Any email address may be accepted, or
* Only email addresses belonging to trusted domains may be allowed.

If authentication fails, verify that your email address is authorized for the target environment.

---

# Authorization

BLS supports fine-grained authorization through user-specific configuration files stored in the environment repository.

## User Configuration Files

Create a `.users` directory in the root of the environment repository.

For each user, create a file named after the user's email address.

### Example

```text
.users/
└── test@test.com
```

---

## Permission Format

Each line contains:

```text
<path-pattern> <permissions>
```

Where:

* `r` = Read
* `w` = Write
* `e` = Execute

### Examples

#### Full Access Everywhere

```text
** rwe
```

#### Read and Execute Access in `cost`

```text
cost/**/* re
```

#### Execute Everywhere, Full Access in `cost`

```text
** e
cost/**/* rw
```

---

## Common Permission Sets

| Permissions | Meaning           |
| ----------- | ----------------- |
| `r`         | Read only         |
| `w`         | Write only        |
| `e`         | Execute only      |
| `rw`        | Read and write    |
| `re`        | Read and execute  |
| `we`        | Write and execute |
| `rwe`       | Full access       |

---

## Wildcard Patterns

| Pattern     | Description                                                  |
| ----------- | ------------------------------------------------------------ |
| `**`        | All paths                                                    |
| `cost/**/*` | Everything under the `cost` directory and its subdirectories |

---

## Example User Configuration

```text
# Full access to all resources
** rwe
```

```text
# Read and execute access only within cost
cost/**/* re
```

```text
# Execute access globally, read/write in cost
** e
cost/**/* rw
```

---

## Summary

1. Obtain a JWT token using your API key and email.
2. Include the token in the `Authorization: Bearer` header.
3. Access permissions are controlled through files in the `.users` directory.
4. Permissions can be granted per path using wildcard patterns and `r`, `w`, `e` flags.
