# CashBox Frontend

## Start frontend

1. `cd "d:\C# Projects\CashBox.WebApi\frontend"`
2. `npm install`
3. `npm run dev`

Frontend will run on `http://localhost:5173`.

## Backend

Backend `CashBox.WebApi` is already configured to run on `http://localhost:5107`.
Use `dotnet run` from the `CashBox.WebApi` project or start from Visual Studio with the `CashBox.WebApi` profile.

## Features

- Login via `POST /api/auth/login`
- Users CRUD via `/api/user/*`
- Organizations CRUD via `/api/organization/*`

## Notes

- API token is saved in `localStorage`.
- Make sure backend is running before using the frontend.
