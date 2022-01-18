export const fetchShowsForCity = (cityId) => {
  return fetch(`http://localhost:6000/api/shows/cities/${cityId}`);
};
