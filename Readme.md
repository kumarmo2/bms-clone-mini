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


