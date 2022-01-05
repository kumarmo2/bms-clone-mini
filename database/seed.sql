
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
(2, 'PVR', 1)
on conflict(id)
do nothing;




