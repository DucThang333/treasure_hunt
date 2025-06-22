# Treasure Hunt API Documentation

## Base URL
```
http://localhost:5000/api/TreasuareHunt
```

## Endpoints

### 1. Solve Treasure Hunt
**POST** `/api/TreasuareHunt`

Solves a treasure hunt puzzle and saves the result to history.

**Request Body:**
```json
{
  "row": 3,
  "col": 3,
  "treasure": 3,
  "treasure_map": [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
  ]
}
```

**Response:**
```json
{
  "message": "Treasure hunt request validated successfully",
  "solution": {
    "nodes": [
      {
        "row": 0,
        "col": 0,
        "step": 0,
        "key": 1
      },
      {
        "row": 0,
        "col": 1,
        "step": 1,
        "key": 2
      },
      {
        "row": 0,
        "col": 2,
        "step": 2,
        "key": 3
      }
    ],open
    "totalEnergyConsumed": 2.0
  },
  "databaseSave": {
    "success": true,
    "message": "Result saved to database",
    "error": null
  }
}
```

**Response (when database save fails):**
```json
{
  "message": "Treasure hunt request validated successfully",
  "solution": {
    "nodes": [
      {
        "row": 0,
        "col": 0,open
      },
      {
        "row": 0,
        "col": 1,
        "step": 1,
        "key": 2
      },
      {
        "row": 0,
        "col": 2,
        "step": 2,
        "key": 3
      }
    ],
    "totalEnergyConsumed": 2.0
  },
  "databaseSave": {
    "success": false,
    "message": "Failed to save result to database",
    "error": "Connection string is invalid"
  }
}
```

### 2. Get All History
**GET** `/api/TreasuareHunt/history`

Retrieves all treasure hunt history records, ordered by creation date (newest first).

**Response:**
```json
{
  "message": "History retrieved successfully",
  "data": [
    {
      "id": 1,
      "row": 3,
      "col": 3,
      "treasure": 3,
      "treasure_Map": [[1,2,3],[4,5,6],[7,8,9]],
      "minimum_Path": 2.0,
      "track_Path": [
        {
          "row": 0,
          "col": 0,
          "step": 0,
          "key": 1
        }
      ],
      "createdAt": "2024-01-01T12:00:00Z"
    }
  ]
}
```

### 3. Get History by ID
**GET** `/api/TreasuareHunt/history/{id}`

Retrieves a specific treasure hunt history record by ID.

**Parameters:**
- `id` (int): The ID of the history record

**Response:**
```json
{
  "message": "History record retrieved successfully",
  "data": {
    "id": 1,
    "row": 3,
    "col": 3,
    "treasure": 3,
    "treasure_Map": [[1,2,3],[4,5,6],[7,8,9]],
    "minimum_Path": 2.0,
    "track_Path": [
      {
        "row": 0,
        "col": 0,
        "step": 0,
        "key": 1
      }
    ],
    "createdAt": "2024-01-01T12:00:00Z"
  }
}
```

**Error Response (404):**
```json
{
  "error": "History record not found"
}
```

## Error Responses

All endpoints may return the following error responses:

### 400 Bad Request
```json
{
  "error": "Treasure_Map cannot be null or empty"
}
```

### 404 Not Found
```json
{
  "error": "History record not found"
}
```

### 500 Internal Server Error
```json
{
  "error": "Internal server error",
  "details": "Error details here"
}
```

## Data Models

### TreasureHuntRequest
```json
{
  "row": "int - Number of rows in the treasure map",
  "col": "int - Number of columns in the treasure map", 
  "treasure": "int - Number of treasures to find",
  "treasure_map": "int[][] - 2D array representing the treasure map"
}
```

### Solution
```json
{
  "nodes": "SolutionNode[] - List of nodes in the solution path",
  "totalEnergyConsumed": "double - Total energy consumed for the path"
}
```

### SolutionNode
```json
{
  "row": "int - Row position",
  "col": "int - Column position",
  "step": "int - Step number in the path",
  "key": "int - Key value at this position"
}
```

### HistoryTreasure
```json
{
  "id": "int - Unique identifier",
  "row": "int - Number of rows",
  "col": "int - Number of columns",
  "treasure": "int - Number of treasures",
  "treasure_Map": "int[][] - Treasure map",
  "minimum_Path": "double - Minimum energy path found",
  "track_Path": "SolutionNode[] - Solution path",
  "createdAt": "DateTime - Creation timestamp"
}
```

## Notes

- All treasure hunt solutions are automatically saved to the database
- History records are ordered by creation date (newest first)
- The API includes comprehensive error handling and validation
- All endpoints return consistent JSON response formats  