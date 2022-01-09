
 --  populate states.
insert into location.states (id, name) values (1, 'Maharashtra')
on conflict (name) do nothing;


--  populate cities
insert into location.cities (id, name, stateid)
values
(1, 'Mumbai', 1),
(2, 'Pune',1)
on conflict (name, stateid)
do nothing;

-- populate cinemas
insert into cinema.cinemas(id, name, cityid)
values
(1, 'Cinepolis', 1),
(2, 'PVR', 1),
(3, 'Cinepolis-VIP', 1),
(4, 'PVR-VIP', 1)
on conflict(id)
do nothing;

-- populate auditoriums

insert into cinema.auditoriums (id,"name",cinemaid,layout,numofrows,numofcols) VALUES
	 (1,'imax screen - 1',1,'{{true,true,false,true,true},{true,true,false,true,true}}',2,5),
    (2, 'imax screen -2', 1, '{{true,false,false,true,true},{true,true,true,false,false}}', 2, 5),
    (3, 'screen - 1', 1, '{{true,false,false,true,true, false},{true,true,true,false,false, true}, {true,true,true,false,false, true}}', 3, 6),
    (4, 'screen - 1', 2, '{{true,false,false,true,true, false},{true,true,true,false,false, true}, {true,true,true,false,false, true}}', 3, 6),
    (5, 'screen - 2', 2, '{{true,false,false,true,true, false},{true,true,true,false,false, true}, {true,true,true,false,false, true}}', 3, 6),
    (6, 'screen - 1', 3, '{{true,false,false,true,true, false, true},{true,true,true,false,false, true, true}, {true,true,true,false,false, true, true}}', 3, 7),
    (7, 'screen - 1', 4, '{{true,false,false,true,true, false, true},{true,true,true,false,false, true, true}, {true,true,true,false,false, true, true}}', 3, 7),
    (8, 'screen - 2', 4, '{{true,false,false,true,true, false, true},{true,true,true,false,false, true, true}, {true,true,true,false,false, true, true}}', 3, 7)
    on conflict(id) do nothing;

-- populate movies
INSERT INTO movie.movies (id,"name",directorname,releasedate) VALUES
	 (1478732685412274176,'Dr. Strange, Multiverse of Madness','Sam raimy','2022-05-06 00:00:00'),
	 (1479794584988028928,'Thor: Love and Thunder','Sam raimy','2022-08-07 00:00:00'),
	 (1479794888273956864,'Black Panther: Wakanda Forever','Sam raimy','2022-11-11 00:00:00'),
	 (1479801659411533824,'The Marvels','Sam raimy','2023-02-23 00:00:00')
    on conflict(id) do nothing;


