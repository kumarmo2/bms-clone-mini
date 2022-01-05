-- schema creation starts

create schema if not exists location;
create schema if not exists users;
create schema if not exists movie;
create schema if not exists cinema;
create schema if not exists booking;


-- schema creation ends


-- table creation starts ---

create table  if not exists location.states
(
	id int not null,
	name text not null,
	primary key(id),
	constraint ix_state_name unique(name)
);


create table if not exists location.cities
(
  id int not null,
  name text not null,
  stateid int not null,
  primary key(id),
  constraint fk_cities_states_cityid foreign key(stateid) references location.states(id),
  constraint ix_cities_name_cityid unique(name,stateid)
);


create table if not exists users.users
(
 id bigint not null,
 firstname text not null,
 lastname text not null,
 email text not null,
 hashedpass text not null,
 constraint ix_users_email unique(email),
 primary key(id)
);

create table if not exists movie.movies
(
 id bigint not null,
 name text not null,
 directorname text not null,
 releasedate timestamp not null,
 constraint ix_movies_name unique(name),
 primary key(id)
);

create table if not exists cinema.cinemas
(
 id int not null,
 name text not null,
 cityid int not null,
 constraint fk_cinemas_city_cityid foreign key(cityid) references location.cities(id),
 constraint ix_cinemas_name_cityid unique(name, cityid),
 primary key(id)
);


create table if not exists cinema.auditoriums
(
 id int not null,
 name text not null,
 cinemaid int not null,
 layout bool[][] not null,
 numofrows int not null,
 numofcols int not null,
 constraint ix_auditoriums_name_cinemaid unique(name, cinemaid),
 primary key(id)
);


create table if not exists booking.shows
(
  id bigint not null,
  movieid bigint not null,
  starttime timestamp not null,
  endtime timestamp not null,
  audiid int not null,
  constraint fk_shows_movie_movieid foreign key(movieid) references movie.movies(id),
  constraint fk_shows_auditoriums foreign key(audiid) references cinema.auditoriums(id),
  primary key(id)
);


create table if not exists booking.showseatavailabilities
(
 id bigint not null,
 showid bigint not null,
 rowindex int not null,
 colindex int not null,
 isbooked bool not null default(false),
 constraint fk_showseatavailabilities_show_showid foreign key(showid) references booking.shows(id),
 primary key(id)
);




--  TODO: add more tables for bookings

-- table creation ends ---



