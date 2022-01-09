# Book My Show Clone

## Pre-requisites

- Required
    - git
    - dotnet 6
    - docker, docker-compose
    - Postgres db client, preferrably `dbeaver`(Free CE, available on `windows/mac/linux` , GUI, and open source).

## Get Source Locally

1. Open terminal
2. run `git clone --recurse-submodules git@github.com:kumarmo2/bms-clone-mini.git`.

## Build Locally

1. `cd` into the cloned repo.
2. run `dotnet --version`. Version must be >= `6.0.101`.
3. run `dotnet build`. This should build without any errors.

## One time db Setup and data seeding to get started
 Before we can start using the apis, we must have some basic db, tables setup.  
 Also I have added some basic seed data so that some apis will give results from get go.

1. `cd` into the repo folder locally.
2.  run `cd infra`.
3. run `sudo docker-compose up`. This will start a postgres server listening on port `5433`.
4. Open your postgres client of choice. eg: `dbeaver`. But any is fine. 
5. Connect to server. `Host: localhost, port: 5433, username: postgres, password: admin, database: postgres`.
6. copy the contents of file `database/one-time.sql` and run it using the db client.
7. Either reconnect or open a new connection with `database: bms`. And rest of the ..
   properties   of connection string as step 5.
8. Copy the contents of file `database/setup.sql` and run the statements in db client. This sets up the tables  
   and schemas for the system.
9. Copy the contents of file `database/seed.sql` and run the statements in db client. This creates some basic  
   initial data for the system like `states`, `cities`, `movies`, `cinemas` and `auditoriums`.


## Run Locally
1. Make sure the `db setup and initial data` step has been completed.
2. Make sure you have run `sudo docker-compose up` in directory `infra`.
3. `cd` into `src/Service/`.
4. run `dotnet run`. This starts the server on port `6000`.


## Some Details
1. An Auditorium is represented as 2-d matrix. The `Auditorium`'s seat layout is represented  
   as bool matrix. `true` represents there is a seat  and `false` represents there is no seat.  
   eg:

   ```
   [[true, false, false, true, true ], [ true, true, true, false, false ]]
   ```

2. For visualization and the app logic, `(0,0)` starts at the top left cornor of the audi.
   For a audi of m rows and n columns, `(m - 1, n - 1)` represents the bottom right seat.

3. The Seat Availability of a show is represented as 2-D Matrix of values including -1, 0, 1.  
   0 represents there is no seat. 1 represents the seat is not booked yet. -1 means the seat  
   has been booked already.

   ```
   [ [ -1, 0, 0, 1, 1 ], [ 1, 1, 1, 0, 0 ] ]
   ```


## Apis Examples.
  At the root of the cloned repo, there is a File `Insomnia_<timestamp>.json` which you can import in  
  `Insomnia` Rest Client(Postman Alternative).

1. User Signup api.

```
    curl --request POST \
  --url http://localhost:6000/api/users/signup \
  --header 'Content-Type: application/json' \
  --data '{
	"email": "kr2@ssdf.com",
	"firstName": "mohit",
	"lastName": "kumarmo2",
	"password": "kumarmo2"
}'
  ```
2. User Login Api

```
    curl --request POST \
  --url http://localhost:6000/api/users/login \
  --header 'Content-Type: application/json' \
  --data '{
	"email": "kr2@ssdf.com",
	"password": "kumarmo2"
}'

```

3. Get Shows in a city

```
curl --request GET \
  --url http://localhost:6000/api/shows/cities/{cityId}
```

4. Get Shows For a movie

```
curl --request GET \
  --url http://localhost:6000/api/shows/movies/{movieId}
```

5. Book a show

  This should only be called after user has logged in.

```
curl --request POST \
  --url http://localhost:6000/api/shows/booking \
  --header 'Content-Type: application/json' \
  --cookie userAuth=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyaWQiOjE0NzkwNTExMjU1NjE4ODg3NjgsImV4cCI6MTY0NDM1MzUwNS4wfQ.JjJu-c27a-zBUkgXcawmLVwtfZdS8qbG-khlTTV9QsI \
  --data '{
	"showId": 1478815783118311424,
	"seats": [{ "rowIndex": 1, "colIndex": 0}]
}'
```

6. Get Show Info

```
curl --request GET \
  --url http://localhost:6000/api/shows/{showid}
```

7. Create a show

```
curl --request POST \
  --url http://localhost:6000/api/shows \
  --header 'Content-Type: application/json' \
  --data '{
	"movieid": 1478732685412274176,
	"audiid": 2,
	"startTime": "2022-07-07 01:00:00.000",
	"endTime": "2022-07-07 03:00:00.000"
}'
```

8. Create a Movie.

```
curl --request POST \
  --url http://localhost:6000/api/movies \
  --header 'Content-Type: application/json' \
  --data '{
	"name": "The Marvels",
	"releaseDate": "02/23/2023",
	"directorName": "Sam raimy"
}'
```

